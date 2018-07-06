using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ASPSnippets.FaceBookAPI;

namespace MNPOSTWEBSITE.Controllers
{
    public class HomeController : Controller
    {
        MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities();
        public ActionResult Index()
        {
            FaceBookConnect.API_Key = "273429170069624";
            FaceBookConnect.API_Secret = "01dda29a1052ab142ad14ca4d208d48b";

            MNPOSTWEBSITEMODEL.WS_FacebookUser faceBookUser = new MNPOSTWEBSITEMODEL.WS_FacebookUser();
            if (Request.QueryString["error"] == "access_denied")
            {
                ViewBag.Message = "User has denied access.";
            }
            else
            {
                string code = Request.QueryString["code"];
                if (!string.IsNullOrEmpty(code))
                {
                    string data = FaceBookConnect.Fetch(code, "me?fields=id,name,email");
                    faceBookUser = new JavaScriptSerializer().Deserialize<MNPOSTWEBSITEMODEL.WS_FacebookUser>(data);
                    faceBookUser.PictureUrl = string.Format("https://graph.facebook.com/{0}/picture", faceBookUser.ID);
                }
            }
            //Session["Username"] = faceBookUser.Name;
            return View();
        }
    }
}