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
        public ActionResult ShowMailer(int? page) => View();

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
        public ActionResult CalBillPrice(float weight = 0, float width = 0, float length = 0, float height = 0, float cod = 0, float goodValue = 0) 
        {
            return Json(new { price = 10000, codPrice = 10000 }, JsonRequestBehavior.AllowGet);
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