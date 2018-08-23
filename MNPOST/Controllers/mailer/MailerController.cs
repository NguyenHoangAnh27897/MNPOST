using MNPOSTCOMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOST.Models;

namespace MNPOST.Controllers.mailer
{
    public class MailerController : BaseController
    {

        [HttpGet]
        public ActionResult ShowMailer()
        {
            ViewBag.PostOffices = EmployeeInfo.postOffices;

            ViewBag.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.FromDate = DateTime.Now.ToString("dd/MM/yyyy");

            return View();
        }

        [HttpPost]
        public ActionResult GetMailers(int? page, string search, string fromDate, string toDate, string customer, string postId)
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

            if (!CheckPostOffice(postId))
                return Json(new { error = 1, msg = "Không phải bưu cục" }, JsonRequestBehavior.AllowGet);

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

            var data = db.MAILER_GETALL(paserFromDate.ToString("yyyy-MM-dd"), paserToDate.ToString("yyyy-MM-dd"), "%" + postId + "%", "%" + customer + "%").ToList();

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


        public ActionResult GetCustomers(string postId)
        {
            var data = db.BS_Customers.Where(p => p.PostOfficeID == postId).Select(p => new CommonData() { name = p.CustomerName, code = p.CustomerCode }).ToList();

            return Json(new ResultInfo() { error = 0, msg = "", data = data }, JsonRequestBehavior.AllowGet);

        }

        public string GeneralMailerCode(string cusId)
        {

            var find = db.GeneralCodeInfoes.Where(p=> p.Id == "mailer" && p.FirstChar == "MN").FirstOrDefault();

            if (find == null)
            {
                var generalCode = new GeneralCodeInfo()
                {
                    Id = "mailer",
                    PreNumber = 0,
                    FirstChar = "MN"
                };
                db.GeneralCodeInfoes.Add(generalCode);
                db.SaveChanges();

                return GeneralMailerCode(cusId);
            }

            var number = find.PreNumber + 1;

            string code = number.ToString();
            int count = 6;
            if (code.Count() < 6)
            {

                // quy dinh chi 6 ki tu

                count = count - code.Count();

                while (count > 0)
                {
                    code = "0" + code;
                    count--;
                }
            }

            find.PreNumber = find.PreNumber + 1;
            db.Entry(find).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return find.FirstChar + code;

        }

        [HttpPost]
        public ActionResult CalBillPrice(float weight = 0, float volume = 0, float cod = 0, float merchandiseValue = 0) 
        {
            return Json(new { price =0, codPrice = 0 }, JsonRequestBehavior.AllowGet);
        }

        protected bool CheckPostOffice(string postId)
        {
            var check = db.BS_PostOffices.Find(postId);

            return check == null ? false : true;
        }

        protected List<EmployeeInfoCommon> GetEmployeeByPost(string postId)
        {
            return db.BS_Employees.Where(p => p.PostOfficeID == postId && p.IsActive == true).Select(p => new EmployeeInfoCommon()
            {
                code = p.EmployeeID,
                name = p.EmployeeName,
                email = p.Email,
                phone = p.Phone
            }).ToList();
        }
    }
}