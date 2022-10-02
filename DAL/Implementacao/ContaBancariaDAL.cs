using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class ContaBancariaDAL : DAL<ContaBancaria>, IDAL<ContaBancaria>
    {
        public ContaBancaria Criar(ContaBancaria contaBancaria)
        {
            string sql = $@"INSERT INTO [dbo].[ContaBancaria]
                               ([NumeroAgencia]
                               ,[NumeroConta]
                               ,[NumeroBanco]
                               ,[NomeBanco]
                               ,[Cnpj])
                         VALUES 
                                ('{contaBancaria.NumeroAgencia}'
                                ,'{contaBancaria.NumeroConta}'
                                ,{contaBancaria.NumeroBanco}
                                ,'{contaBancaria.NomeBanco}'
                                ,'{contaBancaria.Cnpj}')";

            contaBancaria.IdContaBancaria = this.CriarDAL(sql);

            return contaBancaria;
        }

        public void Deletar(int id)
        {
            string sql = $@"DELETE FROM ContaBancaria WHERE [IdContaBancaria] = {id}";

            this.DeletarDAL(sql);
        }

        public ContaBancaria Editar(ContaBancaria contaBancaria)
        {
            string sql = $@"UPDATE [dbo].[ContaBancaria]
                             SET [NumeroAgencia] = '{contaBancaria.NumeroAgencia}'
                                  ,[NumeroConta] = '{contaBancaria.NumeroConta}'
                                  ,[NumeroBanco] = {contaBancaria.NumeroBanco}
                                  ,[NomeBanco] = '{contaBancaria.NomeBanco}'
                                  ,[Cnpj] = '{contaBancaria.Cnpj}'
                             WHERE [IdContaBancaria] = {contaBancaria.IdContaBancaria}";

            this.EditarDAL(sql);

            return contaBancaria;
        }

        public ContaBancaria ObterPorId(int id)
        {
            string sql = $@"SELECT [IdContaBancaria]
                              ,[NumeroAgencia]
                              ,[NumeroConta]
                              ,[NumeroBanco]
                              ,[NomeBanco]
                              ,[Cnpj]
                          FROM [dbo].[ContaBancaria]
                          WHERE [IdContaBancaria] = {id}";

            return this.ObterPorIdDAL(sql);
        }

        public List<ContaBancaria> ObterVarios()
        {
            string sql = @"SELECT [IdContaBancaria]
                          ,[NumeroAgencia]
                          ,[NumeroConta]
                          ,[NumeroBanco]
                          ,[NomeBanco]
                          ,[Cnpj]
                      FROM [dbo].[ContaBancaria]";

            return this.ObterVariosDAL(sql);
        }
    }
}
