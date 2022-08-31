using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
namespace Getnet.DTO
{
    /// <summary>
    /// Conjunto de dados para identificação da compra
    /// </summary>
    public class Order
    {

        /// <summary>
        /// Código de identificação da compra utilizado pelo e-commerce
        /// </summary>
        [JsonRequired]
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        /// <summary>
        /// Valor de impostos
        /// </summary>
        [JsonProperty("sales_tax")]
        public decimal SalesTax { get; set; }

        /// <summary>
        /// Identificador do tipo de produto vendido dentre as opções
        /// </summary>

       [JsonIgnore]
        public ProductType ProductType { get; set; }

    }
}
