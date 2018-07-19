using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;

namespace MNPOST.Controllers.mnpostinfo
{
    public class ProvinceController : BaseController
    {
        //
        // GET: /Province/
        public ActionResult Show()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetProvince(int? page)
        {
            int pageSize = 50;

            int pageNumber = (page ?? 1);
            

            var data = db.BS_Provinces.ToList();

            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                page = pageNumber,
                pageSize  = pageSize,
                toltalSize = data.Count(),
                data = data.Skip((pageNumber - 1)*pageSize).Take(pageSize).ToList()
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}