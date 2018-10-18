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
            ViewBag.AllCustomerGroup = db.BS_CustomerGroups.ToList();
            ViewBag.AllDistrict = db.BS_Districts.ToList();
            ViewBag.AllProvince = db.BS_Provinces.ToList();
            ViewBag.AllCountry = db.BS_Countries.ToList();
            ViewBag.AllPostOffice = db.BS_PostOffices.ToList();
            return View();
        }
        private string GeneralCusCode(string groupId)
        {

            var find = db.GeneralCodeInfoes.Where(p => p.Code == "CUSTOMER").FirstOrDefault();

            if (find == null)
            {
                var data = new GeneralCodeInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "CUSTOMER",
                    FirstChar = groupId,
                    PreNumber = 0
                };

                db.GeneralCodeInfoes.Add(data);
                db.SaveChanges();

                GeneralCusCode(groupId);
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
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.BS_Customers.Where(p => p.CustomerID.Contains(search) || p.CustomerName.Contains(search)).ToList();

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
        public ActionResult create(BS_Customers cus)
        {

            var check = db.BS_Customers.Find(cus.CustomerCode);

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);


            var checkGroup = db.BS_CustomerGroups.Find(cus.CustomerGroupID);

            if (checkGroup == null)
                return Json(new ResultInfo() { error = 1, msg = "Sai mã nhóm" }, JsonRequestBehavior.AllowGet);

            cus.CustomerCode = GeneralCusCode(cus.CustomerGroupID);

            cus.CustomerID = Guid.NewGuid().ToString();
            cus.CreateDate = DateTime.Now;
            cus.IsActive = true;
            db.BS_Customers.Add(cus);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = cus }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(BS_Customers cus)
        {
            if (String.IsNullOrEmpty(cus.CustomerID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.BS_Customers.Find(cus.CustomerID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);
            check.CustomerID = cus.CustomerID;
            check.CustomerName = cus.CustomerName;
            check.CustomerType = cus.CustomerType;
            check.CustomerGroupID = cus.CustomerGroupID;
            check.Address = cus.Address;
            check.DistrictID = cus.DistrictID;
            check.ProvinceID = cus.ProvinceID;
            check.CountryID = cus.CountryID;
            check.FaxNo = cus.FaxNo;
            check.Email = cus.Email;
            check.LastEditDate = DateTime.Now;
            check.Phone = cus.Phone;
            check.CompanyPhone = cus.CompanyPhone;
            check.Mobile = cus.Mobile;
            check.PersonalInfo = cus.PersonalInfo;
            check.BankAccount = cus.BankAccount;
            check.BankName = cus.BankName;
            check.TaxCode = cus.TaxCode;
            check.PostOfficeID = cus.PostOfficeID;
            check.IsActive = cus.IsActive;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
	}
}