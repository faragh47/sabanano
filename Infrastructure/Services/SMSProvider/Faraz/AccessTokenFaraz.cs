using System;
namespace CleanArchitecture.Infrastructure.Services.SMSProvider.Faraz
{
  
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public User user { get; set; }
    }

    public class AccessTokenFaraz
    {
        public string status { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string document_block { get; set; }
        public string send_block { get; set; }
        public string mobile { get; set; }
        public string tellephone { get; set; }
        public string national_id { get; set; }
        public string certificate_id { get; set; }
        public string address { get; set; }
        public string postal_code { get; set; }
        public string company { get; set; }
        public DateTime expire { get; set; }
        public string status { get; set; }
    }



}

