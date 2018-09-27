using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNPOSTAPI.Controllers.mobile.user
{
    public interface UserAPIContract
    {
        UserInfoResult GetUserInfo(string user);
    }
}