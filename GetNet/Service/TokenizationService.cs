using Getnet.IService;
using Getnet.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GetNet.Util;
namespace Getnet.Service
{
    public class TokenizationService : Key, ITokenizationService
    {
        private readonly string _tokenBearer;
        public TokenizationService(string tokenBearer):base()
        {
            _tokenBearer = tokenBearer;
        }

        public async Task<string> GetCreditCardToken(string cardNumber, string custumerId)
        {
            
            TokenizationRequest tokenizationRequest = new TokenizationRequest()
            {
                CardNumber = cardNumber,
                CustomerId = custumerId
            };

            string requestUri = this.UrlApi + "/v1/tokens/card";
            string requestBody = JsonConvert.SerializeObject(tokenizationRequest);

            HttpClientHandler clientHandler = new HttpClientHandler();

            clientHandler.AutomaticDecompression |= DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None;

            using (HttpClient httpClient = new HttpClient(clientHandler))
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpRequestMessage requestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri(requestUri),
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };

                ////headers
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenBearer);
                requestMessage.Headers.TryAddWithoutValidation("seller_id", this.SellerId);


                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(requestMessage);

                var result = await httpResponseMessage.Content.ReadAsStringAsync();


                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    string errorMessage = httpResponseMessage.StatusCode + ";" + httpResponseMessage.ReasonPhrase + ";" + result;
                    throw new Exception(errorMessage);
                }

                TokenizationResponse tokenizationResponse = JsonConvert.DeserializeObject<TokenizationResponse>(result);

                return tokenizationResponse.NumberToken;
            }

        }
    }
}
