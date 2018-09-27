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
	}
}