using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssinaturaSistemasSolution.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult About()
        {
            return View();
        }
        public ViewResult BuyNow()
        {
            return View();
        }
        public ViewResult ContactUS()
        {
            return View();
        }
        public ViewResult Versions()
        {
            return View();
        }
    
    }
}