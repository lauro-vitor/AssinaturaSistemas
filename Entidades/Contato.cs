using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Contato
    {
        public int IdContato { get; set; }
        public int IdCliente { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }

    }
}
