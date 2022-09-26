using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models.ClienteModel
{
    public class ClienteViewModel : BaseManterViewModel
    {
        public Cliente  Cliente { get; set; }
        public List<Pais> Paises { get; set; }
        public List<Estado> Estados { get; set; }
        public ClienteViewModel()
        {
            Paises = new List<Pais>();

            Estados = new List<Estado>();
        }

        public IEnumerable<SelectListItem> ObterPaisesDropDownListFor()
        {
            var paisesDropDownList = this.Paises.Select(p => new SelectListItem() { 
                Value = p.IdPais.ToString(),
                Text = p.NomePais
            }).ToList();

            paisesDropDownList.Insert(0, new SelectListItem()
            {
                Value = "0",
                Text ="(Selecione um País)"
            });

            return paisesDropDownList;
        }
        public IEnumerable<SelectListItem> ObterEstadosDropDownListFor()
        {
            var estadosDropDownList = this.Estados.Select(e => new SelectListItem()
            {
                Value = e.IdEstado.ToString(),
                Text = e.NomeEstado
            }).ToList();

            estadosDropDownList.Insert(0, new SelectListItem()
            {
                Value = "0",
                Text = "(Selecione um Estado)"
            });

            return estadosDropDownList;
        }
    }
}