using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MNPOST.Controllers.mnpostinfo
{
    // postoffice 
    public class PostOfficeController : BaseController
    {

        private string menuCode = "postoffice";
        //
        // GET: /PostOffice/

        [HttpGet]
        public ActionResult Show()
        {
            checkAccess(menuCode);

            var allPost = db.BS_PostOffices.ToList();

            return View(allPost);
        }


        [HttpPost]
        public ActionResult Add(string PostName, string Address)
        {

            checkAccess(menuCode, 1);

            return View();
        }
	}
}