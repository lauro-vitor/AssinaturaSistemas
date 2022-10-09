using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class PagamentoParcela
    {
        public int IdPagamentoParcela { get; set; }
        public int IdParcela { get; set; }
        public DateTime DataPagamento { get; set; }
        public string DataPagamentoVM { get; set; }
        public decimal ValorDepositoBancario { get; set; }
        public decimal ValorCartaoCredito { get; set; }

        public decimal ValorCartaoDebito { get; set; }
        public string StripePaymentIntentId { get; set; }

        
    }
}
