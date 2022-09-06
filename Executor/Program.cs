using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Getnet.Service;
using Executor.GetNet;
namespace Executor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AutenticacaoService autenticacao = new AutenticacaoService();
            string token =  await autenticacao.GeracaoTokenAcesso();


            await TestesClienteServiceGetNet(token);

            //Console.WriteLine(token);
            Console.ReadKey();
        }

        public static async Task TestesClienteServiceGetNet(string token)
        {
            ClienteServiceGetNetTeste clienteServiceGetNetTeste = new ClienteServiceGetNetTeste(token);

            await clienteServiceGetNetTeste.CadastraUmNovoClienteTeste();
        }
    }
}
