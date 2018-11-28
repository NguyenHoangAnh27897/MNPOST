﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using PagedList;
using PagedList.Mvc;
using System.Drawing;
using MNPOSTWEBSITE.Models;

namespace MNPOSTWEBSITE.Controllers
{
    public class PostController : Controller
    {
        MNPOSTWEBSITEEntities db = new MNPOSTWEBSITEEntities();
        //
        // GET: /Post/
        public ActionResult Create()
        {
            if (Session["Authentication"] != null)
            {
                if (Session["RoleID"].ToString().Equals("Admin"))
                {
                    ViewBag.ThumbSize = ThumbSize;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index","Manage");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
          
        }
        public string picturename= "";
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(string Title, string PostBy,string Service, string Postcontent, HttpPostedFileBase filepdf, HttpPostedFileBase avatar)
        {
            try
            {
                WS_Post data = new WS_Post();
                data.PostName = Title;
                data.PostBy = PostBy;
                data.Service = Service;
                string Avatar = "";
                if (avatar != null)
                {
                    if (avatar.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(avatar.FileName);
                        var fname = filename.Replace(" ", "_");
                        var path = Path.Combine(Server.MapPath("~/images/post"), fname);
                        avatar.SaveAs(path);
                        Avatar += fname;
                    }

                }
                if (Avatar != "")
                {
                    data.Avatar = Avatar;
                }
                data.CreatedDate = DateTime.Now;
                data.ContentPost = Postcontent;
                db.WS_Post.Add(data);
                db.SaveChanges();
                ViewBag.Message = "Đăng bài thành công";
                return RedirectToAction("Index", "Manage");
            }
            catch(Exception ex)
            {
                return RedirectToAction("ErrorPage","Error");
            }
          
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
            try
            {
                var pst = db.WS_Post.Where(s => s.ID == id);
                return View(pst);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        public ActionResult PostList(int? page = 1)
        {
            try
            {
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                var lst = db.WS_Post.ToList();
                return View(lst.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        public ActionResult AccountPost(int? page = 1)
        {
            if (Session["Authentication"] != null)
            {
                if (Session["RoleID"].ToString().Equals("Admin"))
                {
                    try
                    {
                        int pageSize = 5;
                        int pageNumber = (page ?? 1);
                        var lst = db.WS_Post.ToList();
                        return View(lst.ToPagedList(pageNumber, pageSize));
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("ErrorPage", "Error");
                    }
                }
                else
                {
                    return RedirectToAction("Index","Manage");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (Session["Authentication"] != null)
            {
                if (Session["RoleID"].ToString().Equals("Admin"))
                {
                    try
                    {
                        var pst = db.WS_Post.Find(id);
                        return View(pst);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("ErrorPage", "Error");
                    }
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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int ID, string Title, string PostBy, string Service, string Postcontent, HttpPostedFileBase filepdf, HttpPostedFileBase avatar)
        {
            try
            {
                if (Session["Authentication"] != null)
                {
                    if (Session["RoleID"].ToString().Equals("Admin"))
                    {
                        var data = db.WS_Post.Find(ID);
                        data.PostName = Title;
                        data.PostBy = PostBy;
                        data.Service = Service;
                        string Avatar = "";
                        if (avatar != null)
                        {
                            if (avatar.ContentLength > 0)
                            {
                                var filename = Path.GetFileName(avatar.FileName);
                                var fname = filename.Replace(" ", "_");
                                var path = Path.Combine(Server.MapPath("~/images/post"), fname);
                                avatar.SaveAs(path);
                                Avatar += fname;
                            }

                        }
                        if (Avatar != "")
                        {
                            data.Avatar = Avatar;
                        }
                        data.CreatedDate = DateTime.Now;
                        data.ContentPost = Postcontent;
                        db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Message = "Chỉnh sửa bài thành công";
                        return RedirectToAction("AccountPost");
                    }
                    return RedirectToAction("Login", "Account");
                }
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }

        }

        public ActionResult Delete(int? id)
        {
            try
            {
                var pst = db.WS_Post.Find(id);
                db.Entry(pst).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("AccountPost", "Post");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
        }

        public ActionResult Recruitment()
        {
            if (Session["Authentication"] != null)
            {
                if (Session["RoleID"].ToString().Equals("Admin"))
                {
                    try
                    {
                        var lst = db.WS_Recruitment.ToList();
                        return View(lst);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("ErrorPage", "Error");
                    }
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

        public ActionResult EditRecruitment(string ID)
        {
            if (Session["Authentication"] != null)
            {
                if (Session["RoleID"].ToString().Equals("Admin"))
                {
                    try
                    {
                        int id = int.Parse(ID);
                        var rs = db.WS_Recruitment.Where(s => s.ID == id);
                        return View(rs);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("ErrorPage", "Error");
                    }
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

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditRecruitment(string ID, string Recruitment, string Content)
        {
            if (Session["Authentication"] != null)
            {
                if (Session["RoleID"].ToString().Equals("Admin"))
                {
                    try
                    {
                        int id = int.Parse(ID);
                        var rs = db.WS_Recruitment.Find(id);
                        rs.Name = Recruitment;
                        rs.ContentRecruitment = Content;
                        db.Entry(rs).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Recruitment", "Post");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("ErrorPage", "Error");
                    }
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

        public ActionResult PricePage()
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

        [HttpPost]
        public ActionResult UploadPDF(HttpPostedFileBase FilePDF)
        {
            if (Session["Authentication"] != null)
            {
                if (Session["RoleID"].ToString().Equals("Admin"))
                {
                    try
                    {
                        if(Request.Files["FilePDF"].ContentLength > 0)
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
                            }
                        }
                        return RedirectToAction("AccountPost", "Post");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("ErrorPage", "Error");
                    }
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
    }
}