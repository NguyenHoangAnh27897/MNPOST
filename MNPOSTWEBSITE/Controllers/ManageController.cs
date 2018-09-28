using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using MNPOSTWEBSITEMODEL;
using System.Threading.Tasks;
using MNPOSTWEBSITE.Models;
using Newtonsoft.Json.Linq;

namespace MNPOSTWEBSITE.Controllers
{
    public class ManageController : Controller
    {
        MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEEntities();
        //
        // GET: /Manage/
        public ActionResult Index()
        {
            if (Session["Authentication"] != null)
            {
                ViewBag.CusInfo = getcusinfo().Result;
                Session["CustomerID"] = getcusinfo().Result;
                string id = Session["ID"].ToString();
                var user = db.AspNetUsers.Where(s => s.Id == id);
                return View(user);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public async Task<string> getcusinfo()
        {
            string id = Session["ID"].ToString();
            var cusid = db.AspNetUsers.Where(s => s.Id == id).FirstOrDefault().IDClient;
            if(cusid != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["token"].ToString());
                    using (HttpResponseMessage response = await client.GetAsync("http://221.133.7.74:90/api/customer/getcustomerinfo/" + cusid).ConfigureAwait(continueOnCapturedContext: false))
                    {

                        using (HttpContent content = response.Content)
                        {
                            string token = await content.ReadAsStringAsync();
                            var obj = JObject.Parse(token);
                            string idsv = (string)obj["customer"]["CustomerCode"];
                            return idsv;
                        }
                    }
                }
            }
            else
            {
                return "";
            }   
        }

        public ActionResult EditAboutus()
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Admin"))
                    {

                        var rs = db.WS_AboutUs.Where(s => s.ID == "firstabu");
                        return View(rs);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Manage");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditAboutus(string Content)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Admin"))
                    {

                        string id = "firstabu";
                        var rs = db.WS_AboutUs.Find(id);
                        rs.ContentAbout = Content;
                        db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Manage");
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Manage");
            }
        }

        public ActionResult ManageService()
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Admin"))
                    {
                        var lst = db.WS_ServiceType.ToList();
                        return View(lst);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }

        }

        public ActionResult EditService(string ID)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Admin"))
                    {
                        int id = int.Parse(ID);
                        var rs = db.WS_ServiceType.Where(s => s.ID == id);
                        return View(rs);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }

        }

        [HttpPost]
        public ActionResult EditService(string Service, string ID, string Content)
        {
            try
            {
                int id = int.Parse(ID);
                var sv = db.WS_ServiceType.Find(id);
                sv.Name = Service;
                sv.ContentService = Content;
                db.Entry(sv).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageService", "Manage");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        public ActionResult EditAccount(string id)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                        var rs = db.AspNetUsers.Where(s => s.Id == id);
                        return View(rs);
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }
      //  [HttpPost]
      //public async Task<ActionResult> EditCustomer()
      //  {

      //  }
    }
}