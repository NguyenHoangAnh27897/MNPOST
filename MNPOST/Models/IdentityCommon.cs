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

     
    }

    public class ResultWithPaging : ResultInfo
    {
        public int page { get; set; }

        public int toltalSize { get; set; }

        public int pageSize { get; set; }
    }

    //
    public class CustomerInfoResult
    {
        public string code { get; set; }
        public string name { get; set; }

        public string address { get; set; }

        public string phone { get; set; }

        public string provinceId { get; set; }

        public string districtId { get; set; }

        public string wardId { get; set; }
    }

    public class CommonData
    {
        public string code { get; set; }

        public string name { get; set; }
    }
}