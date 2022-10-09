using DAL.Interfaces;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Implementacao
{
    public class UsuarioDAL : DAL<Usuario>, IDAL<Usuario>
    {
        public Usuario Criar(Usuario objeto)
        {
            string comandoSql = $@"
                INSERT INTO [dbo].[Usuario]
                           ([NomeCompleto]
                           ,[Email]
                           ,[Senha]
                           ,[Desabilitado])
                     VALUES
                           ('{objeto.NomeCompleto}'
                           ,'{objeto.Email}'
                           ,'{objeto.Senha}'
                           , {(objeto.Desabilitado ? 1 : 0)})";

            objeto.IdUsuario = this.DALcriar(comandoSql);


            return objeto;
        }

        public void Deletar(int id)
        {
            string comandoSql = $@"DELETE FROM [dbo].[Usuario]
                                  WHERE  IdUsuario = {id}";

            this.DALdeletar(comandoSql);
        }

        public Usuario Editar(Usuario objeto)
        {
            string comandoSql = $@"UPDATE [dbo].[Usuario]
                                   SET [NomeCompleto] = '{objeto.NomeCompleto}'
                                      ,[Email] = '{objeto.Email}'
                                      ,[Senha] = '{objeto.Senha}'
                                      ,[Desabilitado] = {(objeto.Desabilitado ? 1 : 0)}
                                 WHERE IdUsuario  = {objeto.IdUsuario}";

            this.DALeditar(comandoSql);

            return objeto;
        }

        public Usuario ObterPorId(int id)
        {
            string comandoSql = $@"SELECT [IdUsuario]
                                      ,[NomeCompleto]
                                      ,[Email]
                                      ,[Senha]
                                      ,[Desabilitado]
                                  FROM [dbo].[Usuario]
                                  WHERE IdUsuario = {id}";

            return this.DALobterPorId(comandoSql);
        }

        public List<Usuario> ObterVarios()
        {
            string comandoSql = $@"SELECT [IdUsuario]
                                      ,[NomeCompleto]
                                      ,[Email]
                                      ,[Senha]
                                      ,[Desabilitado]
                                  FROM [dbo].[Usuario]";

            return this.DALobterVarios(comandoSql);
        }
    }
}
