using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{
    public class TokenizationRequest
    {
        [JsonProperty("card_number")]
        public string CardNumber { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }
    }
}
