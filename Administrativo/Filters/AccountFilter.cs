using Administrativo.Infraestructure.Abstract;
using Administrativo.Infraestructure.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Filters
{
    public class AccountFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IAuthProvider authProvider = new FormsAuthProvider();

            var usuario = authProvider.ObterUsuarioAtual(filterContext.HttpContext);

            filterContext.Controller.ViewBag.UsuarioEmail = usuario.Email;
             
            base.OnActionExecuting(filterContext);
        }
    }
}