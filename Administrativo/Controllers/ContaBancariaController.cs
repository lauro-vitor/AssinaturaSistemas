using Administrativo.Filters;
using BLL;
using DAL.Implementacao;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Controllers
{
    [AccountFilter]
    [Authorize]
    public class ContaBancariaController : Controller
    {
        private readonly ContaBancariaDAL _contaBancariaDAL;
        public ContaBancariaController()
        {
            _contaBancariaDAL = new ContaBancariaDAL();
        }
        // GET: ContaBancaria
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObterVarios()
        {
            try
            {
                var contasBancarias = _contaBancariaDAL.ObterVarios();
                int total = contasBancarias.Count;

                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    total,
                    contasBancarias
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
        public JsonResult ObterPorId(int id)
        {
            try
            {
                var contaBancariaRetorno = _contaBancariaDAL.ObterPorId(id);
                HttpContext.Response.StatusCode = 200;
                return Json(new
                {
                    contaBancariaRetorno
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
                });
            }
        }

        [HttpPost]
        public JsonResult Criar(ContaBancaria contaBancaria)
        {
            try
            {
                var contaBancariaBll = new ContaBancariaBLL();

                var erros = contaBancariaBll.ValidarContaBancaria(contaBancaria);

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        erros
                    });
                }

                var contaBancariaRetorno = _contaBancariaDAL.Criar(contaBancaria);

                HttpContext.Response.StatusCode = 201;

                return Json(new
                {
                    contaBancaria = contaBancariaRetorno,
                    mensagem = "Conta Bancária criada com sucesso"
                });
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
                });
            }
        }

        [HttpPost]
        public JsonResult Editar(ContaBancaria contaBancaria)
        {
            try
            {

                var contaBancariaBll = new ContaBancariaBLL();

                var erros = contaBancariaBll.ValidarContaBancaria(contaBancaria);

                if (contaBancaria.IdContaBancaria <= 0)
                {
                    erros.Add("IdContaBancária é obrigatório");
                }

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        erros
                    });
                }

                var contaBancariaRetorno = _contaBancariaDAL.Editar(contaBancaria);

                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    contaBancaria = contaBancariaRetorno,
                    mensagem = "Conta Bancária editada com sucesso"
                });
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
                });
            }
        }
        [HttpPost]
        public JsonResult Deletar(int id)
        {
            try
            {
                _contaBancariaDAL.Deletar(id);

                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    mensagem = "Conta bancária excluída com sucesso"
                });
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
                });
            }
        }
    }
}