using ElectricEase.BLL.AssembliesMaster;
using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.Master;
using ElectricEase.BLL.PartsMaster;
using ElectricEase.Data.DataBase;
using ElectricEase.Data.PartsMaster;
using ElectricEase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ElectricEase.Web.Controllers
{
    public class NationwideAssemblyController : Controller
    {
        List<AssembliesParts_DetailsInfoList> parts = new List<AssembliesParts_DetailsInfoList>();
        AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //
        // GET: /NationwideAssemblyMaster/ 

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AssemblyInfo(int AssebliesId = 0)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
            ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            Assembly_MasterInfo model = new Assembly_MasterInfo();
            //model.Client_ID = result.Client_ID;
            ViewBag.Edit = "";
            if (AssebliesId != 0)
            {
                ViewBag.Edit = "EditAS";
                CheckAssemblesnameIsExist = AssembliesResponse.NationwideAssemblieNameIsExist(AssebliesId);
                if (CheckAssemblesnameIsExist.ResultCode == 1)
                {
                    response = AssembliesResponse.GetNationWideAssembliesListDetails(AssebliesId);
                    ViewBag.ASname = response.Data.Assemblies_Name;
                    model = response.Data;
                }

            }
            return PartialView("_AssemblyInfo", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult SelectedAssemblyPartsLaborInfo(string lstparts, int AssebliesId = 0, string SelectedAssemblyPartsLaborInfo = "", string SearchStr = null)
        {
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            string[] partslist;
            if (lstparts == "selectall")
            {
                partslist = PartsResponse.SearchNationalWidePartsData(SearchStr, null).Select(a => a.Part_Number).ToArray();
            }
            else
            {
                partslist = lstparts.Split(',');
            }
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
            CheckAssemblesnameIsExist = AssembliesResponse.NationwideAssemblieNameIsExist(AssebliesId);
            if (CheckAssemblesnameIsExist.ResultCode == 0)
            {
                foreach (string item in partslist)
                {
                    if (item != "" && item != "selectall")
                    {

                            AsResponse = AssembliesResponse.GetNationWideAssembliesListDetails(AssebliesId);
                            model.assmeblymasterinfo = AsResponse.Data;


                        ViewBag.Edit = "EditAS";

                        response = AssembliesResponse.GetMyNationWideAssembliesPartsList(item.ToString());
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
                model.partslist = parts;
            }
            else
            {
                AsResponse = AssembliesResponse.GetNationWideAssembliesListDetails(AssebliesId);
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
                                    response = AssembliesResponse.GetMyNationWideAssembliesPartsList(item.ToString());
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
                                            // Actual_Qty = response.ListData[0].Actual_Qty,
                                            isChekced = true,
                                            LaborUnit = response.ListData[0].LaborUnit,
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
                            response = AssembliesResponse.GetMyNationWideAssembliesPartsList(item.ToString());
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
                                    // Actual_Qty = response.ListData[0].Actual_Qty,
                                    isChekced = true,
                                    IsActivePartsDetails = response.ListData[0].IsActivePartsDetails,
                                    LaborUnit = response.ListData[0].LaborUnit,

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

        public ActionResult getallAssembliesList(string assembName = "")
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            Assembly_MasterInfo model = new Assembly_MasterInfo();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.GetNationWideAssembliesList();
            if (assembName == "")
                model.asList = response;
            else
                model.asList = response.Where(x => x.Assemblies_Category == assembName).ToList();
            ViewBag.AssembliesName = null;
            ViewBag.Description = null;
            ViewBag.AssembliesList = "Show";
            return PartialView("_AssembliesList", model);

        }

        //public ActionResult AssemblyPartsLaborInfo(string name = "", List<ListPart> lstparts = null)
        //{
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
        //    ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
        //    ServiceResult<bool> CheckAssemblesnameIsExist = new ServiceResult<bool>();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    AssemblyPartsModel model = new AssemblyPartsModel();
        //    model.partslist = new List<AssembliesParts_DetailsInfoList>();
        //    if (name != "")
        //    {
        //        CheckAssemblesnameIsExist = AssembliesResponse.NationwideAssemblieNameIsExist(name);
        //        if (CheckAssemblesnameIsExist.ResultCode == 1)
        //        {
        //            response = AssembliesResponse.GetNationWideAssembliesListDetails(name);
        //            model.assmeblymasterinfo = response.Data;
        //            model.partslist = response.Data.PartsListData.ToList();
        //            ViewBag.Edit = "EditAS";
        //        }

        //    }

        //    return PartialView("_Partslaborinfo", model);
        //}

        [HttpPost]
        public ActionResult GetallAssemblyParts(string lstparts = "", string PartCatgory = "", int AssebliesId = 0)
        {
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            ServiceResult<bool> CheckAssemblesnameIsExist = AssembliesResponse.NationwideAssemblieNameIsExist(AssebliesId);
            if (CheckAssemblesnameIsExist.ResultCode == 0)
            {
                ViewBag.Edit = "";

            }
            return PartialView("_showallParts");
        }

        public JsonResult PartsDatatable(DataTableAjaxPostModel model)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = GetNationalWidePartsDetalis(model, out filteredResultsCount, out totalResultsCount);

            var result = new List<Parts_DetailsInfoList>(res.Count);

            int i = 1;
            foreach (var m in res)
            {
                result.Add(new Parts_DetailsInfoList
                {
                    Client_ID = m.Client_Id,
                    Part_Category = m.Part_Category,
                    Part_Number = m.Part_Number,
                    Cost = m.Cost,
                    Resale_Cost = m.Resale_Cost ?? 0,
                    Purchased_From = m.Purchased_From,
                    Description = m.Description,
                    UOM = m.UOM ?? "",
                    ID = i,
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

        public IList<Super_Admin_Parts_Details> GetNationalWidePartsDetalis(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
            var result = PartsResponse.GetNationalWidePartsData(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, model.extra_search);
            if (result == null)
            {
                // empty collection...
                return new List<Super_Admin_Parts_Details>();
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
                ASInfoIresponse = AssembliesResponse.AddNewNationWideAssembliesDetails(model);
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
            //model.Client_ID = result.Client_ID;
            model.Created_By = Loginuser;
            model.Updated_By = Loginuser;
            if (model.PartsListData.Count() != 0)
            {
                // ASInfoIresponse = AssembliesResponse.AddNewAssembliesInformation(model);
                ASInfoIresponse = AssembliesResponse.UpdateNationWideAssembliesDetails(model);
                if (ASInfoIresponse.ResultCode > 0)
                {
                    ViewBag.ASInfoSuccessMsg = ASInfoIresponse.Message;
                    TempData["ASMupdateSuccessMsg"] = ASInfoIresponse.Message;
                    return Json(ASInfoIresponse.Message);

                }
                else
                {
                    ViewBag.ASInfoFailMsg = ASInfoIresponse.Message;
                    TempData["ASMupdateSuccessMsg"] = ASInfoIresponse.Message;
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

        public ActionResult DeleteAssembliesDetails(string AssembliesName, int AssemblieId)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.DeleteNationaWideAssembliesDetails(AssembliesName, AssemblieId, Loginuser);
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
        public ActionResult AssemblyExport()
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
            ServiceResultList<Standaredclient> standareduser = new ServiceResultList<Standaredclient>();
            standareduser = ClientResponse.StandaredUsers();
            ViewBag.Standared = new SelectList(standareduser.ListData, "value", "Name");
            return View();
        }

        public ActionResult GetNationAssembliesList(string assembName = "")
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            AssembliesMasterBLL AssembliesResponse = new AssembliesMasterBLL();
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            Assembly_MasterInfo model = new Assembly_MasterInfo();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            response = AssembliesResponse.GetNationWideAssembliesList();
            response = response.Where(x => x.IsActive == true).ToList();
            if (assembName == "")
                model.asList = response;
            else
                model.asList = response.Where(x => x.Assemblies_Category == assembName).ToList();
            ViewBag.AssembliesName = null;
            ViewBag.Description = null;
            ViewBag.AssembliesList = "Show";
            return PartialView("_GetNationAssembliesList", model);
        }
        [HttpPost]
        public ActionResult AssmbliesExport(selectedAssembilesIds model)
        {
            
            string result = AssembliesResponse.AssemblyExport(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNationWideAssembliesGrid()
        {
            var assemblyList = AssembliesResponse.GetNationWideAssembliesGrid();
            return Json(new { data = assemblyList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateLaborUnit(selectedAssembilesIds model)
        {
            string result = AssembliesResponse.ValidateLaborUnit(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //

            /*
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                      bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
        */

        public JsonResult AssembliesDatatable(DataTableAjaxPostModel model)
        {
            var results = new List<Dictionary<string, object>>();
            var filteredCount = 0;

            string queryString = @"
                SELECT
                    saam.*,
	                (SELECT COUNT(saap.Part_ID) FROM Super_Admin_Assemblies_Parts AS saap WHERE saam.Assemblies_Id = saap.Assemblies_Id) AS Parts_Count
                FROM
                    Super_Admin_Assemblies_Master AS saam
            ";

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                //connection.Open();

                //

                DataTable dt = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(dt);

                var sampleResults = dt.AsEnumerable()
                    .Where(row => row.Field<bool>("IsActive") == true)  
                ;

                var categoryFilter = model.columns[1].search.value;
                var filteredByCategory = sampleResults;
                if (String.IsNullOrWhiteSpace(categoryFilter) == false)
                {
                    filteredByCategory = sampleResults
                        .Where(row =>
                            row["Assemblies_Category"].ToString().ToLower().Equals(categoryFilter.ToLower())
                        )
                    ;
                }

                var filtered = filteredByCategory;
                if (String.IsNullOrWhiteSpace(model.search.value) == false)
                {
                    filtered = filteredByCategory
                        .Where(row => 
                            row["Assemblies_Name"].ToString().ToLower().Contains(model.search.value.ToLower())
                            || row["Assemblies_Category"].ToString().ToLower().Contains(model.search.value.ToLower())
                            || row["Assemblies_Description"].ToString().ToLower().Contains(model.search.value.ToLower())
                        )
                    ;
                }
                filteredCount = filtered.Count();

                var OrderByName = model.columns[model.order[0].column].data;
                var sorted = filtered.OrderBy(row => row[OrderByName]);
                if (model.order[0].dir == "desc")
                {
                    sorted = filtered.OrderByDescending(row => row[OrderByName]);
                }

                var paginated = sorted.Skip(model.start).Take(model.length);

                foreach (DataRow row in paginated)
                {
                    var result = new Dictionary<string, object>();

                    result.Add("Assemblies_Id", row["Assemblies_Id"]);
                    result.Add("Assemblies_Name", row["Assemblies_Name"]);
                    result.Add("Assemblies_Category", row["Assemblies_Category"]);
                    result.Add("Assemblies_Description", row["Assemblies_Description"]);
                    result.Add("Parts_Count", row["Parts_Count"]);
                    result.Add("IsActive", row["IsActive"]);

                    results.Add(result);
                }

                //

                //connection.Close();
            }

            return Json(new
            {
                draw = model.draw,
                recordsTotal = filteredCount,
                recordsFiltered = filteredCount,
                data = results
            });
        }
    }
}
