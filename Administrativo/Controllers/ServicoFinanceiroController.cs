using Administrativo.Filters;
using BLL;
using DAL.Implementacao;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Controllers
{
    [AccountFilter]
    [Authorize]
    public class ServicoFinanceiroController : Controller
    {
        private readonly ServicoFinanceiroDAL _servicoFinanceirDAL;
        private readonly ServicoFinanceiroBLL _servicoFinanceiroBLL;
        private readonly ContaBancariaDAL _contaBancariaDAL;
        private readonly PeriodoCobrancaDAL _periodoCobrancaDAL;
        private readonly VwServicoFinanceiroDAL _vwServicoFinanceiroDAL;

        public ServicoFinanceiroController()
        {
            _servicoFinanceirDAL = new ServicoFinanceiroDAL();
            _servicoFinanceiroBLL = new ServicoFinanceiroBLL();
            _contaBancariaDAL = new ContaBancariaDAL();
            _periodoCobrancaDAL = new PeriodoCobrancaDAL();
            _vwServicoFinanceiroDAL = new VwServicoFinanceiroDAL();
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObterVarios()
        {
            try
            {
               

                var servicosFinanceiro = _vwServicoFinanceiroDAL.ObterVarios();
                int total = servicosFinanceiro.Count();

                var periodosCobranca = _periodoCobrancaDAL
                    .ObterVarios()
                    .Select(c => new SelectListItem()
                    {
                        Text = c.Descricao,
                        Value = c.IdPeriodoCobranca.ToString()
                    });

                var contasBancarias = _contaBancariaDAL
                    .ObterVarios()
                    .Select(c => new SelectListItem
                    {
                        Text = $"Conta: {c.NumeroConta} Ag: {c.NumeroAgencia}",
                        Value = c.IdContaBancaria.ToString()
                    });

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new
                {
                    servicosFinanceiro,
                    total,
                    periodosCobranca,
                    contasBancarias
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return Json(new
                {
                    erros = new List<string>()
                    {
                        ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public JsonResult Criar(ServicoFinanceiro servicoFinanceiroRequest)
        {
            try
            {
                var erros = _servicoFinanceiroBLL.ValidarServicoFinanceiro(servicoFinanceiroRequest);

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new
                    {
                        erros
                    }, JsonRequestBehavior.AllowGet);
                }

                var servicoFinanceiro = _servicoFinanceirDAL.Criar(servicoFinanceiroRequest);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

                return Json(new
                {
                    servicoFinanceiro,
                    mensagem = "Serviço financeiro criado com sucesso"
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return Json(new
                {
                    erros = new List<string>()
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
                var servicoFinanceiro = _servicoFinanceirDAL.ObterPorId(id);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new
                {
                    servicoFinanceiro
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return Json(new
                {
                    erros = new List<string>()
                    {
                        ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Editar(ServicoFinanceiro servicoFinanceiroRequest)
        {
            try
            {
                var erros = _servicoFinanceiroBLL.ValidarServicoFinanceiro(servicoFinanceiroRequest);

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new
                    {
                        erros
                    }, JsonRequestBehavior.AllowGet);
                }

                var servicoFinanceiro = _servicoFinanceirDAL.Editar(servicoFinanceiroRequest);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new
                {
                    servicoFinanceiro,
                    mensagem = "Serviço financeiro editado com sucesso"
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return Json(new
                {
                    erros = new List<string>()
                    {
                        ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Deletar(int id)
        {
            try
            {
                _servicoFinanceirDAL.Deletar(id);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new
                {
                    mensagem = "Excluído com sucesso"
                },JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return Json(new
                {
                    erros = new List<string>()
                    {
                        ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}