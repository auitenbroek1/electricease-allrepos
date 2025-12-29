<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientEstimationReport.aspx.cs" Inherits="ElectricEase.Web.ClientEstimationReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            var iframe_parent_div = window.frameElement ? window.frameElement.parentNode : null;
            var tomail = $(iframe_parent_div).parent().parent().parent().find('#ToMail');
            var subj = $(iframe_parent_div).parent().parent().parent().find('#txtsubject');
            $(tomail).val($('#<% = hdnclientid.ClientID %>').val());
            $(subj).val($('#<%= Subject.ClientID%>').val());
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" name="Subject" id="Subject" runat="server" />
        <input type="hidden" name="Clientname" id="Clientname" runat="server" />
         <input type="hidden" name="jobClientname" id="jobClientname" runat="server" />
        <input type="hidden" name="desc" id="desc" runat="server" />
        <input type="hidden" name="MailID" id="MailID" runat="server" />
         <input type="hidden" name="MailID" id="CCmailID" runat="server" />
        <asp:HiddenField ID="hdnclientid" runat="server" />
         <asp:Button name="click" ID="clickbtn" runat="server" Text="click" OnClick="click_Click" style="display:none" />
    <div>
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="600px" Font-Names="Verdana" ForeColor="Black" BackColor="Silver" Font-Size="9pt" ShowExportControls="true" ShowPrintButton="true" AsyncRendering="false" SizeToReportContent="false" Width="100%"></rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
