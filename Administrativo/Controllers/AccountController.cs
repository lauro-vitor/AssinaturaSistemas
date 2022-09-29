using Administrativo.Infraestructure.Abstract;
using Administrativo.Infraestructure.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Controllers
{

    public class AccountController : Controller
    {
        private readonly IAuthProvider authProvider;
        public AccountController()
        {
            authProvider = new FormsAuthProvider();
        }
        [AllowAnonymous]
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(FormCollection formCollection)
        {
            try
            {
                ViewBag.MensagemErro = "";

                string username = formCollection["username"];

                string password = formCollection["password"];

                var usuario = authProvider.Authenticate(username, password);

                if (usuario != null)
                {
                    return Redirect("~/Home/Index");
                }
                else
                {
                    ViewBag.MensagemErro = "Usuário ou senha incorreto";
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
            }

            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult LogOut()
        {
            authProvider.SingOut();

            return Redirect("~/Account/Login");
        }

    }
}