using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MNPOSTWEBSITE.Controllers
{
    public class TransportController : Controller
    {
        //
        // GET: /Transport/
        public ActionResult FastTransportation()
        {
            return View();
        }

        public ActionResult NormalTransportation()
        {
            return View();
        }
    }
}