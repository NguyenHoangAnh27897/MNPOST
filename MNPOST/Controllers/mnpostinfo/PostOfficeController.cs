using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;

namespace MNPOST.Controllers.mnpostinfo
{
    // postoffice 
    public class PostOfficeController : BaseController
    {

        private string menuCode = "postoffice";
        //
        // GET: /PostOffice/

        [HttpGet]
        public ActionResult Show(string search = "")
        {
            if (!checkAccess("postoffice"))
                return Redirect("/error/relogin");

            var allPost = db.POSTOFFICE_GETALL().ToList();

            ViewBag.Zones = db.BS_Zones.ToList();

            return View(allPost);
        }


        [HttpPost]
        public ActionResult Add(BS_PostOffices info)
        {
            if (!checkAccess("postoffice", 1))
                return Json(new { error = 1, msg = "you don't have permission for this" }, JsonRequestBehavior.AllowGet);


            try
            {
                db.BS_PostOffices.Add(info);
                db.SaveChanges();
            }
            catch
            {
                return Json(new { error = 0, msg = "the post has exist" }, JsonRequestBehavior.AllowGet);

            }


            return Json(new { error = 0 }, JsonRequestBehavior.AllowGet);
        }

	}
}