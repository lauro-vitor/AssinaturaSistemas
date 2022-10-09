using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class AssinaturaDAL
    {
        private readonly SqlConnection _sqlConnection;
        private readonly IDALTransacao<Cliente> _clienteDAL;
        private readonly IDALTransacao<Contato> _contatoDAL;
        private readonly IDALTransacao<Sistema> _sistemaDAL;
        private readonly IDALTransacao<Parcela> _parcelaDAL;
        private readonly IDALTransacao<PagamentoParcela> _pagamentoParcela;
        public AssinaturaDAL()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            _clienteDAL = new ClienteDAL();
            _contatoDAL = new ContatoDAL();
            _sistemaDAL = new SistemaDAL();
            _parcelaDAL = new ParcelaDAL();
            _pagamentoParcela = new PagamentoParcelaDAL();

            _sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AssinaturaSistemaSqlServer"].ConnectionString);
        }

        public void Criar(Cliente cliente, Contato contato, Sistema sistema, Parcela parcela, PagamentoParcela pagamentoParcela)
        {
            _sqlConnection.Open();

            SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction();

            try
            {

                cliente = _clienteDAL.Criar(cliente, _sqlConnection, sqlTransaction);

                contato.IdCliente = cliente.IdCliente;
                contato = _contatoDAL.Criar(contato, _sqlConnection, sqlTransaction);

                sistema.IdCliente = cliente.IdCliente;
                sistema = _sistemaDAL.Criar(sistema, _sqlConnection, sqlTransaction);

                parcela.IdSistema = sistema.IdSistema;
                parcela = _parcelaDAL.Criar(parcela, _sqlConnection, sqlTransaction);

                pagamentoParcela.IdParcela = parcela.IdParcela;
                pagamentoParcela = _pagamentoParcela.Criar(pagamentoParcela, _sqlConnection, sqlTransaction);

                sqlTransaction.Commit();

            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw ex;
            }
            finally
            {
                sqlTransaction.Dispose();
                _sqlConnection.Close();
            }

        }
    }
}
