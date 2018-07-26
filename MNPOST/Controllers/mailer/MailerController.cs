using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MNPOST.Controllers.mailer
{
    public class MailerController : BaseController
    {

        [HttpGet]
        public ActionResult Show(int? page)
        {



            return View();
        }


	}
}