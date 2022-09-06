using BLL;
using Getnet.DTO;
using GetNet.Service;
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
        
        public async Task CadastraUmNovoClienteTeste()
        {
            try
            {
                //testar cada campo do request e ver se vai quebrar e adicionar na regra de negócio

                ClienteService clienteService = new ClienteService(_tokenBearer);
                string customerId = "1";
                string firstName =   "Lauro Vitor";
                string lastName = "Pina Teste";
                DocumentType? documentType = DocumentType.CPF;
                string documentNumber = "318.596.007-60";
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
                    state, country, postalCode );

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

                Console.WriteLine("Ok em: ClienteService.CadastraUmNovoCliente");

            }catch(Exception ex)
            {

                Console.WriteLine($"Erro em : {this.GetType().FullName} \n \t {ex.Message}");
            }
        }
    }
}
