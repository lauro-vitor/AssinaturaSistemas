using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ServicoFinanceiro
    {

        public int IdServicoFinanceiro { get; set; }
        public int IdContaBancaria { get; set; }
        public int IdPeriodoCobranca { get; set; }
        public string DescricaoServico { get; set; }
        public int DiaVencimento { get; set; }
        public decimal ValorCobranca { get; set; }
        public int QuantidadeParcelas { get; set; }
        public string StripePriceId { get; set; }
        public int StripeOrdem { get; set; }

        public int? IdTipoSistema { get; set; }

    }
}
