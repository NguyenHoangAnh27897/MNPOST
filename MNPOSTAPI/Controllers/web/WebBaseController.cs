using MNPOSTCOMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MNPOSTAPI.Controllers.web
{
    public class WebBaseController : ApiController
    {
        protected MNPOSTEntities db = new MNPOSTEntities();


        protected bool checkToken(string token)
        {
            return true;
        }
        protected string generalCusCode()
        {
            return "customer" + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute;
        }
        
    }
}
