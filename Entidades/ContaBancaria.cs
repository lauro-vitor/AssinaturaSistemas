using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class ContaBancaria
    {
        public int IdContaBancaria { get; set; }
        public string NumeroAgencia { get; set; }
        public string NumeroConta { get; set; }
        public int NumeroBanco { get; set; }
        public string NomeBanco { get; set; }
        public string  Cnpj { get; set; }

    }
}
