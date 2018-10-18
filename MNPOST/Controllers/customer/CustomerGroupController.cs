using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;
namespace MNPOST.Controllers.customer
{
    public class CustomerGroupController : BaseController
    {
        //
        // GET: /CustomerGroup/
        public ActionResult Show()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetCustomerGroup(int? page, string search = "")
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);


            var data = db.BS_CustomerGroups.Where(p => p.CustomerGroupID.Contains(search) || p.CustomerGroupName.Contains(search)).ToList();

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
        public ActionResult create(BS_CustomerGroups customergroup)
        {

            if (String.IsNullOrEmpty(customergroup.CustomerGroupID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.BS_CustomerGroups.Find(customergroup.CustomerGroupID);

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            customergroup.CreationDate = DateTime.Now;
            customergroup.CustomerGroupID = GeneralCusGroupCode();
            db.BS_CustomerGroups.Add(customergroup);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = customergroup }, JsonRequestBehavior.AllowGet);

        }


        private string GeneralCusGroupCode()
        {
            var find = db.GeneralCodeInfoes.Where(p => p.Code == "GCUSTOMER").FirstOrDefault();

            if (find == null)
            {
                var data = new GeneralCodeInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "GCUSTOMER",
                    FirstChar = "",
                    PreNumber = 0
                };

                db.GeneralCodeInfoes.Add(data);
                db.SaveChanges();

                GeneralCusGroupCode();
            }


            var number = find.PreNumber + 1;

            string code = number.ToString();

            int count = 4;

            if (code.Count() < 4)
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

            return code;

        }

      

        [HttpPost]
        public ActionResult edit(BS_CustomerGroups customergroup)
        {
            if (String.IsNullOrEmpty(customergroup.CustomerGroupID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.BS_CustomerGroups.Find(customergroup.CustomerGroupID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.CustomerGroupName = customergroup.CustomerGroupName;
            check.Notes = customergroup.Notes;
            check.LastEditDate = DateTime.Now;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string CustomerGroupID)
        {
            if (String.IsNullOrEmpty(CustomerGroupID))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.BS_CustomerGroups.Find(CustomerGroupID);

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
	}
}