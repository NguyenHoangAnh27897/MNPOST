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
            var customers = db.BS_Customers.Select(p => new {
                name = p.CustomerName,
                code = p.CustomerCode,
                address = p.Address,
                phone = p.Phone
            }).ToList();

            // dịch vu

            ViewBag.MailerTypes = db.BS_ServiceTypes.Select(p => new CommonData()
            {
                code = p.ServiceID,
                name = p.ServiceName
            }).ToList();

            // hinh thuc thanh toan
            ViewBag.Payments = db.CDatas.Where(p => p.CType == "MAILERPAY").Select(p => new CommonData() { code = p.Code, name = p.Name }).ToList();


            // danh sach phu phi
            ViewBag.Services = db.BS_Services.Select(p => new ItemPriceCommon()
            {
                code = p.ServiceID,
                name = p.ServiceName,
                price = p.Price,
                choose = false

            }).ToList(); ;

            // tinh thanh
            ViewBag.Provinces = GetProvinceDatas("", "province");

            ViewBag.AllCustomer = customers;

            return View();
        }

        [HttpGet]
        public ActionResult SearchMailer()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetMailers(int? page, string search, string fromDate, string toDate, int status, string postId, string customerId)
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

            var data = db.MAILER_GETALL(paserFromDate.ToString("yyyy-MM-dd"), paserToDate.ToString("yyyy-MM-dd"), "%" + postId + "%", "%" + search + "%").Where(p=> p.SenderID.Contains(customerId)).ToList();

            if (status != -1)
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
        public ActionResult FindMailer(string mailerId)
        {
            var mailer = db.MAILER_GETINFO_BYID(mailerId).FirstOrDefault();

            if (mailer == null)
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Không tìm thấy"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = db.MAILER_GETTRACKING(mailerId).ToList();
                var images = db.MailerImages.Where(p => p.MailerID == mailerId).Select(p => new {
                        url = p.PathImage,
                        time = p.CreateTime.Value.ToString("dd/MM/yyyy HH:mm")
                });
                return Json(new ResultInfo()
                {
                    error = 0,
                    data = new
                    {
                        mailer = mailer,
                        tracks = data
                    }

                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetTracking(string mailerId)
        {
            var data = db.MAILER_GETTRACKING_BY_MAILERID(mailerId).ToList();

            return Json(new ResultInfo()
            {
                data = data,
                error = 0
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalBillPrice(float weight = 0, string customerId = "", string provinceId = "", string serviceTypeId = "", string postId = "", float cod = 0, float merchandiseValue = 0)
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