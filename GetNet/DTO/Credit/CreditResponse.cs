using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Getnet.DTO.Credit
{
    public class CreditResponse
    {
        [JsonProperty("delayed")]
        public bool Delayed { get; set; }

        [JsonProperty("authorization_code")]
        public long AuthorizationCode { get; set; }

        [JsonProperty("authorized_at")]
        public string AuthorizedAt { get; set; }

        [JsonProperty("reason_code")]
        public string ReasonCode { get; set; }

        [JsonProperty("reason_message")]
        public string ReasonMessage { get; set; }

        [JsonProperty("acquirer")]
        public string Acquirer { get; set; }

        [JsonProperty("soft_descriptor")]
        public string SoftDescriptor { get; set; }

        [JsonProperty("brand")]
        public Brand Brand { get; set; }

        [JsonProperty("terminal_nsu")]
        public long TerminalNsu { get; set; }

        [JsonProperty("acquirer_transaction_id")]
        public string AcquirerTransactionId { get; set; }

        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }

        [JsonProperty("first_installment_amount")]
        public string FirstInstallmentAmount { get; set; }

        [JsonProperty("other_installment_amount")]
        public string OtherInstallmentAmount { get; set; }

        [JsonProperty("total_installment_amount")]
        public string TotalInstallmentAmount { get; set; }

        public override string ToString()
        {
            return $@"
    delayed = {Delayed}
    AuthorizationCode = {AuthorizationCode}
    AuthorizedAt = {AuthorizedAt}
    ReasonCode = {ReasonCode}
    ReasonMessage = {ReasonMessage}
    acquirer = {Acquirer}
    soft_descriptor = {SoftDescriptor}
    brand = {Brand}
    terminal_nsu = {TerminalNsu}
    acquirer_transaction_id = {AcquirerTransactionId}
    transaction_id = {TransactionId}
    first_installment_amount =  {FirstInstallmentAmount}
    other_installment_amount = {OtherInstallmentAmount}
    total_installment_amount = {TotalInstallmentAmount}
";
        }

    }
}
