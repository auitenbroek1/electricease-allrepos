using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.BLL;
using ElectricEase.Models;
using ElectricEase.Helpers;
using System.IO;
using System.Web.Security;
using ElectricEase.BLL.AccountMaster;
using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.PartsMaster;
using ElectricEase.BLL.LegalMaster;
using ElectricEase.BLL.LabourMaster;
using ElectricEase.BLL.AssembliesMaster;
using ElectricEase.BLL.JobMaster;
using ElectricEase.Data.DataBase;
using ElectricEase.Data;
using Ionic.Zlib;
using Ionic.Zip;
using System.Web.Script.Serialization;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Text;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using ElectricEase.Data.PartsMaster;
using log4net;
using System.Text.RegularExpressions;

namespace ElectricEase.Web.Controllers
{
    public class JobController : Controller
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(JobController));
        List<Parts_DetailsInfoList> parts = new List<Parts_DetailsInfoList>();
        List<Labor_DetailsInfoList> LaborList = new List<Labor_DetailsInfoList>();
        List<Legal_DetailsInfoList> LegalList = new List<Legal_DetailsInfoList>();
        List<Assembly_MasterInfo> AssembliesList = new List<Assembly_MasterInfo>();
        ClientMasterBLL ClientResponse = new ClientMasterBLL();
        PartsMasterBLL PartsResponse = new PartsMasterBLL();


        [Authorize(Roles = "SiteAdmin, JobAdmin")]
        public ActionResult JobsIndex(int JobID = 0)
        {
            TempData["jobid"] = JobID;
            if (JobID > 0)
            {
                ViewBag.EditJobDetails = "EditJobDetails";
            }
            return View();
        }
        public ActionResult JobInfo(string JobID = "")
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            ServiceResult<Job_MasterInfo> response = new ServiceResult<Job_MasterInfo>();
            ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            Job_MasterInfo model = new Job_MasterInfo();
            ServiceResult<bool> CheckJobIDIsExist = new ServiceResult<bool>();
            ServiceResult<string> mycolor = new ServiceResult<string>();
            ServiceResultList<MyUserList> Userlistresponse = new ServiceResultList<MyUserList>();
            List<clients> clientList = new List<clients>();
            clients clients = new ElectricEase.Models.clients();
            clientList = JobResponse.getallJobclients(result.Client_ID);
            clients.name = "New Client";
            clientList.Add(clients);
            ViewBag.clientlist = new SelectList(clientList, "name", "name");
            List<clients> clientLocationList = new List<clients>();
            clientLocationList = JobResponse.getallJobclientsLocation(result.Client_ID);
            clients clientsLocation = new ElectricEase.Models.clients();
            clientsLocation.name = "New Location";
            clientLocationList.Add(clientsLocation);
            ViewBag.locationlist = new SelectList(clientLocationList, "name", "name");
            ViewBag.State = new SelectList(Utility.GetStates(), "Value", "Name");
            if (JobID != "" && Convert.ToInt32(JobID) != 0)
            {
                ViewBag.EditJobDetails = "EditJobDetails";
                CheckJobIDIsExist = JobResponse.JobIDIsExist(Convert.ToInt32(JobID));
                List<MyUserList> userlist = new List<MyUserList>();
                userlist = JobResponse.GetMyUserList(result.Client_ID, "jobassign");
                MyUserList usernull = new ElectricEase.Models.MyUserList();
                if (userlist == null || userlist.Count == 0)
                {
                    usernull.name = "User Not Available";
                    userlist.Add(usernull);
                }
                ViewBag.UserList = new SelectList(userlist, "name", "name");

                if (CheckJobIDIsExist.ResultCode == 1)
                {

                    response = JobResponse.GetJobDetails(Convert.ToInt32(JobID), result.Client_ID);
                    mycolor = JobResponse.GetMycolor(result.Client_ID, response.Data.Job_AssignedUser);
                    ViewBag.ASname = response.Data.Assemblies_Name;
                    model = response.Data;
                    model.UserColor = mycolor.Data;
                    model.Userlist = Userlistresponse.ListData;
                    model.Client_Id = result.Client_ID;
                }

            }
            return PartialView("_JobInfo", model);
        }

        public ActionResult UpdateJobDetails(int JobID)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            ServiceResult<Job_MasterInfo> response = new ServiceResult<Job_MasterInfo>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            string res = JobResponse.UpdateJobDetails(JobID, result.Client_ID);
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddCalenderEvent(JobCalendarinfo Model)
        {
            if (Model.FullDay == false && Model.stTime != null && Model.etTime != null)
            {
                DateTime sdateTime = DateTime.ParseExact(Model.stTime,
                                   "hh:mm tt", CultureInfo.InvariantCulture);
                TimeSpan span = sdateTime.TimeOfDay;

                Model.StartTime = span;
                DateTime edateTime = DateTime.ParseExact(Model.etTime,
                                        "hh:mm tt", CultureInfo.InvariantCulture);
                TimeSpan espan = edateTime.TimeOfDay;
                Model.EndTime = espan;
            }
            //Here condition for overnight shedule
            // string overtime = "21:00:00";
            // string midtime = "5:00:00";

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            ServiceResult<JobCalendarinfo> response = new ServiceResult<JobCalendarinfo>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            Model.Client_Id = result.Client_ID;
            if (Model.sDate != "" && Model.eDate != "")
            {
                Model.StartDate = Convert.ToDateTime(Model.sDate);
                Model.EndDate = Convert.ToDateTime(Model.eDate);
            }
            if (Model.StartDate < DateTime.Now.Date || Model.EndDate < DateTime.Now.Date)
            {
                var dav = DateTime.Now.Date;
                if (Model.StartDate < DateTime.Now.Date)
                {
                    response.Message = "StartDate is Lapsed,Please change the date";
                    return Json(response.Message);
                }
                else
                {
                    response.Message = "EndDate is Lapsed,Please change the date";
                    return Json(response.Message);
                }
            }
            else
            {
                if (Model.StartTime != null && Model.EndTime != null)
                {
                    if (Model.StartTime.Value.Hours > Model.EndTime.Value.Hours || (Model.StartDate == Model.EndDate && Model.StartTime == Model.EndTime))
                    {
                        response.Message = " Total Time hours is Lapsed,Please change the Date or Time";
                        return Json(response.Message);
                    }
                    //Here condition for overnight shedule......
                    //if (Model.StartTime > TimeSpan.Parse(overtime) || Model.EndTime > TimeSpan.Parse(overtime) || Model.StartTime < TimeSpan.Parse(midtime) || Model.EndTime < TimeSpan.Parse(midtime))
                    //{
                    //    response.Message = " In calendar you cannot schedule a job over night";
                    //    return Json(response.Message);
                    //}
                    response = JobResponse.AddJobCalenderInfo(Model);
                    return Json(response.Message);
                }

                response = JobResponse.AddJobCalenderInfo(Model);
                return Json(response.Message);

            }
        }

        public ActionResult GetJobEventList(string JobID = "")
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            ServiceResultList<JobCalendarinfo> response = new ServiceResultList<JobCalendarinfo>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            if (JobID != "" && JobID != null)
            {
                response = JobResponse.GetJobCalenderEventsList(result.Client_ID, Convert.ToInt32(JobID));
                return PartialView("_JobSheduleList", response.ListData);
            }
            else
            {
                return PartialView("_JobSheduleList", response.ListData);
            }


        }
        public ActionResult DeleteJobEvent(string EventID = "", string JobID = "")
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            if (EventID != "" && JobID != "")
            {
                response = JobResponse.DeleteJobCalenderEvent(result.Client_ID, Convert.ToInt32(EventID), Convert.ToInt32(JobID));
                return Json(response.Message, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json("Please check your Calender Event", JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult AttachmentInfo(string JobID = "")
        {
            List<Job_Attachments> model = new List<Job_Attachments>();
            if (JobID != "" && JobID != "0")
            {
                ViewBag.EditJobDetails = "EditJobDetails";
                model = new JobMasterBLL().getattchments(Convert.ToInt32(JobID));
            }
            if (model == null)
                model = new List<Job_Attachments>();
            return PartialView("_Attachments", model);
        }

        public ActionResult GetallAssembliesList(string JobID = "", string lstAsName = "")
        {
            List<ElectricEase.Models.Assembly_MasterInfoList> modal = new List<Assembly_MasterInfoList>();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.GetAssembliesList(result.Client_ID);
            modal = response.Where(x => x.IsActive == true).ToList();
            if (lstAsName != "")
            {
                string tmp1 = Server.UrlDecode(lstAsName);
                string[] tmp2 = tmp1.Replace("&quot;", "\"").Split(',');

                List<string> updated_list = new List<string>();
                foreach (string item in tmp2)
                {
                    string tmp = item;
                    tmp = Regex.Replace(tmp, "{{{comma}}}", ",");
                    tmp = Regex.Replace(tmp, "{{{hash}}}", "#");
                    updated_list.Add(tmp);
                }
                string[] partsno = updated_list.ToArray();
                // return Json(partslist);

                if (partsno.Length > 0)
                {
                    foreach (var item in partsno)
                    {
                        int isused = 0;
                        foreach (var items in modal)
                        {
                            if (items.Assemblies_Name == item)
                            {
                                items.isChekced = true;
                                break;
                            }
                        }

                    }
                }
            }
            return PartialView("_ShowAssembliesListDetails", modal);
        }

        [HttpPost]
        public ActionResult SelectedAssembliesInfo(string lstparts, string JobID = "")
        {
            string lstparts2 = Server.UrlDecode(lstparts);
            string[] lstparts3 = lstparts2.Replace("&quot;", "\"").Split(',');

            List<string> updated_list = new List<string>();
            foreach (string item in lstparts3) {
                string tmp = item;
                tmp = Regex.Replace(tmp, "{{{comma}}}", ",");
                tmp = Regex.Replace(tmp, "{{{hash}}}", "#");
                updated_list.Add(tmp);
            }
            string[] partslist = updated_list.ToArray();
            // return Json(partslist);

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();

            ServiceResult<Assembly_MasterInfo> AsResponse = new ServiceResult<Assembly_MasterInfo>();

            ServiceResult<bool> CheckJobIDIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            JobPartsInfo model = new JobPartsInfo();
            // model.partslist = new List<AssembliesParts_DetailsInfoList>();
            if (JobID != "")
            {
                int job_ID = Convert.ToInt32(JobID);
                CheckJobIDIsExist = JobResponse.JobIDIsExist(job_ID);
            }

            if (JobID == "" || CheckJobIDIsExist.ResultCode == 0)
            {
                foreach (string item in partslist)
                {
                    //string[] splitData = selectedPart.Split(',');
                    //In argument we are passing clientID and partId to db.
                    AsResponse = AssembliesResponse.GetAssembliesListDetails(item.ToString(), Convert.ToInt16(result.Client_ID));
                    if (AsResponse.Data != null)
                    {
                        AssembliesList.Add(new Assembly_MasterInfo
                        {
                            Assemblies_Name = AsResponse.Data.Assemblies_Name,
                            Assemblies_Category = AsResponse.Data.Assemblies_Category,
                            severity = AsResponse.Data.severity,
                            Est_Cost_Total = AsResponse.Data.GrandCostTotal,
                            Est_Resale_Total = AsResponse.Data.GrandResaleTotal,
                            //Estimated_Qty_Total = AsResponse.Data.Estimated_Qty_Total,
                            Multiplier = 1,
                            // Estimated_Qty_Total = 0,
                            Estimated_Qty_Total = AsResponse.Data.Estimated_Qty,
                            // Actual_Qty = Convert.ToInt32(AsResponse.Data.Estimated_Qty_Total),
                            Actual_Qty = Convert.ToInt32(AsResponse.Data.Actual_Qty),
                            Actual_Total = AsResponse.Data.Actual_Total,
                            labor_cost = AsResponse.Data.labor_cost,
                            PartCostTotal = AsResponse.Data.PartCostTotal,
                            PartResaleTotal = AsResponse.Data.PartResaleTotal,
                            labor_EstimatedHours = AsResponse.Data.labor_EstimatedHours,
                            Lobor_Resale = AsResponse.Data.Lobor_Resale,
                            Estimated_Hour = AsResponse.Data.Estimated_Hour,
                            LaborEst_CostTotal = AsResponse.Data.LaborEst_CostTotal,
                            LaborEst_ResaleTotal = AsResponse.Data.LaborEst_ResaleTotal,
                            GrandResaleTotal = AsResponse.Data.GrandResaleTotal,
                            GrandCostTotal = AsResponse.Data.GrandCostTotal,
                            PartsListData = AsResponse.Data.PartsListData,
                            assemblypartsCount = AsResponse.Data.assemblypartsCount
                            //isChekced = true
                        });
                    }

                }
            }
            else
            {
                if (lstparts == "")
                {
                    ServiceResult<Job_MasterInfo> mod = JobResponse.GetJobsAssemblies(Convert.ToInt32(JobID), result.Client_ID);
                    foreach (var item in mod.Data.Jobsassemblyparts)
                    {
                        AssembliesList.Add(new Assembly_MasterInfo
                        {
                            Assemblies_Name = item.Assemblies_Name,
                            Assemblies_Category = item.Assemblies_Category,
                            Multiplier = item.Multiplier ?? 1,
                            JobAssembly_Id = item.JobAssembly_Id,
                            severity = item.severity,
                            Est_Cost_Total = item.EstCost_Total,
                            Est_Resale_Total = item.EstResaleCost_Total,
                            Estimated_Qty_Total = item.Estimated_Qty,
                            PartCostTotal = item.Part_Cost,
                            PartResaleTotal = item.PartResaleTotal,
                            labor_EstimatedHours = item.labor_EstimatedHours,
                            GrandCostTotal = item.GrandCostTotal,
                            GrandResaleTotal = item.GrandResaleTotal,
                            GrandActualTotal = item.GrandActualTotal,
                            Actual_Qty = item.Actual_Qty,
                            Actual_Total = item.Actual_Total,
                            isChekced = true,
                            assemblypartsCount = item.assemblypartsCount

                        });

                    }
                }
                foreach (string item in partslist)
                {
                    AsResponse = AssembliesResponse.GetAssembliesListDetails(item.ToString(), Convert.ToInt16(result.Client_ID));
                    if (AsResponse.Data != null)
                    {
                        AssembliesList.Add(new Assembly_MasterInfo
                        {
                            Assemblies_Name = AsResponse.Data.Assemblies_Name,
                            Assemblies_Category = AsResponse.Data.Assemblies_Category,
                            severity = AsResponse.Data.severity,
                            Est_Cost_Total = AsResponse.Data.GrandCostTotal,
                            Est_Resale_Total = AsResponse.Data.GrandResaleTotal,
                            Estimated_Qty_Total = 0,
                            Actual_Qty = Convert.ToInt32(AsResponse.Data.Actual_Qty),
                            //Actual_Total = AsResponse.Data.GrandResaleTotal,
                            Actual_Total = AsResponse.Data.Actual_Total,
                            labor_cost = AsResponse.Data.labor_cost,
                            PartCostTotal = AsResponse.Data.PartCostTotal,
                            PartResaleTotal = AsResponse.Data.PartResaleTotal,
                            labor_EstimatedHours = AsResponse.Data.labor_EstimatedHours,
                            Lobor_Resale = AsResponse.Data.Lobor_Resale,
                            Estimated_Hour = AsResponse.Data.Estimated_Hour,
                            LaborEst_CostTotal = AsResponse.Data.LaborEst_CostTotal,
                            LaborEst_ResaleTotal = AsResponse.Data.LaborEst_ResaleTotal,
                            GrandResaleTotal = AsResponse.Data.GrandResaleTotal,
                            GrandCostTotal = AsResponse.Data.GrandCostTotal,
                            PartsListData = AsResponse.Data.PartsListData,
                            Multiplier = 1,
                            isChekced = true,
                            assemblypartsCount = AsResponse.Data.assemblypartsCount
                        });
                    }
                    //}
                }
            }

            AssembliesList = AssembliesList.OrderBy(x => x.Assemblies_Name).ToList();
            return Json(AssembliesList);
            //return PartialView("_SelectedAssembliesListDetails", AssembliesList);

        }

        public ActionResult GetMyAssembliesDataInfo(string AsName, string JobID = "", string AssemblyJobId = "")
        {
            AsName = Server.UrlDecode(AsName);
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            JobMasterBLL jobbll = new JobMasterBLL();
            ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
            ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            AssemblyPartsModel model = new AssemblyPartsModel();
            model.partslist = new List<AssembliesParts_DetailsInfoList>();
            if (AsName != "" && (JobID == "0" || JobID == null || JobID == ""))
            {
                CheckAssemblesnameIsExist = AssembliesResponse.AssemblieNameIsExist(AsName);
                if (CheckAssemblesnameIsExist.ResultCode == 1)
                {
                    response = AssembliesResponse.GetAssembliesListDetails(AsName, result.Client_ID);
                    model.assmeblymasterinfo = response.Data;
                    model.partslist = response.Data.PartsListData.ToList();
                    //ViewBag.Edit = "EditAS";
                }

            }
            else
            {
                CheckAssemblesnameIsExist = AssembliesResponse.AssemblieNameIsExist(AsName);
                if (CheckAssemblesnameIsExist.ResultCode == 1)
                {
                    model.partslist = jobbll.GetJobsAssembliesParts(Convert.ToInt32(JobID), AsName, result.Client_ID, Convert.ToInt32(AssemblyJobId));
                    model.assmeblymasterinfo = new Assembly_MasterInfo();
                    model.assmeblymasterinfo = jobbll.getjobassemblydetails(Convert.ToInt32(JobID), AsName, result.Client_ID, Convert.ToInt32(AssemblyJobId));
                }
            }

            // return Json(new { AsName, model }, JsonRequestBehavior.AllowGet);

            //To work on the add parts and delete parts
            TempData["_EditPartsModel"] = model;

            return PartialView("_AssembliesDetailsInfo", model);

        }
        [HttpPost]
        public ActionResult GetallPartsList(string lstparts = "", string JobID = "")
        {
            //List<ElectricEase.Models.Parts_DetailsInfoList> modal = new List<Parts_DetailsInfoList>();
            //PartsMasterBLL PartsResponse = new PartsMasterBLL();
            //ClientMasterBLL ClientResponse = new ClientMasterBLL();
            //JobMasterBLL JobResponse = new JobMasterBLL();
            //ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            //ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            //string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            //var result = ClientResponse.GetClientName(Loginuser);
            //response = PartsResponse.GetMyPartsList(result.Client_ID);
            //modal = response.ListData.ToList();
            //if (lstparts != "")
            //{
            //    string[] partsno = lstparts.Split(',');
            //    if (partsno.Length > 0)
            //    {
            //        var test = modal.Where(x => partsno.Contains(x.Part_Number)).ToList();
            //        if (test != null)
            //        {
            //            foreach (var item in partsno)
            //            {
            //                int isused = 0;
            //                foreach (var items in modal)
            //                {
            //                    if (items.Part_Number == item)
            //                    {
            //                        items.isChekced = true;
            //                        break;
            //                    }
            //                }

            //            }
            //        }
            //    }
            //}
            //else if (JobID != "0" || JobID != "")
            //{

            //}
            //return PartialView("_ShowAllPartsList", modal);
            return PartialView("_ShowAllPartsList");
        }

        [HttpPost]
        public ActionResult SelectedPartsInfo(string lstparts, string JobID = "", string SelectedPartsStatus = "")
        {
            string[] partslist = lstparts.Split(',');

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            ServiceResult<Job_MasterInfo> jobresponse = new ServiceResult<Job_MasterInfo>();
            ServiceResult<bool> CheckJobIDIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            JobPartsInfo model = new JobPartsInfo();

            //if (JobID != "" && JobID!="0")
            //{
            //    CheckJobIDIsExist = JobResponse.JobIDIsExist(Convert.ToInt32(JobID));
            //    if (CheckJobIDIsExist.ResultCode==1)
            //    {
            //        jobresponse = JobResponse.GetJobDetails(Convert.ToInt32(JobID), result.Client_ID);
            //        model.NewPartsListInJob = jobresponse.Data.NewPartsListInJob;

            //    }

            //}
            if (JobID == "" || JobID == "0")
            {
                if (partslist.Count() > 0)
                {
                    foreach (string item in partslist)
                    {
                        if (item != "")
                        {
                            response = PartsResponse.GetJobPartsList(Convert.ToInt16(result.Client_ID), item.ToString());
                            if (response.ListData != null && response.ListData.Count > 0)
                            {
                                parts.Add(new Parts_DetailsInfoList
                                {
                                    Part_Number = response.ListData[0].Part_Number,
                                    Description = response.ListData[0].Description,
                                    Part_Cost = response.ListData[0].Cost,
                                    Resale_Cost = response.ListData[0].Resale_Cost,
                                    Estimated_Qty = response.ListData[0].Estimated_Qty,
                                    LaborUnit = response.ListData[0].LaborUnit ?? 0,
                                    isChekced = true
                                });
                            }
                        }
                        //string[] splitData = selectedPart.Split(',');
                        //In argument we are passing clientID and partId to db.

                    }
                    model.NewPartsListInJob = parts;
                }

            }
            else
            {
                if (lstparts == "")
                {
                    if (lstparts == "" && SelectedPartsStatus == "selected")
                        model.NewPartsListInJob = new List<Parts_DetailsInfoList>();
                    else
                        model.NewPartsListInJob = JobResponse.GetJobsParts(Convert.ToInt32(JobID), result.Client_ID);

                }
                else
                {


                    foreach (string item in partslist)
                    {

                        if (item != "" && item != "partsselectall")
                        {
                            response = PartsResponse.GetJobPartsList(Convert.ToInt16(result.Client_ID), item.ToString());
                            parts.Add(new Parts_DetailsInfoList
                            {
                                Part_Number = response.ListData[0].Part_Number,
                                Description = response.ListData[0].Description,
                                Part_Cost = response.ListData[0].Cost,
                                Resale_Cost = response.ListData[0].Resale_Cost,
                                Estimated_Qty = response.ListData[0].Estimated_Qty,
                                LaborUnit = response.ListData[0].LaborUnit ?? 0,
                                isChekced = true
                            });

                        }
                        //string[] splitData = selectedPart.Split(',');
                        //In argument we are passing clientID and partId to db.

                    }
                    model.NewPartsListInJob = parts;
                }
            }
            if (model.NewPartsListInJob == null)
                model.NewPartsListInJob = new List<Parts_DetailsInfoList>();
            return Json(model);

            //return Json(model);
        }

        public ActionResult deletejobparts(int JobId, string PartId)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            string stat = JobResponse.deleteJobParts(JobId, PartId, result.Client_ID);
            return Json(stat, JsonRequestBehavior.AllowGet);
        }
        public ActionResult deletejoblabour(int JobId, string LabourName)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            string stat = JobResponse.deleteJobLabour(JobId, LabourName, result.Client_ID);
            return Json(stat, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetallLaborList(string JobID = "", string lstparts = "")
        {
            List<ElectricEase.Models.Labor_DetailsInfoList> modal = new List<Labor_DetailsInfoList>();
            LabourMasterBLL LaborResponse = new LabourMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            ServiceResultList<Labor_DetailsInfoList> response = new ServiceResultList<Labor_DetailsInfoList>();
            ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = LaborResponse.GetMyLabourList(result.Client_ID);
            modal = response.ListData.ToList();
            if (lstparts != "")
            {
                string[] partsno = lstparts.Split(',');
                if (partsno.Length > 0)
                {
                    foreach (var item in partsno)
                    {
                        int isused = 0;
                        foreach (var items in modal)
                        {
                            if (items.Laborer_Name == item)
                            {
                                items.isChekced = true;
                                break;
                            }
                        }

                    }
                }
            }
            if (JobID != "0" && JobID != "")
            {
                //List<Labor_DetailsInfoList> lstlabour = JobResponse.GetJobsLabour(Convert.ToInt32(JobID), Convert.ToInt16(result.Client_ID));
                //if (lstlabour != null && lstlabour.Count > 0)
                //{

                //    foreach (var item in lstlabour)
                //    {
                //        int isused = 0;
                //        foreach (var items in modal)
                //        {
                //            if (items.Laborer_Name == item.Laborer_Name)
                //            {
                //                items.isChekced = true;
                //                break;
                //            }
                //        }

                //    }
                //}
            }
            return PartialView("_ShowAllLaborList", modal);
        }

        [HttpPost]
        public ActionResult SelectedLaborInfo(string lstparts, string JobID = "", string SelectedLaborStatus = "")
        {
            string[] partslist = lstparts.Split(',');

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            LabourMasterBLL LaborResponse = new LabourMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResultList<Labor_DetailsInfoList> response = new ServiceResultList<Labor_DetailsInfoList>();
            ServiceResult<Assembly_MasterInfo> AsResponse = new ServiceResult<Assembly_MasterInfo>();
            Assembly_MasterInfo ModelOBJ = new Assembly_MasterInfo();
            ServiceResult<bool> CheckJobIDIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            JobLaborInfo model = new JobLaborInfo();
            if (JobID != "")
            {
                int job_ID = Convert.ToInt32(JobID);
                CheckJobIDIsExist = JobResponse.JobIDIsExist(job_ID);
            }

            if (JobID == "" || JobID == "0" || CheckJobIDIsExist.ResultCode == 0)
            {
                foreach (string item in partslist)
                {
                    response = LaborResponse.GetMyJobLaborerList(Convert.ToInt16(result.Client_ID), item.ToString());
                    if (response.ListData != null && response.ListData.Count > 0)
                    {
                        LaborList.Add(new Labor_DetailsInfoList
                        {
                            Laborer_Name = response.ListData[0].Laborer_Name,
                            Cost = response.ListData[0].Cost,
                            Resale_Cost = response.ListData[0].Resale_Cost,
                            Estimated_Hour = response.ListData[0].Estimated_Hour,
                            isChekced = true
                        });
                    }
                }
                model.NewLabourListInJob = LaborList;

            }
            else
            {
                if (lstparts == "")
                {
                    if (lstparts == "" && SelectedLaborStatus == "selectedlabor")
                        model.NewLabourListInJob = new List<Labor_DetailsInfoList>();
                    else
                        model.NewLabourListInJob = JobResponse.GetJobsLabour(Convert.ToInt32(JobID), Convert.ToInt16(result.Client_ID));
                }
                else
                {
                    foreach (string item in partslist)
                    {
                        if (item != "" && item != "laborselectall")
                        {

                            response = LaborResponse.GetMyJobLaborerList(Convert.ToInt16(result.Client_ID), item.ToString());
                            if (response != null && response.ListData != null && response.ListData.Count > 0)
                            {
                                LaborList.Add(new Labor_DetailsInfoList
                                {
                                    Laborer_Name = response.ListData[0].Laborer_Name,
                                    Cost = response.ListData[0].Cost,
                                    Resale_Cost = response.ListData[0].Resale_Cost,
                                    Estimated_Hour = response.ListData[0].Estimated_Hour,
                                    isChekced = true
                                });
                            }
                        }
                        model.NewLabourListInJob = LaborList;
                    }
                }
            }
            if (model.NewLabourListInJob == null)
            {
                model.NewLabourListInJob = new List<Labor_DetailsInfoList>();
            }
            return Json(model);
        }

        public ActionResult GetallLegalList(string JobID = "", string lstparts = "")
        {
            List<ElectricEase.Models.Legal_DetailsInfoList> modal = new List<Legal_DetailsInfoList>();
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            ServiceResultList<Legal_DetailsInfoList> response = new ServiceResultList<Legal_DetailsInfoList>();
            ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = LegalResponse.GetMyLegalList(result.Client_ID);
            modal = response.ListData.ToList();
            // CheckAssemblesnameIsExist = AssembliesResponse.AssemblieNameIsExist(name);
            //if (name == "" || CheckAssemblesnameIsExist.ResultCode == 0)
            //{
            //    ViewBag.Edit = "";

            //}
            if (lstparts != "")
            {
                string[] partsno = lstparts.Split(',');
                if (partsno.Length > 0)
                {
                    foreach (var item in partsno)
                    {
                        int isused = 0;
                        foreach (var items in modal)
                        {
                            if (item.Trim() != "" && item != null)
                            {
                                if (items.Legal_ID == Convert.ToInt32(item))
                                {
                                    items.isChekced = true;
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            return PartialView("_ShowAllLegalList", modal);
        }
        [HttpPost]
        public ActionResult SelectedLegalInfo(string lstparts, string JobID = "", string selectedLegalstatus = "")
        {
            string[] partslist = lstparts.Split(',');

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResultList<Legal_DetailsInfoList> response = new ServiceResultList<Legal_DetailsInfoList>();
            ServiceResult<Assembly_MasterInfo> AsResponse = new ServiceResult<Assembly_MasterInfo>();
            Assembly_MasterInfo ModelOBJ = new Assembly_MasterInfo();
            ServiceResult<bool> CheckJobIDIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            JobLaborInfo model = new JobLaborInfo();
            // model.partslist = new List<AssembliesParts_DetailsInfoList>();
            List<Legal_DetailsInfoList> AddlegalList = new List<Legal_DetailsInfoList>();
            if (JobID != "")
            {
                int job_ID = Convert.ToInt32(JobID);
                CheckJobIDIsExist = JobResponse.JobIDIsExist(job_ID);
            }

            if (JobID == "" || CheckJobIDIsExist.ResultCode == 0)
            {
                foreach (string item in partslist)
                {
                    if (item != "" && item != "legalselectall")
                    {
                        response = LegalResponse.GetSelectedLegalListDetails(Convert.ToInt16(result.Client_ID), Convert.ToInt32(item));
                        LegalList.Add(new Legal_DetailsInfoList
                        {
                            Legal_ID = response.ListData[0].Legal_ID,
                            Legal_Category = response.ListData[0].Legal_Category,
                            Legal_Detail = response.ListData[0].Legal_Detail,
                            isChekced = true
                        });
                    }
                }
            }
            else
            {
                if (lstparts == "")
                {
                    if (lstparts == "" && selectedLegalstatus == "selectedLegal")
                        LegalList = new List<Legal_DetailsInfoList>();
                    else
                        LegalList = JobResponse.GetJobsLegalList(Convert.ToInt32(JobID), result.Client_ID);
                }
                else
                {
                    foreach (string item in partslist)
                    {
                        if (item != "" && item != "legalselectall")
                        {

                            response = LegalResponse.GetSelectedLegalListDetails(Convert.ToInt16(result.Client_ID), Convert.ToInt32(item));
                            AddlegalList.Add(new Legal_DetailsInfoList
                            {
                                Legal_ID = response.ListData[0].Legal_ID,
                                Legal_Category = response.ListData[0].Legal_Category,
                                Legal_Detail = response.ListData[0].Legal_Detail,
                                isChekced = true
                            });
                        }
                        LegalList = AddlegalList;
                    }


                    //LegalList = JobResponse.GetJobsLegalList(Convert.ToInt32(JobID), result.Client_ID);
                    //foreach (string item in partslist)
                    //{
                    //    if (item != "" && item != "legalselectall")
                    //    {
                    //        int availablity = 0;
                    //        foreach (var items in LegalList)
                    //        {
                    //            if (items.Legal_ID.ToString() == item)
                    //            {
                    //                availablity++;
                    //                break;
                    //            }
                    //        }
                    //        if (availablity == 0)
                    //        {
                    //            response = LegalResponse.GetSelectedLegalListDetails(Convert.ToInt16(result.Client_ID), Convert.ToInt32(item));
                    //            LegalList.Add(new Legal_DetailsInfoList
                    //            {
                    //                Legal_ID = response.ListData[0].Legal_ID,
                    //                Legal_Category = response.ListData[0].Legal_Category,
                    //                Legal_Detail = response.ListData[0].Legal_Detail,
                    //                isChekced = true
                    //            });
                    //        }
                    //    }
                    //}
                }

            }
            return PartialView("_SelectedLegalListDetails", LegalList);
        }

        [Authorize(Roles = "SiteAdmin, JobAdmin")]
        public ActionResult JobList(string status = "")
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            ServiceResultList<Job_MasterInfoList> response = new ServiceResultList<Job_MasterInfoList>();
            response = JobResponse.GetJobDetailList(result.Client_ID);
            ViewBag.successmsg = response.Message;
            if (status == "Open")
            {
                ViewBag.successmsg = "";
                List<Job_MasterInfoList> model = new List<Job_MasterInfoList>();
                model = response.ListData.Where(m => m.Job_Status == status).ToList();
                return View("JobDetailsList", model);
            }
            if (status == "Closed")
            {
                ViewBag.successmsg = "";
                List<Job_MasterInfoList> model = new List<Job_MasterInfoList>();
                model = response.ListData.Where(m => m.Job_Status == status).ToList();
                return View("JobDetailsList", model);
            }
            return View("JobDetailsList", response.ListData);
        }

        [HttpPost]
        public JsonResult Upload()
        {
            int filecount = Request.Files.AllKeys.Count();
            int jobid = Convert.ToInt32(Request.Form["JobId"]);
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            List<Job_Attachments> lstjobattach = new List<Job_Attachments>();
            for (int i = 0; i < filecount; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                var result = ClientResponse.GetClientName(Loginuser);
                Job_Attachments jobattach = new Job_Attachments();
                var filetype = file.ContentType;
                jobattach.Client_Id = result.Client_ID;
                jobattach.Attachement_Name = file.FileName;
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    jobattach.Attachment = ms.ToArray();

                }
                jobattach.Job_ID = jobid;
                lstjobattach.Add(jobattach);
                //System.IO.File.Delete(filepath);
            }
            string status = JobResponse.saveattachments(lstjobattach);
            return Json(status);
        }

        public JsonResult AddNewJobDetails(Job_MasterInfo Model)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            Model.Client_Id = result.Client_ID;
            Model.Created_By = Loginuser;
            ServiceResult<int> Response = new ServiceResult<int>();
            string jobid = JobResponse.SaveJobDetails(Model);
            return Json(jobid);
        }

        [HttpPost]
        public JsonResult SaveNewJobDetails(Job_MasterInfo Model)
        {
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //string modeldata = Request.Form["Model"];
            //Job_MasterInfo Model = jss.Deserialize<Job_MasterInfo>(modeldata);

            //Job_MasterInfo Model = jss.Deserialize<Job_MasterInfo>(modeldata);
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            Model.Client_Id = result.Client_ID;
            Model.Created_By = Loginuser;
            Model.Updated_By = Loginuser;
            ServiceResult<int> Response = new ServiceResult<int>();
            string jobid = JobResponse.SaveJobDetails(Model);
            return Json(jobid);
        }

        public ActionResult Realignassemblyparts(string parts, string assem)
        {
            AssemblyPartsModel Model = new AssemblyPartsModel();
            Model.partslist = new List<AssembliesParts_DetailsInfoList>();
            Model.assmeblymasterinfo = new Assembly_MasterInfo();
            //JObject result = JObject.Parse(Numb);
            JavaScriptSerializer j = new JavaScriptSerializer();
            List<ElectricEase.Models.AssembliesParts_DetailsInfoList> partslist = j.Deserialize<List<ElectricEase.Models.AssembliesParts_DetailsInfoList>>(parts);
            ElectricEase.Models.Assembly_MasterInfo assmeblymasterinfo = j.Deserialize<ElectricEase.Models.Assembly_MasterInfo>(assem);

            //List<ElectricEase.Models.AssembliesParts_DetailsInfoList> a = j.Deserialize(Numb, typeof(List<ElectricEase.Models.AssembliesParts_DetailsInfoList>));
            Model.partslist = partslist;
            Model.assmeblymasterinfo = assmeblymasterinfo;
            // AssemblyPartsModel model = new AssemblyPartsModel();
            // model.partslist = new List<AssembliesParts_DetailsInfoList>();
            return PartialView("_AssembliesDetailsInfo", Model);
        }
        public ActionResult EditJobDetails(int JobID)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            ServiceResult<Job_MasterInfo> response = new ServiceResult<Job_MasterInfo>();
            response = JobResponse.GetJobDetails(JobID, result.Client_ID);
            ViewBag.EditJobDetails = "EditJobDetails";
            TempData["JobAssembliesParts"] = response.Data.NewJobAssemblyPartsList;
            TempData["JobAssembliesLabor"] = response.Data.NewJobAssemblyLaborList;
            TempData["MyPartsInJob"] = response.Data.NewPartsListInJob;
            TempData["MyLaborListInJob"] = response.Data.NewLabourListInJob;
            TempData["MyLegalList"] = response.Data.NewLegalListInJob;
            TempData["MyDJEDetails"] = response.Data.DJEVQDetails;
            return View("JobsIndex", response.Data);
        }


        [HttpPost]
        public JsonResult UpdateJobDetails(Job_MasterInfo Model)
        {
            return Json("");
        }
        public ActionResult DeleteJobDetails(int JobID)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = JobResponse.DeleteJobDetails(result.Client_ID, JobID, Loginuser);
            if (response.ResultCode > 0)
            {
                //TempData["DeleteSuccessMsg"] = response.Message;
                return Json("Success", JsonRequestBehavior.AllowGet);

            }
            else
            {
                //TempData["DeleteFailMsg"] = response.Message;
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult checkAssemblyIsExist(string name = "")
        {
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            CheckAssemblesnameIsExist = AssembliesResponse.AssemblieNameIsactive(name);
            if (CheckAssemblesnameIsExist.ResultCode == 1)
            {
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("false", JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult GetallPartsListAssembly(string name = "", string lstparts = "")
        {
            List<ElectricEase.Models.Parts_DetailsInfoList> modal = new List<Parts_DetailsInfoList>();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResult<Assembly_MasterInfo> partsresponse = new ServiceResult<Assembly_MasterInfo>();
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = PartsResponse.GetMyPartsList(result.Client_ID);
            modal = response.ListData.ToList();
            ViewBag.nullpartsassemblyname = name;
            if (lstparts != "")
            {
                string[] partsno = lstparts.Split(',');
                if (partsno.Length > 0)
                {
                    partsresponse = AssembliesResponse.GetAssembliesListDetails(name, result.Client_ID);
                    foreach (var item in partsno)
                    {
                        int isused = 0;
                        foreach (var items in modal)
                        {
                            items.AssemblyName = name;
                            if (items.Part_Number == item)
                            {
                                items.isChekced = true;
                                var isparts = partsresponse.Data.PartsListData.Where(x => x.Part_Number == items.Part_Number).FirstOrDefault();
                                if (isparts != null)
                                {
                                    items.isdiabled = true;
                                }
                                break;
                            }
                        }

                    }
                }
            }
            else
            {
                //To update the assembly name
                modal.ForEach(z => z.AssemblyName = name);
            }
            return PartialView("_Showallpartslistassembly", modal);

        }

        public ActionResult SelectedAssemblyPartsInfo(string lstparts = "", string JobID = "")
        {
            string[] partslist = lstparts.Split(',');
            List<ElectricEase.Models.Parts_DetailsInfoList> modal = new List<Parts_DetailsInfoList>();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            foreach (string item in partslist)
            {
                //string[] splitData = selectedPart.Split(',');
                //In argument we are passing clientID and partId to db.
                if (item != "" && item != "ASPselectall")
                {
                    response = PartsResponse.GetJobPartsList(Convert.ToInt16(result.Client_ID), item.ToString());
                    parts.Add(new Parts_DetailsInfoList
                    {
                        Part_Number = response.ListData[0].Part_Number,
                        Part_Category = response.ListData[0].Part_Category,
                        PartDescription = response.ListData[0].Description,
                        Part_Cost = response.ListData[0].Cost,
                        Resale_Cost = response.ListData[0].Resale_Cost,
                        Estimated_Qty = response.ListData[0].Estimated_Qty,
                        LaborUnit = response.ListData[0].LaborUnit ?? 0,
                        isChekced = true
                    });
                }
            }
            return Json(parts);
        }

        public ActionResult deleteAssemblyPartsInfo(string part)
        {
            AssemblyPartsModel tempModel = TempData["_EditPartsModel"] as AssemblyPartsModel;
            tempModel.partslist.RemoveAll(p => p.Part_Number == part);
            TempData["_EditPartsModel"] = tempModel;
            return PartialView("_AssembliesDetailsInfo", tempModel);
        }

        public ActionResult deleteJobAssembly(int Id, int Jobid)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            var status = JobResponse.deleteJobassembly(Jobid, Id, result.Client_ID, Loginuser);
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getjobclientdetails(string name)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            return Json(JobResponse.getjobclientdetails(name), JsonRequestBehavior.AllowGet);
        }
        public ActionResult getjobclientlocationdetails(string name)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            return Json(JobResponse.getjobclientLocationdetails(name), JsonRequestBehavior.AllowGet);
        }
        public ActionResult deletejobattachment(int AttachmentId)
        {
            JobMasterBLL JobResponse = new JobMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            string stat = JobResponse.deleteJobattachment(AttachmentId, result.Client_ID, Loginuser);
            return Json(stat, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPreviousSOWInfo()
        {
            ServiceResultList<SOWInfoList> response = new ServiceResultList<SOWInfoList>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = JobResponse.GetPreviousSOWList(result.Client_ID);

            return PartialView("_PervoiusSOWList", response.ListData);
            // return PartialView("_PervoiusSOWList");
        }
        public ActionResult GetPreviousJobdesc()
        {
            ServiceResultList<SOWInfoList> response = new ServiceResultList<SOWInfoList>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = JobResponse.GetPreviousJobDesc(result.Client_ID);

            return PartialView("_PreviousList", response.ListData);
        }
        public ActionResult GetPreviousDirection()
        {
            ServiceResultList<SOWInfoList> response = new ServiceResultList<SOWInfoList>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = JobResponse.GetPreviousDirection(result.Client_ID);

            return PartialView("_PreviousList", response.ListData);
        }
        [HttpPost]
        public ActionResult SendreportEmail()
        {
            var keys = Request.Form["report"];
            return null;
        }

        [Authorize(Roles = "SiteAdmin, JobAdmin")]
        public ActionResult Job_CalenderEvents()
        {
            return View();
        }

        public JsonResult List(string start = "", string end = "")
        {
            //var epoch = new DateTime(1970, 1, 1);
            //var startDate = epoch.AddMilliseconds(start);
            //var endDate = epoch.AddMilliseconds(end);

            ServiceResultList<CalendarEventItem> response = new ServiceResultList<CalendarEventItem>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = JobResponse.GetCalenderEvents(result.Client_ID, result.User_ID, result.CreatedBy_SuperUser);
            var eventlist = from m in response.ListData
                            select new
                            {
                                id = m.ID,
                                // title = m.Fullday == false ? m.Title + "-" + m.ID.ToString() + '\n' + m.sTime + " To " + m.eTime : m.Title + "-" + m.ID.ToString(),
                                title = m.Title + "-" + m.ID.ToString(),
                                start = m.Start.ToString("s"),
                                end = m.End.ToString("s"),
                                allday = true,
                                editable = true,
                                backgroundColor = m.backgroundcolor,
                                borderColor = m.border,
                                Fullday = m.Fullday,
                                TotalHours = m.TotalHours

                            };


            return Json(eventlist.ToArray(), JsonRequestBehavior.AllowGet);
            //var ctx = new AdventureWorksEntities();
            //var data = ctx.SalesOrderHeaders
            //.Where(i => startDate <= i.OrderDate && i.OrderDate <= endDate)
            //.GroupBy(i => i.OrderDate)
            //.Select(i => new { OrderDate = i.Key, Sales = i.FirstOrDefault() }).Take(20).ToList();

            //return Json(data.Select(i => new
            //{
            //    title = (i.Sales.Customer != null) ? i.Sales.Customer.AccountNumber : “Untitled”,
            //    start = i.OrderDate.ToShortDateString(),
            //    allDay = true
            //}));
        }
        public JsonResult GetCalendarEvents(double start = 0, double end = 0)
        {

            ServiceResultList<CalendarEventItem> response = new ServiceResultList<CalendarEventItem>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = JobResponse.GetCalenderEvents(result.Client_ID, result.User_ID, result.CreatedBy_SuperUser);

            var eventlist = from m in response.ListData
                            select new
                            {
                                //id = m.ID,
                                title = m.Fullday == false ? m.Title + "-" + m.ID.ToString() + " \n " + m.sTime + " To " + m.eTime : m.Title + "-" + m.ID.ToString(),
                                //title = m.Title + "-" + m.ID.ToString(),
                                start = m.Start.ToString("s"),
                                end = m.Fullday == false ? m.End.ToString("s") : m.End.AddDays(1).ToString("s"),
                                url = "../Reports/Index?JobID=" + m.ID,
                                // start = string.Format("{0:s}", m.Start),
                                // end = string.Format("{0:s}", m.End),
                                draggable = false,
                                allday = true,
                                // editable = true,
                                backgroundColor = m.backgroundcolor,
                                borderColor = m.border,
                                // Fullday=m.Fullday,
                                //TotalHours=m.TotalHours
                                timezone = "America/Chicago",
                                handleWindowResize = true


                            };

            return Json(eventlist.ToArray(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveSowMaster(string sow, string Category, string Subject)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            Sow_Master sowmaster = JobResponse.SaveSowMaster(sow, Category, Subject, result.Client_ID);
            if (sowmaster != null)
            {
                return Json("success");
            }
            else
            {
                return Json("Fail");
            }

        }
        [HttpPost]
        public ActionResult RemoveSow(int id)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            string sowmaster = JobResponse.RemoveSow(id, result.Client_ID);
            return Json(sowmaster);
        }
        [HttpGet]
        public ActionResult GetsowList()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            List<Sow_Master> sowmaster = JobResponse.GetSowMasterList(result.Client_ID);
            List<sowdata> sowdata = new List<sowdata>();
            foreach (var item in sowmaster)
            {
                sowdata ddata = new ElectricEase.Models.sowdata();
                ddata.ClientId = item.ClientId;
                ddata.Id = item.Id;
                if (item.Sow_desc.Length > 40)
                {
                    ddata.Sow_desc = item.Sow_desc.ToString().Substring(0, 40);
                }
                else
                {
                    ddata.Sow_desc = item.Sow_desc;
                }

                ddata.Category = item.Category;
                ddata.Subject = item.Subject;
                sowdata.Add(ddata);
            }
            return PartialView("_GetsowList", sowdata);
        }

        public ActionResult SowMasterDetails(int id = 0)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            //sowdata sow = new sowdata();
            Sow_Master sow = JobResponse.SowMasterDetails(id, result.Client_ID);
            return Json(sow.Sow_desc);
        }

        public ActionResult GetUserColor(string assigneduser)
        {
            ServiceResult<string> response = new ServiceResult<string>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = JobResponse.GetMycolor(result.Client_ID, assigneduser);
            if (response.ResultCode > 0)
            {
                return Json(response.Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Downloadattachment(int JobId)
        {
            byte[] fileBytes = null;
            string filename = "";
            ZipFile s = new ZipFile();
            using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
            {
                List<Job_Attachments> jobattachement = db.Job_Attachments.Where(x => x.Job_ID == JobId).ToList();
                using (var zip = new ZipFile())
                {
                    var outputStream = new MemoryStream();
                    foreach (var item in jobattachement)
                    {
                        fileBytes = item.Attachment;
                        filename = item.Attachement_Name;
                        zip.AddEntry(filename, fileBytes);
                        //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                    }
                    zip.Save(outputStream);
                    outputStream.Position = 0;
                    return File(outputStream, "application/zip", "AssociatedFiles_Job_" + JobId + ".zip");
                }
            }
        }
        [HttpGet]
        public ActionResult GetDjeDetails(string JobID = "")
        {
            // List<Job_DJE_VQ_Details> modal = new List<Job_DJE_VQ_Details>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            List<Job_DJE_VQ_Detailsinfo> listData = new List<Job_DJE_VQ_Detailsinfo>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            listData = JobResponse.JobDjeDeltiles(Convert.ToInt32(JobID), result.Client_ID);
            return Json(listData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DeleteDjeRow(string StatusID)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            var status = JobResponse.DeleteDjeRow(Convert.ToInt32(StatusID));
            return Json(status, JsonRequestBehavior.AllowGet);

        }

        public void ExportJobParts(int JobId, string Format)
        {
            _log.Info("Enters ExportJobParts");
            _log.Info("JobId - " + JobId);
            _log.Info("Format - " + Format);
            try
            {
                JobMasterBLL objJobMasterBLL = new JobMasterBLL();
                List<exportAssemblyAndParts> exportList = new List<exportAssemblyAndParts>();
                exportList = objJobMasterBLL.ExportPartsInJob(JobId);

                if (Format == "csv")
                {
                    WriteToCSV(exportList, JobId);
                }
                else if (Format == "xlsx")
                {
                    WriteToExcel(exportList, JobId);

                }
                else if (Format == "pdf")
                {
                    WriteToPDF(exportList, JobId);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }
            _log.Info("Exits ExportJobParts");
        }

        public byte[] GetClientImage()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            Account_MasterInfo modelobj = new Account_MasterInfo();
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            modelobj = ClientResponse.GetClientName(username);
            byte[] imageByteData = modelobj.MyClientlogo;
            return imageByteData;
        }

        public JsonResult PartsDatatable(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = GetClientsPartsDetalis(model, out filteredResultsCount, out totalResultsCount);

            var result = new List<Parts_DetailsInfoList>(res.Count);

            int i = 1;
            foreach (var m in res)
            {
                result.Add(new Parts_DetailsInfoList
                {
                    Client_ID = m.Client_ID,
                    Part_Category = m.Part_Category,
                    Part_Number = m.Part_Number,
                    Cost = m.Cost,
                    Resale_Cost = m.Resale_Cost,
                    Purchased_From = m.Purchased_From,
                    Description = m.Description,
                    UOM = m.UOM ?? "",
                    ID = i,
                    Distributor_ID = m.Distributor_ID,
                    isChekced = model.existing_data == "selectall" ? true : IsChecked(model.existing_data, m.Part_Number)
                });
                i++;
            };

            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            });
        }

        public IList<Parts_DetailsInfoList> GetClientsPartsDetalis(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = (model.order[0].dir ?? "").ToLower() == "asc";
            }
            sortBy = sortBy == "ID" ? "Created_Date" : sortBy;
            // search the dbase taking into consideration table sorting and paging
            model.extra_search = (model.extra_search != null && model.extra_search.Trim() == "") ? model.extra_search = null : model.extra_search;
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var client = ClientResponse.GetClientName(Loginuser);
            model.clientId = client.Client_ID;
            var result = PartsResponse.GetPartsDetalis(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, model.extra_search, model.clientId);
            if (result == null)
            {
                // empty collection...
                return new List<Parts_DetailsInfoList>();
            }
            return result;
        }

        public bool IsChecked(string existing_data, string Part_Number)
        {
            if (!string.IsNullOrEmpty(existing_data))
            {
                string[] partsno = existing_data.Split(',');
                if (partsno.Length > 0)
                {
                    foreach (var item in partsno)
                    {
                        if (Part_Number == item)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void WriteToCSV(List<exportAssemblyAndParts> exportList, int JobId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Category, Part Number, Part Desc., Estimated Qty, \r\n");
            foreach (exportAssemblyAndParts item in exportList)
            {
                stringBuilder.Append((item.PartCategory ?? "").Replace(",", ";") + ",");
                stringBuilder.Append((item.PartNumber ?? "").Replace(",", ";") + ",");
                stringBuilder.Append((item.PartDescription ?? "").Replace(",", ";") + ",");
                stringBuilder.Append(Convert.ToString(item.EstimatedQty).Replace(",", ";") + ",");
                stringBuilder.Append("\r\n");
            }

            string csv = stringBuilder.ToString();
            string attachment = "attachment; filename=Job-" + JobId + "-PartsList.csv";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(csv);
            Response.Flush();
            Response.End();
        }

        public void WriteToExcel(List<exportAssemblyAndParts> exportList, int JobId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Category\t Part Number\t Part Desc.\t Estimated Qty\t \r\n");
            foreach (exportAssemblyAndParts item in exportList)
            {
                stringBuilder.Append(item.PartCategory + "\t");
                stringBuilder.Append(item.PartNumber + "\t");
                stringBuilder.Append(item.PartDescription + "\t");
                stringBuilder.Append(item.EstimatedQty + "\t");
                stringBuilder.Append("\r\n");
            }

            string xls = stringBuilder.ToString();
            string attachment = "attachment; filename=Job-" + JobId + "-PartsList.xls";
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);
            Response.Charset = "";
            Response.ContentType = "application/ms-excel";
            Response.Output.Write(xls);
            Response.Flush();
            Response.End();
        }

        public void WriteToPDF(List<exportAssemblyAndParts> exportList, int JobId)
        {
            //To get the logo
            //var logo = GetClientImage();
            //var base64 = Convert.ToBase64String(logo);
            //var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<html><body>");
            stringBuilder.Append("<div style='text-align: center; margin: 10px 0 30px;'><h1>Parts List</h1><p>Job ID - "+ JobId.ToString() +"</p></div>");
            stringBuilder.Append("<table border='1' cellpadding='8' style='border-collapse: collapse;'>");
            stringBuilder.Append("<tr><th width='150' style='background-color: #f0f0f0'>");
            stringBuilder.Append("Category");
            stringBuilder.Append("</th><th style='background-color: #f0f0f0'>");
            stringBuilder.Append("Part Number");
            stringBuilder.Append("</th><th width='310' style='background-color: #f0f0f0'>");
            stringBuilder.Append("Part Desc.");
            stringBuilder.Append("</th><th style='background-color: #f0f0f0'>");
            stringBuilder.Append("Estimated Qty");
            stringBuilder.Append("</th></tr>");
            foreach (exportAssemblyAndParts item in exportList)
            {
                stringBuilder.Append("<tr><td>");
                stringBuilder.Append(item.PartCategory);
                stringBuilder.Append("</td><td>");
                stringBuilder.Append(item.PartNumber);
                stringBuilder.Append("</td><td>");
                stringBuilder.Append(item.PartDescription);
                stringBuilder.Append("</td><td>");
                stringBuilder.Append(item.EstimatedQty);
                stringBuilder.Append("</td></tr>");
            }
            stringBuilder.Append("</table>");
            stringBuilder.Append("</body></html>");

            StringReader sr = new StringReader(stringBuilder.ToString());
            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 30f, 30f, 30f, 30f);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                string attachment = "attachment; filename=Job-" + JobId + "-PartsList.pdf";
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", attachment);
                Response.Charset = "";
                Response.ContentType = "application/pdf";
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
        }

        public JsonResult GetPartsListByClientID()
        {
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var client = ClientResponse.GetClientName(Loginuser);

            var partsList = PartsResponse.GetPartsListByClientID(client.Client_ID);
            var jsonResult = Json( new { data = partsList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}

