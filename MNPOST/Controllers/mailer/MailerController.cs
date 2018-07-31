using MNPOSTCOMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MNPOST.Controllers.mailer
{
    public class MailerController : BaseController
    {

        [HttpGet]
        public ActionResult Show(int? page)
        {



            return View();
        }


        public string generalMailerCode(string cusId)
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

                return generalMailerCode(cusId);
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

	}
}