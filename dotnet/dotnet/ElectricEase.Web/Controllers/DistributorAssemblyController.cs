using ElectricEase.BLL.AssembliesMaster;
using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.Master;
using ElectricEase.BLL.PartsMaster;
using ElectricEase.Data.PartsMaster;
using ElectricEase.Data.DataBase;
using ElectricEase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using log4net;

namespace ElectricEase.Web.Controllers
{
    public class DistributorAssemblyController : Controller
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(DistributorAssemblyController));
        AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        PartsMasterBLL PartsResponse = new PartsMasterBLL();
        ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //
        // GET: /DistributorAssembly/
        List<AssembliesParts_DetailsInfoList> parts = new List<AssembliesParts_DetailsInfoList>();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            List<AssembliesCategoryList> response = AssembliesResponse.GetDistributorCategoryList();
            AssembliesCategoryList asembly = new AssembliesCategoryList();
            asembly.Assemblies_Category = "All Assembly Category";
            response = response.OrderBy(a => a.Assemblies_Category).ToList();
            response.Insert(0, asembly);
            ViewBag.AssemblyCategory = new SelectList(response, "Assemblies_Category", "Assemblies_Category");
            return View();
        }

        public ActionResult DistributorAssemblyInfo(string name = "", int Did = 0)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
            ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            Assembly_MasterInfo model = new Assembly_MasterInfo();
            // model.Client_ID = result.Client_ID;
            ServiceResultList<distributordropdown> distributorlist = new ServiceResultList<distributordropdown>();
            distributorlist = ClientResponse.distributorList();
            var itemToRemove = distributorlist.ListData.FirstOrDefault(r => r.value == -1);
            if (itemToRemove != null)
            {
                distributorlist.ListData.Remove(itemToRemove);
            }
            ViewBag.Distributor = new SelectList(distributorlist.ListData, "value", "Name");
            ViewBag.Edit = "";
            if (name != "")
            {
                ViewBag.Edit = "EditAS";
                CheckAssemblesnameIsExist = AssembliesResponse.DistributorAssemblieNameIsExist(name, Did);
                if (CheckAssemblesnameIsExist.ResultCode == 1)
                {
                    response = AssembliesResponse.GetDistributorAssembliesListDetails(name, Did);
                    ViewBag.ASname = response.Data.Assemblies_Name;
                    model = response.Data;
                }

            }
            return PartialView("_DistributorAssemblyInfo", model);
        }

        [HttpPost]
        public ActionResult SelectedAssemblyPartsLaborInfo(string lstparts, string Assemblies_Name = "", int Did = 0, string SelectedAssemblyPartsLaborInfo = "")
        {
            _log.Info("Enters: SelectedAssemblyPartsLaborInfo");
            _log.Info("lstparts: " + lstparts);
            _log.Info("Assemblies_Name: " + Assemblies_Name);
            _log.Info("Did: " + Did.ToString());
            _log.Info("SelectedAssemblyPartsLaborInfo: " + SelectedAssemblyPartsLaborInfo);
            try
            {
                string[] partslist = lstparts.Split(',');
                ClientMasterBLL ClientResponse = new ClientMasterBLL();
                AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
                ServiceResultList<AssembliesParts_DetailsInfoList> response = new ServiceResultList<AssembliesParts_DetailsInfoList>();
                ServiceResult<Assembly_MasterInfo> AsResponse = new ServiceResult<Assembly_MasterInfo>();
                Assembly_MasterInfo ModelOBJ = new Assembly_MasterInfo();
                ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
                string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                var result = ClientResponse.GetClientName(Loginuser);
                AssemblyPartsModel model = new AssemblyPartsModel();
                model.partslist = new List<AssembliesParts_DetailsInfoList>();
                CheckAssemblesnameIsExist = AssembliesResponse.DistributorAssemblieNameIsExist(Assemblies_Name, Did);
                if (Assemblies_Name == "" || CheckAssemblesnameIsExist.ResultCode == 0)
                {
                    foreach (string item in partslist)
                    {
                        if (item != "" && item != "selectall")
                        {
                            if (Assemblies_Name != "")
                            {
                                AsResponse = AssembliesResponse.GetDistributorAssembliesListDetails(Assemblies_Name, Did);
                                model.assmeblymasterinfo = AsResponse.Data;
                            }

                            ViewBag.Edit = "EditAS";

                            response = AssembliesResponse.GetMyDistributorAssembliesPartsList(Did, item.ToString());
                            parts.Add(new AssembliesParts_DetailsInfoList
                            {
                                AssemblyID = response.ListData[0].AssemblyID,
                                Part_Number = response.ListData[0].Part_Number,
                                Part_Category = response.ListData[0].Part_Category,
                                Part_Cost = response.ListData[0].Cost,
                                Parts_Description = response.ListData[0].Description,
                                Resale_Cost = response.ListData[0].Resale_Cost,
                                Estimated_Qty = response.ListData[0].Estimated_Qty,
                                EstCost_Total = response.ListData[0].EstCost_Total,
                                EstResaleCost_Total = response.ListData[0].EstResaleCost_Total,
                                LaborUnit = response.ListData[0].LaborUnit,
                                // Actual_Qty = response.ListData[0].Actual_Qty,
                                isChekced = true,
                                IsActivePartsDetails = response.ListData[0].IsActivePartsDetails

                            });
                        }
                    }
                    model.partslist = parts;
                }
                else
                {
                    AsResponse = AssembliesResponse.GetDistributorAssembliesListDetails(Assemblies_Name, Did);
                    ViewBag.Edit = "EditAS";
                    model.assmeblymasterinfo = AsResponse.Data;

                    if (lstparts == "")
                    {
                        if (AsResponse.Data != null)
                        {
                            foreach (string item in partslist)
                            {
                                if (item != "" && item != "selectall")
                                {
                                    bool CheckIsExist = AsResponse.Data.PartsListData.Any(m => m.Part_Number == item);
                                    if (CheckIsExist == false)
                                    {
                                        response = AssembliesResponse.GetMyDistributorAssembliesPartsList(Did, item.ToString());
                                        if (response.ListData.Count > 0)
                                        {
                                            AsResponse.Data.PartsListData.Add(new AssembliesParts_DetailsInfoList
                                            {
                                                AssemblyID = response.ListData[0].AssemblyID,
                                                Part_Number = response.ListData[0].Part_Number,
                                                Part_Category = response.ListData[0].Part_Category,
                                                Part_Cost = response.ListData[0].Cost,
                                                Parts_Description = response.ListData[0].Description,
                                                Resale_Cost = response.ListData[0].Resale_Cost,
                                                Estimated_Qty = response.ListData[0].Estimated_Qty,
                                                EstCost_Total = response.ListData[0].EstCost_Total,
                                                EstResaleCost_Total = response.ListData[0].EstResaleCost_Total,
                                                LaborUnit = response.ListData[0].LaborUnit,
                                                // Actual_Qty = response.ListData[0].Actual_Qty,
                                                isChekced = true
                                            });
                                        }
                                    }
                                }
                                model.partslist = AsResponse.Data.PartsListData.ToList();
                            }
                        }
                    }
                    else
                    {
                        foreach (string item in partslist)
                        {
                            if (item != "" && item != "selectall")
                            {
                                response = AssembliesResponse.GetMyDistributorAssembliesPartsList(Did, item.ToString());
                                if (response.ListData.Count > 0)
                                {
                                    parts.Add(new AssembliesParts_DetailsInfoList
                                    {
                                        AssemblyID = response.ListData[0].AssemblyID,
                                        Part_Number = response.ListData[0].Part_Number,
                                        Part_Category = response.ListData[0].Part_Category,
                                        Part_Cost = response.ListData[0].Cost,
                                        Parts_Description = response.ListData[0].Description,
                                        Resale_Cost = response.ListData[0].Resale_Cost,
                                        Estimated_Qty = response.ListData[0].Estimated_Qty,
                                        EstCost_Total = response.ListData[0].EstCost_Total,
                                        EstResaleCost_Total = response.ListData[0].EstResaleCost_Total,
                                        LaborUnit = response.ListData[0].LaborUnit,
                                        // Actual_Qty = response.ListData[0].Actual_Qty,
                                        isChekced = true,
                                        IsActivePartsDetails = response.ListData[0].IsActivePartsDetails
                                    });
                                }
                            }
                        }

                        model.partslist = parts;
                    }
                }
                return Json(model);
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message);
                _log.Error(ex.InnerException);
            }
            _log.Info("Exits: SelectedAssemblyPartsLaborInfo");
            return Json("");
            //return PartialView("_Partslaborinfo", model);
        }

        public ActionResult getallAssembliesList(string assembName = "")
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            Assembly_MasterInfo model = new Assembly_MasterInfo();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.GetDistributorAssembliesList();
            if (assembName == "")
                model.asList = response;
            else
                model.asList = response.Where(x => x.Assemblies_Category.TrimEnd() == assembName).ToList();
            ViewBag.AssembliesName = null;
            ViewBag.Description = null;
            ViewBag.AssembliesList = "Show";
            return PartialView("_DistributorAssembliesList", model);

        }

        [HttpPost]
        public ActionResult GetallAssemblyParts(string name = "", string lstparts = "", string PartCatgory = "", int Did = 0)
        {
            //AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            //ServiceResult<bool> CheckAssemblesnameIsExist = AssembliesResponse.DistributorAssemblieNameIsExist(AssebliesId);
            //if (CheckAssemblesnameIsExist.ResultCode == 0)
            //{
            //    ViewBag.Edit = "";

            //}

            return PartialView("_showallParts");

            //List<ElectricEase.Models.Parts_DetailsInfoList> modal = new List<Parts_DetailsInfoList>();
            //PartsMasterBLL PartsResponse = new PartsMasterBLL();
            //ClientMasterBLL ClientResponse = new ClientMasterBLL();
            //AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            //ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            //ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            //List<PartsCategoryList> partslist = new List<PartsCategoryList>();
            //string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            //var result = ClientResponse.GetClientName(Loginuser);
            //partslist = new MasterServiceBLL().GetDistributorPartsCategoryList();
            //response = PartsResponse.GetSelectedDistributorPartsDetalisList(PartCatgory, Did);
            //modal = response.ListData.ToList();
            //CheckAssemblesnameIsExist = AssembliesResponse.DistributorAssemblieNameIsExist(name, Did);
            //if (name == "" || CheckAssemblesnameIsExist.ResultCode == 0)
            //{
            //    ViewBag.Edit = "";

            //}
            //if (lstparts != "")
            //{
            //    string[] partsno = lstparts.Split(',');
            //    if (partsno.Length > 0)
            //    {
            //        foreach (var item in partsno)
            //        {
            //            int isused = 0;
            //            foreach (var items in modal)
            //            {
            //                if (items.Part_Number == item)
            //                {
            //                    items.isChekced = true;
            //                    break;
            //                }
            //            }

            //        }
            //    }
            //}

            //return new JsonResult { Data = modal, MaxJsonLength = Int32.MaxValue };

        }

        public JsonResult PartsDatatable(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = GetDistributorPartsDetalis(model, out filteredResultsCount, out totalResultsCount);

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

        public IList<Parts_DetailsInfoList> GetDistributorPartsDetalis(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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

            var result = PartsResponse.GetDistributorPartsByDistId(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, model.extra_search, model.distributorId);
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

        public JsonResult DistributorAssembliesDatatable(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var result = GetDistributorAssembliesDetalis(model, out filteredResultsCount, out totalResultsCount);


            return Json(new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
            });
        }

        public IList<Assembly_MasterInfoList> GetDistributorAssembliesDetalis(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            string sortDir = "";

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = string.IsNullOrEmpty(model.order[0].dir) ? "asc" : model.order[0].dir;
            }
            // sortBy = sortBy == "ID" ? "Created_Date" : (sortBy == "Company" ? "Distributor_Master.Company" : sortBy);
            // search the dbase taking into consideration table sorting and paging
            model.extra_search = (model.extra_search != null && (model.extra_search.Trim() == "" || model.extra_search.Trim() == "All Assembly Category")) ? model.extra_search = null : model.extra_search;
            var result = AssembliesResponse.GetDistributorAssembliesDetalis(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, model.extra_search);
            if (result == null)
            {
                // empty collection...
                return new List<Assembly_MasterInfoList>();
            }
            return result;
        }


       
               
        [HttpPost]
        public ActionResult AddAssembliesDetails(FormCollection form)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            string modeldata = Request.Form["model"];
            Assembly_MasterInfo model = jss.Deserialize<Assembly_MasterInfo>(modeldata);

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ServiceResult<int> ASInfoIresponse = new ServiceResult<int>();
            var result = ClientResponse.GetClientName(Loginuser);
            // model.Client_ID = result.Client_ID;
            model.Created_By = Loginuser;
            model.Updated_By = Loginuser;
            if (model.PartsListData.Count() != 0)
            {
                // ASInfoIresponse = AssembliesResponse.AddNewAssembliesInformation(model);
                ASInfoIresponse = AssembliesResponse.AddNewDistributorAssembliesDetails(model);
                if (ASInfoIresponse.ResultCode > 0)
                {
                    ViewBag.ASInfoSuccessMsg = ASInfoIresponse.Message;
                    TempData["ASMSuccessMsg"] = ASInfoIresponse.Message;
                    return Json(ASInfoIresponse.Message);

                }
                else
                {
                    ViewBag.ASInfoFailMsg = ASInfoIresponse.Message;
                    //return View("ASMIndex", model);
                    return Json(ASInfoIresponse.Message);
                }

            }
            else
            {
                ViewBag.ASInfoFailMsg = "Error Occured While Saving Assemblies Details!";
                //return View("ASMIndex", model);
                return Json("");
            }


        }

        [HttpPost]
        public ActionResult UpdateAssembliesDetail()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string modeldata = Request.Form["model"];
            Assembly_MasterInfo model = jss.Deserialize<Assembly_MasterInfo>(modeldata);

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ServiceResult<int> ASInfoIresponse = new ServiceResult<int>();
            var result = ClientResponse.GetClientName(Loginuser);
            //  model.Client_ID = result.Client_ID;
            model.Created_By = Loginuser;
            model.Updated_By = Loginuser;
            if (model.PartsListData.Count() != 0)
            {
                // ASInfoIresponse = AssembliesResponse.AddNewAssembliesInformation(model);
                ASInfoIresponse = AssembliesResponse.UpdateDistribotorAssembliesDetails(model);
                if (ASInfoIresponse.ResultCode > 0)
                {
                    ViewBag.ASInfoSuccessMsg = ASInfoIresponse.Message;
                    TempData["ASMupdateSuccessMsg"] = ASInfoIresponse.Message;
                    return Json(ASInfoIresponse.Message);

                }
                else
                {
                    ViewBag.ASInfoFailMsg = ASInfoIresponse.Message;
                    //return View("ASMIndex", model);
                    return Json(ASInfoIresponse.Message);
                }

            }
            else
            {
                ViewBag.ASInfoFailMsg = "Error Occured While Saving Assemblies Details!";
                //return View("ASMIndex", model);
                return Json("");
            }
        }

        public ActionResult DeleteAssembliesDetails(string AssembliesName, int Did)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.DeleteDistributorAssembliesDetails(AssembliesName, Did, Loginuser);
            if (response.ResultCode > 0)
            {
                //TempData["AsDeleteSuccessMSg"] = response.Message;
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //ViewBag.AsDeleteFailMsg = response.Message;
                // TempData["AsDeleteFailMsg"] = response.Message;
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPreviousDescriptionList()
        {
            ServiceResultList<Assembly_DescriptionList> response = new ServiceResultList<Assembly_DescriptionList>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.GetNationDescriptionList();

            return PartialView("_PreviousDescriptionList", response.ListData);
            // return PartialView("_PervoiusSOWList");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DistributorAssemblyExport()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResultList<distributordropdown> distributorlist = new ServiceResultList<distributordropdown>();
            distributorlist = ClientResponse.distributorList();
            var itemToRemove = distributorlist.ListData.FirstOrDefault(r => r.value == -1);
            if (itemToRemove != null)
            {
                distributorlist.ListData.Remove(itemToRemove);
            }
            ViewBag.Distributor = new SelectList(distributorlist.ListData, "value", "Name");
            //ServiceResultList<Standaredclient> standareduser = new ServiceResultList<Standaredclient>();
            //standareduser = ClientResponse.StandaredUsers();
            //ViewBag.Standared = new SelectList(standareduser.ListData, "value", "Name");
            return View();
        }

        public ActionResult GetDistributorAssembliesList(string assembName = "", int Did = 0)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            Assembly_MasterInfo model = new Assembly_MasterInfo();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.GetDistributorsAssembliesList(Did);
            ServiceResultList<Standaredclient> standareduser = new ServiceResultList<Standaredclient>();
            standareduser = ClientResponse.DistributorClients(Did);
            ViewBag.Client = new SelectList(standareduser.ListData, "value", "Name");
            response = response.Where(x => x.IsActive == true).ToList();
            if (assembName == "")
                model.asList = response;
            else
                model.asList = response.Where(x => x.Assemblies_Category == assembName).ToList();
            ViewBag.AssembliesName = null;
            ViewBag.Description = null;
            ViewBag.AssembliesList = "Show";
            return PartialView("_GetDistributorAssembliesList", model);
        }
        [HttpPost]
        public ActionResult AssmbliesExport(DistributorAssemblies model)
        {
            string result = AssembliesResponse.DistributorAssemblyExport(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetDistributorAssembliesGrid()
        {
            var assemblyList = AssembliesResponse.GetDistributorAssembliesGrid();
            return Json(new { data = assemblyList }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetPartsListByDistributorID(int distributorID)
        {
            _log.Info("Enters: GetPartsListByDistributorID");
            _log.Debug("distributorID: " + Convert.ToString(distributorID));
            var partsList = PartsResponse.GetDistributorPartsByDistID(distributorID);
            _log.Info("Data: " + partsList);
            var jsonResult = Json(new { data = partsList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            _log.Info("Exits: GetPartsListByDistributorID");
            return jsonResult;
        }

        [Authorize]
        public JsonResult ValidateDistributorClientsLaborUnit(DistributorAssemblies model)
        {
            string result = AssembliesResponse.ValidateDistributorClientsLaborUnit(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult GetAssembliesByDistributorID(int distributorID)
        {
            var response = AssembliesResponse.GetDistributorsAssembliesList(distributorID);
            var jsonResult = Json(new { data = response }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult GetClientsByDistributorID(int distributorID)
        {
            Assembly_MasterInfo model = new Assembly_MasterInfo();
            ServiceResultList<Standaredclient> standareduser = new ServiceResultList<Standaredclient>();
            standareduser = ClientResponse.DistributorClients(distributorID);
            ViewBag.Client = new SelectList(standareduser.ListData, "value", "Name");
            return PartialView("_GetDistributorAssembliesList", model);

            //var clients = ClientResponse.DistributorClients(distributorID);
            //var jsonResult = Json(new { data = clients }, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
        }
    }
}
