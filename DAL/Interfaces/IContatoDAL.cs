using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IContatoDAL
    {
        Contato Criar(int idCliente,string nomeCompleto, string email, string celular, string telefone,  string senha);
        Contato Editar(int idContato, int idCliente ,string nomeCompleto, string email, string celular, string telefone,  string senha);
        void Deletar(int idContato);
        List<Contato> ObterVarios(string nomeCompleto, string email, string celular, string telefone);
        Contato ObterContatoPorId(int idContato);
    }
}
