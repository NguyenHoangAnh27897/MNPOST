using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MNPOSTWEBSITE.Controllers
{
    public class AboutUsController : Controller
    {
        MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities();
        //
        // GET: /AboutUs/
        public ActionResult Introduce()
        {
            var rs = db.WS_AboutUs.Where(s=>s.ID.Equals("firstabu"));
            return View(rs);
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(string fullname = "", string Phone = "", string Email = "", string Title = "", string ContentContact = "")
        {
            MNPOSTWEBSITEMODEL.WS_Contact ct = new MNPOSTWEBSITEMODEL.WS_Contact();
            ct.Fullname = fullname;
            ct.Phone = Phone;
            ct.Email = Email;
            ct.Title = Title;
            ct.ContentContact = ContentContact;
            db.WS_Contact.Add(ct);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
	}
}