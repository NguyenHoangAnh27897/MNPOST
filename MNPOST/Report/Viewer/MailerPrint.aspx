<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MailerPrint.aspx.cs" Inherits="MNPOST.Report.Viewer.MailerPrint" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Show Report</title>
</head>
<body>
     <form id="form1" runat="server">
     <h1>XEM PHIẾU</h1>
         <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
     </form>
    </body>
</html>
