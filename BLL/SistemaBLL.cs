using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SistemaBLL
    {

        public List<string> ValidarSistema(Sistema sistema, List<Sistema> sistemas)
        {
            var erros = new List<string>();

            if (sistema.IdCliente == 0)
                erros.Add("Cliente é obrigatório");

            if (sistema.IdTipoSistema == 0)
                erros.Add("Tipo sistema é obrigatório");

            if (sistema.DataInicio == null || sistema.DataInicio == new DateTime())
                erros.Add("Data Início é obrigatório");

            if (string.IsNullOrEmpty(sistema.Dominio) && string.IsNullOrEmpty(sistema.DominioProvisorio))
                erros.Add("Preencha Domínio ou Domínio provisório");

            if (sistemas.Count > 0)
            {
                Func<Sistema, bool> filtro = s => true;

                if (sistema.IdSistema > 0)
                    filtro = s => s.IdSistema != sistema.IdSistema;

                var sistemasAux = sistemas.Where(filtro);

                if (!string.IsNullOrEmpty(sistema.Dominio) &&
                    sistemasAux.Any(s => s.Dominio.ToLower().Equals(sistema.Dominio.ToLower().Trim())))
                {
                    erros.Add("Domínio existente escolha outro");
                }


                if (!string.IsNullOrEmpty(sistema.DominioProvisorio) &&
                     sistemasAux.Any(s => s.DominioProvisorio.ToLower().Equals(sistema.DominioProvisorio.ToLower().Trim())))
                {
                    erros.Add("Domínio provisório existente escolha outro");
                }
            }

            return erros;

        }
    }
}
