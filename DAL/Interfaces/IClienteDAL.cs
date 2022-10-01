using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
namespace DAL.Interfaces
{
    public interface IClienteDAL
    {
        void Criar(Cliente cliente);
        void Alterar(Cliente cliente);
        Cliente ObterPorId(int idCliente);
        void Deletar(int idCliente);
        List<Cliente> ObterVarios(Func<Cliente, bool> filtro);

        List<VwListaClientes> ObterVwListaClientes(string nomeEmpresa, int idPais, int idEstado,
            string codigoPostal, string endereco, string dataCadastroInicial, string dataCadastroFinal,
            bool? ativo);


    }
}
