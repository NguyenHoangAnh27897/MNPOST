using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOSTCOMMON;

namespace MNPOST.Controllers.manage
{

    public class MenuController : AdminController
    {
        //
        // GET: /Menu/
        public ActionResult Show(string search = "", string group = "all", string msg = "")
        {

            ViewBag.GroupMenu = db.UMS_GroupMenu.OrderBy(p => p.Position).ToList();

            ViewBag.Search = search;
            ViewBag.GroupId = group;

            ViewBag.MSG = msg;

            if (group == "all")
            {
                return View(db.UMS_Menu.Where(p => p.Name.Contains(search)).OrderBy(p => p.GroupMenuId).OrderBy(p => p.Position).ToList());
            }

            return View(db.UMS_Menu.Where(p => p.Name.Contains(search) && p.GroupMenuId == group).OrderBy(p => p.GroupMenuId).OrderBy(p => p.Position).ToList());

        }


        [HttpPost]
        public ActionResult AddMenu(string name, string code, string link, string group, int? position)
        {

            if (String.IsNullOrEmpty(code))
                return Redirect("/error");

            code = code.Trim();
            var menu = new UMS_Menu()
            {
                Id = Guid.NewGuid().ToString(),
                GroupMenuId = group,
                Link = link,
                Name = name,
                Code = code,
                Position = position
            };

            db.UMS_Menu.Add(menu);

            return RedirectToAction("show", "menu", new { msg = "Đã thêm menu" });
        }

        public ActionResult Group()
        {
            return View(db.UMS_GroupMenu.ToList());
        }

        [HttpPost]
        public ActionResult AddGroup(string name, string icon, int? position)
        {
            var data = new UMS_GroupMenu()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Position = position,
                Icon = icon
            };

            db.UMS_GroupMenu.Add(data);
            db.SaveChanges();
            return RedirectToAction("group", "menu");
        }

        [HttpPost]
        public ActionResult EditGroup(string id, string name, string icon, int? position)
        {
            var check = db.UMS_GroupMenu.Find(id);

            if (check == null)
                return Redirect("/error");

            check.Name = name;
            check.Position = position;
            check.Icon = icon;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("group", "menu");
        }



        //
        public ActionResult GrouUserMenu(string id = "")
        {

            ViewBag.GroupUser = db.UMS_UserGroups.ToList();

            ViewBag.GroupChoose = id;

            if (id != "")
            {
                var data = db.GROUPUSER_GETLISTMENU(id).ToList();

                return View(data);
            }



            return View();
        }


        [HttpPost]
        public ActionResult AddRole(string menuId, string groupId, bool role)
        {
            var check = db.UMS_MenuGroupUser.Where(p => p.MenuId == menuId && p.GroupUserId == groupId).FirstOrDefault();

            var checkMenu = db.UMS_Menu.Find(menuId);

            if (checkMenu == null)
                return Json(new { id = 0, msg = "Lỗi khi cấp quyền" }, JsonRequestBehavior.AllowGet);


            var checkGroup = db.UMS_UserGroups.Find(groupId);

            if (checkGroup == null)
                return Json(new { id = 0, msg = "Lỗi khi cấp quyền" }, JsonRequestBehavior.AllowGet);


            if (role)
            {

                if (check == null)
                {
                    var data = new UMS_MenuGroupUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        GroupUserId = groupId,
                        CanEdit = 0,
                        MenuId = menuId
                    };

                    db.UMS_MenuGroupUser.Add(data);
                    db.SaveChanges();

                    return Json(new { id = 1, msg = "Đã cấp quyền" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (check != null)
                {
                    db.UMS_MenuGroupUser.Remove(check);
                    db.SaveChanges();
                }

                return Json(new { id = 1, msg = "Đã bỏ quyền" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { id = 0, msg = "Lỗi khi cấp quyền" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddEdit(string menuId, string groupId, bool role)
        {
            var check = db.UMS_MenuGroupUser.Where(p => p.MenuId == menuId && p.GroupUserId == groupId).FirstOrDefault();

            if (check != null)
            {
                check.CanEdit = role ? 1 : 0;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { id = 1, msg = "Đã thực hiện" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { id = 0, msg = "Lỗi khi cấp quyền" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}