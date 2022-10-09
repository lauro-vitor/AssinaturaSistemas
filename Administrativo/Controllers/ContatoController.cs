using DAL.Interfaces;
using DAL.Implementacao;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Administrativo.Filters;

namespace Administrativo.Controllers
{
    [AccountFilter]
    [Authorize]
    public class ContatoController : Controller
    {
        private readonly ContatoDAL _contatoDAL;
        public ContatoController()
        {
            _contatoDAL = new ContatoDAL();
        }
        // GET: Contato
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Criar(Contato contato)
        {
            
            try
            {
                var contatoBLL = new ContataoBLL();

                var contatos = _contatoDAL.ObterVarios();

                var erros = contatoBLL.ValidarContato(contato.IdContato, contato.IdCliente, contato.NomeCompleto, contato.Email,
                    contato.Celular, contato.Telefone, contato.Senha, contatos);

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        erros
                    });
                }

                Contato contatoRetorno = _contatoDAL.Criar(contato);

                HttpContext.Response.StatusCode = 201;

                return Json(new
                {
                    dados = contatoRetorno,
                    mensagemSucesso = "Contato criado com sucesso!"
                });
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;

                return Json(new
                {
                    erros = new List<string>()
                    {
                        ex.Message
                    }
                });
            }

        }

        [HttpPost]
        public JsonResult Editar(Contato contato)
        {

            try
            {
                var contatoBLL = new ContataoBLL();

                var contatos = _contatoDAL.ObterVarios();

                var erros = contatoBLL.ValidarContato(contato.IdContato, contato.IdCliente, contato.NomeCompleto, contato.Email,
                    contato.Celular, contato.Telefone, contato.Senha, contatos);

                if (contato.IdContato <= 0)
                    erros.Add("Id do contato é obrigatório");

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        erros
                    });
                }


                Contato contatoRetorno = _contatoDAL.Editar(contato);

                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    dados = contatoRetorno,
                    mensagemSucesso = "Contato editado com sucesso!"
                });
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;

                return Json(new
                {
                    erros = new List<string>()
                    {
                        ex.Message
                    }
                });
            }

        }
        [HttpPost]
        public JsonResult Deletar(int idContato)
        {
            try
            {
                _contatoDAL.Deletar(idContato);

                HttpContext.Response.StatusCode = 200;

                return Json(new { });

            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;
                return Json(new
                {
                    erros = new List<string>()
                    {
                        ex.Message
                    }
                });
            }
        }

        [HttpGet]
        public JsonResult ObterVarios(int idCliente, string nomeCompleto, string email, string celular, string telefone, int page, int pageSize)
        {
            try
            {
              
                int total = 0;

                var contatosAux = _contatoDAL.ObterVarios().Where(c => c.IdCliente == idCliente &&
                    (string.IsNullOrEmpty(nomeCompleto) || c.NomeCompleto.ToLower().Contains(nomeCompleto.ToLower().Trim())) &&
                    (string.IsNullOrEmpty(email) || c.Email.ToLower().Contains(email.ToLower().Trim())) &&
                    (string.IsNullOrEmpty(celular) || c.Celular.ToLower().Contains(celular.ToLower().Trim())) &&
                    (string.IsNullOrEmpty(telefone) || c.Telefone.ToLower().Contains(telefone.ToLower().Trim())));

                total = contatosAux.Count();

             
                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    dados = contatosAux,
                    total,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;

                return Json(new
                {
                    erros = new List<string>
                    {
                        ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult ObterPorId(int idContato)
        {
            try
            {
                var contato = _contatoDAL.ObterPorId(idContato);


                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    dados = contato,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;

                return Json(new
                {
                    erros = new List<string>
                    {
                        ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}