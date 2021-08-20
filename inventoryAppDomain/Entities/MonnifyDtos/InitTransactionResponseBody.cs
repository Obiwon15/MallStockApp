namespace inventoryAppDomain.Entities.MonnifyDtos
{
    public class InitTransactionResponseBody
    {
        public string transactionReference { get; set; }
        public string paymentReference { get; set; }
        public string merchantName { get; set; }
        public string apiKey { get; set; }
        public string[] enabledPaymentMethod { get; set; }
        public string checkoutUrl { get; set; }
        public object[] incomeSplitConfig { get; set; }
    }
}