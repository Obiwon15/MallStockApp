namespace inventoryAppDomain.Entities.Dtos
{
    public class ChargeResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public ChargeData data { get; set; }
    }
}