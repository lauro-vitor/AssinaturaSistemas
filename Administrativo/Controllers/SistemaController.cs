using Administrativo.Filters;
using Administrativo.Models.SistemaModel;
using BLL;
using DAL.Implementacao;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Util;

namespace Administrativo.Controllers
{
  
    [AccountFilter]
    [Authorize]
    public class SistemaController : Controller
    {
        private readonly SistemaDAL _sistemaDAL;
        private readonly ClienteDAL _clienteDAL;
        private readonly TipoSistemaDAL _tipoSistemaDAL;
        private readonly VwSistemaDAL _vwSistemaDAL;
        public SistemaController()
        {
            _clienteDAL = new ClienteDAL();
            _sistemaDAL = new SistemaDAL();
            _tipoSistemaDAL = new TipoSistemaDAL();
            _vwSistemaDAL = new VwSistemaDAL();
        }
        // GET: Sistema

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manter(string id)
        {
            return View();
        }


        [HttpGet]
        public JsonResult ObterVarios(string idCliente, string idTipoSistema, string dominioProvisorio, string dominio,
           bool? ativo, string periodoInicialDataInicio, string periodoFinalDataInicio, string periodoInicialDataCancelamento,
           string periodoFinalDataCancelamento)
        {
            try
            {
                int idClienteFiltro = 0;

                int idTipoSistemaFiltro = 0;

                if (idCliente != "0")
                {
                    int.TryParse(CriptografiaUtil.Decrypt(idCliente), out idClienteFiltro);
                }

                if (idTipoSistema != "0")
                {
                    int.TryParse(CriptografiaUtil.Decrypt(idTipoSistema), out idTipoSistemaFiltro);
                }

                var clientes = _clienteDAL.ObterVarios();

                var tiposSistema = _tipoSistemaDAL.ObterVarios();

                var sistemas = _sistemaDAL.ObterVarios();

                var sistemasViewModel =
                    from s in sistemas
                    join c in clientes on s.IdCliente equals c.IdCliente
                    join ts in tiposSistema on s.IdTipoSistema equals ts.IdTipoSistema
                    where (1 == 1
                       && (string.IsNullOrEmpty(dominioProvisorio) || s.DominioProvisorio.ToLower().Contains(dominioProvisorio.ToLower().Trim()))
                       && (string.IsNullOrEmpty(dominio) || s.Dominio.ToLower().Contains(dominio.ToLower().Trim()))
                       && (idTipoSistemaFiltro == 0 || s.IdTipoSistema == idTipoSistemaFiltro)
                       && (idClienteFiltro == 0 || s.IdCliente == idClienteFiltro)
                       && (string.IsNullOrEmpty(periodoInicialDataInicio) || s.DataInicio >= Convert.ToDateTime(periodoInicialDataInicio))
                       && (string.IsNullOrEmpty(periodoFinalDataInicio) || s.DataInicio <= Convert.ToDateTime(periodoFinalDataInicio)
                       && (string.IsNullOrEmpty(periodoInicialDataCancelamento) || s.DataCancelamento >= Convert.ToDateTime(periodoInicialDataCancelamento)))
                       && (string.IsNullOrEmpty(periodoFinalDataCancelamento) || s.DataCancelamento <= Convert.ToDateTime(periodoFinalDataCancelamento))
                     )

                    select new SistemaViewModel
                    {
                        IdSistema = CriptografiaUtil.Encrypt(s.IdSistema.ToString()),
                        IdCliente = CriptografiaUtil.Encrypt(s.IdCliente.ToString()),
                        IdTipoSistema = CriptografiaUtil.Encrypt(s.IdTipoSistema.ToString()),
                        Ativo = s.Ativo,
                        BancoDeDados = s.BancoDeDados,
                        ClienteNomeEmpresa = c.NomeEmpresa,
                        DataCancelamento = (s.DataCancelamento.HasValue ? s.DataCancelamento.Value.ToString("dd/MM/yyy") : ""),
                        DataInicio = (s.DataInicio.ToString("dd/MM/yyyy")),
                        Dominio = s.Dominio,
                        DominioProvisorio = s.DominioProvisorio,
                        Pasta = s.Pasta,
                        TipoSistemaDescricao = ts.Descricao,
                    };

                int total = sistemasViewModel.Count();

                var clientesViewModel = clientes.Select(c => new SelectListItem
                {
                    Text = c.NomeEmpresa,
                    Value = CriptografiaUtil.Encrypt(c.IdCliente.ToString())
                });

                var tiposSistemaViewModel = tiposSistema.Select(ts => new SelectListItem
                {
                    Text = ts.Descricao,
                    Value = CriptografiaUtil.Encrypt(ts.IdTipoSistema.ToString())
                });


                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    total,
                    sistemas = sistemasViewModel,
                    tiposSistema = tiposSistemaViewModel,
                    clientes = clientesViewModel
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
        public JsonResult ObterPorId(string id)
        {
            try
            {
                int idDescriptografado = int.Parse(CriptografiaUtil.Decrypt(id));

                var sistema = _sistemaDAL.ObterPorId(idDescriptografado);

                if (sistema == null)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        erros = new List<string>
                        {
                            "Sistema não existe"
                        }
                    }, JsonRequestBehavior.AllowGet);
                }


                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    sistema
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;

                return Json(new
                {
                    erros = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ObterVwSistemaPorId(string id)
        {
            try
            {
                int idDescriptografado = int.Parse(CriptografiaUtil.Decrypt(id));

                var sistema = _vwSistemaDAL.ObterPorId(idDescriptografado);

                if (sistema == null)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        erros = new List<string>
                        {
                            "Sistema não existe"
                        }
                    }, JsonRequestBehavior.AllowGet);
                }


                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    sistema
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;

                return Json(new
                {
                    erros = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Criar(Sistema sistema)
        {
            var sistemaBLL = new SistemaBLL();
            try
            {
                var sistemas = _sistemaDAL.ObterVarios();

                var erros = sistemaBLL.ValidarSistema(sistema, sistemas);

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        erros,
                    });
                }

                var sistemaRetorno = _sistemaDAL.Criar(sistema);

                HttpContext.Response.StatusCode = 201;

                return Json(new
                {
                    mensagem = "Sistema criado com suceso",
                    sistema = sistemaRetorno,
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
        public JsonResult Editar(Sistema sistemaRequest)
        {
            try
            {
                var sistemaBLL = new SistemaBLL();

                var sistemas = _sistemaDAL.ObterVarios();

                var erros = sistemaBLL.ValidarSistema(sistemaRequest, sistemas);

                if (sistemaRequest.IdSistema <= 0)
                {
                    erros.Add("Id sistema é obrigatório");
                }

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        erros,
                    });
                }

                var sistema = _sistemaDAL.Editar(sistemaRequest);

                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    sistema,
                    mensagem = "Sistema editado com sucesso",
                });

            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = 500;

                return Json(new
                {
                    erros = new List<string>
                    {
                        ex.Message,
                    }
                });
            }
        }

        [HttpPost]
        public JsonResult Deletar(string id)
        {
            try
            {
                int idDescriptografado = int.Parse(CriptografiaUtil.Decrypt(id));

                _sistemaDAL.Deletar(idDescriptografado);

                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    mensagem = "Sistema excluído com sucesso"
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


        [HttpGet]
        public JsonResult ObterTiposSistemaDropDownList()
        {
            try
            {
                var tiposSistema = _tipoSistemaDAL
                    .ObterVarios()
                    .Select(ts => new SelectListItem
                    {
                        Value = ts.IdTipoSistema.ToString(),
                        Text = ts.Descricao
                    });

                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    tiposSistema
                }, JsonRequestBehavior.AllowGet);

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

                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}