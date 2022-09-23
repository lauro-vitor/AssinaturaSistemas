using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripeNet
{
    public class AssinaturaService
    {  
        public AssinaturaService()
        {
            
            SubscriptionItemOptions subscriptionItemOptions = new SubscriptionItemOptions()
            {   //opicional
                //definir valor minimo na subcription
                BillingThresholds = null,

                //opicional
                Metadata = null,

                //obrigatorio
                //o id do objeto price, ##### parei aqui para realizar o cadastro de price
                Price = null,

                PriceData = null,

                Quantity = null,

                TaxRates = null,

                Plan = null,
            };
        }
        /// <summary>
        /// Cada assinatura, é obrigatório 
        /// um cliente
        /// e pelo menos de  1 a 20 SubscriptionItemOptions, com o preco 
        /// </summary>
        public void CriarAssinatura()
        {

        }
    }
}
