using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Util;

namespace Administrativo.Models.PaisModel
{
    public class ListaPaisViewModel
    {
        public string MensagemSucesso { get; set; }
        public string NomePais { get; set; }
        public bool TemErro { get { return Erros.Count > 0; } }
        public List<Pais> Paises { get; set; }
        public List<string> Erros { get; set; }
        public ListaPaisViewModel()
        {
            Paises = new List<Pais>();
            Erros = new List<string>();
        }

        public string MontarParametroQueryString(int idPais)
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();

            parametros.Add("idPais", idPais.ToString());

            return UrlUtil.MontarParametrosUrl(parametros);
        }
    }
}