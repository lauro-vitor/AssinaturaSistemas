using Administrativo.Models.ClienteModel;
using BLL;
using DAL.Interfaces;
using DAL.Servico;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Util;

namespace Administrativo.Controllers
{
  
    public class ClienteController : Controller
    {
     
        private readonly IClienteDAL _clienteDAL;
        private readonly IPaisDAL _paisDAL;
        private readonly IEstadoDAL _estadoDAL;
        public ClienteController()
        {
            _clienteDAL = new ClienteDAL();
            _paisDAL = new PaisDAL();
            _estadoDAL = new EstadoDAL();
        }
        // GET: Cliente
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Manter(string idCliente)
        {

            return View();
        }

        [HttpPost]
        public JsonResult SalvarCliente(Cliente cliente)
        {
            try
            {
                ClienteBLL clienteBLL = new ClienteBLL();

                var erros = clienteBLL.ValidarCliente(cliente.NomeEmpresa, cliente.IdPais, cliente.IdEstado, cliente.CodigoPostal);

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        mensagens = erros
                    });
                }
                else
                {
                    HttpContext.Response.StatusCode = 200;

                    string mensagem = "";

                    if (cliente.IdCliente == 0)
                    {
                        _clienteDAL.Criar(cliente);
                        mensagem = "Cliente registrado com sucesso!";
                    }
                    else
                    {
                        _clienteDAL.Alterar(cliente);
                        mensagem = "Cliente alterado com sucesso!";
                    }


                    return Json(new
                    {
                        cliente,
                        mensagem
                    });
                }
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;

                return Json(new
                {
                    mensagem = ex.Message
                });
            }
        }

        [HttpGet]
        public ActionResult ObterClienteViewModel(string idCliente)
        {
            try
            {
                HttpContext.Response.StatusCode = 200;

                if (!string.IsNullOrEmpty(idCliente))
                {
                    int idClienteDescriptografado = 0;


                    idClienteDescriptografado = int.Parse(CriptografiaUtil.Decrypt(idCliente));

                    var clienteAux = _clienteDAL.ObterPorId(idClienteDescriptografado);

                    var paises = _paisDAL.Obter()
                    .OrderBy(p => p.NomePais)
                    .Select(p => new SelectListItem
                    {
                        Text = p.NomePais,
                        Value = p.IdPais.ToString()
                    });

                    var estados = _estadoDAL.Obter()
                        .Where(e => e.IdPais == clienteAux.IdPais)
                        .OrderBy(e => e.NomeEstado)
                        .Select(e => new SelectListItem()
                        {
                            Text = e.NomeEstado,
                            Value = e.IdEstado.ToString()
                        });

                    string DataCadastro = clienteAux.DataCadastro.HasValue ? clienteAux.DataCadastro.Value.ToString("yyyy-MM-dd") : "";

                    return Json(new
                    {
                        cliente = new
                        {
                            clienteAux.IdCliente,
                            clienteAux.NomeEmpresa,
                            clienteAux.Ativo,
                            clienteAux.IdPais,
                            clienteAux.IdEstado,
                            clienteAux.CodigoPostal,
                            clienteAux.Endereco,
                            clienteAux.Observacao,
                            DataCadastro
                        },
                        paises,
                        estados
                    }, JsonRequestBehavior.AllowGet); ;

                }
                else
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        mensagem = "Informe o Id do Cliente"
                    }, JsonRequestBehavior.AllowGet);
                }
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

        [HttpGet]
        public JsonResult ObterListaDeClientesViewModel(
            string nomeEmpresa, int idPais, int idEstado, string codigoPostal,
            string endereco, string dataCadastroInicial, string dataCadastroFinal,
            bool? ativo, int pageNumber, int pageSize)
        {
            try
            {
                HttpContext.Response.StatusCode = 200;

                var clientesDataSource = _clienteDAL.ObterVwListaClientes(
                    nomeEmpresa,
                    idPais,
                    idEstado,
                    codigoPostal,
                    endereco,
                    dataCadastroInicial,
                    dataCadastroFinal,
                    ativo
                );

                int total = clientesDataSource.Count();


                var clientes = clientesDataSource
                      .Skip((pageNumber - 1) * pageSize)
                      .Take(pageSize)
                      .Select(c =>
                  {
                      string DataCadastro = c.DataCadastro.HasValue ? c.DataCadastro.Value.ToString("dd/MM/yyyy") : "-";
                      string UltimaAtualizacao = c.UltimaAtualizacao.HasValue ? c.UltimaAtualizacao.Value.ToString("dd/MM/yyyy") : "-";
                      string IdCliente = CriptografiaUtil.Encrypt(c.IdCliente.ToString());

                      return new
                      {
                          IdCliente,
                          c.NomeEmpresa,
                          c.NomePais,
                          c.NomeEstado,
                          c.CodigoPostal,
                          c.Endereco,
                          c.Ativo,
                          DataCadastro,
                          UltimaAtualizacao
                      };
                  });

                return Json(new
                {
                    total,
                    clientes
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

        [HttpPost]
        public JsonResult DeletarCliente(string idCliente)
        {
            try
            {
                int idClienteDescriptografado = int.Parse(CriptografiaUtil.Decrypt(idCliente));

                _clienteDAL.Deletar(idClienteDescriptografado);

                HttpContext.Response.StatusCode = 200;

                return Json(new { });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    mensagem = ex.Message
                });
            }
           
        }

    }
}