using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripeNet
{
    public class PrecoServico
    {
        private readonly PriceService _priceService;
        public PrecoServico()
        {
            StripeConfiguration.ApiKey = System.Configuration.ConfigurationManager.AppSettings["secret_key"];
            _priceService = new PriceService();
        }

        /// <summary>
        /// Metodo utilzado parar criar preco de produtos que terao cobranca recorrente
        /// </summary>
        /// <param name="idProduto"></param>
        /// <param name="valorUnidade"></param>
        /// <param name="currency"></param>
        /// <param name="intervaloRecorrencia">
        /// valores possiveis sao: day, week, month or year
        /// </param>
        /// <param name="quantidadeIntervaloRecorrencia">
        /// quantidade em que sera cobrada a recorrencia por exemplo, se for mensal e quantidade = 3 entao sera cobrado somente durante 3 meses o valor
        /// O maximo permitido eh 1 ano, 12 meses e 52 semanas  
        /// </param>
        public void CriarPreco(string idProduto, long quanidadeUnidade, string currency, string intervaloRecorrencia, long quantidadeIntervaloRecorrencia)
        {
            PriceRecurringOptions priceRecurringOptions = new PriceRecurringOptions()
            {
                Interval = intervaloRecorrencia,
                IntervalCount = quantidadeIntervaloRecorrencia,

            };

            var options = new PriceCreateOptions
            {
                Product = idProduto,
                UnitAmount = quanidadeUnidade,
                Currency = currency,
                Recurring = priceRecurringOptions
            };

            _priceService.Create(options);
        }

        public StripeList<Price> ListarTodosPrecos(int limite)
        {
            var options = new PriceListOptions()
            {
                Limit = limite,
            };

            return _priceService.List(options);
        }

    }
}
