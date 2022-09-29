using Administrativo.Models;
using Administrativo.Models.PaisModel;
using BLL;
using DAL.Interfaces;
using DAL.Implementacao;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Util;
using Administrativo.Filters;

namespace Administrativo.Controllers
{
    [AccountFilter]
    [Authorize]
    public class PaisController : Controller
    {
        private readonly IPaisDAL _paisDAL;
        public PaisController()
        {
            _paisDAL = new PaisDAL();
        }
        // GET: Pais
        [HttpGet]
        public ActionResult Index(string nomePais)
        {
            ListaPaisViewModel listaPaisModel = new ListaPaisViewModel();

            try
            {
                listaPaisModel.Paises = _paisDAL.Obter()
                    .Where(p => string.IsNullOrEmpty(nomePais) || p.NomePais.ToLower().Contains(nomePais.ToLower()))
                    .OrderBy(p => p.NomePais)
                    .ToList();
            }
            catch (Exception ex)
            {
                listaPaisModel.Erros.Add(ex.Message);
            }

            return View(listaPaisModel);
        }

        [HttpGet]
        public ActionResult Manter(string q)
        {
            PaisViewModel paisViewModel = new PaisViewModel();

            try
            {
                if (!string.IsNullOrEmpty(q))
                {
                    var parametros = UrlUtil.RecuperarParametrosUrl(q);

                    if (parametros["idPais"] != null)
                    {
                        int idPais = int.Parse(parametros["idPais"]);

                        paisViewModel.Pais = _paisDAL.ObterPorId(idPais);
                    }
                }
            }
            catch (Exception ex)
            {
                paisViewModel.Erros.Add(ex.Message);
            }

            return View(paisViewModel);
        }

        [HttpPost]
        public ActionResult Manter(FormCollection formCollection)
        {
            PaisViewModel novoPaisViewModel = new PaisViewModel();

            try
            {
                PaisBLL paisBLL = new PaisBLL();

                string nomePais = formCollection["Pais.NomePais"];

                int idPais = 0;

                int.TryParse(formCollection["Pais.IdPais"], out idPais);

                var paisRetorno = new Pais();

                var paises = _paisDAL.Obter();

                var mensagens = paisBLL.ValidarPais(idPais, nomePais, paises);

                novoPaisViewModel.Erros.AddRange(mensagens);

                novoPaisViewModel.Pais.NomePais = nomePais;

                novoPaisViewModel.Pais.IdPais = idPais;

                if (!novoPaisViewModel.TemErro)
                {

                    if (idPais == 0)
                    {
                        paisRetorno = _paisDAL.Salvar(null, nomePais);
                        novoPaisViewModel.MensagemSucesso = "País cadastrado com sucesso";
                    }
                    else
                    {
                        paisRetorno = _paisDAL.Salvar(idPais, nomePais);
                        novoPaisViewModel.MensagemSucesso = "País alterado com sucesso";
                    }
                }

                novoPaisViewModel.Pais = paisRetorno;
            }
            catch (Exception ex)
            {
                novoPaisViewModel.Erros.Add(ex.Message);
            }

            return View(novoPaisViewModel);
        }

        [HttpGet]
        public ActionResult Excluir(string q)
        {

            ListaPaisViewModel listaPaisModel = new ListaPaisViewModel();

            try
            {

                var parametros = UrlUtil.RecuperarParametrosUrl(q);

                if (parametros["idPais"] == null)
                {
                    throw new Exception("Id Pais é Obrigatório!");
                }

                int idPais = int.Parse(parametros["idPais"]);

                _paisDAL.Excluir(idPais);

                listaPaisModel.Paises = _paisDAL.Obter().OrderBy(p => p.NomePais).ToList();

                listaPaisModel.MensagemSucesso = "País Excluido com sucesso!";
            }
            catch (Exception ex)
            {
                listaPaisModel.Erros.Add(ex.Message);
            }


            return View("Index", listaPaisModel);
        }
        [HttpGet]
        public JsonResult ObterPaises()
        {
            try
            {
              
                HttpContext.Response.StatusCode = 200;

                var paises = _paisDAL.Obter()
                    .OrderBy(p => p.NomePais)
                    .Select(p => new SelectListItem()
                    {
                        Value = p.IdPais.ToString(),
                        Text = p.NomePais
                    });

                return Json(new
                {
                    paises
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;

                return Json(new
                {
                    mensagem = ex.Message
                }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}