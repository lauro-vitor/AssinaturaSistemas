using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Transactions;

namespace DAL.Servico
{
    public class PaisDAL : IPaisDAL
    {
        private readonly SqlConnection _conexao;
        public PaisDAL()
        {
            _conexao = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AssinaturaSistemaSqlServer"].ConnectionString);
        }
        public void Excluir(int idPais)
        {
            var sqlCommand = _conexao.CreateCommand();

            _conexao.Open();

            var transaction = _conexao.BeginTransaction();

            sqlCommand.CommandText = $@"DELETE FROM Pais
                                        WHERE IdPais = {idPais}";

            try
            {
                sqlCommand.Transaction = transaction;

                sqlCommand.ExecuteNonQuery();

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
                _conexao.Close();

            }
        }

        public List<Pais> Obter()
        {
            try
            {
                _conexao.Open();

                StringBuilder query = new StringBuilder(" SELECT * FROM Pais WHERE 1 = 1 ");

              
                IEnumerable<Pais> paises = _conexao.Query<Pais>(query.ToString());

                return paises.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conexao.Close();
            }
        }

        public Pais ObterPorId(int idPais)
        {
            try
            {
                _conexao.Open();

                string query = $@"
                    SELECT * 
                    FROM Pais WHERE IdPais = {idPais}";

                Pais pais = _conexao.Query<Pais>(query.ToString()).FirstOrDefault();

                return pais;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _conexao.Close();
            }
        }

        public Pais Salvar(int? idPais, string nomePais)
        {

            Pais pais = new Pais();

            SqlCommand query = _conexao.CreateCommand();

            if (idPais.HasValue)
            {
                query.CommandText = $@"UPDATE [Pais]
                             SET NomePais = '{nomePais}'
                             WHERE IdPais = {idPais.Value} ";
            }
            else
            {
                query.CommandText = $@"INSERT INTO [Pais]([NomePais])
                                       VALUES ('{nomePais}');

                                      SELECT @@IDENTITY AS [Last-Inserted Identity Value];";
            }


            _conexao.Open();

            var transaction = _conexao.BeginTransaction();

            try
            {
                query.Transaction = transaction;

                var id = query.ExecuteScalar();

                if (idPais.HasValue)
                {
                    pais.IdPais = idPais.Value;
                }
                else
                {
                    pais.IdPais = int.Parse(id.ToString());
                }

                pais.NomePais = nomePais;

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
                _conexao.Close();
            }
            return pais;
        }
    }
}
