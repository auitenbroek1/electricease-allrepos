using ClosedXML.Excel;
using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.ReportsMaster;
using ElectricEase.BLL.TimeSheetMaster;
using ElectricEase.Data.DataBase;
using ElectricEase.Data.TimeSheetMaster;
using ElectricEase.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ElectricEase.Web.Controllers
{
    [Authorize(Roles = "SiteAdmin,JobDescriptionReportAdmin")]
    public class TimeSheetController : Controller
    {
        //
        // GET: /TimeSheet/

        public ActionResult Index()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            TimeSheetMasterBLL timeSheetResponse = new TimeSheetMasterBLL();
            ServiceResult<TimeSheetDTO> reponse = new ServiceResult<TimeSheetDTO>();
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            reponse.Data = new TimeSheetDTO();
            reponse.Data.UserDetails = new Account_MasterInfo();
            reponse.Data.UserDetails.User_ID = username;
            if (username != null)
            {
                reponse = timeSheetResponse.GetTimeSheetMasterList(reponse.Data, 0);

            }
            // ViewBag.JobReportRights = result.Job_Description_Report;


            //if (ViewData["PageNo"] == null || ViewData["PageNo"] == "")
            //    ViewData["PageNo"] = 1;
            //if (Request.Cookies["currentpage"] != null && Request.Cookies["currentpage"].Value != null && Request.Cookies["currentpage"].Value != "")
            //{
            //    ViewData["PageNo"] = Convert.ToInt32(Request.Cookies["currentpage"].Value);
            //}
            // ViewBag.JobReportRights = false;
            return View(reponse.Data);
        }

        
        public ActionResult TimeCard(string status)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            TimeSheetMasterBLL timeSheetResponse = new TimeSheetMasterBLL();
            ServiceResult<TimeSheetDTO> reponse = new ServiceResult<TimeSheetDTO>();
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            reponse.Data = new TimeSheetDTO();
            reponse.Data.UserDetails = new Account_MasterInfo();
            reponse.Data.UserDetails.User_ID = username;
            if (username != null)
            {
                reponse = timeSheetResponse.GetTimeSheetMasterList(reponse.Data, 1);
                reponse.Data.IsApprovedOrRejected = (string.IsNullOrEmpty(status) || status == "Saved" || status == "Pending") ? false : true;
            }
            return View("Index", reponse.Data);
        }

        public ActionResult NewTimeSheet(long? timeSheetId, int from)
        {
            TimeSheetMasterBLL timeSheetResponse = new TimeSheetMasterBLL();
            ServiceResult<TimeSheetDTO> responce = new ServiceResult<TimeSheetDTO>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            responce = timeSheetResponse.TimeSheetDetails(timeSheetId, Loginuser, false);
            responce.Data.From = from;

            if(timeSheetId == null)
            {
                ViewBag.HeadText = "Add Timesheet";
            }
            else
            {
                ViewBag.HeadText = "Edit Timesheet";
            }
            return View("NewTimeSheet", responce.Data);
        }

        public ActionResult ViewTimeSheet(long? timeSheetId, int from)
        {
            TimeSheetMasterBLL timeSheetResponse = new TimeSheetMasterBLL();
            ServiceResult<TimeSheetDTO> responce = new ServiceResult<TimeSheetDTO>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            responce = timeSheetResponse.TimeSheetDetails(timeSheetId, Loginuser, true);
            responce.Data.From = from;
            ViewBag.HeadText = "View Timesheet";
            return View("NewTimeSheet", responce.Data);
        }

        [HttpPost]
        public ActionResult ChangeDates(TimeSheetDTO modal)
        {
            ModelState.Clear();
            TimeSheetMasterBLL timeSheetResponse = new TimeSheetMasterBLL();
            ServiceResult<TimeSheetDTO> responce = new ServiceResult<TimeSheetDTO>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            responce = timeSheetResponse.ChangeDates(modal, Loginuser);
            return PartialView("_TimeSheetDetails", responce.Data);
        }

        [HttpPost]
        public ActionResult DateWiseTimeSheets(TimeSheetDTO modal)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            TimeSheetMasterBLL timeSheetResponse = new TimeSheetMasterBLL();
            ServiceResult<TimeSheetDTO> reponse = new ServiceResult<TimeSheetDTO>();
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            reponse.Data = modal;
            reponse.Data.UserDetails = new Account_MasterInfo();
            reponse.Data.UserDetails.User_ID = username;
            if (username != null)
            {
                reponse = timeSheetResponse.GetTimeSheetMasterList(reponse.Data, modal.From);

            }
            return PartialView("_TimeSheetMasterDetails", reponse.Data);
        }

        [HttpPost]
        public ActionResult AddJobRow(TimeSheetDTO modal)
        {
            ReportsMasterDLL reportResponse = new ReportsMasterDLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            modal.Jobs = reportResponse.GetMyDescriptionReport(result.Client_ID).ListData;
            modal.Jobs.ToList().ForEach(u => u.Job_Description = (u.Job_ID) + " - " + u.Client_Name); ;
            USP_TimeSheetDetails_Result time = new USP_TimeSheetDetails_Result();
            System.Reflection.PropertyInfo[] properties = typeof(ElectricEase.Data.DataBase.USP_TimeSheetDetails_Result).GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                var res = property.GetValue(modal.TimeSheetDetails[0], null);
                if (res != null)
                {
                    string[] val = res.ToString().Split('_');
                    if (val.Length > 2)
                    {
                        val[0] = "0.00";
                        val[2] = "0";
                        switch (property.Name)
                        {
                            case "Monday":
                                time.Monday = string.Join("_", val); ;
                                break;
                            case "Tuesday":
                                time.Tuesday = string.Join("_", val); ;
                                break;
                            case "Wednesday":
                                time.Wednesday = string.Join("_", val); ;
                                break;
                            case "Thursday":
                                time.Thursday = string.Join("_", val); ;
                                break;
                            case "Friday":
                                time.Friday = string.Join("_", val); ;
                                break;
                            case "Saturday":
                                time.Saturday = string.Join("_", val); ;
                                break;
                            case "Sunday":
                                time.Sunday = string.Join("_", val); ;
                                break;
                            default:
                                Console.WriteLine("Default case");
                                break;
                        }
                    }
                }
            }
            //time.Monday = "0.00";
            //time.Tuesday = "0.00";
            //time.Wednesday = "0.00";
            //time.Thursday = "0.00";
            //time.Friday = "0.00";
            //time.Saturday = "0.00";
            //time.Sunday = "0.00";
            time.IsDeleted = false;
            modal.TimeSheetDetails.Add(time);
            return PartialView("_TimeSheetDetails", modal);
        }

        [HttpPost]
        public ActionResult RemoveJobRow(TimeSheetDTO modal)
        {
            ModelState.Clear();
            ReportsMasterDLL reportResponse = new ReportsMasterDLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            modal.Jobs = reportResponse.GetMyDescriptionReport(result.Client_ID).ListData;
            modal.Jobs.ToList().ForEach(u => u.Job_Description = (u.Job_ID) + " - " + u.Client_Name); ;

            return PartialView("_TimeSheetDetails", modal);
        }

        [HttpPost]
        public string SaveTimeSheet(TimeSheetDTO modal)
        {
            TimeSheetMasterBLL timeSheetResponse = new TimeSheetMasterBLL();
            ServiceResult<string> responce = timeSheetResponse.SaveTimeSheet(modal);
            return responce.Data;
        }

        [HttpPost]
        public string ApproveOrRejectTimeSheet(TimeSheetDTO modal)
        {
            TimeSheetMasterBLL timeSheetResponse = new TimeSheetMasterBLL();
            ServiceResult<string> responce = new ServiceResult<string>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            responce = timeSheetResponse.ApproveOrRejectTimeSheet(modal, Loginuser);
            return responce.Data;
        }

        [HttpPost]
        public ActionResult DeleteTimeSheet(TimeSheetDTO modal)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            TimeSheetMasterBLL timeSheetResponse = new TimeSheetMasterBLL();
            ServiceResult<TimeSheetDTO> reponse = new ServiceResult<TimeSheetDTO>();
            string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            reponse.Data = modal;
            reponse.Data.UserDetails = new Account_MasterInfo();
            reponse.Data.UserDetails.User_ID = username;
            if (username != null)
            {
                reponse = timeSheetResponse.DeleteTimeSheet(reponse.Data);

            }
            return PartialView("_TimeSheetMasterDetails", reponse.Data);
        }

        public void TimeSheetExport(long? timeSheetId, int from)
        {
            TimeSheetMasterBLL timeSheetResponse = new TimeSheetMasterBLL();
            ServiceResult<TimeSheetDTO> responce = new ServiceResult<TimeSheetDTO>();
            List<USP_TimeSheetDetails_Result> timeSheetData = new List<USP_TimeSheetDetails_Result>();
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            decimal totalMondayHours = 0;
            decimal totalTuesdayHours = 0;
            decimal totalWednesdayHours = 0;
            decimal totalThursdayHours = 0;
            decimal totalFridayHours = 0;
            decimal totalSaturdayHours = 0;
            decimal totalSundayHours = 0;

            float otMondayHours = 0;
            float otTuesdayHours = 0;
            float otWednesdayHours = 0;
            float otThursdayHours = 0;
            float otFridayHours = 0;
            float otSaturdayHours = 0;
            float otSundayHours = 0;


            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            responce = timeSheetResponse.TimeSheetDetails(timeSheetId, Loginuser, true);
            responce.Data.From = from;
            timeSheetData = responce.Data.TimeSheetDetails;

            //for (int i = 0; i < timeSheetData.Count; i++)
            //{

            //    if (SplitHours(timeSheetData[i].Monday) <= 8)
            //    {
            //        totalMondayHours += SplitHours(timeSheetData[i].Monday);
            //    }
            //    else
            //    {
            //        totalMondayHours += 8;
            //        otMondayHours += SplitHours(timeSheetData[i].Monday) - 8;
            //    }

            //    if (SplitHours(timeSheetData[i].Tuesday) <= 8)
            //    {
            //        totalTuesdayHours += SplitHours(timeSheetData[i].Tuesday);
            //    }
            //    else
            //    {
            //        totalTuesdayHours += 8;
            //        otTuesdayHours += SplitHours(timeSheetData[i].Tuesday) - 8;
            //    }

            //    if (SplitHours(timeSheetData[i].Wednesday) <= 8)
            //    {
            //        totalWednesdayHours += SplitHours(timeSheetData[i].Wednesday);
            //    }
            //    else
            //    {
            //        totalWednesdayHours += 8;
            //        otWednesdayHours += SplitHours(timeSheetData[i].Wednesday) - 8;
            //    }

            //    if (SplitHours(timeSheetData[i].Thursday) <= 8)
            //    {
            //        totalThursdayHours += SplitHours(timeSheetData[i].Thursday);
            //    }
            //    else
            //    {
            //        totalThursdayHours += 8;
            //        otThursdayHours += SplitHours(timeSheetData[i].Thursday) - 8;
            //    }

            //    if (SplitHours(timeSheetData[i].Friday) <= 8)
            //    {
            //        totalFridayHours += SplitHours(timeSheetData[i].Friday);
            //    }
            //    else
            //    {
            //        totalFridayHours += 8;
            //        otFridayHours += SplitHours(timeSheetData[i].Friday) - 8;
            //    }

            //    if (SplitHours(timeSheetData[i].Saturday) <= 8)
            //    {
            //        totalSaturdayHours += SplitHours(timeSheetData[i].Saturday);
            //    }
            //    else
            //    {
            //        totalSaturdayHours += 8;
            //        otSaturdayHours += SplitHours(timeSheetData[i].Saturday) - 8;
            //    }

            //    if (SplitHours(timeSheetData[i].Sunday) <= 8)
            //    {
            //        totalSundayHours += SplitHours(timeSheetData[i].Sunday);
            //    }
            //    else
            //    {
            //        totalSundayHours += 8;
            //        otSundayHours += SplitHours(timeSheetData[i].Sunday) - 8;
            //    }
            //}
            totalMondayHours = timeSheetData.Where(a => a.IsDeleted == false).Sum(a => Convert.ToDecimal(a.Monday.Split('_')[0]));
            totalTuesdayHours = timeSheetData.Where(a => a.IsDeleted == false).Sum(a => Convert.ToDecimal(a.Tuesday.Split('_')[0]));
            totalWednesdayHours = timeSheetData.Where(a => a.IsDeleted == false).Sum(a => Convert.ToDecimal(a.Wednesday.Split('_')[0]));
            totalThursdayHours = timeSheetData.Where(a => a.IsDeleted == false).Sum(a => Convert.ToDecimal(a.Thursday.Split('_')[0]));
            totalFridayHours = timeSheetData.Where(a => a.IsDeleted == false).Sum(a => Convert.ToDecimal(a.Friday.Split('_')[0]));
            totalSaturdayHours = timeSheetData.Where(a => a.IsDeleted == false).Sum(a => Convert.ToDecimal(a.Saturday.Split('_')[0]));
            totalSundayHours = timeSheetData.Where(a => a.IsDeleted == false).Sum(a => Convert.ToDecimal(a.Sunday.Split('_')[0]));
            decimal totalHours = totalMondayHours + totalTuesdayHours + totalWednesdayHours + totalThursdayHours
                + totalFridayHours + totalSaturdayHours + totalSundayHours;

            float totalOTHours = otMondayHours + otTuesdayHours + otWednesdayHours + otThursdayHours +
                otFridayHours + otSaturdayHours + otSundayHours;

            var filename = "TimeSheet.xlsx";
            string AppLocation = System.Web.HttpContext.Current.Server.MapPath("~/Content/ExcelFiles/" + filename);

            if (System.IO.File.Exists(AppLocation))
            {
                System.IO.File.Delete(AppLocation);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                IXLWorksheet ws = wb.Worksheets.Add("Time Sheet");

                ws.Row(1).Height = 32;
                ws.Row(3).Height = 25;

                ws.Range("A1:J1").Style.Alignment.WrapText = true;
                ws.Style.Font.FontName = "Calibri";
                ws.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                ws.Row(3).Style.Font.FontSize = 12;
                ws.Range("A3:F3").Style.Font.SetBold(true);
                ws.Row(3).Style.Alignment.WrapText = true;
                ws.Row(3).Height = 20;

                ws.Row(6).Style.Font.FontSize = 12;
                ws.Range("A6:F6").Style.Font.SetBold(true);
                ws.Row(6).Style.Alignment.WrapText = true;
                ws.Row(6).Height = 30;

                ws.Cell("A1").Value = "Timesheet Report (" + SplitDays(timeSheetData[0].Saturday) + " to " + SplitDays(timeSheetData[0].Friday) + ")";
                ws.Cell("A1").Style.Font.FontSize = 16;
                ws.Range("A1:J1").Row(1).Merge();

                ws.Cell("A3").WorksheetColumn().Width = 25;
                ws.Cell("A3").Value = "Timesheet ID";

                ws.Cell("B3").WorksheetColumn().Width = 25;
                ws.Cell("B3").Value = "Full Name";

                ws.Cell("C3").WorksheetColumn().Width = 25;
                ws.Cell("C3").Value = "Reporting Manager";

                ws.Cell("D3").WorksheetColumn().Width = 25;
                ws.Cell("D3").Value = "Status";

                ws.Cell("E3").WorksheetColumn().Width = 25;
                ws.Cell("E3").Value = "Start Date";

                ws.Cell("F3").WorksheetColumn().Width = 25;
                ws.Cell("F3").Value = "End Date";

                ws.Cell("A4").WorksheetColumn().Width = 25;
                ws.Cell("A4").Value = responce.Data.TimeSheetMaster.TimeSheetCode;

                ws.Cell("B4").WorksheetColumn().Width = 25;
                ws.Cell("B4").Value = textInfo.ToTitleCase(responce.Data.UserDetails.First_Name + " " + responce.Data.UserDetails.Last_Name);

                ws.Cell("C4").WorksheetColumn().Width = 25;
                ws.Cell("C4").Value = textInfo.ToTitleCase(responce.Data.ReportingManager);

                ws.Cell("D4").WorksheetColumn().Width = 25;
                ws.Cell("D4").Value = responce.Data.TimeSheetMaster.Status;

                ws.Cell("E4").WorksheetColumn().Width = 25;
                ws.Cell("E4").Value = SplitDays(timeSheetData[0].Saturday);

                ws.Cell("F4").WorksheetColumn().Width = 25;
                ws.Cell("F4").Value = SplitDays(timeSheetData[0].Friday);

                ws.Range("C4:E4").Style.Font.SetBold(false);

                ws.Cell("A6").WorksheetColumn().Width = 25;
                ws.Cell("A6").Value = "Job_ID & Client Name";

                ws.Cell("B6").WorksheetColumn().Width = 25;
                ws.Cell("B6").Value = "Cost Codes";

                ws.Cell("C6").WorksheetColumn().Width = 25;
                ws.Cell("C6").Value = "Saturday(" + SplitDays(timeSheetData[0].Saturday) + ")";

                ws.Cell("D6").WorksheetColumn().Width = 25;
                ws.Cell("D6").Value = "Sunday(" + SplitDays(timeSheetData[0].Sunday) + ")";

                ws.Cell("E6").WorksheetColumn().Width = 25;
                ws.Cell("E6").Value = "Monday(" + SplitDays(timeSheetData[0].Monday) + ")";

                ws.Cell("F6").WorksheetColumn().Width = 25;
                ws.Cell("F6").Value = "Tuesday(" + SplitDays(timeSheetData[0].Tuesday) + ")";

                ws.Cell("G6").WorksheetColumn().Width = 25;
                ws.Cell("G6").Value = "Wednesday(" + SplitDays(timeSheetData[0].Wednesday) + ")";

                ws.Cell("H6").WorksheetColumn().Width = 25;
                ws.Cell("H6").Value = "Thursday(" + SplitDays(timeSheetData[0].Thursday) + ")";

                ws.Cell("I6").WorksheetColumn().Width = 25;
                ws.Cell("I6").Value = "Friday(" + SplitDays(timeSheetData[0].Friday) + ")";

                ws.Cell("J6").WorksheetColumn().Width = 25;
                ws.Cell("J6").Value = "Project Hours";

                int rownumber = 7;

                foreach (var item in timeSheetData)
                {
                    var total = 0.00;

                    total = SplitHours(item.Monday) + SplitHours(item.Tuesday) + SplitHours(item.Wednesday) +
                        SplitHours(item.Thursday) + SplitHours(item.Friday) + SplitHours(item.Saturday) + SplitHours(item.Sunday);

                    ws.Row(rownumber).Height = 20;
                    ws.Cell(rownumber, 1).Value = textInfo.ToTitleCase(item.Job);
                    ws.Cell(rownumber, 2).Value = item.CostCode;
                    ws.Cell(rownumber, 3).Value = SplitHours(item.Saturday);
                    ws.Cell(rownumber, 4).Value = SplitHours(item.Sunday);
                    ws.Cell(rownumber, 5).Value = SplitHours(item.Monday);
                    ws.Cell(rownumber, 6).Value = SplitHours(item.Tuesday);
                    ws.Cell(rownumber, 7).Value = SplitHours(item.Wednesday);
                    ws.Cell(rownumber, 8).Value = SplitHours(item.Thursday);
                    ws.Cell(rownumber, 9).Value = SplitHours(item.Friday);
                    ws.Cell(rownumber, 10).Value = Math.Round(total, 2);
                    rownumber += 1;
                }

                var tableRange = ("A6:J" + (rownumber - 1)).ToString();

                ws.Range(tableRange).Style.Border.InsideBorder = XLBorderStyleValues.Medium;
                ws.Range(tableRange).Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                ws.Range("A6:J6").Style.Font.SetBold(true);
                ws.Cells("A6:J6").Style.Fill.BackgroundColor = XLColor.SteelBlue;
                ws.Cells("A6:J6").Style.Font.FontColor = XLColor.White;

                ws.Row(rownumber + 1).Height = 20;

                ws.Cell(rownumber + 1, 3).WorksheetColumn().Width = 25;
                ws.Cell(rownumber + 1, 3).Value = "Total Hours";
                ws.Cell(rownumber + 1, 3).WorksheetColumn().Style.Font.SetBold(true);
                ws.Cell(rownumber + 1, 3).WorksheetColumn().Style.Font.FontSize = 12;

                ws.Cell(rownumber + 1, 4).WorksheetColumn().Width = 25;
                ws.Cell(rownumber + 1, 4).Value = "Regular Hours";
                ws.Cell(rownumber + 1, 4).WorksheetColumn().Style.Font.SetBold(true);
                ws.Cell(rownumber + 1, 4).WorksheetColumn().Style.Font.FontSize = 12;

                ws.Cell(rownumber + 1, 5).WorksheetColumn().Width = 25;
                ws.Cell(rownumber + 1, 5).Value = "OT Hours";
                ws.Cell(rownumber + 1, 5).WorksheetColumn().Style.Font.SetBold(true);
                ws.Cell(rownumber + 1, 5).WorksheetColumn().Style.Font.FontSize = 12;

                ws.Row(rownumber + 2).Style.Font.SetBold(false);

                ws.Cell(rownumber + 2, 3).WorksheetColumn().Width = 25;
                ws.Cell(rownumber + 2, 3).WorksheetRow().Height = 20;
                ws.Cell(rownumber + 2, 3).Value = Math.Round(totalHours, 2);
                ws.Cell(rownumber + 2, 3).WorksheetColumn().Style.Font.FontSize = 12;
                ws.Cell(rownumber + 2, 3).Style.Fill.BackgroundColor = XLColor.LawnGreen;

                ws.Cell(rownumber + 2, 4).WorksheetColumn().Width = 25;
                ws.Cell(rownumber + 2, 4).WorksheetRow().Height = 20;
                ws.Cell(rownumber + 2, 4).Value = Math.Round((totalHours <= 40 ? totalHours : 40), 2);
                ws.Cell(rownumber + 2, 4).WorksheetColumn().Style.Font.FontSize = 12;
                ws.Cell(rownumber + 2, 4).Style.Fill.BackgroundColor = XLColor.Yellow;

                ws.Cell(rownumber + 2, 5).WorksheetColumn().Width = 25;
                ws.Cell(rownumber + 2, 5).WorksheetRow().Height = 20;
                ws.Cell(rownumber + 2, 5).Value = Math.Round((totalHours > 40 ? (totalHours - (totalHours <= 40 ? totalHours : 40)) : 0), 2);
                ws.Cell(rownumber + 2, 5).WorksheetColumn().Style.Font.FontSize = 12;
                ws.Cell(rownumber + 2, 5).Style.Fill.BackgroundColor = XLColor.Magenta;

                ws.Cell(rownumber + 4, 3).WorksheetColumn().Width = 25;
                ws.Cell(rownumber + 4, 3).WorksheetColumn().Style.Font.SetBold(true);
                ws.Cell(rownumber + 4, 3).Value = "Comments";
                ws.Cell(rownumber + 4, 3).WorksheetColumn().Style.Font.FontSize = 12;


                for (int i = 0; i < responce.Data.TimeSheetLogDetails.Count; i++)
                {
                    ws.Cell(rownumber + 5 + i, 3).WorksheetColumn().Width = 25;
                    ws.Cell(rownumber + 5 + i, 3).Value = "#" + responce.Data.TimeSheetLogDetails[i].CreatedOn;
                    ws.Cell(rownumber + 5 + i, 3).WorksheetColumn().Style.Font.FontSize = 12;
                    ws.Cell(rownumber + 5 + i, 3).WorksheetColumn().Style.Font.SetBold(true);

                    var range = ("D" + (rownumber + 5 + i) + ":F" + (rownumber + 5 + i)).ToString();

                    ws.Range(range).Row(rownumber + 5 + i).Merge();

                    ws.Cell(rownumber + 5 + i, 4).WorksheetColumn().Width = 25;
                    ws.Cell(rownumber + 5 + i, 4).Value = responce.Data.TimeSheetLogDetails[i].Comments;
                    ws.Cell(rownumber + 5 + i, 4).WorksheetColumn().Style.Font.FontSize = 12;
                    ws.Cell(rownumber + 5 + i, 4).WorksheetColumn().Style.Font.SetBold(false);
                    ws.Row(rownumber + 5 + i).Height = 20;
                    ws.Row(rownumber + 5 + i).Style.Alignment.WrapText = true;

                }

                wb.SaveAs(AppLocation);

                var fileName = "TimeSheet_" + responce.Data.TimeSheetMaster.TimeSheetCode + "_from_"
                    + SplitDays(timeSheetData[0].Saturday) + "_to_"
                    + SplitDays(timeSheetData[0].Friday) + ".xlsx";

                WebClient req = new WebClient();
                HttpResponse response = System.Web.HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + fileName + "\"");
                byte[] data = req.DownloadData(AppLocation);
                response.BinaryWrite(data);
                response.End();
            }
        }


        public double SplitHours(string date)
        {
            List<string> list = new List<string>();

            list = date.Split('_').ToList();

            return double.Parse(list[0].ToString());
        }

        public string SplitDays(string date)
        {
            List<string> list = new List<string>();

            list = date.Split('_').ToList();

            var dateParts = list[1].Split('-');

            return dateParts[1] + "-" + dateParts[2] + "-" + dateParts[0].ToString();
        }

    }
}
