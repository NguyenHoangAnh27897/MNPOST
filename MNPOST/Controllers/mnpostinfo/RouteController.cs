using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;
using MNPOST.Models;
namespace MNPOST.Controllers.mnpostinfo
{
    public class RouteController : BaseController
    {
        // GET: Route
        public ActionResult Show()
        {
            ViewBag.AllProvince = db.BS_Provinces.Select(p=>new CommonData() { code = p.ProvinceID, name = p.ProvinceName}).ToList();
            ViewBag.AllPost = EmployeeInfo.postOffices;
            return View();
        }

        //
        [HttpGet]
        public ActionResult GetProvinces(string parentId, string type)
        {
            return Json(GetProvinceDatas(parentId, type), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEmployeeRoutes(string postId)
        {
            var data = db.ROUTE_GET_ALLEMPLOYEE_ROUTE(postId).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDistrictRoutes(string provinceId, string employeeId, string type)
        {
            var routes = db.BS_Routes.Where(p => p.ProvinceID == provinceId && p.EmployeeID == employeeId && p.Type == type).ToList();

            List<ERouteInfo> routeInfos = new List<ERouteInfo>();

            var districts = GetProvinceDatas(provinceId, "district");

            foreach(var item in routes)
            {
                var findDistrict = districts.Where(p => p.code == item.DistrictID).FirstOrDefault();

                if(findDistrict != null)
                {

                    routeInfos.Add(new ERouteInfo()
                    {
                        RouteID = item.RouteID,
                        DistrictID = item.DistrictID,
                        DistrictName = findDistrict.name,
                        IsDetail = item.IsDetail
                    });

                    districts.Remove(findDistrict);

                }
            }


            return Json(new {routes = routeInfos, districts = districts }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult AddDistrictRoutes(string provinceId, string employeeId, string type, string district)
        {
            var check = db.BS_Routes.Where(p => p.ProvinceID == provinceId && p.EmployeeID == employeeId && p.Type == type && p.DistrictID == district).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() {error = 1, msg = "Đã tạo" }, JsonRequestBehavior.AllowGet);

            var routes = new BS_Routes()
            {
                RouteID = Guid.NewGuid().ToString(),
                DistrictID = district,
                EmployeeID = employeeId,
                IsDetail = true,
                ProvinceID = provinceId,
                Type = type
            };

            db.BS_Routes.Add(routes);
            db.SaveChanges();

            var routesInfo = new ERouteInfo()
            {
                RouteID = routes.RouteID,
                DistrictID = routes.DistrictID,
                IsDetail = routes.IsDetail
            };

            return Json(new ResultInfo() {error = 0, msg = "", data = routesInfo }, JsonRequestBehavior.AllowGet);

        }
    }
}