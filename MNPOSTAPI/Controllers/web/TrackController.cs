using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MNPOSTCOMMON;
using MNPOSTAPI.Models;

namespace MNPOSTAPI.Controllers.web
{
    public class TrackController : ApiController
    {
        MNPOSTEntities db = new MNPOSTEntities();

        [HttpGet]
        public ResultInfo Find(string mailerId)
        {
            var data = db.MAILER_GETTRACKING(mailerId).ToList();

            return new ResponseInfo()
            {
                data = data,
                error = 0
            };
        }
    }
}
