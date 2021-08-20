namespace inventoryAppDomain.Entities.Dtos
{
    public class InitiateSugestionResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public InitiateChargeData InitiateChargeData { get; set; }
    }
}