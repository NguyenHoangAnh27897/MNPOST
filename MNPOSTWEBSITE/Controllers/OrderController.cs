using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using MNPOSTCOMMON;
using System.Web.Script.Serialization;
using MNPOSTWEBSITE.Models;
using MNPOSTWEBSITEMODEL;

namespace MNPOSTWEBSITE.Controllers
{
    public class OrderController : Controller
    {
        MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEEntities();
        // GET: Order
        public ActionResult Create()
        {
            ViewBag.SenderName = db.WS_Mailer.LastOrDefault().SenderName;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string SenderID ="", string SenderName="", string SenderAddress="", string SenderPhone="", string SenderWardID="", string SenderDistrictID="", string SenderProvinceID="", string RecieverName="", string RecieverAddress= "", string RecieverPhone="", string RecieverWardID="", string RecieverDistrictID="", string RecieverProvinceID="", int? Quantity=0, double? Weight=0)
        {
            MNPOSTWEBSITEMODEL.WS_Mailer miler = new MNPOSTWEBSITEMODEL.WS_Mailer
            {
                MailerID = "007",
                SenderName = SenderName,
                SenderAddress = SenderAddress,
                SenderPhone = SenderPhone,
                SenderDistrictID = SenderDistrictID,
                SenderProvinceID = SenderProvinceID,
                SenderWardID = SenderWardID,
                RecieverName = RecieverName,
                RecieverAddress = RecieverAddress,
                RecieverPhone = RecieverPhone,
                RecieverDistrictID = RecieverDistrictID,
                RecieverProvinceID = RecieverProvinceID,
                RecieverWardID = RecieverWardID,
                Weight = Weight,
                Quantity = Quantity
            };
            db.WS_Mailer.Add(miler);
            db.SaveChanges();
            MM_Mailers mailers = new MM_Mailers
            {
                MailerID = "007",
                SenderName =SenderName,
                SenderAddress = SenderAddress,
                SenderPhone = SenderPhone,
                SenderDistrictID = SenderDistrictID,
                SenderProvinceID = SenderProvinceID,
                SenderWardID = SenderWardID,
                RecieverName = RecieverName,
                RecieverAddress = RecieverAddress,
                RecieverPhone = RecieverPhone,
                RecieverDistrictID = RecieverDistrictID,
                RecieverProvinceID = RecieverProvinceID,
                RecieverWardID = RecieverWardID,
                Weight = Weight,
                Quantity = Quantity
            };
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            string api = "http://35.231.147.186:89/api/mailer/addmailer";
            var response = await client.PostAsJsonAsync(api, new { mailer = mailers});
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index","Manage");
            }
            return View();
        }


        public ActionResult OrderList()
        {
            return View();
        }
    }
}