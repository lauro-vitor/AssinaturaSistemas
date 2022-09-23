using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssinaturaSistemasSolution.Models
{
    public class AccountModel
    {
        public string CustomerId { get; set; }
        public bool HasError { get { return this.MessageErros.Count() > 0; } }
        public List<Subscription> Subscriptions { get; set; }
        public List<string> MessageErros { get; set; }
        public AccountModel()
        {   
            this.Subscriptions = new List<Subscription>();

            this.MessageErros = new List<string>();
        }
       
    }
}