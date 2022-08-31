using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{
    public class Customer
    {
        /// <summary>
        /// Identificador do comprador. 
        /// </summary>
        [JsonRequired]
        [JsonProperty("customer_id")]
        public string CustomerID { get; set; }

        /// <summary>
        /// Primeiro nome do comprador.
        /// </summary>
        [JsonRequired]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Último nome do comprador.
        /// </summary>
        [JsonRequired]
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Nome completo do comprador. Obrigatório para boleto.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonRequired]
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Tipo do documento de identificação do comprador. Obrigatório para boleto.
        /// Campo obrigatório para o Antifraude
        /// </summary>
        [JsonRequired]
        [JsonProperty("document_type")]
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// Tipo do documento de identificação do comprador. 
        /// </summary>
        [JsonRequired]
        [JsonProperty("document_number")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Telefone do comprador. (sem máscara)
        /// </summary>
        [JsonRequired]
        [JsonProperty("phone_number")]
        public string PhoneNumber   { get; set; }

        /// <summary>
        /// Conjunto de dados referentes ao endereço de cobrança
        /// </summary>
        [JsonRequired]
        [JsonProperty("billing_address")]
        public BillingAddress BillingAddress { get; set; }

    }
}
