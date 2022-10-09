using Administrativo.Filters;
using BLL;
using Entidades;
using DAL.Implementacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Util;

namespace Administrativo.Controllers
{
    /// <summary>
    /// falta fazer o delete de parcela , alteração da parcela, reconstrução financeira
    /// </summary>
    [AccountFilter]
    [Authorize]
    public class FichaFinanceiraController : Controller
    {
        private readonly ServicoFinanceiroDAL _servicoFinanceiroDAL;
        private readonly ServicoFinanceiroBLL _servicoFinanceiroBLL;

        private readonly ParcelaDAL _parcelaDAL;
        private readonly ParcelaBLL _parcelaBLL;

        private readonly PagamentoParcelaDAL _pagamentoParcelaDAL;
        private readonly PagamentoParcelaBLL _pagamentoParcelaBLL;

        private readonly VwParcelaDAL _vwParcelaDAL;

        public FichaFinanceiraController()
        {
            _vwParcelaDAL = new VwParcelaDAL();
            _servicoFinanceiroBLL = new ServicoFinanceiroBLL();
            _servicoFinanceiroDAL = new ServicoFinanceiroDAL();
            _parcelaDAL = new ParcelaDAL();
            _parcelaBLL = new ParcelaBLL();
            _pagamentoParcelaDAL = new PagamentoParcelaDAL();
            _pagamentoParcelaBLL = new PagamentoParcelaBLL();
        }

        // GET: FichaFinanceira
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Manter(string id)
        {
            return View();
        }


        #region ___SERVICO FINANCEIRO___

        [HttpPost]
        public JsonResult ConcederServicoFinanceiro(string idSistema, int idServicoFinanceiro)
        {
            var erros = new List<string> { };

            if (string.IsNullOrEmpty(idSistema))
                erros.Add("Id Sistema é obrigatório");

            if (idServicoFinanceiro <= 0)
                erros.Add("Id serviço financeiro é obrigatório");

            if (erros.Count > 0)
            {

                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(new
                {
                    erros
                }, JsonRequestBehavior.AllowGet);

            }

            try
            {
                int idSistemaDecript = int.Parse(CriptografiaUtil.Decrypt(idSistema));

                var servicoFinanceiro = _servicoFinanceiroDAL.ObterPorId(idServicoFinanceiro);

                var vwParcelas = _vwParcelaDAL.ObterVarios(idSistemaDecript);

                if (_servicoFinanceiroBLL.ValidarConcessao(idServicoFinanceiro, idSistemaDecript, vwParcelas))
                {
                    var parcelas = _servicoFinanceiroBLL.ConcederServicoFinanceiro(servicoFinanceiro, idSistemaDecript);

                    _parcelaDAL.Criar(parcelas);

                    return Json(new
                    {
                        mensagem = "Serviço financeiro concedido com sucesso"
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    erros.Add("O sistema já possui este serviço financeiro");

                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return Json(new
                    {
                        erros
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                erros.Add(ex.Message);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return Json(new
                {
                    erros
                }, JsonRequestBehavior.AllowGet);

            }

        }


        #endregion

        #region ___PARCELAS___

        [HttpGet]
        public JsonResult ObterParcelas(string idSistema)
        {
            try
            {
                if (string.IsNullOrEmpty(idSistema))
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return Json(new
                    {
                        erros = new List<string>
                        {
                            "Id do Sistema é obrigatório"
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                int idSistemaDecript = int.Parse(CriptografiaUtil.Decrypt(idSistema));

                var parcelas = _vwParcelaDAL.ObterVarios(idSistemaDecript);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new
                {
                    parcelas
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

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
        public JsonResult ObterParcelaPorId(int id)
        {
            try
            {
                bool parcelaPaga = false;

                var parcela = _parcelaDAL.ObterPorId(id);

                var pagamentoParcela = _pagamentoParcelaDAL.ObterPagamentoParcelaPorIdParcela(id);

                parcelaPaga = pagamentoParcela != null;
              

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new
                {
                    parcela,
                    parcelaPaga
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(new
                {
                    erros = new List<string>
                    {
                        ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditarParcela(Parcela parcelaRequest)
        {
            try
            {
                var erros = _parcelaBLL.ValidarEditarParcela(parcelaRequest);

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return Json(new
                    {
                        erros
                    }, JsonRequestBehavior.AllowGet);
                }
                var parcela = _parcelaDAL.Editar(parcelaRequest);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new
                {
                    mensagem = "Parcela editada com sucesso"
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return Json(new
                {
                    erros = new List<string>
                    {
                        ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult CancelarParcela(int idParcela)
        {
            try
            {
                _parcelaDAL.CancelarParcela(idParcela);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new
                {
                    mensagem = "Parcela cancelada com sucesso"
                });
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(new
                {
                    erros = new List<string>
                    {
                        ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeletarParcela(int idParcela)
        {
            try
            {
                _parcelaDAL.Deletar(idParcela);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new
                {
                    mensagem = "Parcela excluída com sucesso"
                });
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return Json(new
                {
                    erros = new List<string>
                    {
                        ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


        #region ___PAGAMENTO PARCELA___
        [HttpGet]
        public JsonResult ObterPagamentoParcela(int idParcela)
        {
            try
            {
                
                var pagamentoParcela = _pagamentoParcelaDAL.ObterPagamentoParcelaPorIdParcela(idParcela);
                bool pagamentoStripe = false;

                if (pagamentoParcela == null)
                {
                    pagamentoParcela = new PagamentoParcela()
                    {
                        IdPagamentoParcela = 0,
                        IdParcela = idParcela,
                        DataPagamento = DateTime.Now,
                        DataPagamentoVM = DateTime.Now.ToString("yyyy-MM-dd"),
                        ValorCartaoCredito = 0,
                        ValorCartaoDebito = 0,
                        ValorDepositoBancario = 0,
                        StripePaymentIntentId = null
                    };
                }
                else
                {
                    pagamentoStripe = !string.IsNullOrEmpty(pagamentoParcela.StripePaymentIntentId);
                }

                return Json(new
                {
                    pagamentoStripe,
                    pagamentoParcela
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return Json(new
                {
                    erros = new List<string>
                    {
                       ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult PagarParcela(PagamentoParcela pagamentoParcelaRequest)
        {
            try
            {
                var parcela = _parcelaDAL.ObterPorId(pagamentoParcelaRequest.IdParcela);

                var pagamentoDaParcela = _pagamentoParcelaDAL.ObterPagamentoParcelaPorIdParcela(pagamentoParcelaRequest.IdParcela);

                var erros = _pagamentoParcelaBLL.ValidarPagamentoParcela(pagamentoParcelaRequest, pagamentoDaParcela);

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return Json(new
                    {
                        erros
                    }, JsonRequestBehavior.AllowGet);
                }

                var pagamentoParcela = new PagamentoParcela();
                var mensagem = "";

                if (pagamentoParcelaRequest.IdPagamentoParcela <= 0)
                {
                    pagamentoParcela = _pagamentoParcelaDAL.Criar(pagamentoParcelaRequest);
                    mensagem = "Pagamento realizado com sucesso";
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

                }
                else
                {
                    pagamentoParcela = _pagamentoParcelaDAL.Editar(pagamentoParcelaRequest);
                    mensagem = "Pagamento editado com sucesso";
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                }

                _parcelaDAL.PagarParcela(pagamentoParcelaRequest.IdParcela);

                return Json(new
                {
                    pagamentoParcela,
                    mensagem
                });


            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return Json(new
                {
                    erros = new List<string>
                    {
                       ex.Message
                    }
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}