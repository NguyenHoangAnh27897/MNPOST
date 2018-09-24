using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MNPOSTWEBSITE.Models;
using MNPOSTCOMMON;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

namespace MNPOSTWEBSITE.Controllers
{
    public class BaseController : Controller
    {
        protected SqlConnection conn;
        protected RoleManager<IdentityRole> RoleManager { get; private set; }

        protected UserManager<ApplicationUser> UserManager { get; private set; }
        private ApplicationDbContext sdb = new ApplicationDbContext();

        public BaseController()
        {
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(sdb));
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(sdb));
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        protected SqlDataAdapter GetSqlDataAdapter(string store, Dictionary<string, string> parameter)
        {
            SqlCommand cmd = new SqlCommand(store, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameter != null)
            {
                foreach (var item in parameter)
                {
                    cmd.Parameters.Add(new SqlParameter(item.Key, SqlDbType.Text)).Value = item.Value;
                }
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            return da;
        }
    }
}