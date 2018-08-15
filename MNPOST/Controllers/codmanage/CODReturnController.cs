using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;
namespace MNPOST.Controllers.codmanage
{
    public class CODReturnController : BaseController
    {
        // GET: CODReturn
        public ActionResult Show()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetCODReturn(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.MM_RecieveMoney.Where(p => p.DocumentID.Contains(search) || p.DocumentNumber.Contains(search)).ToList();

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
        public ActionResult create(MM_RecieveMoney remon)
        {

            if (String.IsNullOrEmpty(remon.DocumentID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_RecieveMoney.Find(remon.DocumentID);

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            db.MM_RecieveMoney.Add(remon);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = remon }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult edit(MM_RecieveMoney remon)
        {
            if (String.IsNullOrEmpty(remon.DocumentID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_RecieveMoney.Find(remon.DocumentID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.DocumentID = remon.DocumentID;
            check.DocumentNumber = remon.DocumentNumber;
            check.DocumentDate = remon.DocumentDate;
            check.MoneyColector = remon.MoneyColector;
            check.EmployeeID = remon.EmployeeID;
            check.MailerAccount = remon.MailerAccount;
            check.TotalMoney = remon.TotalMoney;
            check.PostOfficeID = remon.PostOfficeID;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string DocumentID)
        {
            if (String.IsNullOrEmpty(DocumentID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_RecieveMoney.Find(DocumentID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
    
}