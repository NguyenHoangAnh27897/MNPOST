using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MNPOSTCOMMON;

namespace MNPOSTAPI.Controllers.mobile.user
{
    public class UserAPIPresenter
    {
        MNPOSTEntities db = new MNPOSTEntities();

        public UserInfoResult GetUserInfo(string user)
        {

            var checkUser = db.BS_Employees.Where(p => p.UserLogin == user).FirstOrDefault();

            if (checkUser == null)
            {
                return new UserInfoResult()
                {
                    error = 1,
                    msg = "Tài khoản không có quyền truy cập"
                };
            }

            return new UserInfoResult()
            {
                error = 0,
                msg = "",
                FullName = checkUser.EmployeeName,
                EmployeeCode = checkUser.EmployeeID,
                Department = checkUser.DepartmentID
            };
        }
    }
}