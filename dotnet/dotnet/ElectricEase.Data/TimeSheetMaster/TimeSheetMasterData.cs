using ElectricEase.Data.ClientMaster;
using ElectricEase.Data.DataBase;
using ElectricEase.Data.ReportsMaster;
using ElectricEase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElectricEase.Data.TimeSheetMaster
{
    public class TimeSheetMasterData
    {
        public ServiceResult<TimeSheetDTO> GetTimeSheetMasterList(TimeSheetDTO modal, int from)
        {
            ServiceResult<TimeSheetDTO> response = new ServiceResult<TimeSheetDTO>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    response.Data = modal;
                    response.Data.TimeSheetMasterDetails = new List<TimeSheetMastr>();
                    response.Data.TimeSheetMaster = new TimeSheetMastr();
                    ReportsMasterData reportResponse = new ReportsMasterData();
                    ClientMasterData ClientResponse = new ClientMasterData();
                    response.Data.UserDetails = ClientResponse.GetClientName(response.Data.UserDetails.User_ID);
                    response.Data.From = from;
                    if (from == 0)
                    {
                        response.Data.TimeSheetMasterDetails = db.TimeSheet_Master.Where(a => a.Client_ID == response.Data.UserDetails.Client_ID && a.User_ID == response.Data.UserDetails.User_ID).Select(a => new TimeSheetMastr
                        {
                            ClientName = a.Client_Master.Client_Company,
                            Client_ID = a.Client_ID,
                            CreatedOn = a.CreatedOn,
                            EndDate = a.EndDate,
                            OTHours = a.OTHours,
                            StartDate = a.StartDate,
                            Status = a.Status,
                            TimeSheetCode = a.TimeSheetCode,
                            TimeSheetID = a.TimeSheetID,
                            TotalHours = a.TotalHours,
                            UpdatedOn = a.UpdatedOn,
                            User_ID = a.User_ID,
                            Comments = a.Comments,
                            ReportingManager = db.Account_Master.Where(b => b.Client_ID == a.Client_ID && b.Site_Administrator == true).Select(b => b.First_Name + " " + (b.Last_Name ?? "")).FirstOrDefault()
                        }).ToList();
                    }
                    else if (from == 1)
                    {
                        response.Data.TimeSheetMasterDetails = db.TimeSheet_Master.Where(a => a.Client_ID == response.Data.UserDetails.Client_ID).Select(a => new TimeSheetMastr
                        {
                            ClientName = a.Client_Master.Client_Company,
                            Client_ID = a.Client_ID,
                            CreatedOn = a.CreatedOn,
                            EndDate = a.EndDate,
                            OTHours = a.OTHours,
                            StartDate = a.StartDate,
                            Status = a.Status,
                            TimeSheetCode = a.TimeSheetCode,
                            TimeSheetID = a.TimeSheetID,
                            TotalHours = a.TotalHours,
                            UpdatedOn = a.UpdatedOn,
                            User_ID = a.User_ID,
                            Comments = a.Comments,
                            ReportingManager = db.Account_Master.Where(b => b.Client_ID == a.Client_ID && b.Site_Administrator == true).Select(b => b.First_Name + " " + (b.Last_Name ?? "")).FirstOrDefault()
                        }).ToList();
                    }

                    if (response.Data.TimeSheetMasterDetails.Any())
                    {
                        if (!string.IsNullOrEmpty(response.Data.StartDate))
                        {
                            int[] stDate = modal.StartDate.Split('-').Select(x => int.Parse(x)).ToArray();
                            int[] edDate = modal.EndDate.Split('-').Select(x => int.Parse(x)).ToArray();
                            response.Data.TimeSheetMaster.StartDate = new DateTime(stDate[2], stDate[0], stDate[1]);
                            response.Data.TimeSheetMaster.EndDate = new DateTime(edDate[2], edDate[0], edDate[1]);
                            response.Data.TimeSheetMasterDetails = response.Data.TimeSheetMasterDetails.Where(a => a.StartDate.Value.Date >= modal.TimeSheetMaster.StartDate.Value.Date && a.StartDate.Value.Date <= modal.TimeSheetMaster.EndDate.Value.Date).ToList();
                        }
                        response.ResultCode = 1;
                        response.Message = "Success";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "No “Records” found!";
                        return response;

                    }
                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResult<TimeSheetDTO> DeleteTimeSheet(TimeSheetDTO modal)
        {
            ServiceResult<TimeSheetDTO> response = new ServiceResult<TimeSheetDTO>();
            using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
            {
                response.Data = new TimeSheetDTO();
                var timeSheet = db.TimeSheet_Master.Where(a => a.TimeSheetID == modal.TimeSheetMaster.TimeSheetID).FirstOrDefault();
                if (timeSheet != null)
                {
                    var details = db.TimeSheet_Details.Where(a => a.TimeSheetID == timeSheet.TimeSheetID).ToList();
                    foreach (var item in details)
                    {
                        db.TimeSheet_Details.Remove(item);
                    }
                    var logs = db.TimeSheet_Log.Where(a => a.TimeSheetID == timeSheet.TimeSheetID).ToList();
                    foreach (var item in logs)
                    {
                        db.TimeSheet_Log.Remove(item);
                    }
                    db.SaveChanges();
                    db.TimeSheet_Master.Remove(timeSheet);
                    db.SaveChanges();
                }
                response = GetTimeSheetMasterList(modal, modal.From);
            }
            return response;
        }

        public ServiceResult<TimeSheetDTO> ChangeDates(TimeSheetDTO modal, string loginuser)
        {
            ServiceResult<TimeSheetDTO> response = new ServiceResult<TimeSheetDTO>();
            using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
            {
                int[] stDate = modal.StartDate.Split('-').Select(x => int.Parse(x)).ToArray();
                int[] edDate = modal.EndDate.Split('-').Select(x => int.Parse(x)).ToArray();
                response.Data = new TimeSheetDTO();
                response.Data.TimeSheetMaster = new TimeSheetMastr();
                response.Data.TimeSheetMaster.StartDate = new DateTime(stDate[2], stDate[0], stDate[1]);
                response.Data.TimeSheetMaster.EndDate = new DateTime(edDate[2], edDate[0], edDate[1]);
                response.Data = GetTimeSheetDetails(response.Data, loginuser, db);
            }
            return response;
        }

        public ServiceResult<string> SaveTimeSheet(TimeSheetDTO modal)
        {

            ServiceResult<string> response = new ServiceResult<string>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    int[] stDate = modal.StartDate.Split('-').Select(x => int.Parse(x)).ToArray();
                    int[] edDate = modal.EndDate.Split('-').Select(x => int.Parse(x)).ToArray();
                    if (modal.TimeSheetMaster.TimeSheetID > 0)
                    {
                        TimeSheet_Master tm = db.TimeSheet_Master.Where(a => a.TimeSheetID == modal.TimeSheetMaster.TimeSheetID).FirstOrDefault();
                        tm.UpdatedOn = DateTime.Now;
                        tm.Status = modal.TimeSheetMaster.Status;
                        tm.TotalHours = modal.TotalHours;
                        tm.OTHours = modal.OTHours;
                        db.SaveChanges();

                    }
                    else
                    {
                        TimeSheet_Master tm = new TimeSheet_Master();
                        tm.Client_ID = modal.UserDetails.Client_ID;
                        tm.CreatedOn = DateTime.Now;
                        tm.EndDate = new DateTime(edDate[2], edDate[0], edDate[1]);
                        tm.OTHours = modal.OTHours;
                        tm.StartDate = new DateTime(stDate[2], stDate[0], stDate[1]);
                        tm.Status = modal.TimeSheetMaster.Status;
                        tm.TotalHours = modal.TotalHours;
                        tm.UpdatedOn = DateTime.Now;
                        tm.User_ID = modal.UserDetails.User_ID;
                        db.TimeSheet_Master.Add(tm);
                        db.SaveChanges();
                        tm.TimeSheetCode = "TS" + tm.TimeSheetID.ToString().PadLeft(4, '0');
                        db.SaveChanges();
                        modal.TimeSheetMaster.TimeSheetID = tm.TimeSheetID;
                    }
                    SaveAndUpdateTimeSheetDetails(modal);
                    response.Data = "Success";
                }
            }
            catch (Exception ex)
            {
                response.Data = ex.Message;
            }
            return response;
        }

        public void SaveAndUpdateTimeSheetDetails(TimeSheetDTO modal)
        {
            using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
            {
                foreach (var item in modal.TimeSheetDetails)
                {
                    int i = 0;
                    System.Reflection.PropertyInfo[] properties = typeof(ElectricEase.Data.DataBase.USP_TimeSheetDetails_Result).GetProperties();
                    foreach (System.Reflection.PropertyInfo property in properties)
                    {
                        var res = property.GetValue(item, null);
                        if (res != null)
                        {
                            string[] val = res.ToString().Split('_');
                            if (val.Length > 1 && item.Job_ID.HasValue)
                            {
                                long detid = Convert.ToInt64(val[2]);
                                if (detid == 0 && item.IsDeleted == false)
                                {
                                    TimeSheet_Details td = new TimeSheet_Details();
                                    td.TimeSheetID = modal.TimeSheetMaster.TimeSheetID;
                                    td.Job_ID = item.Job_ID;
                                    td.CostCode = item.CostCode;
                                    td.NoOfHours = Convert.ToDecimal(string.IsNullOrEmpty(val[0]) ? "0" : val[0]);
                                    td.WorkDate = Convert.ToDateTime(val[1]);
                                    td.CreatedOn = DateTime.Now;
                                    td.UpdatedOn = DateTime.Now;
                                    db.TimeSheet_Details.Add(td);
                                    db.SaveChanges();
                                }
                                else if (detid > 0)
                                {
                                    TimeSheet_Details td = db.TimeSheet_Details.Where(a => a.DetID == detid).FirstOrDefault();
                                    if (item.IsDeleted == true)
                                    {
                                        db.TimeSheet_Details.Remove(td);
                                    }
                                    else
                                    {
                                        td.Job_ID = item.Job_ID;
                                        td.CostCode = item.CostCode;
                                        td.NoOfHours = Convert.ToDecimal(string.IsNullOrEmpty(val[0]) ? "0" : val[0]);
                                        td.WorkDate = Convert.ToDateTime(val[1]);
                                        td.UpdatedOn = DateTime.Now;
                                    }
                                    db.SaveChanges();
                                }
                                else
                                {

                                }
                            }
                        }
                        i++;
                    }

                }
            }
        }

        public ServiceResult<TimeSheetDTO> TimeSheetDetails(long? timeSheetID, string Loginuser, bool isView)
        {
            ServiceResult<TimeSheetDTO> response = new ServiceResult<TimeSheetDTO>();
            try
            {
                using (ElectricEaseEntitiesContext myContext = new ElectricEaseEntitiesContext())
                {
                    DayOfWeek day = DateTime.Now.DayOfWeek;
                    int days = day - DayOfWeek.Saturday;
                    //int days3 = day - DayOfWeek.Monday;
                    DayOfWeek desiredDay = DayOfWeek.Saturday;
                    int offsetAmount = (int)desiredDay - (int)DateTime.Now.DayOfWeek;
                    //DateTime lastWeekWednesday = DateTime.Now.AddDays(-7 + offsetAmount);
                    response.Data = new TimeSheetDTO();
                    response.Data.TimeSheetMaster = new TimeSheetMastr();
                    response.Data.TimeSheetDetails = new List<USP_TimeSheetDetails_Result>();
                    response.Data.TimeSheetMaster.TimeSheetID = timeSheetID ?? 0;
                    response.Data.TimeSheetMaster.StartDate = offsetAmount > 0 ? DateTime.Now.AddDays(-7 + offsetAmount) : DateTime.Now;
                    response.Data.TimeSheetMaster.EndDate = response.Data.TimeSheetMaster.StartDate.Value.AddDays(6);
                    response.Data.IsView = isView;
                    response.Data = GetTimeSheetDetails(response.Data, Loginuser, myContext);
                }

            }
            catch (Exception e)
            {

            }
            return response;
        }

        public TimeSheetDTO GetTimeSheetDetails(TimeSheetDTO modal, string Loginuser, ElectricEaseEntitiesContext myContext)
        {
            ReportsMasterData reportResponse = new ReportsMasterData();
            ClientMasterData ClientResponse = new ClientMasterData();
            modal.TimeSheetLogDetails = new List<TimeSheet_Log>();
            modal.LoginUser = ClientResponse.GetClientName(Loginuser);
            var result = myContext.TimeSheet_Master.Where(a => a.TimeSheetID == modal.TimeSheetMaster.TimeSheetID).Select(a => new TimeSheetMastr
            {
                TimeSheetID = a.TimeSheetID,
                TimeSheetCode = a.TimeSheetCode,
                Client_ID = a.Client_ID,
                User_ID = a.User_ID,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                CreatedOn = a.CreatedOn,
                UpdatedOn = a.UpdatedOn,
                Status = a.Status,
                TotalHours = a.TotalHours,
                OTHours = a.OTHours,
                ClientName = a.Client_Master.Client_Company,
                Comments = a.Comments

            }).FirstOrDefault();
            modal.TimeSheetMaster = result != null ? result : modal.TimeSheetMaster;
            modal.TimeSheetMaster.User_ID = modal.TimeSheetMaster.User_ID ?? Loginuser;
            modal.UserDetails = ClientResponse.GetClientName(modal.TimeSheetMaster.User_ID);
            modal.TimeSheetMaster.Client_ID = modal.UserDetails.Client_ID;
            //if (modal.TimeSheetMaster.TimeSheetID == 0)
            //{
            //    result = myContext.TimeSheet_Master.Where(a => EntityFunctions.TruncateTime(a.StartDate) == EntityFunctions.TruncateTime(modal.TimeSheetMaster.StartDate) && EntityFunctions.TruncateTime(a.EndDate) == EntityFunctions.TruncateTime(modal.TimeSheetMaster.EndDate) && a.User_ID == modal.UserDetails.User_ID && a.Client_ID == modal.UserDetails.Client_ID).Select(a => new TimeSheetMastr
            //    {
            //        TimeSheetID = a.TimeSheetID,
            //        TimeSheetCode = a.TimeSheetCode,
            //        Client_ID = a.Client_ID,
            //        User_ID = a.User_ID,
            //        StartDate = a.StartDate,
            //        EndDate = a.EndDate,
            //        CreatedOn = a.CreatedOn,
            //        UpdatedOn = a.UpdatedOn,
            //        Status = a.Status,
            //        TotalHours = a.TotalHours,
            //        OTHours = a.OTHours,
            //        ClientName = a.Client_Master.Client_Company,
            //        Comments = a.Comments

            //    }).FirstOrDefault();
            //}
            //modal.TimeSheetMaster = result != null ? result : modal.TimeSheetMaster;

            if (modal.TimeSheetMaster.TimeSheetID > 0)
            {
                modal.TimeSheetDetails = myContext.USP_TimeSheetDetails(modal.TimeSheetMaster.TimeSheetID, modal.TimeSheetMaster.StartDate.Value, modal.TimeSheetMaster.EndDate.Value).ToList();
                modal.TimeSheetLogDetails = myContext.TimeSheet_Log.Where(a => a.Status == "Rejected" && a.TimeSheetID == modal.TimeSheetMaster.TimeSheetID).ToList();
            }
            else
            {
                var timeSheet = myContext.TimeSheet_Master.Where(a => EntityFunctions.TruncateTime(a.StartDate) == EntityFunctions.TruncateTime(modal.TimeSheetMaster.StartDate) && EntityFunctions.TruncateTime(a.EndDate) == EntityFunctions.TruncateTime(modal.TimeSheetMaster.EndDate) && a.User_ID == modal.UserDetails.User_ID && a.Client_ID == modal.UserDetails.Client_ID).FirstOrDefault();
                if (timeSheet == null)
                {
                    modal.TimeSheetDetails = myContext.USP_TimeSheetDetails(modal.TimeSheetMaster.TimeSheetID, modal.TimeSheetMaster.StartDate.Value, modal.TimeSheetMaster.EndDate.Value).ToList();
                }
            }
            ServiceResultList<Job_MasterInfo> jobs = reportResponse.GetDescriptionReportsList(modal.TimeSheetMaster.Client_ID ?? 0);
            if (jobs != null)
            {
                modal.Jobs = jobs.ListData;
                modal.Jobs.ToList().ForEach(u => u.Job_Description = (u.Job_ID) + " - " + u.Client_Name);
            }
            modal.ReportingManager = GetReportingManager(modal.TimeSheetMaster.Client_ID ?? 0, myContext);
            if (!modal.IsView)
            {
                modal.TimeSheetMasterDetails = myContext.TimeSheet_Master.Where(a => a.Client_ID == modal.UserDetails.Client_ID && a.User_ID == modal.UserDetails.User_ID).Select(a => new TimeSheetMastr
                {
                    ClientName = a.Client_Master.Client_Company,
                    Client_ID = a.Client_ID,
                    CreatedOn = a.CreatedOn,
                    EndDate = a.EndDate,
                    OTHours = a.OTHours,
                    StartDate = a.StartDate,
                    Status = a.Status,
                    TimeSheetCode = a.TimeSheetCode,
                    TimeSheetID = a.TimeSheetID,
                    TotalHours = a.TotalHours,
                    UpdatedOn = a.UpdatedOn,
                    User_ID = a.User_ID,
                    Comments = a.Comments,
                    ReportingManager = myContext.Account_Master.Where(b => b.Client_ID == a.Client_ID && b.Site_Administrator == true).Select(b => b.First_Name + " " + (b.Last_Name ?? "")).FirstOrDefault()
                }).ToList(); ;
            }
            else
            {
                modal.TimeSheetMasterDetails = new List<TimeSheetMastr>();
            }

            return modal;
        }

        public ServiceResult<string> ApproveOrRejectTimeSheet(TimeSheetDTO modal)
        {
            ServiceResult<string> response = new ServiceResult<string>();
            try
            {
                using (ElectricEaseEntitiesContext myContext = new ElectricEaseEntitiesContext())
                {
                    var timeSheet = myContext.TimeSheet_Master.Where(a => a.TimeSheetID == modal.TimeSheetMaster.TimeSheetID).FirstOrDefault();
                    if (timeSheet != null)
                    {
                        timeSheet.Status = modal.TimeSheetMaster.Status;
                        //timeSheet.Comments = modal.TimeSheetMaster.Comments;
                        timeSheet.UpdatedOn = DateTime.Now;
                        TimeSheet_Log log = new TimeSheet_Log();
                        log.TimeSheetID = timeSheet.TimeSheetID;
                        log.Comments = modal.TimeSheetMaster.Comments;
                        log.CreatedOn = DateTime.Now;
                        log.Status = modal.TimeSheetMaster.Status;
                        myContext.TimeSheet_Log.Add(log);
                        myContext.SaveChanges();

                        response.Data = "Success";
                    }
                }
            }
            catch (Exception e)
            {
                response.Data = e.Message;
            }
            return response;
        }

        public string GetReportingManager(int clientId, ElectricEaseEntitiesContext myContext)
        {
            return myContext.Account_Master.Where(a => a.Client_ID == clientId && a.Site_Administrator == true).Select(a => a.First_Name + " " + (a.Last_Name ?? "")).FirstOrDefault();
        }

    }



    public class TimeSheetDTO
    {
        public List<USP_TimeSheetDetails_Result> TimeSheetDetails { get; set; }
        public TimeSheetMastr TimeSheetMaster { get; set; }
        public List<TimeSheetMastr> TimeSheetMasterDetails { get; set; }
        public IList<Job_MasterInfo> Jobs { get; set; }
        public Account_MasterInfo UserDetails { get; set; }
        public Account_MasterInfo LoginUser { get; set; }
        public decimal TotalHours { get; set; }
        public decimal OTHours { get; set; }
        public decimal RegularHours { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int From { get; set; }
        public bool IsView { get; set; }
        public bool IsApprovedOrRejected { get; set; }
        public string ReportingManager { get; set; }
        public List<TimeSheet_Log> TimeSheetLogDetails { get; set; }
    }
}
