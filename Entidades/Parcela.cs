using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Parcela
    {
        public int IdParcela { get; set; }
        public int IdSistema { get; set; }
        public int IdServicoFinanceiro { get; set; }
        public int IdStatusParcela { get; set; }
        public int Numero { get; set; }
        public DateTime DataGeracao { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public decimal Valor { get; set; }
        public decimal Desconto { get; set; }
        public decimal Acrescimo { get; set; }
        public string Observacao { get; set; }

        public string DataVencimentoVM
        {
            get
            {
                return this.DataVencimento.ToString("yyyy-MM-dd");
            }
        }

    }
}
