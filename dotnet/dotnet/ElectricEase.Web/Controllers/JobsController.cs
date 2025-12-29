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
namespace ElectricEase.Web.Controllers
{
    public class JobsController : Controller
    {
        //
        // GET: /Jobs/
        public  static Job_MasterInfo JobMasterTempObj = new Job_MasterInfo();
        List<Parts_DetailsInfoList> parts = new List<Parts_DetailsInfoList>();
        List<Parts_GrandTotal> partGrandTotalList = new List<Parts_GrandTotal>();
        List<Labor_GrandTotal> laborGrandTotalList = new List<Labor_GrandTotal>();
        public decimal partsListEstTotal;
        public decimal partsListActTotal;
        public decimal partsListExpTotal;
        public decimal laborListEstTotal;
        public decimal laborListActTotal;
        public decimal laborListExpTotal;
        

       

        public ActionResult JobsIndex()
        {
            return View();
        }
        //public ActionResult AddNewJobDetails()
        //{
        //    return View();

        //}
          [OutputCache(Duration = 30)]
        public ActionResult AddJobDetails()
        {
              //TempData["ASPartsListData"] = "";
            TempData.Remove("ASPartsListData");
            TempData.Remove("ASLaborListData");
            TempData.Remove("MyASparts");
            TempData.Remove("MyAsLabourList");
            TempData.Remove("MyLegalList");
            TempData.Remove("MyLaborListInJob");
            TempData.Remove("MyPartsInJob");
            TempData.Remove("JobAssembliesParts");
            TempData.Remove("JobAssembliesLabor");
            if (TempData["JobSuccessMsg"]!=null)
           {
               ViewBag.JobSuccessMsg = TempData["JobSuccessMsg"];
           }
            return View(new Job_MasterInfo());
        }
        public PartialViewResult AssembliesPartialView()
        {
            return PartialView("_AssembliesList Details",new Job_MasterInfo());
        }

        public PartialViewResult GetAssembliesDetails(string Assemblies_Name)
        {
            TempData["selectedASname"] = Assemblies_Name;
            ViewBag.AssembliesName = Assemblies_Name;
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
            Job_MasterInfo JobModelOBj = new Job_MasterInfo();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.GetAssembliesListDetails(Assemblies_Name, result.Client_ID);
            var assemliesData = response.Data;
            //JobModelOBj.PartsListData = assemliesData.PartsListData;
           // JobModelOBj.LabourListData = assemliesData.LabourListData;
            //var jsonResult = Json(response.Data, JsonRequestBehavior.AllowGet);
            // jsonResult.MaxJsonLength = int.MaxValue;
            ViewBag.assembliesName = Assemblies_Name;
           // TempData["Assemblies_Name"] = Assemblies_Name;
           //TempData["ASPartsListData"] = "";
           // TempData["ASLaborListData"] = "";
           // TempData["MyASparts"]="";
           // TempData["MyAsLabourList"]="";
           // TempData["MyLegalList"]="";
           // TempData["MyLaborListInJob"]="";
           // TempData["MyPartsInJob"]="";
            TempData.Remove("ASPartsListData");
            TempData.Remove("ASLaborListData");
            TempData.Remove("MyASparts");
            TempData.Remove("MyAsLabourList");
            TempData.Remove("MyLegalList");
            TempData.Remove("MyLaborListInJob");
            TempData.Remove("MyPartsInJob");
            TempData.Remove("JobAssembliesParts");
            TempData.Remove("JobAssembliesLabor");
            return PartialView("_AssembliesList Details", JobModelOBj);
            //return Json( JsonRequestBehavior.AllowGet);
        }

        //public PartialViewResult GetAssembliesPartsDetails(string Assemblies_Name)
        //{
        //    TempData["selectedASname"] = Assemblies_Name;
        //    ViewBag.AssembliesName = Assemblies_Name;
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
        //    Job_MasterInfo JobModelOBj = new Job_MasterInfo();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    response = AssembliesResponse.GetAssembliesListDetails(Assemblies_Name, result.Client_ID);
        //    var assemliesData = response.Data;
        //    List<AssembliesParts_DetailsInfoList> objSumCal = new List<AssembliesParts_DetailsInfoList>();
        //    foreach (var objCalculate in assemliesData.PartsListData)
        //    {
        //        objCalculate.Expected_Total = objCalculate.Part_Cost * objCalculate.Actual_Qty;
        //        objCalculate.Estimated_Total = objCalculate.Resale_Cost * objCalculate.Estimated_Qty;
        //        objCalculate.Actual_Total = objCalculate.Resale_Cost * objCalculate.Actual_Qty;
        //        objSumCal.Add(new AssembliesParts_DetailsInfoList { Part_Number = objCalculate.Part_Number, Part_Cost = objCalculate.Part_Cost, Resale_Cost = objCalculate.Resale_Cost, Estimated_Qty = objCalculate.Estimated_Qty, Actual_Qty = objCalculate.Actual_Qty, Expected_Total = objCalculate.Expected_Total, Estimated_Total = objCalculate.Estimated_Total, Actual_Total = objCalculate.Actual_Total });
        //    }
        //    AssembliesParts_DetailsInfoList objcal = new AssembliesParts_DetailsInfoList();
        //    Parts_GrandTotal partsgrand = new Parts_GrandTotal();

        //    foreach (AssembliesParts_DetailsInfoList grandTotal in objSumCal)
        //    {
        //        partsgrand.ExpGrandTotal += grandTotal.Expected_Total;
        //        partsgrand.EstGrandTotal += grandTotal.Estimated_Total;
        //        partsgrand.ActGrandTotal += grandTotal.Actual_Total;
        //    }
        //    partGrandTotalList.Add(new Parts_GrandTotal { ExpGrandTotal = partsgrand.ExpGrandTotal, EstGrandTotal = partsgrand.EstGrandTotal, ActGrandTotal = partsgrand.ActGrandTotal });
        //    ViewData["GrandTotalList"] = partGrandTotalList.ToList();
        //    TempData["PartsGrandTotalList"] = partGrandTotalList.ToList();
        //   // JobModelOBj.PartsListData = objSumCal;
        //    //JobModelOBj.PartsListData = assemliesData.PartsListData;
        //    //JobModelOBj.LabourListData = assemliesData.LabourListData;
        //    //var jsonResult = Json(response.Data, JsonRequestBehavior.AllowGet);
        //   // jsonResult.MaxJsonLength = int.MaxValue;
           
            
        //   ViewBag.assembliesName = TempData["Assemblies_Name"];
        //   TempData["ASPartsListData"] = null;
        //   TempData["ASLaborListData"] = null;
        //   TempData["MyASparts"] = null;
        //   TempData["MyAsLabourList"] = null;
        //   TempData["MyLegalList"] = null;
        //   TempData["MyLaborListInJob"] = null;
        //   TempData["MyPartsInJob"] = null;
        //   TempData.Remove("ASPartsListData");
        //   TempData.Remove("ASLaborListData");
        //   TempData.Remove("MyASparts");
        //   TempData.Remove("MyAsLabourList");
        //   TempData.Remove("MyLegalList");
        //   TempData.Remove("MyLaborListInJob");
        //   TempData.Remove("MyPartsInJob");
        //    return PartialView("_AssembliesPartsDetailsList", JobModelOBj.PartsListData);
        //    //return Json( JsonRequestBehavior.AllowGet);
        //}

        //public PartialViewResult GetAssembliesLbourDetails(string Assemblies_Name)
        //{
        //   // TempData["selectedASname"] = Assemblies_Name;
        //    ViewBag.AssembliesName = Assemblies_Name;
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
        //    Job_MasterInfo JobModelOBj = new Job_MasterInfo();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    response = AssembliesResponse.GetAssembliesListDetails(Assemblies_Name, result.Client_ID);
        //    var assemliesData = response.Data;
        //    List<AssembliesLabourDetailsList> objLaborSumCal = new List<AssembliesLabourDetailsList>();
        //    foreach (var objLaborCalculate in assemliesData.LabourListData)
        //    {
        //        objLaborCalculate.Expected_Total = (objLaborCalculate.Cost + objLaborCalculate.Burden) * objLaborCalculate.Actual_Hours;
        //        objLaborCalculate.Estimated_Total = objLaborCalculate.Resale_Cost * objLaborCalculate.Estimated_Hour;
        //        objLaborCalculate.Actual_Total = objLaborCalculate.Resale_Cost * objLaborCalculate.Actual_Hours;
        //        objLaborSumCal.Add(new AssembliesLabourDetailsList { Laborer_Name = objLaborCalculate.Laborer_Name, Cost = objLaborCalculate.Cost, Resale_Cost = objLaborCalculate.Resale_Cost, Estimated_Hour = objLaborCalculate.Estimated_Hour, Actual_Hours = objLaborCalculate.Actual_Hours, Expected_Total = objLaborCalculate.Expected_Total, Estimated_Total = objLaborCalculate.Estimated_Total, Actual_Total = objLaborCalculate.Actual_Total });
        //    }
        //    AssembliesLabourDetailsList objcal = new AssembliesLabourDetailsList();
        //    Labor_GrandTotal laborGrand = new Labor_GrandTotal();

        //    foreach (AssembliesLabourDetailsList grandTotal in objLaborSumCal)
        //    {
        //        laborGrand.ExpGrandTotal += grandTotal.Expected_Total;
        //        laborGrand.EstGrandTotal += grandTotal.Estimated_Total;
        //        laborGrand.ActGrandTotal += grandTotal.Actual_Total;
        //    }
        //    laborGrandTotalList.Add(new Labor_GrandTotal { ExpGrandTotal = laborGrand.ExpGrandTotal, EstGrandTotal = laborGrand.EstGrandTotal, ActGrandTotal = laborGrand.ActGrandTotal });
        //    ViewData["GrandLaborTotalList"] = laborGrandTotalList.ToList();
        //    List<Parts_GrandTotal> partGrandTotalList = TempData["PartsGrandTotalList"] as List<Parts_GrandTotal> ?? new List<Parts_GrandTotal>();
        //    foreach (var partsSubTotaList in partGrandTotalList)
        //    {
        //        partsListEstTotal = partsSubTotaList.EstGrandTotal;
        //        partsListActTotal = partsSubTotaList.ActGrandTotal;
        //        partsListExpTotal = partsSubTotaList.ExpGrandTotal;
        //    }
        //    foreach (var laborSubTotalList in laborGrandTotalList)
        //    {
        //        laborListEstTotal = laborSubTotalList.EstGrandTotal;
        //        laborListExpTotal = laborSubTotalList.ExpGrandTotal;
        //        laborListActTotal = laborSubTotalList.ActGrandTotal;
        //    }
        //    var subTotal = partsListEstTotal + laborListEstTotal;
        //    var expenseTotal = partsListExpTotal + laborListExpTotal;
        //    var actualTotal = partsListActTotal + laborListActTotal;
        //    ViewData["subTotal"] = subTotal;
        //    ViewData["expenseTotal"] = expenseTotal;
        //    ViewData["actualTotal"] = actualTotal;
        //    JobModelOBj.PartsListData = assemliesData.PartsListData;
        //    JobModelOBj.LabourListData = assemliesData.LabourListData;
        //    //var jsonResult = Json(response.Data, JsonRequestBehavior.AllowGet);
        //    // jsonResult.MaxJsonLength = int.MaxValue;
        //    TempData["ASPartsListData"] = null;
        //    TempData["ASLaborListData"] = null;
        //    TempData["MyASparts"] = null;
        //    TempData["MyAsLabourList"] = null;
        //    return PartialView("_AssembliesLabourDetailsList", JobModelOBj.LabourListData);
        //    //return Json( JsonRequestBehavior.AllowGet);
        //}
        ////Here Add New Parts in Existing Assemblies Details
        //[HttpGet]
        //public ActionResult AddNewPartsinAssembliesDetails(string ASPartsgriddata, string AsLabourDridata, string LegalGridData, string AsName, string JobID, string JobStatus, string JobDescription, string ClientName, string OtherClientName, string ClientContactPerson, string ClientAddress, string ClientAddress2, string ClientCity, string ClientState, string ClientZipCode, string ClientFax, string ClientEmail, string ClientPhone, string ClientMobile, string PreviousLocation, string OtherPreviousLocation, string WorkAddress, string WorkAddress2, string WorkCity, string WorkState, string DirectionsTo, string DoingWhat, string MileageEstimated, string MileageActualCost, string MileageCost, string MileExpTotal, string MileagExpTotal, string MileEstTotal, string MileageActTotal, string EstimateOverExpTotal, string EstimateProfitPercentage, string EstimateSubTotal, string EstimateExpTotal, string EstimateProfitTotal, string EstimateOverride, string JobExpensesTotal, string JobEstimateTotal, string JobActualTotal, string JobProfitLossTotal)
        //{
        //    string Assemblies_Name = TempData["selectedASname"].ToString();
        //    JobMasterTempObj.Assemblies_Name = AsName;
        //    JobMasterTempObj.Job_ID = Convert.ToInt32(JobID);
        //    JobMasterTempObj.Job_Status = JobStatus;
        //    JobMasterTempObj.Job_Description = JobDescription;
        //    JobMasterTempObj.Client_Name = ClientName;
        //    JobMasterTempObj.Other_ClientName = OtherClientName;
        //    JobMasterTempObj.Client_ContactPerson = ClientContactPerson;
        //    JobMasterTempObj.Client_Address = ClientAddress;
        //    JobMasterTempObj.Client_Address2 = ClientAddress2;
        //    JobMasterTempObj.Client_City = ClientCity;
        //    JobMasterTempObj.Client_State = ClientState;
        //    JobMasterTempObj.Client_ZipCode = ClientZipCode;
        //    JobMasterTempObj.Client_Fax = ClientFax;
        //    JobMasterTempObj.Client_Email = ClientEmail;
        //    JobMasterTempObj.Client_Phone = ClientPhone;
        //    JobMasterTempObj.Client_Mobile = ClientMobile;
        //    JobMasterTempObj.Previous_Location = PreviousLocation;
        //    JobMasterTempObj.Other_Previous_Location = OtherPreviousLocation;
        //    JobMasterTempObj.Work_Address = WorkAddress;
        //    JobMasterTempObj.Work_Address2 = WorkAddress2;
        //    JobMasterTempObj.Work_City = WorkCity;
        //    JobMasterTempObj.Work_State = WorkState;
        //    JobMasterTempObj.Directions_To = DirectionsTo;
        //    JobMasterTempObj.Doing_What = DoingWhat;
        //    //Mileage Table Values
        //    if (MileageEstimated != null && MileageEstimated != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_Estimated = Convert.ToDecimal(MileageEstimated);
        //    }

        //    if (MileageActualCost != null && MileageActualCost != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_Actual = Convert.ToDecimal(MileageActualCost);
        //    }

        //    if (MileageCost != null && MileageCost != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_Cost = Convert.ToDecimal(MileageCost);
        //    }

        //    if (MileagExpTotal != null && MileagExpTotal != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_ExpTotal = Convert.ToDecimal(MileagExpTotal);
        //    }

        //    if (MileEstTotal != null && MileEstTotal != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_EstTotal = Convert.ToDecimal(MileEstTotal);
        //    }

        //    if (MileageActTotal != null && MileageActTotal != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_ActTotal = Convert.ToDecimal(MileageActTotal);
        //    }

        //    //Estimate Values

        //    if (EstimateOverExpTotal != null && EstimateOverExpTotal != "")
        //    {
        //        JobMasterTempObj.EstimateValue_Overhead_ExpTotal = Convert.ToDecimal(EstimateOverExpTotal);
        //    }
        //    if (EstimateSubTotal != null && EstimateSubTotal != "")
        //    {
        //        JobMasterTempObj.EstimateValue_SubTotal = Convert.ToDecimal(EstimateSubTotal);
        //    }
        //    if (EstimateExpTotal != null && EstimateExpTotal != "")
        //    {
        //        JobMasterTempObj.EstimateValue_ExpTotal = Convert.ToDecimal(EstimateExpTotal);
        //    }
        //    if (EstimateProfitTotal != null && EstimateProfitTotal != "")
        //    {
        //        JobMasterTempObj.EstimateValue_Profit_EstTotal = Convert.ToDecimal(EstimateProfitTotal);
        //    }
        //    if (EstimateOverride != null && EstimateOverride != "")
        //    {
        //        JobMasterTempObj.Estimate_Override = Convert.ToDecimal(EstimateOverride);
        //    }

        //    //Job Total Values
        //    if (JobExpensesTotal != null && JobExpensesTotal != "")
        //    {
        //        JobMasterTempObj.Job_ExpenseTotal = Convert.ToDecimal(JobExpensesTotal);
        //    }
        //    if (JobEstimateTotal != null && JobEstimateTotal != "")
        //    {
        //        JobMasterTempObj.Job_EstimatedTotal = Convert.ToDecimal(JobEstimateTotal);
        //    }
        //    if (JobActualTotal != null && JobActualTotal != "")
        //    {
        //        JobMasterTempObj.Job_ActualTotal = Convert.ToDecimal(JobActualTotal);

        //    }
        //    if (JobProfitLossTotal != null && JobProfitLossTotal != "")
        //    {
        //        JobMasterTempObj.Profit_Loss_Total = Convert.ToDecimal(JobProfitLossTotal);
        //    }
               
               
        //    if (ASPartsgriddata != null)
        //    {
        //        var ASPartsData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(ASPartsgriddata);
        //        TempData["AsPartsListData"] = ASPartsData;
        //    }
        //    if (ASPartsgriddata != null)
        //    {
        //        var ASLabourData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(AsLabourDridata);
        //        TempData["ASLabourListData"] = ASLabourData;
        //    }
        //    if (LegalGridData != null)
        //    {
        //        var MyLegalGridData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LegalGridData);
        //        TempData["LegalGridData"] = MyLegalGridData;
        //    }
        //    ViewBag.assembliesName = TempData["Assemblies_Name"];
        //    PartsMasterBLL PartsResponse = new PartsMasterBLL();
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    ServiceResult<Assembly_MasterInfo> AssembliesDetails = new ServiceResult<Assembly_MasterInfo>();
        //    //Here Get Parts List From Assemblies_Parts Table
        //    AssembliesDetails = AssembliesResponse.GetAssembliesListDetails(Assemblies_Name, result.Client_ID);
        //    ServiceResultList<Parts_DetailsInfoList> PartsDetailsList = new ServiceResultList<Parts_DetailsInfoList>();
        //    //Here Get Parts List From Parts_Master Table
        //    PartsDetailsList = PartsResponse.GetMyPartsList(result.Client_ID);

        //    foreach (AssembliesParts_DetailsInfoList Assembliesparts in AssembliesDetails.Data.PartsListData)
        //    {
        //        bool CheckIsExist = PartsDetailsList.ListData.Any(m => m.Part_Number == Assembliesparts.Part_Number);
        //        if (CheckIsExist==true)
        //        {
        //            var ExistingParts = (from PartsItem in PartsDetailsList.ListData where PartsItem.Part_Number == Assembliesparts.Part_Number select PartsItem).FirstOrDefault();

        //            ExistingParts.isChekced = true;
        //        }

        //    }

        //    return View(PartsDetailsList.ListData);
        //}

        //[HttpPost]
        //[OutputCache(Duration = 30)]
        //public ActionResult AddPartsInAssembliesDetails(FormCollection form)
        //{

        //    //string SelectedCheckBx;
        //   var SelectedValue = form.GetValues("assignChkBx");
        //    PartsMasterBLL PartsResponse = new PartsMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse=new AssembliesMasterBLL();
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    List<AssembliesParts_DetailsInfoList> PartsList = new List<AssembliesParts_DetailsInfoList>();
        //    ServiceResultList<AssembliesParts_DetailsInfoList> response = new ServiceResultList<AssembliesParts_DetailsInfoList>();
        //    List<AssembliesParts_DetailsInfoList> partsListOBJ = new List<AssembliesParts_DetailsInfoList>();
        //    List<AssembliesLabourDetailsList> LabourlistOBJ = new List<AssembliesLabourDetailsList>();
        //    Job_MasterInfo JobModelOBj = new Job_MasterInfo();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);



        //    if (TempData["ASLabourListData"] != null)
        //    {
        //        var ls = (Dictionary<string, string[]>)TempData["ASLabourListData"];
        //        foreach (string[] str in ls.Values)
        //        {
        //            AssembliesLabourDetailsList laborobj = new AssembliesLabourDetailsList();
        //            laborobj.Laborer_Name = str[0];
        //            laborobj.Cost = Convert.ToDecimal(str[1]);
        //            laborobj.Burden = Convert.ToDecimal(str[2]);
        //            laborobj.Resale_Cost = Convert.ToDecimal(str[3]);
        //            laborobj.isChekced = Convert.ToBoolean(true);
        //            laborobj.Estimated_Hour = Convert.ToDecimal(str[4]);
        //            laborobj.Actual_Hours = Convert.ToDecimal(str[5]);
        //            if(str[6]=="")
        //            {
        //                str[6]="0.0";
        //            }
        //            laborobj.Expected_Total = Convert.ToDecimal(str[6]);
        //            if (str[7] == "")
        //            {
        //                str[7] = "0.0";
        //            }
        //            laborobj.Estimated_Total = Convert.ToDecimal(str[7]);
        //            if (str[8] == "")
        //            {
        //                str[8] = "0.0";
        //            }
        //            laborobj.Actual_Total =Convert.ToDecimal(str[8]);
        //            LabourlistOBJ.Add(new AssembliesLabourDetailsList
        //            {
        //                Laborer_Name = laborobj.Laborer_Name,
        //                Labor_Category = laborobj.Labor_Category,
        //                Cost = laborobj.Cost,
        //                Burden = laborobj.Burden,
        //                Resale_Cost = laborobj.Resale_Cost,
        //                Actual_Hours = laborobj.Actual_Hours,
        //                Estimated_Hour = laborobj.Estimated_Hour,
        //                Estimated_Total=laborobj.Estimated_Total,
        //                Expected_Total=laborobj.Expected_Total,
        //                Actual_Total=laborobj.Actual_Total,
        //                isChekced = Convert.ToBoolean(true)
        //            });
        //        }

        //    }
        //    TempData["MyAsLabourList"] = LabourlistOBJ;

        //    List<Legal_DetailsInfoList> LegalListOBJ = new List<Legal_DetailsInfoList>();
        //    if (TempData["LegalGridData"] != null)
        //    {
        //        // List<Legal_DetailsInfoList> LegalList = new List<Legal_DetailsInfoList>();
        //        var Legal = (Dictionary<string, string[]>)TempData["LegalGridData"];
        //        foreach (string[] LegalItem in Legal.Values)
        //        {
        //            Legal_DetailsInfoList LegalList = new Legal_DetailsInfoList();
        //            LegalList.Legal_ID = Convert.ToInt32(LegalItem[0]);
        //            LegalList.Legal_Detail = LegalItem[1];
        //            LegalListOBJ.Add(new Legal_DetailsInfoList
        //            {
        //                //Client_ID = obj.Client_ID,
        //                Legal_ID = LegalList.Legal_ID,
        //                Legal_Detail = LegalList.Legal_Detail,
        //                isChekced = Convert.ToBoolean(true)
        //            });

        //        }

        //    }
        //    TempData["MyLegalList"] = LegalListOBJ;

        //    if (TempData["AsPartsListData"] != null)
        //    {
        //        var ps = (Dictionary<string, string[]>)TempData["AsPartsListData"];
        //        foreach (string[] s in ps.Values)
        //        {
        //            AssembliesParts_DetailsInfoList obj = new AssembliesParts_DetailsInfoList();
        //            obj.Part_Number = s[0];
        //            obj.Part_Cost = Convert.ToDecimal(s[1]);
        //            obj.Resale_Cost = Convert.ToDecimal(s[2]);
        //            obj.Estimated_Qty = Convert.ToInt32(s[3]);
        //            obj.Actual_Qty = Convert.ToInt32(s[4]);
        //            obj.Expected_Total = Convert.ToDecimal(s[5]);
        //            obj.Estimated_Total = Convert.ToDecimal(s[6]);
        //            obj.Actual_Total = Convert.ToDecimal(s[7]);
        //            obj.isChekced = Convert.ToBoolean(true);
        //            partsListOBJ.Add(new AssembliesParts_DetailsInfoList
        //            {
        //                Part_Number = obj.Part_Number,
        //                Part_Category = obj.Part_Category,
        //                Part_Cost = obj.Part_Cost,
        //                Resale_Cost = obj.Resale_Cost,
        //                Purchased_From = obj.Purchased_From,
        //                Actual_Qty = obj.Actual_Qty,
        //                Estimated_Qty = obj.Estimated_Qty,
        //                Estimated_Total=obj.Estimated_Total,
        //                Expected_Total=obj.Expected_Total,
        //                Actual_Total=obj.Actual_Total,
        //                isChekced = Convert.ToBoolean(true)
        //            });
        //        }
        //    }

           


        //    if (SelectedValue != null)
        //    {
        //        foreach (var selectedPart in SelectedValue)
        //        {
        //            string[] splitData = selectedPart.Split(',');
        //            //In argument we are passing clientID and partId to db.
        //            response = AssembliesResponse.GetMyAssembliesPartsList(Convert.ToInt16(splitData[0]), splitData[1]);
        //            PartsList.Add(new AssembliesParts_DetailsInfoList
        //            {
        //                Part_Number = response.ListData[0].Part_Number,
        //                Part_Category = response.ListData[0].Part_Category,
        //                Part_Cost = response.ListData[0].Part_Cost,
        //                Resale_Cost = response.ListData[0].Resale_Cost,
        //               // Purchased_From = response.ListData[0].Purchased_From,
        //                Estimated_Qty = response.ListData[0].Estimated_Qty,
        //                Actual_Qty = response.ListData[0].Actual_Qty,
        //                isChekced = true
        //            });
        //        }
        //    }
        //    if (partsListOBJ.Count != 0)
        //    {
        //        foreach (var newitem in PartsList)
        //        {
        //            foreach (var PreviousselectedItem in partsListOBJ)
        //            {
        //                if (PreviousselectedItem.Part_Number.Trim() == newitem.Part_Number.Trim() && PreviousselectedItem.isChekced == newitem.isChekced)
        //                {
        //                    newitem.Part_Number = PreviousselectedItem.Part_Number;
        //                    newitem.Part_Cost = PreviousselectedItem.Part_Cost;
        //                    newitem.Resale_Cost = PreviousselectedItem.Resale_Cost;
        //                    newitem.Actual_Qty = PreviousselectedItem.Actual_Qty;
        //                    newitem.Estimated_Qty = PreviousselectedItem.Estimated_Qty;
        //                    newitem.Expected_Total=PreviousselectedItem.Expected_Total;
        //                    newitem.Actual_Total = PreviousselectedItem.Actual_Total;
        //                    newitem.Expected_Total = PreviousselectedItem.Expected_Total;
        //                    newitem.Estimated_Total = PreviousselectedItem.Estimated_Total;

        //                }

        //            }

        //        }
        //    }

        //    TempData["MyASparts"] = PartsList;

        //    JobModelOBj.Job_ID = JobMasterTempObj.Job_ID;
        //    JobModelOBj.Job_Status = JobMasterTempObj.Job_Status;
        //    JobModelOBj.Job_Description = JobMasterTempObj.Job_Description;
        //    JobModelOBj.Assemblies_Name = JobMasterTempObj.Assemblies_Name;
        //    JobModelOBj.Client_Name = JobMasterTempObj.Client_Name;
        //    JobModelOBj.Other_ClientName = JobMasterTempObj.Other_ClientName;
        //    JobModelOBj.Client_ContactPerson = JobMasterTempObj.Client_ContactPerson;
        //    JobModelOBj.Client_Address = JobMasterTempObj.Client_Address;
        //    JobModelOBj.Client_Address2 = JobMasterTempObj.Client_Address2;
        //    JobModelOBj.Client_City = JobMasterTempObj.Client_City;
        //    JobModelOBj.Client_State = JobMasterTempObj.Client_State;
        //    JobModelOBj.Client_ZipCode = JobMasterTempObj.Client_ZipCode;
        //    JobModelOBj.Client_Fax = JobMasterTempObj.Client_Fax;
        //    JobModelOBj.Client_Email = JobMasterTempObj.Client_Email;
        //    JobModelOBj.Client_Phone = JobMasterTempObj.Client_Phone;
        //    JobModelOBj.Client_Mobile = JobMasterTempObj.Client_Mobile;
        //    JobModelOBj.Previous_Location = JobMasterTempObj.Previous_Location;
        //    JobModelOBj.Other_Previous_Location = JobMasterTempObj.Other_Previous_Location;
        //    JobModelOBj.Work_Address = JobMasterTempObj.Work_Address;
        //    JobModelOBj.Work_Address2 = JobMasterTempObj.Work_Address2;
        //    JobModelOBj.Work_City = JobMasterTempObj.Work_City;
        //    JobModelOBj.Work_State = JobMasterTempObj.Work_State;
        //    JobModelOBj.Directions_To = JobMasterTempObj.Directions_To;
        //    JobModelOBj.Doing_What = JobMasterTempObj.Doing_What;
        //    //Mileage Table Values
        //    JobModelOBj.Job_Mileage_Estimated = JobMasterTempObj.Job_Mileage_Estimated;
        //    JobModelOBj.Job_Mileage_Actual = JobMasterTempObj.Job_Mileage_Actual;
        //    JobModelOBj.Job_Mileage_Cost = JobMasterTempObj.Job_Mileage_Cost;
        //    JobModelOBj.Job_Mileage_ExpTotal = JobMasterTempObj.Job_Mileage_ExpTotal;
        //    JobModelOBj.Job_Mileage_EstTotal = JobMasterTempObj.Job_Mileage_EstTotal;
        //    JobModelOBj.Job_Mileage_ActTotal = JobMasterTempObj.Job_Mileage_ActTotal;
        //    //Estimate Values
        //    JobModelOBj.EstimateValue_Overhead_ExpTotal = JobMasterTempObj.EstimateValue_Overhead_ExpTotal;
        //    JobModelOBj.EstimateValue_Profit_Percentage = JobMasterTempObj.EstimateValue_Profit_Percentage;
        //    JobModelOBj.EstimateValue_SubTotal = JobMasterTempObj.EstimateValue_SubTotal;
        //    JobModelOBj.EstimateValue_ExpTotal = JobMasterTempObj.EstimateValue_ExpTotal;
        //    JobModelOBj.EstimateValue_Profit_EstTotal = JobMasterTempObj.EstimateValue_Profit_EstTotal;
        //    JobModelOBj.Estimate_Override = JobMasterTempObj.Estimate_Override;
        //    //Job Total Values
        //    JobModelOBj.Job_ExpenseTotal = JobMasterTempObj.Job_ExpenseTotal;
        //    JobModelOBj.Job_EstimatedTotal = JobMasterTempObj.Job_EstimatedTotal;
        //    JobModelOBj.Job_ActualTotal = JobMasterTempObj.Job_ActualTotal;
        //    JobModelOBj.Profit_Loss_Total = JobMasterTempObj.Profit_Loss_Total;
        //    ViewBag.AsAddLabour = "Aslabour";
        //    ViewBag.AsAddParts = "AsPartsList";
        //    return View("AddJobDetails", JobModelOBj);
        //}

        //[HttpGet]
        //[OutputCache(Duration = 30)]
        //public ActionResult AddNewLabourinAssembliesDetails(string ASPartsgriddata, string AsLabourDridata, string LegalGridData, string AsName, string JobID, string JobStatus, string JobDescription, string ClientName, string OtherClientName, string ClientContactPerson, string ClientAddress, string ClientAddress2, string ClientCity, string ClientState, string ClientZipCode, string ClientFax, string ClientEmail, string ClientPhone, string ClientMobile, string PreviousLocation, string OtherPreviousLocation, string WorkAddress, string WorkAddress2, string WorkCity, string WorkState, string DirectionsTo, string DoingWhat, string MileageEstimated, string MileageActualCost, string MileageCost, string MileExpTotal, string MileagExpTotal, string MileEstTotal, string MileageActTotal, string EstimateOverExpTotal, string EstimateProfitPercentage, string EstimateSubTotal, string EstimateExpTotal, string EstimateProfitTotal, string EstimateOverride, string JobExpensesTotal, string JobEstimateTotal, string JobActualTotal, string JobProfitLossTotal)
        //{
        //    string Assemblies_Name = TempData["selectedASname"].ToString();
        //    JobMasterTempObj.Assemblies_Name = AsName;
        //    JobMasterTempObj.Job_ID = Convert.ToInt32(JobID);
        //    JobMasterTempObj.Job_Status = JobStatus;
        //    JobMasterTempObj.Job_Description = JobDescription;
        //    JobMasterTempObj.Client_Name = ClientName;
        //    JobMasterTempObj.Other_ClientName = OtherClientName;
        //    JobMasterTempObj.Client_ContactPerson = ClientContactPerson;
        //    JobMasterTempObj.Client_Address = ClientAddress;
        //    JobMasterTempObj.Client_Address2 = ClientAddress2;
        //    JobMasterTempObj.Client_City = ClientCity;
        //    JobMasterTempObj.Client_State = ClientState;
        //    JobMasterTempObj.Client_ZipCode = ClientZipCode;
        //    JobMasterTempObj.Client_Fax = ClientFax;
        //    JobMasterTempObj.Client_Email = ClientEmail;
        //    JobMasterTempObj.Client_Phone = ClientPhone;
        //    JobMasterTempObj.Client_Mobile = ClientMobile;
        //    JobMasterTempObj.Previous_Location = PreviousLocation;
        //    JobMasterTempObj.Other_Previous_Location = OtherPreviousLocation;
        //    JobMasterTempObj.Work_Address = WorkAddress;
        //    JobMasterTempObj.Work_Address2 = WorkAddress2;
        //    JobMasterTempObj.Work_City = WorkCity;
        //    JobMasterTempObj.Work_State = WorkState;
        //    JobMasterTempObj.Directions_To = DirectionsTo;
        //    JobMasterTempObj.Doing_What = DoingWhat;
        //    //Mileage Table Values
        //    if (MileageEstimated != null && MileageEstimated != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_Estimated = Convert.ToDecimal(MileageEstimated);
        //    }

        //    if (MileageActualCost != null && MileageActualCost != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_Actual = Convert.ToDecimal(MileageActualCost);
        //    }

        //    if (MileageCost != null && MileageCost != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_Cost = Convert.ToDecimal(MileageCost);
        //    }

        //    if (MileagExpTotal != null && MileagExpTotal != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_ExpTotal = Convert.ToDecimal(MileagExpTotal);
        //    }

        //    if (MileEstTotal != null && MileEstTotal != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_EstTotal = Convert.ToDecimal(MileEstTotal);
        //    }

        //    if (MileageActTotal != null && MileageActTotal != "")
        //    {
        //        JobMasterTempObj.Job_Mileage_ActTotal = Convert.ToDecimal(MileageActTotal);
        //    }

        //    //Estimate Values

        //    if (EstimateOverExpTotal != null && EstimateOverExpTotal != "")
        //    {
        //        JobMasterTempObj.EstimateValue_Overhead_ExpTotal = Convert.ToDecimal(EstimateOverExpTotal);
        //    }
        //    if (EstimateSubTotal != null && EstimateSubTotal != "")
        //    {
        //        JobMasterTempObj.EstimateValue_SubTotal = Convert.ToDecimal(EstimateSubTotal);
        //    }
        //    if (EstimateExpTotal != null && EstimateExpTotal != "")
        //    {
        //        JobMasterTempObj.EstimateValue_ExpTotal = Convert.ToDecimal(EstimateExpTotal);
        //    }
        //    if (EstimateProfitTotal != null && EstimateProfitTotal != "")
        //    {
        //        JobMasterTempObj.EstimateValue_Profit_EstTotal = Convert.ToDecimal(EstimateProfitTotal);
        //    }
        //    if (EstimateOverride != null && EstimateOverride != "")
        //    {
        //        JobMasterTempObj.Estimate_Override = Convert.ToDecimal(EstimateOverride);
        //    }

        //    //Job Total Values
        //    if (JobExpensesTotal != null && JobExpensesTotal != "")
        //    {
        //        JobMasterTempObj.Job_ExpenseTotal = Convert.ToDecimal(JobExpensesTotal);
        //    }
        //    if (JobEstimateTotal != null && JobEstimateTotal != "")
        //    {
        //        JobMasterTempObj.Job_EstimatedTotal = Convert.ToDecimal(JobEstimateTotal);
        //    }
        //    if (JobActualTotal != null && JobActualTotal != "")
        //    {
        //        JobMasterTempObj.Job_ActualTotal = Convert.ToDecimal(JobActualTotal);

        //    }
        //    if (JobProfitLossTotal != null && JobProfitLossTotal != "")
        //    {
        //        JobMasterTempObj.Profit_Loss_Total = Convert.ToDecimal(JobProfitLossTotal);
        //    }
               
        //  //  string Assemblies_Name = Convert.ToString(asname);
        //    if (ASPartsgriddata != null)
        //    {
        //        var ASPartsData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(ASPartsgriddata);
        //        TempData["AsPartsListData"] = ASPartsData;
        //    }
        //    if (ASPartsgriddata != null)
        //    {
        //        var ASLabourData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(AsLabourDridata);
        //        TempData["ASLabourListData"] = ASLabourData;
        //    }
        //    if (LegalGridData != null)
        //    {
        //        var MyLegalGridData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LegalGridData);
        //        TempData["LegalGridData"] = MyLegalGridData;
        //    }
        //    ViewBag.assembliesName = TempData["Assemblies_Name"];
        //    PartsMasterBLL PartsResponse = new PartsMasterBLL();
        //    LabourMasterBLL LabourResponse = new LabourMasterBLL();
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    ServiceResult<Assembly_MasterInfo> AssembliesDetails = new ServiceResult<Assembly_MasterInfo>();
        //    //Here Get Parts List From Assemblies_Parts Table
        //    AssembliesDetails = AssembliesResponse.GetAssembliesListDetails(Assemblies_Name, result.Client_ID);
        //    ServiceResultList<Labor_DetailsInfoList> LabourDetailsList = new ServiceResultList<Labor_DetailsInfoList>();
        //    //Here Get Parts List From Parts_Master Table
        //    LabourDetailsList = LabourResponse.GetMyLabourList(result.Client_ID);
           
        //    foreach (AssembliesLabourDetailsList AssembliesLabour in AssembliesDetails.Data.LabourListData)
        //    {
        //        bool CheckIsExist = LabourDetailsList.ListData.Any(m => m.Laborer_Name == AssembliesLabour.Laborer_Name);
        //        if (CheckIsExist == true)
        //        {
        //            var ExistingParts = (from LabourItem in LabourDetailsList.ListData where LabourItem.Laborer_Name == AssembliesLabour.Laborer_Name select LabourItem).FirstOrDefault();

        //            ExistingParts.isChekced = true;
        //        }

        //    }


        //    return View(LabourDetailsList.ListData);
        //}

        //[HttpPost]
        //[OutputCache(Duration = 30)]
        //public ActionResult AddLabourInAssembliesDetails(FormCollection form)
        //{
        //    //string SelectedCheckBx;
           
        //    PartsMasterBLL PartsResponse = new PartsMasterBLL();
        //    LabourMasterBLL LabourResponse = new LabourMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    List<AssembliesParts_DetailsInfoList> PartsList = new List<AssembliesParts_DetailsInfoList>();
        //    List<AssembliesLabourDetailsList> ASLabourList = new List<AssembliesLabourDetailsList>();
        //    ServiceResultList<AssembliesParts_DetailsInfoList> response = new ServiceResultList<AssembliesParts_DetailsInfoList>();
        //    ServiceResultList<AssembliesLabourDetailsList> LabourUpdateList = new ServiceResultList<AssembliesLabourDetailsList>();
        //    List<AssembliesParts_DetailsInfoList> partsListOBJ = new List<AssembliesParts_DetailsInfoList>();
        //    List<AssembliesLabourDetailsList> LabourlistOBJ = new List<AssembliesLabourDetailsList>();
        //    Job_MasterInfo JobModelOBj = new Job_MasterInfo();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);

        //    if (TempData["AsPartsListData"] != null)
        //    {
        //        var ps = (Dictionary<string, string[]>)TempData["AsPartsListData"];
        //        foreach (string[] s in ps.Values)
        //        {
        //            AssembliesParts_DetailsInfoList obj = new AssembliesParts_DetailsInfoList();
        //            obj.Part_Number = s[0];
        //            obj.Part_Cost = Convert.ToDecimal(s[1]);
        //            obj.Resale_Cost = Convert.ToDecimal(s[2]);
        //            obj.Estimated_Qty = Convert.ToInt32(s[3]);
        //            obj.Actual_Qty = Convert.ToInt32(s[4]);
        //            obj.Expected_Total = Convert.ToDecimal(s[5]);
        //            obj.Estimated_Total = Convert.ToDecimal(s[6]);
        //            obj.Actual_Total = Convert.ToDecimal(s[7]);
        //            obj.isChekced = Convert.ToBoolean(true);
        //            partsListOBJ.Add(new AssembliesParts_DetailsInfoList
        //            {
        //                Part_Number = obj.Part_Number,
        //                Part_Category = obj.Part_Category,
        //                Part_Cost = obj.Part_Cost,
        //                Resale_Cost = obj.Resale_Cost,
        //                Purchased_From = obj.Purchased_From,
        //                Actual_Qty = obj.Actual_Qty,
        //                Estimated_Qty = obj.Estimated_Qty,
        //                Estimated_Total=obj.Estimated_Total,
        //                Expected_Total=obj.Expected_Total,
        //                Actual_Total=obj.Actual_Total,
        //                isChekced = Convert.ToBoolean(true)
        //            });
        //        }
        //    }

        //    TempData["MyASparts"] = partsListOBJ;

        //    List<Legal_DetailsInfoList> LegalListOBJ = new List<Legal_DetailsInfoList>();
        //    if (TempData["LegalGridData"] != null)
        //    {
        //        // List<Legal_DetailsInfoList> LegalList = new List<Legal_DetailsInfoList>();
        //        var Legal = (Dictionary<string, string[]>)TempData["LegalGridData"];
        //        foreach (string[] LegalItem in Legal.Values)
        //        {
        //            Legal_DetailsInfoList LegalList = new Legal_DetailsInfoList();
        //            LegalList.Legal_ID = Convert.ToInt32(LegalItem[0]);
        //            LegalList.Legal_Detail = LegalItem[1];
        //            LegalListOBJ.Add(new Legal_DetailsInfoList
        //            {
        //                //Client_ID = obj.Client_ID,
        //                Legal_ID = LegalList.Legal_ID,
        //                Legal_Detail = LegalList.Legal_Detail,
        //                isChekced = Convert.ToBoolean(true)
        //            });

        //        }

        //    }
        //    TempData["MyLegalList"] = LegalListOBJ;

        //    if (TempData["ASLabourListData"] != null)
        //    {
        //        var ls = (Dictionary<string, string[]>)TempData["ASLabourListData"];


        //        foreach (string[] str in ls.Values)
        //        {
        //            AssembliesLabourDetailsList laborobj = new AssembliesLabourDetailsList();
        //            //obj.Client_ID = s[0];
        //            // labourobj.Client_ID = Convert.ToInt32(str[0]);
        //            laborobj.Laborer_Name = str[0];
        //            // labourobj.Labor_Category = str[2];
        //            laborobj.Cost = Convert.ToDecimal(str[1]);
        //            laborobj.Burden = Convert.ToDecimal(str[2]);
        //            laborobj.Resale_Cost = Convert.ToDecimal(str[3]);
        //            laborobj.isChekced = Convert.ToBoolean(true);
        //            laborobj.Estimated_Hour = Convert.ToDecimal(str[4]);
        //            laborobj.Actual_Hours = Convert.ToDecimal(str[5]);
        //            if (str[6] == "")
        //            {
        //                str[6] = "0.0";
        //            }
        //            laborobj.Expected_Total = Convert.ToDecimal(str[6]);
        //            if (str[7] == "")
        //            {
        //                str[7] = "0.0";
        //            }
        //            laborobj.Estimated_Total = Convert.ToDecimal(str[7]);
        //            if (str[8] == "")
        //            {
        //                str[8] = "0.0";
        //            }
        //            laborobj.Actual_Total = Convert.ToDecimal(str[8]);
        //            // Client_ID = response.ListData[i].Client_ID, Part_Number = response.ListData[i].Part_Number, Part_Category = response.ListData[i].Part_Category, Cost = response.ListData[i].Cost, Resale_Cost = response.ListData[i].Resale_Cost, Purchased_From = response.ListData[i].Purchased_From, isChekced = response.ListData[i].isChekced });
        //            LabourlistOBJ.Add(new AssembliesLabourDetailsList
        //            {
        //                // Client_ID = labourobj.Client_ID,
        //                Laborer_Name = laborobj.Laborer_Name,
        //                Labor_Category = laborobj.Labor_Category,
        //                Cost = laborobj.Cost,
        //                Burden = laborobj.Burden,
        //                Resale_Cost = laborobj.Resale_Cost,
        //                Actual_Hours = laborobj.Actual_Hours,
        //                Estimated_Hour = laborobj.Estimated_Hour,
        //                Expected_Total=laborobj.Expected_Total,
        //                Estimated_Total = laborobj.Estimated_Total,
        //                Actual_Total=laborobj.Actual_Total,
        //                isChekced = Convert.ToBoolean(true)
        //            });
        //        }

        //    }
        //    var SelectedValue = form.GetValues("assignChkBx");
        //    if (SelectedValue != null)
        //    {
        //        foreach (var selectedPart in SelectedValue)
        //        {
        //            string[] splitData = selectedPart.Split(',');
        //            //In argument we are passing clientID and partId to db.
        //            LabourUpdateList = AssembliesResponse.GetMyAssembliesLaborerList(Convert.ToInt16(splitData[0]), splitData[1]);
        //            ASLabourList.Add(new AssembliesLabourDetailsList
        //            {
        //                Client_ID = LabourUpdateList.ListData[0].Client_ID,
        //                Laborer_Name = LabourUpdateList.ListData[0].Laborer_Name,
        //                Labor_Category = LabourUpdateList.ListData[0].Labor_Category,
        //                Cost = LabourUpdateList.ListData[0].Cost,
        //                Burden = LabourUpdateList.ListData[0].Burden,
        //                Resale_Cost = LabourUpdateList.ListData[0].Resale_Cost,
        //                Estimated_Hour = LabourUpdateList.ListData[0].Estimated_Hour,
        //                Actual_Hours = LabourUpdateList.ListData[0].Actual_Hours, 
        //                isChekced = true });
        //        }
        //    }
        //    foreach (var newitem in ASLabourList)
        //    {
        //        foreach (var PreviousselectedItem in LabourlistOBJ)
        //        {
        //            if (PreviousselectedItem.Laborer_Name.Trim() == newitem.Laborer_Name.Trim() && PreviousselectedItem.isChekced == newitem.isChekced)
        //            {
        //                newitem.Laborer_Name = PreviousselectedItem.Laborer_Name;
        //                newitem.Cost = PreviousselectedItem.Cost;
        //                newitem.Resale_Cost = PreviousselectedItem.Resale_Cost;
        //                newitem.Actual_Hours = PreviousselectedItem.Actual_Hours;
        //                newitem.Estimated_Hour = PreviousselectedItem.Estimated_Hour;
        //            }

        //        }
        //    }

        //    TempData["MyAsLabourList"] = ASLabourList;
        //    JobModelOBj.Job_ID = JobMasterTempObj.Job_ID;
        //    JobModelOBj.Job_Status = JobMasterTempObj.Job_Status;
        //    JobModelOBj.Job_Description = JobMasterTempObj.Job_Description;
        //    JobModelOBj.Assemblies_Name = JobMasterTempObj.Assemblies_Name;
        //    JobModelOBj.Client_Name = JobMasterTempObj.Client_Name;
        //    JobModelOBj.Other_ClientName = JobMasterTempObj.Other_ClientName;
        //    JobModelOBj.Client_ContactPerson = JobMasterTempObj.Client_ContactPerson;
        //    JobModelOBj.Client_Address = JobMasterTempObj.Client_Address;
        //    JobModelOBj.Client_Address2 = JobMasterTempObj.Client_Address2;
        //    JobModelOBj.Client_City = JobMasterTempObj.Client_City;
        //    JobModelOBj.Client_State = JobMasterTempObj.Client_State;
        //    JobModelOBj.Client_ZipCode = JobMasterTempObj.Client_ZipCode;
        //    JobModelOBj.Client_Fax = JobMasterTempObj.Client_Fax;
        //    JobModelOBj.Client_Email = JobMasterTempObj.Client_Email;
        //    JobModelOBj.Client_Phone = JobMasterTempObj.Client_Phone;
        //    JobModelOBj.Client_Mobile = JobMasterTempObj.Client_Mobile;
        //    JobModelOBj.Previous_Location = JobMasterTempObj.Previous_Location;
        //    JobModelOBj.Other_Previous_Location = JobMasterTempObj.Other_Previous_Location;
        //    JobModelOBj.Work_Address = JobMasterTempObj.Work_Address;
        //    JobModelOBj.Work_Address2 = JobMasterTempObj.Work_Address2;
        //    JobModelOBj.Work_City = JobMasterTempObj.Work_City;
        //    JobModelOBj.Work_State = JobMasterTempObj.Work_State;
        //    JobModelOBj.Directions_To = JobMasterTempObj.Directions_To;
        //    JobModelOBj.Doing_What = JobMasterTempObj.Doing_What;
        //    //Mileage Table Values
        //    JobModelOBj.Job_Mileage_Estimated = JobMasterTempObj.Job_Mileage_Estimated;
        //    JobModelOBj.Job_Mileage_Actual = JobMasterTempObj.Job_Mileage_Actual;
        //    JobModelOBj.Job_Mileage_Cost = JobMasterTempObj.Job_Mileage_Cost;
        //    JobModelOBj.Job_Mileage_ExpTotal = JobMasterTempObj.Job_Mileage_ExpTotal;
        //    JobModelOBj.Job_Mileage_EstTotal = JobMasterTempObj.Job_Mileage_EstTotal;
        //    JobModelOBj.Job_Mileage_ActTotal = JobMasterTempObj.Job_Mileage_ActTotal;
        //    //Estimate Values
        //    JobModelOBj.EstimateValue_Overhead_ExpTotal = JobMasterTempObj.EstimateValue_Overhead_ExpTotal;
        //    JobModelOBj.EstimateValue_Profit_Percentage = JobMasterTempObj.EstimateValue_Profit_Percentage;
        //    JobModelOBj.EstimateValue_SubTotal = JobMasterTempObj.EstimateValue_SubTotal;
        //    JobModelOBj.EstimateValue_ExpTotal = JobMasterTempObj.EstimateValue_ExpTotal;
        //    JobModelOBj.EstimateValue_Profit_EstTotal = JobMasterTempObj.EstimateValue_Profit_EstTotal;
        //    JobModelOBj.Estimate_Override = JobMasterTempObj.Estimate_Override;
        //    //Job Total Values
        //    JobModelOBj.Job_ExpenseTotal = JobMasterTempObj.Job_ExpenseTotal;
        //    JobModelOBj.Job_EstimatedTotal = JobMasterTempObj.Job_EstimatedTotal;
        //    JobModelOBj.Job_ActualTotal = JobMasterTempObj.Job_ActualTotal;
        //    JobModelOBj.Profit_Loss_Total = JobMasterTempObj.Profit_Loss_Total;
        //    ViewBag.AsAddLabour = "Aslabour";
        //    ViewBag.AsAddParts = "AsPartsList";
        //    return View("AddJobDetails", JobModelOBj);
        //}


          [OutputCache(Duration = 30)]
        public ActionResult AddNewPartsInJob(string JobPartsGridData, string JobLabourGridData,string ASPartsGridData,string ASLabourGridData, string LegalGridData, string JobID, string JobStatus, string JobDescription, string ClientName, string OtherClientName, string ClientContactPerson, string ClientAddress, string ClientAddress2, string ClientCity, string ClientState, string ClientZipCode, string ClientFax, string ClientEmail, string ClientPhone, string ClientMobile, string PreviousLocation, string OtherPreviousLocation, string WorkAddress, string WorkAddress2, string WorkCity, string WorkState, string DirectionsTo, string DoingWhat, string MileageEstimated, string MileageActualCost, string MileageCost, string MileExpTotal, string MileagExpTotal, string MileEstTotal, string MileageActTotal, string EstimateOverExpTotal, string EstimateProfitPercentage, string EstimateSubTotal, string EstimateExpTotal, string EstimateProfitTotal, string EstimateOverride, string JobExpensesTotal, string JobEstimateTotal, string JobActualTotal, string JobProfitLossTotal)
        {

            if (JobID != "")
            {
                //JobMasterTempObj.Job_ID = Convert.ToInt32(JobID);
                JobMasterTempObj.Job_Status = JobStatus;
                JobMasterTempObj.Job_Description = JobDescription;
                JobMasterTempObj.Client_Name = ClientName;
                JobMasterTempObj.Other_ClientName = OtherClientName;
                JobMasterTempObj.Client_ContactPerson = ClientContactPerson;
                JobMasterTempObj.Client_Address = ClientAddress;
                JobMasterTempObj.Client_Address2 = ClientAddress2;
                JobMasterTempObj.Client_City = ClientCity;
                JobMasterTempObj.Client_State = ClientState;
                JobMasterTempObj.Client_ZipCode = ClientZipCode;
                JobMasterTempObj.Client_Fax = ClientFax;
                JobMasterTempObj.Client_Email = ClientEmail;
                JobMasterTempObj.Client_Phone = ClientPhone;
                JobMasterTempObj.Client_Mobile = ClientMobile;
                JobMasterTempObj.Previous_Location = PreviousLocation;
                JobMasterTempObj.Other_Previous_Location = OtherPreviousLocation;
                JobMasterTempObj.Work_Address = WorkAddress;
                JobMasterTempObj.Work_Address2 = WorkAddress2;
                JobMasterTempObj.Work_City = WorkCity;
                JobMasterTempObj.Work_State = WorkState;
                JobMasterTempObj.Directions_To = DirectionsTo;
                JobMasterTempObj.Doing_What = DoingWhat;
                //Mileage Table Values
                if (MileageEstimated != null && MileageEstimated != "")
                {
                    JobMasterTempObj.Job_Mileage_Estimated = Convert.ToDecimal(MileageEstimated);
                }

                if (MileageActualCost != null && MileageActualCost != "")
                {
                    JobMasterTempObj.Job_Mileage_Actual = Convert.ToDecimal(MileageActualCost);
                }

                if (MileageCost != null && MileageCost != "")
                {
                    JobMasterTempObj.Job_Mileage_Cost = Convert.ToDecimal(MileageCost);
                }

                if (MileagExpTotal != null && MileagExpTotal != "")
                {
                    JobMasterTempObj.Job_Mileage_ExpTotal = Convert.ToDecimal(MileagExpTotal);
                }

                if (MileEstTotal != null && MileEstTotal != "")
                {
                    JobMasterTempObj.Job_Mileage_EstTotal = Convert.ToDecimal(MileEstTotal);
                }

                if (MileageActTotal != null && MileageActTotal != "")
                {
                    JobMasterTempObj.Job_Mileage_ActTotal = Convert.ToDecimal(MileageActTotal);
                }

                //Estimate Values

                if (EstimateOverExpTotal != null && EstimateOverExpTotal != "")
                {
                    JobMasterTempObj.EstimateValue_Overhead_ExpTotal = Convert.ToDecimal(EstimateOverExpTotal);
                }
                if (EstimateSubTotal != null && EstimateSubTotal != "")
                {
                    JobMasterTempObj.EstimateValue_SubTotal = Convert.ToDecimal(EstimateSubTotal);
                }
                if (EstimateExpTotal != null && EstimateExpTotal != "")
                {
                    JobMasterTempObj.EstimateValue_ExpTotal = Convert.ToDecimal(EstimateExpTotal);
                }
                if (EstimateProfitTotal != null && EstimateProfitTotal != "")
                {
                    JobMasterTempObj.EstimateValue_Profit_EstTotal = Convert.ToDecimal(EstimateProfitTotal);
                }
                if (EstimateOverride != null && EstimateOverride != "")
                {
                    JobMasterTempObj.Estimate_Override = Convert.ToDecimal(EstimateOverride);
                }

                //Job Total Values
                if (JobExpensesTotal != null && JobExpensesTotal != "")
                {
                    JobMasterTempObj.Job_ExpenseTotal = Convert.ToDecimal(JobExpensesTotal);
                }
                if (JobEstimateTotal != null && JobEstimateTotal != "")
                {
                    JobMasterTempObj.Job_EstimatedTotal = Convert.ToDecimal(JobEstimateTotal);
                }
                if (JobActualTotal != null && JobActualTotal != "")
                {
                    JobMasterTempObj.Job_ActualTotal = Convert.ToDecimal(JobActualTotal);

                }
                if (JobProfitLossTotal != null && JobProfitLossTotal != "")
                {
                    JobMasterTempObj.Profit_Loss_Total = Convert.ToDecimal(JobProfitLossTotal);
                }




                if (JobPartsGridData != null)
                {
                    var JobPartsListData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(JobPartsGridData);
                    TempData["JobPartsListData"] = JobPartsListData;
                }
                if (JobLabourGridData != null)
                {
                    var JobLabourListData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(JobLabourGridData);
                    TempData["JobLabourListData"] = JobLabourListData;
                }
                if (LegalGridData != null)
                {
                    var MyLegalGridData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LegalGridData);
                    TempData["LegalGridData"] = MyLegalGridData;
                }
                if (ASPartsGridData != null)
                {
                    var ASPartsData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(ASPartsGridData);
                    TempData["AsPartsListData"] = ASPartsData;
                }
                if (ASLabourGridData != null)
                {
                    var ASLabourData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(ASLabourGridData);
                    TempData["ASLabourListData"] = ASLabourData;
                }
            }
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            List<Parts_DetailsInfoList> partsList = new List<Parts_DetailsInfoList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = PartsResponse.GetMyPartsList(result.Client_ID);
            List<Parts_DetailsInfoList> selectedParts = TempData["MyPartsInJob"] as List<Parts_DetailsInfoList> ?? new List<Parts_DetailsInfoList>();

            for (int i = 0; i < response.ListData.Count; i++)
            {
                if (selectedParts.Count > 0)
                {
                    foreach (Parts_DetailsInfoList list in selectedParts)
                    {
                        if (list.Part_Number.Trim() == response.ListData[i].Part_Number.Trim())
                        {
                            response.ListData[i].isChekced = true;
                        }
                    }
                }
                partsList.Add(new Parts_DetailsInfoList
                {
                    Client_ID = response.ListData[i].Client_ID,
                    Part_Number = response.ListData[i].Part_Number,
                    Part_Category = response.ListData[i].Part_Category,
                    Part_Cost = response.ListData[i].Cost,
                    Resale_Cost = response.ListData[i].Resale_Cost,
                    Purchased_From = response.ListData[i].Purchased_From,
                    isChekced = response.ListData[i].isChekced
                });
            }
            
            // return View(response.ListData);
            return View(partsList.ToList());

        }

        [HttpPost]
        [OutputCache(Duration = 30)]
        public ActionResult AddNewPartsListInJob(FormCollection Form,Job_MasterInfo model)
        {
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
           // List<Assembly_MasterInfoList> aslistdata = new List<Assembly_MasterInfoList>();
            List<Parts_DetailsInfoList> partsListOBJ = new List<Parts_DetailsInfoList>();
            List<Labor_DetailsInfoList> LabourlistOBJ = new List<Labor_DetailsInfoList>();
            List<Assembly_MasterInfo> asLaborListObj = new List<Assembly_MasterInfo>();
            List<Assembly_MasterInfo> asPartsListObj = new List<Assembly_MasterInfo>();

            if (TempData["AsPartsListData"] != null)
            {
                var ps = (Dictionary<string, string[]>)TempData["AsPartsListData"];
                foreach (string[] s in ps.Values)
                {
                    Assembly_MasterInfo obj = new Assembly_MasterInfo();
                    obj.Assemblies_Name = s[0];
                    obj.Assemblies_Category = s[1];
                    obj.Part_Number = s[2];
                    obj.Part_Cost = Convert.ToDecimal(s[3]);
                    obj.Part_Resale = Convert.ToDecimal(s[4]);
                    obj.Estimated_Qty = Convert.ToInt32(s[5]);
                    obj.Actual_Qty = Convert.ToInt32(s[6]);
                    obj.Expected_Total = Convert.ToDecimal(s[6]);
                    obj.Estimated_Total = Convert.ToDecimal(s[7]);
                    obj.Actual_Total = Convert.ToDecimal(s[8]);
                    obj.isChekced = Convert.ToBoolean(true);
                    asPartsListObj.Add(new Assembly_MasterInfo
                    {
                        Assemblies_Name = obj.Assemblies_Name,
                        Assemblies_Category = obj.Assemblies_Category,
                        Part_Number = obj.Part_Number,
                        Part_Cost = obj.Part_Cost,
                        Part_Resale = obj.Part_Resale,
                        Actual_Qty = obj.Actual_Qty,
                        Estimated_Qty = obj.Estimated_Qty,
                        Estimated_Total = obj.Estimated_Total,
                        Expected_Total = obj.Expected_Total,
                        Actual_Total = obj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                }
            }

            TempData["JobAssembliesParts"] = asPartsListObj;

            if (TempData["ASLabourListData"] != null)
            {
                var ls = (Dictionary<string, string[]>)TempData["ASLabourListData"];
                foreach (string[] str in ls.Values)
                {
                    Assembly_MasterInfo laborobj = new Assembly_MasterInfo();
                    laborobj.Assemblies_Name = str[0];
                    laborobj.Assemblies_Category = str[1];
                    laborobj.labor_cost = Convert.ToDecimal(str[2]);
                    laborobj.labor_ResaleCost = Convert.ToDecimal(str[3]);
                    if (str[4] == "")
                    {
                        str[4] = "0.0";
                    }
                    laborobj.Estimated_Hour = Convert.ToDecimal(str[4]);
                    if (str[5] == "")
                    {
                        str[5] = "0.0";
                    }
                    laborobj.Actual_Hours = Convert.ToDecimal(str[5]);

                    laborobj.isChekced = Convert.ToBoolean(true);

                    if (str[6] == "")
                    {
                        str[6] = "0.0";
                    }
                    laborobj.Expected_Total = Convert.ToDecimal(str[6]);
                    if (str[7] == "")
                    {
                        str[7] = "0.0";
                    }
                    laborobj.Estimated_Total = Convert.ToDecimal(str[7]);
                    if (str[8] == "")
                    {
                        str[8] = "0.0";
                    }
                    laborobj.Actual_Total = Convert.ToDecimal(str[8]);
                    asLaborListObj.Add(new Assembly_MasterInfo
                    {
                        // Laborer_Name = laborobj.Laborer_Name,
                        Assemblies_Name = laborobj.Assemblies_Name,
                        Assemblies_Category = laborobj.Assemblies_Category,
                        labor_cost = laborobj.labor_cost,
                        labor_ResaleCost = laborobj.labor_ResaleCost,
                        Actual_Hours = laborobj.Actual_Hours,
                        Estimated_Hour = laborobj.Estimated_Hour,
                        Estimated_Total = laborobj.Estimated_Total,
                        Expected_Total = laborobj.Expected_Total,
                        Actual_Total = laborobj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                }

            }
            TempData["JobAssembliesLabor"] = asLaborListObj;

            if (TempData["JobLabourListData"] != null)
            {
                var ls = (Dictionary<string, string[]>)TempData["JobLabourListData"];


                foreach (string[] str in ls.Values)
                {
                    Labor_DetailsInfoList laborobj = new Labor_DetailsInfoList();
                    laborobj.Laborer_Name = str[0];
                    laborobj.Cost = Convert.ToDecimal(str[1]);
                    laborobj.Burden = Convert.ToDecimal(str[2]);
                    laborobj.Resale_Cost = Convert.ToDecimal(str[3]);
                    laborobj.isChekced = Convert.ToBoolean(true);
                    laborobj.Estimated_Hour = Convert.ToDecimal(str[4]);
                    laborobj.Actual_Hours = Convert.ToDecimal(str[5]);
                    if (str[6] == "")
                    {
                        str[6] = "0.0";
                    }
                    laborobj.Expected_Total = Convert.ToDecimal(str[6]);
                    if (str[7] == "")
                    {
                        str[7] = "0.0";
                    }
                    laborobj.Estimated_Total = Convert.ToDecimal(str[7]);
                    if (str[8] == "")
                    {
                        str[8] = "0.0";
                    }
                    laborobj.Actual_Total = Convert.ToDecimal(str[8]);
                    LabourlistOBJ.Add(new Labor_DetailsInfoList
                    {
                        Laborer_Name = laborobj.Laborer_Name,
                        Labor_Category = laborobj.Labor_Category,
                        Cost = laborobj.Cost,
                        Burden = laborobj.Burden,
                        Resale_Cost = laborobj.Resale_Cost,
                        Actual_Hours = laborobj.Actual_Hours,
                        Estimated_Hour = laborobj.Estimated_Hour,
                        Estimated_Total=laborobj.Estimated_Total,
                        Expected_Total=laborobj.Expected_Total,
                        Actual_Total=laborobj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                }

            }
            TempData["MyLaborListInJob"] = LabourlistOBJ;


            List<Legal_DetailsInfoList> LegalListOBJ = new List<Legal_DetailsInfoList>();
            if (TempData["LegalGridData"] != null)
            {
                // List<Legal_DetailsInfoList> LegalList = new List<Legal_DetailsInfoList>();
                var Legal = (Dictionary<string, string[]>)TempData["LegalGridData"];
                foreach (string[] LegalItem in Legal.Values)
                {
                    Legal_DetailsInfoList LegalList = new Legal_DetailsInfoList();
                    LegalList.Legal_ID = Convert.ToInt32(LegalItem[0]);
                    LegalList.Legal_Detail = LegalItem[1];
                    LegalListOBJ.Add(new Legal_DetailsInfoList
                    {
                        //Client_ID = obj.Client_ID,
                        Legal_ID = LegalList.Legal_ID,
                        Legal_Detail = LegalList.Legal_Detail,
                        isChekced = Convert.ToBoolean(true)
                    });

                }

            }
            TempData["MyLegalList"] = LegalListOBJ;

            if (TempData["JobPartsListData"] != null)
            {
                var ps = (Dictionary<string, string[]>)TempData["JobPartsListData"];
                foreach (string[] s in ps.Values)
                {
                    Parts_DetailsInfoList obj = new Parts_DetailsInfoList();
                    obj.Part_Number = s[0];
                    obj.Description = s[1];
                    obj.Part_Cost = Convert.ToDecimal(s[2]);
                    obj.Resale_Cost = Convert.ToDecimal(s[3]);
                    obj.Estimated_Qty = Convert.ToInt32(s[4]);
                    obj.Actual_Qty = Convert.ToInt32(s[5]);
                    obj.Expected_Total = Convert.ToDecimal(s[6]);
                    obj.Estimated_Total = Convert.ToDecimal(s[7]);
                    obj.Actual_Total = Convert.ToDecimal(s[8]);
                    obj.isChekced = Convert.ToBoolean(true);
                    partsListOBJ.Add(new Parts_DetailsInfoList
                     {   Part_Number = obj.Part_Number,
                         Description=obj.Description,
                        Part_Category = obj.Part_Category,
                        Part_Cost = obj.Part_Cost,
                        Resale_Cost = obj.Resale_Cost,
                        Purchased_From = obj.Purchased_From,
                        Actual_Qty = obj.Actual_Qty,
                        Estimated_Qty = obj.Estimated_Qty,
                        Estimated_Total = obj.Estimated_Total,
                        Expected_Total = obj.Expected_Total,
                        Actual_Total = obj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                }
            }




            var selectedValue = Form.GetValues("assignChkBx");
            if (selectedValue != null)
            {
                foreach (var selectedPart in selectedValue)
                {
                    string[] splitData = selectedPart.Split(',');
                    //In argument we are passing clientID and partId to db.
                    response = PartsResponse.GetJobPartsList(Convert.ToInt16(splitData[0]), splitData[1]);
                    parts.Add(new Parts_DetailsInfoList
                    {
                        Part_Number = response.ListData[0].Part_Number,
                        Description=response.ListData[0].Description,
                        Part_Category = response.ListData[0].Part_Category,
                        Part_Cost = response.ListData[0].Cost,
                        Resale_Cost = response.ListData[0].Resale_Cost,
                        Purchased_From = response.ListData[0].Purchased_From,
                        Estimated_Qty = response.ListData[0].Estimated_Qty,
                        Actual_Qty = response.ListData[0].Actual_Qty,
                        Expected_Total = response.ListData[0].Expected_Total,
                        Estimated_Total = response.ListData[0].Estimated_Total,
                        Actual_Total = response.ListData[0].Actual_Total,
                        isChekced = true
                    });
                }
                model.NewPartsListInJob = parts;

                if (partsListOBJ.Count != 0)
                {
                    foreach (var newitem in parts)
                    {
                        foreach (var PreviousselectedItem in partsListOBJ)
                        {
                            if (PreviousselectedItem.Part_Number.Trim() == newitem.Part_Number.Trim() && PreviousselectedItem.isChekced == newitem.isChekced)
                            {
                                newitem.Part_Number = PreviousselectedItem.Part_Number;
                                newitem.Description = PreviousselectedItem.Description;
                                newitem.Part_Cost = PreviousselectedItem.Cost;
                                newitem.Resale_Cost = PreviousselectedItem.Resale_Cost;
                                newitem.Actual_Qty = PreviousselectedItem.Actual_Qty;
                                newitem.Estimated_Qty = PreviousselectedItem.Estimated_Qty;
                                newitem.Estimated_Total=PreviousselectedItem.Estimated_Total;
                                newitem.Expected_Total = PreviousselectedItem.Expected_Total;
                                newitem.Actual_Total = PreviousselectedItem.Actual_Total;
                                //isChekced = Convert.ToBoolean(true)

                            }

                        }


                    }
                }
                
                TempData["MyPartsInJob"] = parts;
                TempData.Peek("MyPartsInJob");
            }

            model.Job_ID = JobMasterTempObj.Job_ID;
            model.Job_Status = JobMasterTempObj.Job_Status;
            model.Job_Description = JobMasterTempObj.Job_Description;

            model.Client_Name = JobMasterTempObj.Client_Name;
            model.Other_ClientName = JobMasterTempObj.Other_ClientName;
            model.Client_ContactPerson = JobMasterTempObj.Client_ContactPerson;
            model.Client_Address = JobMasterTempObj.Client_Address;
            model.Client_Address2 = JobMasterTempObj.Client_Address2;
            model.Client_City = JobMasterTempObj.Client_City;
            model.Client_State = JobMasterTempObj.Client_State;
            model.Client_ZipCode = JobMasterTempObj.Client_ZipCode;
            model.Client_Fax = JobMasterTempObj.Client_Fax;
            model.Client_Email = JobMasterTempObj.Client_Email;
            model.Client_Phone = JobMasterTempObj.Client_Phone;
            model.Client_Mobile = JobMasterTempObj.Client_Mobile;
            model.Previous_Location = JobMasterTempObj.Previous_Location;
            model.Other_Previous_Location = JobMasterTempObj.Other_Previous_Location;
            model.Work_Address = JobMasterTempObj.Work_Address;
            model.Work_Address2 = JobMasterTempObj.Work_Address2;
            model.Work_City = JobMasterTempObj.Work_City;
            model.Work_State = JobMasterTempObj.Work_State;
            model.Directions_To = JobMasterTempObj.Directions_To;
            model.Doing_What = JobMasterTempObj.Doing_What;
            //Mileage Table Values
            model.Job_Mileage_Estimated = JobMasterTempObj.Job_Mileage_Estimated;
            model.Job_Mileage_Actual = JobMasterTempObj.Job_Mileage_Actual;
            model.Job_Mileage_Cost = JobMasterTempObj.Job_Mileage_Cost;
            model.Job_Mileage_ExpTotal = JobMasterTempObj.Job_Mileage_ExpTotal;
            model.Job_Mileage_EstTotal = JobMasterTempObj.Job_Mileage_EstTotal;
            model.Job_Mileage_ActTotal = JobMasterTempObj.Job_Mileage_ActTotal;
            //Estimate Values
            model.EstimateValue_Overhead_ExpTotal = JobMasterTempObj.EstimateValue_Overhead_ExpTotal;
            model.EstimateValue_Profit_Percentage = JobMasterTempObj.EstimateValue_Profit_Percentage;
            model.EstimateValue_SubTotal = JobMasterTempObj.EstimateValue_SubTotal;
            model.EstimateValue_ExpTotal = JobMasterTempObj.EstimateValue_ExpTotal;
            model.EstimateValue_Profit_EstTotal = JobMasterTempObj.EstimateValue_Profit_EstTotal;
            model.Estimate_Override = JobMasterTempObj.Estimate_Override;
            //Job Total Values
            model.Job_ExpenseTotal = JobMasterTempObj.Job_ExpenseTotal;
            model.Job_EstimatedTotal = JobMasterTempObj.Job_EstimatedTotal;
            model.Job_ActualTotal = JobMasterTempObj.Job_ActualTotal;
            model.Profit_Loss_Total = JobMasterTempObj.Profit_Loss_Total;
            ViewBag.JobParts = "JobParts";
            ViewBag.JobLabour = "JobLabour";
            return View("AddJobDetails", model);
        }
          [OutputCache(Duration = 30)]
        public ActionResult AddNewLabourInJob(string JobPartsGridData, string JobLabourGridData, string ASPartsGridData, string ASLabourGridData, string LegalGridData, string JobID, string JobStatus, string JobDescription, string ClientName, string OtherClientName, string ClientContactPerson, string ClientAddress, string ClientAddress2, string ClientCity, string ClientState, string ClientZipCode, string ClientFax, string ClientEmail, string ClientPhone, string ClientMobile, string PreviousLocation, string OtherPreviousLocation, string WorkAddress, string WorkAddress2, string WorkCity, string WorkState, string DirectionsTo, string DoingWhat, string MileageEstimated, string MileageActualCost, string MileageCost, string MileExpTotal, string MileagExpTotal, string MileEstTotal, string MileageActTotal, string EstimateOverExpTotal, string EstimateProfitPercentage, string EstimateSubTotal, string EstimateExpTotal, string EstimateProfitTotal, string EstimateOverride, string JobExpensesTotal, string JobEstimateTotal, string JobActualTotal, string JobProfitLossTotal)
        {
            if (JobPartsGridData != null)
            {
                var JobPartsListData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(JobPartsGridData);
                TempData["JobPartsListData"] = JobPartsListData;
            }
            if (JobLabourGridData != null)
            {
                var JobLabourListData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(JobLabourGridData);
                TempData["JobLabourListData"] = JobLabourListData;
            }
            if (LegalGridData != null)
            {
                var MyLegalGridData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LegalGridData);
                TempData["LegalGridData"] = MyLegalGridData;
            }
            if (ASPartsGridData != null)
            {
                var ASPartsData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(ASPartsGridData);
                TempData["AsPartsListData"] = ASPartsData;
            }
            if (ASLabourGridData != null)
            {
                var ASLabourData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(ASLabourGridData);
                TempData["ASLabourListData"] = ASLabourData;
            }
            JobMasterTempObj.Job_ID = Convert.ToInt32(JobID);
            JobMasterTempObj.Job_Status = JobStatus;
            JobMasterTempObj.Job_Description = JobDescription;
            JobMasterTempObj.Client_Name = ClientName;
            JobMasterTempObj.Other_ClientName = OtherClientName;
            JobMasterTempObj.Client_ContactPerson = ClientContactPerson;
            JobMasterTempObj.Client_Address = ClientAddress;
            JobMasterTempObj.Client_Address2 = ClientAddress2;
            JobMasterTempObj.Client_City = ClientCity;
            JobMasterTempObj.Client_State = ClientState;
            JobMasterTempObj.Client_ZipCode = ClientZipCode;
            JobMasterTempObj.Client_Fax = ClientFax;
            JobMasterTempObj.Client_Email = ClientEmail;
            JobMasterTempObj.Client_Phone = ClientPhone;
            JobMasterTempObj.Client_Mobile = ClientMobile;
            JobMasterTempObj.Previous_Location = PreviousLocation;
            JobMasterTempObj.Other_Previous_Location = OtherPreviousLocation;
            JobMasterTempObj.Work_Address = WorkAddress;
            JobMasterTempObj.Work_Address2 = WorkAddress2;
            JobMasterTempObj.Work_City = WorkCity;
            JobMasterTempObj.Work_State = WorkState;
            JobMasterTempObj.Directions_To = DirectionsTo;
            JobMasterTempObj.Doing_What = DoingWhat;
            //Mileage Table Values
            if (MileageEstimated != null && MileageEstimated != "")
            {
                JobMasterTempObj.Job_Mileage_Estimated = Convert.ToDecimal(MileageEstimated);
            }

            if (MileageActualCost != null && MileageActualCost != "")
            {
                JobMasterTempObj.Job_Mileage_Actual = Convert.ToDecimal(MileageActualCost);
            }

            if (MileageCost != null && MileageCost != "")
            {
                JobMasterTempObj.Job_Mileage_Cost = Convert.ToDecimal(MileageCost);
            }

            if (MileagExpTotal != null && MileagExpTotal != "")
            {
                JobMasterTempObj.Job_Mileage_ExpTotal = Convert.ToDecimal(MileagExpTotal);
            }

            if (MileEstTotal != null && MileEstTotal != "")
            {
                JobMasterTempObj.Job_Mileage_EstTotal = Convert.ToDecimal(MileEstTotal);
            }

            if (MileageActTotal != null && MileageActTotal != "")
            {
                JobMasterTempObj.Job_Mileage_ActTotal = Convert.ToDecimal(MileageActTotal);
            }

            //Estimate Values

            if (EstimateOverExpTotal != null && EstimateOverExpTotal != "")
            {
                JobMasterTempObj.EstimateValue_Overhead_ExpTotal = Convert.ToDecimal(EstimateOverExpTotal);
            }
            if (EstimateSubTotal != null && EstimateSubTotal != "")
            {
                JobMasterTempObj.EstimateValue_SubTotal = Convert.ToDecimal(EstimateSubTotal);
            }
            if (EstimateExpTotal != null && EstimateExpTotal != "")
            {
                JobMasterTempObj.EstimateValue_ExpTotal = Convert.ToDecimal(EstimateExpTotal);
            }
            if (EstimateProfitTotal != null && EstimateProfitTotal != "")
            {
                JobMasterTempObj.EstimateValue_Profit_EstTotal = Convert.ToDecimal(EstimateProfitTotal);
            }
            if (EstimateOverride != null && EstimateOverride != "")
            {
                JobMasterTempObj.Estimate_Override = Convert.ToDecimal(EstimateOverride);
            }

            //Job Total Values
            if (JobExpensesTotal != null && JobExpensesTotal != "")
            {
                JobMasterTempObj.Job_ExpenseTotal = Convert.ToDecimal(JobExpensesTotal);
            }
            if (JobEstimateTotal != null && JobEstimateTotal != "")
            {
                JobMasterTempObj.Job_EstimatedTotal = Convert.ToDecimal(JobEstimateTotal);
            }
            if (JobActualTotal != null && JobActualTotal != "")
            {
                JobMasterTempObj.Job_ActualTotal = Convert.ToDecimal(JobActualTotal);

            }
            if (JobProfitLossTotal != null && JobProfitLossTotal != "")
            {
                JobMasterTempObj.Profit_Loss_Total = Convert.ToDecimal(JobProfitLossTotal);
            }
               
            
            
            LabourMasterBLL LabourResponse = new LabourMasterBLL();
            ServiceResultList<Labor_DetailsInfoList> response = new ServiceResultList<Labor_DetailsInfoList>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            List<Parts_DetailsInfoList> partsListOBJ = new List<Parts_DetailsInfoList>();
            List<Labor_DetailsInfoList> LabourlistOBJ = new List<Labor_DetailsInfoList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = LabourResponse.GetMyLabourList(result.Client_ID);
           // TempData.Keep("MyLaborList");
            //TempData.Keep("MyParts");
            List<Labor_DetailsInfoList> selectedLaborList = TempData["MyLaborListInJob"] as List<Labor_DetailsInfoList> ?? new List<Labor_DetailsInfoList>();
            for (int i = 0; i < response.ListData.Count; i++)
            {
                if (selectedLaborList.Count > 0)
                {
                    foreach (var list in selectedLaborList)
                    {
                        if (list.Laborer_Name.Trim() == response.ListData[i].Laborer_Name.Trim())
                            response.ListData[i].isChekced = true;
                    }
                }
                LabourlistOBJ.Add(new Labor_DetailsInfoList
                {
                    Client_ID = response.ListData[i].Client_ID,
                    Laborer_Name = response.ListData[i].Laborer_Name,
                    Labor_Category = response.ListData[i].Labor_Category,
                    Cost = response.ListData[i].Cost,
                    Burden = response.ListData[i].Burden,
                    Resale_Cost = response.ListData[i].Resale_Cost,
                    //Estimated_Hour = response.ListData[i].Estimated_Hour,
                    //Actual_Hours = response.ListData[i].Actual_Hours,
                    //Expected_Total = response.ListData[i].Expected_Total,
                    //Estimated_Total = response.ListData[i].Estimated_Total,
                    //Actual_Total = response.ListData[i].Actual_Total
                    isChekced = response.ListData[i].isChekced
                });
            }
            return View(LabourlistOBJ.ToList());
        }

        [HttpPost]
        [OutputCache(Duration = 30)]
        public ActionResult AddNewLabourListInJob(FormCollection Form,Job_MasterInfo model)
        {
            LabourMasterBLL LabourResponse = new LabourMasterBLL();
            ServiceResultList<Labor_DetailsInfoList> response = new ServiceResultList<Labor_DetailsInfoList>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            List<Parts_DetailsInfoList> partsListOBJ = new List<Parts_DetailsInfoList>(); 
            List<Labor_DetailsInfoList> LabourlistOBJ = new List<Labor_DetailsInfoList>();
            List<Labor_DetailsInfoList> laborerList = new List<Labor_DetailsInfoList>();
            List<Assembly_MasterInfo> asLaborListObj = new List<Assembly_MasterInfo>();
            List<Assembly_MasterInfo> asPartsListObj = new List<Assembly_MasterInfo>();

            if (TempData["AsPartsListData"] != null)
            {
                var ps = (Dictionary<string, string[]>)TempData["AsPartsListData"];
                foreach (string[] s in ps.Values)
                {
                    Assembly_MasterInfo obj = new Assembly_MasterInfo();
                    obj.Assemblies_Name = s[0];
                    obj.Assemblies_Category = s[1];
                    obj.Part_Number=s[2];
                    obj.Part_Cost = Convert.ToDecimal(s[3]);
                    obj.Part_Resale = Convert.ToDecimal(s[4]);
                    obj.Estimated_Qty = Convert.ToInt32(s[5]);
                    obj.Actual_Qty = Convert.ToInt32(s[6]);
                    obj.Expected_Total = Convert.ToDecimal(s[6]);
                    obj.Estimated_Total = Convert.ToDecimal(s[7]);
                    obj.Actual_Total = Convert.ToDecimal(s[8]);
                    obj.isChekced = Convert.ToBoolean(true);
                    asPartsListObj.Add(new Assembly_MasterInfo
                    {
                        Assemblies_Name=obj.Assemblies_Name,
                        Assemblies_Category=obj.Assemblies_Category,
                        Part_Number = obj.Part_Number,
                        Part_Cost = obj.Part_Cost,
                        Part_Resale = obj.Part_Resale,
                        Actual_Qty = obj.Actual_Qty,
                        Estimated_Qty = obj.Estimated_Qty,
                        Estimated_Total = obj.Estimated_Total,
                        Expected_Total = obj.Expected_Total,
                        Actual_Total = obj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                }
            }

            TempData["JobAssembliesParts"] = asPartsListObj;

            if (TempData["ASLabourListData"] != null)
            {
                var ls = (Dictionary<string, string[]>)TempData["ASLabourListData"];
                foreach (string[] str in ls.Values)
                {
                    Assembly_MasterInfo laborobj = new Assembly_MasterInfo();
                    laborobj.Assemblies_Name = str[0];
                    laborobj.Assemblies_Category = str[1];
                    laborobj.labor_cost = Convert.ToDecimal(str[2]);
                    laborobj.labor_ResaleCost = Convert.ToDecimal(str[3]);
                    if (str[4]=="")
                    {
                        str[4] = "0.0";
                    }
                    laborobj.Estimated_Hour = Convert.ToDecimal(str[4]);
                    if (str[5] == "")
                    {
                        str[5] = "0.0";
                    }
                    laborobj.Actual_Hours = Convert.ToDecimal(str[5]);
                   
                    laborobj.isChekced = Convert.ToBoolean(true);
                  
                    if (str[6] == "")
                    {
                        str[6] = "0.0";
                    }
                    laborobj.Expected_Total = Convert.ToDecimal(str[6]);
                    if (str[7] == "")
                    {
                        str[7] = "0.0";
                    }
                    laborobj.Estimated_Total = Convert.ToDecimal(str[7]);
                    if (str[8] == "")
                    {
                        str[8] = "0.0";
                    }
                    laborobj.Actual_Total = Convert.ToDecimal(str[8]);
                    asLaborListObj.Add(new Assembly_MasterInfo
                    {
                       // Laborer_Name = laborobj.Laborer_Name,
                       Assemblies_Name=laborobj.Assemblies_Name,
                      Assemblies_Category=laborobj.Assemblies_Category,
                       labor_cost = laborobj.labor_cost,
                       labor_ResaleCost = laborobj.labor_ResaleCost,
                        Actual_Hours = laborobj.Actual_Hours,
                        Estimated_Hour = laborobj.Estimated_Hour,
                        Estimated_Total = laborobj.Estimated_Total,
                        Expected_Total = laborobj.Expected_Total,
                        Actual_Total = laborobj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                }

            }
            TempData["JobAssembliesLabor"] = asLaborListObj;
            if (TempData["JobPartsListData"] != null)
            {
                var ps = (Dictionary<string, string[]>)TempData["JobPartsListData"];
                foreach (string[] s in ps.Values)
                {
                    Parts_DetailsInfoList obj = new Parts_DetailsInfoList();
                   
                   obj.Part_Number = s[0];
                   obj.Description = s[1];
                    obj.Part_Cost = Convert.ToDecimal(s[2]);
                    obj.Resale_Cost = Convert.ToDecimal(s[3]);
                    obj.Estimated_Qty = Convert.ToInt32(s[4]);
                    obj.Actual_Qty = Convert.ToInt32(s[5]);
                    obj.Expected_Total = Convert.ToDecimal(s[6]);
                    obj.Estimated_Total = Convert.ToDecimal(s[7]);
                    obj.Actual_Total = Convert.ToDecimal(s[8]);
                    obj.isChekced = Convert.ToBoolean(true);
                    partsListOBJ.Add(new Parts_DetailsInfoList
                    {
                        Part_Number = obj.Part_Number,
                        Description=obj.Description,
                        Part_Category = obj.Part_Category,
                        Part_Cost = obj.Part_Cost,
                        Resale_Cost = obj.Resale_Cost,
                        Purchased_From = obj.Purchased_From,
                        Actual_Qty = obj.Actual_Qty,
                        Estimated_Qty = obj.Estimated_Qty,
                        Estimated_Total=obj.Estimated_Total,
                        Expected_Total=obj.Expected_Total,
                        Actual_Total=obj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                }
                
            }

            TempData["MyPartsInJob"] = partsListOBJ;
            if (TempData["JobLabourListData"] != null)
            {
                var ls = (Dictionary<string, string[]>)TempData["JobLabourListData"];


                foreach (string[] str in ls.Values)
                {
                    Labor_DetailsInfoList laborobj = new Labor_DetailsInfoList();
                    laborobj.Laborer_Name = str[0];
                    laborobj.Cost = Convert.ToDecimal(str[1]);
                    laborobj.Burden = Convert.ToDecimal(str[2]);
                    laborobj.Resale_Cost = Convert.ToDecimal(str[3]);
                    laborobj.isChekced = Convert.ToBoolean(true);
                    laborobj.Estimated_Hour = Convert.ToDecimal(str[4]);
                    laborobj.Actual_Hours = Convert.ToDecimal(str[5]);
                    if (str[6] == "")
                    {
                        str[6] = "0.0";
                    }
                    laborobj.Expected_Total = Convert.ToDecimal(str[6]);
                    if (str[7] == "")
                    {
                        str[7] = "0.0";
                    }
                    laborobj.Estimated_Total = Convert.ToDecimal(str[7]);
                    if (str[8] == "")
                    {
                        str[8] = "0.0";
                    }
                    laborobj.Actual_Total = Convert.ToDecimal(str[8]);
                    LabourlistOBJ.Add(new Labor_DetailsInfoList
                    {
                        Laborer_Name = laborobj.Laborer_Name,
                        Labor_Category = laborobj.Labor_Category,
                        Cost = laborobj.Cost,
                        Burden = laborobj.Burden,
                        Resale_Cost = laborobj.Resale_Cost,
                        Actual_Hours = laborobj.Actual_Hours,
                        Estimated_Hour = laborobj.Estimated_Hour,
                        Estimated_Total=laborobj.Estimated_Total,
                        Expected_Total=laborobj.Expected_Total,
                        Actual_Total=laborobj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                
                }

            }
            List<Legal_DetailsInfoList> LegalListOBJ = new List<Legal_DetailsInfoList>();
            if (TempData["LegalGridData"] != null)
            {
                // List<Legal_DetailsInfoList> LegalList = new List<Legal_DetailsInfoList>();
                var Legal = (Dictionary<string, string[]>)TempData["LegalGridData"];
                foreach (string[] LegalItem in Legal.Values)
                {
                    Legal_DetailsInfoList LegalList = new Legal_DetailsInfoList();
                    LegalList.Legal_ID = Convert.ToInt32(LegalItem[0]);
                    LegalList.Legal_Detail = LegalItem[1];
                    LegalListOBJ.Add(new Legal_DetailsInfoList
                    {
                        //Client_ID = obj.Client_ID,
                        Legal_ID = LegalList.Legal_ID,
                        Legal_Detail = LegalList.Legal_Detail,
                        isChekced = Convert.ToBoolean(true)
                    });

                }

            }
            TempData["MyLegalList"] = LegalListOBJ;
            var selectedLaborer = Form.GetValues("assignChkBx");
            if (selectedLaborer != null)
            {
                foreach (var selectedList in selectedLaborer)
                {
                    string[] splitData = selectedList.Split(',');
                    //In argument we are passing clientID and laborerName to db.
                    response = LabourResponse.GetMyJobLaborerList(Convert.ToInt16(splitData[0]), splitData[1]);
                    laborerList.Add(new Labor_DetailsInfoList 
                    { 
                        Client_ID = response.ListData[0].Client_ID, 
                        Laborer_Name = response.ListData[0].Laborer_Name,
                        Labor_Category = response.ListData[0].Labor_Category, 
                        Cost = response.ListData[0].Cost, 
                        Burden = response.ListData[0].Burden,
                        Resale_Cost = response.ListData[0].Resale_Cost, 
                       // Estimated_Hour = response.ListData[0].Estimated_Hour,
                       // Actual_Hours = response.ListData[0].Actual_Hours,
                        //Estimated_Total = response.ListData[0].Estimated_Total,
                        // Expected_Total = response.ListData[0].Expected_Total,
                        // Actual_Total = response.ListData[0].Actual_Total,
                        isChekced = true });
                }

            }

            foreach (var newitem in laborerList)
            {
                foreach (var PreviousselectedItem in LabourlistOBJ)
                {
                    if (PreviousselectedItem.Laborer_Name.Trim() == newitem.Laborer_Name.Trim() && PreviousselectedItem.isChekced == newitem.isChekced)
                    {
                        newitem.Client_ID = PreviousselectedItem.Client_ID;
                        newitem.Laborer_Name = PreviousselectedItem.Laborer_Name;
                        newitem.Cost = PreviousselectedItem.Cost;
                        newitem.Resale_Cost = PreviousselectedItem.Resale_Cost;
                        newitem.Actual_Hours = PreviousselectedItem.Actual_Hours;
                        newitem.Estimated_Hour = PreviousselectedItem.Estimated_Hour;
                        newitem.Estimated_Total = PreviousselectedItem.Estimated_Total;
                        newitem.Expected_Total = PreviousselectedItem.Expected_Total;
                        newitem.Actual_Total = PreviousselectedItem.Actual_Total;
                    }

                }
            }
           
            TempData["MyLaborListInJob"] = laborerList;
            // TempData.Keep("MyLaborList");
            model.Job_ID = JobMasterTempObj.Job_ID;
            model.Job_Status = JobMasterTempObj.Job_Status;
            model.Job_Description = JobMasterTempObj.Job_Description;

            model.Client_Name = JobMasterTempObj.Client_Name;
            model.Other_ClientName = JobMasterTempObj.Other_ClientName;
            model.Client_ContactPerson = JobMasterTempObj.Client_ContactPerson;
            model.Client_Address = JobMasterTempObj.Client_Address;
            model.Client_Address2 = JobMasterTempObj.Client_Address2;
            model.Client_City = JobMasterTempObj.Client_City;
            model.Client_State = JobMasterTempObj.Client_State;
            model.Client_ZipCode = JobMasterTempObj.Client_ZipCode;
            model.Client_Fax = JobMasterTempObj.Client_Fax;
            model.Client_Email = JobMasterTempObj.Client_Email;
            model.Client_Phone = JobMasterTempObj.Client_Phone;
            model.Client_Mobile = JobMasterTempObj.Client_Mobile;
            model.Previous_Location = JobMasterTempObj.Previous_Location;
            model.Other_Previous_Location = JobMasterTempObj.Other_Previous_Location;
            model.Work_Address = JobMasterTempObj.Work_Address;
            model.Work_Address2 = JobMasterTempObj.Work_Address2;
            model.Work_City = JobMasterTempObj.Work_City;
            model.Work_State = JobMasterTempObj.Work_State;
            model.Directions_To = JobMasterTempObj.Directions_To;
            model.Doing_What = JobMasterTempObj.Doing_What;
            //Mileage Table Values
            model.Job_Mileage_Estimated = JobMasterTempObj.Job_Mileage_Estimated;
            model.Job_Mileage_Actual = JobMasterTempObj.Job_Mileage_Actual;
            model.Job_Mileage_Cost = JobMasterTempObj.Job_Mileage_Cost;
            model.Job_Mileage_ExpTotal = JobMasterTempObj.Job_Mileage_ExpTotal;
            model.Job_Mileage_EstTotal = JobMasterTempObj.Job_Mileage_EstTotal;
            model.Job_Mileage_ActTotal = JobMasterTempObj.Job_Mileage_ActTotal;
            //Estimate Values
            model.EstimateValue_Overhead_ExpTotal = JobMasterTempObj.EstimateValue_Overhead_ExpTotal;
            model.EstimateValue_Profit_Percentage = JobMasterTempObj.EstimateValue_Profit_Percentage;
            model.EstimateValue_SubTotal = JobMasterTempObj.EstimateValue_SubTotal;
            model.EstimateValue_ExpTotal = JobMasterTempObj.EstimateValue_ExpTotal;
            model.EstimateValue_Profit_EstTotal = JobMasterTempObj.EstimateValue_Profit_EstTotal;
            model.Estimate_Override = JobMasterTempObj.Estimate_Override;
            //Job Total Values
            model.Job_ExpenseTotal = JobMasterTempObj.Job_ExpenseTotal;
            model.Job_EstimatedTotal = JobMasterTempObj.Job_EstimatedTotal;
            model.Job_ActualTotal = JobMasterTempObj.Job_ActualTotal;
            model.Profit_Loss_Total = JobMasterTempObj.Profit_Loss_Total;
            ViewBag.JobLabour = "JobLabour";
            ViewBag.JobParts = "JobParts";
            return View("AddJobDetails", model);
            
           
        }

          [OutputCache(Duration = 30)]
        public ActionResult AddNewLegalDetailsList(string LegalGridData, string _JobPartsgriddata, string _JobLabourGridData, string ASPartsGridData, string ASLabourGridData, string JobID, string JobStatus, string JobDescription, string ClientName, string OtherClientName, string ClientContactPerson, string ClientAddress, string ClientAddress2, string ClientCity, string ClientState, string ClientZipCode, string ClientFax, string ClientEmail, string ClientPhone, string ClientMobile, string PreviousLocation, string OtherPreviousLocation, string WorkAddress, string WorkAddress2, string WorkCity, string WorkState, string DirectionsTo, string DoingWhat, string MileageEstimated, string MileageActualCost, string MileageCost, string MileExpTotal, string MileagExpTotal, string MileEstTotal, string MileageActTotal, string EstimateOverExpTotal, string EstimateProfitPercentage, string EstimateSubTotal, string EstimateExpTotal, string EstimateProfitTotal, string EstimateOverride, string JobExpensesTotal, string JobEstimateTotal, string JobActualTotal, string JobProfitLossTotal)
        {
            if (LegalGridData != null)
            {
                var MyLegalGridData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LegalGridData);
                TempData["LegalGridData"] = MyLegalGridData;
            }
            if (_JobPartsgriddata != null)
            {
                var JobPartsListData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(_JobPartsgriddata);
                TempData["JobPartsListData"] = JobPartsListData;
            }
            if (_JobLabourGridData != null)
            {
                var JobLabourListData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(_JobLabourGridData);
                TempData["JobLabourListData"] = JobLabourListData;
            }
            if (ASPartsGridData != null)
            {
                var ASPartsData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(ASPartsGridData);
                TempData["AsPartsListData"] = ASPartsData;
            }
            if (ASLabourGridData != null)
            {
                var ASLabourData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(ASLabourGridData);
                TempData["ASLabourListData"] = ASLabourData;
            }
            //JobMasterTempObj.Job_ID = Convert.ToInt32(JobID);
            JobMasterTempObj.Job_Status = JobStatus;
            JobMasterTempObj.Job_Description = JobDescription;
            JobMasterTempObj.Client_Name = ClientName;
            JobMasterTempObj.Other_ClientName = OtherClientName;
            JobMasterTempObj.Client_ContactPerson = ClientContactPerson;
            JobMasterTempObj.Client_Address = ClientAddress;
            JobMasterTempObj.Client_Address2 = ClientAddress2;
            JobMasterTempObj.Client_City = ClientCity;
            JobMasterTempObj.Client_State = ClientState;
            JobMasterTempObj.Client_ZipCode = ClientZipCode;
            JobMasterTempObj.Client_Fax = ClientFax;
            JobMasterTempObj.Client_Email = ClientEmail;
            JobMasterTempObj.Client_Phone = ClientPhone;
            JobMasterTempObj.Client_Mobile = ClientMobile;
            JobMasterTempObj.Previous_Location = PreviousLocation;
            JobMasterTempObj.Other_Previous_Location = OtherPreviousLocation;
            JobMasterTempObj.Work_Address = WorkAddress;
            JobMasterTempObj.Work_Address2 = WorkAddress2;
            JobMasterTempObj.Work_City = WorkCity;
            JobMasterTempObj.Work_State = WorkState;
            JobMasterTempObj.Directions_To = DirectionsTo;
            JobMasterTempObj.Doing_What = DoingWhat;
            //Mileage Table Values
            if (MileageEstimated != null && MileageEstimated != "")
            {
                JobMasterTempObj.Job_Mileage_Estimated = Convert.ToDecimal(MileageEstimated);
            }

            if (MileageActualCost != null && MileageActualCost != "")
            {
                JobMasterTempObj.Job_Mileage_Actual = Convert.ToDecimal(MileageActualCost);
            }

            if (MileageCost != null && MileageCost != "")
            {
                JobMasterTempObj.Job_Mileage_Cost = Convert.ToDecimal(MileageCost);
            }

            if (MileagExpTotal != null && MileagExpTotal != "")
            {
                JobMasterTempObj.Job_Mileage_ExpTotal = Convert.ToDecimal(MileagExpTotal);
            }

            if (MileEstTotal != null && MileEstTotal != "")
            {
                JobMasterTempObj.Job_Mileage_EstTotal = Convert.ToDecimal(MileEstTotal);
            }

            if (MileageActTotal != null && MileageActTotal != "")
            {
                JobMasterTempObj.Job_Mileage_ActTotal = Convert.ToDecimal(MileageActTotal);
            }

            //Estimate Values

            if (EstimateOverExpTotal != null && EstimateOverExpTotal != "")
            {
                JobMasterTempObj.EstimateValue_Overhead_ExpTotal = Convert.ToDecimal(EstimateOverExpTotal);
            }
            if (EstimateSubTotal != null && EstimateSubTotal != "")
            {
                JobMasterTempObj.EstimateValue_SubTotal = Convert.ToDecimal(EstimateSubTotal);
            }
            if (EstimateExpTotal != null && EstimateExpTotal != "")
            {
                JobMasterTempObj.EstimateValue_ExpTotal = Convert.ToDecimal(EstimateExpTotal);
            }
            if (EstimateProfitTotal != null && EstimateProfitTotal != "")
            {
                JobMasterTempObj.EstimateValue_Profit_EstTotal = Convert.ToDecimal(EstimateProfitTotal);
            }
            if (EstimateOverride != null && EstimateOverride != "")
            {
                JobMasterTempObj.Estimate_Override = Convert.ToDecimal(EstimateOverride);
            }

            //Job Total Values
            if (JobExpensesTotal != null && JobExpensesTotal != "")
            {
                JobMasterTempObj.Job_ExpenseTotal = Convert.ToDecimal(JobExpensesTotal);
            }
            if (JobEstimateTotal != null && JobEstimateTotal != "")
            {
                JobMasterTempObj.Job_EstimatedTotal = Convert.ToDecimal(JobEstimateTotal);
            }
            if (JobActualTotal != null && JobActualTotal != "")
            {
                JobMasterTempObj.Job_ActualTotal = Convert.ToDecimal(JobActualTotal);

            }
            if (JobProfitLossTotal != null && JobProfitLossTotal != "")
            {
                JobMasterTempObj.Profit_Loss_Total = Convert.ToDecimal(JobProfitLossTotal);
            }
               
            
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResultList<Legal_DetailsInfoList> response = new ServiceResultList<Legal_DetailsInfoList>();
            List<Legal_DetailsInfoList> LegalList = new List<Legal_DetailsInfoList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = LegalResponse.GetMyLegalList(result.Client_ID);

            List<Legal_DetailsInfoList> selectedParts = TempData["MyLegalList"] as List<Legal_DetailsInfoList> ?? new List<Legal_DetailsInfoList>();
            for (int i = 0; i < response.ListData.Count; i++)
            {
                if (selectedParts.Count > 0)
                {
                    foreach (Legal_DetailsInfoList list in selectedParts)
                    {
                        if (list.Legal_ID == response.ListData[i].Legal_ID)
                        {
                            response.ListData[i].isChekced = true;
                        }
                    }
                }
                LegalList.Add(new Legal_DetailsInfoList
                {
                    Client_ID = response.ListData[i].Client_ID,
                    Legal_Detail = response.ListData[i].Legal_Detail,
                    Legal_Category = response.ListData[i].Legal_Category,
                    isChekced = response.ListData[i].isChekced
                });
            }
            return View("_AddNewLegalDetails", LegalList.ToList());
        }

        [HttpPost]
        [OutputCache(Duration = 30)]
        public ActionResult AddLegalDetailsInJob(FormCollection Form, Job_MasterInfo model)
        {
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
           
            ServiceResultList<Legal_DetailsInfoList> response = new ServiceResultList<Legal_DetailsInfoList>();
           
            List<Legal_DetailsInfoList> NewLegalListOBJ = new List<Legal_DetailsInfoList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
          // List< Legal_DetailsInfoList> LegalListOBJ = new list <Legal_DetailsInfoList>();
            var result = ClientResponse.GetClientName(Loginuser);
            List<Parts_DetailsInfoList> partsListOBJ = new List<Parts_DetailsInfoList>();
            List<Labor_DetailsInfoList> LabourlistOBJ = new List<Labor_DetailsInfoList>();
            List<AssembliesParts_DetailsInfoList> ASpartsListOBJ = new List<AssembliesParts_DetailsInfoList>();
            List<AssembliesLabourDetailsList> ASLabourlistOBJ = new List<AssembliesLabourDetailsList>();
            List<Labor_DetailsInfoList> laborerList = new List<Labor_DetailsInfoList>();
            List<Assembly_MasterInfo> asLaborListObj = new List<Assembly_MasterInfo>();
            List<Assembly_MasterInfo> asPartsListObj = new List<Assembly_MasterInfo>();

            if (TempData["AsPartsListData"] != null)
            {
                var ps = (Dictionary<string, string[]>)TempData["AsPartsListData"];
                foreach (string[] s in ps.Values)
                {
                    Assembly_MasterInfo obj = new Assembly_MasterInfo();
                    obj.Assemblies_Name = s[0];
                    obj.Assemblies_Category = s[1];
                    obj.Part_Number = s[2];
                    obj.Part_Cost = Convert.ToDecimal(s[3]);
                    obj.Part_Resale = Convert.ToDecimal(s[4]);
                    obj.Estimated_Qty = Convert.ToInt32(s[5]);
                    obj.Actual_Qty = Convert.ToInt32(s[6]);
                    obj.Expected_Total = Convert.ToDecimal(s[6]);
                    obj.Estimated_Total = Convert.ToDecimal(s[7]);
                    obj.Actual_Total = Convert.ToDecimal(s[8]);
                    obj.isChekced = Convert.ToBoolean(true);
                    asPartsListObj.Add(new Assembly_MasterInfo
                    {
                        Assemblies_Name = obj.Assemblies_Name,
                        Assemblies_Category = obj.Assemblies_Category,
                        Part_Number = obj.Part_Number,
                        Part_Cost = obj.Part_Cost,
                        Part_Resale = obj.Part_Resale,
                        Actual_Qty = obj.Actual_Qty,
                        Estimated_Qty = obj.Estimated_Qty,
                        Estimated_Total = obj.Estimated_Total,
                        Expected_Total = obj.Expected_Total,
                        Actual_Total = obj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                }
            }

            TempData["JobAssembliesParts"] = asPartsListObj;

            if (TempData["ASLabourListData"] != null)
            {
                var ls = (Dictionary<string, string[]>)TempData["ASLabourListData"];
                foreach (string[] str in ls.Values)
                {
                    Assembly_MasterInfo laborobj = new Assembly_MasterInfo();
                    laborobj.Assemblies_Name = str[0];
                    laborobj.Assemblies_Category = str[1];
                    laborobj.labor_cost = Convert.ToDecimal(str[2]);
                    laborobj.labor_ResaleCost = Convert.ToDecimal(str[3]);
                    if (str[4] == "")
                    {
                        str[4] = "0.0";
                    }
                    laborobj.Estimated_Hour = Convert.ToDecimal(str[4]);
                    if (str[5] == "")
                    {
                        str[5] = "0.0";
                    }
                    laborobj.Actual_Hours = Convert.ToDecimal(str[5]);

                    laborobj.isChekced = Convert.ToBoolean(true);

                    if (str[6] == "")
                    {
                        str[6] = "0.0";
                    }
                    laborobj.Expected_Total = Convert.ToDecimal(str[6]);
                    if (str[7] == "")
                    {
                        str[7] = "0.0";
                    }
                    laborobj.Estimated_Total = Convert.ToDecimal(str[7]);
                    if (str[8] == "")
                    {
                        str[8] = "0.0";
                    }
                    laborobj.Actual_Total = Convert.ToDecimal(str[8]);
                    asLaborListObj.Add(new Assembly_MasterInfo
                    {
                        // Laborer_Name = laborobj.Laborer_Name,
                        Assemblies_Name = laborobj.Assemblies_Name,
                        Assemblies_Category = laborobj.Assemblies_Category,
                        labor_cost = laborobj.labor_cost,
                        labor_ResaleCost = laborobj.labor_ResaleCost,
                        Actual_Hours = laborobj.Actual_Hours,
                        Estimated_Hour = laborobj.Estimated_Hour,
                        Estimated_Total = laborobj.Estimated_Total,
                        Expected_Total = laborobj.Expected_Total,
                        Actual_Total = laborobj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                }

            }
            TempData["JobAssembliesLabor"] = asLaborListObj;
            if (TempData["JobPartsListData"] != null)
            {
                var ps = (Dictionary<string, string[]>)TempData["JobPartsListData"];
                foreach (string[] s in ps.Values)
                {
                    Parts_DetailsInfoList obj = new Parts_DetailsInfoList();

                    obj.Part_Number = s[0];
                    obj.Description = s[1];
                    obj.Part_Cost = Convert.ToDecimal(s[2]);
                    obj.Resale_Cost = Convert.ToDecimal(s[3]);
                    obj.Estimated_Qty = Convert.ToInt32(s[4]);
                    obj.Actual_Qty = Convert.ToInt32(s[5]);
                    obj.Expected_Total = Convert.ToDecimal(s[6]);
                    obj.Estimated_Total = Convert.ToDecimal(s[7]);
                    obj.Actual_Total = Convert.ToDecimal(s[8]);
                    obj.isChekced = Convert.ToBoolean(true);
                    partsListOBJ.Add(new Parts_DetailsInfoList
                    {
                        Part_Number = obj.Part_Number,
                        Description=obj.Description,
                        Part_Category = obj.Part_Category,
                        Part_Cost = obj.Part_Cost,
                        Resale_Cost = obj.Resale_Cost,
                        Purchased_From = obj.Purchased_From,
                        Actual_Qty = obj.Actual_Qty,
                        Estimated_Qty = obj.Estimated_Qty,
                        Estimated_Total = obj.Estimated_Total,
                        Expected_Total = obj.Expected_Total,
                        Actual_Total = obj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                }

            }

            TempData["MyPartsInJob"] = partsListOBJ;
            if (TempData["JobLabourListData"] != null)
            {
                var ls = (Dictionary<string, string[]>)TempData["JobLabourListData"];


                foreach (string[] str in ls.Values)
                {
                    Labor_DetailsInfoList laborobj = new Labor_DetailsInfoList();
                    laborobj.Laborer_Name = str[0];
                    laborobj.Cost = Convert.ToDecimal(str[1]);
                    laborobj.Burden = Convert.ToDecimal(str[2]);
                    laborobj.Resale_Cost = Convert.ToDecimal(str[3]);
                    laborobj.isChekced = Convert.ToBoolean(true);
                    laborobj.Estimated_Hour = Convert.ToDecimal(str[4]);
                    laborobj.Actual_Hours = Convert.ToDecimal(str[5]);
                    if (str[6] == "")
                    {
                        str[6] = "0.0";
                    }
                    laborobj.Expected_Total = Convert.ToDecimal(str[6]);
                    if (str[7] == "")
                    {
                        str[7] = "0.0";
                    }
                    laborobj.Estimated_Total = Convert.ToDecimal(str[7]);
                    if (str[8] == "")
                    {
                        str[8] = "0.0";
                    }
                    laborobj.Actual_Total = Convert.ToDecimal(str[8]);
                    LabourlistOBJ.Add(new Labor_DetailsInfoList
                    {
                        Laborer_Name = laborobj.Laborer_Name,
                        Labor_Category = laborobj.Labor_Category,
                        Cost = laborobj.Cost,
                        Burden = laborobj.Burden,
                        Resale_Cost = laborobj.Resale_Cost,
                        Actual_Hours = laborobj.Actual_Hours,
                        Estimated_Hour = laborobj.Estimated_Hour,
                        Estimated_Total = laborobj.Estimated_Total,
                        Expected_Total = laborobj.Expected_Total,
                        Actual_Total = laborobj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });

                }

            }
            TempData["MyLaborListInJob"] = LabourlistOBJ;




          
            List<Legal_DetailsInfoList> LegalListOBJ = new List<Legal_DetailsInfoList>();
            if (TempData["LegalGridData"] != null)
            {
                 // List<Legal_DetailsInfoList> LegalList = new List<Legal_DetailsInfoList>();
                var Legal = (Dictionary<string, string[]>)TempData["LegalGridData"];
                foreach (string[] LegalItem in Legal.Values)
                {
                  Legal_DetailsInfoList LegalList = new Legal_DetailsInfoList();
                    LegalList.Legal_ID = Convert.ToInt32(LegalItem[0]);
                    LegalList.Legal_Detail = LegalItem[1];
                    LegalListOBJ.Add(new Legal_DetailsInfoList
                    {
                        //Client_ID = obj.Client_ID,
                        Legal_ID = LegalList.Legal_ID,
                        Legal_Detail = LegalList.Legal_Detail,
                        isChekced = Convert.ToBoolean(true)
                    });

                }
                
            }

            var SelectedLegal = Form.GetValues("assignChkBx");
            if (SelectedLegal != null)
            {
                foreach (var selectedList in SelectedLegal)
                {
                    string[] splitData = selectedList.Split(',');
                    //In argument we are passing clientID and laborerName to db.
                    response = LegalResponse.GetMyLegalList(Convert.ToInt16(splitData[0]));
                    NewLegalListOBJ.Add(new Legal_DetailsInfoList
                    { 
                        Client_ID = response.ListData[0].Client_ID, 
                        Legal_ID = response.ListData[0].Legal_ID, 
                        Legal_Category = response.ListData[0].Legal_Category,
                        Legal_Detail = response.ListData[0].Legal_Detail,
                         isChekced = true });

                }
            }
            foreach (var newitem in NewLegalListOBJ)
            {
                foreach (Legal_DetailsInfoList PreviousselectedItem in LegalListOBJ)
                {
                    if (PreviousselectedItem.Legal_ID == newitem.Legal_ID && PreviousselectedItem.isChekced == newitem.isChekced)
                    {
                        newitem.Legal_ID = PreviousselectedItem.Legal_ID;
                        newitem.Legal_Detail = PreviousselectedItem.Legal_Detail;
                        //isChekced = Convert.ToBoolean(true)
                    }

                }
            }
            TempData["MyLegalList"] = NewLegalListOBJ;
            model.Job_ID = JobMasterTempObj.Job_ID;
            model.Job_Status = JobMasterTempObj.Job_Status;
            model.Job_Description = JobMasterTempObj.Job_Description;

            model.Client_Name = JobMasterTempObj.Client_Name;
            model.Other_ClientName = JobMasterTempObj.Other_ClientName;
            model.Client_ContactPerson = JobMasterTempObj.Client_ContactPerson;
            model.Client_Address = JobMasterTempObj.Client_Address;
            model.Client_Address2 = JobMasterTempObj.Client_Address2;
            model.Client_City = JobMasterTempObj.Client_City;
            model.Client_State = JobMasterTempObj.Client_State;
            model.Client_ZipCode = JobMasterTempObj.Client_ZipCode;
            model.Client_Fax = JobMasterTempObj.Client_Fax;
            model.Client_Email = JobMasterTempObj.Client_Email;
            model.Client_Phone = JobMasterTempObj.Client_Phone;
            model.Client_Mobile = JobMasterTempObj.Client_Mobile;
            model.Previous_Location = JobMasterTempObj.Previous_Location;
            model.Other_Previous_Location = JobMasterTempObj.Other_Previous_Location;
            model.Work_Address = JobMasterTempObj.Work_Address;
            model.Work_Address2 = JobMasterTempObj.Work_Address2;
            model.Work_City = JobMasterTempObj.Work_City;
            model.Work_State = JobMasterTempObj.Work_State;
            model.Directions_To = JobMasterTempObj.Directions_To;
            model.Doing_What = JobMasterTempObj.Doing_What;
            //Mileage Table Values
            model.Job_Mileage_Estimated = JobMasterTempObj.Job_Mileage_Estimated;
            model.Job_Mileage_Actual = JobMasterTempObj.Job_Mileage_Actual;
            model.Job_Mileage_Cost = JobMasterTempObj.Job_Mileage_Cost;
            model.Job_Mileage_ExpTotal = JobMasterTempObj.Job_Mileage_ExpTotal;
            model.Job_Mileage_EstTotal = JobMasterTempObj.Job_Mileage_EstTotal;
            model.Job_Mileage_ActTotal = JobMasterTempObj.Job_Mileage_ActTotal;
            //Estimate Values
            model.EstimateValue_Overhead_ExpTotal = JobMasterTempObj.EstimateValue_Overhead_ExpTotal;
            model.EstimateValue_Profit_Percentage = JobMasterTempObj.EstimateValue_Profit_Percentage;
            model.EstimateValue_SubTotal = JobMasterTempObj.EstimateValue_SubTotal;
            model.EstimateValue_ExpTotal = JobMasterTempObj.EstimateValue_ExpTotal;
            model.EstimateValue_Profit_EstTotal = JobMasterTempObj.EstimateValue_Profit_EstTotal;
            model.Estimate_Override = JobMasterTempObj.Estimate_Override;
            //Job Total Values
            model.Job_ExpenseTotal = JobMasterTempObj.Job_ExpenseTotal;
            model.Job_EstimatedTotal = JobMasterTempObj.Job_EstimatedTotal;
            model.Job_ActualTotal = JobMasterTempObj.Job_ActualTotal;
            model.Profit_Loss_Total = JobMasterTempObj.Profit_Loss_Total;

            ViewBag.Legal = "Legal";
            return View("AddJobDetails", model);

        }


        public ActionResult GetTableListData(string JobPartsGridata, string JobLabourGridData, string AsPartsGridData, string AsLabourGridData, string LegalGridData, string AssembliesPartsList, string AssembliesLaborList)
        {
            if (LegalGridData != null)
            {
                var MyLegalGridData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LegalGridData);
                TempData["LegalGridData"] = MyLegalGridData;
            }
            if (JobPartsGridata != null)
            {
                var JobPartsListData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(JobPartsGridata);
                TempData["JobPartsListData"] = JobPartsListData;
            }
            if (JobLabourGridData != null)
            {
                var JobLabourListData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(JobLabourGridData);
                TempData["JobLabourListData"] = JobLabourListData;
            }
            if (AsPartsGridData != null)
            {
                var ASPartsData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(AsPartsGridData);
                TempData["AsPartsListData"] = ASPartsData;
            }
            if (AsLabourGridData != null)
            {
                var ASLabourData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(AsLabourGridData);
                TempData["ASLabourListData"] = ASLabourData;
            }
            //if (AssembliesPartsList != null)
            //{
            //    var ASPartsListData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(AssembliesPartsList);
            //    TempData["JobASPartsListData"] = ASPartsListData;
            //}
            //if (AssembliesLaborList != null)
            //{
            //    var ASLaborListData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(AssembliesLaborList);
            //    TempData["JobASLabourListData"] = ASLaborListData;
            //}
            return Json("Success");
        }

        [HttpPost]
        public ActionResult SaveJobDetails(Job_MasterInfo Model)
        {
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            Assembly_MasterInfo AssemblyResponse = new Assembly_MasterInfo();

            ServiceResultList<Legal_DetailsInfoList> response = new ServiceResultList<Legal_DetailsInfoList>();
            List<Legal_DetailsInfoList> NewLegalListOBJ = new List<Legal_DetailsInfoList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            // List< Legal_DetailsInfoList> LegalListOBJ = new list <Legal_DetailsInfoList>();
            var result = ClientResponse.GetClientName(Loginuser);
            List<Parts_DetailsInfoList> partsListOBJ = new List<Parts_DetailsInfoList>();
            List<Labor_DetailsInfoList> LabourlistOBJ = new List<Labor_DetailsInfoList>();
            List<AssembliesParts_DetailsInfoList> ASpartsListOBJ = new List<AssembliesParts_DetailsInfoList>();
            List<AssembliesLabourDetailsList> ASLabourlistOBJ = new List<AssembliesLabourDetailsList>();
            List<Labor_DetailsInfoList> laborerList = new List<Labor_DetailsInfoList>();
            List<Assembly_MasterInfo> jobAssemblyPartList = new List<Assembly_MasterInfo>();
            List<Assembly_MasterInfo> jobAssemblyLaborList = new List<Assembly_MasterInfo>();
            if (TempData["JobPartsListData"] != null)
            {
                var ps = (Dictionary<string, string[]>)TempData["JobPartsListData"];
                foreach (string[] s in ps.Values)
                {
                    Parts_DetailsInfoList obj = new Parts_DetailsInfoList();

                    obj.Part_Number = s[0];
                    obj.Part_Cost = Convert.ToDecimal(s[1]);
                    obj.Resale_Cost = Convert.ToDecimal(s[2]);
                    obj.Estimated_Qty = Convert.ToInt32(s[3]);
                    obj.Actual_Qty = Convert.ToInt32(s[4]);
                    obj.Expected_Total = Convert.ToDecimal(s[5]);
                    obj.Estimated_Total = Convert.ToDecimal(s[6]);
                    obj.Actual_Total = Convert.ToDecimal(s[7]);
                    obj.isChekced = Convert.ToBoolean(true);
                    partsListOBJ.Add(new Parts_DetailsInfoList
                    {
                        Part_Number = obj.Part_Number,
                        Part_Category = obj.Part_Category,
                        Part_Cost = obj.Part_Cost,
                        Resale_Cost = obj.Resale_Cost,
                        Purchased_From = obj.Purchased_From,
                        Actual_Qty = obj.Actual_Qty,
                        Estimated_Qty = obj.Estimated_Qty,
                        Estimated_Total = obj.Estimated_Total,
                        Expected_Total = obj.Expected_Total,
                        Actual_Total = obj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });
                }

            }

           // TempData["MyPartsInJob"] = partsListOBJ;
            Model.NewPartsListInJob = partsListOBJ;
            if (TempData["JobLabourListData"] != null)
            {
                var ls = (Dictionary<string, string[]>)TempData["JobLabourListData"];


                foreach (string[] str in ls.Values)
                {
                    Labor_DetailsInfoList laborobj = new Labor_DetailsInfoList();
                    laborobj.Laborer_Name = str[0];
                    laborobj.Cost = Convert.ToDecimal(str[1]);
                    laborobj.Burden = Convert.ToDecimal(str[2]);
                    laborobj.Resale_Cost = Convert.ToDecimal(str[3]);
                    laborobj.isChekced = Convert.ToBoolean(true);
                    laborobj.Estimated_Hour = Convert.ToDecimal(str[4]);
                    laborobj.Actual_Hours = Convert.ToDecimal(str[5]);
                    if (str[6] == "")
                    {
                        str[6] = "0.0";
                    }
                    laborobj.Expected_Total = Convert.ToDecimal(str[6]);
                    if (str[7] == "")
                    {
                        str[7] = "0.0";
                    }
                    laborobj.Estimated_Total = Convert.ToDecimal(str[7]);
                    if (str[8] == "")
                    {
                        str[8] = "0.0";
                    }
                    laborobj.Actual_Total = Convert.ToDecimal(str[8]);
                    LabourlistOBJ.Add(new Labor_DetailsInfoList
                    {
                        Laborer_Name = laborobj.Laborer_Name,
                        Labor_Category = laborobj.Labor_Category,
                        Cost = laborobj.Cost,
                        Burden = laborobj.Burden,
                        Resale_Cost = laborobj.Resale_Cost,
                        Actual_Hours = laborobj.Actual_Hours,
                        Estimated_Hour = laborobj.Estimated_Hour,
                        Estimated_Total = laborobj.Estimated_Total,
                        Expected_Total = laborobj.Expected_Total,
                        Actual_Total = laborobj.Actual_Total,
                        isChekced = Convert.ToBoolean(true)
                    });

                }

            }
           // TempData["MyLaborListInJob"] = LabourlistOBJ;
            Model.NewLabourListInJob = LabourlistOBJ;
            if (TempData["JobAssembliesParts"] != null)
            {
                List<Assembly_MasterInfo> selectedParts = TempData["JobAssembliesParts"] as List<Assembly_MasterInfo> ?? new List<Assembly_MasterInfo>();
                Assembly_MasterInfo objAssembliesPartsInfo = new Assembly_MasterInfo();
                foreach (Assembly_MasterInfo objAssParts in selectedParts)
                {
                    objAssembliesPartsInfo.Assemblies_Name = objAssParts.Assemblies_Name;
                    objAssembliesPartsInfo.Assemblies_Category = objAssParts.Assemblies_Category;
                    objAssembliesPartsInfo.Assemblies_Description = objAssParts.Assemblies_Description;
                    objAssembliesPartsInfo.Part_Number = objAssParts.Part_Number;
                    objAssembliesPartsInfo.Part_Cost = objAssParts.Part_Cost;
                    objAssembliesPartsInfo.Part_Resale = objAssParts.Part_Resale;
                    objAssembliesPartsInfo.Estimated_Qty = objAssParts.Estimated_Qty;
                    objAssembliesPartsInfo.Actual_Qty = objAssParts.Actual_Qty;
                    objAssembliesPartsInfo.Expected_Total = objAssParts.Expected_Total;
                    objAssembliesPartsInfo.Estimated_Total = objAssParts.Estimated_Total!=null ?objAssParts.Estimated_Total:Convert.ToDecimal("0.0");
                    objAssembliesPartsInfo.Actual_Total = objAssParts.Actual_Total != null ? objAssParts.Actual_Total : Convert.ToDecimal("0.0");
                    jobAssemblyPartList.Add(new Assembly_MasterInfo
                    {
                        Assemblies_Name = objAssembliesPartsInfo.Assemblies_Name,
                        Assemblies_Category = objAssembliesPartsInfo.Assemblies_Category,
                        Assemblies_Description = objAssembliesPartsInfo.Assemblies_Description,
                        Part_Number=objAssembliesPartsInfo.Part_Number,
                        Part_Cost = objAssembliesPartsInfo.Part_Cost,
                        Part_Resale = objAssembliesPartsInfo.Part_Resale,
                        Estimated_Qty = objAssembliesPartsInfo.Estimated_Qty,
                        Actual_Qty = objAssembliesPartsInfo.Actual_Qty,
                        Expected_Total = objAssembliesPartsInfo.Expected_Total,
                        Estimated_Total = objAssembliesPartsInfo.Estimated_Total
                    });
                }
                
            }
            Model.NewJobAssemblyPartsList = jobAssemblyPartList;
            if (TempData["JobAssembliesLabor"] != null)
            {
                List<Assembly_MasterInfo> selectedParts = TempData["JobAssembliesLabor"] as List<Assembly_MasterInfo> ?? new List<Assembly_MasterInfo>();
                Assembly_MasterInfo objAssembliesPartsInfo = new Assembly_MasterInfo();
                foreach (Assembly_MasterInfo objAssParts in selectedParts)
                {
                    objAssembliesPartsInfo.Assemblies_Name = objAssParts.Assemblies_Name;
                    objAssembliesPartsInfo.Assemblies_Category = objAssParts.Assemblies_Category;
                    objAssembliesPartsInfo.Assemblies_Description = objAssParts.Assemblies_Description;
                    objAssembliesPartsInfo.Part_Number = objAssParts.Part_Number;
                    objAssembliesPartsInfo.labor_cost = objAssParts.labor_cost;
                    objAssembliesPartsInfo.Burden = objAssParts.Burden;
                    objAssembliesPartsInfo.Lobor_Resale = objAssParts.labor_ResaleCost;
                    objAssembliesPartsInfo.Estimated_Hour = objAssParts.Estimated_Hour;
                    objAssembliesPartsInfo.Actual_Hours = objAssParts.Actual_Hours;
                    objAssembliesPartsInfo.Expected_Total = objAssParts.Expected_Total;
                    objAssembliesPartsInfo.Estimated_Total = objAssParts.Estimated_Total;
                    objAssembliesPartsInfo.Actual_Total = objAssParts.Actual_Total;
                    jobAssemblyLaborList.Add(new Assembly_MasterInfo
                    {
                        Assemblies_Name = objAssembliesPartsInfo.Assemblies_Name,
                        Assemblies_Category = objAssembliesPartsInfo.Assemblies_Category,
                        Assemblies_Description = objAssembliesPartsInfo.Assemblies_Description,
                        Part_Number = objAssembliesPartsInfo.Part_Number,
                        Part_Cost = objAssembliesPartsInfo.Part_Cost,
                        Part_Resale = objAssembliesPartsInfo.Part_Resale,
                        Estimated_Qty = objAssembliesPartsInfo.Estimated_Qty,
                        Actual_Qty = objAssembliesPartsInfo.Actual_Qty,
                        Expected_Total = objAssembliesPartsInfo.Expected_Total,
                        Estimated_Total = objAssembliesPartsInfo.Estimated_Total
                    });
                }

            }
            Model.NewJobAssemblyLaborList = jobAssemblyLaborList;
           // if (TempData["AsPartsListData"] != null)
           // {
           //     var ps = (Dictionary<string, string[]>)TempData["AsPartsListData"];
           //     foreach (string[] s in ps.Values)
           //     {
           //         AssembliesParts_DetailsInfoList obj = new AssembliesParts_DetailsInfoList();
           //         obj.Part_Number = s[0];
           //         obj.Part_Cost = Convert.ToDecimal(s[1]);
           //         obj.Resale_Cost = Convert.ToDecimal(s[2]);
           //         obj.Estimated_Qty = Convert.ToInt32(s[3]);
           //         obj.Actual_Qty = Convert.ToInt32(s[4]);
           //         obj.Expected_Total = Convert.ToDecimal(s[5]);
           //         obj.Estimated_Total = Convert.ToDecimal(s[6]);
           //         obj.Actual_Total = Convert.ToDecimal(s[7]);
           //         obj.isChekced = Convert.ToBoolean(true);
           //         ASpartsListOBJ.Add(new AssembliesParts_DetailsInfoList
           //         {
           //             Part_Number = obj.Part_Number,
           //             Part_Category = obj.Part_Category,
           //             Part_Cost = obj.Part_Cost,
           //             Resale_Cost = obj.Resale_Cost,
           //             Purchased_From = obj.Purchased_From,
           //             Actual_Qty = obj.Actual_Qty,
           //             Estimated_Qty = obj.Estimated_Qty,
           //             Estimated_Total = obj.Estimated_Total,
           //             Expected_Total = obj.Expected_Total,
           //             Actual_Total = obj.Actual_Total,
           //             isChekced = Convert.ToBoolean(true)
           //         });
           //     }
           // }

           //// TempData["MyASparts"] = ASpartsListOBJ;

           // Model.PartsListData = ASpartsListOBJ;

            //if (TempData["ASLabourListData"] != null)
            //{
            //    var ls = (Dictionary<string, string[]>)TempData["ASLabourListData"];


            //    foreach (string[] str in ls.Values)
            //    {
            //        AssembliesLabourDetailsList laborobj = new AssembliesLabourDetailsList();
            //        //obj.Client_ID = s[0];
            //        // labourobj.Client_ID = Convert.ToInt32(str[0]);
            //        laborobj.Laborer_Name = str[0];
            //        // labourobj.Labor_Category = str[2];
            //        laborobj.Cost = Convert.ToDecimal(str[1]);
            //        laborobj.Burden = Convert.ToDecimal(str[2]);
            //        laborobj.Resale_Cost = Convert.ToDecimal(str[3]);
            //        laborobj.isChekced = Convert.ToBoolean(true);
            //        laborobj.Estimated_Hour = Convert.ToDecimal(str[4]);
            //        laborobj.Actual_Hours = Convert.ToDecimal(str[5]);
            //        if (str[6] == "")
            //        {
            //            str[6] = "0.0";
            //        }
            //        laborobj.Expected_Total = Convert.ToDecimal(str[6]);
            //        if (str[7] == "")
            //        {
            //            str[7] = "0.0";
            //        }
            //        laborobj.Estimated_Total = Convert.ToDecimal(str[7]);
            //        if (str[8] == "")
            //        {
            //            str[8] = "0.0";
            //        }
            //        laborobj.Actual_Total = Convert.ToDecimal(str[8]);
            //        // Client_ID = response.ListData[i].Client_ID, Part_Number = response.ListData[i].Part_Number, Part_Category = response.ListData[i].Part_Category, Cost = response.ListData[i].Cost, Resale_Cost = response.ListData[i].Resale_Cost, Purchased_From = response.ListData[i].Purchased_From, isChekced = response.ListData[i].isChekced });
            //        ASLabourlistOBJ.Add(new AssembliesLabourDetailsList
            //        {
            //            // Client_ID = labourobj.Client_ID,
            //            Laborer_Name = laborobj.Laborer_Name,
            //            Labor_Category = laborobj.Labor_Category,
            //            Cost = laborobj.Cost,
            //            Burden = laborobj.Burden,
            //            Resale_Cost = laborobj.Resale_Cost,
            //            Actual_Hours = laborobj.Actual_Hours,
            //            Estimated_Hour = laborobj.Estimated_Hour,
            //            Expected_Total = laborobj.Expected_Total,
            //            Estimated_Total = laborobj.Estimated_Total,
            //            Actual_Total = laborobj.Actual_Total,
            //            isChekced = Convert.ToBoolean(true)
            //        });
            //    }

            //}

            ////TempData["MyAsLabourList"] = ASLabourlistOBJ;
            //Model.LabourListData = ASLabourlistOBJ;

            List<Legal_DetailsInfoList> LegalListOBJ = new List<Legal_DetailsInfoList>();
            if (TempData["LegalGridData"] != null)
            {
                // List<Legal_DetailsInfoList> LegalList = new List<Legal_DetailsInfoList>();
                var Legal = (Dictionary<string, string[]>)TempData["LegalGridData"];
                foreach (string[] LegalItem in Legal.Values)
                {
                    Legal_DetailsInfoList LegalList = new Legal_DetailsInfoList();
                    LegalList.Legal_ID = Convert.ToInt32(LegalItem[0]);
                    LegalList.Legal_Detail = LegalItem[1];
                    LegalListOBJ.Add(new Legal_DetailsInfoList
                    {
                        //Client_ID = obj.Client_ID,
                        Legal_ID = LegalList.Legal_ID,
                        Legal_Detail = LegalList.Legal_Detail,
                        isChekced = Convert.ToBoolean(true)
                    });

                }

            }
            Model.NewLegalListInJob = LegalListOBJ;
            Model.Client_Id = result.Client_ID;
            Model.Created_By = Loginuser;
            if (Model.UploadedFile != null)
            {
                string AttachedFileName = System.IO.Path.GetFileName(Model.UploadedFile.FileName);
                //string path = System.IO.Path.Combine(Server.MapPath("~/AppImges/Client_Master"), img);
                //model.UploadedFile.SaveAs(path);
                using (MemoryStream ms = new MemoryStream())
                {
                    Model.UploadedFile.InputStream.CopyTo(ms);
                    Model.Job_Attachment = ms.ToArray();
                    Model.Attachement_Name = AttachedFileName;
                }
            }
            ServiceResult<int> JobInfoResponse = new ServiceResult<int>();
            ServiceResult<int> JobPartsResponse = new ServiceResult<int>();
            ServiceResult<int> JobLabourResponse = new ServiceResult<int>();
            ServiceResult<int> AsPartsResponse = new ServiceResult<int>();
            ServiceResult<int> AsLabourResponse = new ServiceResult<int>();
            ServiceResult<int> JobLegalResponse = new ServiceResult<int>();
            ServiceResult<int> JobAttachmentResponse = new ServiceResult<int>();
            ServiceResult<int> JobAssembliesPartsInfo = new ServiceResult<int>();
            ServiceResult<int> JobAssembliesLaborInfo = new ServiceResult<int>();
            JobMasterBLL JobResponse=new JobMasterBLL();


            if(ModelState.IsValid)
            {
                if (Model.NewPartsListInJob.Count != 0 && Model.NewLabourListInJob.Count != 0 && Model.NewJobAssemblyPartsList.Count != 0 && Model.NewJobAssemblyLaborList.Count != 0 && Model.NewLegalListInJob.Count != 0)
                {

                    JobInfoResponse = JobResponse.AddJobInformation(Model);
                    if (JobInfoResponse.ResultCode > 0)
                    {
                        ViewBag.JobInfoSuccessMsg = JobInfoResponse.Message;
                        JobPartsResponse = JobResponse.AddPartsListDetailsInJob(Model);
                    }
                    else
                    {
                        ViewBag.JobInfoFailMsg = JobInfoResponse.Message;
                        return View("AdJobDetails", Model);
                    }
                    if (JobPartsResponse.ResultCode > 0)
                    {
                        ViewBag.JobPartsSuccessMsg = JobPartsResponse.Message;
                        JobLabourResponse = JobResponse.AddLabouListDetailsInJob(Model);
                    }
                    else
                    {
                        ViewBag.JobPartsFailMsg = JobPartsResponse.Message;
                        return View("AddJobDetails", Model);
                    }
                    if (JobLabourResponse.ResultCode > 0)
                    {
                        ViewBag.JobLabourSuccessMsg = JobLabourResponse.Message;
                        JobAssembliesPartsInfo = JobResponse.AddJobAssembliesPartsList(Model);
                    }
                    else
                    {
                        ViewBag.JobLabourFailMsg = JobLabourResponse.Message;
                        return View("AddJobDetails", Model);
                    }
                    if (JobAssembliesPartsInfo.ResultCode > 0)
                    {
                        ViewBag.JobASPartsSuccessMsg = JobAssembliesPartsInfo.Message;
                        //JobAssembliesLaborInfo = JobResponse.AddJobAssembliesLaborList(Model);
                    }
                    else
                    {
                        ViewBag.JobASPartsFailMsg = JobAssembliesPartsInfo.Message;
                        return View("AddJobDetails", Model);
                    }

                    if (JobAssembliesLaborInfo.ResultCode > 0)
                    {
                        ViewBag.JobASLaborSuccessMsg = JobAssembliesLaborInfo.Message;
                        JobLegalResponse = JobResponse.AddLegalDetailsInJob(Model);
                    }
                    else
                    {
                        ViewBag.JobASLaborFailMsg = JobAssembliesLaborInfo.Message;
                        return View("AddJobDetails", Model);
                    }
                    if (JobLegalResponse.ResultCode > 0)
                    {
                        ViewBag.JobLegalSuccessMsg = JobLegalResponse.Message;
                        if (Model.UploadedFile != null)
                        {
                            JobAttachmentResponse = JobResponse.AddJobAttachMents(Model);
                            ViewBag.JobSuccessMsg = JobAttachmentResponse.Message;
                            TempData["JobSuccessMsg"] = JobAttachmentResponse.Message;
                            return RedirectToAction("AddJobDetails");
                        }
                        else
                        {
                            TempData["JobSuccessMsg"] = "Job Details Saved Successfully!";
                            return RedirectToAction("AddJobDetails");
                        }
                    }
                    else
                    {
                        ViewBag.JobLegalFailMsg = JobLegalResponse.Message;
                        return View("AddJobDetails", Model);
                    }


                }
                if (Model.NewJobAssemblyPartsList.Count != 0 && Model.NewJobAssemblyLaborList.Count != 0 && Model.NewLegalListInJob.Count != 0)
                {

                    JobInfoResponse = JobResponse.AddJobInformation(Model);
                    if (JobInfoResponse.ResultCode > 0)
                    {
                        ViewBag.JobInfoSuccessMsg = JobInfoResponse.Message;
                        JobAssembliesPartsInfo = JobResponse.AddJobAssembliesPartsList(Model);
                    }
                    else
                    {
                        ViewBag.JobInfoFailMsg = JobInfoResponse.Message;
                        return View("AdJobDetails", Model);
                    }
                    if (JobAssembliesPartsInfo.ResultCode > 0)
                    {
                        ViewBag.JobASPartsSuccessMsg = JobAssembliesPartsInfo.Message;
                       // JobAssembliesLaborInfo = JobResponse.AddJobAssembliesLaborList(Model);
                    }
                    else
                    {
                        ViewBag.JobASPartsFailMsg = JobAssembliesPartsInfo.Message;
                        return View("AdJobDetails", Model);
                    }
                    if (JobAssembliesLaborInfo.ResultCode > 0)
                    {
                        ViewBag.JobASLaborSuccessMsg = JobAssembliesLaborInfo.Message;
                        JobLegalResponse = JobResponse.AddLegalDetailsInJob(Model);

                    }
                    else
                    {
                        ViewBag.JobASLaborFailMsg = JobAssembliesLaborInfo.Message;
                        return View("AdJobDetails", Model);
                    }
                    if (JobLegalResponse.ResultCode > 0)
                    {
                        ViewBag.JobLegalSuccessMsg = JobLegalResponse.Message;
                        if (Model.UploadedFile != null)
                        {
                            JobAttachmentResponse = JobResponse.AddJobAttachMents(Model);
                            ViewBag.JobSuccessMsg = JobAttachmentResponse.Message;
                            TempData["JobSuccessMsg"] = JobAttachmentResponse.Message;
                            return RedirectToAction("AddJobDetails");
                        }
                        else
                        {
                            TempData["JobSuccessMsg"] = "Job Details Saved Successfully!";
                            return RedirectToAction("AddJobDetails");
                        }
                    }
                    else
                    {
                        ViewBag.JobLegalFailMsg = JobLegalResponse.Message;
                        return View("AddJobDetails", Model);
                    }

                }
            }
            else
            {
                return View("AddJobDetails", Model);
            }

            return View("AddJobDetails", Model);
            
        }

        #region Add New Assemblies Popup Window List
        public ActionResult AddNewAssembliesPartsListPopUp(string JobID, string JobStatus, string JobDescription, string ClientName, string OtherClientName, string ClientContactPerson, string ClientAddress, string ClientAddress2, string ClientCity, string ClientState, string ClientZipCode, string ClientFax, string ClientEmail, string ClientPhone, string ClientMobile, string PreviousLocation, string OtherPreviousLocation, string WorkAddress, string WorkAddress2, string WorkCity, string WorkState, string DirectionsTo, string DoingWhat, string MileageEstimated, string MileageActualCost, string MileageCost, string MileExpTotal, string MileagExpTotal, string MileEstTotal, string MileageActTotal, string EstimateOverExpTotal, string EstimateProfitPercentage, string EstimateSubTotal, string EstimateExpTotal, string EstimateProfitTotal, string EstimateOverride, string JobExpensesTotal, string JobEstimateTotal, string JobActualTotal, string JobProfitLossTotal)
        {
            //JobMasterTempObj.Assemblies_Name = AsName;
           // JobMasterTempObj.Job_ID = Convert.ToInt32(JobID);
            JobMasterTempObj.Job_Status = JobStatus;
            JobMasterTempObj.Job_Description = JobDescription;
            JobMasterTempObj.Client_Name = ClientName;
            JobMasterTempObj.Other_ClientName = OtherClientName;
            JobMasterTempObj.Client_ContactPerson = ClientContactPerson;
            JobMasterTempObj.Client_Address = ClientAddress;
            JobMasterTempObj.Client_Address2 = ClientAddress2;
            JobMasterTempObj.Client_City = ClientCity;
            JobMasterTempObj.Client_State = ClientState;
            JobMasterTempObj.Client_ZipCode = ClientZipCode;
            JobMasterTempObj.Client_Fax = ClientFax;
            JobMasterTempObj.Client_Email = ClientEmail;
            JobMasterTempObj.Client_Phone = ClientPhone;
            JobMasterTempObj.Client_Mobile = ClientMobile;
            JobMasterTempObj.Previous_Location = PreviousLocation;
            JobMasterTempObj.Other_Previous_Location = OtherPreviousLocation;
            JobMasterTempObj.Work_Address = WorkAddress;
            JobMasterTempObj.Work_Address2 = WorkAddress2;
            JobMasterTempObj.Work_City = WorkCity;
            JobMasterTempObj.Work_State = WorkState;
            JobMasterTempObj.Directions_To = DirectionsTo;
            JobMasterTempObj.Doing_What = DoingWhat;
            //Mileage Table Values
            if (MileageEstimated != null && MileageEstimated != "")
            {
                JobMasterTempObj.Job_Mileage_Estimated = Convert.ToDecimal(MileageEstimated);
            }

            if (MileageActualCost != null && MileageActualCost != "")
            {
                JobMasterTempObj.Job_Mileage_Actual = Convert.ToDecimal(MileageActualCost);
            }

            if (MileageCost != null && MileageCost != "")
            {
                JobMasterTempObj.Job_Mileage_Cost = Convert.ToDecimal(MileageCost);
            }

            if (MileagExpTotal != null && MileagExpTotal != "")
            {
                JobMasterTempObj.Job_Mileage_ExpTotal = Convert.ToDecimal(MileagExpTotal);
            }

            if (MileEstTotal != null && MileEstTotal != "")
            {
                JobMasterTempObj.Job_Mileage_EstTotal = Convert.ToDecimal(MileEstTotal);
            }

            if (MileageActTotal != null && MileageActTotal != "")
            {
                JobMasterTempObj.Job_Mileage_ActTotal = Convert.ToDecimal(MileageActTotal);
            }

            //Estimate Values

            if (EstimateOverExpTotal != null && EstimateOverExpTotal != "")
            {
                JobMasterTempObj.EstimateValue_Overhead_ExpTotal = Convert.ToDecimal(EstimateOverExpTotal);
            }
            if (EstimateSubTotal != null && EstimateSubTotal != "")
            {
                JobMasterTempObj.EstimateValue_SubTotal = Convert.ToDecimal(EstimateSubTotal);
            }
            if (EstimateExpTotal != null && EstimateExpTotal != "")
            {
                JobMasterTempObj.EstimateValue_ExpTotal = Convert.ToDecimal(EstimateExpTotal);
            }
            if (EstimateProfitTotal != null && EstimateProfitTotal != "")
            {
                JobMasterTempObj.EstimateValue_Profit_EstTotal = Convert.ToDecimal(EstimateProfitTotal);
            }
            if (EstimateOverride != null && EstimateOverride != "")
            {
                JobMasterTempObj.Estimate_Override = Convert.ToDecimal(EstimateOverride);
            }

            //Job Total Values
            if (JobExpensesTotal != null && JobExpensesTotal != "")
            {
                JobMasterTempObj.Job_ExpenseTotal = Convert.ToDecimal(JobExpensesTotal);
            }
            if (JobEstimateTotal != null && JobEstimateTotal != "")
            {
                JobMasterTempObj.Job_EstimatedTotal = Convert.ToDecimal(JobEstimateTotal);
            }
            if (JobActualTotal != null && JobActualTotal != "")
            {
                JobMasterTempObj.Job_ActualTotal = Convert.ToDecimal(JobActualTotal);

            }
            if (JobProfitLossTotal != null && JobProfitLossTotal != "")
            {
                JobMasterTempObj.Profit_Loss_Total = Convert.ToDecimal(JobProfitLossTotal);
            }
               
            List<Assembly_MasterInfo> assembliesPartsList = new List<Assembly_MasterInfo>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            ServiceResultList<Assembly_MasterInfo> AssembliesDetails = new ServiceResultList<Assembly_MasterInfo>();
            AssembliesDetails = AssembliesResponse.GetNewAssembliesListInJobs(result.Client_ID);
            List<Assembly_MasterInfo> selectedParts = TempData["JobAssembliesParts"] as List<Assembly_MasterInfo> ?? new List<Assembly_MasterInfo>();
            for (int i = 0; i < AssembliesDetails.ListData.Count; i++)
            {
                if (selectedParts.Count > 0)
                {
                    foreach (Assembly_MasterInfo list in selectedParts)
                    {
                        if (list.Assemblies_Name.Trim() == AssembliesDetails.ListData[i].Assemblies_Name.Trim())
                            AssembliesDetails.ListData[i].isChekced = true;
                    }
                }
                assembliesPartsList.Add(new Assembly_MasterInfo{ Assemblies_Name = AssembliesDetails.ListData[i].Assemblies_Name, Assemblies_Category = AssembliesDetails.ListData[i].Assemblies_Category, Assemblies_Description = AssembliesDetails.ListData[i].Assemblies_Description, isChekced = AssembliesDetails.ListData[i].isChekced});
            }
            return View("AddNewAssembliesPartsListPopUp",AssembliesDetails.ListData);
        }
        #endregion

        #region Job Assemblies Parts
        public ActionResult JobAssembliesPartsDetails(FormCollection form)
        {
            List<Assembly_MasterInfo> JobAssembliesParts = new List<Assembly_MasterInfo>();
            List<Assembly_MasterInfo> JobAssembliesLabor = new List<Assembly_MasterInfo>();
            string[] selectedValue = form.GetValues("assignChkBx");
            if (selectedValue != null && selectedValue.Count()!=0)
            {
                for(int i=0;i<selectedValue.Count();i++)
                {
                    string checkedValues = selectedValue[i];
                    string[] splitData = checkedValues.Split(',');
                    AssembliesMasterBLL AssembliesDetailsResponse = new AssembliesMasterBLL();
                    ServiceResultList<Assembly_MasterInfo> assembliesDetails = new ServiceResultList<Assembly_MasterInfo>();
                    assembliesDetails = AssembliesDetailsResponse.GetJobAssembliesDetailsList(Convert.ToInt32(splitData[0]), splitData[1]);
                    //add parts details in jobs for webgrid
                    foreach (Assembly_MasterInfo assembliesList in assembliesDetails.ListData)
                    {
                        JobAssembliesParts.Add(new Assembly_MasterInfo{Assemblies_Name = assembliesList.Assemblies_Name, Assemblies_Category = assembliesList.Assemblies_Category, Part_Number = assembliesList.Part_Number, Part_Cost = assembliesList.Part_Cost, Estimated_Qty=assembliesList.Estimated_Qty, isChekced=true});
                    }
                    //add labor details in jobs for webgrid.
                    foreach (Assembly_MasterInfo assembliesList in assembliesDetails.ListData)
                    {
                        string assembleName = splitData[1].ToString();
                        if (!JobAssembliesLabor.Exists(s=>s.Assemblies_Name.Contains(assembleName)))
                        {
                            JobAssembliesLabor.Add(new Assembly_MasterInfo{Assemblies_Name = assembliesList.Assemblies_Name, Assemblies_Category = assembliesList.Assemblies_Category, labor_cost = assembliesList.labor_cost, labor_ResaleCost = assembliesList.labor_ResaleCost, labor_EstimatedHours = assembliesList.labor_EstimatedHours});
                        }
                    }
                    //assembliesDetails = AssembliesDetailsResponse.GetJobAssembliesLaborDetailsList(Convert.ToInt32(splitData[0]), splitData[1]);
                }
            }
           TempData["JobAssembliesParts"] = JobAssembliesParts;
           TempData["JobAssembliesLabor"] = JobAssembliesLabor;

           Job_MasterInfo model = new Job_MasterInfo();
           model.Job_ID = JobMasterTempObj.Job_ID;
           model.Job_Status = JobMasterTempObj.Job_Status;
           model.Job_Description = JobMasterTempObj.Job_Description;

           model.Client_Name = JobMasterTempObj.Client_Name;
           model.Other_ClientName = JobMasterTempObj.Other_ClientName;
           model.Client_ContactPerson = JobMasterTempObj.Client_ContactPerson;
           model.Client_Address = JobMasterTempObj.Client_Address;
           model.Client_Address2 = JobMasterTempObj.Client_Address2;
           model.Client_City = JobMasterTempObj.Client_City;
           model.Client_State = JobMasterTempObj.Client_State;
           model.Client_ZipCode = JobMasterTempObj.Client_ZipCode;
           model.Client_Fax = JobMasterTempObj.Client_Fax;
           model.Client_Email = JobMasterTempObj.Client_Email;
           model.Client_Phone = JobMasterTempObj.Client_Phone;
           model.Client_Mobile = JobMasterTempObj.Client_Mobile;
           model.Previous_Location = JobMasterTempObj.Previous_Location;
           model.Other_Previous_Location = JobMasterTempObj.Other_Previous_Location;
           model.Work_Address = JobMasterTempObj.Work_Address;
           model.Work_Address2 = JobMasterTempObj.Work_Address2;
           model.Work_City = JobMasterTempObj.Work_City;
           model.Work_State = JobMasterTempObj.Work_State;
           model.Directions_To = JobMasterTempObj.Directions_To;
           model.Doing_What = JobMasterTempObj.Doing_What;
           //Mileage Table Values
           model.Job_Mileage_Estimated = JobMasterTempObj.Job_Mileage_Estimated;
           model.Job_Mileage_Actual = JobMasterTempObj.Job_Mileage_Actual;
           model.Job_Mileage_Cost = JobMasterTempObj.Job_Mileage_Cost;
           model.Job_Mileage_ExpTotal = JobMasterTempObj.Job_Mileage_ExpTotal;
           model.Job_Mileage_EstTotal = JobMasterTempObj.Job_Mileage_EstTotal;
           model.Job_Mileage_ActTotal = JobMasterTempObj.Job_Mileage_ActTotal;
           //Estimate Values
           model.EstimateValue_Overhead_ExpTotal = JobMasterTempObj.EstimateValue_Overhead_ExpTotal;
           model.EstimateValue_Profit_Percentage = JobMasterTempObj.EstimateValue_Profit_Percentage;
           model.EstimateValue_SubTotal = JobMasterTempObj.EstimateValue_SubTotal;
           model.EstimateValue_ExpTotal = JobMasterTempObj.EstimateValue_ExpTotal;
           model.EstimateValue_Profit_EstTotal = JobMasterTempObj.EstimateValue_Profit_EstTotal;
           model.Estimate_Override = JobMasterTempObj.Estimate_Override;
           //Job Total Values
           model.Job_ExpenseTotal = JobMasterTempObj.Job_ExpenseTotal;
           model.Job_EstimatedTotal = JobMasterTempObj.Job_EstimatedTotal;
           model.Job_ActualTotal = JobMasterTempObj.Job_ActualTotal;
           model.Profit_Loss_Total = JobMasterTempObj.Profit_Loss_Total;
           return View("AddJobDetails", model);
        }
        #endregion
        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                file.SaveAs(Server.MapPath("~/Update/" + file.FileName));
            }

            return View();
        }

        public ActionResult JobList()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            ServiceResultList<Job_MasterInfoList> response = new ServiceResultList<Job_MasterInfoList>();
            response = JobResponse.GetJobDetailList(result.Client_ID);


            return View(response.ListData);
        }

        public ActionResult EditJobDetails(int JobID)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            JobMasterBLL JobResponse = new JobMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            ServiceResult<Job_MasterInfo> response = new ServiceResult<Job_MasterInfo>();
            response = JobResponse.GetJobDetails(JobID,result.Client_ID);
            ViewBag.EditJobDetails = "EditJobDetails";
            TempData["JobAssembliesParts"] = response.Data.NewJobAssemblyPartsList;
            TempData["JobAssembliesLabor"] = response.Data.NewJobAssemblyLaborList;
            TempData["MyPartsInJob"] = response.Data.NewPartsListInJob;
            TempData["MyLaborListInJob"] = response.Data.NewLabourListInJob;
            TempData["MyLegalList"] = response.Data.NewLegalListInJob;

            return View(response.Data);
        }
    }
}
