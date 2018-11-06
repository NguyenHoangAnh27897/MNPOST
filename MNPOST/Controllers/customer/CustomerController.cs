using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;
namespace MNPOST.Controllers.customer
{
    public class CustomerController : BaseController
    {
        public ActionResult Show()
        {
            ViewBag.Provinces = GetProvinceDatas("", "province");
            ViewBag.AllPostOffice = db.BS_PostOffices.Select(p => new
            {
                code = p.PostOfficeID,
                name = p.PostOfficeName
            }).ToList();

            return View();
        }
        private string GeneralCusCode(string groupId)
        {
            string codeSearch = "CUSTOMER" + groupId;
            var find = db.GeneralCodeInfoes.Where(p => p.Code == codeSearch).FirstOrDefault();

            if (find == null)
            {
                var data = new GeneralCodeInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = codeSearch,
                    FirstChar = groupId,
                    PreNumber = 0
                };

                db.GeneralCodeInfoes.Add(data);
                db.SaveChanges();

                return GeneralCusCode(groupId);
            }

            var number = find.PreNumber + 1;

            string code = number.ToString();

            int count = 2;

            if (code.Count() < 2)
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

            return groupId + code;

        }

        [HttpGet]
        public ActionResult GetCustomer(int? page, string search = "")
        {
            var data = db.CUSTOMER_GET_BYGROUP(search).ToList();

            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Active(string cusId, bool isActive)
        {
            var find = db.BS_Customers.Find(cusId);

            if(find != null)
            {
                find.IsActive = isActive;
                db.Entry(find).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new ResultInfo()
            {
                error = 0

            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(CustomerInfo cus)
        {

            var checkGroup = db.BS_CustomerGroups.Where(p => p.CustomerGroupCode == cus.CustomerGroupCode).FirstOrDefault();

            if (checkGroup == null)
                return Json(new ResultInfo() { error = 1, msg = "Sai mã nhóm" }, JsonRequestBehavior.AllowGet);
            var code = GeneralCusCode(checkGroup.CustomerGroupCode);
            var ins = new BS_Customers()
            {
                CustomerID = Guid.NewGuid().ToString(),
                CustomerName = cus.CustomerName,
                CountryID = "VN",
                Address = cus.Address,
                CreateDate = DateTime.Now,
                CustomerCode = code,
                CustomerGroupID = checkGroup.CustomerGroupID,
                Deputy = cus.Deputy,
                DistrictID = cus.DistrictID,
                Email = cus.Email,
                IsActive = true,
                Phone = cus.Phone,
                PostOfficeID = cus.PostOfficeID,
                ProvinceID = cus.ProvinceID,
                UserLogin = "",
                WardID = cus.WardID
            };

          
            db.BS_Customers.Add(ins);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = checkGroup.CustomerGroupCode }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Edit(CustomerInfo cus)
        {
            var check = db.BS_Customers.Find(cus.CustomerID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            var checkGroup = db.BS_CustomerGroups.Where(p => p.CustomerGroupCode == cus.CustomerGroupCode).FirstOrDefault();

            if (checkGroup == null)
                return Json(new ResultInfo() { error = 1, msg = "Sai mã nhóm" }, JsonRequestBehavior.AllowGet);

            check.CustomerName = cus.CustomerName;
            check.CustomerGroupID = cus.CustomerGroupID;
            check.Address = cus.Address;
            check.DistrictID = cus.DistrictID;
            check.ProvinceID = cus.ProvinceID;
            check.CountryID = cus.CountryID;
            check.Email = cus.Email;
            check.Phone = cus.Phone;
            check.PostOfficeID = cus.PostOfficeID;
            check.Deputy = cus.Deputy;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = ""}, JsonRequestBehavior.AllowGet);

        }
	}
}