using MNPOSTWEBSITE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MNPOSTWEBSITE.Controllers
{
    public class TransportController : Controller
    {
        MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEEntities();
        public ActionResult Service(string sv = "")
        {
            var lst = db.WS_ServiceType.ToList();
            ViewBag.service = sv;
            return View(lst);
        }

        public ActionResult Recruitment()
        {
            var lst = db.WS_Recruitment.ToList();
            return View(lst);
        }

        public ActionResult ShowFile(string id)
        {
            id += ".pdf";
            var path = Server.MapPath("~/document/pdf");
            var file = Path.Combine(path, id);
            file = Path.GetFullPath(file);
            if (!file.StartsWith(path))
            {
                // someone tried to be smart and sent 
                // ?filename=..\..\creditcard.pdf as parameter
                throw new HttpException(403, "Forbidden");
            }
            return File(file, "application/pdf");
        }
    }
}