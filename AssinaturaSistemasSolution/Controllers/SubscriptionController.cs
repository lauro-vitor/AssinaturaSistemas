using AssinaturaSistemasSolution.Models;
using AssinaturaSistemasSolution.Service;
using DAL.Implementacao;
using Entidades;
using Entidades.enums;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AssinaturaSistemasSolution.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly EstadoDAL _estadoDAL;
        private readonly ServicoFinanceiroDAL _servicoFinanceiroDAL;
        private readonly StripeService _stripeService;
        private readonly TipoSistemaDAL _tipoSistemaDAL;
        private readonly AssinaturaDAL _assinaturaDAL;

        public SubscriptionController()
        {
            _estadoDAL = new EstadoDAL();
            _servicoFinanceiroDAL = new ServicoFinanceiroDAL();
            _stripeService = new StripeService();
            _tipoSistemaDAL = new TipoSistemaDAL();
            _assinaturaDAL = new AssinaturaDAL();
        }


        [HttpPost]
        public JsonResult Subscribe(SubscriptionModel subscriptionModel)
        {
            try
            {
                var erros = _stripeService.ValidateCreateSubscription(subscriptionModel);

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    return Json(new
                    {
                        erros
                    }, JsonRequestBehavior.AllowGet);
                }

                //criar cliente no stripe
                string customerId = _stripeService.CreateCustomer(subscriptionModel.Email);

                //cria a assinatura no stripe
                var subscription = _stripeService.CreateSubscription(customerId, subscriptionModel.PriceId);



                return Json(new
                {
                    subscritpionId = subscription.Id,

                    paymentIntentClientSecret = subscription.LatestInvoice.PaymentIntent.ClientSecret,

                    publishableKey = System.Configuration.ConfigurationManager.AppSettings["public_key"]

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
        public JsonResult ConfirmPaymentStripe(ConfirmPaymentModel confirmPaymentModel)
        {
            var erros = _stripeService.ValidateConfirmPaymentModel(confirmPaymentModel);

            try
            {
               
                if(erros.Count > 0)
                {
                    throw new Exception("");
                }

                var estado = _estadoDAL.Obter().FirstOrDefault(e => e.Padrao);

                if (estado == null)
                    throw new Exception("Estado não contrado");

                var servicoFinacieroContratado = _servicoFinanceiroDAL
                    .ObterVarios()
                    .FirstOrDefault(sf => confirmPaymentModel.PriceId.Equals(sf.StripePriceId) && sf.IdTipoSistema.HasValue);

                if (servicoFinacieroContratado == null)
                    throw new Exception("Serviço financeiro não encontrado");

                var tipoSistema = _tipoSistemaDAL
                    .ObterVarios()
                    .FirstOrDefault(ts => ts.IdTipoSistema == servicoFinacieroContratado.IdTipoSistema.Value);



                Cliente cliente = new Cliente()
                {
                    Ativo = true,
                    CodigoPostal = "",
                    DataCadastro = DateTime.Now,
                    Endereco = "",
                    IdEstado = estado.IdEstado,
                    IdPais = estado.IdPais,
                    NomeEmpresa = confirmPaymentModel.Company,
                    Observacao = "Cliente criado por meio de pagamento stripe",
                    UltimaAtualizacao = DateTime.Now
                };


                Contato contato = new Contato()
                {
                    Email = confirmPaymentModel.Email,
                    Telefone = confirmPaymentModel.Phone,
                    NomeCompleto = confirmPaymentModel.FullName,
                    Senha = "123456",
                    Celular = "",
                };

                string descricaoNomeCliente = confirmPaymentModel.Company.Replace(" ", "").ToLower().Trim();

                string descricaoTipoSistema = tipoSistema.Descricao.ToLower().Trim();


                Sistema sistema = new Sistema()
                {
                    Ativo = true,
                    BancoDeDados = $"bd_{descricaoTipoSistema}_{descricaoNomeCliente}",
                    DataCancelamento = null,
                    DataInicio = DateTime.Now,
                    Dominio = "",
                    DominioProvisorio = $"www.{descricaoNomeCliente}-test.com",
                    Pasta = $"/{descricaoTipoSistema}/{descricaoNomeCliente}",
                    IdTipoSistema = tipoSistema.IdTipoSistema,
                };



                Parcela parcela = new Parcela()
                {
                    Acrescimo = 0,
                    DataCancelamento = null,
                    DataGeracao = DateTime.Now,
                    DataVencimento = DateTime.Now,
                    Desconto = 0,
                    IdServicoFinanceiro = servicoFinacieroContratado.IdServicoFinanceiro,
                    IdStatusParcela = (int)EnumStatusParcela.Pago,
                    Numero = 1,
                    Observacao = "Parcela Stripe",
                    Valor = servicoFinacieroContratado.ValorCobranca
                };


                PagamentoParcela pagamentoParcela = new PagamentoParcela()
                {
                    DataPagamento = DateTime.Now,
                    ValorCartaoCredito = confirmPaymentModel.PaymentIntent.Amount,
                    ValorCartaoDebito = 0,
                    ValorDepositoBancario = 0,
                    StripePaymentIntentId = confirmPaymentModel.PaymentIntent.Id
                };

                _assinaturaDAL.Criar(cliente, contato, sistema, parcela, pagamentoParcela);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new
                {
                    mensagem = "Ok"
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //faz o estorno
                _stripeService.Refund(confirmPaymentModel.PaymentIntent);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (!string.IsNullOrEmpty(ex.Message))
                    erros.Add(ex.Message);

                return Json(new
                {
                   erros
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult ObtemServicosFinanceiroStripe()
        {
            try
            {
                var servicosFinanceiro = _servicoFinanceiroDAL
                    .ObterVarios()
                    .Where(sf => !string.IsNullOrEmpty(sf.StripePriceId))
                    .OrderBy(sf => sf.StripeOrdem);

                HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new
                {
                    servicosFinanceiro
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
    }
}