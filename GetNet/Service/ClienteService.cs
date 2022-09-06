using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Getnet.DTO;
using GetNet.DTO.ClienteDTO;
using GetNet.IService;
using GetNet.Util;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GetNet.Service
{
    public class ClienteService : IClienteService
    {
        private readonly Key _key;
        private readonly string _tokenBearer;
        public ClienteService(string tokenBearer)
        {
            _key = new Key();
            _tokenBearer = tokenBearer;
        }

        

        public async Task CadastraUmNovoCliente(string customerId, string firstName, string lastName, DocumentType? documentType, string documentNumber, string birthDate, string phoneNumber, string celphoneNumber, string email, string observation, string district, string city, string state, string country, string postalCode, string number, string street, string complement)
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            string uri = $"{_key.UrlApi}/v1/customers";
            string requestBody = "";
            string result = "";
            string errorMessage = "";

            Address address = new Address()
            {
                District = district,
                City = city,
                State = state,
                Country = country,
                PostalCode = postalCode,
                Number = number,
                Street = street,
                Complement = complement, // nao pode ser null
            };

            ClienteRequest cliente = new ClienteRequest()
            {
                SellerId = _key.SellerId,
                CustomerId = customerId,
                FirstName = firstName,
                LastName = lastName,
                DocumentType = documentType,
                DocumentNumber = documentNumber,
                BirthDate = birthDate,
                PhoneNumber = phoneNumber,
                CelphoneNumber = celphoneNumber,
                Email = email,
                Observation = observation,
                Address = address
            };

            httpClientHandler.AutomaticDecompression |= DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None;

            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                requestBody = JsonConvert.SerializeObject(cliente);

                httpRequestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri(uri),
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json"),

                };

                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenBearer);
                httpRequestMessage.Headers.TryAddWithoutValidation("seller_id", _key.SellerId);

                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                result = await httpResponseMessage.Content.ReadAsStringAsync();

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    errorMessage = $"{httpResponseMessage.StatusCode}; {httpResponseMessage.ReasonPhrase}; {result}";

                    throw new Exception(errorMessage);
                }

            }
        }
    }
}
