using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MNPOSTCOMMON;

namespace MNPOSTAPI.Controllers.mobile.user
{
    public class UserAPIPresenter : UserAPIContract
    {
        MNPOSTEntities db = new MNPOSTEntities();

        public UserInfoResult GetUserInfo(string user)
        {
            return new UserInfoResult()
            {
                error = 0,
                msg = "",
                FullName = "Hoai Nam"
            };
        }
    }
}