using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class PeriodoCobrancaDAL : DAL<PeriodoCobranca>, IDAL<PeriodoCobranca>
    {
        public PeriodoCobranca Criar(PeriodoCobranca objeto)
        {
            throw new NotImplementedException();
        }

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public PeriodoCobranca Editar(PeriodoCobranca objeto)
        {
            throw new NotImplementedException();
        }

        public PeriodoCobranca ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<PeriodoCobranca> ObterVarios()
        {
            string sql = @"SELECT 
	                        IdPeriodoCobranca, 
	                        Descricao
                        FROM PeriodoCobranca";

            return this.DALobterVarios(sql);
        }
    }
}
