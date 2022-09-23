using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class EstadoBLL
    {
        public List<string> ValidarEstado(int idEstado, string nomeEstado, int idPais, IEnumerable<Estado> estados)
        {
            IEnumerable<Estado> estadosAux;

            List<string> erros = new List<string>();

            if (string.IsNullOrEmpty(nomeEstado))
                erros.Add("Nome Estado é obrigatório!");
            

            if (idPais <= 0)
                erros.Add("País é obrigatório!");
            

            if (idEstado > 0)
                estadosAux = estados.Where(e => e.IdEstado != idEstado);
            else
                estadosAux = estados;
            

            if (estadosAux.Any(e => e.IdPais == idPais && e.NomeEstado.ToLower().Equals(nomeEstado.ToLower())))
                erros.Add("Já existe esse estado para esse país!");
            

            return erros;
        }
    }
}
