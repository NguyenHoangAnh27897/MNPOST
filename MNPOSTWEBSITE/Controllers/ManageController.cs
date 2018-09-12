using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MNPOSTWEBSITE.Controllers
{
    public class ManageController : Controller
    {
        MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities();
        //
        // GET: /Manage/
        public ActionResult Index()
        {
            if(Session["Authentication"].ToString() != null)
            {
                string id = Session["ID"].ToString();
                var user = db.AspNetUsers.Where(s => s.Id == id);
                return View(user);
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
           
        }

	}
}