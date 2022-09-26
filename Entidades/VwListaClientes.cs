using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class VwListaClientes
    {
        public int IdCliente { get; set; }
        public string NomeEmpresa { get; set; }
        public int IdPais { get; set; }
        public string NomePais { get; set; }
        public int IdEstado { get; set; }
        public string NomeEstado { get; set; }
        public string CodigoPostal { get; set; }
        public string Endereco { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public bool Ativo { get; set; }
    }
}
