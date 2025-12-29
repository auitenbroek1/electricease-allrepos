using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ElectricEase.Models;
using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.ReportsMaster;
using ElectricEase.BLL.JobMaster;
using ElectricEase.Data.DataBase;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web;
using System.Reflection;
using System.Web.Http.Cors;

namespace ElectricEase.Web.Controllers
{
  
    [AllowAnonymous]
    public class MobileAppController : ApiController
    {
        private ClientMasterBLL ClientResponse = new ClientMasterBLL();
        [HttpGet]
        public LoginResult VerifyUser(string uname, string pwd)
        {
            LoginInfo model = new LoginInfo();
            model.UserName = uname;
            model.Password = pwd;
            LoginResult _result = new LoginResult();
            if (uname.Trim() != "" && pwd.Trim() != "")
            {
                bool uservalid = ClientResponse.CheckValidUser(model);
                if (uservalid)
                {
                    _result = new LoginResult();
                    _result.LoginStatus = true;
                    Account_MasterInfo modelobj = new Account_MasterInfo();
                    modelobj = ClientResponse.GetClientName(uname);
                    if (modelobj != null)
                    {
                        _result.ClientID = modelobj.Client_ID;
                        _result.ClientCompany = modelobj.Client_Company;
                        _result.ClientLogo = modelobj.MyClientlogo;
                        _result.Photo = modelobj.Photo;
                    }
                }
                else
                {
                    _result = new LoginResult();
                    _result.LoginStatus = false;
                }
            }
            else
            {
                _result = new LoginResult();
                _result.LoginStatus = false;
            }
            return _result;
        }
        [HttpGet]
        public ServiceResultList<Job_MasterInfo> GetJobReport(int clientId = 0)
        {
            ServiceResultList<Job_MasterInfo> response = new ServiceResultList<Job_MasterInfo>();
            ReportsMasterDLL reportResponse = new ReportsMasterDLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            response = reportResponse.GetMyDescriptionReport(clientId);
            response.ListData = response.ListData.OrderByDescending(x => x.Job_ID).ToList();
            return response;
        }
        [HttpGet]
        public ServiceResultList<CalendarEventItem> GetCalendarEvents(string userename)
        {
            ServiceResultList<CalendarEventItem> response = new ServiceResultList<CalendarEventItem>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            var result = ClientResponse.GetClientName(userename);
            response = JobResponse.GetCalenderEvents(result.Client_ID, result.User_ID, result.CreatedBy_SuperUser);
            foreach (var item in response.ListData.Where(x=>x.Fullday == true).ToList())
            {
                item.End = item.End.AddDays(1).AddMinutes(-1);
            }
            return response;
        }
        [HttpGet]
        public string Jobpdf(int Job_id, int ClientId, bool ispdf)
        {
            string url = "";
            
            try
            {
                ServiceResult<GetjobReportPdf> model = new ServiceResult<GetjobReportPdf>();
                
                using (ElectricEase.Data.DataBase.ElectricEaseEntitiesContext db = new ElectricEase.Data.DataBase.ElectricEaseEntitiesContext())
                {
                    try
                    {
                        Job_Master JobTblOBJ = new Job_Master();
                        try
                        {
                            JobTblOBJ = (from m in db.Job_Master where (m.Client_Id == ClientId && m.Job_ID == Job_id && m.Isactive == true) select m).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            return ex.Message + " Zero try";
                        }
                            model.Data = new GetjobReportPdf()
                            {
                                Job_id = JobTblOBJ.Job_ID,
                                Job_Number = JobTblOBJ.Job_Number,
                                ClientAddress = JobTblOBJ.Client_Address + ", <br/>" + JobTblOBJ.Client_City + JobTblOBJ.Client_State + "," + JobTblOBJ.Client_ZipCode,
                                WOrkAddress = JobTblOBJ.Work_Address + "<br/> " + JobTblOBJ.Work_City + " " + JobTblOBJ.Work_Location + "<br/>" + JobTblOBJ.Work_State + "," + JobTblOBJ.Work_ZipCode,
                                Email = JobTblOBJ.Client_Email,
                                Dirctionto = JobTblOBJ.Directions_To,
                                JobDiscription = JobTblOBJ.Job_Description,
                                scope_of_work = JobTblOBJ.Doing_What
                            };
                        
                            model.Data.jobPartsData = new List<Parts_DetailsInfoList>();
                            List<ElectricEase.Models.Parts_DetailsInfo> JobPartsresult = new List<ElectricEase.Models.Parts_DetailsInfo>();
                            JobPartsresult = db.Database.SqlQuery<ElectricEase.Models.Parts_DetailsInfo>("exec EE_GetAssembliesJobParts @JobID", new System.Data.SqlClient.SqlParameter("JobID", Job_id)).ToList();
                            JobPartsresult = JobPartsresult.OrderBy(x => x.Part_Number).ToList();
                            model.Data.jobPartsData = (from j in JobPartsresult
                                                       group j by j.Part_Number into grp
                                                       select new ElectricEase.Models.Parts_DetailsInfoList
                                                        {
                                                            Part_Number = grp.Key,
                                                            Estimated_Qty = grp.Sum(x => x.Estimated_Qty),
                                                            Description = db.Parts_Details.Where(x => x.Part_Number == grp.Key && x.Client_Id == ClientId).Select(x => x.Description).FirstOrDefault(),
                                                            Part_Category = db.Parts_Details.Where(x => x.Part_Number == grp.Key && x.Client_Id == ClientId).Select(x => x.Part_Category).FirstOrDefault(),
                                                        }).OrderBy(x => x.Part_Category).ToList();

                            if (model.Data.jobPartsData == null)
                                model.Data.jobPartsData = new List<Parts_DetailsInfoList>();
                            model.Data.jobLaborData = (from m in db.Job_Labor
                                                       where m.Client_Id == ClientId && m.Job_ID == Job_id
                                                       select new Labor_DetailsInfoList
                                                       {
                                                           Laborer_Name = m.Laborer_Name,
                                                           Estimated_Hour = m.Estimated_Hour,
                                                           Resale_Cost = m.Lobor_Resale

                                                       }).ToList();

                            model.Data.LegalData = (from l in db.Job_Legal
                                                    where l.Client_ID == ClientId && l.Job_ID == Job_id
                                                    select new Legal_DetailsInfoList
                                                    {
                                                        Job_Legal_ID = l.Job_Legal_ID,
                                                        Legal_Detail = l.Legal_Detail
                                                    }).ToList();

                            List<Job_Attachments> attach = db.Job_Attachments.Where(x => x.Client_Id == ClientId && x.Job_ID == Job_id).ToList();
                            url = "inside func";
                       
                            string reportdat = "";

                            Client_Master clientdetails = db.Client_Master.Where(x => x.Client_ID == ClientId).FirstOrDefault();
                        
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(clientdetails.Client_Logo);
                        image.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                        //image.WidthPercentage = 50;
                        image.ScalePercent(25f);
                        PdfPTable imgtable = new PdfPTable(new float[]{1,3});
                        imgtable.WidthPercentage = 100;
                        imgtable.DefaultCell.Border = Rectangle.NO_BORDER;
                        //imgtable.setWidths(new float[] { 1, 3 });
                        imgtable.AddCell(new Phrase("Job Number : "+model.Data.Job_Number));
                        PdfPCell cel = new PdfPCell(image,false);
                        cel.Border = Rectangle.NO_BORDER;
                        imgtable.AddCell(cel);
                        reportdat = "<html><body>";
                        if (model.Data.scope_of_work != null && model.Data.scope_of_work != "")
                            model.Data.scope_of_work = model.Data.scope_of_work.Replace("\n", "<br/>");
                        reportdat += "<table><tbody><tr><td>" + clientdetails.Client_Company + "<br/>" + clientdetails.Address + "</td><td>&nbsp;</td><td><b>Job Number: <br/>Job Id:</b> </td><td>"+model.Data.Job_Number+"<br/>" + model.Data.Job_id + "</td></tr></tbody></table>";
                        //reportdat += "<table><tbody><tr><td>"+clientdetails.Client_Company+"<br/>"+clientdetails.Address+"</td></tr></tbody></table>";
                        reportdat += "<table><tbody><tr><td><b>Client Address:</b> <br/>"+ model.Data.ClientAddress +"&nbsp;</td><td><b>Work Address:</b> <br/>"+ model.Data.WOrkAddress +"&nbsp;</td></tr></tbody></table>";
                        reportdat += "<table><tbody><tr><td><b>Email ID:</b> " + model.Data.Email+ "</td></tr></tbody></table>";
                        reportdat += "<table><tbody><tr><td><b>Direction To Site:</b> " + model.Data.Dirctionto + "</td></tr></tbody></table>";
                        reportdat += "<table><tbody><tr><td><b>Job Description:</b> <br/>" + model.Data.scope_of_work + "</td></tr></tbody></table>";
                       
                        reportdat += "<h4><b>Materials List:</b></h4><br/>";
                        
                        PdfPTable table = new PdfPTable(4);
                        table.WidthPercentage = 100;
                        PdfPCell cell2 = new PdfPCell(new Phrase("Category", new Font(Font.FontFamily.TIMES_ROMAN,13, Font.BOLD)));
                        table.AddCell(cell2);
                        cell2 = new PdfPCell(new Phrase("Part Number", new Font(Font.FontFamily.TIMES_ROMAN,13, Font.BOLD)));
                        table.AddCell(cell2);
                        cell2 = new PdfPCell(new Phrase("Part Description", new Font(Font.FontFamily.TIMES_ROMAN,13, Font.BOLD)));
                        table.AddCell(cell2);
                        cell2 = new PdfPCell(new Phrase("Estimated Qty", new Font(Font.FontFamily.TIMES_ROMAN,13, Font.BOLD)));
                        table.AddCell(cell2);
                        foreach (var item in model.Data.jobPartsData)
                        {
                            table.AddCell(item.Part_Category);
                            table.AddCell(item.Part_Number);
                            table.AddCell(item.Description);
                            table.AddCell(item.Estimated_Qty.ToString());
                        }
                        Chunk laborhead = new Chunk("Labor List: ", new Font(Font.FontFamily.TIMES_ROMAN, 13, Font.BOLD));
                        PdfPTable table2 = new PdfPTable(2);
                        table2.WidthPercentage = 100;

                        if (model.Data.jobLaborData != null && model.Data.jobLaborData.Count > 0)
                        {
                            PdfPCell cell3 = new PdfPCell(new Phrase("Name", new Font(Font.FontFamily.TIMES_ROMAN, 13, Font.BOLD)));
                            table2.AddCell(cell3);
                            cell3 = new PdfPCell(new Phrase("Estimated Hours", new Font(Font.FontFamily.TIMES_ROMAN, 13, Font.BOLD)));
                            table2.AddCell(cell3);
                            
                            foreach (var item in model.Data.jobLaborData)
                            {
                               
                                table2.AddCell(item.Laborer_Name);
                                table2.AddCell(item.Estimated_Hour.ToString());
                            }
                        }
                        string legal = "";
                        if (attach != null && attach.Count > 0)
                        {
                            legal += "<h4><b>Associated files:</b></h4></br>";
                            foreach (var item in attach)
                            {
                                legal += "<h5>&nbsp; <b>-</b> " + item.Attachement_Name + "</h5></br>";
                            }
                        }
                        reportdat += "</body></html>";
                        StringBuilder strb = new StringBuilder();
                        strb.Append(reportdat);
                        StringReader sr = new StringReader(strb.ToString());
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                            pdfDoc.Open();
                            pdfDoc.Add(image);
                            htmlparser.Parse(sr);
                            table.DefaultCell.Border = Rectangle.TOP_BORDER;
                            pdfDoc.Add(table);
                            if (model.Data.jobLaborData != null && model.Data.jobLaborData.Count > 0)
                            {
                                pdfDoc.Add(new Phrase(laborhead));
                                pdfDoc.Add(table2);
                            }
                            if (attach != null && attach.Count > 0)
                            {
                                strb = new StringBuilder();
                                strb.Append(legal);
                                sr = new StringReader(strb.ToString());
                                htmlparser.Parse(sr);
                            }
                            pdfDoc.Close();
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Close();
                            string appPath = HttpContext.Current.Request.ApplicationPath;
                            try
                            {

                                string rootFolderPath = System.Web.HttpContext.Current.Server.MapPath("~/HelpPdf/");
                                string filesToDelete = @"*" + ClientId + "-" + Job_id.ToString() + "*.pdf";   // Only delete DOC files containing "DeleteMe" in their filenames
                                string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                                foreach (string file in fileList)
                                {
                                    //System.Diagnostics.Debug.WriteLine(file + "will be deleted");
                                    System.IO.File.Delete(file);
                                }
                                string timestamp = DateTime.Now.ToString("ddMM_HHmm");
                                string path = System.Web.HttpContext.Current.Server.MapPath("~/HelpPdf/");
                                path += ClientId + "-" + Job_id.ToString()+"_"+timestamp +".pdf";
                                System.IO.File.WriteAllBytes(path, bytes);
                                model.Data.PdfURL = "http://" + Request.RequestUri.Authority + "/HelpPdf/" + ClientId + "-" + Job_id.ToString() + "_" + timestamp + ".pdf";
                                url = "http://" + Request.RequestUri.Authority + "/HelpPdf/" + ClientId + "-" + Job_id.ToString() + "_" + timestamp + ".pdf";
                            }
                            catch (Exception ex)
                            {
                                url = ex.Message+" first try";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        url = ex.Message + " second try";
                    }
                }
            }
            catch (Exception ex)
            {
                url = ex.Message + " Third try";
            }
            return url;
        }
    }
}
