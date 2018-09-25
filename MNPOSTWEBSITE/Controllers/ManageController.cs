using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTWEBSITEMODEL;

namespace MNPOSTWEBSITE.Controllers
{
    public class ManageController : Controller
    {
        MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEEntities();
        //
        // GET: /Manage/
        public ActionResult Index()
        {
            if(Session["Authentication"].ToString() != null)
            {
                string id = Session["ID"].ToString();
                var user = db.AspNetUsers.Where(s => s.Id == id);
                return View(user);
            }
            else
            {
                return RedirectToAction("Login","Account");
            }
           
        }

        public ActionResult EditAboutus()
        {
            try
            {
                if (Session["RoleID"].ToString().Equals("Admin"))
                {
                    if (Session["Authentication"] != null)
                    {
                        var rs = db.WS_AboutUs.Where(s=>s.ID == "firstabu");
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
                }catch(Exception ex)
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
                if (Session["RoleID"].ToString().Equals("Admin"))
                {
                    if (Session["Authentication"] != null)
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
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Manage");
            }
        }

        public ActionResult ManageService()
        {
            try
            {
                var lst = db.WS_ServiceType.ToList();
                return View(lst);
            }
            catch(Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        
        }

        public ActionResult EditService(string ID)
        {
            try
            {
                int id = int.Parse(ID);
                var rs = db.WS_ServiceType.Where(s=>s.ID == id);
                return View(rs);
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
                return RedirectToAction("ManageService","Manage");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }
    }
}