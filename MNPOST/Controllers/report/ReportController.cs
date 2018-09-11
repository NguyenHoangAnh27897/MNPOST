using Microsoft.Reporting.WebForms;
using MNPOSTCOMMON;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.ComponentModel;
using MNPOST.Utils;
using System.Data.SqlClient;
using System.Configuration;
using MNPOST.DS;

namespace MNPOST.Controllers.report
{
    public class ReportController : Controller
    {

        MNPOSTEntities db = new MNPOSTEntities();

        MNPOSTDS ds = new MNPOSTDS();
        
        //
        // GET: /Report/
        public ActionResult Index()
        {
            
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MAILER_GETINFO_BYID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@mailerId", SqlDbType.Text)).Value = "MN000020";

            SqlDataAdapter da = new SqlDataAdapter(cmd);
    
            da.Fill(ds, ds.MAILER_GETINFO_BYID.TableName);

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"\Report\mailer.rdlc";

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("MAILERINFO", ds.Tables[ds.MAILER_GETINFO_BYID.TableName]));

            reportViewer.LocalReport.Refresh();
      
            ViewBag.ReportViewer = reportViewer;
            return View();
        }

    }
}