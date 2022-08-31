using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO
{
    /// <summary>
    /// Para que a Getnet possa cumprir com as regras das Bandeiras e Arranjos, as Leis Federais e determinações 
    /// do BACEN (Banco Central do Brasil) para identificação das entidades finais (subcomércios) que fazem as 
    /// transações financeiras, os Facilitadores de Pagamento devem enviar os dados de identificação de seus clientes 
    /// a cada transação enviada à Getnet.
    /// </summary>
    public class SubMerchant
    {
        /// <summary>
        /// ID do Sub comércio.
        /// </summary>
        [JsonProperty("identification_code")]
        public string IdentificationCode { get; set; }

        /// <summary>
        /// Tipo de documento do Subcomércio.
        /// </summary>
        [JsonProperty("document_type")]
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// CNPJ ou CPF do Subcomércio.
        /// </summary>
        [JsonProperty("document_number")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Logradouro do Subcomércio. (Não permitido o uso de vírgula)
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Cidade do Subcomércio.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// Estado do Subcomércio.
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// Código Postal (CEP) do Subcomércio.
        /// </summary>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

    }
}
