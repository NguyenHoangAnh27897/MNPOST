using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using MNPOSTWEBSITE.Models;
using System.IO;

namespace MNPOSTWEBSITE.Controllers
{
    [Authorize (Roles ="admin")]
    public class ManageController : Controller
    {
        MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEEntities();
        //
        // GET: /Manage/
        public ActionResult Index()
        {
            return View();
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
        [ValidateInput(false)]
        public ActionResult EditService(string Service, string ID, string Content, HttpPostedFileBase FilePDF)
        {
            try
            {
                int id = int.Parse(ID);
                var sv = db.WS_ServiceType.Find(id);
                sv.Name = Service;
                sv.ContentService = Content;
                if (Request.Files["FilePDF"].ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["FilePDF"].FileName);
                    if (fileExtension == ".pdf")
                    {
                        string fileLocation = Server.MapPath("~/document/pdf/") + Request.Files["FilePDF"].FileName;
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        Request.Files["FilePDF"].SaveAs(fileLocation);
                        string fname = Request.Files["FilePDF"].FileName;
                        fname = fname.Remove(fname.Length - 4);
                        sv.ExcelFile = fname;
                    }
                }
                db.Entry(sv).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageService", "Manage");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

       
        [HttpPost]
        public ActionResult HideService(string check1 = "", string check2 = "", string check3 = "", string check4 = "", string check5 = "")
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if(check1 != "")
                    {
                        if(check1 == "true")
                        {
                            var rs = db.WS_ServiceType.Find(1);
                            rs.Hide = true;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }else if(check1 == "false")
                        {
                            var rs = db.WS_ServiceType.Find(1);
                            rs.Hide = false;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }   
                    }
                    if (check2 != "")
                    {
                        if (check2 == "true")
                        {
                            var rs = db.WS_ServiceType.Find(2);
                            rs.Hide = true;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (check2 == "false")
                        {
                            var rs = db.WS_ServiceType.Find(2);
                            rs.Hide = false;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    if (check3 != "")
                    {
                        if (check3 == "true")
                        {
                            var rs = db.WS_ServiceType.Find(3);
                            rs.Hide = true;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (check3 == "false")
                        {
                            var rs = db.WS_ServiceType.Find(3);
                            rs.Hide = false;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    if (check4 != "")
                    {
                        if (check4 == "true")
                        {
                            var rs = db.WS_ServiceType.Find(4);
                            rs.Hide = true;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (check4 == "false")
                        {
                            var rs = db.WS_ServiceType.Find(4);
                            rs.Hide = false;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    if (check5 != "")
                    {
                        if (check5 == "true")
                        {
                            var rs = db.WS_ServiceType.Find(5);
                            rs.Hide = true;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (check5 == "false")
                        {
                            var rs = db.WS_ServiceType.Find(5);
                            rs.Hide = false;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("ManageService", "Manage");
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
        public ActionResult HideRecruiment(string check1 = "", string check2 = "", string check3 = "", string check4 = "", string check5 = "")
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (check1 != "")
                    {
                        if (check1 == "true")
                        {
                            var rs = db.WS_Recruitment.Find(1);
                            rs.Hide = true;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (check1 == "false")
                        {
                            var rs = db.WS_Recruitment.Find(1);
                            rs.Hide = false;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    if (check2 != "")
                    {
                        if (check2 == "true")
                        {
                            var rs = db.WS_Recruitment.Find(2);
                            rs.Hide = true;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (check2 == "false")
                        {
                            var rs = db.WS_Recruitment.Find(2);
                            rs.Hide = false;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    if (check3 != "")
                    {
                        if (check3 == "true")
                        {
                            var rs = db.WS_Recruitment.Find(3);
                            rs.Hide = true;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (check3 == "false")
                        {
                            var rs = db.WS_Recruitment.Find(3);
                            rs.Hide = false;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    if (check4 != "")
                    {
                        if (check4 == "true")
                        {
                            var rs = db.WS_Recruitment.Find(4);
                            rs.Hide = true;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (check4 == "false")
                        {
                            var rs = db.WS_Recruitment.Find(4);
                            rs.Hide = false;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    if (check5 != "")
                    {
                        if (check5 == "true")
                        {
                            var rs = db.WS_Recruitment.Find(5);
                            rs.Hide = true;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else if (check5 == "false")
                        {
                            var rs = db.WS_Recruitment.Find(5);
                            rs.Hide = false;
                            db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Recruitment", "Post");
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

        public ActionResult EditSlider()
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    var rs = db.WS_Slider.Where(s => s.ID == 1);
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

        [HttpPost]
        public ActionResult EditSlider(string caption01, string caption02, string caption03, HttpPostedFileBase image01, HttpPostedFileBase image02, HttpPostedFileBase image03)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    string Image01 = "";
                    if (image01 != null)
                    {
                        if (image01.ContentLength > 0)
                        {
                            var filename = Path.GetFileName(image01.FileName);
                            var fname = filename.Replace(" ", "_");
                            var path = Path.Combine(Server.MapPath("~/images/slider"), fname);
                            image01.SaveAs(path);
                            Image01 += fname;
                        }

                    }
                    string Image02 = "";
                    if (image02 != null)
                    {
                        if (image02.ContentLength > 0)
                        {
                            var filename = Path.GetFileName(image02.FileName);
                            var fname = filename.Replace(" ", "_");
                            var path = Path.Combine(Server.MapPath("~/images/slider"), fname);
                            image02.SaveAs(path);
                            Image02 += fname;
                        }

                    }
                    string Image03 = "";
                    if (image03 != null)
                    {
                        if (image03.ContentLength > 0)
                        {
                            var filename = Path.GetFileName(image03.FileName);
                            var fname = filename.Replace(" ", "_");
                            var path = Path.Combine(Server.MapPath("~/images/slider"), fname);
                            image03.SaveAs(path);
                            Image03 += fname;
                        }

                    }
                    var rs = db.WS_Slider.Find(1);
                    rs.Caption01 = caption01;
                    rs.Caption02 = caption02;
                    rs.Caption03 = caption03;
                    if (Image01 != "")
                    {
                        rs.Image01 = Image01;
                    }
                    if (Image02 != "")
                    {
                        rs.Image02 = Image02;
                    }
                    if (Image03 != "")
                    {
                        rs.Image03 = Image03;
                    }
                    db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Manage");
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

        public async Task<ActionResult> Test(string Fullname, string Phone, RegisterViewModel model)
        {
            AspNetUser cus = new AspNetUser
            {
                FullName = Fullname,
                Phone = Phone,
                UserName = model.UserName,
                PasswordHash = model.Password
            };
            var client = new HttpClient();
            string api = "http://localhost:21360/api/customer/AddCustomer";
            var response = await client.PostAsJsonAsync(api, new { customer = cus }).ConfigureAwait(continueOnCapturedContext: false);
            if (response.IsSuccessStatusCode)
            {
                Json(new ResultInfo() { error = 0, msg = "Thành công" }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}