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
    public class ContatoDAL : DAL<Contato>, IDAL<Contato>, IDALTransacao<Contato>
    {

        public ContatoDAL()
        {

        }

        public Contato Criar(Contato contato)
        {
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

            contato.IdContato = this.DALcriar(sql);

            return contato;
        }

        public Contato Criar(Contato contato, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
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

            contato.IdContato = this.DALcriarComTransacao(sql, sqlConnection, sqlTransaction);

            return contato;
        }

        public void Deletar(int id)
        {
            string sql = $@"DELETE FROM [Contato]
                            WHERE [IdContato] = {id}";

            this.DALdeletar(sql);
        }

        public Contato Editar(Contato contato)
        {
            string sql = $@"
                UPDATE [Contato] SET 
                    [NomeCompleto] = '{contato.NomeCompleto}',
                    [Email] = '{contato.Email}',
                    [Celular] = '{contato.Celular}',
                    [Telefone] = '{contato.Telefone}',
                    Senha = '{contato.Senha}'
                WHERE [IdContato] = {contato.IdContato}
            ";

            this.DALeditar(sql);

            return contato;
        }

        public Contato ObterPorId(int id)
        {
            string sql = $"SELECT * FROM Contato WHERE [IdContato] = {id}";

            return this.DALobterPorId(sql);
        }

        public List<Contato> ObterVarios()
        {
            string sql = "SELECT * FROM Contato";

            return this.DALobterVarios(sql);
        }
    }
}
