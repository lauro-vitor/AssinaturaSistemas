using AssinaturaSistemasSolution.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssinaturaSistemasSolution.Controllers
{
    public class SubscriptionController : Controller
    {

        public SubscriptionController()
        {
            StripeConfiguration.ApiKey = System.Configuration.ConfigurationManager.AppSettings["secret_key"];
        }
        // GET: Subscription
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]//create-customer
        public ActionResult Register(FormCollection formCollection)
        {
            BillingModel billingModel = new BillingModel();

            var customerService = new CustomerService();

            string email = formCollection["email"];

            var creatCustomerOptions = new CustomerCreateOptions
            {
                Email = email,
            };

            var customer = customerService.Create(creatCustomerOptions);


            billingModel.CustomerId = customer.Id;

            billingModel.LoadPrices();

            return View("Prices", billingModel);
        }

        [HttpPost]//create-subscription
        public ActionResult Prices(FormCollection formCollection)
        {
            BillingModel billingModel = new BillingModel();

            string priceId = formCollection["priceId"];

            string customerId = formCollection["customerId"];

            billingModel.CustomerId = customerId;


            // Create subscription
            var subscriptionOptions = new SubscriptionCreateOptions
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

            subscriptionOptions.AddExpand("latest_invoice.payment_intent");

            var subscriptionService = new SubscriptionService();

            try
            {
                Subscription subscription = subscriptionService.Create(subscriptionOptions);

                billingModel.SubscrptionId = subscription.Id;

                billingModel.PaymentIntentClientSecret = subscription.LatestInvoice.PaymentIntent.ClientSecret;

                return View("Subscribe", billingModel);
            }
            catch (StripeException e)
            {
                billingModel.MessageErros.Add($"Failed to create subscription.{e.Message}");
            }

            billingModel.LoadPrices();

            return View(billingModel);
        }

        [HttpPost]
        public ActionResult Subscribe(FormCollection formCollection)
        {

            AccountModel accountModel = new AccountModel();

            try
            {
                string customerId = formCollection["customerId"];

                accountModel.CustomerId = customerId;

                var options = new SubscriptionListOptions
                {
                    Customer = customerId,
                    Status = "all",
                };

                options.AddExpand("data.default_payment_method");

                var service = new SubscriptionService();

                var subscriptions = service.List(options);

                accountModel.Subscriptions = subscriptions.ToList();
            }
            catch(Exception ex)
            {
                accountModel.MessageErros.Add(ex.Message);
            }

            return View("Account", accountModel);
        }

        [HttpGet]
        public ActionResult Account()
        {
            //depois fazer area do cliente
            return View();
        }
    }
}