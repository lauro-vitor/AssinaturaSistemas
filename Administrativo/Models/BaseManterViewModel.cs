using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    public class BaseManterViewModel
    {
        public string MensagemSucesso { get; set; }
        public string MensagemAviso { get; set; }
        public List<string> Erros { get; set; }
        public bool TemErro { get { return Erros.Count > 0; } }
        public BaseManterViewModel()
        {
            this.Erros = new List<string>();
        }
    }
}