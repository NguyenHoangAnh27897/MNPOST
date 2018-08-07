using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;
namespace MNPOST.Controllers.mailer
{
    public class MailerDeliveryController : BaseController
    {
        // GET: MailerDelivery
        public ActionResult Show()
        {
            ViewBag.AllEmployee = db.BS_Employees.ToList();//load du lieu sang combobox
            ViewBag.AllRoute = db.BS_Routes.ToList();
            return View();
        }


        [HttpGet]
        public ActionResult GetMailerDelivery(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.MM_MailerDelivery.Where(p => p.DocumentID.Contains(search) || p.EmployeeID.Contains(search)).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                page = pageNumber,
                pageSize = pageSize,
                toltalSize = data.Count(),
                data = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public ActionResult create(MM_MailerDelivery mailer)
        {

            if (String.IsNullOrEmpty(mailer.DocumentID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_MailerDelivery.Find(mailer.DocumentID);

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);


            mailer.CreateDate = DateTime.Now;
            mailer.LastEditDate = DateTime.Now;
            db.MM_MailerDelivery.Add(mailer);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = mailer }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult edit(MM_MailerDelivery mailer)
        {
            if (String.IsNullOrEmpty(mailer.DocumentID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_MailerDelivery.Find(mailer.DocumentID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.DocumentDate = mailer.DocumentDate;
            check.EmployeeID = mailer.EmployeeID;
            check.Notes = mailer.Notes;
            check.Quantity = mailer.Quantity;
            check.LastEditDate = mailer.LastEditDate;
            check.StatusID = mailer.StatusID;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string DocumentID)
        {
            if (String.IsNullOrEmpty(DocumentID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_MailerDelivery.Find(DocumentID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);
            var detail = db.MM_MailerDeliveryDetail.Find(DocumentID);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.Entry(detail).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}