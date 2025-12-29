<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        table {
            list-style-image: none;
        }

            table td {
                padding-left: 50px;
                padding: 0px 0px 0px 0px;
                /*border: 0 none;*/
            }

        input, textarea {
        }

        image {
            padding: 0px 0px 0px 0px;
        }
    </style>
    <script type="text/javascript">
        $(".content-title").text("Work Orders");
        $(document).ready(function () {
            var isIE =/*@cc_on!@*/false || !!document.documentMode;
            var isEdge = !isIE && !!window.StyleMedia;
            if (isEdge == true || isIE) {
                $("#ReportViewer1").css("overflow", "auto");
            }

            if ($.browser.webkit) {
                $(".ms-report-viewer-control :nth-child(3) table").each(function (i, item) {
                    $(item).css('display', 'inline-block');
                });

            }
            var re = $('#<% = ReportViewer1.ClientID%>');
            var s = $('#<% = ReportViewer1.ClientID%>').find('a');
            for (var i = 0; i < s.length; i++) {
                if (s[i].innerHTML == "Excel" || s[i].innerHTML.toUpperCase() == "PDF" || s[i].innerHTML.toUpperCase() == "WORD") {
                    var onclic = $(s[i]).attr("onclick");
                    $(s[i]).attr("onclick", "if(confirm('To download, \"Associated Files & Job Description\" click \"OK\" or \"Cancel\" for \"Job Description\" only.')){" + onclic + "downloadfiles();}else{" + onclic + "}");
                }
            }

            var first = $(re).find("input[title='First Page']").first();
            var next = $(re).find("input[title='Next Page']").first();
            var prev = $(re).find("input[title='Previous Page']").first();
            var last = $(re).find("input[title='Last Page']").first();
            var totalpages = $('#ReportViewer1_ctl05_ctl00_TotalPages').text();
            $(next).click(function () {
                var currpage = 1;
                if (Cookies.get('currentpage') != null && Cookies.get('currentpage') != "")
                    currpage = Cookies.get('currentpage');
                currpage = Number(currpage) + 1;
                Cookies.set('currentpage', currpage);

                window.location.reload();
            });
            $(prev).click(function () {
                var currpage = 2;
                if (Cookies.get('currentpage') != null && Cookies.get('currentpage') != "")
                    currpage = Cookies.get('currentpage');
                currpage = Number(currpage) - 1;
                Cookies.set('currentpage', currpage);
                window.location.reload();
            });
            $(first).click(function () {
                Cookies.set('currentpage', 1);
                window.location.reload();
            });
            $(last).click(function () {
                Cookies.set('currentpage', Number(totalpages));
                window.location.reload();
            });
        });
        function downloadfiles() {
            window.open('../Job/Downloadattachment?JobId=<%=Request.QueryString["JobID"]%>');

        }

    </script>
    <script runat="server">
        List<ElectricEase.Data.DataBase.Client_Master> parts = null;
        List<ElectricEase.Models.Report_Jobs_Parts> Jobparts = null;
        List<ElectricEase.Data.DataBase.Job_Master> JobMaster = null;
        List<ElectricEase.Data.DataBase.Job_Labor> JobLabor = null;
        List<ElectricEase.Data.DataBase.Job_Assembly_Parts> JobAssemblyParts = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ReportViewer1.Reset();
                    int jobID = Convert.ToInt32(ViewData["MyVar"]);
                    int pageno = Convert.ToInt32(ViewData["PageNo"]);
                    ElectricEase.BLL.ClientMaster.ClientMasterBLL ClientResponse = new ElectricEase.BLL.ClientMaster.ClientMasterBLL();
                    ElectricEase.Models.ServiceResultList<ElectricEase.Models.Job_MasterInfo> response = new ElectricEase.Models.ServiceResultList<ElectricEase.Models.Job_MasterInfo>();
                    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                    var result = ClientResponse.GetClientName(Loginuser);
                    using (ElectricEase.Data.DataBase.ElectricEaseEntitiesContext db = new ElectricEase.Data.DataBase.ElectricEaseEntitiesContext())
                    {
                        var data = (from a in db.Account_Master where a.Client_ID == result.Client_ID select a).FirstOrDefault();
                        var jobPartsData = (from m in db.Job_Parts where m.Client_ID == result.Client_ID && m.Job_ID == jobID select m).ToList();
                        var jobMasterData = (from a in db.Job_Master where a.Client_Id == result.Client_ID && a.Job_ID == jobID select a).FirstOrDefault();
                        var jobLaborData = (from s in db.Job_Labor where s.Client_Id == result.Client_ID && s.Job_ID == jobID select s).ToList();
                        var jobAssemblyPartsData = (from b in db.Job_Assembly_Parts where b.Client_ID == result.Client_ID && b.Job_ID == jobID select b).ToList();
                        List<ElectricEase.Models.Parts_DetailsInfo> JobPartsresult = new List<ElectricEase.Models.Parts_DetailsInfo>();
                        string Job_ID = Convert.ToString(jobID);
                        JobPartsresult = db.Database.SqlQuery<ElectricEase.Models.Parts_DetailsInfo>("exec EE_GetAssembliesJobParts @JobID", new System.Data.SqlClient.SqlParameter("JobID", Job_ID)).ToList();
                        //var assem = db.Job_AssembliesDetails.Where(x => x.Job_ID == jobID).ToList();
                        //foreach (var assemitem in assem)
                        //{

                        //}
                        //foreach(var item in JobPartsresult)
                        //{
                        //    var count = (from m in JobPartsresult where m.Part_Number == item.Part_Number select m.Part_Number).Count();
                        //}
                        Jobparts = new List<ElectricEase.Models.Report_Jobs_Parts>();
                        JobMaster = new List<ElectricEase.Data.DataBase.Job_Master>();
                        if (jobMasterData.Job_ID != null)
                        {
                            parts = (from a in db.Client_Master where a.Client_ID == result.Client_ID select a).ToList();
                            if (parts.Count > 0)
                            {
                                if (parts[0].State != null && parts[0].State != "")
                                    //parts[0].State = ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == parts[0].State.ToUpper()).Select(x => x.Name).FirstOrDefault().Replace(parts[0].State.ToUpper(), "").Replace("- ", "");
                                    parts[0].State = ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == parts[0].State.ToUpper()).Select(x => x.Value).FirstOrDefault();
                            }
                            foreach (var jobParts in JobPartsresult)
                            {
                                Jobparts.Add(new ElectricEase.Models.Report_Jobs_Parts
                                {
                                    Part_Number = jobParts.Part_Number,
                                    Estimated_Qty = jobParts.Estimated_Qty,


                                });
                            }
                            Jobparts = Jobparts.OrderBy(x => x.Part_Number).ToList();
                            Jobparts = (from j in Jobparts
                                        group j by j.Part_Number into grp
                                        select new ElectricEase.Models.Report_Jobs_Parts
                                        {
                                            Part_Number = grp.Key,
                                            Estimated_Qty = grp.Sum(x => x.Estimated_Qty),
                                            Part_Desc = db.Parts_Details.Where(x => x.Part_Number == grp.Key && x.Client_Id == result.Client_ID).Select(x => x.Description).FirstOrDefault(),
                                            Category = db.Parts_Details.Where(x => x.Part_Number == grp.Key && x.Client_Id == result.Client_ID).Select(x => x.Part_Category).FirstOrDefault(),
                                            // Count = JobPartsresult.Where(x => x.Part_Number == grp.Key).Count()
                                        }).OrderBy(x => x.Category).ToList();
                            JobMaster.Add(new ElectricEase.Data.DataBase.Job_Master
                            {
                                Job_ID = jobMasterData.Job_ID,
                                Job_Number = jobMasterData.Job_Number,
                                Job_Description = jobMasterData.Job_Description,
                                Client_Email = jobMasterData.Client_Email,
                                Client_Id = jobMasterData.Client_Id,
                                Assemblies_Name = jobMasterData.Assemblies_Name,
                                Client_Name = jobMasterData.Client_Name,
                                Client_Address = jobMasterData.Client_Name + (jobMasterData.Client_Address == null ? "" : "\n" + jobMasterData.Client_Address) + (jobMasterData.Client_Address2 == null ? "" : "\n" + jobMasterData.Client_Address2)
                                + (jobMasterData.Client_City == null ? "" : "\n" + jobMasterData.Client_City)
                                 + (jobMasterData.Client_State == null ? "" : ", " + ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Client_State).Select(x => x.Value).FirstOrDefault())
                                + (jobMasterData.Client_ZipCode == null ? "" : " " + jobMasterData.Client_ZipCode),
                                //Client_Address2 = jobMasterData.Client_Address2,
                                //Client_City = jobMasterData.Client_City,
                                //Client_State = jobMasterData.Client_State == null ? "" : ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Client_State).Select(x => x.Name).FirstOrDefault().Replace(jobMasterData.Client_State, "").Replace("- ", ""),
                                //Client_ZipCode = jobMasterData.Client_ZipCode,
                                Work_Location = jobMasterData.Work_Location,
                                Work_Address = jobMasterData.Work_Location + (jobMasterData.Work_Address == null ? "" : "\n" + jobMasterData.Work_Address) + (jobMasterData.Work_Address2 == null ? "" : "\n" + jobMasterData.Work_Address2)
                                + (jobMasterData.Work_City == null ? "" : "\n" + jobMasterData.Work_City)
                                  // + (jobMasterData.Work_State == null ? "" : "\n" + ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Work_State).Select(x => x.Name).FirstOrDefault().Replace(jobMasterData.Work_State, "").Replace("- ", ""))
                                  + (jobMasterData.Work_State == null ? "" : ", " + ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Work_State).Select(x => x.Value).FirstOrDefault())
                                + (jobMasterData.Work_ZipCode == null ? "" : " " + jobMasterData.Work_ZipCode),
                                Work_Address2 = jobMasterData.Work_Address2,
                                Work_City = jobMasterData.Work_City,
                                Work_State = jobMasterData.Work_State == null ? "" : ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Work_State).Select(x => x.Name).FirstOrDefault().Replace(jobMasterData.Work_State, "").Replace("- ", ""),
                                Work_ZipCode = jobMasterData.Work_ZipCode,
                                Directions_To = jobMasterData.Directions_To,
                                Doing_What = jobMasterData.Doing_What
                            });
                            JobLabor = new List<ElectricEase.Data.DataBase.Job_Labor>();
                            foreach (var laborList in jobLaborData)
                            {
                                JobLabor.Add(new ElectricEase.Data.DataBase.Job_Labor
                                {
                                    Laborer_Name = laborList.Laborer_Name,
                                    Estimated_Hour = laborList.Estimated_Hour
                                });
                            }
                            foreach (var assemblyPartsList in JobPartsresult)
                            {
                                JobAssemblyParts = new List<ElectricEase.Data.DataBase.Job_Assembly_Parts>();
                                JobAssemblyParts.Add(new ElectricEase.Data.DataBase.Job_Assembly_Parts
                                {
                                    Part_Number = assemblyPartsList.Part_Number,
                                    Estimated_Qty = assemblyPartsList.Estimated_Qty
                                });
                            }
                            List<ElectricEase.Data.DataBase.Job_Attachments> attach = new List<ElectricEase.Data.DataBase.Job_Attachments>();
                            attach.Add(new ElectricEase.Data.DataBase.Job_Attachments());
                            List<ElectricEase.Data.DataBase.Job_Attachments> jobattach = db.Job_Attachments.Where(x => x.Job_ID == jobID).ToList();
                            foreach (var item in jobattach)
                            {
                                attach[0].Attachement_Name += item.Attachement_Name + "\n";
                            }
                            if (attach[0].Attachement_Name != null && attach[0].Attachement_Name != "")
                            {
                                attach[0].Attachement_Name = attach[0].Attachement_Name.Substring(0, attach[0].Attachement_Name.Length - 1);
                            }
                            ReportViewer1.LocalReport.EnableHyperlinks = true;
                            ReportViewer1.ShowPrintButton = true;
                            ReportViewer1.ShowToolBar = true;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/JobDescription.rdlc");
                            ReportDataSource rdc = new ReportDataSource("DataSet1", JobMaster);
                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(rdc);
                            ReportViewer1.LocalReport.Refresh();
                            ReportDataSource JobMasterrdc = new ReportDataSource("DataSet2", Jobparts);
                            ReportViewer1.LocalReport.DataSources.Add(JobMasterrdc);
                            ReportViewer1.LocalReport.Refresh();
                            ReportDataSource rdcimg = new ReportDataSource("DataSet3", parts);
                            ReportViewer1.LocalReport.DataSources.Add(rdcimg);
                            ReportViewer1.LocalReport.Refresh();
                            ReportDataSource rdcattach = new ReportDataSource("DataSet4", attach);
                            ReportViewer1.LocalReport.DataSources.Add(rdcattach);
                            //Passing parameter to subreport.
                            ReportParameter parameter = new ReportParameter("SubReportVisiblility", JobLabor != null ? "True" : "False");
                            ReportParameter materialListpara = new ReportParameter("SubReportVisiblility", Jobparts.Count > 0 ? "True" : "False");
                            ReportViewer1.LocalReport.SetParameters(parameter);
                            ReportViewer1.LocalReport.SetParameters(materialListpara);
                            ReportViewer1.CurrentPage = pageno;
                            ViewData["PageNo"] = Convert.ToInt32(ViewData["PageNo"]) + 1;
                            //ReportViewer1.LocalReport.Refresh();
                            //ReportViewer1.PageCountMode = PageCountMode.Actual;
                            //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                        }
                    }
                }
                ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubReportProcessing);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void SubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("LaborJobDetails", (object)JobLabor));
        }

        //protected void btnsendemail_Click(object sender, EventArgs e)
        //{
        //  //  ReportViewer1
        //}

        protected void ReportViewer1_PageNavigation(object sender, PageNavigationEventArgs e)
        {

        }


    </script>

</head>
<body>
    <form id="Form1" runat="server">
        <%--<iframe id="frmPrint" visible="false" name="IframeName" width="500" 
        height="200" runat="server" 
        style="display: none"></iframe>
        <asp:ImageButton ID="ImageButton1" Visible="false" runat="server" Text="Print" OnClick="btnPrint_Click" />--%>
        <%--<asp:Button runat="server" ID="btnsendemail" Text="Send As Emaill" OnClick="btnsendemail_Click" />--%>
        <asp:ScriptManager ID="ScriptManager1" AsyncPostBackTimeout="36000" runat="server" EnablePageMethods="true" EnablePartialRendering="true"></asp:ScriptManager>
        <%--  <asp:UpdatePanel runat="server">
            <ContentTemplate>--%>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Height="1000px" Font-Names="Verdana" OnPageNavigation="ReportViewer1_PageNavigation" ForeColor="Black" BackColor="Silver" Font-Size="9pt" ShowExportControls="true" ShowPrintButton="true" AsyncRendering="false" SizeToReportContent="false" Width="100%" PageCountMode="Actual">
        </rsweb:ReportViewer>
        <%--   </ContentTemplate>
        </asp:UpdatePanel>--%>
    </form>

</body>
</html>
