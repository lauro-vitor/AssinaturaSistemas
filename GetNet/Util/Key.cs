using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GetNet.Util
{
    public class Key
    {
        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }
        public string UrlApi { get; private set; }
        public string SellerId { get; private set; }
        public Key()
        {
            ClientId = ConfigurationManager.AppSettings["client_id"];
            ClientSecret = ConfigurationManager.AppSettings["client_secret"];
            UrlApi = ConfigurationManager.AppSettings["url_api"];
            SellerId = ConfigurationManager.AppSettings["seller_id"];
        }
    }
}
