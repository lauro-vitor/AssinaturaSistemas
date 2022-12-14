using DAL.Interfaces;
using Entidades;
using Entidades.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using System.Data.SqlClient;

namespace DAL.Implementacao
{
    public class ParcelaDAL : DAL<Parcela>, IDAL<Parcela>, IDALTransacao<Parcela>, IParcelaDAL
    {
        public void AbrirParcela(int idParcela)
        {
            string sql = $@"
                 UPDATE [dbo].[Parcela]
                    SET [IdStatusParcela] = {(int)EnumStatusParcela.Aberto}
                 WHERE IdParcela = {idParcela}
            ";

            this.DALeditar(sql);
        }

        public void CancelarParcela(int idParcela)
        {
            string sql = $@"
                SET DATEFORMAT DMY;

                 UPDATE [dbo].[Parcela]
                    SET [IdStatusParcela] = {(int)EnumStatusParcela.Cancelado}
                    ,[DataCancelamento] = '{DateTime.Now}'
                 WHERE IdParcela = {idParcela}
            ";

            this.DALeditar(sql);
        }

        public List<Parcela> Criar(List<Parcela> parcelas)
        {
            _sqlConnection.Open();

            var comando = _sqlConnection.CreateCommand();

            var transaction = _sqlConnection.BeginTransaction();

            try
            {
                comando.Transaction = transaction;

                foreach (var parcela in parcelas)
                {
                    this.Criar(parcela, _sqlConnection, transaction);
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            finally
            {
                transaction.Dispose();
                _sqlConnection.Close();
            }

            return parcelas;
        }

        public Parcela Criar(Parcela objeto)
        {
            throw new NotImplementedException();
        }

        public Parcela Criar(Parcela parcela, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            string sql = $@"
                    SET DATEFORMAT YMD;

                    INSERT INTO [dbo].[Parcela]
                       ([IdSistema]
                       ,[IdServicoFinanceiro]
                       ,[IdStatusParcela]
                       ,[Numero]
                       ,[DataGeracao]
                       ,[DataVencimento]
                       ,[DataCancelamento]
                       ,[Valor]
                       ,[Desconto]
                       ,[Acrescimo]
                       ,[Observacao])
                 VALUES
                     (  {parcela.IdSistema}
                       ,{parcela.IdServicoFinanceiro}
                       ,{parcela.IdStatusParcela}
                       ,{parcela.Numero}
                       ,'{parcela.DataGeracao}'
                       ,'{parcela.DataVencimento}'
                       ,{(parcela.DataCancelamento.HasValue ? $"'{parcela.DataCancelamento.Value}'" : "NULL")}
                       ,{parcela.Valor}
                       ,{parcela.Desconto}
                       ,{parcela.Acrescimo}
                       ,'{parcela.Observacao}' )

                  SELECT @@IDENTITY AS [Last-Inserted Identity Value];
                    ";

            parcela.IdParcela = this.DALcriarComTransacao(sql, sqlConnection, sqlTransaction);

            return parcela;
        }

        public void Deletar(int id)
        {
            string sql = $@"  
               DELETE FROM [dbo].[Parcela]
               WHERE IdParcela = {id}";

            this.DALdeletar(sql);
        }

        public Parcela Editar(Parcela parcela)
        {
            string sql = $@"
                SET DATEFORMAT YMD;
                UPDATE [dbo].[Parcela]
                   SET [DataVencimento] = '{parcela.DataVencimento}'
                      ,[Valor] = {parcela.Valor}
                      ,[Desconto] = {parcela.Desconto}
                      ,[Acrescimo] = {parcela.Acrescimo}
                      ,[Observacao] = '{parcela.Observacao}'
                 WHERE IdParcela = {parcela.IdParcela}";

            this.DALeditar(sql);

            return parcela;
        }

        public Parcela ObterPorId(int id)
        {
            string sql = $@"SELECT [IdParcela]
                          ,[IdSistema]
                          ,[IdServicoFinanceiro]
                          ,[IdStatusParcela]
                          ,[Numero]
                          ,[DataGeracao]
                          ,[DataVencimento]
                          ,[DataCancelamento]
                          ,[Valor]
                          ,[Desconto]
                          ,[Acrescimo]
                          ,[Observacao]
                      FROM [dbo].[Parcela]
                      WHERE IdParcela = {id} ";

            return this.DALobterPorId(sql);
        }

        public List<Parcela> ObterVarios()
        {
            throw new NotImplementedException();
        }

        public void PagarParcela(int idParcela)
        {
            string sql = $@"
                 UPDATE [dbo].[Parcela]
                    SET [IdStatusParcela] = {(int)EnumStatusParcela.Pago}
                 WHERE IdParcela = {idParcela}
            ";

            this.DALeditar(sql);
        }
    }
}
