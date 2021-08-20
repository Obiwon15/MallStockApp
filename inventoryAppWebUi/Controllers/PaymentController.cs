using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using inventoryAppDomain.Services;

namespace inventoryAppWebUi.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        public async Task<ActionResult> ProcessPayment(int orderId)
        {
            try
            {
                var result = await _paymentService.InitiatePayment(orderId);
                return Redirect(result.checkoutUrl);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> VerifyPayment(string paymentReference)
        {
            try
            {
                var response = await _paymentService.VerifyPayment(paymentReference);
                if (response)
                {
                    ViewBag.PaymentResponse = true;
                    return RedirectToAction("Index", "Home", new{paymentCompleted="True"});
                }
                ViewBag.PaymentResponse = false;
                return RedirectToAction("Index", "Home", new{paymentCompleted="False"});

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}