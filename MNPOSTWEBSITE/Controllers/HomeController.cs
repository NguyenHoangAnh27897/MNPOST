﻿using System;
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
            List<Province> lst = new List<Province>();
            string token = getToken().Result;
            int count = getCount(token).Result;
            for(int i = 0; i < count; i++)
            {
                lst.Add(new Province()
                {
                    ProvinceName = getDistrict(token, i).Result
                });
            }
            return View(lst);
        }

        async static Task<string> getDistrict(string tokenaccess, int i)
        {

            string token = "";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenaccess);
                using (HttpResponseMessage response = await client.GetAsync("http://35.231.147.186:89/api/catalog/GetProvince"))
                {

                    using (HttpContent content = response.Content)
                    {
                        token = await content.ReadAsStringAsync();
                        var obj = JObject.Parse(token);
                        var count = obj["provinces"].ToList();
                        var tokenstr = (string)obj["provinces"][i]["ProvinceName"];
                        return tokenstr;

                    }
                }
            }
        }

        async static Task<int> getCount(string tokenaccess)
        {

            string token = "";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenaccess);
                using (HttpResponseMessage response = await client.GetAsync("http://35.231.147.186:89/api/catalog/GetProvince"))
                {

                    using (HttpContent content = response.Content)
                    {
                        token = await content.ReadAsStringAsync();
                        var obj = JObject.Parse(token);
                        var count = obj["provinces"].ToList();
                        return count.Count;
                    }
                }
            }
        }

        async static Task<string> getToken()
        {
            string token = "";
            IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username","mnwebsite"),
                new KeyValuePair<string, string>("password","mnpost@123"),
                new KeyValuePair<string, string>("grant_type","password")
            };
            HttpContent q = new FormUrlEncodedContent(queries);
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.PostAsync("http://35.231.147.186:89/mntoken", q))
                {
                    using (HttpContent content = response.Content)
                    {
                        token = await content.ReadAsStringAsync();
                        HttpContentHeaders headers = content.Headers;
                        var obj = JObject.Parse(token);
                        var tokenstr = (string)obj["access_token"];
                        return tokenstr;
                    }
                }
            }
        }
    }
}