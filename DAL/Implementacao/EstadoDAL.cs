using DAL.Interfaces;
using Dapper;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class EstadoDAL : IEstadoDAL
    {
        private readonly SqlConnection _sqlConnection;
        public EstadoDAL()
        {
            _sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AssinaturaSistemaSqlServer"].ConnectionString);
        }

        public Estado Alterar(int idEstado, int idPais, string nomeEstado)
        {
            Estado estado = null;

            _sqlConnection.Open();

            SqlCommand comando = _sqlConnection.CreateCommand();

            var transcao = _sqlConnection.BeginTransaction();

            try
            {
                string sql = $@"UPDATE [Estado] 
                              SET 
	                            [IdPais] = {idPais},
	                            [NomeEstado]  =  '{nomeEstado}'
                             WHERE [IdEstado] = {idEstado}";

                comando.Transaction = transcao;

                comando.CommandText = sql;

                comando.ExecuteNonQuery();

                transcao.Commit();

                estado = new Estado
                {
                    IdEstado = idEstado,
                    IdPais = idPais,
                    NomeEstado = nomeEstado,
                };
            }
            catch (Exception ex)
            {
                transcao.Rollback();

                throw ex;
            }
            finally
            {
                transcao.Dispose();

                _sqlConnection.Close();
            }

            return estado;

        }

        public Estado Criar(int idPais, string nomeEstado)
        {
            Estado estado = null;

            _sqlConnection.Open();

            SqlCommand comando = _sqlConnection.CreateCommand();

            var transacao = _sqlConnection.BeginTransaction();

            try
            {
                string sql = $@" INSERT INTO  [Estado] ([IdPais],[NomeEstado])
                                 VALUES ({idPais},'{nomeEstado}');

                                SELECT @@IDENTITY AS [Last-Inserted Identity Value];";

                comando.Transaction = transacao;

                comando.CommandText = sql;

                var idInserido = comando.ExecuteScalar();

                int id = int.Parse(idInserido.ToString());

                estado = new Estado()
                {
                    IdEstado = id,
                    IdPais = idPais,
                    NomeEstado = nomeEstado
                };

                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao.Rollback();
                throw ex;
            }
            finally
            {
                transacao.Dispose();
                _sqlConnection.Close();
            }

            return estado;
        }

        public void Excluir(int idEstado)
        {
          
            _sqlConnection.Open();

            SqlCommand comando = _sqlConnection.CreateCommand();

            var transcao = _sqlConnection.BeginTransaction();

            try
            {
                string sql = $@"DELETE FROM [Estado] 
                                WHERE [IdEstado] = {idEstado}";

                comando.Transaction = transcao;

                comando.CommandText = sql;

                comando.ExecuteNonQuery();

                transcao.Commit();

            }
            catch (Exception ex)
            {
                transcao.Rollback();

                throw ex;
            }
            finally
            {
                transcao.Dispose();

                _sqlConnection.Close();
            }

        }

        public List<Estado> Obter()
        {
            string sql = @"
                         SELECT 
	                        E.IdEstado,
	                        E.IdPais,
	                        E.NomeEstado,
	                        P.NomePais
                        FROM Estado E
                        INNER JOIN Pais P ON E.IdPais = P.IdPais
                        WHERE  1 = 1
                        ORDER BY E.NomeEstado, P.NomePais";

          

            var estados = _sqlConnection.Query<Estado>(sql);

            _sqlConnection.Close();

            return estados.ToList();
        }

        public Estado ObterPorId(int idEstado)
        {
            string sql = $@"
                           SELECT 
	                            E.IdEstado,
	                            E.IdPais,
	                            E.NomeEstado,
	                            P.NomePais
                            FROM Estado E
                            INNER JOIN Pais P ON E.IdPais = P.IdPais
                            WHERE  1 = 1
                            AND E.IdEstado  = {idEstado} ";

            var estado = _sqlConnection.Query<Estado>(sql).FirstOrDefault();

            _sqlConnection.Close();

            return estado;
        }
    }
}
