namespace inventoryAppDomain.Entities.MonnifyDtos
{
    public class VerificationResponseBody
    {
        public string transactionReference { get; set; }
        public string paymentReference { get; set; }
        public string amountPaid { get; set; }
        public string totalPayable { get; set; }
        public string settlementAmount { get; set; }
        public string paidOn { get; set; }
        public string paymentStatus { get; set; }
        public string paymentDescription { get; set; }
        public string currency { get; set; }
        public string paymentMethod { get; set; }
        public object product { get; set; }
        public object cardDetails { get; set; }
        public object accountDetails { get; set; }
        public object[] accountPayments { get; set; }
        public object customer { get; set; }
        public object metaData { get; set; }
    }
}