using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class PagamentoParcelaDAL : DAL<PagamentoParcela>, IDAL<PagamentoParcela>, IPagamentoParcelaDAL
    {
        public PagamentoParcela Criar(PagamentoParcela pagamentoParcela)
        {
            string sql = $@"
            SET DATEFORMAT YMD;

            INSERT INTO [dbo].[PagamentoParcela]
               ([IdParcela]
               ,[DataPagamento]
               ,[ValorDepositoBancario]
               ,[ValorCartaoCredito]
               ,[ValorCartaoDebito])
             VALUES
                   ({pagamentoParcela.IdParcela}
                   ,'{pagamentoParcela.DataPagamento}'
                   ,{pagamentoParcela.ValorDepositoBancario}
                   ,{pagamentoParcela.ValorCartaoCredito}
                   ,{pagamentoParcela.ValorCartaoDebito})
        ";

            pagamentoParcela.IdPagamentoParcela = this.CriarDAL(sql);
            return pagamentoParcela;
        }

        public void Deletar(int id)
        {
            throw new NotImplementedException();
        }

        public PagamentoParcela Editar(PagamentoParcela pagamentoParcela)
        {
            string sql = $@"
             SET DATEFORMAT YMD;

             UPDATE [dbo].[PagamentoParcela]
             SET [IdParcela] = {pagamentoParcela.IdParcela}
                  ,[DataPagamento] = '{pagamentoParcela.DataPagamento}'
                  ,[ValorDepositoBancario] = {pagamentoParcela.ValorDepositoBancario}
                  ,[ValorCartaoCredito] = {pagamentoParcela.ValorCartaoCredito}
                  ,[ValorCartaoDebito] = {pagamentoParcela.ValorCartaoDebito}
             WHERE [IdPagamentoParcela] = {pagamentoParcela.IdPagamentoParcela}
        ";

            this.EditarDAL(sql);

            return pagamentoParcela;
        }

        public PagamentoParcela ObterPagamentoParcelaPorIdParcela(int idParcela)
        {
            string sql = $@"SELECT [IdPagamentoParcela]
                          ,[IdParcela]
                          ,[DataPagamento]
                          ,[ValorDepositoBancario]
                          ,[ValorCartaoCredito]
                          ,[ValorCartaoDebito]
	                      ,[DataPagamentoVM] = FORMAT([dataPagamento],'yyyy-MM-dd')
                      FROM [dbo].[PagamentoParcela]
                      WHERE [IdParcela] = {idParcela}";

            return this.ObterPorIdDAL(sql);
        }

        public PagamentoParcela ObterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<PagamentoParcela> ObterVarios()
        {
            throw new NotImplementedException();
        }
    }
}
