using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models.EstadoModel
{
    public class EstadoViewModel
    {
        public Estado Estado { get; set; }
        public string MensagemSucesso { get; set; }
        public bool TemErro { get { return Erros.Count > 0; } }
        public List<string> Erros { get; set; }
        public List<Pais> Paises { get; set; }

        public EstadoViewModel()
        {
            Paises = new List<Pais>();
            Erros = new List<string>();
            Estado = new Estado();
        }

        public IEnumerable<SelectListItem> GetPaisDropDownList()
        {
            var paisDropDownList = this.Paises.Select(p => new SelectListItem
            {
                Text = p.NomePais,
                Value = p.IdPais.ToString()
            }).ToList();

            paisDropDownList.Insert(0, new SelectListItem
            {
                Text = "(Selecione um País)",
                Value = "0"
            });

            return paisDropDownList;
        }


    }
}