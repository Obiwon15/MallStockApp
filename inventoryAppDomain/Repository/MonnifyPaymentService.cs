using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using AutoMapper;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Entities.MonnifyDtos;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace inventoryAppDomain.Repository
{
    public class MonnifyPaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;
        private readonly ITransactionService _transactionService;
        private readonly ApplicationDbContext _dbContext;
        private readonly string _contractCode = WebConfigurationManager.AppSettings["monnifyContractCode"];
        private readonly string _loginUrl = "https://sandbox.monnify.com/api/v1/auth/login";

        private string _initTransactionUrl =
            "https://sandbox.monnify.com/api/v1/merchant/transactions/init-transaction";

        private string _verifyTransactionUrl = "https://sandbox.monnify.com/api/v2/transactions/";
        private readonly string _secretKey = WebConfigurationManager.AppSettings["monnifySecretKey"];
        private readonly string _apiKey = WebConfigurationManager.AppSettings["monnifyApiKey"];

        public MonnifyPaymentService(IOrderService orderService, ITransactionService transactionService)
        {
            _orderService = orderService;
            _transactionService = transactionService;
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }

        private string GenerateTxRef()
        {
            const int txRefLength = 11;

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToLower();
            var random = new Random();

            var txRef = "MC-" + new string(Enumerable.Repeat(chars, txRefLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            var txRefInDb = _dbContext.Transactions.SingleOrDefault(t => t.GeneratedReferenceNumber == txRef);

            if (txRefInDb != null)
                GenerateTxRef();

            return txRef;
        }

        private async Task<ResponseDto> GetClientResponse(string url, HttpRequestMethod requestMethod,
            string authorizationHeader, object payload = null)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

            switch (requestMethod)
            {
                case HttpRequestMethod.POST:
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    if (payload == null)
                    {
                        var response = await httpClient.PostAsync(url, null);
                        return JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8,
                            "application/json");
                        var response = await httpClient.PostAsync(url, content);
                        return JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());
                    }
                }
                case HttpRequestMethod.GET:
                {
                    var response = await httpClient.GetAsync(url);
                    return JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());
                }
                default: throw new Exception("Something Went Wrong");
            }
        }

        private T GetResponseBody<T>(ResponseDto responseDto)
        {
            return JsonConvert.DeserializeObject<T>(responseDto.responseBody.ToString());
        }

        private async Task<ResponseDto> Login()
        {
            var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_apiKey}:{_secretKey}"));
            return await GetClientResponse(_loginUrl, HttpRequestMethod.GET, $"Basic {encodedString}");
        }

        public async Task<InitTransactionResponseBody> InitiatePayment(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            var payload = Mapper.Map<Order, TransactionPayload>(order);
            payload.contractCode = _contractCode;
            payload.paymentReference = GenerateTxRef();
            payload.paymentDescription = $"Order Payment for {payload.customerName}";
            var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_apiKey}:{_secretKey}"));
            var response = await GetClientResponse(_initTransactionUrl, HttpRequestMethod.POST,
                $"Basic {encodedString}", payload);
            var responseBody = GetResponseBody<InitTransactionResponseBody>(response);
            await _transactionService.CreateTransaction(responseBody.transactionReference, payload.paymentReference,
                orderId);
            return responseBody;
        }

        public async Task<bool> VerifyPayment(string paymentReference)
        {
            var accessToken = JsonConvert
                .DeserializeObject<AccessTokenResponseBody>((await Login()).responseBody.ToString())
                .accessToken;
            
            var transaction = await _transactionService.GetTransactionByGeneratedRef(paymentReference);
            var encodedUrl = HttpUtility.UrlEncode(transaction.ReferenceNumber);
            var fullPath = $"{_verifyTransactionUrl}{encodedUrl}";
            
            var response =
                await GetClientResponse(fullPath, HttpRequestMethod.GET, $"Bearer {accessToken}");
            var responseBody = GetResponseBody<VerificationResponseBody>(response);
            if (responseBody.paymentStatus.Equals("PAID")) return true;
            return false;
        }

        public async Task<string> GetLastPayment()
        {
            return (await _transactionService.GetLastTransaction()).GeneratedReferenceNumber;
        }
    }
}