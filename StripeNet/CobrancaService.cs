using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripeNet
{
    /// <summary>
    /// assinatura funcionando
    /// </summary>
    public class CobrancaService
    {
        public CobrancaService()
        {
            StripeConfiguration.ApiKey = ConfigurationManager.AppSettings["secret_key"];
        }
        public List<Charge>  ListarTodasAssinaturas()
        {
            var options = new ChargeListOptions()
            {
                Limit = 100
            };
            var service = new ChargeService();

            StripeList<Charge> charges = service.List(options);

            return charges.ToList();
        }
        public void CriarAssinaturaTeste()
        {
           

            var options = new ChargeCreateOptions()
            {
                Amount = 200,
                Currency = "brl",
                Source = "tok_visa",
                Description = "My First Test Charge(created for API docs at https://www.stripe.com/docs/api)",
                
            };

            var service = new ChargeService();

            service.Create(options);

        }
        /// <summary>
        ///
        /// </summary>
        public void CriarAssinatura()
        {
            var charge = new Charge()
            {
                Id = "",
                //valor da assinatura em centavos
                Amount = 100,

                //valor em centavos, pode ser feito uma cobrança parcial
                AmountCaptured = 0,

                //valor reenbolsado em centavos
                AmountRefunded = 0,

                //id of the connect application that created the charge
                Application=null,

                //aplicar uma tarifa adicional a assinatura
                ApplicationFee = null,


                Currency = "brl",


            };

            //balanceTransaction
            //representa os detalhes da transacao na conta bancária
            //eles são criados para cada tipo de transação que entre e saí da conta
            BalanceTransaction balanceTransaction = new BalanceTransaction()
            {
                Id = "txn_3LfOuoLmrf2PWwIx0zAP04X8", //identificacao unica do objeto
                Amount = 100,//valor em centavos no caso 1 real
                //AvailableOn = DateTime.Now, // data em que o dinehrio entra na conta
                Currency = "brl", //tipo de moeda que irá receber
                Description = "My First test charge(create for API docs)", //nao obrigatório
                Net = 100, // valor líquido da transacao em centavos
                ReportingCategory = "charge",
                SourceId = "ch_3LfOuoLmrf2PWwIx0ouXRFDe",//objeto a qual a assinatura esta relacionada, deve ser a assinatura
                Status = "available",
                Type = "charge",//assinatura

            };

            //billing details
            //informações associadas a cobranca na hora da transacao
            // bem como o nome do cliente, telefone, email e endereco
            ChargeBillingDetails billingDetails = new ChargeBillingDetails()
            {
                Address = new Address()
                {
                    City = null,
                    Country = null,
                    Line1 = null,
                    Line2 = null,
                    PostalCode = null,
                    State = null,
                },
                Email = null,
                Name = null,
                Phone = null
            };

            Customer customer = new Customer()
            {
                Id = "",
                Address = new Address()
                {
                    City = null,
                    Country = "HN",
                    Line1 = null,
                    Line2 = null,
                    PostalCode = null,
                    State = null,
                },

                Balance = 0, //curent balance =  saldo atual
                Created = DateTime.Now,
                Currency = "brl",//moeada de cobranca
                DefaultSourceId = "card_1LVepd2eZvKYlo2C2BTxeOQu",// id do pagamento padrao do cliente
                // inadimplente
                //quando o pagamento mais recente da assinatura falhou então esse valor é alterado automáticamente para true
                //ideal para verificar os devedores
                Delinquent = false,

                Description = null,

                //descreve o desconto atual para o cliente
                Discount = null,

                Email = "stripe@teste.com",

                //quando for gerar as parcelas esse prefixo vai ser diferenciado para esse cliente
                InvoicePrefix = "AB94287",


            };







        }
    }
}
