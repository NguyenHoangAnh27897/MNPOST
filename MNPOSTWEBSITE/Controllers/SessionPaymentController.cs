using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using MNPOSTWEBSITE.Models;
using MNPOSTWEBSITE.Controllers;
using MNPOSTWEBSITEMODEL;
using PagedList;
using PagedList.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System.Data.OleDb;
using System.Xml;

namespace MNPOSTWEBSITE.Controllers
{
    public class SessionPaymentController : Controller
    {
        // GET: SessionPayment
        public ActionResult Index()
        {
            return View();
        }

        public async Task<List<CODEntity>> getCOD(string customerid, string fromdate, string todate)
        {
            List<CODEntity> cod = new List<CODEntity>();
            string api = "http://noiboapi.miennampost.vn/api/cod/GetCODDebitvoucher?customerid=" + customerid+"&fromdate="+fromdate+"&todate="+todate;
            if (customerid != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
                    using (HttpResponseMessage response = await client.GetAsync(api).ConfigureAwait(continueOnCapturedContext: false))
                    {

                        using (HttpContent content = response.Content)
                        {
                            string token = await content.ReadAsStringAsync();
                            var obj = JObject.Parse(token);
                            var jobj = obj["codinfo"];
                            var jsonstring = JsonConvert.SerializeObject(jobj);
                            if (jsonstring != null)
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                cod = (List<CODEntity>)serializer.Deserialize(jsonstring, typeof(List<CODEntity>));
                                return cod;
                            }
                            return cod;
                        }
                    }
                }
            }
            return cod;
        }

        [HttpGet]
        public ActionResult SearchCOD(string FromDate, string ToDate, int? page = 1)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        string cusid = Session["CustomerID"].ToString();
                        if (String.IsNullOrEmpty(FromDate) || String.IsNullOrEmpty(ToDate))
                        {
                            FromDate = DateTime.Now.ToString("yyyy-MM-dd");
                            ToDate = DateTime.Now.ToString("yyyy-MM-dd");

                        }
                        int pageSize = 5;
                        int pageNumber = (page ?? 1);
                        List<CODEntity> cod = getCOD(cusid, FromDate, ToDate).Result;
                        return View(cod.ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
         
        }

        public ActionResult Detail(string id)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        List<CODDetail> cod = getDetailCOD(id).Result;
                        return View(cod);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("ErrorPage","Error");
            }          
        }

        public async Task<List<CODDetail>> getDetailCOD(string documentid)
        {
            List<CODDetail> cod = new List<CODDetail>();
            string api = "http://noiboapi.miennampost.vn/api/cod/GetCODDebitvoucherDetail?documentid=" + documentid;
            if (documentid != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
                    using (HttpResponseMessage response = await client.GetAsync(api).ConfigureAwait(continueOnCapturedContext: false))
                    {
                        using (HttpContent content = response.Content)
                        {
                            string token = await content.ReadAsStringAsync();
                            var obj = JObject.Parse(token);
                            var jobj = obj["coddetailinfo"];
                            var jsonstring = JsonConvert.SerializeObject(jobj);
                            if (jsonstring != null)
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                cod = (List<CODDetail>)serializer.Deserialize(jsonstring, typeof(List<CODDetail>));
                                return cod;
                            }
                            return cod;
                        }
                    }
                }
            }
            return cod;
        }

        public ActionResult Transaction()
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        string cusid = Session["CustomerID"].ToString();
                        List<CODTotal> cod = getTransaction(cusid).Result;
                        return View(cod);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        public async Task<List<CODTotal>> getTransaction(string customerid)
        {
            List<CODTotal> cod = new List<CODTotal>();
            string api = "http://noiboapi.miennampost.vn/api/cod/GetCODTotal?customerid=" + customerid;
            if (customerid != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
                    using (HttpResponseMessage response = await client.GetAsync(api).ConfigureAwait(continueOnCapturedContext: false))
                    {
                        using (HttpContent content = response.Content)
                        {
                            string token = await content.ReadAsStringAsync();
                            var obj = JObject.Parse(token);
                            var jobj = obj["codtotalinfo"];
                            var jsonstring = JsonConvert.SerializeObject(jobj);
                            if (jsonstring != null)
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                cod = (List<CODTotal>)serializer.Deserialize(jsonstring, typeof(List<CODTotal>));
                                return cod;
                            }
                            return cod;
                        }
                    }
                }
            }
            return cod;
        }
    }
}