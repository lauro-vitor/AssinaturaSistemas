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
    public class AutenticacaoService :  IAutenticacaoService
    {
        private readonly Key _key;
        public AutenticacaoService()
        {
            _key = new Key();
        }

        public async Task<string> GeracaoTokenAcesso()
        {
            string requestUri = _key.UrlApi + "/auth/oauth/v2/token";

            using (HttpClient httpClient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("scope","oob"),
                        new KeyValuePair<string, string>("grant_type","client_credentials"),
                    };

                HttpContent content = new FormUrlEncodedContent(parameters);

                string parameter = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{_key.ClientId}:{_key.ClientSecret}"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", parameter);

                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(requestUri, content);

                string result = await httpResponseMessage.Content.ReadAsStringAsync();

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    throw new Exception(httpResponseMessage + " " + result);
                }

                Authentication authentication = JsonConvert.DeserializeObject<Authentication>(result);

                return authentication.AccessToken;
            }
        }

        
    }
}
