using MNPOSTCOMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOST.Models;
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;

namespace MNPOST.Controllers.mailer
{
    public class MailerController : BaseController
    {
        protected MNHistory HandleHistory = new MNHistory();


        [HttpGet]
        public ActionResult ShowMailer()
        {
            ViewBag.PostOffices = EmployeeInfo.postOffices;

            ViewBag.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.FromDate = DateTime.Now.ToString("dd/MM/yyyy");

            return View();
        }

        [HttpPost]
        public JsonResult GetMailers(int? page, string search, string fromDate, string toDate, int status, string postId)
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);

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

            var data = db.MAILER_GETALL(paserFromDate.ToString("yyyy-MM-dd"), paserToDate.ToString("yyyy-MM-dd"), "%" + postId + "%", "%" + search + "%").ToList();

            if(status != -1)
            {
                data = data.Where(p => p.CurrentStatusID == status).ToList();
            }

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
       public ActionResult GetTracking(string mailerId)
        {
            var data = db.MAILER_GETTRACKING(mailerId).ToList();

            return Json(new ResultInfo()
            {
                data = data,
                error = 0
            }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult ShowReportMailer(string mailers)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@mailers", mailers);
            var sqlAdapter = GetSqlDataAdapter("MAILER_GETINFO_BYLISTID", parameters);

            var reportViewer = GetReportViewer(sqlAdapter, ds.MAILER_GETINFO_BYID.TableName, "MAILERINFO", "mailer.rdlc");
            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public string GeneralMailerCode(string postId)
        {
            var post = db.BS_PostOffices.Where(p => p.PostOfficeID == postId).FirstOrDefault();

            if(post == null)
            {
                return "";
            }

            var charFirst = post.AreaChar + DateTime.Now.ToString("ddMMyy");
            var codeSearch = "mailer" + post.AreaChar;

            var find = db.GeneralCodeInfoes.Where(p=> p.Code == codeSearch && p.FirstChar == charFirst).FirstOrDefault();

            if (find == null)
            {
                var generalCode = new GeneralCodeInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    PreNumber = 0,
                    FirstChar = charFirst,
                    Code = codeSearch
                };
                db.GeneralCodeInfoes.Add(generalCode);
                db.SaveChanges();

                return GeneralMailerCode(postId);
            }

            var number = find.PreNumber + 1;

            string code = number.ToString();
            int count = 5;
            if (code.Count() < 5)
            {
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
        public JsonResult CalBillPrice(float weight = 0, string customerId = "", string provinceId = "",string serviceTypeId = "", string postId = "", float cod = 0, float merchandiseValue = 0) 
        {

            var price = db.CalPrice(weight, customerId, provinceId, serviceTypeId, postId, DateTime.Now.ToString("yyyy-MM-dd")).FirstOrDefault();

            return Json(new { price = price, codPrice = 0 }, JsonRequestBehavior.AllowGet);
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