using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using PagedList;
using PagedList.Mvc;

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
        public ActionResult Create(string Title, string PostBy,string Service, HttpPostedFileBase picture, string Postcontent)
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
            data.Service = Service;
            data.Images = pic;
            data.CreatedDate = DateTime.Now;
            data.PostContent = Postcontent;
            db.WS_Post.Add(data);
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }

        public ActionResult DetailPost(int id)
        {
            var pst = db.WS_Post.Where(s => s.ID == id);
            return View(pst);
        }

        public ActionResult PostList(int? page = 1)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var lst = db.WS_Post.ToList();
            return View(lst.ToPagedList(pageNumber,pageSize));
        }
	}
}