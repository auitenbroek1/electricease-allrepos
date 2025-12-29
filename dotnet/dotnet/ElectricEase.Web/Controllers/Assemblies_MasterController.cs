using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.BLL;
using ElectricEase.Models;
using ElectricEase.Helpers;
using ElectricEase.BLL.PartsMaster;
using ElectricEase.BLL.ClientMaster;
using System.Web.Security;
using ElectricEase.BLL.AssembliesMaster;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using ElectricEase.BLL.Master;
using ElectricEase.Data.PartsMaster;

namespace ElectricEase.Web.Controllers
{
    public class Assemblies_MasterController : Controller
    {
        //
        // GET: /Assemblies_Master/
        //Controller is created by Arunkumar.K
        #region Declaration
        public static Assembly_MasterInfo ASMasterTempObj = new Assembly_MasterInfo();
        List<AssembliesParts_DetailsInfoList> parts = new List<AssembliesParts_DetailsInfoList>();
        List<Parts_DetailsInfoList> partsList = new List<Parts_DetailsInfoList>();
        List<AssembliesLabourDetailsList> laborerList = new List<AssembliesLabourDetailsList>();
        List<AssembliesLabourDetailsList> laborDetails = new List<AssembliesLabourDetailsList>();
        ClientMasterBLL ClientResponse = new ClientMasterBLL();
        AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        PartsMasterBLL PartsResponse = new PartsMasterBLL();
        #endregion

        #region Controller
        [Authorize(Roles = "SiteAdmin, AssemblyAdmin")]
        public ActionResult Index()
        {
            if (TempData["ASMSuccessMsg"] != null)
            {
                ViewBag.ASMSuccessMsg = TempData["ASMSuccessMsg"];
            }
            if (TempData["ASMupdateSuccessMsg"] != null)
            {
                ViewBag.ASMupdateSuccessMsg = TempData["ASMupdateSuccessMsg"];
            }
            ViewBag.Edit = "";
            return View();
        }
        //Here To get and show Assembly Information..
        public ActionResult AssemblyInfo(string name = "")
        {
            name = Server.UrlDecode(name);
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
            ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            Assembly_MasterInfo model = new Assembly_MasterInfo();
            model.Client_ID = result.Client_ID;
            ViewBag.Edit = "";
            if (name != "")
            {
                ViewBag.Edit = "EditAS";
                CheckAssemblesnameIsExist = AssembliesResponse.AssemblieNameIsExist(name);
                if (CheckAssemblesnameIsExist.ResultCode == 1)
                {
                    response = AssembliesResponse.GetAssembliesListDetails(name, result.Client_ID);
                    ViewBag.ASname = response.Data.Assemblies_Name;
                    model = response.Data;
                }

            }
            return PartialView("_AssemblyInfo", model);
        }
        //Here to Get Parts Details Foe Assembly..
        public ActionResult AssemblyPartsLaborInfo(string name = "", List<ListPart> lstparts = null)
        {
            name = Server.UrlDecode(name);
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
            ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            AssemblyPartsModel model = new AssemblyPartsModel();
            model.partslist = new List<AssembliesParts_DetailsInfoList>();
            if (name != "")
            {
                CheckAssemblesnameIsExist = AssembliesResponse.AssemblieNameIsExist(name);
                if (CheckAssemblesnameIsExist.ResultCode == 1)
                {
                    response = AssembliesResponse.GetAssembliesListDetails(name, result.Client_ID);
                    model.assmeblymasterinfo = response.Data;
                    model.partslist = response.Data.PartsListData.ToList();
                    ViewBag.Edit = "EditAS";
                }

            }

            return PartialView("_Partslaborinfo", model);
        }
        //Here To get bind Parts Details when After selected from Popup
        [HttpPost]
        public ActionResult SelectedAssemblyPartsLaborInfo(string lstparts, string Assemblies_Name = "",string SelectedAssemblyPartsLaborInfo="")
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
            CheckAssemblesnameIsExist = AssembliesResponse.AssemblieNameIsExist(Assemblies_Name);
            if (Assemblies_Name == "" || CheckAssemblesnameIsExist.ResultCode == 0)
            {
                foreach (string item in partslist)
                {
                    if (item != "" && item != "selectall")
                    {
                        if(Assemblies_Name!="")
                        {
                            AsResponse = AssembliesResponse.GetAssembliesListDetails(Assemblies_Name, result.Client_ID);
                            model.assmeblymasterinfo = AsResponse.Data;
                        }
                       
                        ViewBag.Edit = "EditAS";
                        
                        response = AssembliesResponse.GetMyAssembliesPartsList(Convert.ToInt16(result.Client_ID), item.ToString());
                        parts.Add(new AssembliesParts_DetailsInfoList
                        {
                            Part_Number = response.ListData[0].Part_Number,
                            Part_Category = response.ListData[0].Part_Category,
                            Part_Cost = response.ListData[0].Cost,
                            Parts_Description = response.ListData[0].Description,
                            Resale_Cost = response.ListData[0].Resale_Cost,
                            Estimated_Qty = response.ListData[0].Estimated_Qty,
                            EstCost_Total = response.ListData[0].EstCost_Total,
                            EstResaleCost_Total = response.ListData[0].EstResaleCost_Total,
                            // Actual_Qty = response.ListData[0].Actual_Qty,
                            LaborUnit = response.ListData[0].LaborUnit,
                            isChekced = true,
                            IsActivePartsDetails= response.ListData[0].IsActivePartsDetails

                        });
                    }
                }
                model.partslist = parts;
            }
            else
            {
                if (lstparts == "")
                {
                    AsResponse = AssembliesResponse.GetAssembliesListDetails(Assemblies_Name, result.Client_ID);
                    ViewBag.Edit = "EditAS";
                    model.assmeblymasterinfo = AsResponse.Data;
                    if (AsResponse.Data != null)
                    {
                        foreach (string item in partslist)
                        {
                            if (item != "" && item != "selectall")
                            {
                                bool CheckIsExist = AsResponse.Data.PartsListData.Any(m => m.Part_Number == item);
                                if (CheckIsExist == false)
                                {
                                    response = AssembliesResponse.GetMyAssembliesPartsList(Convert.ToInt16(result.Client_ID), item.ToString());
                                    if (response.ListData.Count > 0)
                                    {
                                        AsResponse.Data.PartsListData.Add(new AssembliesParts_DetailsInfoList
                                        {
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
                    //AsResponse = AssembliesResponse.GetAssembliesListDetails(Assemblies_Name, result.Client_ID);
                    //ViewBag.Edit = "EditAS";
                    //model.assmeblymasterinfo = AsResponse.Data;
                    foreach (string item in partslist)
                    {
                        if (item != "" && item != "selectall")
                        {

                            //string[] splitData = selectedPart.Split(',');
                            //In argument we are passing clientID and partId to db.
                            response = AssembliesResponse.GetMyAssembliesPartsList(Convert.ToInt16(result.Client_ID), item.ToString());
                            if (response.ListData.Count > 0)
                            {
                                parts.Add(new AssembliesParts_DetailsInfoList
                                {
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
            //return PartialView("_Partslaborinfo", model);
        }
        //here to Get Parts Details
        [HttpPost]
        public ActionResult GetallAssemblyParts(string name = "", string lstparts = "", string PartCatgory = "")
        {
            //List<ElectricEase.Models.Parts_DetailsInfoList> modal = new List<Parts_DetailsInfoList>();
            //PartsMasterBLL PartsResponse = new PartsMasterBLL();
            //ClientMasterBLL ClientResponse = new ClientMasterBLL();
            //AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            //ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            //ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            //List<PartsCategoryList> partslist = new List<PartsCategoryList>();
            //string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            //var result = ClientResponse.GetClientName(Loginuser);
            //partslist = new MasterServiceBLL().GetMyPartsCategoryList(result.Client_ID); 
            //response = PartsResponse.GetMyPartsList(result.Client_ID, PartCatgory);
            //modal = response.ListData.ToList();
            //CheckAssemblesnameIsExist = AssembliesResponse.AssemblieNameIsExist(name);
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
            //return PartialView("_showallParts", modal);
            return PartialView("_showallParts");


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

        public IList<Parts_DetailsInfoList> GetClientsPartsDetalis(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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

        //Here To Add Assembly Details
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
            model.Client_ID = result.Client_ID;
            model.Created_By = Loginuser;
            model.Updated_By = Loginuser;
            if (model.PartsListData.Count() != 0)
            {
                // ASInfoIresponse = AssembliesResponse.AddNewAssembliesInformation(model);
                ASInfoIresponse = AssembliesResponse.AddNewAssembliesDetails(model);
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
        //Here to Update Assembly Details
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
            model.Client_ID = result.Client_ID;
            model.Created_By = Loginuser;
            model.Updated_By = Loginuser;
            if (model.PartsListData.Count() != 0)
            {
                // ASInfoIresponse = AssembliesResponse.AddNewAssembliesInformation(model);
                ASInfoIresponse = AssembliesResponse.UpdateAssembliesDetails(model);
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
        //Here to Get Selected PartsCatgory List
        public ActionResult GetSelectedCatgoryList(string PartCatgory = "")
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            List<PartsCategoryList> partslist = new List<PartsCategoryList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            // partslist = new MasterServiceBLL().GetMyPartsCategoryList(result.Client_ID);
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            response = PartsResponse.GetMyPartsList(result.Client_ID);
            if (PartCatgory.Trim() == "ALL")
            {
                return PartialView("_showallParts", response.ListData);
            }
            else
            {
                List<Parts_DetailsInfoList> selectedList = new List<Parts_DetailsInfoList>();
                selectedList = response.ListData.Where(m => m.Part_Category == PartCatgory).Select(m => new Parts_DetailsInfoList()
                {
                    Part_Category = m.Part_Category,
                    Part_Number = m.Part_Number,
                    Cost = m.Cost,
                    Resale_Cost = m.Resale_Cost,
                    Description = m.Description

                }).Distinct().ToList();

                return PartialView("_showallParts", selectedList);
            }


        }
        //Here to delete Assembly Details
        public ActionResult DeleteAssembliesDetails(string AssembliesName)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.DeleteAssembliesDetails(AssembliesName, result.Client_ID, Loginuser);
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
        //Here to Get Previous Description List
        public ActionResult GetPreviousDescriptionList()
        {
            ServiceResultList<Assembly_DescriptionList> response = new ServiceResultList<Assembly_DescriptionList>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.GetDescriptionList(result.Client_ID);

            return PartialView("_PreviousDescriptionList", response.ListData);
            // return PartialView("_PervoiusSOWList");
        }

        public ActionResult ASMIndex()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            Assembly_MasterInfo model = new Assembly_MasterInfo();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.GetAssembliesList(result.Client_ID);
            model.asList = response;
            ViewBag.LoginUser = Loginuser;


            if (TempData["ASMSuccessMsg"] != null)
            {
                ViewBag.ASMSuccessMsg = TempData["ASMSuccessMsg"];
            }

            if (TempData["AssembliesUpdateSuccessMsg"] != null)
            {
                ViewBag.AssembliesUpdateSuccessMsg = TempData["AssembliesUpdateSuccessMsg"];
            }
            if (TempData["AssembliesUpdateFailMsg"] != null)
            {
                ViewBag.AssembliesUpdateFailMsg = TempData["AssembliesUpdateFailMsg"];
            }
            if (TempData["MyLaborList"] != null)
            {
                TempData.Remove("MyLaborList");
            }
            if (TempData["MyParts"] != null)
            {
                TempData.Remove("MyParts");
            }
            if (TempData["AsDeleteSuccessMSg"] != null)
            {
                ViewBag.AsDeleteSuccessMSg = TempData["AsDeleteSuccessMSg"];
            }
            if (TempData["AsDeleteFailMsg"] != null)
            {
                ViewBag.AsDeleteFailMsg = TempData["AsDeleteFailMsg"];
            }
            return View(model);
        }
        //Here To get Assembly Details
        public ActionResult getallAssembliesList(string assembName="")
        {
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            Assembly_MasterInfo model = new Assembly_MasterInfo();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.GetAssembliesList(result.Client_ID);
            if (assembName == "")
                model.asList = response;
            else
                model.asList = response.Where(x => x.Assemblies_Category == assembName).ToList() ;
            ViewBag.AssembliesName = null;
            ViewBag.Description = null;


            ViewBag.AssembliesList = "Show";
            return PartialView("_assemblyList", model);

        }

        public ContentResult GetAssembliesList()
        {
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var client = ClientResponse.GetClientName(Loginuser);
            var assemblyList = AssembliesResponse.GetAssembliesList(client.Client_ID);
            //return Json(new { data = assemblyList, client = client.Client_ID }, JsonRequestBehavior.AllowGet);
            //return Json(assemblyList, JsonRequestBehavior.AllowGet);

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { data = assemblyList }),
                ContentType = "application/json"
            };
            return result;
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
        #endregion


        #region comand controller
        //public ActionResult AssembliesList()
        //{
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
        //    Assembly_MasterInfo model = new Assembly_MasterInfo();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    response = AssembliesResponse.GetAssembliesList(result.Client_ID);
        //    model.asList = response;
        //    ViewBag.AssembliesName = null;
        //    ViewBag.Description = null;


        //    ViewBag.AssembliesList = "Show";
        //    return View("ASMIndex", model);

        //}
        //public ActionResult AddAssemblyParts(string AssembliesName, string Description, string AssembliesCategory, string OtherAssembliesCategory, string PartCostTotal, string PartsResalTotal, string Laborcost, string LaborRcost, string Estimated_Hour, string LabortEstCostTot, string LaborEstResaleTot, string GrandCostTot, string GrandResaleTot, string PartgridData)
        //{
        //    if (PartgridData != null)
        //    {
        //        var log = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(PartgridData);
        //        TempData["PartsListData"] = log;
        //    }
        //    ASMasterTempObj.Assemblies_Name = AssembliesName;
        //    ASMasterTempObj.Assemblies_Description = Description;
        //    ASMasterTempObj.Assemblies_Category = AssembliesCategory;
        //    ASMasterTempObj.OtherAssemblies_Category = OtherAssembliesCategory;
        //    if (PartCostTotal != null && PartCostTotal != "")
        //    {
        //        ASMasterTempObj.PartCostTotal = Convert.ToDecimal(PartCostTotal);
        //    }
        //    if (PartsResalTotal != null && PartsResalTotal != "")
        //    {
        //        ASMasterTempObj.PartResaleTotal = Convert.ToDecimal(PartsResalTotal);
        //    }
        //    if (Laborcost != null && Laborcost != "")
        //    {
        //        ASMasterTempObj.labor_cost = Convert.ToDecimal(Laborcost);
        //    }
        //    if (LaborRcost != null && LaborRcost != "")
        //    {
        //        ASMasterTempObj.Lobor_Resale = Convert.ToDecimal(LaborRcost);
        //    }
        //    if (Estimated_Hour != null && Estimated_Hour != "")
        //    {
        //        ASMasterTempObj.Estimated_Hour = Convert.ToDecimal(Estimated_Hour);
        //    }
        //    if (LabortEstCostTot != null && LabortEstCostTot != "")
        //    {
        //        ASMasterTempObj.LaborEst_CostTotal = Convert.ToDecimal(LabortEstCostTot);
        //    }
        //    if (LaborEstResaleTot != null && LaborEstResaleTot != "")
        //    {
        //        ASMasterTempObj.LaborEst_ResaleTotal = Convert.ToDecimal(LaborEstResaleTot);
        //    }
        //    if (GrandCostTot != null && GrandCostTot != "")
        //    {
        //        ASMasterTempObj.GrandCostTotal = Convert.ToDecimal(GrandCostTot);
        //    }
        //    if (GrandResaleTot != null && GrandResaleTot != "")
        //    {
        //        ASMasterTempObj.GrandResaleTotal = Convert.ToDecimal(GrandResaleTot);
        //    }


        //    PartsMasterBLL PartsResponse = new PartsMasterBLL();
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
        //    ViewBag.AssembliesName = AssembliesName;
        //    ViewBag.Description = Description;
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    TempData.Keep("MyParts");
        //    if (Loginuser == "Admin")
        //    {
        //        response = PartsResponse.GetPartsDetalisList();
        //        return View(response.ListData);
        //    }
        //    else
        //    {
        //        var result = ClientResponse.GetClientName(Loginuser);
        //        response = PartsResponse.GetMyPartsList(result.Client_ID);
        //        List<AssembliesParts_DetailsInfoList> selectedParts = TempData["MyParts"] as List<AssembliesParts_DetailsInfoList> ?? new List<AssembliesParts_DetailsInfoList>();
        //        for (int i = 0; i < response.ListData.Count; i++)
        //        {
        //            if (selectedParts.Count > 0)
        //            {
        //                foreach (AssembliesParts_DetailsInfoList list in selectedParts)
        //                {
        //                    if (list.Part_Number.Trim() == response.ListData[i].Part_Number.Trim())
        //                    {
        //                        response.ListData[i].isChekced = true;
        //                    }
        //                }
        //            }
        //            partsList.Add(new Parts_DetailsInfoList
        //            {
        //                Client_ID = response.ListData[i].Client_ID,
        //                Part_Number = response.ListData[i].Part_Number,
        //                Part_Category = response.ListData[i].Part_Category,
        //                Part_Cost = response.ListData[i].Cost,
        //                Resale_Cost = response.ListData[i].Resale_Cost,
        //                Purchased_From = response.ListData[i].Purchased_From,
        //                isChekced = response.ListData[i].isChekced
        //            });
        //        }

        //        return View(partsList.ToList());
        //    }
        //}

        //[HttpPost]
        //public ActionResult AddPartsInGrid(FormCollection form, Assembly_MasterInfo model)
        //{
        //    AssembliesMasterBLL PartsResponse = new AssembliesMasterBLL();
        //    ServiceResultList<AssembliesParts_DetailsInfoList> response = new ServiceResultList<AssembliesParts_DetailsInfoList>();
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    List<Assembly_MasterInfoList> aslistdata = new List<Assembly_MasterInfoList>();
        //    List<AssembliesParts_DetailsInfoList> partsListOBJ = new List<AssembliesParts_DetailsInfoList>();
        //    List<AssembliesLabourDetailsList> LabourlistOBJ = new List<AssembliesLabourDetailsList>();
        //    ViewBag.AssembliesName = model.Assemblies_Name;
        //    ViewBag.Description = model.Assemblies_Description;


        //    if (TempData["PartsListData"] != null)
        //    {
        //        var ps = (Dictionary<string, string[]>)TempData["PartsListData"];
        //        foreach (string[] s in ps.Values)
        //        {
        //            AssembliesParts_DetailsInfoList obj = new AssembliesParts_DetailsInfoList();
        //            //obj.Client_ID = s[0];
        //            ///obj.Client_ID=Convert.ToInt32(s[0]);
        //            obj.Part_Number = s[0];
        //            obj.Part_Category = s[1];
        //            obj.Part_Cost = Convert.ToDecimal(s[2]);
        //            obj.Resale_Cost = Convert.ToDecimal(s[3]);
        //            if (s[4] != "")
        //            {
        //                obj.Estimated_Qty = Convert.ToInt32(s[4]);
        //            }
        //            if (s[5] != "")
        //            {
        //                obj.EstCost_Total = Convert.ToDecimal(s[5]);
        //            }
        //            if (s[6] != "")
        //            {
        //                obj.EstResaleCost_Total = Convert.ToDecimal(s[6]);

        //            }


        //            //obj.Actual_Qty = Convert.ToInt32(s[4]);
        //            obj.isChekced = Convert.ToBoolean(true);
        //            // Client_ID = response.ListData[i].Client_ID, Part_Number = response.ListData[i].Part_Number, Part_Category = response.ListData[i].Part_Category, Cost = response.ListData[i].Cost, Resale_Cost = response.ListData[i].Resale_Cost, Purchased_From = response.ListData[i].Purchased_From, isChekced = response.ListData[i].isChekced });
        //            partsListOBJ.Add(new AssembliesParts_DetailsInfoList
        //            {
        //                //Client_ID = obj.Client_ID,
        //                Part_Number = obj.Part_Number,
        //                Part_Category = obj.Part_Category,
        //                Part_Cost = obj.Part_Cost,
        //                Resale_Cost = obj.Resale_Cost,
        //                Estimated_Qty = obj.Estimated_Qty,
        //                EstCost_Total = obj.EstCost_Total,
        //                EstResaleCost_Total = obj.EstResaleCost_Total,
        //                isChekced = Convert.ToBoolean(true)
        //            });
        //            //}
        //        }
        //    }




        //    var selectedValue = form.GetValues("assignChkBx");
        //    if (selectedValue != null)
        //    {
        //        foreach (var selectedPart in selectedValue)
        //        {
        //            string[] splitData = selectedPart.Split(',');
        //            //In argument we are passing clientID and partId to db.
        //            response = PartsResponse.GetMyAssembliesPartsList(Convert.ToInt16(splitData[0]), splitData[1]);
        //            parts.Add(new AssembliesParts_DetailsInfoList
        //            {
        //                Part_Number = response.ListData[0].Part_Number,
        //                Part_Category = response.ListData[0].Part_Category,
        //                Part_Cost = response.ListData[0].Cost,
        //                Resale_Cost = response.ListData[0].Resale_Cost,
        //                Estimated_Qty = response.ListData[0].Estimated_Qty,
        //                EstCost_Total = response.ListData[0].EstCost_Total,
        //                EstResaleCost_Total = response.ListData[0].EstResaleCost_Total,
        //                // Actual_Qty = response.ListData[0].Actual_Qty,
        //                isChekced = true
        //            });
        //        }
        //        model.PartsListData = parts;

        //        if (partsListOBJ.Count != 0)
        //        {
        //            foreach (var newitem in parts)
        //            {
        //                foreach (var PreviousselectedItem in partsListOBJ)
        //                {
        //                    if (PreviousselectedItem.Part_Number.Trim() == newitem.Part_Number.Trim() && PreviousselectedItem.isChekced == newitem.isChekced)
        //                    {
        //                        newitem.Part_Number = PreviousselectedItem.Part_Number;
        //                        newitem.Part_Category = PreviousselectedItem.Part_Category;
        //                        newitem.Part_Cost = PreviousselectedItem.Part_Cost;
        //                        newitem.Resale_Cost = PreviousselectedItem.Resale_Cost;
        //                        newitem.Estimated_Qty = PreviousselectedItem.Estimated_Qty;
        //                        newitem.EstCost_Total = PreviousselectedItem.EstCost_Total;
        //                        newitem.EstResaleCost_Total = PreviousselectedItem.EstResaleCost_Total;
        //                        //isChekced = Convert.ToBoolean(true)

        //                    }

        //                }


        //            }
        //        }
        //        // ViewData["MyParts"] = parts;


        //        TempData["MyParts"] = parts;
        //        ViewBag.PartsData = parts;
        //        TempData.Peek("MyParts");
        //    }


        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    aslistdata = AssembliesResponse.GetAssembliesList(result.Client_ID);
        //    model.asList = aslistdata;

        //    model.Assemblies_Name = ASMasterTempObj.Assemblies_Name;
        //    model.Assemblies_Description = ASMasterTempObj.Assemblies_Description; ;
        //    model.Assemblies_Category = ASMasterTempObj.Assemblies_Category; ;
        //    model.OtherAssemblies_Category = ASMasterTempObj.OtherAssemblies_Category;

        //    model.PartCostTotal = ASMasterTempObj.PartCostTotal;
        //    model.PartResaleTotal = ASMasterTempObj.PartResaleTotal;
        //    model.labor_cost = ASMasterTempObj.labor_cost;
        //    model.Lobor_Resale = ASMasterTempObj.Lobor_Resale;
        //    model.Estimated_Hour = ASMasterTempObj.Estimated_Hour;
        //    model.LaborEst_CostTotal = ASMasterTempObj.LaborEst_CostTotal;
        //    model.LaborEst_ResaleTotal = ASMasterTempObj.LaborEst_ResaleTotal;
        //    model.GrandCostTotal = ASMasterTempObj.GrandCostTotal;
        //    model.GrandResaleTotal = ASMasterTempObj.GrandResaleTotal;




        //    return View("ASMIndex", model);
        //}

        //public ActionResult AddAssembliesLaborDetails(string AssembliesName, string Description, string PartgridData, string LabourgridData)
        //{
        //    try
        //    {
        //        if (PartgridData != null)
        //        {
        //            var log = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(PartgridData);
        //            TempData["PartsListData"] = log;
        //        }
        //        if (LabourgridData != null)
        //        {
        //            var LabourList = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LabourgridData);
        //            TempData["LabourListData"] = LabourList;
        //        }
        //        AssembliesMasterBLL laborDetailsResponse = new AssembliesMasterBLL();
        //        ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //        ServiceResultList<AssembliesLabourDetailsList> response = new ServiceResultList<AssembliesLabourDetailsList>();
        //        ViewBag.AssembliesName = AssembliesName;
        //        ViewBag.Description = Description;
        //        string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //        var result = ClientResponse.GetClientName(Loginuser);
        //        response = laborDetailsResponse.GetMyAssembliesLaborDetailsList(result.Client_ID);
        //        TempData.Keep("MyLaborList");
        //        TempData.Keep("MyParts");
        //        List<AssembliesLabourDetailsList> selectedLaborList = TempData["MyLaborList"] as List<AssembliesLabourDetailsList> ?? new List<AssembliesLabourDetailsList>();
        //        //List<AssembliesLabourDetailsList> selectedLaborList = TempData["MyLaborList"] as List<AssembliesLabourDetailsList> ?? new List<AssembliesLabourDetailsList>();
        //        for (int i = 0; i < response.ListData.Count; i++)
        //        {
        //            if (selectedLaborList.Count > 0)
        //            {
        //                foreach (var list in selectedLaborList)
        //                {
        //                    if (list.Laborer_Name.Trim() == response.ListData[i].Laborer_Name.Trim())
        //                        response.ListData[i].isChekced = true;
        //                }
        //            }
        //            laborDetails.Add(new AssembliesLabourDetailsList { 
        //                Client_ID = response.ListData[0].Client_ID,
        //                Laborer_Name = response.ListData[0].Laborer_Name, 
        //                Labor_Category = response.ListData[0].Labor_Category, 
        //                Cost = response.ListData[0].Cost, Burden = response.ListData[0].Burden, 
        //                Resale_Cost = response.ListData[0].Resale_Cost, 
        //                Estimated_Hour = response.ListData[0].Estimated_Hour, 
        //                Actual_Hours = response.ListData[0].Actual_Hours });
        //        }
        //        return View(response.ListData);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[HttpPost]
        //public ActionResult AddLaborDetailsInWebGrid(FormCollection form, Assembly_MasterInfo model)
        //{
        //    AssembliesMasterBLL PartsResponse = new AssembliesMasterBLL();
        //    ServiceResultList<AssembliesLabourDetailsList> response = new ServiceResultList<AssembliesLabourDetailsList>();
        //    List<AssembliesParts_DetailsInfoList> partsListOBJ = new List<AssembliesParts_DetailsInfoList>();
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    List<Assembly_MasterInfoList> aslistdata = new List<Assembly_MasterInfoList>();
        //    List<AssembliesLabourDetailsList> LabourlistOBJ = new List<AssembliesLabourDetailsList>();
        //    ViewBag.AssembliesName = model.Assemblies_Name;
        //    ViewBag.Description = model.Assemblies_Description;
        //    if (TempData["PartsListData"] != null)
        //    {
        //        var ps = (Dictionary<string, string[]>)TempData["PartsListData"];
        //        foreach (string[] s in ps.Values)
        //        {
        //            AssembliesParts_DetailsInfoList obj = new AssembliesParts_DetailsInfoList();
        //            //obj.Client_ID = s[0];
        //            ///obj.Client_ID=Convert.ToInt32(s[0]);
        //            obj.Part_Number = s[0];
        //            // obj.Part_Category=s[2];
        //            obj.Part_Cost = Convert.ToDecimal(s[1]);
        //            obj.Resale_Cost = Convert.ToDecimal(s[2]);
        //            //obj. Purchased_From=s[5];
        //            obj.Estimated_Qty = Convert.ToInt32(s[3]);
        //            obj.Actual_Qty = Convert.ToInt32(s[4]);
        //            obj.isChekced = Convert.ToBoolean(true);
        //            // Client_ID = response.ListData[i].Client_ID, Part_Number = response.ListData[i].Part_Number, Part_Category = response.ListData[i].Part_Category, Cost = response.ListData[i].Cost, Resale_Cost = response.ListData[i].Resale_Cost, Purchased_From = response.ListData[i].Purchased_From, isChekced = response.ListData[i].isChekced });
        //            partsListOBJ.Add(new AssembliesParts_DetailsInfoList
        //            {
        //                //Client_ID = obj.Client_ID,
        //                Part_Number = obj.Part_Number,
        //                Part_Category = obj.Part_Category,
        //                Part_Cost = obj.Part_Cost,
        //                Resale_Cost = obj.Resale_Cost,
        //                Purchased_From = obj.Purchased_From,
        //                Actual_Qty = obj.Actual_Qty,
        //                Estimated_Qty = obj.Estimated_Qty,
        //                isChekced = Convert.ToBoolean(true)
        //            });
        //            //}
        //        }
        //    }

        //    TempData["MyParts"] = partsListOBJ;
        //    if (TempData["LabourListData"] != null)
        //    {
        //        var ls = (Dictionary<string, string[]>)TempData["LabourListData"];


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
        //                isChekced = Convert.ToBoolean(true)
        //            });
        //        }

        //    }
        //    var selectedLaborer = form.GetValues("assignChkBx");
        //    if (selectedLaborer != null)
        //    {
        //        foreach (var selectedList in selectedLaborer)
        //        {
        //            string[] splitData = selectedList.Split(',');
        //            //In argument we are passing clientID and laborerName to db.
        //            response = PartsResponse.GetMyAssembliesLaborerList(Convert.ToInt16(splitData[0]), splitData[1]);
        //            laborerList.Add(new AssembliesLabourDetailsList { Client_ID = response.ListData[0].Client_ID, Laborer_Name = response.ListData[0].Laborer_Name, Labor_Category = response.ListData[0].Labor_Category, Cost = response.ListData[0].Cost, Burden = response.ListData[0].Burden, Resale_Cost = response.ListData[0].Resale_Cost, Estimated_Hour = response.ListData[0].Estimated_Hour, Actual_Hours = response.ListData[0].Actual_Hours, isChekced=true });
        //        }
        //    }

        //    foreach (var newitem in laborerList)
        //    {
        //        foreach (var PreviousselectedItem in LabourlistOBJ)
        //        {
        //            if (PreviousselectedItem.Laborer_Name.Trim() == newitem.Laborer_Name.Trim() && PreviousselectedItem.isChekced == newitem.isChekced)
        //            {
        //                newitem.Laborer_Name = PreviousselectedItem.Laborer_Name;
        //                //newitem. Part_Category = selectedItem.Part_Category;
        //                newitem.Cost = PreviousselectedItem.Cost;
        //                newitem.Resale_Cost = PreviousselectedItem.Resale_Cost;
        //                // newitem .Purchased_From = selectedItem.Purchased_From;
        //                newitem.Actual_Hours = PreviousselectedItem.Actual_Hours;
        //                newitem.Estimated_Hour = PreviousselectedItem.Estimated_Hour;
        //                //isChekced = Convert.ToBoolean(true)

        //            }

        //        }
        //    }
        //    // ViewData["MyLaborList"] = laborerList;
        //    TempData["MyLaborList"] = laborerList;
        //    ViewBag.PartsData = TempData["MyParts"];
        //    // TempData.Keep("MyLaborList");

        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    aslistdata = AssembliesResponse.GetAssembliesList(result.Client_ID);
        //    model.asList = aslistdata;
        //    if (TempData["Edit"] != null)
        //    {
        //        ViewBag.ASEdit = TempData["Edit"];
        //        if (ViewBag.ASEdit != null)
        //        {
        //            return View("EditAssembliesDetails", model);

        //        }
        //    }

        //        return View("ASMIndex", model);


        //}


        //public ActionResult PartsGridData(string AssembliesName, string Description, string AssembliesCategory, string OtherAssembliesCategory, string PartCostTotal, string PartsResalTotal, string Laborcost, string LaborRcost, string Estimated_Hour, string LabortEstCostTot, string LaborEstResaleTot, string GrandCostTot, string GrandResaleTot, string PartgridData)
        //{
        //    if (PartgridData != null)
        //    {
        //        var log = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(PartgridData);
        //        TempData["PartsListData"] = log;
        //    }


        //    ASMasterTempObj.Assemblies_Name = AssembliesName;
        //    ASMasterTempObj.Assemblies_Description = Description;
        //    ASMasterTempObj.Assemblies_Category = AssembliesCategory;
        //    ASMasterTempObj.OtherAssemblies_Category = OtherAssembliesCategory;
        //    if (PartCostTotal != null && PartCostTotal != "")
        //    {
        //        ASMasterTempObj.PartCostTotal = Convert.ToDecimal(PartCostTotal);
        //    }
        //    if (PartsResalTotal != null && PartsResalTotal != "")
        //    {
        //        ASMasterTempObj.PartResaleTotal = Convert.ToDecimal(PartsResalTotal);
        //    }
        //    if (Laborcost != null && Laborcost != "")
        //    {
        //        ASMasterTempObj.labor_cost = Convert.ToDecimal(Laborcost);
        //    }
        //    if (LaborRcost != null && LaborRcost != "")
        //    {
        //        ASMasterTempObj.Lobor_Resale = Convert.ToDecimal(LaborRcost);
        //    }
        //    if (Estimated_Hour != null && Estimated_Hour != "")
        //    {
        //        ASMasterTempObj.Estimated_Hour = Convert.ToDecimal(Estimated_Hour);
        //    }
        //    if (LabortEstCostTot != null && LabortEstCostTot != "")
        //    {
        //        ASMasterTempObj.LaborEst_CostTotal = Convert.ToDecimal(LabortEstCostTot);
        //    }
        //    if (LaborEstResaleTot != null && LaborEstResaleTot != "")
        //    {
        //        ASMasterTempObj.LaborEst_ResaleTotal = Convert.ToDecimal(LaborEstResaleTot);
        //    }
        //    if (GrandCostTot != null && GrandCostTot != "")
        //    {
        //        ASMasterTempObj.GrandCostTotal = Convert.ToDecimal(GrandCostTot);
        //    }
        //    if (GrandResaleTot != null && GrandResaleTot != "")
        //    {
        //        ASMasterTempObj.GrandResaleTotal = Convert.ToDecimal(GrandResaleTot);
        //    }
        //    // var LabourList = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LgridData);
        //    //TempData["LabourListData"] = LabourList;
        //    // TempData["LabourListData"] = LgridData;
        //    //TempData["LabourListData"] = new JavaScriptSerializer().Deserialize<dynamic>(LgridData);
        //    //TempData["PartsListData"] = new JavaScriptSerializer().Deserialize<dynamic>(gridData);
        //    return Json("Success");
        //}


        //[HttpPost]
        //public JsonResult SaveAssembliesDetails(FormCollection form, Assembly_MasterInfo model)
        //{
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    List<AssembliesParts_DetailsInfoList> partsListOBJ = new List<AssembliesParts_DetailsInfoList>();
        //    List<AssembliesLabourDetailsList> LabourlistOBJ = new List<AssembliesLabourDetailsList>();
        //    ServiceResult<int> ASInfoIresponse = new ServiceResult<int>();
        //    //ServiceResult<int> ASPartsresponse = new ServiceResult<int>();
        //    // ServiceResult<int> ASLabourresponse = new ServiceResult<int>();
        //    List<Assembly_MasterInfoList> aslistdata = new List<Assembly_MasterInfoList>();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    String AssembliesName = model.Assemblies_Name;


        //    var result = ClientResponse.GetClientName(Loginuser);
        //    aslistdata = AssembliesResponse.GetAssembliesList(result.Client_ID);
        //    model.asList = aslistdata;

        //    if (ModelState.IsValid)
        //    {
        //        if (TempData["PartsListData"] != null)
        //        {
        //            var ps = (Dictionary<string, string[]>)TempData["PartsListData"];
        //            foreach (string[] s in ps.Values)
        //            {
        //                AssembliesParts_DetailsInfoList obj = new AssembliesParts_DetailsInfoList();
        //                //obj.Client_ID = s[0];
        //                ///obj.Client_ID=Convert.ToInt32(s[0]);
        //                obj.Part_Number = s[0];
        //                obj.Part_Category = s[1];
        //                obj.Part_Cost = Convert.ToDecimal(s[2]);
        //                obj.Resale_Cost = Convert.ToDecimal(s[3]);
        //                obj.Estimated_Qty = Convert.ToInt32(s[4]);
        //                obj.EstCost_Total = Convert.ToDecimal(s[5]);
        //                obj.EstResaleCost_Total = Convert.ToDecimal(s[6]);

        //                //obj.Actual_Qty = Convert.ToInt32(s[4]);
        //                obj.isChekced = Convert.ToBoolean(true);
        //                // Client_ID = response.ListData[i].Client_ID, Part_Number = response.ListData[i].Part_Number, Part_Category = response.ListData[i].Part_Category, Cost = response.ListData[i].Cost, Resale_Cost = response.ListData[i].Resale_Cost, Purchased_From = response.ListData[i].Purchased_From, isChekced = response.ListData[i].isChekced });
        //                partsListOBJ.Add(new AssembliesParts_DetailsInfoList
        //                {
        //                    //Client_ID = obj.Client_ID,
        //                    Part_Number = obj.Part_Number,
        //                    Part_Category = obj.Part_Category,
        //                    Part_Cost = obj.Part_Cost,
        //                    Resale_Cost = obj.Resale_Cost,
        //                    Estimated_Qty = obj.Estimated_Qty,
        //                    EstCost_Total = obj.EstCost_Total,
        //                    EstResaleCost_Total = obj.EstResaleCost_Total,
        //                    isChekced = Convert.ToBoolean(true)
        //                });
        //                //}
        //            }
        //        }



        //        model.PartsListData = partsListOBJ;
        //        model.Assemblies_Name = ASMasterTempObj.Assemblies_Name;
        //        model.Assemblies_Description = ASMasterTempObj.Assemblies_Description;
        //        model.Assemblies_Category = ASMasterTempObj.Assemblies_Category;
        //        model.OtherAssemblies_Category = ASMasterTempObj.OtherAssemblies_Category;
        //        model.Estimated_Qty = ASMasterTempObj.Est_Qty_Total;
        //        model.PartCostTotal = ASMasterTempObj.PartCostTotal;
        //        model.PartResaleTotal = ASMasterTempObj.PartResaleTotal;
        //        model.labor_cost = ASMasterTempObj.labor_cost;
        //        model.Lobor_Resale = ASMasterTempObj.Lobor_Resale;
        //        model.Estimated_Hour = ASMasterTempObj.Estimated_Hour;
        //        model.LaborEst_CostTotal = ASMasterTempObj.LaborEst_CostTotal;
        //        model.LaborEst_ResaleTotal = ASMasterTempObj.LaborEst_ResaleTotal;
        //        model.GrandCostTotal = ASMasterTempObj.GrandCostTotal;
        //        model.GrandResaleTotal = ASMasterTempObj.GrandResaleTotal;
        //        model.Client_ID = result.Client_ID;
        //        model.Created_By = Loginuser;
        //        if (model.PartsListData.Count() != 0)
        //        {
        //            // ASInfoIresponse = AssembliesResponse.AddNewAssembliesInformation(model);
        //            ASInfoIresponse = AssembliesResponse.AddNewAssembliesDetails(model);
        //            if (ASInfoIresponse.ResultCode > 0)
        //            {
        //                ViewBag.ASInfoSuccessMsg = ASInfoIresponse.Message;
        //                TempData["ASMSuccessMsg"] = ASInfoIresponse.Message;
        //                //return RedirectToAction("ASMIndex");
        //                return Json("");
        //                //ASPartsresponse = AssembliesResponse.AddNewAssembliesPartsDeatails(model);
        //            }
        //            else
        //            {
        //                ViewBag.ASInfoFailMsg = ASInfoIresponse.Message;
        //                //return View("ASMIndex", model);
        //                return Json("");
        //            }

        //        }
        //        else
        //        {
        //            ViewBag.ASInfoFailMsg = "Error Occured While Saving Assemblies Details!";
        //            //return View("ASMIndex", model);
        //            return Json("");
        //        }
        //    }

        //    else
        //    {
        //        //return View("ASMIndex", model);
        //        return Json("");
        //    }

        //    return Json("");
        //}






        //public ActionResult AddAssemblyPartsInEdit(string AssembliesName, string Description, string AssembliesCategory, string OtherAssembliesCategory, string LoborResale, string LoborCost, string EstimatedHour, string PartgridData)
        //{
        //    if (PartgridData != null)
        //    {
        //        var log = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(PartgridData);
        //        TempData["PartsListData"] = log;
        //    }
        //    //if (LabourgridData != null)
        //    //{
        //    //    var LabourList = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LabourgridData);
        //    //    TempData["LabourListData"] = LabourList;
        //    //}
        //    ASMasterTempObj.Assemblies_Name = AssembliesName;
        //    ASMasterTempObj.Assemblies_Description = Description;
        //    ASMasterTempObj.Assemblies_Category = AssembliesCategory;
        //    ASMasterTempObj.OtherAssemblies_Category = OtherAssembliesCategory;
        //    if (LoborCost != null && LoborCost != "")
        //    {
        //        ASMasterTempObj.labor_cost = Convert.ToDecimal(LoborCost);
        //    }
        //    if (LoborResale != null && LoborResale != "")
        //    {
        //        ASMasterTempObj.Lobor_Resale = Convert.ToDecimal(LoborResale);
        //    }
        //    if (EstimatedHour != null && EstimatedHour != "")
        //    {
        //        ASMasterTempObj.Estimated_Hour = Convert.ToDecimal(EstimatedHour);
        //    }
        //    PartsMasterBLL PartsResponse = new PartsMasterBLL();
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
        //    ViewBag.AssembliesName = AssembliesName;
        //    ViewBag.Description = Description;
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    TempData.Keep("MyParts");
        //    if (Loginuser == "Admin")
        //    {
        //        response = PartsResponse.GetPartsDetalisList();
        //        return View(response.ListData);
        //    }
        //    else
        //    {
        //        var result = ClientResponse.GetClientName(Loginuser);
        //        response = PartsResponse.GetMyPartsList(result.Client_ID);
        //        List<AssembliesParts_DetailsInfoList> selectedParts = TempData["MyParts"] as List<AssembliesParts_DetailsInfoList> ?? new List<AssembliesParts_DetailsInfoList>();
        //        for (int i = 0; i < response.ListData.Count; i++)
        //        {
        //            if (selectedParts.Count > 0)
        //            {
        //                foreach (AssembliesParts_DetailsInfoList list in selectedParts)
        //                {
        //                    if (list.Part_Number.Trim() == response.ListData[i].Part_Number.Trim())
        //                    {
        //                        response.ListData[i].isChekced = true;
        //                    }
        //                }
        //            }
        //            partsList.Add(new Parts_DetailsInfoList
        //            {
        //                Client_ID = response.ListData[i].Client_ID,
        //                Part_Number = response.ListData[i].Part_Number.Trim(),
        //                Part_Category = response.ListData[i].Part_Category,
        //                Part_Cost = response.ListData[i].Cost,
        //                Resale_Cost = response.ListData[i].Resale_Cost,
        //                Purchased_From = response.ListData[i].Purchased_From,
        //                isChekced = response.ListData[i].isChekced
        //            });
        //        }
        //        ViewBag.ASEdit = "Edit";
        //        // return View(response.ListData);
        //        return View(partsList.ToList());
        //    }
        //}

        //[HttpPost]
        //public ActionResult AddPartsInGridInEdit(FormCollection form, Assembly_MasterInfo model)
        //{
        //    AssembliesMasterBLL PartsResponse = new AssembliesMasterBLL();
        //    ServiceResultList<AssembliesParts_DetailsInfoList> response = new ServiceResultList<AssembliesParts_DetailsInfoList>();
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    List<Assembly_MasterInfoList> aslistdata = new List<Assembly_MasterInfoList>();
        //    List<AssembliesParts_DetailsInfoList> partsListOBJ = new List<AssembliesParts_DetailsInfoList>();
        //    // List<AssembliesLabourDetailsList> LabourlistOBJ = new List<AssembliesLabourDetailsList>();
        //    ViewBag.AssembliesName = model.Assemblies_Name;
        //    ViewBag.Description = model.Assemblies_Description;



        //    if (TempData["PartsListData"] != null)
        //    {
        //        var ps = (Dictionary<string, string[]>)TempData["PartsListData"];
        //        foreach (string[] s in ps.Values)
        //        {
        //            AssembliesParts_DetailsInfoList obj = new AssembliesParts_DetailsInfoList();
        //            obj.Part_Number = s[0];
        //            obj.Part_Category = s[1];
        //            obj.Part_Cost = Convert.ToDecimal(s[2]);
        //            obj.Estimated_Qty = Convert.ToInt32(s[3]);
        //            obj.isChekced = Convert.ToBoolean(true);
        //            // Client_ID = response.ListData[i].Client_ID, Part_Number = response.ListData[i].Part_Number, Part_Category = response.ListData[i].Part_Category, Cost = response.ListData[i].Cost, Resale_Cost = response.ListData[i].Resale_Cost, Purchased_From = response.ListData[i].Purchased_From, isChekced = response.ListData[i].isChekced });
        //            partsListOBJ.Add(new AssembliesParts_DetailsInfoList
        //            {
        //                //Client_ID = obj.Client_ID,
        //                Part_Number = obj.Part_Number.Trim(),
        //                Part_Category = obj.Part_Category,
        //                Part_Cost = obj.Part_Cost,
        //                //Resale_Cost = obj.Resale_Cost,
        //                //Purchased_From = obj.Purchased_From,
        //                //Actual_Qty = obj.Actual_Qty,
        //                Estimated_Qty = obj.Estimated_Qty,
        //                isChekced = Convert.ToBoolean(true)
        //            });
        //            //}
        //        }
        //    }




        //    var selectedValue = form.GetValues("assignChkBx");
        //    if (selectedValue != null)
        //    {
        //        foreach (var selectedPart in selectedValue)
        //        {
        //            string[] splitData = selectedPart.Split(',');
        //            //In argument we are passing clientID and partId to db.
        //            response = PartsResponse.GetMyAssembliesPartsList(Convert.ToInt16(splitData[0]), splitData[1]);
        //            parts.Add(new AssembliesParts_DetailsInfoList
        //            {
        //                Part_Number = response.ListData[0].Part_Number,
        //                Part_Category = response.ListData[0].Part_Category,
        //                Part_Cost = response.ListData[0].Cost,
        //                Resale_Cost = response.ListData[0].Resale_Cost,
        //                Purchased_From = response.ListData[0].Purchased_From,
        //                Estimated_Qty = response.ListData[0].Estimated_Qty,
        //                Actual_Qty = response.ListData[0].Actual_Qty,
        //                isChekced = true
        //            });
        //        }
        //        model.PartsListData = parts;

        //        if (partsListOBJ.Count != 0)
        //        {
        //            foreach (var newitem in parts)
        //            {
        //                foreach (var PreviousselectedItem in partsListOBJ)
        //                {
        //                    if (PreviousselectedItem.Part_Number.Trim() == newitem.Part_Number.Trim() && PreviousselectedItem.isChekced == newitem.isChekced)
        //                    {
        //                        newitem.Part_Number = PreviousselectedItem.Part_Number.Trim();
        //                        //newitem. Part_Category = selectedItem.Part_Category;
        //                        newitem.Part_Cost = PreviousselectedItem.Part_Cost;
        //                        newitem.Resale_Cost = PreviousselectedItem.Resale_Cost;
        //                        // newitem .Purchased_From = selectedItem.Purchased_From;
        //                        newitem.Actual_Qty = PreviousselectedItem.Actual_Qty;
        //                        newitem.Estimated_Qty = PreviousselectedItem.Estimated_Qty;
        //                        //isChekced = Convert.ToBoolean(true)

        //                    }

        //                }


        //            }
        //        }
        //        // ViewData["MyParts"] = parts;
        //        TempData["MyParts"] = parts;
        //        ViewBag.PartsData = parts;
        //        TempData.Peek("MyParts");
        //    }


        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    aslistdata = AssembliesResponse.GetAssembliesList(result.Client_ID);
        //    model.asList = aslistdata;
        //    ViewBag.ASEdit = "Edit";
        //    return View("EditAssembliesDetails", model);
        //}

        //public ActionResult AddAssembliesLaborDetailsInEdit(string AssembliesName, string Description, string PartgridData, string LabourgridData)
        //{
        //    try
        //    {
        //        if (PartgridData != null)
        //        {
        //            var log = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(PartgridData);
        //            TempData["PartsListData"] = log;
        //        }
        //        if (LabourgridData != null)
        //        {
        //            var LabourList = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string[]>>(LabourgridData);
        //            TempData["LabourListData"] = LabourList;
        //        }
        //        AssembliesMasterBLL laborDetailsResponse = new AssembliesMasterBLL();
        //        ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //        ServiceResultList<AssembliesLabourDetailsList> response = new ServiceResultList<AssembliesLabourDetailsList>();
        //        ViewBag.AssembliesName = AssembliesName;
        //        ViewBag.Description = Description;
        //        string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //        var result = ClientResponse.GetClientName(Loginuser);
        //        response = laborDetailsResponse.GetMyAssembliesLaborDetailsList(result.Client_ID);
        //        TempData.Keep("MyLaborList");
        //        TempData.Keep("MyParts");
        //        List<AssembliesLabourDetailsList> selectedLaborList = TempData["MyLaborList"] as List<AssembliesLabourDetailsList> ?? new List<AssembliesLabourDetailsList>();
        //        //List<AssembliesLabourDetailsList> selectedLaborList = TempData["MyLaborList"] as List<AssembliesLabourDetailsList> ?? new List<AssembliesLabourDetailsList>();
        //        for (int i = 0; i < response.ListData.Count; i++)
        //        {
        //            if (selectedLaborList.Count > 0)
        //            {
        //                foreach (var list in selectedLaborList)
        //                {
        //                    if (list.Laborer_Name.Trim() == response.ListData[i].Laborer_Name.Trim())
        //                        response.ListData[i].isChekced = true;
        //                }
        //            }
        //            laborDetails.Add(new AssembliesLabourDetailsList
        //            {
        //                Client_ID = response.ListData[0].Client_ID,
        //                Laborer_Name = response.ListData[0].Laborer_Name,
        //                Labor_Category = response.ListData[0].Labor_Category,
        //                Cost = response.ListData[0].Cost,
        //                Burden = response.ListData[0].Burden,
        //                Resale_Cost = response.ListData[0].Resale_Cost,
        //                Estimated_Hour = response.ListData[0].Estimated_Hour,
        //                Actual_Hours = response.ListData[0].Actual_Hours
        //            });
        //        }
        //        ViewBag.ASEdit = "Edit";
        //        return View(response.ListData);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[HttpPost]
        //public ActionResult AddLaborDetailsInWebGridInEdit(FormCollection form, Assembly_MasterInfo model)
        //{
        //    AssembliesMasterBLL PartsResponse = new AssembliesMasterBLL();
        //    ServiceResultList<AssembliesLabourDetailsList> response = new ServiceResultList<AssembliesLabourDetailsList>();
        //    List<AssembliesParts_DetailsInfoList> partsListOBJ = new List<AssembliesParts_DetailsInfoList>();
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    List<Assembly_MasterInfoList> aslistdata = new List<Assembly_MasterInfoList>();
        //    List<AssembliesLabourDetailsList> LabourlistOBJ = new List<AssembliesLabourDetailsList>();
        //    ViewBag.AssembliesName = model.Assemblies_Name;
        //    ViewBag.Description = model.Assemblies_Description;
        //    if (TempData["PartsListData"] != null)
        //    {
        //        var ps = (Dictionary<string, string[]>)TempData["PartsListData"];
        //        foreach (string[] s in ps.Values)
        //        {
        //            AssembliesParts_DetailsInfoList obj = new AssembliesParts_DetailsInfoList();
        //            //obj.Client_ID = s[0];
        //            ///obj.Client_ID=Convert.ToInt32(s[0]);
        //            obj.Part_Number = s[0];
        //            // obj.Part_Category=s[2];
        //            obj.Part_Cost = Convert.ToDecimal(s[1]);
        //            obj.Resale_Cost = Convert.ToDecimal(s[2]);
        //            //obj. Purchased_From=s[5];
        //            obj.Estimated_Qty = Convert.ToInt32(s[3]);
        //            obj.Actual_Qty = Convert.ToInt32(s[4]);
        //            obj.isChekced = Convert.ToBoolean(true);
        //            // Client_ID = response.ListData[i].Client_ID, Part_Number = response.ListData[i].Part_Number, Part_Category = response.ListData[i].Part_Category, Cost = response.ListData[i].Cost, Resale_Cost = response.ListData[i].Resale_Cost, Purchased_From = response.ListData[i].Purchased_From, isChekced = response.ListData[i].isChekced });
        //            partsListOBJ.Add(new AssembliesParts_DetailsInfoList
        //            {
        //                //Client_ID = obj.Client_ID,
        //                Part_Number = obj.Part_Number.Trim(),
        //                Part_Category = obj.Part_Category,
        //                Part_Cost = obj.Part_Cost,
        //                Resale_Cost = obj.Resale_Cost,
        //                Purchased_From = obj.Purchased_From,
        //                Actual_Qty = obj.Actual_Qty,
        //                Estimated_Qty = obj.Estimated_Qty,
        //                isChekced = Convert.ToBoolean(true)
        //            });
        //            //}
        //        }
        //    }

        //    TempData["MyParts"] = partsListOBJ;
        //    if (TempData["LabourListData"] != null)
        //    {
        //        var ls = (Dictionary<string, string[]>)TempData["LabourListData"];


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
        //                isChekced = Convert.ToBoolean(true)
        //            });
        //        }

        //    }
        //    var selectedLaborer = form.GetValues("assignChkBx");
        //    if (selectedLaborer != null)
        //    {
        //        foreach (var selectedList in selectedLaborer)
        //        {
        //            string[] splitData = selectedList.Split(',');
        //            //In argument we are passing clientID and laborerName to db.
        //            response = PartsResponse.GetMyAssembliesLaborerList(Convert.ToInt16(splitData[0]), splitData[1]);
        //            laborerList.Add(new AssembliesLabourDetailsList { Client_ID = response.ListData[0].Client_ID, Laborer_Name = response.ListData[0].Laborer_Name, Labor_Category = response.ListData[0].Labor_Category, Cost = response.ListData[0].Cost, Burden = response.ListData[0].Burden, Resale_Cost = response.ListData[0].Resale_Cost, Estimated_Hour = response.ListData[0].Estimated_Hour, Actual_Hours = response.ListData[0].Actual_Hours, isChekced = true });
        //        }
        //    }

        //    foreach (var newitem in laborerList)
        //    {
        //        foreach (var PreviousselectedItem in LabourlistOBJ)
        //        {
        //            if (PreviousselectedItem.Laborer_Name.Trim() == newitem.Laborer_Name.Trim() && PreviousselectedItem.isChekced == newitem.isChekced)
        //            {
        //                newitem.Laborer_Name = PreviousselectedItem.Laborer_Name;
        //                //newitem. Part_Category = selectedItem.Part_Category;
        //                newitem.Cost = PreviousselectedItem.Cost;
        //                newitem.Resale_Cost = PreviousselectedItem.Resale_Cost;
        //                // newitem .Purchased_From = selectedItem.Purchased_From;
        //                newitem.Actual_Hours = PreviousselectedItem.Actual_Hours;
        //                newitem.Estimated_Hour = PreviousselectedItem.Estimated_Hour;
        //                //isChekced = Convert.ToBoolean(true)

        //            }

        //        }
        //    }
        //    // ViewData["MyLaborList"] = laborerList;
        //    TempData["MyLaborList"] = laborerList;
        //    ViewBag.PartsData = TempData["MyParts"];
        //    // TempData.Keep("MyLaborList");

        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    aslistdata = AssembliesResponse.GetAssembliesList(result.Client_ID);
        //    model.asList = aslistdata;

        //    ViewBag.ASEdit = "Edit";
        //    return View("EditAssembliesDetails", model);


        //}
        //public ActionResult EditAssembliesDetails(string AssembliesName)
        //{
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    List<Assembly_MasterInfoList> aslistdata = new List<Assembly_MasterInfoList>();

        //    response = AssembliesResponse.GetAssembliesListDetails(AssembliesName, result.Client_ID);

        //    //List<AssembliesParts_DetailsInfoList> selectedParts = TempData["MyParts"] as List<AssembliesParts_DetailsInfoList> ?? new List<AssembliesParts_DetailsInfoList>();
        //    for (int i = 0; i < response.Data.PartsListData.Count; i++)
        //    {
        //        response.Data.PartsListData[i].isChekced = true;
        //    }
        //    TempData["MyParts"] = response.Data.PartsListData;
        //    // ViewBag.PartsData = parts;
        //    TempData.Peek("MyParts");


        //    //// List<AssembliesLabourDetailsList> selectedLaborList = TempData["MyLaborList"] as List<AssembliesLabourDetailsList> ?? new List<AssembliesLabourDetailsList>();
        //    //for (int i = 0; i < response.Data.LabourListData.Count; i++)
        //    //{
        //    //    response.Data.LabourListData[i].isChekced = true;

        //    //}
        //    //TempData["MyLaborList"] = response.Data.LabourListData;
        //    // ViewBag.PartsData = parts;
        //    //  TempData.Peek("MyLaborList");
        //    ViewBag.EditAssembliesDetails = "Edit";
        //    aslistdata = AssembliesResponse.GetAssembliesList(result.Client_ID);
        //    response.Data.asList = aslistdata;
        //    //TempData["Edit"] = "EdIt";
        //    ViewBag.ASEdit = "Edit";
        //    return View(response.Data);
        //}

        //[HttpPost]
        //public ActionResult UpdateAssembliesDetails(Assembly_MasterInfo model)
        //{
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    List<AssembliesParts_DetailsInfoList> partsListOBJ = new List<AssembliesParts_DetailsInfoList>();
        //    List<AssembliesLabourDetailsList> LabourlistOBJ = new List<AssembliesLabourDetailsList>();
        //    ServiceResult<int> response = new ServiceResult<int>();
        //    // List<Assembly_MasterInfoList> aslistdata = new List<Assembly_MasterInfoList>();

        //    String AssembliesName = model.Assemblies_Name;
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    //aslistdata = AssembliesResponse.GetAssembliesList(result.Client_ID);
        //    //model.asList = aslistdata;

        //    if (TempData["PartsListData"] != null)
        //    {
        //        var ps = (Dictionary<string, string[]>)TempData["PartsListData"];
        //        foreach (string[] s in ps.Values)
        //        {
        //            AssembliesParts_DetailsInfoList obj = new AssembliesParts_DetailsInfoList();
        //            //obj.Client_ID = s[0];
        //            ///obj.Client_ID=Convert.ToInt32(s[0]);
        //            obj.Part_Number = s[0];
        //            obj.Part_Category = s[1];
        //            obj.Part_Cost = Convert.ToDecimal(s[2]);
        //            obj.Resale_Cost = Convert.ToDecimal(s[3]);
        //            obj.Estimated_Qty = Convert.ToInt32(s[4]);
        //            obj.EstCost_Total = Convert.ToDecimal(s[5]);
        //            obj.EstResaleCost_Total = Convert.ToDecimal(s[6]);

        //            //obj.Actual_Qty = Convert.ToInt32(s[4]);
        //            obj.isChekced = Convert.ToBoolean(true);
        //            // Client_ID = response.ListData[i].Client_ID, Part_Number = response.ListData[i].Part_Number, Part_Category = response.ListData[i].Part_Category, Cost = response.ListData[i].Cost, Resale_Cost = response.ListData[i].Resale_Cost, Purchased_From = response.ListData[i].Purchased_From, isChekced = response.ListData[i].isChekced });
        //            partsListOBJ.Add(new AssembliesParts_DetailsInfoList
        //            {
        //                //Client_ID = obj.Client_ID,
        //                Part_Number = obj.Part_Number,
        //                Part_Category = obj.Part_Category,
        //                Part_Cost = obj.Part_Cost,
        //                Resale_Cost = obj.Resale_Cost,
        //                Estimated_Qty = obj.Estimated_Qty,
        //                EstCost_Total = obj.EstCost_Total,
        //                EstResaleCost_Total = obj.EstResaleCost_Total,
        //                isChekced = Convert.ToBoolean(true)
        //            });
        //            //}
        //        }
        //    }



        //    model.PartsListData = partsListOBJ;
        //    model.Assemblies_Name = ASMasterTempObj.Assemblies_Name;
        //    model.Assemblies_Description = ASMasterTempObj.Assemblies_Description; ;
        //    model.Assemblies_Category = ASMasterTempObj.Assemblies_Category; ;
        //    model.OtherAssemblies_Category = ASMasterTempObj.OtherAssemblies_Category; ;
        //    model.labor_cost = ASMasterTempObj.labor_cost;
        //    model.Lobor_Resale = ASMasterTempObj.Lobor_Resale;
        //    model.Estimated_Hour = ASMasterTempObj.Estimated_Hour;
        //    model.Client_ID = result.Client_ID;
        //    model.Updated_By = Loginuser;
        //    response = AssembliesResponse.UpdateAssembliesDetails(model);
        //    if (response.ResultCode > 0)
        //    {
        //        TempData["AssembliesUpdateSuccessMsg"] = response.Message;
        //        return RedirectToAction("ASMIndex");
        //    }
        //    else
        //    {
        //        TempData["AssembliesUpdateFailMsg"] = response.Message;
        //        return RedirectToAction("EditAssembliesDetails", model);
        //    }



        //}

        #endregion

    }
}
