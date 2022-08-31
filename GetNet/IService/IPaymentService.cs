using Getnet.DTO;
using Getnet.DTO.CardVerification;
using Getnet.DTO.Credit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Getnet.DTO.PaymentCreditCard;
namespace Getnet.IService
{
    public interface IPaymentService
    {
        Task<CardVerificationResponse> GetCardVerification(
                string numberToken, 
                Brand brand, 
                string cardHolderName,
                string expirationMonth, 
                string expirationYear, 
                string securityCode
            );
        /// <summary>
        /// Nesse método serão recebidos dados para pagamento com cartão de crédito.
        /// </summary>
        /// <returns></returns>
        Task<PaymentCreditCardResponse> PaymentsCredit(
            long amount,
            Currency currency,
            Order order,
            Customer customer,
            Device device,
            List<Shipping> shippings,
            SubMerchant subMerchant,
            CreditRequest creditRequest
          );
    }
}
