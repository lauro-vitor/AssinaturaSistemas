using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class VwServicoFinanceiroDAL : DAL<VwServicoFinanceiro>, IDAL<VwServicoFinanceiro>
    {
        public VwServicoFinanceiro Criar(VwServicoFinanceiro objeto)
        {
            throw new NotImplementedException();
        }

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public VwServicoFinanceiro Editar(VwServicoFinanceiro objeto)
        {
            throw new NotImplementedException();
        }

        public VwServicoFinanceiro ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<VwServicoFinanceiro> ObterVarios()
        {
            string sql = "select * from VwServicoFinanceiro";

            return this.DALobterVarios(sql);
        }
    }
}
