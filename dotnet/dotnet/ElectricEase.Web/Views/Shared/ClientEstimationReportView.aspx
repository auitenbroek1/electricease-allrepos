<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        table {
            list-style-image: none;
        }

            table td {
                padding-left: 50px;
                padding: 0px 0px 0px 0px;
                border: 0 none;
            }

        input, textarea {
        }

        image {
            padding: 0px 0px 0px 0px;
        }
    </style>
    <script type="text/javascript">
        $(".content-title").text("Client Estimation Report");
        $(document).ready(function () {
            if ($.browser.webkit) {
                $(".ms-report-viewer-control :nth-child(3) table").each(function (i, item) {
                    $(item).css('display', 'inline-block');
                });
            }
           

        });
    </script>
    <script runat="server">
    
        List<ElectricEase.Data.DataBase.Account_Master> parts = null;
        List<ElectricEase.Data.DataBase.Job_Master> JobMaster = null;
        List<ElectricEase.Data.DataBase.Job_Legal> JobLegal = new List<ElectricEase.Data.DataBase.Job_Legal>();
        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int jobID = Convert.ToInt32(ViewData["MyVar"]);
                ElectricEase.BLL.ClientMaster.ClientMasterBLL ClientResponse = new ElectricEase.BLL.ClientMaster.ClientMasterBLL();
                ElectricEase.Models.ServiceResultList<ElectricEase.Models.Job_MasterInfo> response = new ElectricEase.Models.ServiceResultList<ElectricEase.Models.Job_MasterInfo>();
                string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                var result = ClientResponse.GetClientName(Loginuser);
                using (ElectricEase.Data.DataBase.ElectricEaseEntitiesContext db = new ElectricEase.Data.DataBase.ElectricEaseEntitiesContext())
                {
                    var data = (from a in db.Account_Master where a.Client_ID == result.Client_ID select a).FirstOrDefault();
                    var jobMasterData = (from a in db.Job_Master where a.Client_Id == result.Client_ID && a.Job_ID == jobID select a).FirstOrDefault();
                    var jobLegalData = (from a in db.Job_Legal where a.Client_ID == result.Client_ID && a.Job_ID == jobID select a).FirstOrDefault();
                    JobMaster = new List<ElectricEase.Data.DataBase.Job_Master>();
                    if (data != null)
                    {
                        parts = new List<ElectricEase.Data.DataBase.Account_Master>();
                        parts.Add(new ElectricEase.Data.DataBase.Account_Master { Client_ID = data.Client_ID, Photo = data.Photo });
                        JobMaster.Add(new ElectricEase.Data.DataBase.Job_Master
                        {
                            Job_ID = jobMasterData.Job_ID,
                            Job_Number=jobMasterData.Job_Number,
                            Client_Address = jobMasterData.Client_Address,
                            Client_Email = jobMasterData.Client_Email,
                            Client_Name=jobMasterData.Client_Name,
                            Work_Address = jobMasterData.Work_Address,
                            Job_Description = jobMasterData.Job_Description,
                            Job_EstimatedTotal = jobMasterData.Job_EstimatedTotal,
                            EstimateValue_SubTotal = jobMasterData.EstimateValue_SubTotal,
                            SalesTax = jobMasterData.SalesTax,
                            Doing_What = jobMasterData.Doing_What,
                            EstimateValue_Profit_EstTotal = jobMasterData.Job_EstimatedTotal + Convert.ToDecimal(jobMasterData.SalesTax)
                        });
                        if (jobLegalData != null)
                            JobLegal.Add(new ElectricEase.Data.DataBase.Job_Legal { Legal_Detail = jobLegalData.Legal_Detail.TrimStart() });
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/ClientEstimation.rdlc");
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportDataSource rdc = new ReportDataSource("DataSet1", parts);
                        ReportViewer1.LocalReport.DataSources.Add(rdc);
                        ReportDataSource jobMasterrdc = new ReportDataSource("DataSet2", JobMaster);
                        ReportViewer1.LocalReport.DataSources.Add(jobMasterrdc);
                        ReportDataSource jobLegalrdc = new ReportDataSource("DataSet3", JobLegal);
                        ReportViewer1.LocalReport.DataSources.Add(jobLegalrdc);
                        ReportViewer1.LocalReport.Refresh();
                    }
                }
            }
        }

        protected void btn_Click(object sender, EventArgs e)
        {

        }
</script>
</head>
<body>
    <form id="form1" runat="server">
       <asp:Button id="btn" runat="server" OnClick="btn_Click"/>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="600px" Font-Names="Verdana" ForeColor="Black" BackColor="Silver" Font-Size="9pt" ShowExportControls="true" ShowPrintButton="true" AsyncRendering="false" SizeToReportContent="false" Width="100%"></rsweb:ReportViewer>
        </div>
        

    </form>
</body>
</html>
