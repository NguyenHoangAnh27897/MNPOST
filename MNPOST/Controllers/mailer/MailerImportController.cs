using MNPOST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MNPOST.Controllers.mailer
{
    public class MailerImportController : MailerController
    {
        // GET: MailerImport
        public ActionResult Show()
        {
            ViewBag.PostOffices = EmployeeInfo.postOffices;

            return View();
        }

        [HttpGet]
        public ActionResult GetData(string postId)
        {

            var data = db.MAILER_GET_NOT_INVENTORY("%" + postId + "%").ToList();

            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddMailers(List<string> mailers, string postId)
        {
            List<string> listAdds = new List<string>();
            foreach (var item in mailers)
            {
                var find = db.MM_Mailers.Where(p => p.MailerID == item && p.PostOfficeAcceptID == postId).FirstOrDefault();

                if (find != null && find.CurrentStatusID == 0)
                {
                    find.CurrentStatusID = 1; // nhap kho
                    find.LastUpdateDate = DateTime.Now;
                    db.Entry(find).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    listAdds.Add(item);
                }
            }


            return Json(new ResultInfo()
            {
                error = 0,
                data = listAdds

            }, JsonRequestBehavior.AllowGet);
        }
    }
}