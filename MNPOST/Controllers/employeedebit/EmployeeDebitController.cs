using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;
namespace MNPOST.Controllers.employeedebit
{
    public class EmployeeDebitController : BaseController
    {
        // GET: EmployeeDebit
        public ActionResult Show()
        {
            ViewBag.PostOffices = db.BS_PostOffices.ToList();
            ViewBag.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.FromDate = DateTime.Now.ToString("dd/MM/yyyy");
            return View();
        }
        [HttpGet]
        public ActionResult getEmployeeDebit(int? page, string fromDate, string toDate, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);
            if (String.IsNullOrEmpty(fromDate) || String.IsNullOrEmpty(toDate))
            {
                fromDate = DateTime.Now.ToString("dd/MM/yyyy");
                toDate = DateTime.Now.ToString("dd/MM/yyyy");
            }

            DateTime paserFromDate = DateTime.Now;
            DateTime paserToDate = DateTime.Now;

            try
            {
                paserFromDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                paserToDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
            }
            catch
            {
                paserFromDate = DateTime.Now;
                paserToDate = DateTime.Now;
            }

            var data = db.MM_EmployeeDebitVoucher.Where(p => p.DocumentDate >= paserFromDate.Date && p.DocumentDate <= paserToDate.Date).ToList();

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
        [HttpGet]
        public ActionResult getDebitdetail(string DocumentID)
        {
            var coddetail = db.MM_EmployeeDebitVoucherDetails.Where(p => p.DocumentID == DocumentID).ToList();

            List<IdentityCOD> codInfos = new List<IdentityCOD>();


            foreach (var item in coddetail)
            {
                codInfos.Add(new IdentityCOD()
                {
                    MailerID = item.MailerID,
                    COD = item.COD,
                    ReciveCOD = item.ReciveCOD
                });
            }
            return Json(new ResultInfo() { error = 0, msg = "", data = codInfos }, JsonRequestBehavior.AllowGet);
        }

        // update detail
        [HttpPost]
        public ActionResult UpdateDebitDetail(List<IdentityCOD> detail, string documentID)
        {
            foreach (var item in detail)
            {
                if (String.IsNullOrEmpty(item.MailerID))
                    return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

                var check = db.MM_EmployeeDebitVoucherDetails.Where(p => p.DocumentID == documentID && p.MailerID == item.MailerID).FirstOrDefault();

                if (check == null)
                    return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

                check.ReciveCOD = item.ReciveCOD;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            }
           
            db.SaveChanges();
            return Json(new ResultInfo() { }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(MM_EmployeeDebitVoucher remon)
        {
            if (String.IsNullOrEmpty(remon.DocumentID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_EmployeeDebitVoucher.Find(remon.DocumentID);

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

            var check = db.MM_EmployeeDebitVoucher.Find(DocumentID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}