using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class VwParcela
    {
        public int IdParcela { get; set; }
        public int IdSistema { get; set; }
        public int IdServicoFinanceiro { get; set; }
        public string DescricaoServico { get; set; }
        public int IdStatusParcela { get; set; }
        public string StatusParcelaDescricao { get; set; }
        public int Numero { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataGeracao { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public string DataGeracaoVM { get; set; }
        public string DataVencimentoVM { get; set; }
        public string DataCancelamentoVM { get; set; }
        public decimal ValorPagar { get; set; }
        public decimal Desconto { get; set; }
        public decimal Acrescimo { get; set; }
        public decimal ValorPago { get; set; }
        public string Observacao { get; set; }
    }
}
