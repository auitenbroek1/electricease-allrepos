using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.BLL.PartsMaster;
using ElectricEase.Models;
using ElectricEase.Helpers;
using System.IO;
using System.Web.Security;
using ElectricEase.BLL.ClientMaster;
using System.Data.OleDb;
using ElectricEase.Data.DataBase;
using ElectricEase.BLL.Master;
using ElectricEase.Data;
using ElectricEase.Data.PartsMaster;
using System.Text;
using System.Data.SqlClient;
using log4net;
using System.Configuration;
using System.Data;
using ClosedXML.Excel;

namespace ElectricEase.Web.Controllers
{
    public class Parts_MasterController : Controller
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(NationwideAssemblyController));
        LogsMaster logs = new LogsMaster();
        ClientMasterBLL ClientResponse = new ClientMasterBLL();
        PartsMasterBLL PartsResponse = new PartsMasterBLL();

        [Authorize(Roles = "SiteAdmin, PartAdmin")]
        public ActionResult Index()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            PartsCategoryList parts = new PartsCategoryList();
            List<PartsCategoryList> partslist = new List<PartsCategoryList>();

            if (Loginuser == "Admin")
            {
                partslist = new MasterServiceBLL().GetPartsCategoryList();
            }
            else
            {
                partslist = new MasterServiceBLL().GetMyPartsCategoryList(result.Client_ID);
            }
            partslist = partslist.OrderBy(a => a.Part_Category).ToList();
            parts.Part_Category = "ALL Parts Category";
            partslist.Insert(0, parts);
            ViewBag.PartCategory = new SelectList(partslist, "Part_Category", "Part_Category");
            return View();
        }

        //
        // GET: /Parts_Master/
        //Parts Master Index Page
        public ActionResult PMIndex()
        {
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            if (TempData["ADDSuccessMsg"] != null)
            {
                ViewBag.ADDSuccessMsg = TempData["ADDSuccessMsg"];
            }
            if (TempData["UpdateSuccessMsg"] != null)
            {
                ViewBag.UpdateSuccessMsg = TempData["UpdateSuccessMsg"];
            }
            if (TempData["DeleteSuccessMsg"] != null)
            {
                ViewBag.DeleteSuccessMsg = TempData["DeleteSuccessMsg"];
            }
            if (TempData["DeleteFailMsg"] != null)
            {
                ViewBag.DeleteFailMsg = TempData["DeleteFailMsg"];
            }
            if (TempData["ADDFailMsg"] != null)
            {
                ViewBag.ADDFailMsg = TempData["ADDFailMsg"];
            }
            return View();
        }
        //here to get parts list if no parts rights
        public ActionResult PartsListIndex()
        {
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            return View();
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

            if (Loginuser == "Admin")
            {
                partslist = new MasterServiceBLL().GetPartsCategoryList();
            }
            else
            {
                partslist = new MasterServiceBLL().GetMyPartsCategoryList(result.Client_ID);
            }
            partslist = partslist.OrderBy(a => a.Part_Category).ToList();
            parts.Part_Category = "ALL Parts Category";
            partslist.Insert(0, parts);
            ViewBag.PartCategory = new SelectList(partslist, "Part_Category", "Part_Category");
            if (Loginuser == "Admin")
            {
                response = PartsResponse.GetPartsDetalisList(PartCatgory);
                return PartialView("_PartsList", response.ListData);
            }
            else
            {
                response = PartsResponse.GetMyPartsList(result.Client_ID, PartCatgory);
                return PartialView("_PartsList", response.ListData);
            }
        }

        public JsonResult PartsDatatable(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var result = GetPartsDetalis(model, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            });
        }

        public IList<Parts_DetailsInfoList> GetPartsDetalis(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var client = ClientResponse.GetClientName(Loginuser);
            PartsCategoryList parts = new PartsCategoryList();
            int? Client_ID = null;

            if (Loginuser != "Admin")
            {
                Client_ID = client.Client_ID;
            }
            sortBy = sortBy == "ID" ? "Created_Date" : sortBy;
            // search the dbase taking into consideration table sorting and paging
            model.extra_search = (model.extra_search != null && model.extra_search.Trim() == "") ? model.extra_search = null : model.extra_search;
            var result = PartsResponse.GetPartsDetalis(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, model.extra_search, Client_ID);
            if (result == null)
            {
                // empty collection...
                return new List<Parts_DetailsInfoList>();
            }
            return result;
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
                Model.Client_ID = result.Client_ID;
            }
            response = PartsResponse.CheckIsNewPartNumber(Model.Client_ID, Model.Part_Number.Trim());
            if (response.Data == true)
            {
                return Json("Already Exists");
            }
            else
            {
                return Json("New");
            }

        }
        //here to add parts details
        public ActionResult AddPartsDetails(Parts_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            if (ModelState.IsValid)
            {
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
                    Model.Client_ID = result.Client_ID;
                }
                response = PartsResponse.AddNewPartsDetail(Model);
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
            else
            {
                string status = "";
                int i = 0;
                foreach (var item in ModelState.Values)
                {
                    if (item.Errors.Count > 0)
                    {
                        int j = 0;
                        foreach (var keys in ModelState.Keys)
                        {
                            if (j == i)
                            {
                                status += keys.ToString();
                                break;
                            }
                            j++;
                        }
                        status += " - " + item.Errors[0].ErrorMessage + ",";
                    }
                    i++;
                }
                return Json(status, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult GetMyPartsCatgory(string ClientID = "", string partnumber = "")
        {
            List<PartsCategoryList> partslist = new List<PartsCategoryList>();
            ServiceResult<Parts_DetailsInfo> response = new ServiceResult<Parts_DetailsInfo>();
            PartsCategoryList parts = new PartsCategoryList();
            PartsCategoryList parts2 = new PartsCategoryList();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            // parts2.Part_Category = "Select";

            if (ClientID != "" && ClientID != "0")
            {
                partslist = new MasterServiceBLL().GetMyPartsCategoryList(Convert.ToInt32(ClientID));

                partslist = partslist.OrderBy(a => a.Part_Category).ToList();
                parts.Part_Category = "other";
                partslist.Insert(0, parts);

                ViewBag.PartCategory = new SelectList(partslist, "Part_Category", "Part_Category");
                if (partnumber != "")
                {
                    response = PartsResponse.GetMyPartsDetails(partnumber, Convert.ToInt32(ClientID));
                    if (response.ResultCode > 0)
                    {
                        response.Data.partcatgoryList = partslist;
                        return Json(response.Data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(partslist, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(partslist, JsonRequestBehavior.AllowGet);

                }

            }
            else
            {
                parts.Part_Category = "other";

                partslist.Add(parts);
                return Json(partslist, JsonRequestBehavior.AllowGet);
            }

        }
        //here to get Parts details for Edit
        public ActionResult EditPartsDetails(string PartsNumber = "", string ClientID = "")
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
            ViewBag.distcompany = result.distId;
            //if (Loginuser == "Admin")
            //{
            //partslist = new MasterServiceBLL().GetPartsCategoryList(); 
            // parts.Part_Category = "other";

            //partslist.Add(parts);
            //}
            //else
            //{
            partslist = new MasterServiceBLL().GetMyPartsCategoryList(result.Client_ID);
            //}

            //partslist = partslist.OrderBy(a => a.Part_Category).ToList();
            parts.Part_Category = "other";
            partslist.Insert(0, parts);
            ViewBag.PartCategory = new SelectList(partslist, "Part_Category", "Part_Category");

            //Get UserDetails To Edit
            if (PartsNumber.Trim() != "0" && ClientID.Trim() != "0")
            {
                response = PartsResponse.GetMyPartsDetails(PartsNumber, Convert.ToInt32(ClientID));
                if (response.ResultCode > 0)
                {
                    ViewBag.Edit = "EditPartsDetails";

                    return PartialView("_AddPartsDetails", response.Data);
                }
                else
                {
                    ViewBag.EditSavedErrorMsg = response.Message;
                    ViewBag.Edit = "EditPartsDetails";
                    return PartialView("_AddPartsDetails", response.Data);
                }
            }
            else
            {
                Parts_DetailsInfo model = new Parts_DetailsInfo();
                //model.Client_ID = result.Client_ID;


                return PartialView("_AddPartsDetails", model);
            }
        }

        //Here to update parts Details
        [HttpPost]
        public ActionResult UpdatePartsDetails(EditParts_DetailsInfo Model)
        {

            ServiceResult<int> response = new ServiceResult<int>();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();


            //To Get Login User Details
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            if (Loginuser == "Admin")
            {
                Model.Updated_By = Loginuser;
            }
            else
            {

                var result = ClientResponse.GetClientName(Loginuser);
                Model.Client_ID = result.Client_ID;
                Model.Updated_By = result.User_ID;
            }

            //save modified client details after model validation
            if (ModelState.IsValid)
            {

                response = PartsResponse.SavePartsDetails(Model);
                if (response.ResultCode > 0)
                {
                    TempData["UpdateSuccessMsg"] = response.Message;
                    return RedirectToAction("PMIndex");
                }
                else
                {
                    ViewBag.Edit = "EditUserDetails";
                    ViewBag.UpdateErrordMsg = response.Message;
                    return View("_EditPartsDetails", Model);
                }
            }
            else
            {
                ViewBag.Edit = "EditUserDetails";
                return View("_EditPartsDetails", Model);
            }

        }

        //here to delete parts details
        [HttpPost]
        public ActionResult DeletePartsDetails(string PartsNumber = "", string ClientID = "")
        {
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();

            if (PartsNumber.Trim() != "0" && ClientID.Trim() != "0")
            {
                string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                ViewBag.LoginUser = Loginuser;
                response = PartsResponse.DeletePartsDetails(PartsNumber, Convert.ToInt32(ClientID), Loginuser);


                return Json(response.Message, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }

        //[HttpPost]
        //public ActionResult Importparts(string ClientID = "")
        //{
        //    _log.Info("ImportParts - Starts");
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    int Client_ID = 0;
        //    List<PartsNames> partnumber = new List<PartsNames>();
        //    StringBuilder assembliesList = new StringBuilder();
        //    List<string> assemblyToProcess = new List<string>();
        //    List<PartsNames> NewParts = new List<PartsNames>();
        //    if (Loginuser != "Admin" && ClientID == "0")
        //    {
        //        Client_ID = result.Client_ID;
        //    }
        //    else
        //    {
        //        Client_ID = Convert.ToInt32(ClientID);
        //    }
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
        //            if (ext == ".xlsx")
        //            {
        //                file.SaveAs(filepath);
        //                String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0; HDR=YES; IMEX=1\"", filepath);
        //                using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
        //                {
        //                    List<Parts_Details> partslist = new List<Parts_Details>();
        //                    //Create OleDbCommand to fetch data from Excel 
        //                    using (OleDbCommand cmd = new OleDbCommand("Select * from [Sheet1$]", excelConnection))
        //                    {
        //                        excelConnection.Open();
        //                        using (OleDbDataReader dReader = cmd.ExecuteReader())
        //                        {
        //                            using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
        //                            {
        //                                db.Configuration.AutoDetectChangesEnabled = false;
        //                                db.Configuration.ValidateOnSaveEnabled = false;
        //                                //logs.WriteLog("Uploading of Parts Started", Client_ID, Loginuser);
        //                                while (dReader.Read())
        //                                {
        //                                    totalpartsimported++;
        //                                    Parts_Details parts = new Parts_Details();
        //                                    parts.Part_Number = dReader.GetValue(0).ToString().Trim();
        //                                    parts.Part_Category = dReader.GetValue(1).ToString().Trim();

                                            

        //                                    if (parts.Part_Number != "" && parts.Part_Category != "")
        //                                    {
        //                                        if (dReader.GetValue(2) != null && dReader.GetValue(4) != null && dReader.GetValue(2) != DBNull.Value && dReader.GetValue(4) != DBNull.Value)
        //                                        {
        //                                            //if (Convert.ToDecimal(dReader.GetValue(2)) < Convert.ToDecimal(dReader.GetValue(4)))
        //                                            //{
        //                                            parts.Cost = Convert.ToDecimal(dReader.GetValue(2));
        //                                            parts.Resale_Cost = Convert.ToDecimal(dReader.GetValue(4));

        //                                            //Labor Unit Calculation
        //                                            if (string.IsNullOrEmpty(dReader.GetValue(5).ToString().Trim()))
        //                                            {
        //                                                parts.LaborUnit = 0;
        //                                            }
        //                                            else
        //                                            {
        //                                                parts.LaborUnit = Convert.ToDecimal(dReader.GetValue(5));
        //                                            }

        //                                            parts.Purchased_From = dReader.GetValue(6).ToString().Trim();
        //                                            parts.Description = dReader.GetValue(7).ToString().Trim();
        //                                            parts.UOM = dReader.GetValue(8).ToString().Trim();
        //                                            parts.Client_Id = Client_ID;
        //                                            if (Loginuser != "Admin")
        //                                            {
        //                                                parts.Created_By = result.User_ID;
        //                                                parts.Updated_By = result.User_ID;
        //                                            }
        //                                            else
        //                                            {
        //                                                parts.Created_By = "Admin";
        //                                                parts.Updated_By = "Admin";
        //                                            }

        //                                            //New validations
        //                                            if (parts.Part_Number.Length > 50)
        //                                            {
        //                                                _log.Info(parts.Part_Number + " - Failed due to length beyond 50 characters.");
        //                                                totalfailed++;
        //                                                continue;
        //                                            }

        //                                            if (parts.Part_Category.Length > 500)
        //                                            {
        //                                                _log.Info(parts.Part_Number + " - Failed due to part category length beyond 500 characters.");
        //                                                totalfailed++;
        //                                                continue;
        //                                            }

        //                                            if (parts.Description.Length > 1000)
        //                                            {
        //                                                _log.Info(parts.Part_Number + " - Failed due to part description length beyond 1000 characters.");
        //                                                totalfailed++;
        //                                                continue;
        //                                            }

        //                                            if (parts.Purchased_From.Length > 500)
        //                                            {
        //                                                _log.Info(parts.Part_Number + " - Failed due to 'Purchased From' length beyond 500 characters.");
        //                                                totalfailed++;
        //                                                continue;
        //                                            }

        //                                            if (parts.UOM.Length > 150)
        //                                            {
        //                                                _log.Info(parts.Part_Number + " - Failed due to 'UOM' length beyond 150 characters.");
        //                                                totalfailed++;
        //                                                continue;
        //                                            }

        //                                            //Cost validation
        //                                            string numericString = Convert.ToString(parts.Cost);
        //                                            int length = numericString.Substring(numericString.IndexOf(".") + 1).Length;
        //                                            if (length > 2)
        //                                            {
        //                                                _log.Info(parts.Part_Number + " - Failed due to 'Cost' decimal length beyond 2 numbers.");
        //                                                totalfailed++;
        //                                                continue;
        //                                            }

        //                                            length = Math.Truncate(parts.Cost).ToString().Length;
        //                                            if (length > 8)
        //                                            {
        //                                                _log.Info(parts.Part_Number + " - Failed due to 'Cost' whole number length beyond 8 numbers.");
        //                                                totalfailed++;
        //                                                continue;
        //                                            }

        //                                            //Resale Cost validation
        //                                            numericString = Convert.ToString(parts.Resale_Cost);
        //                                            length = numericString.Substring(numericString.IndexOf(".") + 1).Length;
        //                                            if (length > 2)
        //                                            {
        //                                                _log.Info(parts.Part_Number + " - Failed due to 'Resale Cost' decimal length beyond 2 numbers.");
        //                                                totalfailed++;
        //                                                continue;
        //                                            }

        //                                            length = Math.Truncate(parts.Resale_Cost ?? 0).ToString().Length;
        //                                            if (length > 8)
        //                                            {
        //                                                _log.Info(parts.Part_Number + " - Failed due to 'Resale Cost' whole number length beyond 8 numbers.");
        //                                                totalfailed++;
        //                                                continue;
        //                                            }

        //                                            //Labor Unit validation
        //                                            numericString = Convert.ToString(parts.LaborUnit);
        //                                            length = numericString.Substring(numericString.IndexOf(".") + 1).Length;
        //                                            if (length > 4)
        //                                            {
        //                                                _log.Info(parts.Part_Number + " - Failed due to 'Labor Unit' decimal length beyond 4 numbers.");
        //                                                totalfailed++;
        //                                                continue;
        //                                            }

        //                                            length = Math.Truncate(parts.LaborUnit ?? 0).ToString().Length;
        //                                            if (length > 6)
        //                                            {
        //                                                _log.Info(parts.Part_Number + " - Failed due to 'Labor Unit' whole number length beyond 6 numbers.");
        //                                                totalfailed++;
        //                                                continue;
        //                                            }

        //                                            Parts_Details dbparts = db.Parts_Details.Where(x => x.Client_Id == Client_ID && x.Part_Number == parts.Part_Number).FirstOrDefault();
        //                                            if (dbparts != null)
        //                                            {
        //                                                dbparts.Part_Category = parts.Part_Category;
        //                                                dbparts.Cost = parts.Cost;
        //                                                dbparts.Resale_Cost = parts.Resale_Cost;
        //                                                dbparts.LaborUnit = parts.LaborUnit;
        //                                                dbparts.Client_Description = parts.Client_Description;
        //                                                dbparts.Updated_Date = DateTime.Now;
        //                                                dbparts.Updated_By = parts.Updated_By;
        //                                                dbparts.Purchased_From = parts.Purchased_From;
        //                                                dbparts.Description = parts.Description;
        //                                                dbparts.UOM = parts.UOM;
        //                                                if (parts.Part_Category != "" && parts.Part_Category != null && parts.Part_Number != "")
        //                                                {
        //                                                    PartsNames updatepartname = new PartsNames();
        //                                                    updatepartname.Partnumber = parts.Part_Number;
        //                                                    updatepartname.Category = parts.Part_Category;
        //                                                    updatepartname.Cost = Convert.ToString(parts.Cost);
        //                                                    updatepartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                    updatepartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                    updatepartname.Comments = "<span style='color:#4081bd'>“Part” is updated successfully.</span>";
        //                                                    partnumber.Add(updatepartname);

        //                                                    //To updated the existing Client Assembly Parts
        //                                                    List<Assemblies_Parts> assemblyPartsList = db.Assemblies_Parts.Where(x => x.Part_Number == parts.Part_Number).ToList();

        //                                                    foreach (Assemblies_Parts assemblyPart in assemblyPartsList)
        //                                                    {
        //                                                        assemblyPart.Part_Category = parts.Part_Category;
        //                                                        assemblyPart.Part_Cost = parts.Cost;
        //                                                        assemblyPart.Part_Resale = parts.Resale_Cost ?? 0;
        //                                                        assemblyPart.LaborUnit = parts.LaborUnit;
        //                                                        assemblyPart.EstimatedCost_Total = (parts.Cost * assemblyPart.Estimated_Qty);
        //                                                        assemblyPart.EstimatedResale_Total = (parts.Resale_Cost * assemblyPart.Estimated_Qty) ?? 0;

        //                                                        //To get the assemblies to be processed
        //                                                        assemblyToProcess.Add(assemblyPart.Assemblies_Name);
        //                                                    }
        //                                                    totalpartsupdated++;
        //                                                }
        //                                                else
        //                                                {
        //                                                    invalidparts += parts.Part_Number + "<br/>";
        //                                                    PartsNames Addpartname = new PartsNames();
        //                                                    Addpartname.Partnumber = parts.Part_Number;
        //                                                    Addpartname.Category = parts.Part_Category;
        //                                                    Addpartname.Cost = Convert.ToString(parts.Cost);
        //                                                    Addpartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                    Addpartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                    Addpartname.Comments = "<span style='color:#f00'>Failed:Invalid \"Part number\" or \"Part Category\" missing!</span>";
        //                                                    partnumber.Add(Addpartname);
        //                                                    totalfailed++;
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                parts.IsActive = true;
        //                                                parts.Created_Date = DateTime.Now;
        //                                                parts.Updated_Date = DateTime.Now;
        //                                                if (parts.Part_Category != "" && parts.Part_Category != null && parts.Part_Number != "")
        //                                                {

        //                                                    //To validate, whether this new parts already inserted in current import process
        //                                                    PartsNames oldPartToInsert = NewParts.SingleOrDefault(s => s.Partnumber.ToLower() == parts.Part_Number.ToLower());
        //                                                    if (oldPartToInsert != null)
        //                                                    {
        //                                                        _log.Info("Import Failed for the Part Number - " + parts.Part_Number + " due to the duplication when it is inserting for the first time");
        //                                                        totalfailed++;
        //                                                        continue;
        //                                                    }

        //                                                    db.Parts_Details.Add(parts);

        //                                                    totalnewparts++;
        //                                                    PartsNames Addpartname = new PartsNames();
        //                                                    Addpartname.Partnumber = parts.Part_Number;
        //                                                    Addpartname.Category = parts.Part_Category;
        //                                                    Addpartname.Cost = Convert.ToString(parts.Cost);
        //                                                    Addpartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                    Addpartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                    Addpartname.Comments = "<span style='color:#1eca0b'>“New Part” is added successfully.</span>";
        //                                                    partnumber.Add(Addpartname);
        //                                                    NewParts.Add(Addpartname);
        //                                                }
        //                                                else
        //                                                {
        //                                                    invalidparts += parts.Part_Number + "<br/>";
        //                                                    PartsNames Addpartname = new PartsNames();
        //                                                    Addpartname.Partnumber = parts.Part_Number;
        //                                                    Addpartname.Category = parts.Part_Category;
        //                                                    Addpartname.Cost = Convert.ToString(parts.Cost);
        //                                                    Addpartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                    Addpartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                    Addpartname.Comments = "<span style='color:#f00'>Failed:Invalid \"Part number\" or \"Part Category\" missing!</span>";
        //                                                    partnumber.Add(Addpartname);
        //                                                    totalfailed++;
        //                                                }
        //                                            }

        //                                            //}
        //                                            //else
        //                                            //{
        //                                            //    invalidparts += parts.Part_Number + "(Check the “Cost” and “Resale Cost” have values.), ";
        //                                            //    PartsNames Addpartname = new PartsNames();
        //                                            //    Addpartname.Partnumber = parts.Part_Number;
        //                                            //    Addpartname.Category = parts.Part_Category;
        //                                            //    Addpartname.Cost = Convert.ToString(dReader.GetValue(2));
        //                                            //    Addpartname.Resale = Convert.ToString(dReader.GetValue(4));
        //                                            //    Addpartname.Comments = "<span style='color:#f00'>Failed:Make sure \"Resale Cost\" is higher than \"Cost\".</span>";
        //                                            //    partnumber.Add(Addpartname);
        //                                            //    totalfailed++;
        //                                            //}
        //                                        }
        //                                        else
        //                                        {
        //                                            invalidparts += parts.Part_Number + "(Check the “Cost” and “Resale Cost” have values.), ";
        //                                            PartsNames Addpartname = new PartsNames();
        //                                            Addpartname.Partnumber = parts.Part_Number;
        //                                            Addpartname.Category = parts.Part_Category;
        //                                            Addpartname.Cost = Convert.ToString(dReader.GetValue(2));
        //                                            Addpartname.Resale = Convert.ToString(dReader.GetValue(4));
        //                                            Addpartname.LaborUnit = Convert.ToString(dReader.GetValue(5));
        //                                            Addpartname.Comments = "<span style='color:#f00'>Failed:Check the “Cost” and “Resale Cost” have values Not Null</span>";
        //                                            partnumber.Add(Addpartname);
        //                                            totalfailed++;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        _log.Info(parts.Part_Number + " - Failed due to Part Number or Part Cateogry is null.");
        //                                        PartsNames updatepartname = new PartsNames();
        //                                        updatepartname.Partnumber = parts.Part_Number;
        //                                        updatepartname.Category = parts.Part_Category;
        //                                        updatepartname.Cost = dReader.GetValue(2).ToString();
        //                                        updatepartname.Resale = dReader.GetValue(4).ToString();
        //                                        updatepartname.LaborUnit = dReader.GetValue(5).ToString();
        //                                        updatepartname.Comments = "<span style='color:#f00'>Failed:“Part Number” is missing!</span>";
        //                                        partnumber.Add(updatepartname);
        //                                        totalfailed++;
        //                                    }
        //                                    //partslist.Add(partsdetail);
        //                                }
        //                                //To save all the database operations at final
        //                                db.Configuration.AutoDetectChangesEnabled = true;
        //                                db.SaveChanges();

        //                                //To remove the duplicate assembly Ids
        //                                assemblyToProcess = assemblyToProcess.Distinct().ToList();
        //                                foreach (string assembly in assemblyToProcess)
        //                                {
        //                                    assembliesList.Append(assembly);
        //                                    assembliesList.Append(',');
        //                                }

        //                                //To remove last comma in the list of processed assemblies
        //                                if (assembliesList.Length > 0)
        //                                {
        //                                    assembliesList.Remove((assembliesList.Length - 1), 1);
        //                                }
        //                                //db.Database.SqlQuery<int>("exec EE_RecalcClientAssembly @AssemblyList", new SqlParameter("AssemblyList", assembliesList));

        //                                string connStr = ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;
        //                                using (SqlConnection con = new SqlConnection(connStr))
        //                                {
        //                                    using (SqlCommand sqlCmd = new SqlCommand("EE_RecalcClientAssembly", con))
        //                                    {
        //                                        sqlCmd.CommandType = CommandType.StoredProcedure;

        //                                        sqlCmd.Parameters.Add("@AssemblyList", SqlDbType.NVarChar).Value = assembliesList.ToString();
        //                                        sqlCmd.Parameters.Add("@ClientID", SqlDbType.Int).Value = Client_ID;
        //                                        sqlCmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = result.User_ID;

        //                                        con.Open();
        //                                        sqlCmd.ExecuteNonQuery();
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    string partreporttable = "";
        //                    //if (partnumber.Count > 0)
        //                    //{
        //                    //    partreporttable += "<div class='table-responsive' id='partsreport'><table class='table table-bordered' cellspacing='0' width='100%'><thead><tr><th>PartNumber</th><th>Category</th><th>Cost</th><th>ResaleCost</th><th>Comment</th></tr></thead><tbody>";
        //                    //    foreach (var par in partnumber)
        //                    //    {
        //                    //        partreporttable += "<tr>";
        //                    //        partreporttable += "<td>" + par.Partnumber + "</td>";
        //                    //        partreporttable += "<td>" + par.Category + "</td>";
        //                    //        partreporttable += "<td>" + par.Cost + "</td>";
        //                    //        partreporttable += "<td>" + par.Resale + "</td>";
        //                    //        partreporttable += "<td>" + par.Comments + "</td>";
        //                    //        partreporttable += "</tr>";
        //                    //    }
        //                    //    partreporttable += "</tbody></table></div>";
        //                    //}
        //                    status = "<u>\"Parts\" are imported successfully.</u>: <br> Total Parts Imported :" + totalpartsimported + " ;  Newly added Parts :" + totalnewparts + ";  Parts Updated :" + totalpartsupdated + "; Failed Parts:" + totalfailed + " <br><br>" + partreporttable + "";
        //                    //status = "\"Parts\" are uploaded successfully [Total Parts Imported- " + totalpartsimported + " ; Total Parts Added - " + totalpartsupdated + " ]";
        //                    //if (invalidparts != "" && invalidparts.Length > 1)
        //                    //{
        //                    //    invalidparts = invalidparts.Substring(0, invalidparts.Length - 1);
        //                    //    status += "<br/><br/> (Parts Category is missing for some parts:<b> <br/> " + invalidparts + "<br/></b>So please check these parts and try again, other parts are added successfully)";
        //                    //}
        //                    //logs.WriteLog("\"Parts\" are uploaded successfully", Client_ID, Loginuser);
        //                }
        //                System.IO.File.Delete(filepath);
        //            }
        //            else
        //            {
        //                _log.Info("ImportParts - Exits");
        //                return Json("Please import the valid template, the file should be \"xlsx\" format only!");
        //            }
        //        }
        //        _log.Info("ImportParts - Exits");
        //        return Json(status);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error("Error while uploading the Parts [Error Msg - " + ex.Message + "]");
        //        _log.Info("ImportParts - Exits");
        //        return Json(ex.Message);
        //    }
        //}

        public JsonResult GetPartsListByClientID()
        {
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var client = ClientResponse.GetClientName(Loginuser);

            var partsList = PartsResponse.GetPartsListByClientID(client.Client_ID);
            var jsonResult = Json(new { data = partsList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult ImportParts()
        {
            _log.Info("ImportParts - Starts");

            //declarations
            int clientID = 0;

            string returnText = string.Empty;
            DataTable dt = new DataTable();
            List<string> assemblyToProcess = new List<string>();
            StringBuilder assembliesList = new StringBuilder();
            try
            {
                //To get the ClientID
                string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                var result = ClientResponse.GetClientName(Loginuser);
                clientID = result.Client_ID;

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

                                DataTable dtImportPartsReport = PartsResponse.ImportParts(dtExcelParts, clientID);
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
                    Response.AddHeader("content-disposition", "attachment;filename=PartsImport-Report.xlsx");
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
                //return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
