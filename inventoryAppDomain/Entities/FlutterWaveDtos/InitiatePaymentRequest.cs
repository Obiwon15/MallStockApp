namespace inventoryAppDomain.Entities.Dtos
{
    public class InitiatePaymentRequest
    {
        public string PBFPubKey { get; set; }
        public string client { get; set; }
        public string alg = "3DES-24";
    }
}