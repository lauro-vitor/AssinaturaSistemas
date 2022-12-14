using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class SistemaDAL : DAL<Sistema>, IDAL<Sistema>, IDALTransacao<Sistema>
    {
        public Sistema Criar(Sistema sistema)
        {
            string sql = $@"
                INSERT INTO [dbo].[Sistema]
                       ([IdCliente]
                       ,[IdTipoSistema]
                       ,[DominioProvisorio]
                       ,[Dominio]
                       ,[Pasta]
                       ,[BancoDeDados]
                       ,[Ativo]
                       ,[DataInicio]
                       ,[DataCancelamento])
                 VALUES
                       ({sistema.IdCliente}
                       ,{sistema.IdTipoSistema}
                       ,'{sistema.DominioProvisorio}'
                       ,'{sistema.Dominio}'
                       ,'{sistema.Pasta}'
                       ,'{sistema.BancoDeDados}'
                       ,{(sistema.Ativo ? 1 : 0)}
                       ,'{sistema.DataInicio}'
                       ,{(sistema.DataCancelamento.HasValue ? $"'{sistema.DataCancelamento.Value}'" : "NULL")})
            ";

            int idSistema = this.DALcriar(sql);

            sistema.IdSistema = idSistema;

            return sistema;
        }

        public Sistema Criar(Sistema sistema, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {

            string sql = $@"
                INSERT INTO [dbo].[Sistema]
                       ([IdCliente]
                       ,[IdTipoSistema]
                       ,[DominioProvisorio]
                       ,[Dominio]
                       ,[Pasta]
                       ,[BancoDeDados]
                       ,[Ativo]
                       ,[DataInicio]
                       ,[DataCancelamento])
                 VALUES
                       ({sistema.IdCliente}
                       ,{sistema.IdTipoSistema}
                       ,'{sistema.DominioProvisorio}'
                       ,'{sistema.Dominio}'
                       ,'{sistema.Pasta}'
                       ,'{sistema.BancoDeDados}'
                       ,{(sistema.Ativo ? 1 : 0)}
                       ,'{sistema.DataInicio}'
                       ,{(sistema.DataCancelamento.HasValue ? $"'{sistema.DataCancelamento.Value}'" : "NULL")})
            ";

            int idSistema = this.DALcriarComTransacao(sql, sqlConnection, sqlTransaction);

            sistema.IdSistema = idSistema;

            return sistema;
        }

        public void Deletar(int id)
        {
            string sql = $@"DELETE FROM [dbo].[Sistema]
                          WHERE [IdSistema] = {id}";

            this.DALdeletar(sql);
        }

        public Sistema Editar(Sistema sistema)
        {
            string sql = $@"
            UPDATE [dbo].[Sistema]
               SET [IdCliente] = {sistema.IdCliente}
                  ,[IdTipoSistema] = {sistema.IdTipoSistema}
                  ,[DominioProvisorio] = '{sistema.DominioProvisorio}'
                  ,[Dominio] = '{sistema.Dominio}'
                  ,[Pasta] = '{sistema.Pasta}'
                  ,[BancoDeDados] = '{sistema.BancoDeDados}'
                  ,[Ativo] = {(sistema.Ativo ? 1 : 0)}
                  ,[DataInicio] = '{sistema.DataInicio}'
                  ,[DataCancelamento] = {(sistema.DataCancelamento.HasValue ? $"'{sistema.DataCancelamento.Value}'" : "NULL")}
             WHERE [IdSistema] = {sistema.IdSistema}
            ";

            this.DALeditar(sql);

            return sistema;
        }

        public Sistema ObterPorId(int id)
        {
            string sql = $@"SELECT [IdSistema]
                          ,[IdCliente]
                          ,[IdTipoSistema]
                          ,[DominioProvisorio]
                          ,[Dominio]
                          ,[Pasta]
                          ,[BancoDeDados]
                          ,[Ativo]
                          ,[DataInicio]
                          ,[DataCancelamento]
                      FROM [dbo].[Sistema]
                      WHERE [IdSistema] = {id} ";

            return this.DALobterPorId(sql);
        }

        public List<Sistema> ObterVarios()
        {
            string sql = @"SELECT [IdSistema]
                          ,[IdCliente]
                          ,[IdTipoSistema]
                          ,[DominioProvisorio]
                          ,[Dominio]
                          ,[Pasta]
                          ,[BancoDeDados]
                          ,[Ativo]
                          ,[DataInicio]
                          ,[DataCancelamento]
                      FROM [dbo].[Sistema] ";

            return this.DALobterVarios(sql);
        }
    }
}
