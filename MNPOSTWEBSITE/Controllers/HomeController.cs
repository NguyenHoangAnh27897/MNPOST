using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MNPOSTWEBSITE.Models;

namespace MNPOSTWEBSITE.Controllers
{
    public class HomeController : Controller
    {
        MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEEntities();
        public ActionResult Index()
        {
            ViewBag.Slides = db.WS_Slider.Where(s => s.ID == 1).FirstOrDefault();

            var lst = db.WS_ServiceType.ToList();
            return View(lst);
        }


        public async Task<string> getToken()
        {
            string token = "";
            string tokenstr = "";
            IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username","mnwebsite"),
                new KeyValuePair<string, string>("password","mnpost@123"),
                new KeyValuePair<string, string>("grant_type","password")
            };
            HttpContent q = new FormUrlEncodedContent(queries);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync("http://noiboapi.miennampost.vn/mntoken", q).ConfigureAwait(continueOnCapturedContext: false))
                {
                    using (HttpContent content = response.Content)
                    {
                        token = await content.ReadAsStringAsync();
                        HttpContentHeaders headers = content.Headers;
                        var obj = JObject.Parse(token);
                        tokenstr = (string)obj["access_token"];
                        return tokenstr;
                    }
                }
            }
        }
    }
}