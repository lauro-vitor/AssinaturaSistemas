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
    public class UsuarioController : Controller
    {
        private readonly UsuarioDAL _usuarioDAL;
        public UsuarioController()
        {
          
            _usuarioDAL = new UsuarioDAL();
        }
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObterVarios(string nomeCompleto, string email)
        {
            try
            {
                int total = 0;

                var usuarios = _usuarioDAL.ObterVarios()
                    .Where(u => !u.NomeCompleto.Equals("system") &&
                      (string.IsNullOrEmpty(nomeCompleto) || u.NomeCompleto.ToLower().Contains(nomeCompleto.ToLower())) &&
                      (string.IsNullOrEmpty(email) || u.Email.ToLower().Contains(email.ToLower())));

                total = usuarios.Count();

                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    total,
                    usuarios
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
        public JsonResult ObterUsuarioPorId(int idUsuario)
        {
            try
            {
                var usuario = _usuarioDAL.ObterPorId(idUsuario);

                HttpContext.Response.StatusCode = 200;
                return Json(new
                {
                    usuario
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

        [HttpPost]
        public JsonResult Criar(Usuario usuario)
        {
            try
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();

                var usuarios = _usuarioDAL.ObterVarios();

                var erros = usuarioBLL.ValidarUsuario(usuario, usuarios);

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        erros
                    });
                }

                _usuarioDAL.Criar(usuario);

                HttpContext.Response.StatusCode = 201;

                return Json(new
                {
                    usuario,
                    mensagemSucesso = "Usuário criado com sucesso!"
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
        public JsonResult Editar(Usuario usuario)
        {
            try
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();

                var usuarios = _usuarioDAL.ObterVarios();

                var erros = usuarioBLL.ValidarUsuario(usuario, usuarios);

                if (usuario.IdUsuario <= 0)
                {
                    erros.Add("IdUsuario é obrigatório");
                }

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        erros
                    });
                }

                _usuarioDAL.Editar(usuario);

                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    usuario,
                    mensagemSucesso = "Usuário editado com sucesso!"
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
        public JsonResult Deletar(int idUsuario)
        {
            try
            {
                var erros = new List<string>();

                if (idUsuario <= 0)
                {
                    erros.Add("IdUsuario é obrigatório");
                }

                if (erros.Count > 0)
                {
                    HttpContext.Response.StatusCode = 400;

                    return Json(new
                    {
                        erros
                    });
                }

                _usuarioDAL.Deletar(idUsuario);

                HttpContext.Response.StatusCode = 200;

                return Json(new
                {
                    mensagemSucesso = "Usuário excluído com sucesso!"
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