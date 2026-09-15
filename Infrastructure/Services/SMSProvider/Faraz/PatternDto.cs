using System;
namespace CleanArchitecture.Infrastructure.Services.SMSProvider.Faraz
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class PatternCreateDto
    {
        public string pattern { get; set; }
        public string description { get; set; }
        public bool is_shared { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data1
    {
        public int bulk_id { get; set; }
    }

    public class PatternResponseDto
    {
        public Value value { get; set; }
    }

    public class Value
    {
        public string status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }


}

