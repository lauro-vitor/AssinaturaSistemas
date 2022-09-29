using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class UsuarioBLL
    {
        public List<string> ValidarUsuario(Usuario usuario, List<Usuario> usuarios)
        {
            var emailBLL = new EmailBLL();
            int idUsuario = usuario.IdUsuario;
            string nomeCompleto = usuario.NomeCompleto;
            string email = usuario.Email;
            string senha = usuario.Senha;

            var erros = new List<string>();

            if (string.IsNullOrEmpty(nomeCompleto))
                erros.Add("Nome completo é obrigatório");

            if (string.IsNullOrEmpty(email))
                erros.Add("E-mail é obrigatório");

            if (string.IsNullOrEmpty(senha))
                erros.Add("Senha é obrigatório");

            if (!string.IsNullOrEmpty(email) && !emailBLL.ValidarEmail(email))
                erros.Add("E-mail inválido");

            if (!string.IsNullOrEmpty(senha) && senha.Count() < 6)
                erros.Add("Senha deve conter no mínimo 6 caractéres");

            var usuariosAux = usuarios.Where(u => idUsuario == 0 || u.IdUsuario != idUsuario);

            if (!string.IsNullOrEmpty(email) && usuariosAux.Any(u => u.Email.ToLower().Equals(email.ToLower().Trim())))
                erros.Add("E-mail registrado, use outro e-mail");

            return erros;
        }
    }
}
