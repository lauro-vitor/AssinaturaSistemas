using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssinaturaSistemasSolution.Models
{
    public class ConfirmPaymentModel
    {
        public string PriceId { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public PaymentIntent PaymentIntent {get; set;}

    }
}