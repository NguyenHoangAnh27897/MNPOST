using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNPOST.Models
{
    public class IdentityCommon
    {
    }

    public class ResultInfo
    {
        public int error { get; set; }

        public string msg { get; set; }

        public Object data { get; set; }

        public int page { get; set; }

        public int toltalSize { get; set; }

        public int pageSize { get; set; }
    }
}