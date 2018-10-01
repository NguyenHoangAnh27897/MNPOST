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
using MNPOSTWEBSITE.Controllers;
using MNPOSTWEBSITEMODEL;
using PagedList;
using PagedList.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MNPOSTWEBSITE.Controllers
{
    public class OrderController : BaseController
    {
        MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEEntities();
        MNPOSTWEBSITE.Controllers.HomeController home = new HomeController();
        // GET: Order
        public ActionResult Create()
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        //string username = Session["Email"].ToString();
                        //if (db.WS_Mailer.Where(s => s.CustomerAccount.Equals(username)).FirstOrDefault().SenderName != null)
                        //{
                        //ViewBag.SenderName = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().SenderName;
                        //ViewBag.SenderAddress = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().SenderAddress;
                        //ViewBag.SenderPhone = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().SenderPhone;
                        //ViewBag.SenderDistrictID = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().SenderDistrictID;
                        //ViewBag.SenderProvinceID = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().SenderProvinceID;
                        //ViewBag.SenderWardID = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().SenderWardID;
                        //ViewBag.RecieverName = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().RecieverName;
                        //ViewBag.RecieverAddress = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().RecieverAddress;
                        //ViewBag.RecieverPhone = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().RecieverPhone;
                        //ViewBag.RecieverDistrictID = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().RecieverDistrictID;
                        //ViewBag.RecieverProvinceID = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().RecieverProvinceID;
                        //ViewBag.RecieverWardID = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().RecieverWardID;
                        //ViewBag.Weight = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().Weight;
                        //ViewBag.Quantity = db.WS_Mailer.Where(s => s.CustomerAccount == username).FirstOrDefault().Quantity;
                        //var parameters = new Dictionary<string, string>();
                        //parameters.Add("@user", username);
                        //var sqlAdapter = GetSqlDataAdapter("MAILER_GETALL", parameters);

                        //}
                        return View();
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

        [HttpPost]
        public async Task<ActionResult> Create(string SenderName = "", string SenderAddress = "", string SenderPhone = "", string SenderWardID = "", string SenderDistrictID = "", string SenderProvinceID = "", string RecieverName = "", string RecieverAddress = "", string RecieverPhone = "", string RecieverWardID = "", string RecieverDistrictID = "", string RecieverProvinceID = "", int? Quantity = 0, double? Weight = 0, string Purchase = "", string MerchandiseValue = "", string COD = "", string Note = "", string MailerDescription = "", int? Length = 0, int? Width = 0, int? Height = 0, string MailerTypeID = "", string MerchandiseID = "", int? PriceMain = 0, int? CODPrice = 0, int? PriceDefault = 0)
        {
            try
            {
                decimal? cod = decimal.Parse(COD);
                decimal? MerchandiseVal = decimal.Parse(MerchandiseValue);
                Mailer mailers = new Mailer
                {
                    MailerID = "MNPOST0002",
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
                    Quantity = Quantity,
                    PaymentMethodID = Purchase,
                    MerchandiseValue = MerchandiseVal,
                    COD = cod,
                    Notes = Note,
                    MailerDescription = MailerDescription,
                    LengthSize = Length,
                    HeightSize = Height,
                    WidthSize = Width,
                    MailerTypeID = MailerTypeID,
                    MerchandiseID = MerchandiseID,
                    PriceDefault = PriceDefault,
                    SenderID = Session["CustomerID"].ToString()
                };
                await AddorUpdateMailer(0, mailers);
                return RedirectToAction("List", "Order");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        public ActionResult Edit(string id)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        var mailer = getMailerbyMailerID(id).Result;
                        return View(mailer);
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

        [HttpPost]
        public async Task<ActionResult> Edit(string MailerID = "", string SenderName = "", string SenderAddress = "", string SenderPhone = "", string SenderWardID = "", string SenderDistrictID = "", string SenderProvinceID = "", string RecieverName = "", string RecieverAddress = "", string RecieverPhone = "", string RecieverWardID = "", string RecieverDistrictID = "", string RecieverProvinceID = "", int? Quantity = 0, double? Weight = 0, string Purchase = "", string MerchandiseValue = "", string COD = "", string Note = "", string MailerDescription = "", int? Length = 0, int? Width = 0, int? Height = 0, string MailerTypeID = "", string MerchandiseID = "", int? PriceMain = 0, int? CODPrice = 0, int? PriceDefault = 0)
        {
            try
            {
                decimal? cod = decimal.Parse(COD);
                decimal? MerchandiseVal = decimal.Parse(MerchandiseValue);
                Mailer mailers = new Mailer
                {
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
                    Quantity = Quantity,
                    PaymentMethodID = Purchase,
                    MerchandiseValue = MerchandiseVal,
                    COD = cod,
                    Notes = Note,
                    MailerDescription = MailerDescription,
                    LengthSize = Length,
                    HeightSize = Height,
                    WidthSize = Width,
                    MailerTypeID = MailerTypeID,
                    MerchandiseID = MerchandiseID,
                    PriceDefault = PriceDefault,
                    MailerID = MailerID,
                    SenderID = Session["CustomerID"].ToString()
                };
                await AddorUpdateMailer(1, mailers);
                return RedirectToAction("List", "Order");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        public async Task<ActionResult> AddorUpdateMailer(int choose, Mailer mailers)
        {
            string api = "";
            if (choose == 0)
            {
                api = "http://221.133.7.74:90/api/mailer/AddMailer";
            }
            else
            {
                api = "http://221.133.7.74:90/api/mailer/UpdateMailer";
            }
            if (mailers != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
                    var response = await client.PostAsJsonAsync(api, new { mailer = mailers }).ConfigureAwait(continueOnCapturedContext: false);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new ResultInfo() { error = 0, msg = "Thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new ResultInfo() { error = 0, msg = "Lỗi data" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderList()
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        return View();
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

        public ActionResult List(int? page = 1)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        int pageSize = 5;
                        int pageNumber = (page ?? 1);
                        //api/mailer/GetMailerbyCustomerID?customerid=
                        List<Mailer> mailer = getMailerbyCustomerID().Result;
                        return View(mailer.ToPagedList(pageNumber, pageSize));
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

        public async Task<List<Mailer>> getMailerbyCustomerID()
        {
            string cusid = Session["CustomerID"].ToString();
            List<Mailer> mailer = new List<Mailer>();
            if (cusid != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
                    using (HttpResponseMessage response = await client.GetAsync("http://221.133.7.74:90/api/mailer/GetMailerbyCustomerID?customerid=" + cusid).ConfigureAwait(continueOnCapturedContext: false))
                    {

                        using (HttpContent content = response.Content)
                        {
                            string token = await content.ReadAsStringAsync();
                            var obj = JObject.Parse(token);
                            var jobj = obj["mailer"];
                            var jsonstring = JsonConvert.SerializeObject(jobj);
                            if (jsonstring != null)
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                mailer = (List<Mailer>)serializer.Deserialize(jsonstring, typeof(List<Mailer>));
                                return mailer;
                            }
                            return mailer;
                        }
                    }
                }
            }
            return mailer;
        }

        public async Task<List<Mailer>> getMailerbyCustomerIDandDate(string fromdate, string todate)
        {
            string cusid = Session["CustomerID"].ToString();
            List<Mailer> mailer = new List<Mailer>();
            string api = "http://221.133.7.74:90/api/mailer/GetMailerbyCustomerIDandDate?customerid=" + cusid + "&fromdate="+fromdate+"&todate="+todate;
            if (cusid != null)
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
                            var jobj = obj["mailer"];
                            var jsonstring = JsonConvert.SerializeObject(jobj);
                            if (jsonstring != null)
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                mailer = (List<Mailer>)serializer.Deserialize(jsonstring, typeof(List<Mailer>));
                                return mailer;
                            }
                            return mailer;
                        }
                    }
                }
            }
            return mailer;
        }

        //Search Mailer by MailerID
        [HttpGet]
        public ActionResult SearchMailer(string mailerid)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        //api/mailer/GetMailerbyCustomerID?customerid=
                        Mailer mailer = getMailerbyMailerID(mailerid).Result;
                        return View(mailer);
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

        public async Task<Mailer> getMailerbyMailerID(string mailerid)
        {
            Mailer mailer = new Mailer();
            if (mailerid != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
                    using (HttpResponseMessage response = await client.GetAsync("http://221.133.7.74:90/api/mailer/GetMailerbyID/" + mailerid).ConfigureAwait(continueOnCapturedContext: false))
                    {

                        using (HttpContent content = response.Content)
                        {
                            string token = await content.ReadAsStringAsync();
                            var obj = JObject.Parse(token);
                            var jobj = obj["mailer"];
                            var jsonstring = JsonConvert.SerializeObject(jobj);
                            if (jsonstring != null)
                            {
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                mailer = (Mailer)serializer.Deserialize(jsonstring, typeof(Mailer));
                                return mailer;
                            }
                            return mailer;
                        }
                    }
                }
            }
            return mailer;
        }

        [HttpPost]
        public async Task<ActionResult> List(string SenderID = "", string SenderName = "", string SenderAddress = "", string SenderPhone = "", string SenderWardID = "", string SenderDistrictID = "", string SenderProvinceID = "", string RecieverName = "", string RecieverAddress = "", string RecieverPhone = "", string RecieverWardID = "", string RecieverDistrictID = "", string RecieverProvinceID = "", int? Quantity = 0, double? Weight = 0)
        {
            //MM_Mailers mailers = new MM_Mailers
            //{
            //    MailerID = "007",
            //    SenderName = SenderName,
            //    SenderAddress = SenderAddress,
            //    SenderPhone = SenderPhone,
            //    SenderDistrictID = SenderDistrictID,
            //    SenderProvinceID = SenderProvinceID,
            //    SenderWardID = SenderWardID,
            //    RecieverName = RecieverName,
            //    RecieverAddress = RecieverAddress,
            //    RecieverPhone = RecieverPhone,
            //    RecieverDistrictID = RecieverDistrictID,
            //    RecieverProvinceID = RecieverProvinceID,
            //    RecieverWardID = RecieverWardID,
            //    Weight = Weight,
            //    Quantity = Quantity
            //};
            //var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            //string api = "http://35.231.147.186:89/api/mailer/addmailer";
            //var response = await client.PostAsJsonAsync(api, new { mailer = mailers });
            //if (response.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("Index", "Manage");
            //}
            return View();
        }

        //generate ra một id mới
        public static string getGUID()
        {
            string rs = "";
            char replace = '-';
            char to = '_';
            try
            {
                rs = Guid.NewGuid().ToString();
                rs = rs.Replace(replace, to);
            }
            catch (Exception ex)
            {
                string mess = ex.Message.ToString();
            }
            return rs;
        }

        [HttpGet]
        public JsonResult GetDistrict(string provinceid)
        {
            List<District> lstDis = new List<District>();
            var token = Session["token"].ToString();
            lstDis = home.getDistrict(token,provinceid).Result;
            return Json(lstDis, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetWard(string districtid)
        {
            List<Ward> lstWard = new List<Ward>();
            var token = Session["token"].ToString();
            lstWard = home.getWard(token, districtid).Result;
            return Json(lstWard, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetMailer(string FromDate, string ToDate)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        string fromDate;
                        string toDate;
                        //api/mailer/GetMailerbyCustomerID?customerid=
                        if (String.IsNullOrEmpty(FromDate) || String.IsNullOrEmpty(ToDate))
                        {
                            fromDate = DateTime.Now.ToString("yyyy-MM-dd");
                            toDate = DateTime.Now.ToString("yyyy-MM-dd");
                            FromDate = DateTime.Now.ToString("dd/MM/yyyy");
                            ToDate = DateTime.Now.ToString("dd/MM/yyyy");

                        }
                        else
                        {
                            fromDate = DateTime.ParseExact(Request["FromDate"], "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                            toDate = DateTime.ParseExact(Request["ToDate"], "dd/MM/yyyy", null).ToString("yyyy-MM-dd");
                        }
                        List<Mailer> mailer = getMailerbyCustomerIDandDate(fromDate,toDate).Result;
                        //Mailer mailer = getMailerbyMailerID(mailerid).Result;
                        return View(mailer);
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
    }
}