using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{
    /// <summary>
    /// Conjunto de dados referentes ao endereço de entrega.
    /// </summary>
    public class Shipping
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        
        [JsonRequired]
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Valor do frete em centavos.
        /// </summary>
        [JsonProperty("shipping_amount")]
        public long ShippingAmount { get; set; }

        [JsonRequired]
        [JsonProperty("address")]
        public Address Address { get; set; }
    }
}
