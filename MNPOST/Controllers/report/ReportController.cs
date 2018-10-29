using MNPOSTCOMMON;
using System.Web.Mvc;

namespace MNPOST.Controllers.report
{
    public class ReportController : Controller
    {

        MNPOSTEntities db = new MNPOSTEntities();

        // GET: /Report/
        public ActionResult Index(string mailerId)
        {
            var data = db.MAILER_GETINFO_BYID(mailerId);

            return View();
        }

    }
}