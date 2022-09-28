using DAL.Interfaces;
using DAL.Implementacao;
using Entidades;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Util;

namespace Administrativo.Models.EstadoModel
{
    public class ListaEstadoViewModel
    {

        #region PAGINACAO

        public int TamanhoPagina
        {
            get { return 10; }
        }

        public string ChaveQuantidadeRegistroSession
        {
            get
            {
                return "TotalRegistrosEstado";
            }
        }
        public int QuantidadeRegistro { get; set; }


        #endregion

        #region FILTRO
        public string NomeEstado { get; set; }
        public string PaisSelected { get; set; }


        public List<Pais> Paises { get; set; }
        #endregion

        public StaticPagedList<Estado> Estados { get; set; }

        public string MensagemSucesso { get; set; }
        public bool TemErro { get { return Erros.Count > 0; } }
        public List<string> Erros { get; set; }

        public ListaEstadoViewModel()
        {
            Paises = new List<Pais>();
            Erros = new List<string>();
        }

        public ListaEstadoViewModel(string NomeEstado, string PaisSelected, int? pagina, HttpSessionStateBase session)
        {

            IEstadoDAL estadoDAl = new EstadoDAL();
            IPaisDAL paisDAL = new PaisDAL();

            Paises = new List<Pais>();
            Erros = new List<string>();

            int idPais = 0;

            if (!string.IsNullOrEmpty(PaisSelected))
            {
                int.TryParse(CriptografiaUtil.Decrypt(PaisSelected), out idPais);
            }

            Func<Estado, bool> filtro = e =>
            {
                return (string.IsNullOrEmpty(NomeEstado) || e.NomeEstado.ToLower().Contains(NomeEstado.ToLower()))
                      && (idPais == 0 || e.IdPais == idPais);
            };



            int paginaAtual = pagina ?? 1;

            if (paginaAtual == 1 || session[this.ChaveQuantidadeRegistroSession] == null)
            {
                this.QuantidadeRegistro = estadoDAl.Obter().Count(filtro);
            }
            else
            {
                this.QuantidadeRegistro = int.Parse(session[this.ChaveQuantidadeRegistroSession].ToString());
            }

            var estados = estadoDAl.Obter()
                .Where(filtro)
                .Skip((paginaAtual - 1) * this.TamanhoPagina)
                .Take(this.TamanhoPagina)
                .ToList();

            this.Estados = new StaticPagedList<Estado>(estados, paginaAtual, this.TamanhoPagina, this.QuantidadeRegistro);

            this.Paises = paisDAL.Obter();

            this.NomeEstado = NomeEstado;

            this.PaisSelected = PaisSelected;
           
        }


        public IEnumerable<SelectListItem> GetPaisDropDownList()
        {
            var paisDropDownList = this.Paises.Select(p => new SelectListItem
            {
                Text = p.NomePais,
                Value = Util.CriptografiaUtil.Encrypt(p.IdPais.ToString())
            }).ToList();

            paisDropDownList.Insert(0, new SelectListItem
            {
                Text = "(Selecione um País)",
                Value = Util.CriptografiaUtil.Encrypt("0")
            });

            return paisDropDownList;
        }

        public string GetParametrosWeb(int idEstado)
        {
            Dictionary<string, string> parametros = new Dictionary<string, string>();

            parametros.Add("idEstado", idEstado.ToString());

            return Util.UrlUtil.MontarParametrosUrl(parametros);
        }




    }


}