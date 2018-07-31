using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MNPOST.Models;
using System.Web.Mvc;

namespace MNPOST.Controllers.mailer
{
    public class MailerInitController : MailerController
    {

        [HttpGet]
        public ActionResult Init()
        {
            var allCus = db.BS_Customers.ToList();

            List<CustomerInfoResult> cusResult = new List<CustomerInfoResult>();

            foreach (var item in allCus)
            {
                cusResult.Add(new CustomerInfoResult()
                {
                    code = item.CustomerCode,
                    name = item.CustomerName,
                    phone = item.Phone,
                    provinceId = item.ProvinceID,
                    address = item.Address,
                    districtId = item.DistrictID,
                    wardId = item.WardID
                });
            }

            ViewBag.Customers = cusResult;

            //
            List<CommonData> allMailerType = db.BS_ServiceTypes.Select(p=> new CommonData()
            {
                code = p.ServiceID,
                name = p.ServiceName
            }).ToList();

            ViewBag.MailerTypes = allMailerType;

            //
            List<CommonData> allPayment = new List<CommonData>();

            allPayment.Add(new CommonData()
            {
                code = "NGTT",
                name = "Người gửi thanh toán"
            });

            allPayment.Add(new CommonData()
            {
                code = "NNTT",
                name = "Người nhận thanh toán"
            });

            ViewBag.Payments = allPayment;


            //
            List<ItemPriceCommon> allServices = db.BS_Services.Select(p => new ItemPriceCommon()
            {
                code = p.ServiceID,
                name = p.ServiceName,
                price = p.Price,
                choose = false

            }).ToList();

            ViewBag.Services = allServices;

            return View();
        }



        [HttpGet]
        public ActionResult GeneralCode(string cusId)
        {
            var code = generalMailerCode(cusId);

            return Json(new {error = 0, code = code}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetProvinceFromAddress(string province, string district, string ward)
        {
            var findProvince = db.BS_Provinces.Where(p => p.ProvinceName == province).FirstOrDefault();

            if (findProvince == null)
                return Json(new { provinceId = "", districtId = "", wardId = "" }, JsonRequestBehavior.AllowGet);


            var findDistrict = db.BS_Districts.Where(p => p.ProvinceID == findProvince.ProvinceID && p.DistrictName == district).FirstOrDefault();

            if (findDistrict == null)
                return Json(new { provinceId = findProvince.ProvinceID, districtId = "", wardId = "" }, JsonRequestBehavior.AllowGet);


            var findWard = db.BS_Wards.Where(p => p.DistrictID == findDistrict.DistrictID && p.WardName == ward).FirstOrDefault();

            if (findWard == null)
                return Json(new { provinceId = findProvince.ProvinceID, districtId = findDistrict.DistrictID, wardId = "" }, JsonRequestBehavior.AllowGet);


            return Json(new { provinceId = findProvince.ProvinceID, districtId = findDistrict.DistrictID, wardId = findWard.WardID }, JsonRequestBehavior.AllowGet);
        }

	}
}