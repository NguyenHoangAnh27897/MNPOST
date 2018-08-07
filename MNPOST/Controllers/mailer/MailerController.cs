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


        public List<CommonData> GetProvinceDatas(string parentId, string type)
        {
            if (type == "district")
            {
                return db.BS_Districts.Where(p => p.ProvinceID == parentId).Select(p => new CommonData()
                {
                    code = p.DistrictID,
                    name = p.DistrictName
                }).ToList();
            } else if (type == "ward")
            {
                return db.BS_Wards.Where(p => p.DistrictID == parentId).Select(p => new CommonData()
                {
                    code = p.WardID,
                    name = p.WardName
                }).ToList();
            } else if (type == "province")
            {
                return db.BS_Provinces.Select(p => new CommonData()
                {
                    code = p.ProvinceID,
                    name = p.ProvinceName
                }).ToList();
            }
            else
            {
                return new List<CommonData>();
            }

        }

        [HttpPost]
        public ActionResult CalBillPrice(float weight = 0, float width = 0, float length = 0, float height = 0, float cod = 0, float goodValue = 0) 
        {
            return Json(new { price = 10000, codPrice = 10000 }, JsonRequestBehavior.AllowGet);
        }

    }
}