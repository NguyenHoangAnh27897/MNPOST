using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNPOST.Models;
using MNPOSTCOMMON;
using MNPOST.Utils;

namespace MNPOST.Controllers
{

    [Authorize(Roles = "user")]
    public class BaseController : Controller
    {

        protected MNPOSTEntities db = new MNPOSTEntities();


        protected bool checkAccess(string menu, int edit)
        {
            var user = User.Identity.Name;

            var roles = db.USER_GETROLE(user).ToList();


            if (roles == null || roles.Count() == 0)
                return false;

            var checkAdmin = roles.Where(p => p.Name == "admin").FirstOrDefault();

            if (checkAdmin != null)
                return true;


            var groupId = roles.First().GroupId;

            var getAccess = db.USER_CHECKACCESS(groupId, menu).FirstOrDefault();

            if (getAccess == null)
                return false;



            if (getAccess.CanEdit == edit)
                return true;


            return false;
        }

        protected bool checkAccess(string menu)
        {
            var user = User.Identity.Name;

            var roles = db.USER_GETROLE(user).ToList();


            if (roles == null || roles.Count() == 0)
                return false;

            var checkAdmin = roles.Where(p => p.Name == "admin").FirstOrDefault();

            if (checkAdmin != null)
                return true;


            var groupId = roles.First().GroupId;

            var getAccess = db.USER_CHECKACCESS(groupId, menu).FirstOrDefault();

            if (getAccess != null)
                return true;

            return false;
        }


        [HttpGet]
        public ActionResult Menus()
        {
            var user = User.Identity.Name;

            var getMenu = db.USER_GETMENU(user).ToList();

            List<GroupMenuInfo> groupMenus = new List<GroupMenuInfo>();

            var listGroup = db.UMS_GroupMenu.OrderBy(p => p.Position).ToList();

            foreach (var item in listGroup)
            {
                GroupMenuInfo groupInfo = new GroupMenuInfo()
                {
                    name = item.Name,
                    icon = item.Icon,
                    menus = new List<MenuInfo>()
                };

                var listMenu = getMenu.Where(p => p.GroupMenuId == item.Id).OrderBy(p => p.Position).ToList();

                if (listMenu.Count() > 0)
                {
                    foreach (var menuItem in listMenu)
                    {
                        groupInfo.menus.Add(new MenuInfo()
                        {
                            name = menuItem.Name,
                            link = menuItem.Link
                        });
                    }
                    groupMenus.Add(groupInfo);
                }
            }

            return PartialView("_MenuUser", groupMenus);
        }
    }
}