using BLL;
using Getnet.DTO;
using GetNet.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Executor.GetNet
{
    /// <summary>
    /// classe para realizar testes da uri: v1/customeres
    /// </summary>
    public class ClienteServiceGetNetTeste
    {
        private readonly string _tokenBearer;
        public ClienteServiceGetNetTeste(string tokenBearer)
        {
            _tokenBearer = tokenBearer;
        }

        public async Task ListaDosClientes()
        {
            try
            {
                ClienteService clienteService = new ClienteService(_tokenBearer);
                int page = 1;
                int limit = 9999;

                var clienteResponse = await clienteService.ListaDosClientes(page, limit, "", "", "", "", "", "");

              
                ImprimirMensagemSucesso("ClienteService.ListaDosClientes");

                ImprimirDetalhes(JsonConvert.SerializeObject(clienteResponse));
            }
            catch (Exception ex)
            {
                ImprimirMensagemErro("ClienteService.ListaDosClientes", ex.Message);
            }
        }
        public async Task CadastraUmNovoClienteTeste()
        {
            try
            {
                //testar cada campo do request e ver se vai quebrar e adicionar na regra de negócio

                ClienteService clienteService = new ClienteService(_tokenBearer);
                string customerId = "customer_21081827";
                string firstName = "teste Vitor";
                string lastName = "teste Teste";
                DocumentType? documentType = DocumentType.CPF;
                string documentNumber = "31859600760";
                string birthDate = "01/01/1996";
                string phoneNumber = "273333-3333";
                string celphoneNumber = "279999-9999";
                string email = "email@teste.com";
                string observation = "teste obs"; //"nehuma obs";

                //endereco criar uma nova rn para endereco
                string district = "santa monica";
                string city = "vila velha";
                string state = "ES";
                string country = "Bras";
                string postalCode = "29239";
                string number = "0";
                string street = "rua dos bobos";
                string complement = "rua dos bestas";

                ClienteBLL clienteBLL = new ClienteBLL();
                //falta testar o bll por enquanto somente a api
                clienteBLL.ValidarCliente(customerId, firstName, lastName, documentType,
                    documentNumber, birthDate, phoneNumber, celphoneNumber,
                    email, observation, street, number, complement, district, city,
                    state, country, postalCode);

                await clienteService.CadastraUmNovoCliente(
                    customerId,
                    firstName,
                    lastName,
                    documentType,
                    documentNumber,
                    birthDate,
                    phoneNumber,
                    celphoneNumber,
                    email,
                    observation,
                    district,
                    city,
                    state,
                    country,
                    postalCode,
                    number,
                    street,
                    complement);

                ImprimirMensagemSucesso("ClienteService.CadastraUmNovoCliente");

            }
            catch (Exception ex)
            {
                ImprimirMensagemErro("ClienteService.CadastraUmNovoCliente", ex.Message);
            }
        }

        private void ImprimirMensagemSucesso(string nomeMetodo)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Ok em: {nomeMetodo}");

            Console.WriteLine("\n");
        }

        private void ImprimirMensagemErro(string nomeMetodo, string mensagem)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Erro em : {nomeMetodo}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"descricao: {mensagem}");

            Console.WriteLine("\n");
        }
        private void ImprimirDetalhes(string detalhes)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine($"Detalhes: {detalhes}");
        }
    }
}
