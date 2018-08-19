using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MNPOST.Models;
using System.Web.Mvc;
using MNPOSTCOMMON;

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

            //
            ViewBag.PostOffices = EmployeeInfo.postOffices;

            // danh sach tinh thanh
            List<CommonData> allProvince = GetProvinceDatas("", "province");
            ViewBag.Provinces = allProvince;


            return View();
        }

        [HttpPost]
        public ActionResult InsertMailers(List<MailerIdentity> mailers, string postId)
        {
            
            if(mailers == null)
                return Json(new { error = 1, msg = "Hoàn thành" }, JsonRequestBehavior.AllowGet);

            if (mailers.Count() > 100)
                return Json(new {error = 1, msg = "Để đảm bảo hệ thống chỉ update 100/1 lần"}, JsonRequestBehavior.AllowGet);

            var checkPost = db.BS_PostOffices.Find(postId);

            if (checkPost == null)
                return Json(new {error = 1, msg = "chọn bưu cục" }, JsonRequestBehavior.AllowGet);

            List<MailerIdentity> insertFail = new List<MailerIdentity>();

            foreach(var item in mailers)
            {
                // checkMailer
                if (String.IsNullOrEmpty(item.MailerID))
                {
                    insertFail.Add(item);
                    continue;
                }

                var checkExist = db.MM_Mailers.Where(p => p.MailerID == item.MailerID).FirstOrDefault();

                if (checkExist != null)
                {
                    insertFail.Add(item);
                    continue;
                }

                // theem
                var mailerIns = new MM_Mailers()
                {
                    MailerID = item.MailerID,
                    AcceptTime = DateTime.Now,
                    AcceptDate = DateTime.Now,
                    COD = item.COD,
                    CreationDate = DateTime.Now,
                    CurrentStatusID = 1,
                    HeightSize = item.HeightSize,
                    Weight = item.Weight,
                    LengthSize = item.LengthSize,
                    WidthSize = item.WidthSize,
                    Quantity = item.Quantity,
                    PostOfficeAcceptID = postId,
                    CurrentPostOfficeID = postId,
                    EmployeeAcceptID = EmployeeInfo.employeeId,
                    MailerDescription = item.MailerDescription,
                    MailerTypeID = item.MailerTypeID,
                    MerchandiseValue = item.MerchandiseValue,
                    MerchandiseID = item.MerchandiseID,
                    PriceDefault = item.PriceDefault,
                    Price = item.PriceDefault,
                    PriceService = item.PriceService,
                    Notes = item.Notes,
                    PaymentMethodID = item.PaymentMethodID,
                    RecieverAddress = item.RecieverAddress,
                    RecieverName = item.RecieverName,
                    RecieverPhone = item.RecieverPhone,
                    RecieverDistrictID = item.RecieverDistrictID,
                    RecieverWardID = item.RecieverWardID,
                    RecieverProvinceID = item.RecieverProvinceID,
                    SenderID = item.SenderID,
                    SenderAddress = item.SenderAddress,
                    SenderDistrictID = item.SenderDistrictID,
                    SenderName = item.SenderName,
                    SenderPhone = item.SenderPhone,
                    SenderProvinceID = item.SenderProvinceID,
                    SenderWardID = item.SenderWardID
                };

                // 
                db.MM_Mailers.Add(mailerIns);
                db.SaveChanges();
            }


            return Json(new { error = 0, data = insertFail}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetProvinces(string parentId, string type)
        {
            return Json(GetProvinceDatas(parentId, type), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDistrictAndWard(string provinceId, string districtId)
        {
            return Json(new { districts = GetProvinceDatas(provinceId, "district"), wards = GetProvinceDatas(districtId, "ward")}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GeneralCode(string cusId)
        {
            var code = GeneralMailerCode(cusId);

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