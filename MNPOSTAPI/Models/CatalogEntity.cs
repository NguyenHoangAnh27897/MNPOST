using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MNPOSTCOMMON;
namespace MNPOSTAPI.Models
{
    public class ProvinceInfoResult : ResultInfo
    {
        public List<BS_Provinces> provinces { get; set; }
    }
    public class DistrictInfoResult : ResultInfo
    {
        public List<BS_Districts> districts { get; set; }
    }
    public class WardInfoResult : ResultInfo
    {
        public List<BS_Wards> wards { get; set; }
    }
}