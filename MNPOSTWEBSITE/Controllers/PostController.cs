using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using PagedList;
using PagedList.Mvc;
using System.Drawing;

namespace MNPOSTWEBSITE.Controllers
{
    public class PostController : Controller
    {
        MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEMODEL.MNPOSTWEBSITEEntities();
        //
        // GET: /Post/
        public ActionResult Create()
        {
            ViewBag.ThumbSize = ThumbSize;
            return View();
        }
        public string picturename= "";
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(string Title, string PostBy,string Service, string Postcontent)
        {
          
            MNPOSTWEBSITEMODEL.WS_Post data = new MNPOSTWEBSITEMODEL.WS_Post();
            data.PostName = Title;
            data.PostBy = PostBy;
            data.Service = Service;
            data.Images = Session["PictureName"].ToString();
            data.CreatedDate = DateTime.Now;
            data.PostContent = Postcontent;
            db.WS_Post.Add(data);
            db.SaveChanges();
            ViewBag.Message = "Đăng bài thành công";
            return RedirectToAction("Index","Manage");
        }

        private const int ThumbSize = 160;
  
        public ActionResult GetFile(string name, bool thumbnail = false)
        {
            var file = GetFile(name);
            var contentType = MimeMapping.GetMimeMapping(file.Name);
            return thumbnail
                ? Thumb(file, contentType)
                : File(file.FullName, contentType);
        }

        private ActionResult Thumb(FileInfo file, string contentType)
        {
            if (contentType.StartsWith("image"))
            {
                try
                {
                    using (var img = Image.FromFile(file.FullName))
                    {
                        var thumbHeight = (int) (img.Height*(ThumbSize/(double) img.Width));

                        using (var thumb = img.GetThumbnailImage(ThumbSize, thumbHeight, () => false, IntPtr.Zero))
                        {
                            var ms = new MemoryStream();
                            thumb.Save(ms, img.RawFormat);
                            ms.Position = 0;
                            return File(ms, contentType);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // todo log exception
                }
            }

            return Redirect("https://placehold.it/{ThumbSize}?text={Url.Encode(file.Extension)}");
        }

        [HttpPost]
        public ActionResult DeleteFile(string name)
        {
            var file = GetFile(name);
            file.Delete();
            return Json($"{name} was deleted");
        }

        [HttpGet]
        public ActionResult Upload(IEnumerable<string> names = null)
        {
            var folder = GetUploadFolder();

            var files = from file in folder.GetFiles()
                        where names == null || names.Contains(file.Name, StringComparer.OrdinalIgnoreCase)
                        select new
                        {
                            deleteType = "POST",
                            name = file.Name,
                            size = file.Length,
                            url = Url.Action("GetFile", "Post", new { file.Name }),
                            thumbnailUrl = Url.Action("GetFile", "Post", new { file.Name, thumbnail = true }),
                            deleteUrl = Url.Action("DeleteFile", "Post", new { file.Name }),
                        };

            return Json(new
            {
                files
            }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult Upload()
        {
            var files = Request.Files
                .Cast<string>()
                .Select(k => Request.Files[k])
                .ToArray();

            foreach (var file in files)
                SaveFileToDisk(file);

            var names = files.Select(f => f.FileName);
            Session["PictureName"] = Request.Files
                .Cast<string>()
                .Select(k => Request.Files[k])
                .FirstOrDefault().FileName;
            return Upload(names);
        }

        private void SaveFileToDisk(HttpPostedFileBase file)
        {
            var folder = GetUploadFolder();
            var targetPath = Path.Combine(folder.FullName, file.FileName);
            file.SaveAs(targetPath);
        }

        private DirectoryInfo GetUploadFolder()
        {
            //var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var upload = Path.Combine(Server.MapPath("~/images/post"));
            var di = new DirectoryInfo(upload);

            if (!di.Exists)
                di.Create();

            return di;
        }

        private FileInfo GetFile(string name)
        {
            var folder = GetUploadFolder();
            var file = folder.GetFiles(name).Single();
            
            return file;
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

        public ActionResult AccountPost(int? page = 1)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var lst = db.WS_Post.ToList();
            return View(lst.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Edit(int? id)
        {
            var pst = db.WS_Post.Where(s => s.ID == id);
            return View(pst);
        }

        public ActionResult Delete(int? id)
        {
            var pst = db.WS_Post.Find(id);
            db.WS_Post.Remove(pst);
            db.SaveChanges();
            return RedirectToAction("AccountPost","Post");
        }
	}
}