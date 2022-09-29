using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    /// <summary> 
    /// Interface responsavel por padronizar a nomeclatura de métodos que utilizam  o banco de dados
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDAL<T>
    {
        T Criar(T objeto);
        T Editar(T objeto);
        void Deletar(int id);
        T ObterPorId(int id);

        List<T> ObterVarios();
    }
}
