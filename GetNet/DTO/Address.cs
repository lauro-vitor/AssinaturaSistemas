using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{
    public class Address
    { 
        [JsonRequired]
        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonRequired]
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonRequired]
        [JsonProperty("complement")]
        public string Complement { get; set; }

       
        [JsonProperty("district")]
        public string District { get; set; }

        [JsonRequired]
        [JsonProperty("city")]
        public string City { get; set; }

        [JsonRequired]
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonRequired]
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonRequired]
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
    }
}
