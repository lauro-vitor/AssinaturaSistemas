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
    public class ClienteDAL : DAL<Cliente>, IDAL<Cliente>, IDALTransacao<Cliente>
    {
       
        public Cliente Criar(Cliente cliente, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
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

            cliente.IdCliente = this.DALcriarComTransacao(sql, sqlConnection, sqlTransaction);

            return cliente;
        }

        public Cliente Criar(Cliente cliente)
        {
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

            cliente.IdCliente = this.DALcriar(sql);

            return cliente;
        }

        public void Deletar(int id)
        {
            string sql = "DELETE FROM Cliente WHERE IdCliente = " + id;

            this.DALdeletar(sql);
        }

        public Cliente Editar(Cliente cliente)
        {
            string dataCadastro = "null";

            if (cliente.DataCadastro.HasValue)
            {
                dataCadastro = $"'{cliente.DataCadastro}'";
            }

            string sql = $@"    
                                UPDATE Cliente
                                SET [NomeEmpresa] = '{cliente.NomeEmpresa}',
                                    [DataCadastro] = {dataCadastro},
                                    [Ativo] = {(cliente.Ativo ? 1 : 0)},
                                    [IdEstado] = {cliente.IdEstado},
                                    [CodigoPostal] =  '{cliente.CodigoPostal}',
                                    [Endereco] = '{cliente.Endereco}',
                                    [Observacao] = '{cliente.Observacao}',
                                    [UltimaAtualizacao] = '{DateTime.Now}'
                                WHERE [IdCliente] = {cliente.IdCliente}";

            this.DALeditar(sql);

            return cliente;
        }

        public Cliente ObterPorId(int id)
        {
            string sql = $"SELECT * FROM VwListaClientes WHERE Idcliente = {id}";

            return this.DALobterPorId(sql);
        }

        public List<Cliente> ObterVarios()
        {
            string sql = "SELECT * FROM Cliente";

            return this.DALobterVarios(sql);
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
