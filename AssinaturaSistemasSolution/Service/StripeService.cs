using AssinaturaSistemasSolution.Models;
using BLL;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssinaturaSistemasSolution.Service
{
    public class StripeService
    {
        public StripeService()
        {
            StripeConfiguration.ApiKey = System.Configuration.ConfigurationManager.AppSettings["secret_key"];
        }

        public string CreateCustomer(string email)
        {
            var customerService = new CustomerService();

            var customerCreateOptions = new CustomerCreateOptions()
            {
                Email = email
            };

            var customer = customerService.Create(customerCreateOptions);

            return customer.Id;
        }

        public Subscription CreateSubscription(string customerId, string priceId)
        {
            var subscritpionService = new SubscriptionService();

            var subscriptionCreateOptions = new SubscriptionCreateOptions()
            {
                Customer = customerId,

                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = priceId,
                    },
                },
                PaymentBehavior = "default_incomplete",
            };

            subscriptionCreateOptions.AddExpand("latest_invoice.payment_intent");

            var subscription = subscritpionService.Create(subscriptionCreateOptions);

            return subscription;
        }


        public void Refund(PaymentIntent paymentIntent)
        {
            var refundCreateOptions = new RefundCreateOptions() { PaymentIntent = paymentIntent.Id };

            var refundService = new RefundService();

            refundService.Create(refundCreateOptions);
        }

        public List<string> ValidateConfirmPaymentModel(ConfirmPaymentModel confirmPayment)
        {
            var emailBLL = new EmailBLL();

            var erros = new List<string>();

            if (string.IsNullOrEmpty(confirmPayment.PriceId))
                erros.Add("PriceId cant be empty");

            if (string.IsNullOrEmpty(confirmPayment.Email))
                erros.Add("E-mail is mandatory field");
            else if (!emailBLL.ValidarEmail(confirmPayment.Email))
                erros.Add("E-mail invalid, please type another e-email");

            if (string.IsNullOrEmpty("Company is mandatory field"))
                erros.Add(confirmPayment.Company);

            if (string.IsNullOrEmpty(confirmPayment.FullName))
                erros.Add("Full name is mandatory field");

            if (!NumericoBLL.EhNumerico(confirmPayment.Phone))
                erros.Add("Phone must be only numbers");


            return erros;
        }
        public List<string> ValidateCreateSubscription(SubscriptionModel subscription)
        {
            var emailBLL = new EmailBLL();

            var erros = new List<string>();

            if (string.IsNullOrEmpty(subscription.Email))
                erros.Add("E-mail is mandatory field");
            else if (!emailBLL.ValidarEmail(subscription.Email))
                erros.Add("E-mail invalid, please type another e-email");

            if (string.IsNullOrEmpty(subscription.PriceId))
                erros.Add("Plans cant be empty");

            return erros;

        }
    }
}