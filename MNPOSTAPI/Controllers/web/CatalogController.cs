using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MNPOSTCOMMON;
using MNPOSTAPI.Models;
using System.Data.SqlClient;
namespace MNPOSTAPI.Controllers.web
{
    public class CatalogController : WebBaseController
    {
        [HttpGet]
        public ProvinceInfoResult GetProvince()
        {
            ProvinceInfoResult result = new ProvinceInfoResult()
            {
                error = 0,
                msg = "400-OK",
                provinces = db.BS_Provinces.Where(p=> p.IsActive == true).ToList()
            };

            return result;
        }
        public DistrictInfoResult GetDistrict(string provinceid)
        {
            DistrictInfoResult result = new DistrictInfoResult()
            {
                error = 0,
                msg = "400-OK",
                districts = db.BS_Districts.Where(p => p.IsActive == true && p.ProvinceID == provinceid).ToList()
            };

            return result;
        }
        public WardInfoResult GetWard(string districtid)
        {
            WardInfoResult result = new WardInfoResult()
            {
                error = 0,
                msg = "400-OK",
                wards = db.BS_Wards.Where(p => p.IsActive == true && p.DistrictID == districtid).ToList()
            };

            return result;
        }
    }
}
