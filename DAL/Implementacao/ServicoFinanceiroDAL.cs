using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class ServicoFinanceiroDAL : DAL<ServicoFinanceiro>, IDAL<ServicoFinanceiro>
    {
        public ServicoFinanceiro Criar(ServicoFinanceiro servicoFinanceiro)
        {
            string sql = $@"INSERT INTO [dbo].[ServicoFinanceiro]
                               ([IdContaBancaria]
                               ,[IdPeriodoCobranca]
                               ,[DescricaoServico]
                               ,[DiaVencimento]
                               ,[ValorCobranca]
                               ,[QuantidadeParcelas])
                         VALUES
                               ({servicoFinanceiro.IdContaBancaria}
                               ,{servicoFinanceiro.IdPeriodoCobranca}
                               ,'{servicoFinanceiro.DescricaoServico}'
                               ,{servicoFinanceiro.DiaVencimento}
                               ,{servicoFinanceiro.ValorCobranca}
                               ,{servicoFinanceiro.QuantidadeParcelas})";

            int idRetorno = this.DALcriar(sql);

            servicoFinanceiro.IdServicoFinanceiro = idRetorno;

            return servicoFinanceiro;
        }

        public void Deletar(int id)
        {
            string sql = $@"
                DELETE FROM [dbo].[ServicoFinanceiro]
                WHERE [IdServicoFinanceiro] = {id}
            ";

            this.DALdeletar(sql);
        }

        public ServicoFinanceiro Editar(ServicoFinanceiro servicoFinanceiro)
        {
            string sql = $@"
UPDATE [dbo].[ServicoFinanceiro]
   SET [IdContaBancaria] = {servicoFinanceiro.IdContaBancaria}
      ,[IdPeriodoCobranca] = {servicoFinanceiro.IdPeriodoCobranca}
      ,[DescricaoServico] = '{servicoFinanceiro.DescricaoServico}'
      ,[DiaVencimento] = {servicoFinanceiro.DiaVencimento}
      ,[ValorCobranca] =  {servicoFinanceiro.ValorCobranca}
      ,[QuantidadeParcelas] = {servicoFinanceiro.QuantidadeParcelas}
 WHERE [IdServicoFinanceiro] = {servicoFinanceiro.IdServicoFinanceiro}
";
            this.DALeditar(sql);

            return servicoFinanceiro;
        }

        public ServicoFinanceiro ObterPorId(int id)
        {
            string sql = $@"
        SELECT [IdServicoFinanceiro]
              ,[IdContaBancaria]
              ,[IdPeriodoCobranca]
              ,[DescricaoServico]
              ,[DiaVencimento]
              ,[ValorCobranca]
              ,[QuantidadeParcelas]
        FROM [dbo].[ServicoFinanceiro]
        WHERE [IdServicoFinanceiro] = {id}
";
            return this.DALobterPorId(sql);
        }

        public List<ServicoFinanceiro> ObterVarios()
        {
            string sql = @"
SELECT [IdServicoFinanceiro]
      ,[IdContaBancaria]
      ,[IdPeriodoCobranca]
      ,[DescricaoServico]
      ,[DiaVencimento]
      ,[ValorCobranca]
      ,[QuantidadeParcelas]
      ,[StripePriceId]
      ,[StripeOrdem]
      ,[IdTipoSistema]
  FROM [dbo].[ServicoFinanceiro]
";
            return this.DALobterVarios(sql);
        }
    }
}
