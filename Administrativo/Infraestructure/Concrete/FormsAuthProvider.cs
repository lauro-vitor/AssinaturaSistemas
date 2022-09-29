using Administrativo.Infraestructure.Abstract;
using DAL.Implementacao;
using Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Administrativo.Infraestructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        public Usuario Authenticate(string username, string password)
        {
            UsuarioDAL usuarioDAL = new UsuarioDAL();

            var usuario = usuarioDAL
               .ObterVarios()
               .FirstOrDefault(u => u.Email.Equals(username) && u.Senha.Equals(password) && !u.Desabilitado);

            if (usuario != null)
            {
                FormsAuthentication.SetAuthCookie(JsonConvert.SerializeObject(usuario), false);
                return usuario;
            }

            return null;
        }

        public Usuario ObterUsuarioAtual(HttpContextBase contextBase)
        {
            HttpContext context = contextBase.ApplicationInstance.Context;

            if (context.User.Identity.IsAuthenticated)
            {
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(context.User.Identity.Name);

                return usuario;
            }

            return null;
        }

        public void SingOut()
        {
            FormsAuthentication.SignOut();
        }


    }
}