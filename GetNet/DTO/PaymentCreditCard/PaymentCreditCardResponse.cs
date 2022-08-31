using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Getnet.DTO.Credit;

namespace Getnet.DTO.PaymentCreditCard
{
    public class PaymentCreditCardResponse
    {
        [JsonProperty("payment_id")]
        public string PaymentId { get; set; }

        [JsonProperty("seller_id")]
        public string SellerId { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("currency")]
        public Currency Currency { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("received_at")]
        public string ReceivedAt { get; set; }

        [JsonProperty("credit")]
        public CreditResponse Credit { get; set; }

        public override string ToString()
        {
            return $@"
payment_id = {PaymentId},
seller_id = {SellerId}
amount = {Amount}
currency = {Currency}
order_id = {OrderId}
status = {Status}
received_at = {ReceivedAt}
credit =  {Credit}
";
        }

    }
}
