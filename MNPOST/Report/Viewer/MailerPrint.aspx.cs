using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MNPOSTCOMMON;
using CrystalDecisions.CrystalReports.Engine;

namespace MNPOST.Report.Viewer
{
    public partial class MailerPrint : System.Web.UI.Page
    {

        MNPOSTEntities db = new MNPOSTEntities();
        string searchText = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            searchText = Request.QueryString["mailer"];
            LoadReport();
        }

        private void LoadReport()
        {
            var mailer = db.MM_Mailers.Find(searchText);

            if (mailer == null)
                return;

            ReportDocument rptH = new ReportDocument();
            string reportPath = Server.MapPath("~/Report/MNPOSTReport.rpt");
            rptH.Load(reportPath);
            TextObject _txtSenderName = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtSenderName"];
            _txtSenderName.Text = mailer.SenderName;
            TextObject _txtSenderAddres = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtSenderAddress"];
            _txtSenderAddres.Text = mailer.SenderAddress;
            TextObject _txtSenderPhone = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtSenderPhone"];
            _txtSenderPhone.Text = mailer.SenderPhone;
            TextObject _txtReceiverName = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtReceiverName"];
            _txtReceiverName.Text = mailer.RecieverName;
            TextObject _txtReceiverAddress = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtReceiverAddress"];
            _txtReceiverAddress.Text = mailer.RecieverAddress;
            TextObject _txtReceiverPhone = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtReceiverPhone"];
            _txtReceiverPhone.Text = mailer.RecieverPhone;
            TextObject _txtCOD = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtCOD"];
            _txtCOD.Text = mailer.COD.ToString();
            if (mailer.MerchandiseID == "H")
            {
                TextObject _txtTT = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtTT"];
                _txtTT.Text = "";
                TextObject _txtHH = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtHH"];
                _txtHH.Text = "X";
                TextObject _txtMHang = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtMHang"];
                _txtMHang.Text = "";
                if (mailer.MailerTypeID == "SN")
                {
                    TextObject _txtNhanh = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtNhanh"];
                    _txtNhanh.Text = "X";
                    TextObject _txtDBo = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtDBo"];
                    _txtDBo.Text = "";
                    TextObject _txtTK = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtTK"];
                    _txtTK.Text = "";
                }
                else if (mailer.MailerTypeID == "ST")
                {
                    TextObject _txtNhanh = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtNhanh"];
                    _txtNhanh.Text = "";
                    TextObject _txtDBo = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtDBo"];
                    _txtDBo.Text = "X";
                    TextObject _txtTK = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtTK"];
                    _txtTK.Text = "";
                }
            }
            else if (mailer.MerchandiseID == "T")
            {
                TextObject _txtTT = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtTT"];
                _txtTT.Text = "X";
                TextObject _txtHH = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtHH"];
                _txtHH.Text = "";
                if (mailer.MailerTypeID == "SN")
                {
                    TextObject _txtNhanh = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtNhanh"];
                    _txtNhanh.Text = "X";
                    TextObject _txtDBo = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtDBo"];
                    _txtDBo.Text = "";
                    TextObject _txtTK = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtTK"];
                    _txtTK.Text = "";
                }
                else if (mailer.MailerTypeID == "ST")
                {
                    TextObject _txtNhanh = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtNhanh"];
                    _txtNhanh.Text = "";
                    TextObject _txtDBo = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtDBo"];
                    _txtDBo.Text = "X";
                    TextObject _txtTK = (TextObject)rptH.ReportDefinition.Sections["Section3"].ReportObjects["txtTK"];
                    _txtTK.Text = "";
                }
            }



            CrystalReportViewer1.ReportSource = rptH;
        }



        protected void mailercrytalviewer_PreRender(object sender, EventArgs e)
        {

        }
    }
}