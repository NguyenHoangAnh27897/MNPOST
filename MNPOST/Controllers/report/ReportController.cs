using CrystalDecisions.CrystalReports.Engine;
using MNPOSTCOMMON;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using MNPOST.Models;

namespace MNPOST.Controllers.report
{
    public class ReportController : Controller
    {

        MNPOSTEntities db = new MNPOSTEntities();

        // GET: /Report/
        public ActionResult Index(string mailerId)
        {
            var mailer = db.MAILER_GETINFO_BYLISTID(mailerId).Select(p => new MailerRpt()
            {
                SenderName = p.SenderName,
                MailerID = p.MailerID,
                SenderID = p.SenderID,
                SenderPhone = p.SenderPhone,
                SenderAddress = p.SenderAddress,
                RecieverName = p.RecieverName,
                RecieverAddress = p.RecieverAddress,
                RecieverPhone = p.RecieverPhone,
                ReceiverDistrictName = p.ReceiverDistrictName,
                ReceiverProvinceName = p.ReceiverProvinceName,
                SenderDistrictName = p.SenderDistrictName,
                SenderProvinceName = p.SenderProvinceName,
                PostOfficeName = p.PostOfficeName,
                TL = p.MerchandiseID == "T" ? "X" : " ",
                HH = p.MerchandiseID == "H" ? "X" : " ",
                MH = p.MerchandiseID == "M" ? "X" : " ",
                N = p.MailerTypeID == "SN" ? "X" : " ",
                DB = p.MerchandiseID == "ST" ? "X" : " ",
                TK = p.MerchandiseID == "TK" ? "X" : " ",
            }).ToList();


            ReportDocument rptH = new ReportDocument();
            string reportPath = Server.MapPath("~/Report/MNPOSTReport.rpt");
            rptH.Load(reportPath);

            rptH.SetDataSource(mailer);

            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(stream, "application/pdf");
        }

    }
}