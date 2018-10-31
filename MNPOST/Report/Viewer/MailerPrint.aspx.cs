using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MNPOSTCOMMON;
using MNPOST.Models;
using CrystalDecisions.CrystalReports.Engine;

namespace MNPOST.Report.Viewer
{
    public partial class MailerPrint : System.Web.UI.Page
    {

        MNPOSTEntities db = new MNPOSTEntities();
        ReportDocument rptH;
        string searchText = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            searchText = Request.QueryString["mailer"];
            LoadReport();
        }

        private void LoadReport()
        {

            var mailer = db.MAILER_GETINFO_BYLISTID1(searchText).Select(p=> new MailerRpt()
            {
                SenderName = p.SenderName,
                MailerID = "*"+ p.MailerID +"*",
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
                PostOfficeName = p.PostOfficeName
            }).ToList();

            if (mailer == null)
                return;

            if(rptH != null)
            {
                rptH.Close();
                rptH.Dispose();
            }

            rptH = new ReportDocument();
            string reportPath = Server.MapPath("~/Report/MNPOSTReport.rpt");
            rptH.Load(reportPath);

            rptH.SetDataSource(mailer);


            MailerRptViewer.ReportSource = rptH;
        }

        protected void btnprint_Click(object sender, EventArgs e)
        {
            if(rptH != null)
            {
                try
                {
                    rptH.PrintToPrinter(1, false, 0, 0);
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Không tìm thấy thiết bị in')", true);
                }
            }
        }
    }
}