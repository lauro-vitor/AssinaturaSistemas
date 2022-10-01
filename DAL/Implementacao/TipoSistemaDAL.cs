using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class TipoSistemaDAL : DAL<TipoSistema>, IDAL<TipoSistema>
    {
        public TipoSistema Criar(TipoSistema objeto)
        {
            throw new NotImplementedException();
        }

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public TipoSistema Editar(TipoSistema objeto)
        {
            throw new NotImplementedException();
        }

        public TipoSistema ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<TipoSistema> ObterVarios()
        {
            string sql = @" SELECT * FROM TipoSistema";

            return this.ObterVariosDAL(sql);
        }
    }
}
