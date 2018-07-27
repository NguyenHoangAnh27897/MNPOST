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
            List<CommonData> allMailerType = new List<CommonData>();
            allMailerType.Add(new CommonData()
            {
                code = "CPN",
                name = "Chuyển phát nhanh"
            });
            allMailerType.Add(new CommonData()
            {
                code = "CPT",
                name = "Chuyển phát thường"
            });


            ViewBag.MailerTypes = allMailerType;

            //
            List<CommonData> allPayment = new List<CommonData>();

            allPayment.Add(new CommonData()
            {
                code = "SENDERPAY",
                name = "Người gửi thanh toán"
            });

            allPayment.Add(new CommonData()
            {
                code = "RECEIVERPAY",
                name = "Người nhận thanh toán"
            });

            ViewBag.Payments = allPayment;

            return View();
        }


	}
}