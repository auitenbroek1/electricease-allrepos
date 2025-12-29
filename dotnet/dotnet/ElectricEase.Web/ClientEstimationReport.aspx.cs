using ElectricEase.Helpers;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ElectricEase.Web
{
    public partial class ClientEstimationReport : System.Web.UI.Page
    {
        List<ElectricEase.Data.DataBase.Account_Master> parts = null;
        List<ElectricEase.Data.DataBase.Job_Master> JobMaster = null;
        List<ElectricEase.Data.DataBase.Job_Legal> JobLegal = new List<ElectricEase.Data.DataBase.Job_Legal>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int jobID = Convert.ToInt32(Request.QueryString["Jobid"]);
                ElectricEase.BLL.ClientMaster.ClientMasterBLL ClientResponse = new ElectricEase.BLL.ClientMaster.ClientMasterBLL();
                ElectricEase.Models.ServiceResultList<ElectricEase.Models.Job_MasterInfo> response = new ElectricEase.Models.ServiceResultList<ElectricEase.Models.Job_MasterInfo>();
                string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                var result = ClientResponse.GetClientName(Loginuser);
                using (ElectricEase.Data.DataBase.ElectricEaseEntitiesContext db = new ElectricEase.Data.DataBase.ElectricEaseEntitiesContext())
                {
                    var data = (from a in db.Account_Master where a.Client_ID == result.Client_ID select a).FirstOrDefault();
                    var client = (from a in db.Client_Master where a.Client_ID == result.Client_ID select a).ToList();
                    if (client.Count > 0)
                    {
                        if (client[0].State != "" && client[0].State != null)
                        {
                            var sttate = ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == client[0].State.ToUpper()).Select(x => x.Name).FirstOrDefault();
                            if (sttate != null && sttate != "")
                            {
                               // client[0].State = client[0].State == null ? "" : ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == client[0].State.ToUpper()).Select(x => x.Name).FirstOrDefault().Replace(client[0].State, "").Replace("- ", "");
                                client[0].State = client[0].State == null ? "" : ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == client[0].State.ToUpper()).Select(x => x.Value).FirstOrDefault();
                            }
                        }
                    }
                    

                    var jobMasterData = (from a in db.Job_Master where a.Client_Id == result.Client_ID && a.Job_ID == jobID select a).FirstOrDefault();
                    var jobestimation = db.Job_EstimationDetails.Where(x => x.Client_ID == result.Client_ID && x.Job_ID == jobID).FirstOrDefault();
                    decimal? salestax = 0;
                    if (jobestimation.Selected_Estimation_Type == 1)
                    {
                        salestax = jobestimation.Estimation1_Total - jobestimation.Estimation1_GrandTotal;
                    }
                    if (jobestimation.Selected_Estimation_Type == 2)
                    {
                        salestax = jobestimation.Estimation2_Total - jobestimation.Estimation2_SubTotal ;
                    }
                    if (jobestimation.Selected_Estimation_Type == 3)
                    {
                        salestax = null;
                    }
                    if (jobestimation.Selected_Estimation_Type == 4)
                    {
                        salestax = jobestimation.Estimate4_Tax;
                    }
                    var jobLegalData = (from a in db.Job_Legal where a.Client_ID == result.Client_ID && a.Job_ID == jobID select a).ToList();
                    JobMaster = new List<ElectricEase.Data.DataBase.Job_Master>();
                    if (data != null)
                    {
                        parts = new List<ElectricEase.Data.DataBase.Account_Master>();
                        parts.Add(new ElectricEase.Data.DataBase.Account_Master { Client_ID = data.Client_ID, Photo = data.Photo });

                        JobMaster.Add(new ElectricEase.Data.DataBase.Job_Master
                        {
                            Job_ID = jobMasterData.Job_ID,
                            Job_Number=jobMasterData.Job_Number,
                            Client_Address = jobMasterData.Client_Name + (jobMasterData.Client_Address == null ? "" : "\n" + jobMasterData.Client_Address) + (jobMasterData.Client_Address2 == null ? "" : "\n" + jobMasterData.Client_Address2)
                                + (jobMasterData.Client_City == null ? "" : "\n" + jobMasterData.Client_City)
                              //  + (jobMasterData.Client_State == null ? "" : "\n"+ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Client_State).Select(x => x.Name).FirstOrDefault().Replace(jobMasterData.Client_State, "").Replace("- ", ""))
                                //+ (jobMasterData.Client_State == null ? "" : "\n" + ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Client_State).Select(x => x.Name).FirstOrDefault().Replace(jobMasterData.Client_State, "").Replace("- ", ""))
                                 + (jobMasterData.Client_State == null ? "" : ", " + ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Client_State).Select(x => x.Value).FirstOrDefault())
                                + (jobMasterData.Client_ZipCode == null ? "" : " " + jobMasterData.Client_ZipCode),
                            Client_Address2=jobMasterData.Client_Address2,
                            Client_City=jobMasterData.Client_City,
                           Client_State = jobMasterData.Client_State==null?"":ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Client_State).Select(x => x.Name).FirstOrDefault().Replace(jobMasterData.Client_State, "").Replace("- ", ""),
                            //Client_State = jobMasterData.Client_State == null ? "" : ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Client_State).Select(x => x.Value).FirstOrDefault(),
                            Client_ZipCode =jobMasterData.Client_ZipCode,
                            Client_Email = jobMasterData.Client_Email,
                            Client_Name=jobMasterData.Client_Name,
                            Work_Location=jobMasterData.Work_Location,
                            Work_Address = jobMasterData.Work_Location + (jobMasterData.Work_Address == null ? "" : "\n" + jobMasterData.Work_Address) + (jobMasterData.Work_Address2 == null ? "" : "\n" + jobMasterData.Work_Address2)
                                + (jobMasterData.Work_City == null ? "" : "\n" + jobMasterData.Work_City)
                               // + (jobMasterData.Work_State == null ? "" : "\n" + ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Work_State).Select(x => x.Name).FirstOrDefault().Replace(jobMasterData.Work_State, "").Replace("- ", ""))
                                 + (jobMasterData.Work_State == null ? "" : ", " + ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Work_State).Select(x => x.Value).FirstOrDefault())
                                + (jobMasterData.Work_ZipCode == null ? "" : " " + jobMasterData.Work_ZipCode),
                            Work_Address2=jobMasterData.Work_Address2,
                            Work_City=jobMasterData.Work_City,
                            Work_State = jobMasterData.Work_State == null ? "" : ElectricEase.Helpers.Utility.GetStates().Where(x => x.Value == jobMasterData.Work_State).Select(x => x.Name).FirstOrDefault().Replace(jobMasterData.Work_State, "").Replace("- ", ""),
                            Work_ZipCode=jobMasterData.Work_ZipCode,
                            Job_Description = jobMasterData.Job_Description,
                            Job_EstimatedTotal = jobMasterData.Job_EstimatedTotal,
                            EstimateValue_SubTotal = jobMasterData.EstimateValue_SubTotal,
                            SalesTax = salestax,
                            Doing_What = jobMasterData.Doing_What,
                            EstimateValue_Profit_EstTotal = jobMasterData.Job_EstimatedTotal + Convert.ToDecimal(salestax)
                        });
                        string moneyValue = String.Format("{0:C}", JobMaster[0].EstimateValue_Profit_EstTotal);
                        if (jobLegalData != null)
                        {
                            foreach (var item in jobLegalData)
                            {
                                JobLegal.Add(new ElectricEase.Data.DataBase.Job_Legal { Legal_Detail = item.Legal_Detail.TrimStart() });
                            }
                        }
                        Subject.Value = client[0].Client_Company + " - " + "Estimation Report for Job Id " + jobID;
                        Clientname.Value = client[0].Client_Company;
                        desc.Value = JobMaster[0].Job_Description;
                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/ClientEstimation.rdlc");
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportDataSource rdc = new ReportDataSource("DataSet1", parts);
                        ReportViewer1.LocalReport.DataSources.Add(rdc);
                        ReportDataSource rdcclient = new ReportDataSource("DataSet4", client);
                        ReportViewer1.LocalReport.DataSources.Add(rdcclient);
                        ReportDataSource jobMasterrdc = new ReportDataSource("DataSet2", JobMaster);
                        ReportViewer1.LocalReport.DataSources.Add(jobMasterrdc);
                        ReportDataSource jobLegalrdc = new ReportDataSource("DataSet3", JobLegal);
                        ReportViewer1.LocalReport.DataSources.Add(jobLegalrdc);
                        ReportViewer1.LocalReport.Refresh();
                        hdnclientid.Value = JobMaster.Select(x => x.Client_Email).FirstOrDefault();
                        jobClientname.Value = JobMaster[0].Client_Name;
                    }
                }
            }
        }

        protected void click_Click(object sender, EventArgs e)
        {
            byte[] bytes = ReportViewer1.LocalReport.Render("PDF");
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            Attachment att = new Attachment(new MemoryStream(bytes), Clientname.Value + "_" + Request.QueryString["Jobid"] + "_EstimationReport.pdf");
            message.From = new System.Net.Mail.MailAddress(Settings.SenderEmailAddress);
            message.Attachments.Add(att);
            message.Subject = Subject.Value;
            message.Body = "Hi"+" "+ jobClientname.Value + ",<br/><br/>" + desc.Value + "<br/>Please check the attached report <br/><br/><br/>Thanks & Regards,<br/>" + Clientname.Value;
            message.IsBodyHtml = true;
            if (MailID.Value.ToString().Contains(','))
            {
                string[] mailids = MailID.Value.ToString().Split(',');
                foreach (string item in mailids)
                {
                    if (item.Trim() != "")
                    {
                        message.To.Add(new System.Net.Mail.MailAddress(item));
                    }
                }
            }
            else
                message.To.Add(new System.Net.Mail.MailAddress(MailID.Value));
            if (CCmailID.Value != "")
            {
                if (CCmailID.Value.ToString().Contains(','))
                {
                    string[] ccmailids = CCmailID.Value.ToString().Split(',');
                    foreach (string item in ccmailids)
                    {
                        if (item.Trim() != "")
                        {
                            message.CC.Add(new System.Net.Mail.MailAddress(item));
                        }
                    }
                }
                else
                    message.CC.Add(new System.Net.Mail.MailAddress(CCmailID.Value));
            }
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(Settings.SenderEmailAddress, Settings.SenderEmailPassword);
            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtp.Send(message);
        }
    }
}