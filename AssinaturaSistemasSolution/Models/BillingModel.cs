using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssinaturaSistemasSolution.Models
{
    public class BillingModel
    {
        public string CustomerId { get; set; }
        public string SubscrptionId { get; set; }
        public string PaymentIntentClientSecret { get; set; }
        public bool HasError { get { return this.MessageErros.Count() > 0; } }
        public string PublishableKey { get { return System.Configuration.ConfigurationManager.AppSettings["public_key"]; } }
        public  string SecretKey { get { return System.Configuration.ConfigurationManager.AppSettings["public_key"]; } }
        public List<Price> Prices { get; set; }
        public List<string> MessageErros { get; set; }
  

        public BillingModel()
        {
            this.Prices = new List<Price>();

            this.MessageErros = new List<string>();
        }

        public string ObterPrecoPlano(Price price)
        {
            return (price.UnitAmount ?? 0 / 100).ToString();
        }

        #region SERVICES
        public void LoadPrices()
        {
            var priceService = new PriceService();

            var priceListOptions = new PriceListOptions()
            {
                Limit = 100
            };

            this.Prices = priceService.List(priceListOptions).ToList();

        }

        #endregion
       
    }
}