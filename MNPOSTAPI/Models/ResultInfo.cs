using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNPOSTAPI.Models
{
    public class ResultInfo
    {
        public int error { get; set; }

        public string msg { get; set; }
    }

    public class ResponseInfo : ResultInfo
    {

        public Object data { get; set; }
    }

}