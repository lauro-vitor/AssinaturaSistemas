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
    public class ContatoDAL : IContatoDAL
    {
        private readonly SqlConnection _sqlConnection;
        public ContatoDAL()
        {
            _sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AssinaturaSistemaSqlServer"].ConnectionString);
        }
        public Contato Editar(int idContato, int idCliente, string nomeCompleto, string email, string celular, string telefone, string senha)
        {
            _sqlConnection.Open();
            var comando = _sqlConnection.CreateCommand();
            var transaction = _sqlConnection.BeginTransaction();


            var contato = new Contato()
            {
                IdContato = idContato,
                IdCliente = idCliente,
                NomeCompleto = nomeCompleto.Trim(),
                Email = email.Trim(),
                Celular = celular.Trim(),
                Telefone = telefone?.Trim(),
                Senha = senha?.Trim()
            };

            string sql = $@"
                UPDATE [Contato] SET 
                    [NomeCompleto] = '{contato.NomeCompleto}',
                    [Email] = '{contato.Email}',
                    [Celular] = '{contato.Celular}',
                    [Telefone] = '{contato.Telefone}',
                    Senha = '{contato.Senha}'
                WHERE [IdContato] = {contato.IdContato}
            ";

            try
            {
                comando.CommandText = sql;
                comando.Transaction = transaction;
                comando.ExecuteNonQuery();
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

            return contato;
        }

        public Contato Criar(int idCliente, string nomeCompleto, string email, string celular, string telefone, string senha)
        {
            _sqlConnection.Open();
            var comando = _sqlConnection.CreateCommand();
            var transaction = _sqlConnection.BeginTransaction();

            var contato = new Contato()
            {
                IdCliente = idCliente,
                NomeCompleto = nomeCompleto.Trim(),
                Email = email.Trim(),
                Celular = celular.Trim(),
                Telefone = telefone?.Trim(),
                Senha = senha?.Trim()
            };

            string sql = $@"
                INSERT INTO [Contato] ([IdCliente],
                    [NomeCompleto], 
                    [Email],
                    [Celular],
                    [Telefone],
                    [Senha])
                VALUES ({contato.IdCliente},
                        '{contato.NomeCompleto}',
                        '{contato.Email}',
                        '{contato.Celular}',
                        '{contato.Telefone}',
                        '{contato.Senha}'); 

                SELECT @@IDENTITY AS [Lasted-Inserted Identity Value];";

            try
            {
                comando.CommandText = sql;
                comando.Transaction = transaction;
                var idInseridoRetorno = comando.ExecuteScalar();
                contato.IdContato = int.Parse(idInseridoRetorno.ToString());
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

            return contato;
        }

        public void Deletar(int idContato)
        {
            _sqlConnection.Open();

            SqlCommand comando = _sqlConnection.CreateCommand();

            var transcao = _sqlConnection.BeginTransaction();

            try
            {
                string sql = $@"DELETE FROM [Contato]
                                WHERE [IdContato] = {idContato}";

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

        public Contato ObterContatoPorId(int idContato)
        {
            _sqlConnection.Open();

            string sql = $"SELECT * FROM Contato WHERE [IdContato] = {idContato}";

            var contato = _sqlConnection.Query<Contato>(sql).FirstOrDefault();

            _sqlConnection.Close();

            return contato;
        }

        public List<Contato> ObterVarios(Func<Contato, bool> filtro)
        {
            _sqlConnection.Open();

            string sql = "SELECT * FROM Contato";

            var contatosAux = _sqlConnection.Query<Contato>(sql);


            var contatos = contatosAux
                .Where(filtro)
                .ToList();

            _sqlConnection.Close();

            return contatos;
        }
    }
}
