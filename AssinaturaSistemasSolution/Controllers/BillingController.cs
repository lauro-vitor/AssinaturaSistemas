using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.IO;
using AssinaturaSistemasSolution.Models;

namespace AssinaturaSistemasSolution.Controllers
{
    public class BillingController : Controller
    {

        private string SecretKey
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["secret_key"]; }
        }
        private string PublicKey
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["public_key"]; }
        }
        public string WebHookSecret
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["WebHookSecret"]; }
        }

       
        public BillingController()
        {

            StripeConfiguration.ApiKey = this.SecretKey;
        }
        

        [HttpGet]
        [Route("config")]
        public ActionResult GetConfig()
        {

            var options = new PriceListOptions
            {
                LookupKeys = new List<string>
              {
                "sample_basic",
                "sample_premium"
              }
            };
            var service = new PriceService();
            var prices = service.List(options);

            return Json(new
            {
                PublishableKey = this.PublicKey,
                Prices = prices.Data
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("create-customer")]
        public ViewResult CreateCustomer(FormCollection formCollection)
        {
            string email = formCollection["email"];

            var options = new CustomerCreateOptions
            {
                Email = email,
            };
            var service = new CustomerService();
            var customer = service.Create(options);

            // Set the cookie to simulate an authenticated user.
            // In practice, this customer.Id is stored along side your
            // user and retrieved along with the logged in user.

            BillingModel billingModel = new BillingModel();

            billingModel.CustomerId = customer.Id;
         

            return View("", billingModel);
        }

        [HttpPost]
        [Route("create-subscription")]
        public ActionResult CreateSubscription(FormCollection formCollection)
        {
            var priceId = formCollection["PriceId"];
            var customerId = HttpContext.Request.Cookies["customer"];

            // Create subscription
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = customerId.Value,
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

                return Json(new
                {
                    SubscriptionId = subscription.Id,
                    ClientSecret = subscription.LatestInvoice.PaymentIntent.ClientSecret,
                });
            }
            catch (StripeException e)
            {
                this.HttpContext.Response.StatusCode = 400;

                return Content($"Failed to create subscription.{e}");
            }
        }

        [HttpGet]
        [Route("invoice-preview")]
        public ActionResult InvoicePreview(string subscriptionId, string newPriceLookupKey)
        {
            var customerId = HttpContext.Request.Cookies["customer"];
            var service = new SubscriptionService();
            var subscription = service.Get(subscriptionId);

            var invoiceService = new InvoiceService();
            var options = new UpcomingInvoiceOptions
            {
                Customer = customerId.Value,
                Subscription = subscriptionId,
                SubscriptionItems = new List<InvoiceSubscriptionItemOptions>
                {
                    new InvoiceSubscriptionItemOptions
                    {
                        Id = subscription.Items.Data[0].Id,
                        Price = Environment.GetEnvironmentVariable(newPriceLookupKey.ToUpper()),
                    },
                }
            };
            Invoice upcoming = invoiceService.Upcoming(options);
            return Json(new
            {
                Invoice = upcoming,
            });
        }

        [HttpPost]
        [Route("cancel-subscription")]
        public ActionResult CancelSubscription(FormCollection formCollection)
        {
            string subscriptionReq = formCollection["Subscription"];

            var service = new SubscriptionService();
            var subscription = service.Cancel(subscriptionReq, null);
            return Json(new
            {
                Subscription = subscription,
            });
        }

        [HttpPost]
        [Route("update-subscription")]
        public ActionResult UpdateSubscription(FormCollection formCollection)
        {
            var reqSubscription = formCollection["Subscription"];

            var reqNewPrice = formCollection["NewPrice"];

            var service = new SubscriptionService();

            var subscription = service.Get(reqSubscription);

            var options = new SubscriptionUpdateOptions
            {
                CancelAtPeriodEnd = false,
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Id = subscription.Items.Data[0].Id,
                        Price = Environment.GetEnvironmentVariable(reqNewPrice.ToUpper()),
                    }
                }
            };
            var updatedSubscription = service.Update(reqSubscription, options);

            return Json(new
            {
                Subscription = updatedSubscription,
            });
        }

        [HttpGet]
        [Route("subscriptions")]
        public ActionResult ListSubscriptions()
        {
            var customerId = HttpContext.Request.Cookies["customer"];
            var options = new SubscriptionListOptions
            {
                Customer = customerId.Value,
                Status = "all",
            };
            options.AddExpand("data.default_payment_method");
            var service = new SubscriptionService();
            var subscriptions = service.List(options);

            return Json(new
            {
                Subscriptions = subscriptions,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("webhook")]
        public async Task<ActionResult> Webhook()
        {
            var bodyStream = new StreamReader(HttpContext.Request.InputStream);
            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
            var bodyText = bodyStream.ReadToEnd();

            var json = bodyText;

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    this.WebHookSecret
                );
                Console.WriteLine($"Webhook notification with type: {stripeEvent.Type} found for {stripeEvent.Id}");
            }
            catch (Exception e)
            {
                string message = $"Something failed {e}";

                HttpContext.Response.StatusCode = 400;

                return Content(message);
            }

            if (stripeEvent.Type == "invoice.payment_succeeded")
            {
                var invoice = stripeEvent.Data.Object as Invoice;

                if (invoice.BillingReason == "subscription_create")
                {
                    // The subscription automatically activates after successful payment
                    // Set the payment method used to pay the first invoice
                    // as the default payment method for that subscription

                    // Retrieve the payment intent used to pay the subscription
                    var service = new PaymentIntentService();
                    var paymentIntent = service.Get(invoice.PaymentIntentId);

                    // Set the default payment method
                    var options = new SubscriptionUpdateOptions
                    {
                        DefaultPaymentMethod = paymentIntent.PaymentMethodId,
                    };
                    var subscriptionService = new SubscriptionService();
                    subscriptionService.Update(invoice.SubscriptionId, options);

                    Console.WriteLine($"Default payment method set for subscription: {paymentIntent.PaymentMethodId}");
                }
                Console.WriteLine($"Payment succeeded for invoice: {stripeEvent.Id}");
            }

            if (stripeEvent.Type == "invoice.paid")
            {
                // Used to provision services after the trial has ended.
                // The status of the invoice will show up as paid. Store the status in your
                // database to reference when a user accesses your service to avoid hitting rate
                // limits.
            }
            if (stripeEvent.Type == "invoice.payment_failed")
            {
                // If the payment fails or the customer does not have a valid payment method,
                // an invoice.payment_failed event is sent, the subscription becomes past_due.
                // Use this webhook to notify your user that their payment has
                // failed and to retrieve new card details.
            }
            if (stripeEvent.Type == "invoice.finalized")
            {
                // If you want to manually send out invoices to your customers
                // or store them locally to reference to avoid hitting Stripe rate limits.
            }
            if (stripeEvent.Type == "customer.subscription.deleted")
            {
                // handle subscription cancelled automatically based
                // upon your subscription settings. Or if the user cancels it.
            }
            if (stripeEvent.Type == "customer.subscription.trial_will_end")
            {
                // Send notification to your user that the trial will end
            }

            return Json(new { });
        }
    }
}