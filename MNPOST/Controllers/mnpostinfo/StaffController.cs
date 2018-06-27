using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using MNPOSTCOMMON;
using MNPOST.Models;
using System.Threading.Tasks;

namespace MNPOST.Controllers.mnpostinfo
{
    public class StaffController : BaseController
    {

        // code : staff
        public ActionResult Show(int? page, string search = "", string post = "", string msg = "")
        {
            if (!checkAccess("staff"))
                return Redirect("/error/relogin");

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            ViewBag.MSG = msg;

            ViewBag.AllPosition = db.BS_Positions.ToList();

            ViewBag.AllPost = db.BS_PostOffices.ToList();

            ViewBag.GroupStaff = db.UMS_UserGroups.ToList();


            var data = db.EMPLOYEE_GETALL("%" + post + "%", "%" + search + "%").ToList();



            return View(data.ToPagedList(pageNumber, pageSize));
        }


        [HttpPost]
        public ActionResult AddStaff(BS_Employees employee)
        {
            if (!checkAccess("staff", 1))
                return Redirect("/error/relogin");

            var code = generalCode();

            if (code == "")
                RedirectToAction("show", "staff", new { msg = "Loi tao nhan vien" });

            employee.EmployeeID = code;
            employee.IsActive = true;

            employee.CreationDate = DateTime.Now;

            db.BS_Employees.Add(employee);

            db.SaveChanges();

            return RedirectToAction("show", "staff", new { msg = "Da tao nhan vien " + code , search = "StaffCode"});

        }

        private string generalCode()
        {
            var find = db.GeneralCodeInfoes.Find("staff");

            if (find == null)
                return "";

            var number = find.PreNumber + 1;

            string code = number.ToString();
            int count = 4;
            if (code.Count() < 4)
            {

                // quy dinh chi 4 ki tu

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


        // tao tk
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        protected async Task<ActionResult> Register(string UserName, string Password, string GroupId, string StaffCode)
        {

            var findStaff = db.BS_Employees.Find(StaffCode);

            if (findStaff == null)
                return Redirect("/error");

            var user = new ApplicationUser()
            {
                UserName = UserName,
                FullName = findStaff.EmployeeName,
                GroupId = GroupId,
                IsActivced = 1,
                AccountType = "user"
            };

          


            var result = await UserManager.CreateAsync(user, Password);


            if (result.Succeeded)
            {

                findStaff.UserLogin = UserName;
                db.Entry(findStaff).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                await UserManager.AddToRoleAsync(user.Id, "user");

                return RedirectToAction("show", "staff", new { msg = "Da tao tai khoan " + UserName, search = "StaffCode"});
            }
            else
            {
                return RedirectToAction("show", "staff", new { msg = "Khong tao duoc tk " + UserName, search = "StaffCode" });
            }

        }
    }
}