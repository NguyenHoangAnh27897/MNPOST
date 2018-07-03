using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;

namespace MNPOSTWEBSITE.Controllers
{
    public class PostController : Controller
    {
        MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities();
        //
        // GET: /Post/
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(string Title, string PostBy, HttpPostedFileBase picture, string Postcontent)
        {
            string pic = "";
            if (picture.ContentLength > 0)
            {
                var filename = Path.GetFileName(picture.FileName);
                var path = Path.Combine(Server.MapPath("~/images/post"), picture.FileName);
                picture.SaveAs(path);
                pic = picture.FileName;
            }
            MNPOSTWEBSITEMODEL.WS_Post data = new MNPOSTWEBSITEMODEL.WS_Post();
            data.PostName = Title;
            data.PostBy = PostBy;
            data.Images = pic;
            data.CreatedDate = DateTime.Now;
            data.PostContent = Postcontent;
            db.WS_Post.Add(data);
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }

        public ActionResult DetailPost(int id)
        {
            return View();
        }
	}
}