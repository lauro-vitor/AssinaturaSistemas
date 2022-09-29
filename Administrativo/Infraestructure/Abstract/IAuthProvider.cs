using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Administrativo.Infraestructure.Abstract
{
    public interface IAuthProvider
    {
        Usuario ObterUsuarioAtual(HttpContextBase contextBase);
        Usuario Authenticate(string username, string password);
        void SingOut();
    }
}
