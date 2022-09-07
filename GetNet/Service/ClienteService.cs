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
        private readonly string _uri;
        public ClienteService(string tokenBearer)
        {
            _key = new Key();
            _tokenBearer = tokenBearer;
            _uri = $"{_key.UrlApi}/v1/customers";
        }



        public async Task CadastraUmNovoCliente(string customerId, string firstName, string lastName, DocumentType? documentType, string documentNumber, string birthDate, string phoneNumber, string celphoneNumber, string email, string observation, string district, string city, string state, string country, string postalCode, string number, string street, string complement)
        {
            HttpRequestMessage httpRequestMessage;
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            string errorMessage;
            string requestBody;
            string result;


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
                    RequestUri = new Uri(_uri),
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

        public async Task<ClienteResponse> ListaDosClientes(int page, int limit, string customerId, string documentNumber, string firstName, string lastName, string sort, string sortType)
        {
            HttpClientHandler httpClientHandler;
            HttpRequestMessage request;
            HttpResponseMessage response;
            List<KeyValuePair<string, string>> parametros = new List<KeyValuePair<string, string>>();
            string queryString;
            string result;
            string errorMessage;
            string uriString;

            if (page < 1)
                throw new Exception("a paginação deve ser maior do que 1");

            if (limit < 0 && limit > 9999)
                throw new Exception("o limit da paginação deve está entre 0 e 9999");

            parametros.Add(new KeyValuePair<string, string>("page", page.ToString()));

            parametros.Add(new KeyValuePair<string, string>("limit", limit.ToString()));

            if (!string.IsNullOrEmpty(customerId))
                parametros.Add(new KeyValuePair<string, string>("customer_id", customerId));

            if (!string.IsNullOrEmpty(documentNumber))
                parametros.Add(new KeyValuePair<string, string>("document_number", documentNumber));

            if (!string.IsNullOrEmpty(firstName))
                parametros.Add(new KeyValuePair<string, string>("first_name", firstName));

            if (!string.IsNullOrEmpty(lastName))
                parametros.Add(new KeyValuePair<string, string>("last_name", lastName));

            if (!string.IsNullOrEmpty(sort))
                parametros.Add(new KeyValuePair<string, string>("sort", sortType));

            if (!string.IsNullOrEmpty(sortType))
                parametros.Add(new KeyValuePair<string, string>("sort_type", sortType));

            using (FormUrlEncodedContent content = new FormUrlEncodedContent(parametros))
            {
                queryString = "?" + await content.ReadAsStringAsync();
            }

            httpClientHandler = new HttpClientHandler();

            httpClientHandler.AutomaticDecompression |= DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None;

            uriString = _uri +queryString;

            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                request = new HttpRequestMessage()
                {
                    Method = new HttpMethod("GET"),
                    RequestUri = new Uri(uriString),
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _tokenBearer);

                request.Headers.TryAddWithoutValidation("seller_id", _key.SellerId);

                response = await httpClient.SendAsync(request);

                result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    errorMessage = $"{response.StatusCode}; {response.ReasonPhrase}; {result}";

                    throw new Exception(errorMessage);
                }

                ClienteResponse clienteResponse = JsonConvert.DeserializeObject<ClienteResponse>(result);

                return clienteResponse;
            }
        }
    }
}
