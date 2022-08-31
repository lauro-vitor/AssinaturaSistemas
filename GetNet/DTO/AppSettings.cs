using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{
    public class AppSettings
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("url_api")]
        public string UrlApi { get; set; }
        
        [JsonProperty("seller_id")]
        public string SellerId { get; set; }
    }
}
