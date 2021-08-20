namespace inventoryAppDomain.Entities.MonnifyDtos
{
    public class TransactionPayload
    {
        public decimal amount { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string paymentReference { get; set; }
        public string paymentDescription { get; set; }
        public string currencyCode { get; set; } = "NGN";
        public string contractCode { get; set; }
        public string redirectUrl { get; set; } = "https://localhost:44324/Payment/VerifyPayment";
        public string[] paymentMethods { get; set; } = {"CARD"};
    }
}