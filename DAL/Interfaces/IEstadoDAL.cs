using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IEstadoDAL
    {
        List<Estado> Obter();
        Estado ObterPorId(int idEstado);

        Estado Criar(int idPais, string nomeEstado);

        Estado Alterar(int idEstado, int idPais, string nomeEstado);

        void Excluir(int idEstado);

    }
}
