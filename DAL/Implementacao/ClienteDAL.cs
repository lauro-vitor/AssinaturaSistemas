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
    public class ClienteDAL : IClienteDAL
    {
        private readonly SqlConnection _sqlConnection;
        public ClienteDAL()
        {
            _sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AssinaturaSistemaSqlServer"].ConnectionString);
        }

        public void Alterar(Cliente cliente)
        {
            _sqlConnection.Open();
            SqlCommand comando = _sqlConnection.CreateCommand();
            var transaction = _sqlConnection.BeginTransaction();

            string dataCadastro = "null";

            if (cliente.DataCadastro.HasValue)
            {
                dataCadastro = $"'{cliente.DataCadastro}'";
            }

            string sql = $@"UPDATE Cliente
                            SET [NomeEmpresa] = '{cliente.NomeEmpresa}',
                                [DataCadastro] = {dataCadastro},
                                [Ativo] = {(cliente.Ativo ? 1 : 0)},
                                [IdEstado] = {cliente.IdEstado},
                                [CodigoPostal] =  '{cliente.CodigoPostal}',
                                [Endereco] = '{cliente.Endereco}',
                                [Observacao] = '{cliente.Observacao}',
                                [UltimaAtualizacao] = '{DateTime.Now}'
                            WHERE [IdCliente] = {cliente.IdCliente}";
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
        }

        public void Criar(Cliente cliente)
        {
            _sqlConnection.Open();

            SqlCommand comando = _sqlConnection.CreateCommand();

            var transaction = _sqlConnection.BeginTransaction();

            string dataCadastro = "null";

            if (cliente.DataCadastro.HasValue)
            {
                dataCadastro = $"'{cliente.DataCadastro}'";
            }

            string sql = $@"
                INSERT INTO Cliente([NomeEmpresa],
                    [DataCadastro],
                    [Ativo],
                    [IdEstado],
                    [CodigoPostal],
                    [Endereco],
                    [Observacao],
                    [UltimaAtualizacao])
                VALUES('{cliente.NomeEmpresa}',
                       {dataCadastro},
                       {(cliente.Ativo ? 1 : 0)},
                       {cliente.IdEstado},
                       '{cliente.CodigoPostal}',
                       '{cliente.Endereco}',
                       '{cliente.Observacao}',
                       '{DateTime.Now}');

            SELECT @@IDENTITY AS [Last-Inserted Identity Value];
            ";

            try
            {
                comando.CommandText = sql;
                comando.Transaction = transaction;
                object idInserido = comando.ExecuteScalar();
                cliente.IdCliente = int.Parse(idInserido.ToString());

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

        public void Deletar(int idCliente)
        {
            _sqlConnection.Open();
            var comando = _sqlConnection.CreateCommand();
            var transaction = _sqlConnection.BeginTransaction();

            try
            {
                comando.CommandText = "DELETE FROM Cliente WHERE IdCliente = " + idCliente;
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

        public Cliente ObterPorId(int idCliente)
        {
            string query = $"SELECT * FROM VwListaClientes WHERE Idcliente = {idCliente}";

            _sqlConnection.Open();

            var cliente = _sqlConnection.Query<Cliente>(query).FirstOrDefault();

            _sqlConnection.Close();

            return cliente;
        }

        public List<Cliente> ObterVarios(Func<Cliente,bool> filtro)
        {
            List<Cliente> clientes = new List<Cliente>();

            _sqlConnection.Open();

            string sql = "SELECT * FROM Cliente";

            clientes = _sqlConnection.Query<Cliente>(sql).Where(filtro).ToList();
                
            _sqlConnection.Close();

            return clientes;
        }

        public List<VwListaClientes> ObterVwListaClientes(string nomeEmpresa, int idPais, int idEstado, string codigoPostal, string endereco, string dataCadastroInicial, string dataCadastroFinal, bool? ativo)
        {
            List<VwListaClientes> vwListaClientes = new List<VwListaClientes>();

            _sqlConnection.Open();

            string sql = "SELECT * FROM VwListaClientes";

            vwListaClientes = _sqlConnection.Query<VwListaClientes>(sql)
                .Where(c => (string.IsNullOrEmpty(nomeEmpresa) || c.NomeEmpresa.ToLower().Contains(nomeEmpresa.ToLower())) &&
                    (idPais == 0 || c.IdPais == idPais) &&
                    (idEstado == 0 || c.IdEstado == idEstado) &&
                    (string.IsNullOrEmpty(codigoPostal) || c.CodigoPostal.Contains(codigoPostal.ToLower())) &&
                    (string.IsNullOrEmpty(endereco) || c.Endereco.ToLower().Contains(endereco.ToLower())) &&
                    (string.IsNullOrEmpty(dataCadastroInicial) || (c.DataCadastro.HasValue && c.DataCadastro.Value >= Convert.ToDateTime(dataCadastroInicial))) &&
                    (string.IsNullOrEmpty(dataCadastroFinal) || (c.DataCadastro.HasValue && c.DataCadastro.Value <= Convert.ToDateTime(dataCadastroFinal))))
                .OrderBy(c => c.NomeEmpresa)
                .ToList();

            _sqlConnection.Close();

            return vwListaClientes;

        }
    }
}
