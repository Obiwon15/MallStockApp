using System.ComponentModel.DataAnnotations.Schema;

namespace inventoryAppDomain.Entities.Dtos
{
    public class ChargeData
    {
        public string id { get; set; }
        public string txRef { get; set; }
        public string orderRef { get; set; }
        public string flwRef { get; set; }
        public string redirectUrl { get; set; }
        public string amount { get; set; }
        public string charged_amount { get; set; }
        public string chargeResponseCode { get; set; }
        public string chargeResponseMessage { get; set; }
        public string authModelUsed { get; set; }
        public string authurl { get; set; }
        public string paymentType { get; set; }
        public string processor_response { get; set; }
        public string currency { get; set; }
        public string status { get; set; }
        public string fraud_status { get; set; }
    }
}