using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class VwSistemaDAL : DAL<VwSistema>, IDAL<VwSistema>
    {
        public VwSistema Criar(VwSistema objeto)
        {
            throw new NotImplementedException();
        }

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public VwSistema Editar(VwSistema objeto)
        {
            throw new NotImplementedException();
        }

        public VwSistema ObterPorId(int id)
        {
            string sql = $@"
                SELECT * FROM VwSistema
                WHERE IdSistema = {id}
            ";

            return this.ObterPorIdDAL(sql);
        }

        public List<VwSistema> ObterVarios()
        {
            throw new NotImplementedException();
        }
    }
}
