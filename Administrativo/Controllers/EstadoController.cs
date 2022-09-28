using Administrativo.Models.EstadoModel;
using BLL;
using DAL.Interfaces;
using DAL.Implementacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Util;

namespace Administrativo.Controllers
{
    public class EstadoController : Controller
    {
        private readonly IEstadoDAL _estadoDAL;
        private readonly IPaisDAL _paisDAL;
        public EstadoController()
        {
            _estadoDAL = new EstadoDAL();
            _paisDAL = new PaisDAL();
        }

        [HttpGet]

        // GET: Estado
        public ActionResult Index(string NomeEstado, string PaisSelected, int? pagina)
        {
            ListaEstadoViewModel listaEstadoViewModel = new ListaEstadoViewModel();
            try
            {
                listaEstadoViewModel = new ListaEstadoViewModel(NomeEstado, PaisSelected, pagina, Session);
            }
            catch (Exception ex)
            {
                listaEstadoViewModel.Erros.Add(ex.Message);
            }


            return View(listaEstadoViewModel);
        }



        [HttpGet]
        public ActionResult Excluir(string IdEstadoSelected, string NomeEstado, string PaisSelected)
        {
            ListaEstadoViewModel listaEstadoViewModel = new ListaEstadoViewModel();

            try
            {

                int idEstado = 0;

                if (string.IsNullOrEmpty(IdEstadoSelected))
                {
                    throw new Exception("Ocorreu um erro ao exluir!");
                }

                idEstado = int.Parse(CriptografiaUtil.Decrypt(IdEstadoSelected));


                _estadoDAL.Excluir(idEstado);

                listaEstadoViewModel = new ListaEstadoViewModel(NomeEstado, PaisSelected, null, Session);

                listaEstadoViewModel.MensagemSucesso = "Estado excluído com sucesso!";

            }
            catch (Exception ex)
            {
                listaEstadoViewModel.Erros.Add(ex.Message);
            }

            return View("Index", listaEstadoViewModel);
        }

        [HttpGet]
        public ActionResult Manter(string IdEstadoSelected)
        {
            EstadoViewModel estadoViewModel = new EstadoViewModel();

            try
            {
                if (!string.IsNullOrEmpty(IdEstadoSelected))
                {
                    int idEstado = int.Parse(CriptografiaUtil.Decrypt(IdEstadoSelected));

                    estadoViewModel.Estado = _estadoDAL.ObterPorId(idEstado);
                }

                estadoViewModel.Paises = _paisDAL.Obter();
            }
            catch (Exception ex)
            {
                estadoViewModel.Erros.Add(ex.Message);
            }
            return View(estadoViewModel);
        }
        [HttpPost]
        public ActionResult Manter(FormCollection formCollection)
        {
            EstadoViewModel estadoViewModel = new EstadoViewModel();

            try
            {

                var estadoBLL = new EstadoBLL();

                var nomeEstado = formCollection["Estado.NomeEstado"];
                var idEstadoSelected = formCollection["Estado.IdEstado"];
                var idPaisSelected = formCollection["Estado.IdPais"];

                int idPais = int.Parse(idPaisSelected ?? "0");
                int idEstado = int.Parse(idEstadoSelected ?? "0");

                estadoViewModel.Estado.NomeEstado = nomeEstado;
                estadoViewModel.Estado.IdEstado = idEstado;
                estadoViewModel.Estado.IdPais = idPais;

                var estados = _estadoDAL.Obter();

                var resultado = estadoBLL.ValidarEstado(idEstado, nomeEstado, idPais, estados);

                estadoViewModel.Erros.AddRange(resultado);

                if (!estadoViewModel.TemErro)
                {
                    if (idEstado <= 0)
                    {
                        estadoViewModel.Estado = _estadoDAL.Criar(idPais, nomeEstado);
                        estadoViewModel.MensagemSucesso = "Estado cadastrado com sucesso!";
                    }
                    else
                    {
                        estadoViewModel.Estado = _estadoDAL.Alterar(idEstado, idPais, nomeEstado);
                        estadoViewModel.MensagemSucesso = "Estado alterado com sucesso!";
                    }
                }

                estadoViewModel.Paises = _paisDAL.Obter();

            }
            catch (Exception ex)
            {
                estadoViewModel.Erros.Add(ex.Message);
            }

            return View(estadoViewModel);
        }

        [HttpGet]
        public JsonResult ObterEstadosDoPais(int idPais)
        {
            try
            {
                HttpContext.Response.StatusCode = 200;
                var estados = _estadoDAL.Obter()
                    .Where(e => e.IdPais == idPais)
                    .OrderBy(e => e.NomeEstado)
                    .Select(e => new SelectListItem
                    {
                        Text = e.NomeEstado,
                        Value = e.IdEstado.ToString()
                    });

                return Json(new { 
                        estados
                    }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;

                return Json(new { 
                    mensagem = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}