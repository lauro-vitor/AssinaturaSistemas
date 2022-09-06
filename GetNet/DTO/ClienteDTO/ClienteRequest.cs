using Getnet.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetNet.DTO.ClienteDTO
{
    public class ClienteRequest
    {
        [JsonProperty("seller_id")]
        public string SellerId { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("document_type")]
        public DocumentType? DocumentType { get; set; }

        [JsonProperty("document_number")]
        public string DocumentNumber { get; set; }

        [JsonProperty("birth_date")]
        public string BirthDate { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("celphone_number")]
        public string CelphoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("observation")]
        public string Observation { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }
        public ClienteRequest()
        {

        }
        public ClienteRequest(string customerId,
            string firstName,
            string lastName,
            DocumentType documentType,
            string documentNumber,
            string birthDate,
            string phoneNumber,
            string celphoneNumber,
            string email,
            string observation,
            Address address)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            DocumentType = documentType;
            DocumentNumber = documentNumber;
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;
            CelphoneNumber = celphoneNumber;
            Email = email;
            Observation = observation;
            Address = address;
        }
    }
}
