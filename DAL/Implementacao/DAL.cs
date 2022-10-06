using DAL.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class DAL<T>
    {
        protected readonly SqlConnection _sqlConnection;

        public DAL()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            _sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AssinaturaSistemaSqlServer"].ConnectionString);
        }
        protected int CriarDAL(string comandoSql)
        {
            _sqlConnection.Open();
            var comando = _sqlConnection.CreateCommand();
            var transaction = _sqlConnection.BeginTransaction();
            int id = 0;
            try
            {
                comando.CommandText = comandoSql  + "; SELECT @@IDENTITY AS [Last-Inserted Identity Value];";
                comando.Transaction = transaction;
                var idInseridoRetorno = comando.ExecuteScalar();
                id = int.Parse(idInseridoRetorno.ToString());
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

            return id;
        }
        protected void EditarDAL(string comandoSql)
        {
            _sqlConnection.Open();
            var comando = _sqlConnection.CreateCommand();
            var transaction = _sqlConnection.BeginTransaction();
            try
            {
                comando.CommandText = comandoSql;
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

        }
        protected void DeletarDAL(string comandoSql)
        {
            _sqlConnection.Open();

            SqlCommand comando = _sqlConnection.CreateCommand();

            var transcao = _sqlConnection.BeginTransaction();

            try
            {
             
                comando.Transaction = transcao;

                comando.CommandText = comandoSql;

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
        protected T ObterPorIdDAL(string comandoSql)
        {
            _sqlConnection.Open();

            var objeto = _sqlConnection.Query<T>(comandoSql).FirstOrDefault();

            _sqlConnection.Close();

            return objeto;
        }
        protected List<T> ObterVariosDAL(string comandoSql)
        {
            _sqlConnection.Open();

            var listaObjeto = _sqlConnection.Query<T>(comandoSql);

            _sqlConnection.Close();

            return listaObjeto.ToList();
        }
    }
}
