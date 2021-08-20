using System;

namespace inventoryAppDomain.Entities.Dtos
{
    public class TransactionDetails
    {
        public string PBFPubKey { get; set; }
        public string cardno { get; set; }
        public string cvv { get; set; }
        public string expirymonth { get; set; }
        public string expiryyear { get; set; }
        public string currency { get; set; } = "NGN";
        public string country { get; set; } = "NG";
        public string amount { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; } = "";
        public string firstname { get; set; } = "";
        public string lastname { get; set; } = "";
        public string IP { get; set; } = "";
        public string txRef { get; set; }
        public Array meta { get; set; } = new object[1];
        public string redirect_url { get; set; } = "https://localhost:44324";
        public string device_fingerprint { get; set; } = "";
    }
}