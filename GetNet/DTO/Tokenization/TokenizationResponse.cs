using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{
    public class TokenizationResponse
    {
        [JsonProperty("number_token")]
        public string NumberToken { get; set; }
    }
}
