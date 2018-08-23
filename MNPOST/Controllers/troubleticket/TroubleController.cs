using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;
namespace MNPOST.Controllers.troubleticket
{
    public class TroubleController : BaseController
    {
        // GET: Trouble
        public ActionResult Show()
        {
            ViewBag.AllEmployee = db.BS_Employees.Where(p=> p.IsActive == true).ToList();
            ViewBag.AllPostOffice = db.BS_PostOffices.ToList();
           // ViewBag.AllStatus = db.BS_Status.Where(p => p.Type == "T");
            ViewBag.AllCustomer = db.BS_Customers.Where(p => p.IsActive == true).ToList();
            return View();
        }


        [HttpGet]
        public ActionResult GetTrouble(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.MM_TroubleTickets.Where(p => p.TicketID.Contains(search) || p.TicketName.Contains(search)).ToList();

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
        public ActionResult create(MM_TroubleTickets trouble)
        {

            if (String.IsNullOrEmpty(trouble.TicketID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_TroubleTickets.Find(trouble.TicketID);

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            trouble.CreationDate = DateTime.Now;
            trouble.LastUpdateDate = DateTime.Now;
            db.MM_TroubleTickets.Add(trouble);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = trouble }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult edit(MM_TroubleTickets trouble)
        {
            if (String.IsNullOrEmpty(trouble.TicketID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_TroubleTickets.Find(trouble.TicketID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.TicketName = trouble.TicketName;
            check.TicketDate = trouble.TicketDate;
            check.EmployeeID = trouble.EmployeeID;
            check.PostOfficeID = trouble.PostOfficeID;
            check.StatusID = trouble.StatusID;
            check.LastUpdateDate = DateTime.Now;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string ticketid)
        {
            if (String.IsNullOrEmpty(ticketid))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.MM_TroubleTickets.Find(ticketid);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}