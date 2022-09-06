using Getnet.IService;
using Getnet.DTO;
using Getnet.DTO.CardVerification;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Getnet.DTO.Error;
using Getnet.DTO.Credit;
using Getnet.DTO.PaymentCreditCard;
using GetNet.Util;
namespace Getnet.Service
{
    public class PaymentService :  IPaymentService
    {
        private readonly string _tokenBearer;
        private readonly Key _key;

        public PaymentService(string tokenBearer)
        {
            _key = new Key();
            _tokenBearer = tokenBearer;
        }

        public async Task<CardVerificationResponse> GetCardVerification(string numberToken, Brand brand, string cardHolderName, string expirationMonth, string expirationYear, string securityCode)
        {
           
            HttpClientHandler httpClientHandler = new HttpClientHandler();
           

            CardVerificationRequest cardVerificationRequest = new CardVerificationRequest()
            {
                NumberToken = numberToken,
                Brand = brand,
                CardholderName = cardHolderName,
                ExpirationMonth = expirationMonth,
                ExpirationYear = expirationYear,
                SecurityCode = securityCode
            };

            string requestBody = "";
            string responseBody = "";

            

            httpClientHandler.AutomaticDecompression |= DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None;

            requestBody = JsonConvert.SerializeObject(cardVerificationRequest);

            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    RequestUri = new Uri($"{_key.UrlApi}/v1/cards/verification"),
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
                };


                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenBearer);
                httpRequestMessage.Headers.TryAddWithoutValidation("seller_id ", _key.SellerId);

                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseBody);
                    throw new Exception(errorResponse.ToString());
                }

            }

            CardVerificationResponse cardVerificationResponse = JsonConvert.DeserializeObject<CardVerificationResponse>(responseBody);

            return cardVerificationResponse;

        }

        public async Task<PaymentCreditCardResponse> PaymentsCredit(long amount, Currency currency, Order order, Customer customer, Device device, List<Shipping> shippings, SubMerchant subMerchant, CreditRequest creditRequest)
        {
            PaymentCreditCardRequest paymentCreditCardRequest;
            PaymentCreditCardResponse paymentCreditCardResponse;
            HttpClientHandler httpClientHandler;
            HttpRequestMessage httpRequestMessage;
            HttpResponseMessage httpResponseMessage;

            string requestBody = "";
            string result = "";

            //atribuicao
          
            paymentCreditCardRequest = new PaymentCreditCardRequest()
            {
                SellerId = _key.SellerId,
                Amount = amount,
                Currency = currency,
                Order = order,
                Device = device,
                Shippings = shippings,
                SubMerchant = subMerchant,
                Credit = creditRequest,
                Customer = customer
            };

            requestBody = JsonConvert.SerializeObject(paymentCreditCardRequest);

            httpClientHandler = new HttpClientHandler();

            httpClientHandler.AutomaticDecompression |= DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None;


            using (HttpClient httpClient = new HttpClient(httpClientHandler))
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                httpRequestMessage = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    Content = new StringContent(requestBody, Encoding.UTF8, "application/json"),
                    RequestUri = new Uri(_key.UrlApi + "/v1/payments/credit"),
                };

                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenBearer);

                httpRequestMessage.Headers.TryAddWithoutValidation("Content-Type", "application/json;charset=utf-8");

                httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                result = await httpResponseMessage.Content.ReadAsStringAsync();

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(result);
                    throw new Exception(errorResponse.ToString());
                }

                paymentCreditCardResponse = JsonConvert.DeserializeObject<PaymentCreditCardResponse>(result);

                return paymentCreditCardResponse;
            }
        }
    }
}
