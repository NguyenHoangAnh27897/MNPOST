using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;
namespace MNPOST.Controllers.mailer
{
    public class MailerDeliveryConfirmController : BaseController
    {
        // GET: MailerDeliveryConfirm
        public ActionResult Show()
        {
           // ViewBag.AllStatus = db.BS_Status.Where(p=>p.Type =="P" && p.IsActive == true ).ToList();//load du lieu sang combobox
            ViewBag.AllReason = db.BS_ReturnReasons.Where(p=>p.IsActive == true).ToList();
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
        public ActionResult edit(MM_MailerDeliveryDetail mailer)
        {
            if (String.IsNullOrEmpty(mailer.DocumentID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_MailerDeliveryDetail.Find(mailer.MailerID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.DeliveryTo = mailer.DeliveryTo;
            check.DeliveryDate = mailer.DeliveryDate;
            check.DeliveryStatus = mailer.DeliveryStatus;
            check.DeliveryNotes = mailer.DeliveryNotes;
            check.LastEditDate = mailer.LastEditDate;
            check.ReturnReasonID = mailer.ReturnReasonID;
            check.ConfirmUserID = mailer.ConfirmUserID;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
       
    }
}