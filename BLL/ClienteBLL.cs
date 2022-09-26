
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class ClienteBLL
    {
        public List<string> ValidarCliente(string nomeEmpresa, int idPais, int idEstado, string codigoPostal)
        {

            var erros = new List<string>();

            if (string.IsNullOrEmpty(nomeEmpresa))
                erros.Add("Nome empresa é obrigatório!");

            if (idPais == 0)
                erros.Add("País é obrigatório !");

            if (idEstado == 0)
                erros.Add("Estado é obrigatório!");

            if (!string.IsNullOrEmpty(codigoPostal))
            {
                var regex = new Regex("^[0-9]+$");

                if (!regex.IsMatch(codigoPostal))
                {
                    erros.Add("Código postal deve ter somente números");
                }
            }

            return erros;
        }
    }
}
