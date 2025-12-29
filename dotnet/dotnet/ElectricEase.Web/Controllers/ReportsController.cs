using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.BLL.ReportsMaster;
using ElectricEase.Models;
using ElectricEase.BLL.ClientMaster;
using System.Web.Security;
using ElectricEase.Data.DataBase;
using ElectricEase.BLL.JobMaster;
using System.Drawing;
using System.IO;

namespace ElectricEase.Web.Controllers
{
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/

        [Authorize(Roles = "SiteAdmin, JobDescriptionReportAdmin")]
        public ActionResult Index(string JobID)
        {
            if (JobID != null)
            {
                TempData["JobID"] = JobID;
            }
            ViewData["MyVar"] = TempData.Peek("JobID");
            if (ViewData["PageNo"] == null || ViewData["PageNo"] == "")
                ViewData["PageNo"] = 1;
            if (Request.Cookies["currentpage"] != null && Request.Cookies["currentpage"].Value != null && Request.Cookies["currentpage"].Value != "")
            {
                ViewData["PageNo"] = Convert.ToInt32(Request.Cookies["currentpage"].Value);
            }
            
            return View();
        }

        [Authorize(Roles = "SiteAdmin, JobDescriptionReportAdmin")]
        public ActionResult JobDescriptionReport()
        {
            ServiceResultList<Job_MasterInfo> response = new ServiceResultList<Job_MasterInfo>();
            ReportsMasterDLL reportResponse = new ReportsMasterDLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = reportResponse.GetMyDescriptionReport(result.Client_ID);
            if (Request.Cookies["currentpage"] != null && Request.Cookies["currentpage"].Value != null && Request.Cookies["currentpage"].Value != "")
            {
                Response.Cookies["currentpage"].Value = "1";
            }
            else
            {
                HttpCookie _cookie = new HttpCookie("currentpage");
                _cookie.Value = "1";
                Response.Cookies.Add(_cookie);
            }
            return View("JobDescriptionReport", response.ListData);
        }

        [Authorize(Roles = "SiteAdmin, ClientEstimationReportAdmin")]
        public ActionResult ClientEstimationReport()
        {
            ServiceResultList<Job_MasterInfo> response = new ServiceResultList<Job_MasterInfo>();
            ReportsMasterDLL reportResponse = new ReportsMasterDLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = reportResponse.GetMyDescriptionReport(result.Client_ID);
            return View("ClientEstimationReport", response.ListData);
        }

        [Authorize(Roles = "SiteAdmin, ClientEstimationReportAdmin")]
        public ActionResult RSIndex(string JobID)
        {
            if (JobID != null)
            {
                TempData["JobID"] = JobID;
            }
            ViewData["MyVar"] = TempData.Peek("JobID");
            return View();
        }

        public ActionResult GetAssociatedfiles(string JobID = "")
        {
            ReportsMasterDLL reportResponse = new ReportsMasterDLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            List<Job_Attachments> model = new List<Job_Attachments>();
            if (JobID != "" && JobID != "0")
            {
                ViewBag.EditJobDetails = "EditJobDetails";
                model = new JobMasterBLL().getattchments(Convert.ToInt32(JobID));
            }
            if (model == null)
                model = new List<Job_Attachments>();
            return PartialView("_ShowAssociatedFiles", model);
        }
        public ActionResult ViewFile(string JobID = "", string AttachmentName = "")
        {
            List<Job_Attachments> model = new List<Job_Attachments>();
            string[] sr = AttachmentName.Split('.');
            Viewfile Filemodel = new Viewfile();
            //HttpPostedFileBase fii;
            if (JobID != "" && JobID != "0")
            {
                ViewBag.EditJobDetails = "EditJobDetails";
                model = new JobMasterBLL().getattchments(Convert.ToInt32(JobID));
                byte[] fileArray = (from m in model where m.Job_ID == Convert.ToInt32(JobID) && m.Attachement_Name == AttachmentName select m.Attachment).FirstOrDefault();


                return File(fileArray, "application/force-download", "Attachfile_"+ JobID + AttachmentName);

                

            }
            else
            {
                return PartialView("_Viewfile", Filemodel);
            }
        }

        public ActionResult Getpdf(string JobID = "", string AttachmentName = "")
        {

            List<Job_Attachments> model = new List<Job_Attachments>();
            string[] sr = AttachmentName.Split('.');
            Viewfile Filemodel = new Viewfile();
            if (JobID != "" && JobID != "0")
            {
                ViewBag.EditJobDetails = "EditJobDetails";
                model = new JobMasterBLL().getattchments(Convert.ToInt32(JobID));
                byte[] fileArray = (from m in model where m.Job_ID == Convert.ToInt32(JobID) && m.Attachement_Name == AttachmentName select m.Attachment).FirstOrDefault();
                byte[] data = fileArray;

                return File(data, "application/pdf");
            }
            else
            {
                return File("", "application/pdf");
            }

        }
        
    }
}

