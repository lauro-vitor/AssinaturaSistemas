using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Administrativo.Models.PaisModel
{
    public class PaisViewModel
    {
        public string MensagemSucesso { get; set; }
        public bool TemErro { get { return Erros.Count > 0; } }
        public List<string> Erros { get; set; }
        public Pais Pais { get; set; }
        public PaisViewModel()
        {
            Pais = new Pais();
            Erros = new List<string>();
        }

    }
}