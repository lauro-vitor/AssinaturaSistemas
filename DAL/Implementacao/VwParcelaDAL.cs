using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class VwParcelaDAL : DAL<VwParcela>, IDAL<VwParcela>
    {
        public VwParcela Criar(VwParcela objeto)
        {
            throw new NotImplementedException();
        }

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public VwParcela Editar(VwParcela objeto)
        {
            throw new NotImplementedException();
        }

        public VwParcela ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<VwParcela> ObterVarios()
        {
            throw new NotImplementedException();
        }

        public List<VwParcela> ObterVarios(int idSistema)
        {
            string sql = $@"
                SELECT *
                FROM [dbo].[VwParcela]
                WHERE IdSistema = {idSistema}
                ORDER BY [DataVencimento], [Numero]
            ";

            return this.DALobterVarios(sql);
        }
    }
}
