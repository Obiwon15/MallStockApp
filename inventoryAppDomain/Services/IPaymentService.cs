using System;
using System.Threading.Tasks;
using inventoryAppDomain.Entities.Dtos;
using inventoryAppDomain.Entities.MonnifyDtos;

namespace inventoryAppDomain.Services
{
    public interface IPaymentService
    {
        Task<InitTransactionResponseBody> InitiatePayment(int orderId);

        Task<bool> VerifyPayment(string paymentReference);

        Task<string> GetLastPayment();
    }
}