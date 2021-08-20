namespace inventoryAppDomain.Entities.MonnifyDtos
{
    public class ResponseDto
    {
        public bool requestSuccessful { get; set; }
        public string responseMessage { get; set; }
        public string responseCode { get; set; }
        public object responseBody { get; set; }
    }
}