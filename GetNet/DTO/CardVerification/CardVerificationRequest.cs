using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO.CardVerification
{
    public class CardVerificationRequest
    {
        /// <summary>
        /// Número do cartão tokenizado. Gerado previamente por meio do endpoint /v1/tokens/card.
        /// </summary>
        [JsonProperty("number_token")]
        public string NumberToken { get; set; }

        /// <summary>
        /// string <= 50 characters "Mastercard" "Visa" "Amex" "Elo" "Hipercard"
        // Bandeira do cartão.
        /// </summary>
        [JsonProperty("brand")]
        public Brand Brand;

        [JsonProperty("cardholder_name")]
        public string CardholderName { get; set; }

        [JsonProperty("expiration_month")]
        public string ExpirationMonth { get; set; }

        [JsonProperty("expiration_year")]
        public string ExpirationYear { get; set; }

        [JsonProperty("security_code")]
        public string SecurityCode { get; set; }
    }
}
