using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Getnet.DTO.Credit;

namespace Getnet.DTO.PaymentCreditCard
{
    public class PaymentCreditCardRequest
    {
        /// <summary>
        /// Código de identificação do e-commerce.
        /// </summary>
        ///
        [JsonProperty("seller_id")]
        public string SellerId { get; set; }

        /// <summary>
        /// Valor da compra em centavos
        /// </summary>
        [JsonRequired]
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Identificação da moeda (Consultar código em "https://www.currency-iso.org/en/home/tables/table-a1.html").
        /// </summary>
        /// 
        [JsonProperty("currency")]
        public Currency Currency { get; set; }

        /// <summary>
        /// Conjunto de dados para identificação da compra
        /// </summary>
        [JsonRequired]
        [JsonProperty("order")]
        public Order Order { get; set; }

        /// <summary>
        /// Conjunto de dados referentes ao comprador.
        /// </summary>
        [JsonRequired]
        [JsonProperty("customer")]
        public Customer Customer { get; set; }

        /// <summary>
        /// Conjunto de dados referentes ao dispositivo utilizado pelo comprador.
        /// </summary>
        [JsonProperty("device")]
        public Device Device { get; set; }
        /// <summary>
        /// Conjunto de dados referentes ao endereço de entrega.
        /// </summary>
        /// 
        [JsonRequired]
        [JsonProperty("shippings")]
        public List<Shipping> Shippings { get; set; }

        [JsonProperty("sub_merchant")]
        public SubMerchant SubMerchant { get; set; }
        /// <summary>
        /// Conjunto de dados referentes a transação de crédito.
        /// </summary>
        /// 
        [JsonRequired]
        [JsonProperty("credit")]
        public CreditRequest Credit { get; set; }

    }
}
