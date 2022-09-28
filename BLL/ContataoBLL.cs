using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class ContataoBLL
    {
        public List<string> ValidarContato(int idContato, int idCliente, string nomeCompleto, string email, string celular, string telefone, string senha, List<Contato> contatos)
        {
            var erros = new List<string>();
            var emailBLL = new EmailBLL();
            var regexEhNumerico = new Regex("^[0-9]+$");

            if (idCliente < 0)
                erros.Add("Cliente é obrigatório");

            if (string.IsNullOrEmpty(nomeCompleto))
                erros.Add("Nome completo é obrigatório");

            if (string.IsNullOrEmpty(email))
                erros.Add("E-mail é obrigatório");
            else if (!emailBLL.ValidarEmail(email))
                erros.Add("E-mail inváldio");

            Func<Contato, bool> verificarContato = c => true;

            if (idContato > 0)
                verificarContato = c => c.IdContato != idContato;

            if (contatos.Where(verificarContato).Any(c => c.Email.ToLower().Equals(email.ToLower())))
                erros.Add("E-mail está sendo utilizado por outro contato");


            if (string.IsNullOrEmpty(celular))
                erros.Add("Celular é obrigatório");
            else if (!regexEhNumerico.IsMatch(celular))
                erros.Add("Celular deve conter somente números");

            if (!string.IsNullOrEmpty(telefone) && !regexEhNumerico.IsMatch(telefone))
                erros.Add("Telefone deve conter somente números");

            if (!string.IsNullOrEmpty(senha) && senha.Length < 6)
                erros.Add("Senha deve conter no mínimo 6 caractéres");



            return erros;
        }
    }
}
