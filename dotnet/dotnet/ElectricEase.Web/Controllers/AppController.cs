using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.BLL;
using ElectricEase.Models;
using ElectricEase.Helpers;
using System.Web.Security;
using System.IO;
using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.JobMaster;
using ElectricEase.BLL.ReportsMaster;

namespace ElectricEase.Web.Controllers
{
    public class AppController : Controller
    {
        //Main Index  View
        public ActionResult Index()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<CountDetails> reponse = new ServiceResult<CountDetails>();
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            if (username != "Admin")
            {
                var result = ClientResponse.GetClientName(username);
                if (result != null)
                {
                    reponse = ClientResponse.GetCountDetails(result.Client_ID);
                    if (reponse.Data != null)
                    {
                        ViewBag.PartsCount = reponse.Data.Parts_Count;
                        ViewBag.JobOpenCount = reponse.Data.JobOpen_Count;
                        ViewBag.JobClosedCount = reponse.Data.JobClosed_Count;
                        ViewBag.AssembliesCount = reponse.Data.Assembly_Count;
                    }
                }
                ViewBag.JobReportRights = result.Job_Description_Report;

            }
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
            // ViewBag.JobReportRights = false;
            return View();
        }

        //Here To Get client Image
        public ActionResult GetClientImage()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            Account_MasterInfo modelobj = new Account_MasterInfo();
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            modelobj = ClientResponse.GetClientName(username);
            byte[] imageByteData = modelobj.MyClientlogo;
            return File(imageByteData, "image/png");
        }
        //Here to get Profile Image
        public ActionResult GetMyImage()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            Account_MasterInfo modelobj = new Account_MasterInfo();
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            modelobj = ClientResponse.GetClientName(username);
            if (modelobj.Photo == null)
            {
                byte[] imageByteData = modelobj.MyClientlogo;
                return File(imageByteData, "image/png");
            }
            else
            {
                byte[] imageByteData = modelobj.Photo;
                return File(imageByteData, "image/png");
            }

        }
        //Here  to Get Super User Name
        public ActionResult GetSuperUserName()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            Account_MasterInfo modelobj = new Account_MasterInfo();
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            modelobj = ClientResponse.GetClientName(username);
            string Superuser = modelobj.First_Name;
            ViewData[""] = Superuser;

            return View();
        }

        //Here to get PDF Documentation 
        public ActionResult ShowPdf()
        {
            if (System.IO.File.Exists(Server.MapPath("~/HelpPdf/ElectricEaseUserGuide.pdf")))
            {
                string pathsource = Server.MapPath("~/HelpPdf/ElectricEaseUserGuide.pdf");
                FileStream fssource = new FileStream(pathsource, FileMode.Open, FileAccess.Read);
                return new FileStreamResult(fssource, "application/pdf");
            }
            else
            {
                return RedirectToAction("Index");

            }

        }

        public ActionResult ShowMasterPDF(string PdfPath = "")
        {
            if (System.IO.File.Exists(Server.MapPath(PdfPath)))
            {
                string pathsource = Server.MapPath(PdfPath);
                FileStream fssource = new FileStream(pathsource, FileMode.Open, FileAccess.Read);

                return new FileStreamResult(fssource, "application/pdf");
            }
            else
            {
                return RedirectToAction("Index");

            }

        }
        public ActionResult list()
        {
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            return View();
        }
        public JsonResult GetCalendarEvents(double start=0, double end=0)
        {

            ServiceResultList<CalendarEventItem> response = new ServiceResultList<CalendarEventItem>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = JobResponse.GetCalenderEvents(result.Client_ID,result.User_ID,result.CreatedBy_SuperUser);
            if (response.ListData != null && response.ListData.Count > 0)
            {
                var eventlist = from m in response.ListData
                                select new
                                {
                                    //id = m.ID,
                                    title = m.Fullday == false ? m.Title + "-" + m.ID.ToString() + "\n" + m.sTime + " To " + m.eTime : m.Title + "-" + m.ID.ToString(),
                                    // start = m.Start.ToString("s"),
                                    //end = m.End.ToString("s"),
                                    start = string.Format("{0:s}", m.Start),
                                    end = m.Fullday == false ?string.Format("{0:s}", m.End):string.Format("{0:s}", m.End.AddDays(1)),
                                     url = "../Reports/Index?JobID="+m.ID,
                                    allday = true,
                                    // editable = true,
                                    backgroundColor = m.backgroundcolor,
                                    borderColor = m.border,
                                    // Fullday=m.Fullday,
                                    //TotalHours=m.TotalHours
                                    timezone = "America/Chicago"

                                };
                //select new
                //{
                //    id = m.ID,
                //    title = m.Fullday == false ? m.Title + "-" + m.ID.ToString() + " " + m.sTime + " To " + m.eTime: m.Title + "-" + m.ID.ToString(),
                //    start = m.Start.ToString("s"),
                //    end = m.End.ToString("s"),
                //    allday = true,
                //    editable = true,
                //    backgroundColor = m.backgroundcolor,
                //    borderColor = m.border,
                //    Fullday = m.Fullday,
                //    TotalHours = m.TotalHours
                //};
                return Json(eventlist.ToArray(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var eventlist = "";
                return Json(eventlist.ToArray(), JsonRequestBehavior.AllowGet);
            }



        }

        public ActionResult JobDescriptionReport()
        {
            ServiceResultList<Job_MasterInfo> response = new ServiceResultList<Job_MasterInfo>();
            ReportsMasterDLL reportResponse = new ReportsMasterDLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = reportResponse.GetMyDescriptionReport(result.Client_ID);
            return PartialView("_JobDescriptionReport", response.ListData);
        }
    }
}
