using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ASPSnippets.FaceBookAPI;
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
        MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities();
        public ActionResult Index()
        {
            //FaceBookConnect.API_Key = "273429170069624";
            //FaceBookConnect.API_Secret = "01dda29a1052ab142ad14ca4d208d48b";

            //MNPOSTWEBSITEMODEL.WS_FacebookUser faceBookUser = new MNPOSTWEBSITEMODEL.WS_FacebookUser();
            //if (Request.QueryString["error"] == "access_denied")
            //{
            //    ViewBag.Message = "User has denied access.";
            //}
            //else
            //{
            //    string code = Request.QueryString["code"];
            //    if (!string.IsNullOrEmpty(code))
            //    {
            //        string data = FaceBookConnect.Fetch(code, "me?fields=id,name,email");
            //        faceBookUser = new JavaScriptSerializer().Deserialize<MNPOSTWEBSITEMODEL.WS_FacebookUser>(data);
            //        faceBookUser.PictureUrl = string.Format("https://graph.facebook.com/{0}/picture", faceBookUser.ID);
            //    }
            //}
            //Session["Username"] = faceBookUser.Name;
            try
            {
                Session["token"] = getToken().Result;
                var lst = db.WS_Post.ToList();
                return View(lst);
            }
            catch(Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
           
        }

        public async Task<List<Province>> getProvince(string tokenaccess)
        {

            string token = "";
            List<Province> pro = new List<Province>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenaccess);
                using (HttpResponseMessage response = await client.GetAsync("http://221.133.7.74:90/api/catalog/GetProvince").ConfigureAwait(continueOnCapturedContext: false))
                {

                    using (HttpContent content = response.Content)
                    {
                        token = await content.ReadAsStringAsync();
                        var obj = JObject.Parse(token);
                        var jobj = obj["provinces"];
                        var jsonstring = JsonConvert.SerializeObject(jobj);
                        if (jsonstring != null)
                        {
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            pro = (List<Province>)serializer.Deserialize(jsonstring, typeof(List<Province>));
                            return pro;
                        }
                        return pro;
                    }
                }
            }
        }

      

        public async Task<List<District>> getDistrict(string tokenaccess, string provinceid)
        {

            string token = "";
            List<District> dist = new List<District>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenaccess);
                using (HttpResponseMessage response = await client.GetAsync("http://221.133.7.74:90/api/catalog/GetDistrict?provinceid="+ provinceid).ConfigureAwait(continueOnCapturedContext: false))
                {

                    using (HttpContent content = response.Content)
                    {
                        token = await content.ReadAsStringAsync();
                        var obj = JObject.Parse(token);
                        var jobj = obj["districts"];
                        var jsonstring = JsonConvert.SerializeObject(jobj);
                        if (jsonstring != null)
                        {
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            dist = (List<District>)serializer.Deserialize(jsonstring, typeof(List<District>));
                            return dist;
                        }
                        return dist;
                    }
                }
            }
        }

        public async Task<List<Ward>> getWard(string tokenaccess, string districtid)
        {

            string token = "";
            List<Ward> ward = new List<Ward>();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenaccess);
                using (HttpResponseMessage response = await client.GetAsync("http://221.133.7.74:90/api/catalog/GetWard?districtid="+districtid).ConfigureAwait(continueOnCapturedContext: false))
                {

                    using (HttpContent content = response.Content)
                    {
                        token = await content.ReadAsStringAsync();
                        var obj = JObject.Parse(token);
                        var jobj = obj["wards"];
                        var jsonstring = JsonConvert.SerializeObject(jobj);
                        if (jsonstring != null)
                        {
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            ward = (List<Ward>)serializer.Deserialize(jsonstring, typeof(List<Ward>));
                            return ward;
                        }
                        return ward;
                    }
                }
            }
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
                using (HttpResponseMessage response = await client.PostAsync("http://221.133.7.74:90/mntoken", q).ConfigureAwait(continueOnCapturedContext: false))
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