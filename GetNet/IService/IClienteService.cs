using Getnet.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetNet.IService
{
    public interface IClienteService
    {
        /// <summary>
        /// Neste endpoint você pode cadastrar os dados de um novo cliente para recorrência.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="documentType"></param>
        /// <param name="documentNumber"></param>
        /// <param name="brithDate"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="celphoneNumber"></param>
        /// <param name="email"></param>
        /// <param name="observation"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        Task CadastraUmNovoCliente(string customerId,
            string firstName,
            string lastName,
            DocumentType? documentType,
            string documentNumber,
            string birthDate,
            string phoneNumber,
            string celphoneNumber,
            string email,
            string observation,
            string district,
            string city,
            string state,
            string country,
            string postalCode,
            string number,
            string street,
            string complement);



    }
}
