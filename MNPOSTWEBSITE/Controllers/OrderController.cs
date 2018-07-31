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

namespace MNPOSTWEBSITE.Controllers
{
    public class OrderController : Controller
    {

        // GET: Order
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string SenderID ="", string SenderName="", string SenderAddress="", string SenderPhone="", string SenderWardID="", string SenderDistrictID="", string SenderProvinceID="", string RecieverName="", string RecieverAddress="", string RecieverPhone="", string RecieverWardID="", string RecieverDistrictID="", string RecieverProvinceID="")
        {
            MM_Mailers mailers = new MM_Mailers
            {
                MailerID = "00007",
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
                RecieverWardID = RecieverWardID
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