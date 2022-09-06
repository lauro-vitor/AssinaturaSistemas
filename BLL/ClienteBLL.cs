using Getnet.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ClienteBLL
    {
        public void ValidarCliente(string customerId, 
            string firstName, 
            string lastName, 
            DocumentType? documentType, 
            string documentNumber, 
            string birthDate,
            string phoneNumber,
            string celphoneNumber,
            string email,
            string observation,
            string street, 
            string number,
            string complement,
            string district,
            string city,
            string state,
            string county,
            string postalCode)
        {
            if (string.IsNullOrEmpty(customerId))
                throw new Exception("Id do cliente é obrigatório!");


            if (string.IsNullOrEmpty(firstName))
                throw new Exception("Nome é obrigatório");

            if (string.IsNullOrEmpty(lastName))
                throw new Exception("Sobrenome é obrigatório");

            if (!documentType.HasValue)
                throw new Exception("Tipo de Documento Obrigatório");

            if (string.IsNullOrEmpty(documentNumber))
            {
                if (documentType.Value == DocumentType.CPF)
                    throw new Exception("CPF obrigatório");
                else if (documentType.Value == DocumentType.CNPJ)
                    throw new Exception("CNPJ obrigatório");
            }

            if (string.IsNullOrEmpty(birthDate))
                throw new Exception("Data de Aniversário Obrigatório");

            if (string.IsNullOrEmpty(phoneNumber))
                throw new Exception("Telefone é obrigatório");

            if (string.IsNullOrEmpty(celphoneNumber))
                throw new Exception("Celular é obrigatório");

            if (string.IsNullOrEmpty(email))
                throw new Exception("E-mail é obrigatório");

            if (string.IsNullOrEmpty(observation))
                throw new Exception("Observção é obrigatório");

            //endereco

            if (string.IsNullOrEmpty(street))
                throw new Exception("Rua é obrigatório");

            if (string.IsNullOrEmpty(number))
                throw new Exception("Número é obrigatório");

            if (string.IsNullOrEmpty(complement))
                throw new Exception("Complemento é obrigatório");

            if (string.IsNullOrEmpty(district))
                throw new Exception("Bairro é obrigatório");

            if (string.IsNullOrEmpty(city))
                throw new Exception("Cidade é obrigatório");

            if (string.IsNullOrEmpty(state))
                throw new Exception("Estado é obrigatório");

            if (string.IsNullOrEmpty(county))
                throw new Exception("País é obrigatório");

            if (string.IsNullOrEmpty(postalCode))
                throw new Exception("CEP é obrigatório");
        }
    }
}
