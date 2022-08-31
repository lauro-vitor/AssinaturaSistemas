using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{
    public class Card
    {
        /// <summary>
        /// Número do cartão tokenizado.
        /// </summary>
        /// 
        [JsonRequired]
        [JsonProperty("number_token")]
        public string NumberToken { get; set; }

        /// <summary>
        /// Nome do comprador impresso no cartão.
        /// </summary>
        ///
        [JsonRequired]
        [JsonProperty("cardholder_name")]
        public string CardholderName { get; set; }

        /// <summary>
        /// Código de segurança. CVV ou CVC.
        /// </summary>
        /// 
        [JsonRequired]
        [JsonProperty("security_code")]
        public string SecurityCode { get; set; }

        [JsonRequired]
        [JsonProperty("brand")]
        public Brand Brand { get; set; }

        /// <summary>
        /// Mês de expiração do cartão com dois dígitos.
        /// </summary>
        /// 
        [JsonRequired]
        [JsonProperty("expiration_month")]
        public string ExpirationMonth { get; set; }

        /// <summary>
        /// Ano de expiração do cartão com dois dígitos.
        /// </summary>
        /// 
        [JsonRequired]
        [JsonProperty("expiration_year")]
        public string ExpirationYear { get; set; }

    }
}
