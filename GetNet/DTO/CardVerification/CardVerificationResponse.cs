using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO.CardVerification
{
    public class CardVerificationResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("verification_id")]
        public string VerificationId { get; set; }

        [JsonProperty("authorization_code")]
        public string AuthorizationCode { get; set; }

       
        public override string ToString()
        {
            return $@"
status = {Status},
verification_id = {VerificationId},
authorization_code = {AuthorizationCode} ";
        }
    }
}
