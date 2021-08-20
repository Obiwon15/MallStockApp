namespace inventoryAppDomain.Entities.Dtos
{
    public class FinalRequestPayload
    {
        public string cardno { get; set; }
        public string PBFPubKey { get; set; }
        public string cvv { get; set; }
        public string expirymonth { get; set; }
        public string expiryyear { get; set; }
        public string amount { get; set; }
        public string email { get; set; }
        public string txRef { get; set; }
        public string pin { get; set; }
        public string suggested_auth { get; set; }
    }
}