using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.Master;
using ElectricEase.BLL.PartsMaster;
using ElectricEase.Data.DataBase;
using ElectricEase.Data.PartsMaster;
using ElectricEase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using log4net;
using ClosedXML.Excel;

namespace ElectricEase.Web.Controllers
{
    public class DistributorPartsController : Controller
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(DistributorPartsController));
        PartsMasterBLL PartsResponse = new PartsMasterBLL();
        //
        // GET: /DistributorParts/

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResultList<distributordropdown> distributorlist = new ServiceResultList<distributordropdown>();
            distributorlist = ClientResponse.distributorList();
            if (distributorlist.ListData.Count > 0)
            {
                var itemToRemove = distributorlist.ListData.FirstOrDefault(r => r.value == -1);
                if (itemToRemove != null)
                {
                    distributorlist.ListData.Remove(itemToRemove);
                }
            }
            ViewBag.Distributor = new SelectList(distributorlist.ListData, "value", "Name");
            Parts_DetailsInfo model = new Parts_DetailsInfo();
            List<PartsCategoryList> masterpartslist = new List<PartsCategoryList>();
            List<PartsCategoryList> partslist = new List<PartsCategoryList>();
            PartsCategoryList parts = new PartsCategoryList();
            masterpartslist = new MasterServiceBLL().GetMyDistributorPartsCategoryList();
            masterpartslist = masterpartslist.OrderBy(a => a.Part_Category).ToList();
            partslist = masterpartslist.Select(a => new PartsCategoryList
            {
                Part_Category = a.Part_Category,
                Name = a.Name,
                Value = a.Value
            }).ToList();
            parts.Part_Category = "other";
            partslist = partslist.OrderBy(a => a.Part_Category).ToList();
            partslist.Insert(0, parts);
            ViewBag.PartCategory = new SelectList(partslist, "Part_Category", "Part_Category");
            List<PartsCategoryList> categories = masterpartslist.Select(a => new PartsCategoryList
            {
                Part_Category = a.Part_Category,
                Name = a.Name,
                Value = a.Value
            }).ToList();
            parts = new PartsCategoryList();
            parts.Part_Category = "ALL Parts Category";
            categories.Insert(0, parts);
            ViewBag.FilterParts = new SelectList(categories, "Part_Category", "Part_Category");
            return View(model);
        }
        public ActionResult EditPartsDetails(string PartsNumber = "", int D_Id = 0)
        {
            ServiceResult<Parts_DetailsInfo> response = new ServiceResult<Parts_DetailsInfo>();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            //ViewBag.Clientid = new SelectList(new MasterServiceBLL().GetMyClientList(), "Client_ID", "Client_Company");
            List<PartsCategoryList> partslist = new List<PartsCategoryList>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            var result = ClientResponse.GetClientName(Loginuser);
            PartsCategoryList parts = new PartsCategoryList();
                        
            response = PartsResponse.GetMyDistbutorPartsDetails(PartsNumber, D_Id);

            partslist = new MasterServiceBLL().GetMyDistributorPartsCategoryList();
            partslist = partslist.OrderBy(a => a.Part_Category).ToList();
            parts.Part_Category = "other";
            partslist.Insert(0, parts);
            response.Data.partcatgoryList = partslist;// new SelectList(partslist, "Part_Category", "Part_Category");

            ServiceResultList<distributordropdown> distributorlist = new ServiceResultList<distributordropdown>();
            distributorlist = ClientResponse.distributorList();
            response.Data.distributorList = distributorlist.ListData;
            ViewBag.Distributor = new SelectList(distributorlist.ListData, "value", "Name");

            if (response.ResultCode > 0)
            {
                ViewBag.Edit = "EditPartsDetails";
                //response.Data.partcatgoryList = partslist;
                return Json(response.Data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.EditSavedErrorMsg = response.Message;
                ViewBag.Edit = "EditPartsDetails";
                // response.Data.partcatgoryList = partslist;
                return Json(response.Data, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckIsExistPartNumber(Parts_DetailsInfo Model)
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            if (Loginuser != "Admin")
            {
                //Model.Client_ID = result.Client_ID;
            }
            response = PartsResponse.CheckIsDistributorNewPartNumber(Model.DistributorID, Model.Part_Number.Trim());
            if (response.Data == true)
            {
                return Json("Already Exists");
            }
            else
            {
                return Json("New");
            }
        }
        public ActionResult AddPartsDetails(Parts_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            if (Model.OtherPart_Category != null)
            {
                Model.Part_Category = Model.OtherPart_Category;
            }
            if (Loginuser == "Admin")
            {
                Model.Created_By = Loginuser;
            }
            else
            {
                var result = ClientResponse.GetClientName(Loginuser);
                Model.Created_By = Loginuser;
                //Model.Client_ID = result.Client_ID;
            }
            response = PartsResponse.AddNewDistributorPartsDetail(Model);
            if (response.ResultCode > 0)
            {
                TempData["ADDSuccessMsg"] = response.Message;
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["ADDFailMsg"] = response.Message;
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PartsList(string PartCatgory = "")
        {
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            PartsCategoryList parts = new PartsCategoryList();
            List<PartsCategoryList> partslist = new List<PartsCategoryList>();
            partslist = new MasterServiceBLL().GetDistributorPartsCategoryList();
            parts.Part_Category = "ALL Parts Category";
            partslist.Add(parts);
            ViewBag.PartCategory = new SelectList(partslist, "Part_Category", "Part_Category");
            response = PartsResponse.GetDistributorPartsDetalisList(PartCatgory);
            return PartialView("_DistributorPartsList", response.ListData);
        }

        public JsonResult PartsDatatable(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var result = GetDistributorPartsDetalis(model, out filteredResultsCount, out totalResultsCount);


            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            });
        }

        public IList<Parts_DetailsInfoList> GetDistributorPartsDetalis(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
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
            sortBy = sortBy == "ID" ? "Created_Date" : (sortBy == "Company" ? "Distributor_Master.Company" : sortBy);
            // search the dbase taking into consideration table sorting and paging
            model.extra_search = (model.extra_search != null && model.extra_search.Trim() == "") ? model.extra_search = null : model.extra_search;
            var result = PartsResponse.GetDistributorPartsData(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, model.extra_search);
            if (result == null)
            {
                // empty collection...
                return new List<Parts_DetailsInfoList>();
            }
            return result;
        }

        [HttpPost]
        public ActionResult DeletePartsDetails(string PartsNumber = "", string DistributorID = "")
        {
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            if (PartsNumber.Trim() != "0" && DistributorID.Trim() != "0")
            {
                string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                ViewBag.LoginUser = Loginuser;
                response = PartsResponse.DeleteDistributorPartsDetails(PartsNumber, Convert.ToInt32(DistributorID), Loginuser);
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetMyDistributorPartsCatgory(string Did = "", string partnumber = "")
        {
            List<PartsCategoryList> partslist = new List<PartsCategoryList>();
            ServiceResult<Parts_DetailsInfo> response = new ServiceResult<Parts_DetailsInfo>();
            PartsCategoryList parts = new PartsCategoryList();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();

            if (Did != "" && Did != "0" && Did != "null")
            {
                partslist = new MasterServiceBLL().GetMyDistributorPartsCategoryList(Convert.ToInt32(Did)).OrderBy(a => a.Part_Category).ToList();
                parts.Part_Category = "other";
                partslist.Insert(0, parts);
                ViewBag.PartCategory = new SelectList(partslist, "Part_Category", "Part_Category");
                //if (partnumber != "")
                //{
                //    response = PartsResponse.GetMyDistbutorPartsDetails(partnumber, Convert.ToInt32(Did));
                //    if (response.ResultCode > 0)
                //    {
                //        response.Data.partcatgoryList = partslist;
                //        return Json(response.Data, JsonRequestBehavior.AllowGet);
                //    }
                //    else
                //    {
                //        return Json(partslist, JsonRequestBehavior.AllowGet);
                //    }
                //}
                //else
                //{
                    return Json(partslist, JsonRequestBehavior.AllowGet);

                //}
            }
            else
            {
                parts.Part_Category = "other";

                partslist.Add(parts);
                return Json(partslist, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        //public ActionResult Importparts(int Did)
        //{
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    List<PartsNames> partnumber = new List<PartsNames>();
        //    List<PartsNames> NewParts = new List<PartsNames>();
        //    List<int> assemblyToProcess = new List<int>();
        //    StringBuilder assembliesList = new StringBuilder();
        //    string status = "";
        //    try
        //    {
        //        PartsMasterBLL PartsResponse = new PartsMasterBLL();
        //        if (Request.Files.Count > 0)
        //        {
        //            HttpPostedFileBase file = Request.Files[0];
        //            string filepath = Path.Combine(Server.MapPath("../Upload"), file.FileName);
        //            string ext = Path.GetExtension(filepath);
        //            string invalidparts = "";
        //            int totalpartsimported = 0;
        //            int totalpartsupdated = 0;
        //            int totalnewparts = 0;
        //            int totalfailed = 0;
        //            DataTable dat = new DataTable();
        //            if (ext == ".xlsx")
        //            {
        //                file.SaveAs(filepath);
        //                String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0; HDR=YES; IMEX=1\"", filepath);
        //                using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
        //                {
        //                    List<Parts_Details> partslist = new List<Parts_Details>();
        //                    excelConnection.Open();
        //                    dat = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //                    string Excelsheet = "";
        //                    foreach (DataRow row in dat.Rows)
        //                    {
        //                        Excelsheet = row["TABLE_NAME"].ToString();
        //                    }
        //                    excelConnection.Close();
        //                    if (Excelsheet == "Sheet1$")
        //                    {
        //                        string countcmd = "Select count(*)from [Sheet1$]";
        //                        OleDbCommand cocmd = new OleDbCommand(countcmd, excelConnection);
        //                        excelConnection.Open();
        //                        int rowcount = (int)cocmd.ExecuteScalar();
        //                        excelConnection.Close();
        //                        if (rowcount > 0)
        //                        {
        //                            using (OleDbCommand cmd = new OleDbCommand("Select * from [Sheet1$]", excelConnection))
        //                            {
        //                                excelConnection.Open();
        //                                DataTable dt = new DataTable();
        //                                OleDbDataReader dr = cmd.ExecuteReader();
        //                                dt.Load(dr);
        //                                excelConnection.Close();
        //                                excelConnection.Open();
        //                                using (OleDbDataReader dReader = cmd.ExecuteReader())
        //                                {
        //                                    //logs.WriteLog("Uploading of Parts Started", 0, Loginuser);
        //                                    var count = dt.Columns.Count;
        //                                    if (count == 9)
        //                                    {

        //                                        string Temp1 = dt.Columns[0].ToString();//Part Number
        //                                        string Temp2 = dt.Columns[1].ToString();//Category
        //                                        string Temp3 = dt.Columns[2].ToString();//My Cost
        //                                        string Temp4 = dt.Columns[3].ToString();//Markup
        //                                        string Temp5 = dt.Columns[4].ToString();//Resale
        //                                        string Temp6 = dt.Columns[5].ToString();//Labor Unit
        //                                        string Temp7 = dt.Columns[6].ToString();//Purchased Form
        //                                        // string Temp8 = dt.Columns[7].ToString();
        //                                        string Temp9 = dt.Columns[7].ToString();//My Description
        //                                        string Temp10 = dt.Columns[8].ToString();//UOM
        //                                        if (Temp1 == "Part Number" && Temp2 == "Category" && Temp3 == "My Cost" && Temp4 == "Markup " && Temp5 == "Resale" && Temp6 == "Labor Unit" && Temp7 == "Purchased From " && Temp9 == "My Description " && Temp10 == "UOM")
        //                                        {
        //                                            using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
        //                                            {
        //                                                db.Configuration.AutoDetectChangesEnabled = false;
        //                                                db.Configuration.ValidateOnSaveEnabled = false;
        //                                                while (dReader.Read())
        //                                                {
        //                                                    totalpartsimported++;
        //                                                    Distributor_Parts_Details parts = new Distributor_Parts_Details();
        //                                                    parts.Part_Number = dReader.GetValue(0).ToString().Trim();
        //                                                    parts.Part_Category = dReader.GetValue(1).ToString().Trim();
        //                                                    if (parts.Part_Number != "" && parts.Part_Category != "")
        //                                                    {
        //                                                        if (dReader.GetValue(2) != null && dReader.GetValue(4) != null && dReader.GetValue(2) != DBNull.Value && dReader.GetValue(4) != DBNull.Value)
        //                                                        {
        //                                                            //if (Convert.ToDecimal(dReader.GetValue(2)) < Convert.ToDecimal(dReader.GetValue(4)))
        //                                                            //{
        //                                                            if (dReader.GetValue(2).ToString().Trim() == "")
        //                                                            {
        //                                                                parts.Cost = 0;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                parts.Cost = Convert.ToDecimal(dReader.GetValue(2));
        //                                                            }
        //                                                            if (dReader.GetValue(4).ToString().Trim() == "")
        //                                                            {
        //                                                                parts.Resale_Cost = 0;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                parts.Resale_Cost = Convert.ToDecimal(dReader.GetValue(4));
        //                                                            }

        //                                                            //Labor Unit Calculation
        //                                                            if (string.IsNullOrEmpty(dReader.GetValue(5).ToString().Trim()))
        //                                                            {
        //                                                                parts.LaborUnit = 0;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                parts.LaborUnit = Convert.ToDecimal(dReader.GetValue(5));
        //                                                            }

        //                                                            //parts.Resale_Cost = Convert.ToDecimal(dReader.GetValue(3));
        //                                                            parts.Purchased_From = dReader.GetValue(6).ToString().Trim();
        //                                                            parts.Description = dReader.GetValue(7).ToString().Trim();
        //                                                            // parts.Client_Description = dReader.GetValue(7).ToString();
        //                                                            parts.UOM = dReader.GetValue(8).ToString().Trim();
        //                                                            parts.Distributor_ID = Did;
        //                                                            parts.Client_Id = 1;
        //                                                            if (Loginuser != "Admin")
        //                                                            {
        //                                                                parts.Created_By = result.User_ID;
        //                                                                parts.Updated_By = result.User_ID;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                parts.Created_By = "Admin";
        //                                                                parts.Updated_By = "Admin";
        //                                                            }

        //                                                            //New validations
        //                                                            if (parts.Part_Number.Length > 50)
        //                                                            {
        //                                                                _log.Info(parts.Part_Number + " - Failed due to length beyond 50 characters.");
        //                                                                totalfailed++;
        //                                                                continue;
        //                                                            }

        //                                                            if (parts.Part_Category.Length > 500)
        //                                                            {
        //                                                                _log.Info(parts.Part_Number + " - Failed due to part category length beyond 500 characters.");
        //                                                                totalfailed++;
        //                                                                continue;
        //                                                            }

        //                                                            if (parts.Description.Length > 1000)
        //                                                            {
        //                                                                _log.Info(parts.Part_Number + " - Failed due to part description length beyond 1000 characters.");
        //                                                                totalfailed++;
        //                                                                continue;
        //                                                            }

        //                                                            if (parts.Purchased_From.Length > 500)
        //                                                            {
        //                                                                _log.Info(parts.Part_Number + " - Failed due to 'Purchased From' length beyond 500 characters.");
        //                                                                totalfailed++;
        //                                                                continue;
        //                                                            }

        //                                                            if (parts.UOM.Length > 150)
        //                                                            {
        //                                                                _log.Info(parts.Part_Number + " - Failed due to 'UOM' length beyond 150 characters.");
        //                                                                totalfailed++;
        //                                                                continue;
        //                                                            }

        //                                                            //Cost validation
        //                                                            string numericString = Convert.ToString(parts.Cost);
        //                                                            int length = numericString.Substring(numericString.IndexOf(".") + 1).Length;
        //                                                            if (length > 2)
        //                                                            {
        //                                                                _log.Info(parts.Part_Number + " - Failed due to 'Cost' decimal length beyond 2 numbers.");
        //                                                                totalfailed++;
        //                                                                continue;
        //                                                            }

        //                                                            length = Math.Truncate(parts.Cost ?? 0).ToString().Length;
        //                                                            if (length > 8)
        //                                                            {
        //                                                                _log.Info(parts.Part_Number + " - Failed due to 'Cost' whole number length beyond 8 numbers.");
        //                                                                totalfailed++;
        //                                                                continue;
        //                                                            }

        //                                                            //Resale Cost validation
        //                                                            numericString = Convert.ToString(parts.Resale_Cost);
        //                                                            length = numericString.Substring(numericString.IndexOf(".") + 1).Length;
        //                                                            if (length > 2)
        //                                                            {
        //                                                                _log.Info(parts.Part_Number + " - Failed due to 'Resale Cost' decimal length beyond 2 numbers.");
        //                                                                totalfailed++;
        //                                                                continue;
        //                                                            }

        //                                                            length = Math.Truncate(parts.Resale_Cost ?? 0).ToString().Length;
        //                                                            if (length > 8)
        //                                                            {
        //                                                                _log.Info(parts.Part_Number + " - Failed due to 'Resale Cost' whole number length beyond 8 numbers.");
        //                                                                totalfailed++;
        //                                                                continue;
        //                                                            }

        //                                                            //Labor Unit validation
        //                                                            numericString = Convert.ToString(parts.LaborUnit);
        //                                                            length = numericString.Substring(numericString.IndexOf(".") + 1).Length;
        //                                                            if (length > 4)
        //                                                            {
        //                                                                _log.Info(parts.Part_Number + " - Failed due to 'Labor Unit' decimal length beyond 4 numbers.");
        //                                                                totalfailed++;
        //                                                                continue;
        //                                                            }

        //                                                            length = Math.Truncate(parts.LaborUnit ?? 0).ToString().Length;
        //                                                            if (length > 6)
        //                                                            {
        //                                                                _log.Info(parts.Part_Number + " - Failed due to 'Labor Unit' whole number length beyond 6 numbers.");
        //                                                                totalfailed++;
        //                                                                continue;
        //                                                            }

        //                                                            Distributor_Parts_Details dbparts = db.Distributor_Parts_Details.Where(x => x.Part_Number == parts.Part_Number && x.Distributor_ID == Did).FirstOrDefault();
        //                                                            if (dbparts != null)
        //                                                            {
        //                                                                dbparts.Part_Category = parts.Part_Category;
        //                                                                dbparts.Cost = parts.Cost;
        //                                                                dbparts.Resale_Cost = parts.Resale_Cost;
        //                                                                dbparts.LaborUnit = parts.LaborUnit;
        //                                                                dbparts.Client_Description = parts.Client_Description;
        //                                                                dbparts.Updated_Date = DateTime.Now;
        //                                                                dbparts.Updated_By = parts.Updated_By;
        //                                                                dbparts.Purchased_From = parts.Purchased_From;
        //                                                                dbparts.Description = parts.Description;
        //                                                                dbparts.UOM = parts.UOM;
        //                                                                if (parts.Part_Category != "" && parts.Part_Category != null && parts.Part_Number != "")
        //                                                                {
        //                                                                    PartsNames updatepartname = new PartsNames();
        //                                                                    updatepartname.Partnumber = parts.Part_Number;
        //                                                                    updatepartname.Category = parts.Part_Category;
        //                                                                    updatepartname.Cost = Convert.ToString(parts.Cost);
        //                                                                    updatepartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                                    updatepartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                                    updatepartname.Comments = "<span style='color:#4081bd'>“Part” is updated successfully.</span>";
        //                                                                    partnumber.Add(updatepartname);

        //                                                                    //To updated the existing Nationwide Assembly Parts
        //                                                                    List<Distributor_Assemblies_Parts> assemblyPartsList = db.Distributor_Assemblies_Parts.Where(x => x.Part_Number == parts.Part_Number).ToList();

        //                                                                    foreach (Distributor_Assemblies_Parts assemblyPart in assemblyPartsList)
        //                                                                    {
        //                                                                        assemblyPart.Part_Category = parts.Part_Category;
        //                                                                        assemblyPart.Part_Cost = parts.Cost ?? 0;
        //                                                                        assemblyPart.Part_Resale = parts.Resale_Cost ?? 0;
        //                                                                        assemblyPart.LaborUnit = parts.LaborUnit;
        //                                                                        assemblyPart.EstimatedCost_Total = (parts.Cost ?? 0 * assemblyPart.Estimated_Qty);
        //                                                                        assemblyPart.EstimatedResale_Total = (parts.Resale_Cost * assemblyPart.Estimated_Qty) ?? 0;

        //                                                                        //To get the assemblies to be processed
        //                                                                        assemblyToProcess.Add(assemblyPart.AssemblyID);
        //                                                                    }
        //                                                                    totalpartsupdated++;
        //                                                                }
        //                                                                else
        //                                                                {
        //                                                                    _log.Info("Import Failed for the Part Number - " + parts.Part_Number);
        //                                                                    _log.Info("Part number or Part Category missing!");
        //                                                                    invalidparts += parts.Part_Number + "<br/>";
        //                                                                    PartsNames Addpartname = new PartsNames();
        //                                                                    Addpartname.Partnumber = parts.Part_Number;
        //                                                                    Addpartname.Category = parts.Part_Category;
        //                                                                    Addpartname.Cost = Convert.ToString(parts.Cost);
        //                                                                    Addpartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                                    Addpartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                                    Addpartname.Comments = "<span style='color:#f00'>Failed:Invalid \"Part number\" or \"Part Category\" missing!</span>";
        //                                                                    partnumber.Add(Addpartname);
        //                                                                    totalfailed++;
        //                                                                }
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                parts.IsActive = true;
        //                                                                parts.Created_Date = DateTime.Now;
        //                                                                parts.Updated_Date = DateTime.Now;
        //                                                                parts.Distributor_ID = Did;
        //                                                                parts.Client_Id = 1;
        //                                                                if (parts.Part_Category != "" && parts.Part_Category != null && parts.Part_Number != "")
        //                                                                {
        //                                                                    //To validate, whether this new parts already inserted in current import process
        //                                                                    PartsNames oldPartToInsert = NewParts.SingleOrDefault(s => s.Partnumber.ToLower() == parts.Part_Number.ToLower());
        //                                                                    if (oldPartToInsert != null)
        //                                                                    {
        //                                                                        _log.Info("Import Failed for the Part Number - " + parts.Part_Number + " due to the duplication when it is inserting for the first time");
        //                                                                        totalfailed++;
        //                                                                        continue;
        //                                                                    }

        //                                                                    db.Distributor_Parts_Details.Add(parts);

        //                                                                    totalnewparts++;
        //                                                                    PartsNames Addpartname = new PartsNames();
        //                                                                    Addpartname.Partnumber = parts.Part_Number;
        //                                                                    Addpartname.Category = parts.Part_Category;
        //                                                                    Addpartname.Cost = Convert.ToString(parts.Cost);
        //                                                                    Addpartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                                    Addpartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                                    Addpartname.Comments = "<span style='color:#1eca0b'>“New Part” is added successfully.</span>";
        //                                                                    partnumber.Add(Addpartname);
        //                                                                    NewParts.Add(Addpartname);
        //                                                                }
        //                                                                else
        //                                                                {
        //                                                                    _log.Info("Import Failed for the Part Number - " + parts.Part_Number);
        //                                                                    _log.Info("Part number or Part Category missing!");
        //                                                                    invalidparts += parts.Part_Number + "<br/>";
        //                                                                    PartsNames Addpartname = new PartsNames();
        //                                                                    Addpartname.Partnumber = parts.Part_Number;
        //                                                                    Addpartname.Category = parts.Part_Category;
        //                                                                    Addpartname.Cost = Convert.ToString(parts.Cost);
        //                                                                    Addpartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                                    Addpartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                                    Addpartname.Comments = "<span style='color:#f00'>Failed:Invalid \"Part number\" or \"Part Category\" missing!</span>";
        //                                                                    partnumber.Add(Addpartname);
        //                                                                    totalfailed++;
        //                                                                }
        //                                                            }

        //                                                            //}
        //                                                            //else
        //                                                            //{
        //                                                            //    invalidparts += parts.Part_Number + "(Check the “Cost” and “Resale Cost” have values.), ";
        //                                                            //    PartsNames Addpartname = new PartsNames();
        //                                                            //    Addpartname.Partnumber = parts.Part_Number;
        //                                                            //    Addpartname.Category = parts.Part_Category;
        //                                                            //    Addpartname.Cost = Convert.ToString(dReader.GetValue(2));
        //                                                            //    Addpartname.Resale = Convert.ToString(dReader.GetValue(4));
        //                                                            //    Addpartname.Comments = "<span style='color:#f00'>Failed:check resale cost is less then cost or resale cost equal to cost</span>";
        //                                                            //    partnumber.Add(Addpartname);
        //                                                            //    totalfailed++;
        //                                                            //}
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            _log.Info("Import Failed for the Part Number - " + parts.Part_Number);
        //                                                            _log.Info("Cost or Resale cost is missing!");
        //                                                            invalidparts += parts.Part_Number + "(Check the “Cost” and “Resale Cost” have values.), ";
        //                                                            PartsNames Addpartname = new PartsNames();
        //                                                            Addpartname.Partnumber = parts.Part_Number;
        //                                                            Addpartname.Category = parts.Part_Category;
        //                                                            Addpartname.Cost = Convert.ToString(dReader.GetValue(2));
        //                                                            Addpartname.Resale = Convert.ToString(dReader.GetValue(4));
        //                                                            Addpartname.LaborUnit = Convert.ToString(dReader.GetValue(5));
        //                                                            Addpartname.Comments = "<span style='color:#f00'>Failed:Check the “Cost” and “Resale Cost” have values. Not Null</span>";
        //                                                            partnumber.Add(Addpartname);
        //                                                            totalfailed++;
        //                                                        }
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        _log.Info(parts.Part_Number + " - Failed due to Part Number or Part Cateogry is null.");
        //                                                        PartsNames Addpartname = new PartsNames();

        //                                                        Addpartname.Partnumber = parts.Part_Number;
        //                                                        Addpartname.Category = parts.Part_Category;
        //                                                        Addpartname.Cost = Convert.ToString(dReader.GetValue(2));
        //                                                        Addpartname.Resale = Convert.ToString(dReader.GetValue(4));
        //                                                        Addpartname.LaborUnit = Convert.ToString(dReader.GetValue(5));
        //                                                        Addpartname.Comments = "<span style='color:#f00'>Failed:“Part Number” is missing!</span>";
        //                                                        partnumber.Add(Addpartname);
        //                                                        totalfailed++;
        //                                                    }
        //                                                    //partslist.Add(partsdetail);
        //                                                }

        //                                                //To save all the database operations at final
        //                                                db.Configuration.AutoDetectChangesEnabled = true;
        //                                                db.SaveChanges();

        //                                                //To remove the duplicate assembly Ids
        //                                                assemblyToProcess = assemblyToProcess.Distinct().ToList();
        //                                                foreach (int assemly in assemblyToProcess)
        //                                                {
        //                                                    assembliesList.Append(assemly);
        //                                                    assembliesList.Append(',');
        //                                                }

        //                                                //To remove last comma in the list of processed assemblies
        //                                                if (assembliesList.Length > 0)
        //                                                {
        //                                                    assembliesList.Remove((assembliesList.Length - 1), 1);
        //                                                }

        //                                                //To update the labor unit on Client Assembly Master
        //                                                db.Database.ExecuteSqlCommand("exec EE_RecalcDistributorAssembly @AssemblyList, @DistributorID, @UpdatedBy",
        //                                                    new SqlParameter("AssemblyList", assembliesList.ToString()),
        //                                                    new SqlParameter("DistributorID", Did),
        //                                                    new SqlParameter("UpdatedBy", "Admin Excel"));

        //                                                //db.Database.SqlQuery<int>("exec EE_RecalcDistributorAssembly @AssemblyList", new SqlParameter("AssemblyList", assembliesList));
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            status = "Invalid parts template!";
        //                                            return Json(status);
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        status = "Invalid parts template!";
        //                                        return Json(status);
        //                                    }
        //                                }
        //                            }
        //                            string partreporttable = "";
        //                            //if (partnumber.Count > 0)
        //                            //{
        //                            //    partreporttable += "<div class='table-responsive' id='partsreport'><table class='table table-bordered' cellspacing='0' width='100%'><thead><tr><th>PartNumber</th><th>Category</th><th>Cost</th><th>ResaleCost</th><th>Comment</th></tr></thead><tbody>";
        //                            //    foreach (var par in partnumber)
        //                            //    {
        //                            //        partreporttable += "<tr>";
        //                            //        partreporttable += "<td>" + par.Partnumber + "</td>";
        //                            //        partreporttable += "<td>" + par.Category + "</td>";
        //                            //        partreporttable += "<td>" + par.Cost + "</td>";
        //                            //        partreporttable += "<td>" + par.Resale + "</td>";
        //                            //        partreporttable += "<td>" + par.Comments + "</td>";
        //                            //        partreporttable += "</tr>";
        //                            //    }
        //                            //    partreporttable += "</tbody></table></div>";
        //                            //}
        //                            status = "<u>\"Parts\" are imported successfully.</u>: <br> Total Parts Imported :" + totalpartsimported + " ;  Newly added Parts :" + totalnewparts + ";  Parts Updated :" + totalpartsupdated + "; Failed Parts:" + totalfailed + " <br><br>" + partreporttable + "";
        //                            //status = "\"Parts\" are uploaded successfully. [Total Parts Imported- " + totalpartsimported + " ; Total Parts Added - " + totalpartsupdated + ";<br>Failed parts-" + totalfailed + ";  ]";
        //                            //if (invalidparts != "" && invalidparts.Length > 1)
        //                            //{
        //                            //    invalidparts = invalidparts.Substring(0, invalidparts.Length - 1);
        //                            //    status += "<br/><br/> (Parts Category is missing for some parts:<b> <br/> " + invalidparts + "<br/></b>So please check these parts and try again, other parts are added successfully)";
        //                            //}
        //                        }
        //                        else
        //                        {
        //                            status = "In excel \"No\" records are available!";
        //                        }
        //                        //Create OleDbCommand to fetch data from Excel 

        //                        //logs.WriteLog("Parts are Uploaded Successfully", Client_ID, Loginuser);
        //                    }
        //                    else
        //                    {
        //                        status = "Invalid parts template!";
        //                        return Json(status);
        //                    }
        //                }
        //                System.IO.File.Delete(filepath);
        //            }
        //            else
        //            {
        //                status = "Please import the valid template, the file should be \"xlsx\" format only!";
        //            }
        //        }
        //        return Json(status);
        //    }
        //    catch (Exception ex)
        //    {
        //        // logs.WriteLog("[Exception] While uploading the Parts [Error Msg - " + ex.Message, Client_ID, Loginuser);
        //        return Json(ex.Message);
        //    }
        //}

        [HttpPost]
        public ActionResult ImportParts(int distributorID)
        {
            _log.Info("ImportParts - Starts");

            //declarations
            string returnText = string.Empty;
            DataTable dt = new DataTable();
            List<string> assemblyToProcess = new List<string>();
            StringBuilder assembliesList = new StringBuilder();
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string filepath = Path.Combine(Server.MapPath("../Upload"), file.FileName);
                    string ext = Path.GetExtension(filepath);

                    if (ext == ".xlsx")
                    {
                        file.SaveAs(filepath);
                        String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0; HDR=YES; IMEX=1\"", filepath);
                        using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
                        {
                            //Create OleDbCommand to fetch data from Excel 
                            using (OleDbDataAdapter da = new OleDbDataAdapter("Select * from [Sheet1$]", excelConnection))
                            {
                                excelConnection.Open();

                                dt.Columns.Add("Part Number", typeof(string));
                                dt.Columns.Add("Category", typeof(string));
                                dt.Columns.Add("My Cost", typeof(string));
                                dt.Columns.Add("Markup ", typeof(string));
                                dt.Columns.Add("Resale", typeof(string));
                                dt.Columns.Add("Labor Unit", typeof(string));
                                dt.Columns.Add("Purchased From", typeof(string));
                                dt.Columns.Add("My Description", typeof(string));
                                dt.Columns.Add("UOM", typeof(string));
                                da.Fill(dt);

                                //Adding Row Number
                                DataTable dtExcelParts = new DataTable(dt.TableName);
                                DataColumn dc = new DataColumn("RowNumber");
                                dc.AutoIncrement = true;
                                dc.AutoIncrementSeed = 1;
                                dc.AutoIncrementStep = 1;
                                dc.DataType = typeof(Int32);
                                dtExcelParts.Columns.Add(dc);
                                dc.SetOrdinal(0);
                                dtExcelParts.BeginLoadData();
                                DataTableReader dtReader = new DataTableReader(dt);
                                dtExcelParts.Load(dtReader);
                                dtExcelParts.EndLoadData();

                                //Adding Guid
                                Guid id = Guid.NewGuid();
                                dc = new DataColumn("ProcessID", typeof(String));
                                dc.DefaultValue = id;
                                dtExcelParts.Columns.Add(dc);
                                dc.SetOrdinal(0);

                                DataTable dtImportPartsReport = PartsResponse.ImportDistributorParts(dtExcelParts, distributorID);
                                Session.Add("dtImportPartsReport", dtImportPartsReport);

                                int totalRows = dtImportPartsReport.Rows.Count;
                                int failedRows = dtImportPartsReport.Select("Status = 'Error'").Length;
                                int insertedRows = dtImportPartsReport.Select("Status = 'Inserted'").Length;
                                int updatedRows = dtImportPartsReport.Select("Status = 'Updated'").Length;

                                returnText = "Total Parts Imported: " + totalRows + "; Newly added Parts: " + insertedRows + "; Parts Updated : "
                                    + updatedRows + "; Failed Parts: " + failedRows;
                            }
                        }
                    }
                    else
                    {
                        returnText = "Error: File extension is not correct!";
                        _log.Error(returnText);
                        _log.Info("ImportParts - Exits");
                    }
                }
                else
                {
                    returnText = "Error: No file exists!";
                    _log.Error(returnText);
                    _log.Info("ImportParts - Exits");
                }
                return Json(returnText);
            }
            catch (Exception ex)
            {
                returnText = "Error: " + ex.Message;
                _log.Error(ex.Message);
                return Json(returnText);
            }
        }

        public void GenerateExcelReport()
        {
            _log.Info("GenerateExcelReport - Enters");
            try
            {
                DataTable dt = Session["dtImportPartsReport"] as DataTable;

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Customers");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=DistributorPartsImport-Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
            }
            _log.Info("GenerateExcelReport - Exits");
        }
    }
}
