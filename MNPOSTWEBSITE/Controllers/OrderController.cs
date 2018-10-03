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
using System.Data;
using System.Data.OleDb;
using System.Xml;

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
                if(SenderWardID.Count() > 10)
                {
                    SenderWardID = "";
                }else if(RecieverWardID.Count() > 10)
                {
                    RecieverWardID = "";
                }
                Mailer mailers = new Mailer
                {
                    MailerID = getGUID(),
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
                if (SenderWardID.Count() > 10)
                {
                    SenderWardID = "";
                }else if(RecieverWardID.Count() > 10)
                {
                    RecieverWardID = "";
                }
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
            string api = "http://221.133.7.74:90/api/mailer/GetMailerbyCustomerIDandDate?customerid=" + cusid + "&fromdate=" + fromdate + "&todate=" + todate;
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

        public async Task<ActionResult> DeleteMailerById(string mailerid)
        {
            Mailer mailers = new Mailer
            {
                MailerID = mailerid
            };
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
            string api = "http://221.133.7.74:90/api/mailer/DeleteCustomerByCustomerID";
            var response = await client.PostAsJsonAsync(api, new { mailer = mailers }).ConfigureAwait(continueOnCapturedContext: false);
            if (response.IsSuccessStatusCode)
            {
                return Json(new ResultInfo() { error = 0, msg = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultInfo() { error = 1, msg = "Lỗi data" }, JsonRequestBehavior.AllowGet);
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
            string cusid = Session["CustomerID"].ToString();
            string api = "http://221.133.7.74:90/api/mailer/GetMailerbyID?id=" + mailerid + "&customerid=" + cusid;
            if (mailerid != null)
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
            string rs = "MNPOST";
            Random rd = new Random();
            int random = rd.Next(90000);
            rs += random.ToString() + "_";
            random = rd.Next(90000);
            rs += random.ToString();
            return rs;
        }

        [HttpGet]
        public JsonResult GetDistrict(string provinceid)
        {
            List<District> lstDis = new List<District>();
            var token = Session["token"].ToString();
            lstDis = home.getDistrict(token, provinceid).Result;
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

        [HttpPost]
        public async Task<ActionResult> SendMailerCheck(IEnumerable<string> mailerid)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        foreach(var item in mailerid)
                        {
                            var ml = getMailerbyMailerID(item).Result;
                            Mailer mailers = new Mailer()
                            {
                                MailerID = item,
                                SenderID =ml.SenderID,
                                MailerDescription = ml.MailerDescription,
                                SenderName = ml.SenderName,
                                SenderAddress = ml.SenderAddress,
                                SenderPhone = ml.SenderPhone,
                                SenderDistrictID = ml.SenderDistrictID,
                                SenderProvinceID = ml.SenderProvinceID,
                                SenderWardID = ml.SenderWardID,
                                RecieverName = ml.RecieverName,
                                RecieverAddress = ml.RecieverAddress,
                                RecieverPhone = ml.RecieverPhone,
                                RecieverDistrictID = ml.RecieverDistrictID,
                                RecieverProvinceID = ml.RecieverProvinceID,
                                RecieverWardID = ml.RecieverWardID,
                                Weight = ml.Weight,
                                Quantity = ml.Quantity,
                                PaymentMethodID = ml.PaymentMethodID,
                                MerchandiseValue = ml.MerchandiseValue,
                                COD = ml.COD,
                                Notes = ml.Notes,
                                LengthSize = ml.LengthSize,
                                HeightSize = ml.HeightSize,
                                WidthSize = ml.WidthSize,
                                MailerTypeID = ml.MailerTypeID,
                                MerchandiseID = ml.MerchandiseID,
                                PriceDefault = ml.PriceDefault,
                                AcceptDate = ml.AcceptDate,
                                CurrentStatusID = 1
                            };
                            await AddorUpdateMailer(1, mailers);
                        }
                        return RedirectToAction("List");
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
        public async Task<ActionResult> ReturnMailer(IEnumerable<string> mailerid)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        foreach (var item in mailerid)
                        {
                            var ml = getMailerbyMailerID(item).Result;
                            Mailer mailers = new Mailer()
                            {
                                MailerID = item,
                                SenderID = ml.SenderID,
                                MailerDescription = ml.MailerDescription,
                                SenderName = ml.SenderName,
                                SenderAddress = ml.SenderAddress,
                                SenderPhone = ml.SenderPhone,
                                SenderDistrictID = ml.SenderDistrictID,
                                SenderProvinceID = ml.SenderProvinceID,
                                SenderWardID = ml.SenderWardID,
                                RecieverName = ml.RecieverName,
                                RecieverAddress = ml.RecieverAddress,
                                RecieverPhone = ml.RecieverPhone,
                                RecieverDistrictID = ml.RecieverDistrictID,
                                RecieverProvinceID = ml.RecieverProvinceID,
                                RecieverWardID = ml.RecieverWardID,
                                Weight = ml.Weight,
                                Quantity = ml.Quantity,
                                PaymentMethodID = ml.PaymentMethodID,
                                MerchandiseValue = ml.MerchandiseValue,
                                COD = ml.COD,
                                Notes = ml.Notes,
                                LengthSize = ml.LengthSize,
                                HeightSize = ml.HeightSize,
                                WidthSize = ml.WidthSize,
                                MailerTypeID = ml.MailerTypeID,
                                MerchandiseID = ml.MerchandiseID,
                                PriceDefault = ml.PriceDefault,
                                AcceptDate = ml.AcceptDate,
                                CurrentStatusID = 0
                            };
                            await AddorUpdateMailer(1, mailers);
                        }
                        return RedirectToAction("List","Order");
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

        [HttpGet]
        public ActionResult GetMailer(string FromDate, string ToDate, int? page = 1)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        //api/mailer/GetMailerbyCustomerID?customerid=
                        if (String.IsNullOrEmpty(FromDate) || String.IsNullOrEmpty(ToDate))
                        {
                            FromDate = DateTime.Now.ToString("yyyy-MM-dd");
                            ToDate = DateTime.Now.ToString("yyyy-MM-dd");

                        }
                        int pageSize = 5;
                        int pageNumber = (page ?? 1);
                        List<Mailer> mailer = getMailerbyCustomerIDandDate(FromDate, ToDate).Result;
                        //Mailer mailer = getMailerbyMailerID(mailerid).Result;
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

        [HttpGet]
        public async Task<ActionResult> DeleteMailer(string id)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        await DeleteMailerById(id);
                        return RedirectToAction("List");
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
        public async Task<ActionResult> getExcel(HttpPostedFileBase FileExcel)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Customer"))
                    {
                        DataSet ds = new DataSet();
                        if (Request.Files["FileExcel"].ContentLength > 0)
                        {
                            string fileExtension =
                                                 System.IO.Path.GetExtension(Request.Files["FileExcel"].FileName);

                            if (fileExtension == ".xls" || fileExtension == ".xlsx")
                            {
                                string fileLocation = Server.MapPath("~/document/excel/") + Request.Files["FileExcel"].FileName;
                                if (System.IO.File.Exists(fileLocation))
                                {

                                    System.IO.File.Delete(fileLocation);
                                }
                                Request.Files["FileExcel"].SaveAs(fileLocation);
                                string excelConnectionString = string.Empty;
                                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                //connection String for xls file format.
                                if (fileExtension == ".xls")
                                {
                                    excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                                    fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                                }
                                //connection String for xlsx file format.
                                else if (fileExtension == ".xlsx")
                                {
                                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                                }
                                //Create Connection to Excel work book and add oledb namespace
                                OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                                excelConnection.Open();
                                DataTable dt = new DataTable();

                                dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                if (dt == null)
                                {
                                    return null;
                                }

                                String[] excelSheets = new String[dt.Rows.Count];
                                int t = 0;
                                //excel data saves in temp file here.
                                foreach (DataRow row in dt.Rows)
                                {
                                    excelSheets[t] = row["TABLE_NAME"].ToString();
                                    t++;
                                }
                                OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                                string query = string.Format("Select * from [{0}]", excelSheets[0]);
                                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                                {
                                    dataAdapter.Fill(ds);
                                }
                            }
                            if (fileExtension.ToString().ToLower().Equals(".xml"))
                            {
                                string fileLocation = Server.MapPath("~/document/excel/") + Request.Files["FileExcel"].FileName;
                                if (System.IO.File.Exists(fileLocation))
                                {
                                    System.IO.File.Delete(fileLocation);
                                }

                                Request.Files["FileExcel"].SaveAs(fileLocation);
                                XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                                // DataSet ds = new DataSet();
                                ds.ReadXml(xmlreader);
                                xmlreader.Close();
                            }

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                //string conn = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
                                //SqlConnection con = new SqlConnection(conn);
                                //string query = "Insert into Person(Name,Email,Mobile) Values('" +
                                //ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() +
                                //"','" + ds.Tables[0].Rows[i][2].ToString() + "')";
                                //con.Open();
                                //SqlCommand cmd = new SqlCommand(query, con);
                                //cmd.ExecuteNonQuery();
                                //con.Close();
                                decimal? cod = decimal.Parse(ds.Tables[0].Rows[i][12].ToString());
                                decimal? MerchandiseVal = decimal.Parse(ds.Tables[0].Rows[i][16].ToString());
                                int? wei = int.Parse(ds.Tables[0].Rows[i][13].ToString());
                                int? quan = int.Parse(ds.Tables[0].Rows[i][14].ToString());
                                double? hei = double.Parse(ds.Tables[0].Rows[i][20].ToString());
                                double? wid = double.Parse(ds.Tables[0].Rows[i][21].ToString());
                                double? len = double.Parse(ds.Tables[0].Rows[i][19].ToString());
                                decimal? pdf = decimal.Parse(ds.Tables[0].Rows[i][24].ToString());
                                Mailer mailers = new Mailer
                                {
                                    MailerID = getGUID(),
                                    SenderName = ds.Tables[0].Rows[i][0].ToString(),
                                    SenderAddress = ds.Tables[0].Rows[i][2].ToString(),
                                    SenderPhone = ds.Tables[0].Rows[i][1].ToString(),
                                    SenderDistrictID = ds.Tables[0].Rows[i][4].ToString(),
                                    SenderProvinceID = ds.Tables[0].Rows[i][3].ToString(),
                                    SenderWardID = ds.Tables[0].Rows[i][5].ToString(),
                                    RecieverName = ds.Tables[0].Rows[i][6].ToString(),
                                    RecieverAddress = ds.Tables[0].Rows[i][8].ToString(),
                                    RecieverPhone = ds.Tables[0].Rows[i][7].ToString(),
                                    RecieverDistrictID = ds.Tables[0].Rows[i][10].ToString(),
                                    RecieverProvinceID = ds.Tables[0].Rows[i][9].ToString(),
                                    RecieverWardID = ds.Tables[0].Rows[i][11].ToString(),
                                    Weight = wei,
                                    Quantity = quan,
                                    PaymentMethodID = ds.Tables[0].Rows[i][15].ToString(),
                                    MerchandiseValue = MerchandiseVal,
                                    COD = cod,
                                    Notes = ds.Tables[0].Rows[i][17].ToString(),
                                    MailerDescription = ds.Tables[0].Rows[i][18].ToString(),
                                    LengthSize = len,
                                    HeightSize = hei,
                                    WidthSize = wid,
                                    MailerTypeID = ds.Tables[0].Rows[i][22].ToString(),
                                    MerchandiseID = ds.Tables[0].Rows[i][23].ToString(),
                                    PriceDefault = pdf,
                                    SenderID = Session["CustomerID"].ToString(),
                                    AcceptDate = DateTime.Now

                                };
                                await AddorUpdateMailer(0, mailers);
                            }
                        }
                        return RedirectToAction("List", "Order");
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