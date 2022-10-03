using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class VwServicoFinanceiro
    {
        public int IdServicoFinanceiro { get; set; }
        public string ContaBancariaDescricao { get; set; }
        public string PeriodoCobrancaDescricao { get; set; }
        public string DescricaoServico { get; set; }
        public int DiaVencimento { get; set; }
        public decimal ValorCobranca { get; set; }
        public int QuantidadeParcelas { get; set; }
    }
}
