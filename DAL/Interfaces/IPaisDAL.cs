using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IPaisDAL
    {
        List<Pais> Obter();
        Pais ObterPorId(int idPais);
        Pais Salvar(int? idPais, string nomePais);
        void Excluir(int idPais);
    }
}
