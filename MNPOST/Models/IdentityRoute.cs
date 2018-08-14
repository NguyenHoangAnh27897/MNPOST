using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNPOST.Models
{
    public class ERouteInfo
    {
        public string DistrictID { get; set; }

        public string DistrictName { get; set; }

        public bool? IsDetail { get; set; }

        public string RouteID { get; set; }

        public string Type { get; set; }
    }
}