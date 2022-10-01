using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Sistema
    {
        public int IdSistema { get; set; }
        public int IdTipoSistema { get; set; }
        public int IdCliente { get; set; }
        public string DominioProvisorio { get; set; }
        public string Dominio { get; set; }
        public string Pasta { get; set; }
        public string BancoDeDados { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataCancelamento { get; set; }

        public string DataInicioVM
        {
            get
            {
                return DataInicio.ToString("yyyy-MM-dd");
            }
        }
        public string DataCancelamentoVM
        {
            get
            {
                if (DataCancelamento.HasValue)
                {
                    return DataCancelamento.Value.ToString("yyyy-MM-dd");
                }

                return "";
            }
        }
    }
}
