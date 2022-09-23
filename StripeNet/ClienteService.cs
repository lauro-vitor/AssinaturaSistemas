using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripeNet
{
    public class ClienteService
    {
        public ClienteService()
        {
            StripeConfiguration.ApiKey = ConfigurationManager.AppSettings["secret_key"];
        }

        public StripeList<Customer> ObterClientes()
        {
            CustomerService customerService = new CustomerService();
            var clientes = customerService.List();
            return clientes;
        }

        public void CriarCliente(string nome, string email, string descricao, string pais, string estado, string cidade, string bairro, string cep)
        {
            Customer customer = new Customer();

            var options = new CustomerCreateOptions()
            {
                Name = nome,
                Email = email,
                Description = descricao,
                Address = new AddressOptions()
                {
                    Country = pais,
                    State = estado,
                    City = cidade,
                    Line1 = bairro,
                    PostalCode = cep
                },
            };

            CustomerService customerServices = new CustomerService();

            customerServices.Create(options);
        }


    }
}
