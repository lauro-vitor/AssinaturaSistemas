using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PaisBLL
    {
        public List<string> ValidarPais(int idPais, string nomePais, IEnumerable<Pais> paises)
        {
            IEnumerable<Pais> paisesAux;

            var mensagens = new List<string>();

            if (string.IsNullOrEmpty(nomePais))
                mensagens.Add("Nome do País é obrigatório!");


            if (idPais > 0)
                paisesAux = paises.Where(p => p.IdPais != idPais);
            else
                paisesAux = paises;

            if (paisesAux.Any(p => p.NomePais.ToLower().Equals(nomePais.ToLower())))
                mensagens.Add("País existe!");

            return mensagens;
        }
    }
}
