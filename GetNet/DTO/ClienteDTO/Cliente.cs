using Getnet.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetNet.DTO.ClienteDTO
{
    public class Cliente
    {
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("document_type")]
        public DocumentType DocumentType { get; set; }

        [JsonProperty("document_number")]
        public string DocumentNumber { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("celphone_number")]
        public string CelphoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
