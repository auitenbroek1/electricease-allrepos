using ElectricEase.Data.DataBase;
using ElectricEase.Helpers;
using ElectricEase.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectricEase.Data.AssembliesMaster
{
    public class AssembliesMasterData
    {
        int ASMPartsResultCount;
        int ASMLabourResultCount;
        
        LogsMaster logs = new LogsMaster();
        public ServiceResultList<AssembliesParts_DetailsInfoList> GetMyAssembliesPartsList(int ClientID, string Part_Number)
        {
            ServiceResultList<AssembliesParts_DetailsInfoList> response = new ServiceResultList<AssembliesParts_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<AssembliesParts_DetailsInfoList> result = new List<AssembliesParts_DetailsInfoList>();
                    //SqlParameter[] para = new SqlParameter[2];
                    //para[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    //para[1] = new SqlParameter() { ParameterName = "Part_Number", Value = Part_Number };
                    result = db.Database.SqlQuery<AssembliesParts_DetailsInfoList>("exec EE_SelectedAssembliesPartsDetailList @ClientID, @Part_Number", new SqlParameter("ClientID", ClientID), new SqlParameter("Part_Number", Part_Number)).ToList();

                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            item.IsActivePartsDetails = item.Isactive;
                            item.LaborUnit = item.LaborUnit ?? 0;
                        }
                        response.ListData = result;
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

        /// <summary>
        /// GetMyNationWideAssembliesPartsList
        /// </summary>
        /// <param name="Part_Number"></param>
        /// <returns></returns>
        public ServiceResultList<AssembliesParts_DetailsInfoList> GetMyNationWideAssembliesPartsList(string Part_Number)
        {
            ServiceResultList<AssembliesParts_DetailsInfoList> response = new ServiceResultList<AssembliesParts_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<AssembliesParts_DetailsInfoList> result = new List<AssembliesParts_DetailsInfoList>();
                    //SqlParameter[] para = new SqlParameter[2];
                    //para[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    //para[1] = new SqlParameter() { ParameterName = "Part_Number", Value = Part_Number };
                    result = db.Database.SqlQuery<AssembliesParts_DetailsInfoList>("exec EE_SelectedNationWideAssembliesPartsDetailList  @Part_Number", new SqlParameter("Part_Number", Part_Number)).ToList();

                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            item.IsActivePartsDetails = item.Isactive;
                            if (item.LaborUnit == null)
                            {
                                item.LaborUnit = 0;
                            }

                        }
                        response.ListData = result;
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

        public ServiceResultList<AssembliesParts_DetailsInfoList> GetMyDistributorAssembliesPartsList(int Did, string Part_Number)
        {
            ServiceResultList<AssembliesParts_DetailsInfoList> response = new ServiceResultList<AssembliesParts_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<AssembliesParts_DetailsInfoList> result = new List<AssembliesParts_DetailsInfoList>();
                    //SqlParameter[] para = new SqlParameter[2];
                    //para[0] = new SqlParameter() { ParameterName = "Distributor_ID", Value = Did };
                    //para[1] = new SqlParameter() { ParameterName = "Part_Number", Value = Part_Number };
                    result = db.Database.SqlQuery<AssembliesParts_DetailsInfoList>("exec EE_SelectedDistributorAssembliesPartsDetailList @Distributor_ID, @Part_Number", new SqlParameter("Distributor_ID", Did), new SqlParameter("Part_Number", Part_Number)).ToList();

                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            item.IsActivePartsDetails = item.Isactive;
                            item.LaborUnit = item.LaborUnit ?? 0;

                        }
                        response.ListData = result;
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

        public ServiceResultList<AssembliesLabourDetailsList> GetMyAssembliesLaborDetailsList(int ClientID)
        {
            ServiceResultList<AssembliesLabourDetailsList> response = new ServiceResultList<AssembliesLabourDetailsList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<AssembliesLabourDetailsList> result = new List<AssembliesLabourDetailsList>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    result = db.Database.SqlQuery<AssembliesLabourDetailsList>("exec EE_GetMyLabourDetailList @ClientID", para).ToList();

                    if (result != null)
                    {
                        response.ListData = result;
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



        public ServiceResultList<AssembliesLabourDetailsList> GetMyAssembliesLaborerList(int ClientID, string Laborer_Name)
        {
            ServiceResultList<AssembliesLabourDetailsList> response = new ServiceResultList<AssembliesLabourDetailsList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<AssembliesLabourDetailsList> result = new List<AssembliesLabourDetailsList>();
                    result = db.Database.SqlQuery<AssembliesLabourDetailsList>("exec EE_GetSelectedAssembliesLaborerList @ClientID, @Laborer_Name", new SqlParameter("ClientID", ClientID), new SqlParameter("Laborer_Name", Laborer_Name)).ToList();

                    if (result != null)
                    {
                        response.ListData = result;
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

        public ServiceResult<int> AddNewAssembliesInformation(Assembly_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            // List<AssembliesParts_DetailsInfoList> PartsListOBJ = new List<AssembliesParts_DetailsInfoList>();

            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    int ASMresultCount;
                    Assemblies_Master ASMobj = new Assemblies_Master();
                    Assemblies_Master AssembliesIsExist = DbOBJ.Assemblies_Master.Where(x => x.Assemblies_Name == model.Assemblies_Name && x.Client_Id == model.Client_ID && x.Isactive == true).FirstOrDefault();
                    if (AssembliesIsExist != null)
                    {
                        response.Message = "Assemblies Lists are Already Registered.";
                        return response;
                    }
                    ASMobj.Client_Id = model.Client_ID;
                    ASMobj.Assemblies_Name = model.Assemblies_Name;
                    ASMobj.Assemblies_Description = model.Assemblies_Description;
                    ASMobj.Created_By = model.Created_By;
                    ASMobj.Created_Date = DateTime.Now;
                    ASMobj.Created_By = model.Created_By;
                    ASMobj.Created_Date = DateTime.Now;
                    ASMobj.Updated_By = model.Created_By;
                    ASMobj.Isactive = true;
                    DbOBJ.Assemblies_Master.Add(ASMobj);
                    ASMresultCount = DbOBJ.SaveChanges();
                    if (ASMresultCount > 0)
                    {
                        response.Message = "“Assemblies Information” added successfully.";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {
                        response.Message = "Error occurred while adding “Assemblies Information”!";
                        response.ResultCode = -1;
                        return response;
                    }
                }


            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResultCode = -1;
                return response;

            }
        }
        public ServiceResult<int> AddNewAssembliesPartsDeatails(Assembly_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            // List<AssembliesParts_DetailsInfoList> PartsListOBJ = new List<AssembliesParts_DetailsInfoList>();

            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    Assemblies_Parts ASM_PartsTBLobj = new Assemblies_Parts();
                    foreach (var item in model.PartsListData)
                    {
                        ASM_PartsTBLobj.Client_ID = model.Client_ID;
                        ASM_PartsTBLobj.Assemblies_Name = model.Assemblies_Name;
                        // ASM_PartsTBLobj.Part_ID = model.Part_ID;
                        ASM_PartsTBLobj.Part_Number = item.Part_Number.Trim();
                        ASM_PartsTBLobj.Part_Cost = item.Part_Cost;
                        ASM_PartsTBLobj.Parts_Photo = model.Parts_Photo;
                        ASM_PartsTBLobj.Part_Resale = item.Resale_Cost;
                        ASM_PartsTBLobj.Estimated_Qty = item.Estimated_Qty;
                        ASM_PartsTBLobj.Actual_Qty = item.Actual_Qty;
                        ASM_PartsTBLobj.Expected_Total = item.Expected_Total;
                        ASM_PartsTBLobj.Estimated_Total = item.Estimated_Total;
                        ASM_PartsTBLobj.IsActive = true;
                        DbOBJ.Assemblies_Parts.Add(ASM_PartsTBLobj);
                        ASMPartsResultCount = DbOBJ.SaveChanges();

                    }
                    if (ASMPartsResultCount > 0)
                    {
                        response.Message = "“Assembly Parts” details added successfully.";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {
                        response.Message = "Error occurred while adding “Assemblies Information”!";
                        response.ResultCode = -1;
                        return response;
                    }

                }

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResultCode = -1;
                return response;

            }

        }
        public ServiceResult<int> AddNewAssembliesDetails(Assembly_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            // List<AssembliesParts_DetailsInfoList> PartsListOBJ = new List<AssembliesParts_DetailsInfoList>();

            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    int ASMresultCount;
                    Assemblies_Master ASMobj = new Assemblies_Master();
                    model.Assemblies_Name = model.Assemblies_Name.Trim();
                    Assemblies_Master AssembliesIsExist = DbOBJ.Assemblies_Master.Where(x => x.Assemblies_Name == model.Assemblies_Name && x.Client_Id == model.Client_ID).FirstOrDefault();
                    if (AssembliesIsExist != null)
                    {
                        response.Message = "“Assemblies Name” is already registered, please change “Assembly Name”.";
                        return response;
                    }
                    ASMobj.Client_Id = model.Client_ID;
                    ASMobj.Assemblies_Name = model.Assemblies_Name;
                    if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                    {
                        ASMobj.Assemblies_Category = model.Assemblies_Category;
                    }
                    if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                    {
                        ASMobj.Assemblies_Category = model.OtherAssemblies_Category;
                    }

                    ASMobj.Assemblies_Description = model.Assemblies_Description;
                    ASMobj.severity = model.severity;
                    ASMobj.labor_cost = model.labor_cost;
                    ASMobj.labor_ResaleCost = model.Lobor_Resale;
                    ASMobj.labor_EstimatedHours = model.Estimated_Hour;
                    ASMobj.Labor_EstimatedCostTotal = model.LaborEst_CostTotal;
                    ASMobj.Lobor_EstimatedResaleTotal = model.LaborEst_ResaleTotal;
                    ASMobj.Parts_EstimatedCostTotal = model.PartCostTotal;
                    ASMobj.Parts_EstimatedResaleTotal = model.PartResaleTotal;
                    ASMobj.Estimated_Qty_Total = model.Est_Qty_Total;
                    ASMobj.Grand_EstCost_Total = model.GrandCostTotal;
                    ASMobj.Grand_EstResale_Total = model.GrandResaleTotal;

                    ASMobj.Created_By = model.Created_By;
                    ASMobj.Created_Date = DateTime.Now;
                    ASMobj.Updated_By = model.Created_By;
                    ASMobj.Updated_Date = DateTime.Now;
                    ASMobj.Isactive = model.Active;
                    DbOBJ.Assemblies_Master.Add(ASMobj);
                    logs.WriteLog("Adding New Assemnbly - " + model.Assemblies_Name, model.Client_ID, model.Created_By);
                    ASMresultCount = DbOBJ.SaveChanges();
                    logs.WriteLog("Successfully Added New Assemnbly - " + model.Assemblies_Name, model.Client_ID, model.Created_By);
                    if (ASMresultCount > 0)
                    {

                        using (ElectricEaseEntitiesContext DbOBJ2 = new ElectricEaseEntitiesContext())
                        {
                            Assemblies_Parts ASM_PartsTBLobj = new Assemblies_Parts();
                            foreach (var item in model.PartsListData)
                            {
                                ASM_PartsTBLobj.Client_ID = model.Client_ID;
                                ASM_PartsTBLobj.Assemblies_Name = model.Assemblies_Name;
                                if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                                {
                                    ASM_PartsTBLobj.Assemblies_Category = model.Assemblies_Category;
                                }
                                if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                                {
                                    ASM_PartsTBLobj.Assemblies_Category = model.OtherAssemblies_Category;
                                }
                                // ASM_PartsTBLobj.Part_ID = model.Part_ID;
                                ASM_PartsTBLobj.Part_Number = item.Part_Number.Trim();
                                ASM_PartsTBLobj.Part_Category = item.Part_Category;
                                ASM_PartsTBLobj.Part_Cost = item.Part_Cost;
                                ASM_PartsTBLobj.Part_Resale = item.Resale_Cost;
                                ASM_PartsTBLobj.Estimated_Qty = item.Estimated_Qty;
                                ASM_PartsTBLobj.EstimatedCost_Total = item.EstCost_Total;
                                ASM_PartsTBLobj.EstimatedResale_Total = item.EstResaleCost_Total;
                                ASM_PartsTBLobj.IsActive = true;
                                ASM_PartsTBLobj.LaborUnit = item.LaborUnit ?? 0;
                                logs.WriteLog("Adding New Assemnbly Parts - " + item.Part_Number, model.Client_ID, model.Created_By);
                                DbOBJ2.Assemblies_Parts.Add(ASM_PartsTBLobj);
                                ASMPartsResultCount = DbOBJ2.SaveChanges();
                                logs.WriteLog("Successfully Added New Assemnbly Parts - " + item.Part_Number, model.Client_ID, model.Created_By);

                            }
                            if (ASMPartsResultCount > 0)
                            {
                                response.Message = "“Assemblies details” are added successfully.";
                                response.ResultCode = 1;
                                return response;
                            }
                            else
                            {
                                response.Message = "Error occurred while adding “Assemblies Parts List Details”!";
                                response.ResultCode = -1;
                                return response;
                            }

                        }

                    }
                    else
                    {
                        response.Message = "Error occurred while adding “Assemblies Information”!";
                        response.ResultCode = -1;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While adding assembly - " + model.Assemblies_Name + "[Error Msg- " + ex.Message + " ]", model.Client_ID, model.Created_By);
                response.Message = ex.Message;
                response.ResultCode = -1;
                return response;

            }
        }
        /// <summary>
        /// AddNewNationWideAssembliesDetails
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ServiceResult<int> AddNewNationWideAssembliesDetails(Assembly_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            // List<AssembliesParts_DetailsInfoList> PartsListOBJ = new List<AssembliesParts_DetailsInfoList>();

            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    int ASMresultCount;
                    Super_Admin_Assemblies_Master ASMobj = new Super_Admin_Assemblies_Master();
                    model.Assemblies_Name = model.Assemblies_Name.Trim();
                    Super_Admin_Assemblies_Master AssembliesIsExist = DbOBJ.Super_Admin_Assemblies_Master.Where(x => x.Assemblies_Name == model.Assemblies_Name).FirstOrDefault();
                    if (AssembliesIsExist != null)
                    {
                        response.Message = "“Assemblies Name” is already registered, please change “Assembly Name”.";
                        return response;
                    }
                    //  ASMobj.Client_Id = model.Client_ID;
                    ASMobj.Assemblies_Name = model.Assemblies_Name;
                    if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                    {
                        ASMobj.Assemblies_Category = model.Assemblies_Category;
                    }
                    if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                    {
                        ASMobj.Assemblies_Category = model.OtherAssemblies_Category;
                    }

                    ASMobj.Assemblies_Description = model.Assemblies_Description;
                    ASMobj.severity = model.severity;
                    ASMobj.labor_cost = model.labor_cost;
                    ASMobj.labor_ResaleCost = model.Lobor_Resale;
                    ASMobj.labor_EstimatedHours = model.Estimated_Hour;
                    ASMobj.Labor_EstimatedCostTotal = model.LaborEst_CostTotal;
                    ASMobj.Lobor_EstimatedResaleTotal = model.LaborEst_ResaleTotal;
                    ASMobj.Parts_EstimatedCostTotal = model.PartCostTotal;
                    ASMobj.Parts_EstimatedResaleTotal = model.PartResaleTotal;
                    ASMobj.Estimated_Qty_Total = model.Est_Qty_Total;
                    ASMobj.Grand_EstCost_Total = model.GrandCostTotal;
                    ASMobj.Grand_EstResale_Total = model.GrandResaleTotal;
                    ASMobj.Created_By = model.Created_By;
                    ASMobj.Created_Date = DateTime.Now;
                    ASMobj.Updated_By = model.Created_By;
                    ASMobj.Updated_Date = DateTime.Now;
                    ASMobj.Isactive = model.Active;
                    DbOBJ.Super_Admin_Assemblies_Master.Add(ASMobj);
                    logs.WriteLog("Adding New Assemnbly - " + model.Assemblies_Name, model.Client_ID, model.Created_By);
                    ASMresultCount = DbOBJ.SaveChanges();
                    logs.WriteLog("Successfully Added New Assemnbly - " + model.Assemblies_Name, model.Client_ID, model.Created_By);
                    if (ASMresultCount > 0)
                    {

                        using (ElectricEaseEntitiesContext DbOBJ2 = new ElectricEaseEntitiesContext())
                        {
                            Super_Admin_Assemblies_Parts ASM_PartsTBLobj = new Super_Admin_Assemblies_Parts();
                            foreach (var item in model.PartsListData)
                            {
                                ASM_PartsTBLobj.Assemblies_Id = ASMobj.Assemblies_Id;
                                ASM_PartsTBLobj.Assemblies_Name = model.Assemblies_Name;
                                if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                                {
                                    ASM_PartsTBLobj.Assemblies_Category = model.Assemblies_Category;
                                }
                                if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                                {
                                    ASM_PartsTBLobj.Assemblies_Category = model.OtherAssemblies_Category;
                                }
                                // ASM_PartsTBLobj.Part_ID = model.Part_ID;
                                ASM_PartsTBLobj.Part_Number = item.Part_Number.Trim();
                                ASM_PartsTBLobj.Part_Category = item.Part_Category;
                                ASM_PartsTBLobj.Part_Cost = item.Part_Cost;
                                ASM_PartsTBLobj.LaborUnit = item.LaborUnit;
                                ASM_PartsTBLobj.Part_Resale = item.Resale_Cost;
                                ASM_PartsTBLobj.Estimated_Qty = item.Estimated_Qty;
                                ASM_PartsTBLobj.EstimatedCost_Total = item.EstCost_Total;
                                ASM_PartsTBLobj.EstimatedResale_Total = item.EstResaleCost_Total;
                                ASM_PartsTBLobj.IsActive = true;
                                logs.WriteLog("Adding New Assemnbly Parts - " + item.Part_Number, model.Client_ID, model.Created_By);
                                DbOBJ2.Super_Admin_Assemblies_Parts.Add(ASM_PartsTBLobj);
                                ASMPartsResultCount = DbOBJ2.SaveChanges();
                                logs.WriteLog("Successfully Added New Assemnbly Parts - " + item.Part_Number, model.Client_ID, model.Created_By);

                            }
                            if (ASMPartsResultCount > 0)
                            {
                                response.Message = "“Assemblies details” are added successfully.";
                                response.ResultCode = 1;
                                return response;
                            }
                            else
                            {
                                response.Message = "Error occurred while adding “Assemblies Parts List Details”!";
                                response.ResultCode = -1;
                                return response;
                            }

                        }

                    }
                    else
                    {
                        response.Message = "Error occurred while adding “Assemblies Information”!";
                        response.ResultCode = -1;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While adding assembly - " + model.Assemblies_Name + "[Error Msg- " + ex.Message + " ]", model.Client_ID, model.Created_By);
                response.Message = ex.Message;
                response.ResultCode = -1;
                return response;

            }
        }
        /// <summary>
        /// AddNewDistributorAssembliesDetails
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ServiceResult<int> AddNewDistributorAssembliesDetails(Assembly_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            // List<AssembliesParts_DetailsInfoList> PartsListOBJ = new List<AssembliesParts_DetailsInfoList>();

            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    int ASMresultCount;
                    Distributor_Assemblies_Master ASMobj = new Distributor_Assemblies_Master();
                    model.Assemblies_Name = model.Assemblies_Name.Trim();
                    Distributor_Assemblies_Master AssembliesIsExist = DbOBJ.Distributor_Assemblies_Master.Where(x => x.Assemblies_Name == model.Assemblies_Name && x.Distributor_Id == model.DistributorID).FirstOrDefault();
                    if (AssembliesIsExist != null)
                    {
                        response.Message = "“Assemblies Name” is already registered, please change “Assembly Name”.";
                        return response;
                    }
                    ASMobj.Distributor_Id = model.DistributorID;
                    ASMobj.Assemblies_Name = model.Assemblies_Name;
                    if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                    {
                        ASMobj.Assemblies_Category = model.Assemblies_Category;
                    }
                    if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                    {
                        ASMobj.Assemblies_Category = model.OtherAssemblies_Category;
                    }

                    ASMobj.Assemblies_Description = model.Assemblies_Description;
                    ASMobj.severity = model.severity;
                    ASMobj.labor_cost = model.labor_cost;
                    ASMobj.labor_ResaleCost = model.Lobor_Resale;
                    ASMobj.labor_EstimatedHours = model.Estimated_Hour;
                    ASMobj.Labor_EstimatedCostTotal = model.LaborEst_CostTotal;
                    ASMobj.Lobor_EstimatedResaleTotal = model.LaborEst_ResaleTotal;
                    ASMobj.Parts_EstimatedCostTotal = model.PartCostTotal;
                    ASMobj.Parts_EstimatedResaleTotal = model.PartResaleTotal;
                    ASMobj.Estimated_Qty_Total = model.Est_Qty_Total;
                    ASMobj.Grand_EstCost_Total = model.GrandCostTotal;
                    ASMobj.Grand_EstResale_Total = model.GrandResaleTotal;

                    ASMobj.Created_By = model.Created_By;
                    ASMobj.Created_Date = DateTime.Now;
                    ASMobj.Updated_By = model.Created_By;
                    ASMobj.Updated_Date = DateTime.Now;
                    ASMobj.Isactive = model.Active;
                    DbOBJ.Distributor_Assemblies_Master.Add(ASMobj);
                    logs.WriteLog("Adding New Assemnbly - " + model.Assemblies_Name, model.DistributorID, model.Created_By);
                    ASMresultCount = DbOBJ.SaveChanges();
                    logs.WriteLog("Successfully Added New Assemnbly - " + model.Assemblies_Name, model.DistributorID, model.Created_By);
                    if (ASMresultCount > 0)
                    {

                        using (ElectricEaseEntitiesContext DbOBJ2 = new ElectricEaseEntitiesContext())
                        {
                            Distributor_Assemblies_Parts ASM_PartsTBLobj = new Distributor_Assemblies_Parts();
                            foreach (var item in model.PartsListData)
                            {
                                ASM_PartsTBLobj.AssemblyID = ASMobj.AssemblyID;
                                ASM_PartsTBLobj.Distributor_ID = model.DistributorID;
                                ASM_PartsTBLobj.Assemblies_Name = model.Assemblies_Name;
                                if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                                {
                                    ASM_PartsTBLobj.Assemblies_Category = model.Assemblies_Category;
                                }
                                if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                                {
                                    ASM_PartsTBLobj.Assemblies_Category = model.OtherAssemblies_Category;
                                }
                                // ASM_PartsTBLobj.Part_ID = model.Part_ID;
                                ASM_PartsTBLobj.Part_Number = item.Part_Number.Trim();
                                ASM_PartsTBLobj.Part_Category = item.Part_Category;
                                ASM_PartsTBLobj.Part_Cost = item.Part_Cost;
                                ASM_PartsTBLobj.Part_Resale = item.Resale_Cost;
                                ASM_PartsTBLobj.Estimated_Qty = item.Estimated_Qty;
                                ASM_PartsTBLobj.EstimatedCost_Total = item.EstCost_Total;
                                ASM_PartsTBLobj.EstimatedResale_Total = item.EstResaleCost_Total;
                                ASM_PartsTBLobj.LaborUnit = item.LaborUnit;
                                //if(model.Active==true)
                                //{
                                ASM_PartsTBLobj.IsActive = true;
                                //}
                                //else
                                //{
                                //    ASM_PartsTBLobj.IsActive = false;
                                //} 
                                logs.WriteLog("Adding New Assemnbly Parts - " + item.Part_Number, model.DistributorID, model.Created_By);
                                DbOBJ2.Distributor_Assemblies_Parts.Add(ASM_PartsTBLobj);
                                ASMPartsResultCount = DbOBJ2.SaveChanges();
                                logs.WriteLog("Successfully Added New Assemnbly Parts - " + item.Part_Number, model.DistributorID, model.Created_By);

                            }
                            if (ASMPartsResultCount > 0)
                            {
                                response.Message = "“Assemblies details” are added successfully.";
                                response.ResultCode = 1;
                                return response;
                            }
                            else
                            {
                                response.Message = "Error occurred while adding “Assemblies Parts List Details”!";
                                response.ResultCode = -1;
                                return response;
                            }

                        }

                    }
                    else
                    {
                        response.Message = "Error occurred while adding “Assemblies Information”!";
                        response.ResultCode = -1;
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While adding assembly - " + model.Assemblies_Name + "[Error Msg- " + ex.Message + " ]", model.DistributorID, model.Created_By);
                response.Message = ex.Message;
                response.ResultCode = -1;
                return response;

            }
        }



        public ServiceResult<int> AddNewAssembliesLabourDeatails(Assembly_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    model.Assemblies_Name = model.Assemblies_Name.Trim();
                    Assemblies_Labor ASM_LabourTbOBJ = new Assemblies_Labor();
                    ASM_LabourTbOBJ.Client_Id = model.Client_ID;
                    ASM_LabourTbOBJ.Assemblies_Name = model.Assemblies_Name;
                    ASM_LabourTbOBJ.Laborer_Phone = model.Laborer_Phone;
                    ASM_LabourTbOBJ.Lobor_Cost = model.labor_cost;
                    ASM_LabourTbOBJ.Lobor_Resale = model.Lobor_Resale;
                    ASM_LabourTbOBJ.Estimated_Hour = model.Estimated_Hour;
                    ASM_LabourTbOBJ.IsActive = true;
                    DbOBJ.Assemblies_Labor.Add(ASM_LabourTbOBJ);
                    ASMLabourResultCount = DbOBJ.SaveChanges();
                    if (ASMLabourResultCount > 0)
                    {
                        response.Message = "“Assemblies Labor Details” added successfully.";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {
                        response.Message = "Error occurred while adding “Assemblies Labor Details”!";
                        response.ResultCode = -1;
                        return response;
                    }

                }

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResultCode = -1;
                return response;
            }
        }

        public ServiceResult<Assembly_MasterInfo> GetAssembliesListDetails(string AssembliesName, int Client_ID)
        {
            ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
            List<AssembliesParts_DetailsInfoList> ASMPartsListOBJ = new List<AssembliesParts_DetailsInfoList>();
            List<AssembliesLabourDetailsList> ASMLabourListOBJ = new List<AssembliesLabourDetailsList>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    Assemblies_Master ASMTblOBJ = new Assemblies_Master();
                    ASMTblOBJ = (from m in DbOBJ.Assemblies_Master where (m.Client_Id == Client_ID && m.Assemblies_Name == AssembliesName) select m).FirstOrDefault();
                    if (ASMTblOBJ == null)
                    {
                        response.Message = "“Assemblies Data” doesn’t exist!";
                        response.ResultCode = -1;
                        return response;
                    }

                    // Here Get Assemblies_Master table Information
                    response.Data = new Assembly_MasterInfo()
                    {
                        Assemblies_Name = ASMTblOBJ.Assemblies_Name,
                        Assemblies_Description = ASMTblOBJ.Assemblies_Description,
                        Assemblies_Category = ASMTblOBJ.Assemblies_Category,
                        severity = ASMTblOBJ.severity,
                        // Lobor_Cost = ASMTblOBJ.labor_cost,
                        // Lobor_Resale = ASMTblOBJ.labor_ResaleCost,
                        // Estimated_Hour = ASMTblOBJ.labor_EstimatedHours,

                        PartCostTotal = ASMTblOBJ.Parts_EstimatedCostTotal,
                        PartResaleTotal = ASMTblOBJ.Parts_EstimatedResaleTotal,
                        labor_cost = ASMTblOBJ.labor_cost,
                        Lobor_Resale = ASMTblOBJ.labor_ResaleCost,
                        Estimated_Hour = ASMTblOBJ.labor_EstimatedHours,
                        LaborEst_CostTotal = ASMTblOBJ.Labor_EstimatedCostTotal,
                        labor_EstimatedHours = ASMTblOBJ.labor_EstimatedHours,
                        LaborEst_ResaleTotal = ASMTblOBJ.Lobor_EstimatedResaleTotal,
                        GrandCostTotal = ASMTblOBJ.Grand_EstCost_Total,
                        Estimated_Qty_Total = ASMTblOBJ.Estimated_Qty_Total,
                        GrandResaleTotal = ASMTblOBJ.Grand_EstResale_Total,
                        Created_By = ASMTblOBJ.Created_By,
                        Created_Date = ASMTblOBJ.Created_Date,
                        Updated_By = ASMTblOBJ.Updated_By,
                        Updated_Date = ASMTblOBJ.Updated_Date,
                        Isactive = ASMTblOBJ.Isactive ?? false

                        //GrandActualTotal=ASMTblOBJ.gra
                    };

                    response.Data.assemblypartsCount = (from aspartstbl in DbOBJ.Assemblies_Parts
                                                        join PMtbl in DbOBJ.Parts_Details on aspartstbl.Part_Number equals PMtbl.Part_Number
                                                        where aspartstbl.Assemblies_Name == AssembliesName && aspartstbl.Client_ID == Client_ID && aspartstbl.IsActive == true
                                                        select aspartstbl
                                              ).Count();
                    // Here Get Assemblies_Parts table Information
                    response.Data.PartsListData = (from ASMTbl in DbOBJ.Assemblies_Master
                                                   join ASPartsTabl in DbOBJ.Assemblies_Parts
                                                    on ASMTbl.Assemblies_Name equals ASPartsTabl.Assemblies_Name
                                                   join PMtbl in DbOBJ.Parts_Details on ASPartsTabl.Client_ID equals PMtbl.Client_Id
                                                   where (ASMTbl.Client_Id == Client_ID
                                                   && ASMTbl.Assemblies_Name == AssembliesName
                                                      && PMtbl.Client_Id == Client_ID && PMtbl.Part_Number == ASPartsTabl.Part_Number && PMtbl.IsActive == true && ASPartsTabl.IsActive == true)
                                                   select new AssembliesParts_DetailsInfoList()
                                                   {
                                                       Client_ID = ASPartsTabl.Client_ID,
                                                       Assemblies_Name = ASPartsTabl.Assemblies_Name,
                                                       Part_ID = ASPartsTabl.Part_ID,
                                                       Part_Number = ASPartsTabl.Part_Number.Trim(),
                                                       Parts_Description = PMtbl.Description,
                                                       Part_Category = ASPartsTabl.Part_Category,
                                                       Part_Cost = ASPartsTabl.Part_Cost,
                                                       Resale_Cost = ASPartsTabl.Part_Resale,
                                                       Estimated_Qty = ASPartsTabl.Estimated_Qty,
                                                       EstCost_Total = ASPartsTabl.EstimatedCost_Total,
                                                       EstResaleCost_Total = ASPartsTabl.EstimatedResale_Total,
                                                       Isactive = true,
                                                       IsActivePartsDetails = PMtbl.IsActive,
                                                       LaborUnit = ASPartsTabl.LaborUnit ?? 0
                                                   }
                                                 ).Distinct().ToList();
                    response.ResultCode = 1;
                    response.Message = "Success";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResultCode = -1;
                return response;
            }
        }
        /// <summary>
        /// GetNationWideAssembliesListDetails
        /// </summary>
        /// <param name="AssembliesName"></param>
        /// <returns></returns>
        public ServiceResult<Assembly_MasterInfo> GetNationWideAssembliesListDetails(int AssebliesId)
        {
            ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
            List<AssembliesParts_DetailsInfoList> ASMPartsListOBJ = new List<AssembliesParts_DetailsInfoList>();
            List<AssembliesLabourDetailsList> ASMLabourListOBJ = new List<AssembliesLabourDetailsList>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    Super_Admin_Assemblies_Master ASMTblOBJ = new Super_Admin_Assemblies_Master();
                    ASMTblOBJ = (from m in DbOBJ.Super_Admin_Assemblies_Master where (m.Assemblies_Id == AssebliesId && m.Isactive == true) select m).FirstOrDefault();
                    if (ASMTblOBJ == null)
                    {
                        response.Message = "“Assemblies Data” doesn’t exist!";
                        response.ResultCode = -1;
                        return response;
                    }

                    // Here Get Assemblies_Master table Information
                    response.Data = new Assembly_MasterInfo()
                    {
                        AssebliesId = ASMTblOBJ.Assemblies_Id,
                        Assemblies_Name = ASMTblOBJ.Assemblies_Name,
                        Assemblies_Description = ASMTblOBJ.Assemblies_Description,
                        Assemblies_Category = ASMTblOBJ.Assemblies_Category,
                        severity = ASMTblOBJ.severity,
                        // Lobor_Cost = ASMTblOBJ.labor_cost,
                        // Lobor_Resale = ASMTblOBJ.labor_ResaleCost,
                        // Estimated_Hour = ASMTblOBJ.labor_EstimatedHours,
                        AssemblyId = ASMTblOBJ.Assemblies_Id,
                        PartCostTotal = ASMTblOBJ.Parts_EstimatedCostTotal,
                        PartResaleTotal = ASMTblOBJ.Parts_EstimatedResaleTotal,
                        labor_cost = ASMTblOBJ.labor_cost,
                        Lobor_Resale = ASMTblOBJ.labor_ResaleCost,
                        Estimated_Hour = ASMTblOBJ.labor_EstimatedHours,
                        LaborEst_CostTotal = ASMTblOBJ.Labor_EstimatedCostTotal,
                        LaborEst_ResaleTotal = ASMTblOBJ.Lobor_EstimatedResaleTotal,
                        GrandCostTotal = ASMTblOBJ.Grand_EstCost_Total,
                        Estimated_Qty_Total = ASMTblOBJ.Estimated_Qty_Total,
                        GrandResaleTotal = ASMTblOBJ.Grand_EstResale_Total,
                        Created_By = ASMTblOBJ.Created_By,
                        Created_Date = ASMTblOBJ.Created_Date,
                        Updated_By = ASMTblOBJ.Updated_By,
                        Updated_Date = ASMTblOBJ.Updated_Date,
                        Isactive = ASMTblOBJ.Isactive ?? false

                        //GrandActualTotal=ASMTblOBJ.gra
                    };

                    response.Data.assemblypartsCount = (from aspartstbl in DbOBJ.Super_Admin_Assemblies_Parts
                                                        join PMtbl in DbOBJ.Super_Admin_Parts_Details on aspartstbl.Part_Number equals PMtbl.Part_Number
                                                        where aspartstbl.Assemblies_Id == AssebliesId && PMtbl.IsActive == true && aspartstbl.IsActive == true
                                                        select aspartstbl
                                              ).Count();
                    // Here Get Assemblies_Parts table Information
                    response.Data.PartsListData = (from ASMTbl in DbOBJ.Super_Admin_Assemblies_Master
                                                   join ASPartsTabl in DbOBJ.Super_Admin_Assemblies_Parts
                                                    on ASMTbl.Assemblies_Id equals ASPartsTabl.Assemblies_Id
                                                   join PMtbl in DbOBJ.Super_Admin_Parts_Details on ASPartsTabl.Part_Number equals PMtbl.Part_Number
                                                   where (ASMTbl.Assemblies_Id == AssebliesId
                                                    && ASMTbl.Isactive == true
                                                    && ASPartsTabl.IsActive == true
                                                    && PMtbl.IsActive == true
                                                    )
                                                   select new AssembliesParts_DetailsInfoList()
                                                   {
                                                       Assemblies_Name = ASMTbl.Assemblies_Name,
                                                       Part_ID = ASPartsTabl.Part_ID,
                                                       Part_Number = ASPartsTabl.Part_Number.Trim(),
                                                       Parts_Description = PMtbl.Description,
                                                       Part_Category = ASPartsTabl.Part_Category,
                                                       Part_Cost = ASPartsTabl.Part_Cost,
                                                       Resale_Cost = ASPartsTabl.Part_Resale,
                                                       Estimated_Qty = ASPartsTabl.Estimated_Qty,
                                                       EstCost_Total = ASPartsTabl.EstimatedCost_Total,
                                                       EstResaleCost_Total = ASPartsTabl.EstimatedResale_Total,
                                                       Isactive = true,
                                                       IsActivePartsDetails = PMtbl.IsActive,
                                                       LaborUnit = ASPartsTabl.LaborUnit ?? 0
                                                   }
                                                 ).ToList();
                    response.ResultCode = 1;
                    response.Message = "Success";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResultCode = -1;
                return response;
            }
        }
        /// <summary>
        /// GetDistributorAssembliesListDetails
        /// </summary>
        /// <param name="AssembliesName"></param>
        /// <param name="Did"></param>
        /// <returns></returns>
        public ServiceResult<Assembly_MasterInfo> GetDistributorAssembliesListDetails(string AssembliesName, int Did)
        {
            ServiceResult<Assembly_MasterInfo> response = new ServiceResult<Assembly_MasterInfo>();
            List<AssembliesParts_DetailsInfoList> ASMPartsListOBJ = new List<AssembliesParts_DetailsInfoList>();
            List<AssembliesLabourDetailsList> ASMLabourListOBJ = new List<AssembliesLabourDetailsList>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    Distributor_Assemblies_Master ASMTblOBJ = new Distributor_Assemblies_Master();
                    ASMTblOBJ = (from m in DbOBJ.Distributor_Assemblies_Master where (m.Assemblies_Name == AssembliesName && m.Distributor_Id == Did && m.Isactive == true) select m).FirstOrDefault();
                    if (ASMTblOBJ == null)
                    {
                        response.Message = "“Assemblies Data” doesn’t exist!";
                        response.ResultCode = -1;
                        return response;
                    }

                    // Here Get Assemblies_Master table Information
                    response.Data = new Assembly_MasterInfo()
                    {
                        AssemblyId = ASMTblOBJ.AssemblyID,
                        DistributorID = ASMTblOBJ.Distributor_Id,
                        Assemblies_Name = ASMTblOBJ.Assemblies_Name,
                        Assemblies_Description = ASMTblOBJ.Assemblies_Description,
                        Assemblies_Category = ASMTblOBJ.Assemblies_Category,
                        severity = ASMTblOBJ.severity,
                        // Lobor_Cost = ASMTblOBJ.labor_cost,
                        // Lobor_Resale = ASMTblOBJ.labor_ResaleCost,
                        // Estimated_Hour = ASMTblOBJ.labor_EstimatedHours,
                        // AssemblyId = ASMTblOBJ.Assemblies_Id,
                        PartCostTotal = ASMTblOBJ.Parts_EstimatedCostTotal,
                        PartResaleTotal = ASMTblOBJ.Parts_EstimatedResaleTotal,
                        labor_cost = ASMTblOBJ.labor_cost,
                        Lobor_Resale = ASMTblOBJ.labor_ResaleCost,
                        Estimated_Hour = ASMTblOBJ.labor_EstimatedHours,
                        LaborEst_CostTotal = ASMTblOBJ.Labor_EstimatedCostTotal,
                        LaborEst_ResaleTotal = ASMTblOBJ.Lobor_EstimatedResaleTotal,
                        GrandCostTotal = ASMTblOBJ.Grand_EstCost_Total,
                        Estimated_Qty_Total = ASMTblOBJ.Estimated_Qty_Total,
                        GrandResaleTotal = ASMTblOBJ.Grand_EstResale_Total,
                        Created_By = ASMTblOBJ.Created_By,
                        Created_Date = ASMTblOBJ.Created_Date,
                        Updated_By = ASMTblOBJ.Updated_By,
                        Updated_Date = ASMTblOBJ.Updated_Date,
                        Isactive = ASMTblOBJ.Isactive ?? false

                        //GrandActualTotal=ASMTblOBJ.gra
                    };

                    response.Data.assemblypartsCount = (from aspartstbl in DbOBJ.Distributor_Assemblies_Parts
                                                            //join PMtbl in DbOBJ.Distributor_Parts_Details on aspartstbl.Part_Number equals PMtbl.Part_Number
                                                        where aspartstbl.AssemblyID == ASMTblOBJ.AssemblyID && aspartstbl.Distributor_ID == Did && aspartstbl.IsActive == true
                                                        select aspartstbl
                                              ).Count();
                    // Here Get Assemblies_Parts table Information
                    response.Data.PartsListData = (from ASMTbl in DbOBJ.Distributor_Assemblies_Parts
                                                       //join ASPartsTabl in DbOBJ.Distributor_Assemblies_Parts
                                                       // on ASMTbl.Distributor_Id equals ASPartsTabl.Distributor_ID
                                                   join PMtbl in DbOBJ.Distributor_Parts_Details on ASMTbl.Part_Number equals PMtbl.Part_Number
                                                   where (
                                                    ASMTbl.Assemblies_Name == AssembliesName
                                                    && ASMTbl.Distributor_ID == Did
                                                    && PMtbl.Distributor_ID == Did
                                                    && ASMTbl.IsActive == true
                                                    && PMtbl.IsActive == true
                                                   )
                                                   select new AssembliesParts_DetailsInfoList()
                                                   {
                                                       Assemblies_Name = AssembliesName,
                                                       Part_ID = ASMTbl.Part_ID,
                                                       Part_Number = ASMTbl.Part_Number.Trim(),
                                                       Parts_Description = PMtbl.Description,
                                                       Part_Category = ASMTbl.Part_Category,
                                                       Part_Cost = ASMTbl.Part_Cost,
                                                       LaborUnit = ASMTbl.LaborUnit ?? 0,
                                                       Resale_Cost = ASMTbl.Part_Resale,
                                                       Estimated_Qty = ASMTbl.Estimated_Qty,
                                                       EstCost_Total = ASMTbl.EstimatedCost_Total,
                                                       EstResaleCost_Total = ASMTbl.EstimatedResale_Total,
                                                       Isactive = true,
                                                       IsActivePartsDetails = true
                                                   }
                                                 ).ToList();//.Where(a => a.IsActivePartsDetails == true).ToList();
                    response.ResultCode = 1;
                    response.Message = "Success";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.ResultCode = -1;
                return response;
            }
        }

        public List<Assembly_MasterInfoList> GetAssembliesList(int Client_ID)
        {
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            List<Assembly_MasterInfoList> listOBJ = new List<Assembly_MasterInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Assembly_MasterInfoList> result = new List<Assembly_MasterInfoList>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "ClientID", Value = Client_ID };
                    response = db.Database.SqlQuery<Assembly_MasterInfoList>("exec EE_GetAssembliesList @ClientID", para).ToList();
                    return response;
                }

            }
            catch (Exception ex)
            {

                return response;
            }

        }

        /// <summary>
        /// GetNationWideAssembliesList
        /// </summary>
        /// <returns></returns>
        public List<Assembly_MasterInfoList> GetNationWideAssembliesList()
        {
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            List<Assembly_MasterInfoList> listOBJ = new List<Assembly_MasterInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Assembly_MasterInfoList> result = new List<Assembly_MasterInfoList>();
                    //SqlParameter[] para = new SqlParameter[1];
                    //para[0] = new SqlParameter() { ParameterName = "ClientID", Value = Client_ID };
                    response = db.Database.SqlQuery<Assembly_MasterInfoList>("exec EE_GetNationWideAssembliesList").Where(a => a.IsActive == true).ToList();
                    if (response != null && response.Count > 0)
                    {
                        foreach (var item in response)
                        {
                            item.assemblypartsCount = (from aspartstbl in db.Super_Admin_Assemblies_Parts
                                                       join PMtbl in db.Super_Admin_Parts_Details on aspartstbl.Part_Number equals PMtbl.Part_Number
                                                       where aspartstbl.Assemblies_Id == item.Assemblies_Id && aspartstbl.IsActive == true && PMtbl.IsActive == true
                                                       select aspartstbl
                                             ).Distinct().Count();
                            var presult = (from aspartstbl in db.Super_Admin_Assemblies_Parts
                                           join PMtbl in db.Super_Admin_Parts_Details on aspartstbl.Part_Number equals PMtbl.Part_Number
                                           where aspartstbl.Assemblies_Id == item.Assemblies_Id && aspartstbl.IsActive == true && PMtbl.IsActive == true
                                           select PMtbl
                                             ).ToList();

                        }
                        return response.OrderBy(x => x.Assemblies_Name).ToList();
                    }
                    else
                    {
                        return response.OrderBy(x => x.Assemblies_Name).ToList();
                    }
                }

            }
            catch (Exception ex)
            {

                return response;
            }

        }

        public List<Assembly_MasterInfoList> GetNationWideAssembliesGrid()
        {
            List<Assembly_MasterInfoList> assemblyList = new List<Assembly_MasterInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    assemblyList = db.Database.SqlQuery<Assembly_MasterInfoList>("exec EE_GetNationWideAssembliesGrid").ToList();
                    return assemblyList;
                }
            }
            catch (Exception ex)
            {
                return assemblyList;
            }
        }

        public List<Assembly_MasterInfoList> GetDistributorAssembliesGrid()
        {
            List<Assembly_MasterInfoList> assemblyList = new List<Assembly_MasterInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    assemblyList = db.Database.SqlQuery<Assembly_MasterInfoList>("exec EE_GetDistributorAssembliesGrid").ToList();
                    return assemblyList;
                }
            }
            catch (Exception ex)
            {
                return assemblyList;
            }
        }

        /// <summary>
        /// GetDistributorAssembliesList
        /// </summary>
        /// <returns></returns>
        public List<Assembly_MasterInfoList> GetDistributorAssembliesList()
        {
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            List<Assembly_MasterInfoList> listOBJ = new List<Assembly_MasterInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Assembly_MasterInfoList> result = new List<Assembly_MasterInfoList>();
                    //SqlParameter[] para = new SqlParameter[1];
                    //para[0] = new SqlParameter() { ParameterName = "ClientID", Value = Client_ID };
                    response = db.Database.SqlQuery<Assembly_MasterInfoList>("exec EE_GetDistributorAssembliesList").Where(a => a.IsActive == true).ToList();
                    //if (response != null && response.Count > 0)
                    //{
                    //    foreach (var item in response)
                    //    {
                    //        item.assemblypartsCount = (from aspartstbl in db.Distributor_Assemblies_Parts
                    //                                   where aspartstbl.Assemblies_Name == item.Assemblies_Name && aspartstbl.Distributor_ID == item.Distributor_Id && aspartstbl.IsActive == true
                    //                                   select aspartstbl
                    //                         ).Distinct().Count();
                    //        //var presult = (from aspartstbl in db.Distributor_Assemblies_Parts
                    //        //               join PMtbl in db.Distributor_Parts_Details on aspartstbl.Part_Number equals PMtbl.Part_Number
                    //        //               where aspartstbl.Distributor_ID == item.Distributor_Id && aspartstbl.IsActive == true && PMtbl.IsActive == true
                    //        //               select PMtbl
                    //        //                 ).ToList();
                    //        item.distributorName = db.Distributor_Master.Where(x => x.ID == item.Distributor_Id && x.IsActive == true).Select(x => x.Company).FirstOrDefault();

                    //    }
                    //    return response;
                    //}
                    //else
                    //{
                    //    return response;
                    //}
                    return response;
                }

            }
            catch (Exception ex)
            {

                return response;
            }

        }

        public List<Assembly_MasterInfoList> GetDistributorAssembliesDetalis(string searchBy, int take, int skip, string sortBy, string sortDir, out int filteredResultsCount, out int totalResultsCount, string extra_search)
        {
            filteredResultsCount = 0;
            totalResultsCount = 0;
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            List<Assembly_MasterInfoList> listOBJ = new List<Assembly_MasterInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Assembly_MasterInfoList> result = new List<Assembly_MasterInfoList>();
                    //SqlParameter[] para = new SqlParameter[1];
                    //para[0] = new SqlParameter() { ParameterName = "ClientID", Value = Client_ID };
                    var filterCountParam = new SqlParameter
                    {
                        ParameterName = "filteredResultsCount",
                        Value = 0,
                        Direction = ParameterDirection.Output
                    };
                    var totalCountParam = new SqlParameter
                    {
                        ParameterName = "totalResultsCount",
                        Value = 0,
                        Direction = ParameterDirection.Output
                    };
                    response = db.Database.SqlQuery<Assembly_MasterInfoList>("exec EE_GetDistributorAssembliesList @searchStr,@sortBy,@sortDir,@Skip,@Take,@filteredResultsCount out,@totalResultsCount out,@Category"
                            , new SqlParameter("searchStr", searchBy ?? "")
                            , new SqlParameter("sortBy", sortBy)
                            , new SqlParameter("sortDir", sortDir)
                            , new SqlParameter("Skip", skip)
                            , new SqlParameter("Take", take), filterCountParam, totalCountParam,
                            new SqlParameter("Category", extra_search ?? "")
                            ).ToList();
                    filteredResultsCount = (int)filterCountParam.Value;
                    totalResultsCount = (int)totalCountParam.Value;
                    return response;
                }

            }
            catch (Exception ex)
            {

                return response;
            }
        }

        public List<Assembly_MasterInfoList> GetDistributorsAssembliesList(int distributorID)
        {
            List<Assembly_MasterInfoList> response = new List<Assembly_MasterInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    return response = db.Database.SqlQuery<Assembly_MasterInfoList>("EE_GetDistributorExpAssembliesList @Distributor_ID", new SqlParameter("Distributor_ID", distributorID)).ToList();
                }
            }
            catch (Exception ex)
            {

                return response;
            }

        }

        public ServiceResult<int> UpdateAssembliesDetails(Assembly_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            List<AssembliesParts_DetailsInfoList> PartsList = new List<AssembliesParts_DetailsInfoList>();
            List<AssembliesParts_DetailsInfoList> ExistAssembliesPartsList = new List<AssembliesParts_DetailsInfoList>();
            try
            {
                //Here  starts in Updating Assemblies Details.....
                using (ElectricEaseEntitiesContext DbASOBJ = new ElectricEaseEntitiesContext())
                {
                    //Here Updating Assemblies Information.....
                    Assemblies_Master ASMtbl = new Assemblies_Master();
                    model.Assemblies_Name = model.Assemblies_Name.Trim();
                    ASMtbl = (from m in DbASOBJ.Assemblies_Master where m.Client_Id == model.Client_ID && m.Assemblies_Name == model.Assemblies_Name select m).FirstOrDefault();

                    ASMtbl.Client_Id = model.Client_ID;
                    ASMtbl.Assemblies_Name = model.Assemblies_Name;
                    if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                    {
                        ASMtbl.Assemblies_Category = model.Assemblies_Category;
                    }
                    if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                    {
                        ASMtbl.Assemblies_Category = model.OtherAssemblies_Category;
                    }
                    ASMtbl.Assemblies_Description = model.Assemblies_Description;
                    ASMtbl.severity = model.severity;
                    ASMtbl.labor_cost = model.labor_cost;
                    ASMtbl.labor_ResaleCost = model.labor_ResaleCost;
                    ASMtbl.labor_EstimatedHours = model.labor_EstimatedHours;
                    ASMtbl.Labor_EstimatedCostTotal = model.LaborEst_CostTotal;
                    ASMtbl.Lobor_EstimatedResaleTotal = model.LaborEst_ResaleTotal;
                    ASMtbl.Parts_EstimatedCostTotal = model.PartCostTotal;
                    ASMtbl.Parts_EstimatedResaleTotal = model.PartResaleTotal;
                    ASMtbl.Estimated_Qty_Total = model.Est_Qty_Total;
                    ASMtbl.Grand_EstCost_Total = model.GrandCostTotal;
                    ASMtbl.Grand_EstResale_Total = model.GrandResaleTotal;


                    ASMtbl.Updated_By = model.Updated_By;
                    ASMtbl.Updated_Date = DateTime.Now;
                    ASMtbl.Isactive = model.Active;
                    int AStblResultCount = DbASOBJ.SaveChanges();
                    logs.WriteLog("Updated the Assembly - " + model.Assemblies_Name, model.Client_ID, model.Updated_By);
                    //Here Updating Assemblies Parts Details....
                    if (AStblResultCount > 0)
                    {
                        using (ElectricEaseEntitiesContext DBPartsOBJ = new ElectricEaseEntitiesContext())
                        {
                            //Here Getting Previously Added Assemblies parts List from Database...
                            Assemblies_Parts ASM_PartsTbOBJ = new Assemblies_Parts();
                            //List<AssembliesLabourDetailsList> ExistAssembliesLabourList = new List<AssembliesLabourDetailsList>();
                            ExistAssembliesPartsList = (from m in DBPartsOBJ.Assemblies_Parts
                                                        where m.Client_ID == model.Client_ID && m.Assemblies_Name == model.Assemblies_Name
                                                        select new AssembliesParts_DetailsInfoList()
                                                        {
                                                            Part_Number = m.Part_Number.Trim(),
                                                            Assemblies_Name = m.Assemblies_Name

                                                        }).ToList();

                            //Here Existing  Db assemblies partslists is compare with new Selected List from Model....
                            foreach (var ExistSelectedItem in ExistAssembliesPartsList)
                            {
                                var MatchItem = (from m in model.PartsListData where m.Part_Number.Trim() == ExistSelectedItem.Part_Number.Trim() select m).FirstOrDefault();
                                if (MatchItem == null)
                                {
                                    //Here to Set Existing selected Parts Record if not selected from Updated Assemblies Parts Details...
                                    ASM_PartsTbOBJ = (from m in DBPartsOBJ.Assemblies_Parts
                                                      where m.Client_ID == model.Client_ID && m.Assemblies_Name == model.Assemblies_Name && m.Part_Number == ExistSelectedItem.Part_Number.Trim()
                                                      select m).FirstOrDefault();
                                    ASM_PartsTbOBJ.IsActive = false;
                                    int count = DBPartsOBJ.SaveChanges();
                                }

                            }
                            //Here Starts to Update assemblies Parts Details....
                            using (ElectricEaseEntitiesContext DBPartsUpdateOBJ = new ElectricEaseEntitiesContext())
                            {
                                Assemblies_Parts ASM_PartsTBLobj = new Assemblies_Parts();

                                //Here Assemblies Parts Lists is Updated based on the new updated list in the Model....
                                foreach (var item in model.PartsListData)
                                {
                                    ASM_PartsTBLobj = (from m in DBPartsUpdateOBJ.Assemblies_Parts
                                                       where m.Client_ID == model.Client_ID && m.Assemblies_Name == model.Assemblies_Name && m.Part_Number == item.Part_Number.Trim()
                                                       select m).FirstOrDefault();
                                    if (ASM_PartsTBLobj != null)
                                    {
                                        if (item.Part_Number.Trim() == ASM_PartsTBLobj.Part_Number.Trim())
                                        {
                                            ASM_PartsTBLobj.Part_Number = item.Part_Number.Trim();
                                            ASM_PartsTBLobj.Part_Category = item.Part_Category;
                                            ASM_PartsTBLobj.Part_Cost = item.Part_Cost;
                                            ASM_PartsTBLobj.Part_Resale = item.Resale_Cost;
                                            ASM_PartsTBLobj.Estimated_Qty = item.Estimated_Qty;
                                            ASM_PartsTBLobj.EstimatedCost_Total = item.EstCost_Total;
                                            ASM_PartsTBLobj.EstimatedResale_Total = item.EstResaleCost_Total;
                                            ASM_PartsTBLobj.IsActive = true;
                                            ASM_PartsTBLobj.LaborUnit = item.LaborUnit;
                                            ASMPartsResultCount = DBPartsUpdateOBJ.SaveChanges();
                                            logs.WriteLog("Updated the Assembly Parts - " + ASM_PartsTBLobj.Part_Number, model.Client_ID, model.Updated_By);
                                        }
                                    }
                                    else
                                    {
                                        //Here to Inserting Records if selected lists is not exist from Previues Selected  Parts lists in Database
                                        bool newItemIsExist = DBPartsUpdateOBJ.Assemblies_Parts.Any(m => m.Client_ID == model.Client_ID && m.Part_Number == item.Part_Number && m.Assemblies_Name == model.Assemblies_Name);
                                        if (newItemIsExist == false)
                                        {

                                            Assemblies_Parts ASM_PartsTbInsertOBJ = new Assemblies_Parts();
                                            ASM_PartsTbInsertOBJ.Client_ID = model.Client_ID;
                                            ASM_PartsTbInsertOBJ.Assemblies_Name = model.Assemblies_Name;
                                            if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                                            {
                                                ASM_PartsTbInsertOBJ.Assemblies_Category = model.Assemblies_Category;
                                            }
                                            if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                                            {
                                                ASM_PartsTbInsertOBJ.Assemblies_Category = model.OtherAssemblies_Category;
                                            }
                                            ASM_PartsTbInsertOBJ.Part_Number = item.Part_Number.Trim();
                                            ASM_PartsTbInsertOBJ.Part_Category = item.Part_Category;
                                            ASM_PartsTbInsertOBJ.Part_Cost = item.Part_Cost;
                                            ASM_PartsTbInsertOBJ.Part_Resale = item.Resale_Cost;
                                            ASM_PartsTbInsertOBJ.EstimatedCost_Total = item.EstCost_Total;
                                            ASM_PartsTbInsertOBJ.EstimatedResale_Total = item.EstResaleCost_Total;
                                            ASM_PartsTbInsertOBJ.Estimated_Qty = item.Estimated_Qty;
                                            ASM_PartsTbInsertOBJ.IsActive = true;
                                            ASM_PartsTbInsertOBJ.LaborUnit = item.LaborUnit;
                                            DBPartsUpdateOBJ.Assemblies_Parts.Add(ASM_PartsTbInsertOBJ);
                                            ASMLabourResultCount = DBPartsUpdateOBJ.SaveChanges();
                                            logs.WriteLog("Updated the Assembly Parts - " + ASM_PartsTbInsertOBJ.Part_Number, model.Client_ID, model.Updated_By);
                                        }

                                    }

                                }

                            }
                            if (AStblResultCount > 0 || ASMPartsResultCount > 0)
                            {
                                response.ResultCode = 1;
                                response.Message = "“Assemblies details” has been updated successfully.";
                                return response;
                            }
                            else
                            {
                                response.ResultCode = -1;
                                response.Message = "No changes found in your “Assembly’s Labor Details”.";
                                return response;
                            }
                        }
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "An error occurred while updating “Assembly Part Details”!";
                        return response;
                    }

                }

            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While updating the Assembly - " + model.Assemblies_Name + " [Error Msg - " + ex.Message + " ]", model.Client_ID, model.Updated_By);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;

            }
        }
        /// <summary>
        /// UpdateNationWideAssembliesDetails
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ServiceResult<int> UpdateNationWideAssembliesDetails(Assembly_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            List<AssembliesParts_DetailsInfoList> PartsList = new List<AssembliesParts_DetailsInfoList>();
            List<AssembliesParts_DetailsInfoList> ExistAssembliesPartsList = new List<AssembliesParts_DetailsInfoList>();
            try
            {
                //Here  starts in Updating Assemblies Details.....
                using (ElectricEaseEntitiesContext DbASOBJ = new ElectricEaseEntitiesContext())
                {
                    //Here Updating Assemblies Information.....
                    Super_Admin_Assemblies_Master ASMtbl = new Super_Admin_Assemblies_Master();
                    model.Assemblies_Name = model.Assemblies_Name.Trim();

                    var checkAssembly = (from m in DbASOBJ.Super_Admin_Assemblies_Master where m.Assemblies_Id != model.AssemblyId && m.Assemblies_Name == model.Assemblies_Name select m).FirstOrDefault();

                    if (checkAssembly == null)
                    {
                        ASMtbl = (from m in DbASOBJ.Super_Admin_Assemblies_Master where m.Assemblies_Id == model.AssemblyId select m).FirstOrDefault();

                        ASMtbl.Assemblies_Id = model.AssemblyId;
                        ASMtbl.Assemblies_Name = model.Assemblies_Name;

                        if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                        {
                            ASMtbl.Assemblies_Category = model.Assemblies_Category;
                        }
                        if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                        {
                            ASMtbl.Assemblies_Category = model.OtherAssemblies_Category;
                        }

                        ASMtbl.Assemblies_Description = model.Assemblies_Description;
                        ASMtbl.severity = model.severity;
                        ASMtbl.labor_cost = model.labor_cost;
                        ASMtbl.labor_ResaleCost = model.labor_ResaleCost;
                        ASMtbl.labor_EstimatedHours = model.labor_EstimatedHours;
                        ASMtbl.Labor_EstimatedCostTotal = model.LaborEst_CostTotal;
                        ASMtbl.Lobor_EstimatedResaleTotal = model.LaborEst_ResaleTotal;
                        ASMtbl.Parts_EstimatedCostTotal = model.PartCostTotal;
                        ASMtbl.Parts_EstimatedResaleTotal = model.PartResaleTotal;
                        ASMtbl.Estimated_Qty_Total = model.Est_Qty_Total;
                        ASMtbl.Grand_EstCost_Total = model.GrandCostTotal;
                        ASMtbl.Grand_EstResale_Total = model.GrandResaleTotal;


                        ASMtbl.Updated_By = model.Updated_By;
                        ASMtbl.Updated_Date = DateTime.Now;
                        ASMtbl.Isactive = model.Active;
                        int AStblResultCount = DbASOBJ.SaveChanges();
                        logs.WriteLog("Updated the Assembly - " + model.Assemblies_Name, model.Client_ID, model.Updated_By);

                        //Here Updating Assemblies Parts Details....
                        if (AStblResultCount > 0)
                        {
                            using (ElectricEaseEntitiesContext DBPartsOBJ = new ElectricEaseEntitiesContext())
                            {
                                //Here Getting Previously Added Assemblies parts List from Database...
                                Super_Admin_Assemblies_Parts ASM_PartsTbOBJ = new Super_Admin_Assemblies_Parts();

                                ExistAssembliesPartsList = (from m in DBPartsOBJ.Super_Admin_Assemblies_Parts
                                                            where m.Assemblies_Id == model.AssemblyId && m.IsActive == true
                                                            select new AssembliesParts_DetailsInfoList()
                                                            {
                                                                Part_Number = m.Part_Number.Trim(),
                                                                Assemblies_Name = m.Assemblies_Name

                                                            }).ToList();

                                //Here Existing  Db assemblies partslists is compare with new Selected List from Model....
                                foreach (var ExistSelectedItem in ExistAssembliesPartsList)
                                {
                                    var MatchItem = (from m in model.PartsListData where m.Part_Number.Trim() == ExistSelectedItem.Part_Number.Trim() select m).FirstOrDefault();
                                    if (MatchItem == null)
                                    {
                                        //Here to Set Existing selected Parts Record if not selected from Updated Assemblies Parts Details...
                                        ASM_PartsTbOBJ = (from m in DBPartsOBJ.Super_Admin_Assemblies_Parts
                                                          where m.Assemblies_Id == model.AssemblyId && m.IsActive == true && m.Part_Number == ExistSelectedItem.Part_Number.Trim()
                                                          select m).FirstOrDefault();
                                        ASM_PartsTbOBJ.IsActive = false;
                                        int count = DBPartsOBJ.SaveChanges();
                                    }

                                }
                                //Here Starts to Update assemblies Parts Details....
                                using (ElectricEaseEntitiesContext DBPartsUpdateOBJ = new ElectricEaseEntitiesContext())
                                {
                                    Super_Admin_Assemblies_Parts ASM_PartsTBLobj = new Super_Admin_Assemblies_Parts();

                                    //Here Assemblies Parts Lists is Updated based on the new updated list in the Model....
                                    foreach (var item in model.PartsListData)
                                    {
                                        ASM_PartsTBLobj = (from m in DBPartsUpdateOBJ.Super_Admin_Assemblies_Parts
                                                           where m.Assemblies_Id == model.AssemblyId && m.Part_Number == item.Part_Number.Trim()
                                                           select m).FirstOrDefault();
                                        if (ASM_PartsTBLobj != null)
                                        {
                                            if (item.Part_Number.Trim() == ASM_PartsTBLobj.Part_Number.Trim())
                                            {
                                                ASM_PartsTBLobj.Part_Number = item.Part_Number.Trim();
                                                ASM_PartsTBLobj.Part_Category = item.Part_Category;
                                                ASM_PartsTBLobj.Part_Cost = item.Part_Cost;
                                                ASM_PartsTBLobj.Part_Resale = item.Resale_Cost;
                                                ASM_PartsTBLobj.Estimated_Qty = item.Estimated_Qty;
                                                ASM_PartsTBLobj.EstimatedCost_Total = item.EstCost_Total;
                                                ASM_PartsTBLobj.EstimatedResale_Total = item.EstResaleCost_Total;
                                                ASM_PartsTBLobj.IsActive = true;
                                                ASM_PartsTBLobj.LaborUnit = item.LaborUnit ?? 0;
                                                ASMPartsResultCount = DBPartsUpdateOBJ.SaveChanges();
                                                logs.WriteLog("Updated the Assembly Parts - " + ASM_PartsTBLobj.Part_Number, model.Client_ID, model.Updated_By);
                                            }
                                        }
                                        else
                                        {
                                            //Here to Inserting Records if selected lists is not exist from Previues Selected  Parts lists in Database
                                            bool newItemIsExist = DBPartsUpdateOBJ.Super_Admin_Assemblies_Parts.Any(m => m.Assemblies_Id == model.AssemblyId && m.Part_Number == item.Part_Number && m.IsActive == true);
                                            if (newItemIsExist == false)
                                            {

                                                Super_Admin_Assemblies_Parts ASM_PartsTbInsertOBJ = new Super_Admin_Assemblies_Parts();
                                                ASM_PartsTbInsertOBJ.Assemblies_Id = model.AssemblyId;
                                                ASM_PartsTbInsertOBJ.Assemblies_Name = model.Assemblies_Name;
                                                if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                                                {
                                                    ASM_PartsTbInsertOBJ.Assemblies_Category = model.Assemblies_Category;
                                                }
                                                if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                                                {
                                                    ASM_PartsTbInsertOBJ.Assemblies_Category = model.OtherAssemblies_Category;
                                                }
                                                ASM_PartsTbInsertOBJ.Part_Number = item.Part_Number.Trim();
                                                ASM_PartsTbInsertOBJ.Part_Category = item.Part_Category;
                                                ASM_PartsTbInsertOBJ.Part_Cost = item.Part_Cost;
                                                ASM_PartsTbInsertOBJ.Part_Resale = item.Resale_Cost;
                                                ASM_PartsTbInsertOBJ.EstimatedCost_Total = item.EstCost_Total;
                                                ASM_PartsTbInsertOBJ.EstimatedResale_Total = item.EstResaleCost_Total;
                                                ASM_PartsTbInsertOBJ.Estimated_Qty = item.Estimated_Qty;
                                                ASM_PartsTbInsertOBJ.IsActive = true;
                                                ASM_PartsTbInsertOBJ.LaborUnit = item.LaborUnit ?? 0;
                                                DBPartsUpdateOBJ.Super_Admin_Assemblies_Parts.Add(ASM_PartsTbInsertOBJ);
                                                ASMLabourResultCount = DBPartsUpdateOBJ.SaveChanges();
                                                logs.WriteLog("Updated the Assembly Parts - " + ASM_PartsTbInsertOBJ.Part_Number, model.Client_ID, model.Updated_By);
                                            }

                                        }

                                    }

                                }
                                if (AStblResultCount > 0 || ASMPartsResultCount > 0)
                                {
                                    response.ResultCode = 1;
                                    response.Message = "“Assemblies details” has been updated successfully.";
                                    return response;
                                }
                                else
                                {
                                    response.ResultCode = -1;
                                    response.Message = "No changes found in your “Assembly’s Labor Details”.";
                                    return response;
                                }
                            }
                        }
                        else
                        {
                            response.ResultCode = -1;
                            response.Message = "An error occurred while updating “Assembly Part Details”!";
                            return response;
                        }
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "Assembly already exists. Please try different name!";
                        return response;
                    }

                }
                

            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While updating the Assembly - " + model.Assemblies_Name + " [Error Msg - " + ex.Message + " ]", model.Client_ID, model.Updated_By);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;

            }
        }
        /// <summary>
        /// UpdateDistribotorAssembliesDetails
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ServiceResult<int> UpdateDistribotorAssembliesDetails(Assembly_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            List<AssembliesParts_DetailsInfoList> PartsList = new List<AssembliesParts_DetailsInfoList>();
            List<AssembliesParts_DetailsInfoList> ExistAssembliesPartsList = new List<AssembliesParts_DetailsInfoList>();
            try
            {
                //Here  starts in Updating Assemblies Details.....
                using (ElectricEaseEntitiesContext DbASOBJ = new ElectricEaseEntitiesContext())
                {
                    //Here Updating Assemblies Information.....
                    Distributor_Assemblies_Master ASMtbl = new Distributor_Assemblies_Master();
                    model.Assemblies_Name = model.Assemblies_Name.Trim();

                    var checkAssembly = (from m in DbASOBJ.Distributor_Assemblies_Master where m.Distributor_Id == model.DistributorID && m.AssemblyID != model.AssemblyId && m.Assemblies_Name == model.Assemblies_Name select m).FirstOrDefault();

                    if (checkAssembly == null)
                    {
                        ASMtbl = (from m in DbASOBJ.Distributor_Assemblies_Master where m.Distributor_Id == model.DistributorID && m.AssemblyID == model.AssemblyId select m).FirstOrDefault();

                        ASMtbl.Distributor_Id = model.DistributorID;
                        ASMtbl.Assemblies_Name = model.Assemblies_Name;

                        if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                        {
                            ASMtbl.Assemblies_Category = model.Assemblies_Category;
                        }
                        if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                        {
                            ASMtbl.Assemblies_Category = model.OtherAssemblies_Category;
                        }

                        ASMtbl.Assemblies_Description = model.Assemblies_Description;
                        ASMtbl.severity = model.severity;
                        ASMtbl.labor_cost = model.labor_cost;
                        ASMtbl.labor_ResaleCost = model.labor_ResaleCost;
                        ASMtbl.labor_EstimatedHours = model.labor_EstimatedHours;
                        ASMtbl.Labor_EstimatedCostTotal = model.LaborEst_CostTotal;
                        ASMtbl.Lobor_EstimatedResaleTotal = model.LaborEst_ResaleTotal;
                        ASMtbl.Parts_EstimatedCostTotal = model.PartCostTotal;
                        ASMtbl.Parts_EstimatedResaleTotal = model.PartResaleTotal;
                        ASMtbl.Estimated_Qty_Total = model.Est_Qty_Total;
                        ASMtbl.Grand_EstCost_Total = model.GrandCostTotal;
                        ASMtbl.Grand_EstResale_Total = model.GrandResaleTotal;


                        ASMtbl.Updated_By = model.Updated_By;
                        ASMtbl.Updated_Date = DateTime.Now;
                        ASMtbl.Isactive = model.Active;
                        int AStblResultCount = DbASOBJ.SaveChanges();
                        logs.WriteLog("Updated the Assembly - " + model.Assemblies_Name, model.Client_ID, model.Updated_By);
                        //Here Updating Assemblies Parts Details....
                        if (AStblResultCount > 0)
                        {
                            using (ElectricEaseEntitiesContext DBPartsOBJ = new ElectricEaseEntitiesContext())
                            {
                                //Here Getting Previously Added Assemblies parts List from Database...
                                Distributor_Assemblies_Parts ASM_PartsTbOBJ = new Distributor_Assemblies_Parts();
                                //List<AssembliesLabourDetailsList> ExistAssembliesLabourList = new List<AssembliesLabourDetailsList>();
                                ExistAssembliesPartsList = (from m in DBPartsOBJ.Distributor_Assemblies_Parts
                                                            where m.Distributor_ID == model.DistributorID && m.AssemblyID == model.AssemblyId

                                                            select new AssembliesParts_DetailsInfoList()
                                                            {
                                                                Part_Number = m.Part_Number.Trim(),
                                                                Assemblies_Name = m.Assemblies_Name,
                                                                LaborUnit = m.LaborUnit,
                                                                AssemblyID = m.AssemblyID

                                                            }).ToList();

                                //Here Existing  Db assemblies partslists is compare with new Selected List from Model....
                                foreach (var ExistSelectedItem in ExistAssembliesPartsList)
                                {
                                    var MatchItem = (from m in model.PartsListData where m.Part_Number.Trim() == ExistSelectedItem.Part_Number.Trim() select m).FirstOrDefault();
                                    if (MatchItem == null)
                                    {
                                        //Here to Set Existing selected Parts Record if not selected from Updated Assemblies Parts Details...
                                        ASM_PartsTbOBJ = (from m in DBPartsOBJ.Distributor_Assemblies_Parts
                                                          where m.Distributor_ID == model.DistributorID && m.AssemblyID == model.AssemblyId && m.Part_Number == ExistSelectedItem.Part_Number.Trim()
                                                          select m).FirstOrDefault();
                                        ASM_PartsTbOBJ.IsActive = false;
                                        int count = DBPartsOBJ.SaveChanges();
                                    }

                                }
                                //Here Starts to Update assemblies Parts Details....
                                using (ElectricEaseEntitiesContext DBPartsUpdateOBJ = new ElectricEaseEntitiesContext())
                                {
                                    Distributor_Assemblies_Parts ASM_PartsTBLobj = new Distributor_Assemblies_Parts();

                                    //Here Assemblies Parts Lists is Updated based on the new updated list in the Model....
                                    foreach (var item in model.PartsListData)
                                    {
                                        ASM_PartsTBLobj = (from m in DBPartsUpdateOBJ.Distributor_Assemblies_Parts
                                                           where m.Distributor_ID == model.DistributorID && m.AssemblyID == model.AssemblyId && m.Part_Number == item.Part_Number.Trim()
                                                           select m).FirstOrDefault();
                                        if (ASM_PartsTBLobj != null)
                                        {
                                            if (item.Part_Number.Trim() == ASM_PartsTBLobj.Part_Number.Trim())
                                            {
                                                ASM_PartsTBLobj.AssemblyID = model.AssemblyId;
                                                ASM_PartsTBLobj.Assemblies_Name = model.Assemblies_Name;
                                                ASM_PartsTBLobj.Part_Number = item.Part_Number.Trim();
                                                ASM_PartsTBLobj.Part_Category = item.Part_Category;
                                                ASM_PartsTBLobj.Part_Cost = item.Part_Cost;
                                                ASM_PartsTBLobj.Part_Resale = item.Resale_Cost;
                                                ASM_PartsTBLobj.Estimated_Qty = item.Estimated_Qty;
                                                ASM_PartsTBLobj.EstimatedCost_Total = item.EstCost_Total;
                                                ASM_PartsTBLobj.EstimatedResale_Total = item.EstResaleCost_Total;
                                                ASM_PartsTBLobj.IsActive = true;
                                                ASM_PartsTBLobj.LaborUnit = item.LaborUnit ?? 0;
                                                ASMPartsResultCount = DBPartsUpdateOBJ.SaveChanges();
                                                logs.WriteLog("Updated the Assembly Parts - " + ASM_PartsTBLobj.Part_Number, model.DistributorID, model.Updated_By);
                                            }
                                        }
                                        else
                                        {
                                            //Here to Inserting Records if selected lists is not exist from Previues Selected  Parts lists in Database
                                            bool newItemIsExist = DBPartsUpdateOBJ.Distributor_Assemblies_Parts.Any(m => m.Distributor_ID == model.DistributorID && m.Part_Number == item.Part_Number && m.AssemblyID == model.AssemblyId);
                                            if (newItemIsExist == false)
                                            {

                                                Distributor_Assemblies_Parts ASM_PartsTbInsertOBJ = new Distributor_Assemblies_Parts();
                                                ASM_PartsTbInsertOBJ.AssemblyID = model.AssemblyId;
                                                ASM_PartsTbInsertOBJ.Distributor_ID = model.DistributorID;
                                                ASM_PartsTbInsertOBJ.Assemblies_Name = model.Assemblies_Name;

                                                if (model.Assemblies_Category != null && model.Assemblies_Category != "0" && model.Assemblies_Category != "1" && model.Assemblies_Category.ToLower() != "other")
                                                {
                                                    ASM_PartsTbInsertOBJ.Assemblies_Category = model.Assemblies_Category;
                                                }
                                                if ((model.Assemblies_Category.ToLower() == "other" || model.Assemblies_Category == "1") && model.OtherAssemblies_Category != null)
                                                {
                                                    ASM_PartsTbInsertOBJ.Assemblies_Category = model.OtherAssemblies_Category;
                                                }
                                                ASM_PartsTbInsertOBJ.Part_Number = item.Part_Number.Trim();
                                                ASM_PartsTbInsertOBJ.Part_Category = item.Part_Category;
                                                ASM_PartsTbInsertOBJ.Part_Cost = item.Part_Cost;
                                                ASM_PartsTbInsertOBJ.Part_Resale = item.Resale_Cost;
                                                ASM_PartsTbInsertOBJ.EstimatedCost_Total = item.EstCost_Total;
                                                ASM_PartsTbInsertOBJ.EstimatedResale_Total = item.EstResaleCost_Total;
                                                ASM_PartsTbInsertOBJ.Estimated_Qty = item.Estimated_Qty;
                                                ASM_PartsTbInsertOBJ.IsActive = true;
                                                ASM_PartsTbInsertOBJ.LaborUnit = item.LaborUnit ?? 0;
                                                DBPartsUpdateOBJ.Distributor_Assemblies_Parts.Add(ASM_PartsTbInsertOBJ);
                                                ASMLabourResultCount = DBPartsUpdateOBJ.SaveChanges();
                                                logs.WriteLog("Updated the Assembly Parts - " + ASM_PartsTbInsertOBJ.Part_Number, model.DistributorID, model.Updated_By);
                                            }

                                        }

                                    }

                                }
                                if (AStblResultCount > 0 || ASMPartsResultCount > 0)
                                {
                                    response.ResultCode = 1;
                                    response.Message = "“Assemblies details” has been updated successfully.";
                                    return response;
                                }
                                else
                                {
                                    response.ResultCode = -1;
                                    response.Message = "No changes found in your “Assembly’s Labor Details”.";
                                    return response;
                                }
                            }
                        }
                        else
                        {
                            response.ResultCode = -1;
                            response.Message = "An error occurred while updating “Assembly Part Details”!";
                            return response;
                        }

                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "Assembly already exists. Please choose different name.";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While updating the Assembly - " + model.Assemblies_Name + " [Error Msg - " + ex.Message + " ]", model.DistributorID, model.Updated_By);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;

            }
        }

        public ServiceResult<bool> AssemblieNameIsactive(string Assemblies_Name)
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    bool result = DbOBJ.Assemblies_Master.Any(m => m.Assemblies_Name == Assemblies_Name && m.Isactive == true);

                    if (result == true)
                    {
                        response.ResultCode = 1;
                        response.Message = "“Assemblies Name” does Exist!";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = 0;
                        response.Message = "“Assembly Name” does not exist!";
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
        /// <summary>
        /// AssemblyExport
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string AssemblyExport(selectedAssembilesIds model)
        {
            string status = "";
            string Assemblies_Name = "";
            string delimiter = "";
            string EmailAssembileslist = "";
            List<PartsNames> partnumber = new List<PartsNames>();
            StringBuilder assembliesList = new StringBuilder();
            StringBuilder clientList = new StringBuilder();
            StringBuilder distributorList = new StringBuilder();

            List<string> AssemblyNames = new List<string>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    if (model.ClienId != null)
                    {
                        foreach (var item in model.ClienId)
                        {
                            partnumber.Clear();
                            AssemblyNames.Clear();
                            EmailAssembileslist = "";
                            delimiter = "";
                            if (model.AssemblyId.Count > 0)
                            {
                                foreach (var assemid in model.AssemblyId)
                                {
                                    decimal EstcostTotal = 0;
                                    decimal EstReasaleTotal = 0;
                                    var NationawideAddemblydetails = db.Super_Admin_Assemblies_Master.Where(x => x.Assemblies_Id == assemid.ID && x.Isactive == true).FirstOrDefault();
                                    // var NationWideAssemblyParts = db.Super_Admin_Assemblies_Parts.Join(db.Super_Admin_Parts_Details,a=>a.Part_Number,b=>b.Part_Number).Where(x => x.Assemblies_Id == assemid.ID && x.IsActive == true).ToList();
                                    var NationWideAssemblyParts = db.Super_Admin_Assemblies_Parts.Join(db.Super_Admin_Parts_Details, a => a.Part_Number, b => b.Part_Number, (a, b) => new { AssmblyPart = a, NationPart = b }).Where(x => x.AssmblyPart.Assemblies_Id == assemid.ID && x.AssmblyPart.IsActive == true).ToList();
                                    if (NationawideAddemblydetails != null)
                                    {
                                        int ASMresultCount;
                                        Assemblies_Master ASMobj = new Assemblies_Master();
                                        NationawideAddemblydetails.Assemblies_Name = NationawideAddemblydetails.Assemblies_Name.Trim();
                                        Assemblies_Master AssembliesIsExist = db.Assemblies_Master.Where(x => x.Assemblies_Name == NationawideAddemblydetails.Assemblies_Name && x.Client_Id == item.ID && x.Isactive == true).FirstOrDefault();
                                        if (AssembliesIsExist != null)
                                        {
                                            //Skipping the loop when the assembly already exist at client side
                                            status = "Success";
                                            continue;

                                            //var count = db.Assemblies_Master.Where(x => x.Assemblies_Name.Contains(NationawideAddemblydetails.Assemblies_Name) && x.Client_Id == item.ID).ToList();
                                            //Assemblies_Name = String.Format("{0}_{1}", NationawideAddemblydetails.Assemblies_Name, count.Count + 1);
                                        }
                                        else
                                        {
                                            Assemblies_Name = NationawideAddemblydetails.Assemblies_Name;

                                            //To store Assembly List for the Parts calculation
                                            assembliesList.Append(Assemblies_Name);
                                            assembliesList.Append(",");

                                            clientList.Append(item.ID.ToString());
                                            clientList.Append(',');
                                        }
                                        ASMobj.Client_Id = item.ID;
                                        ASMobj.Assemblies_Name = Assemblies_Name;
                                        ASMobj.Assemblies_Category = NationawideAddemblydetails.Assemblies_Category;
                                        ASMobj.Assemblies_Description = NationawideAddemblydetails.Assemblies_Description;
                                        ASMobj.severity = NationawideAddemblydetails.severity;
                                        ASMobj.labor_cost = NationawideAddemblydetails.labor_cost;
                                        ASMobj.labor_ResaleCost = NationawideAddemblydetails.labor_ResaleCost;
                                        ASMobj.labor_EstimatedHours = NationawideAddemblydetails.labor_EstimatedHours;
                                        ASMobj.Labor_EstimatedCostTotal = NationawideAddemblydetails.Labor_EstimatedCostTotal;
                                        ASMobj.Lobor_EstimatedResaleTotal = NationawideAddemblydetails.Lobor_EstimatedResaleTotal;
                                        ASMobj.Parts_EstimatedCostTotal = NationawideAddemblydetails.Parts_EstimatedCostTotal;
                                        ASMobj.Parts_EstimatedResaleTotal = NationawideAddemblydetails.Parts_EstimatedResaleTotal;
                                        ASMobj.Estimated_Qty_Total = NationawideAddemblydetails.Estimated_Qty_Total;
                                        ASMobj.Grand_EstCost_Total = NationawideAddemblydetails.Grand_EstCost_Total;
                                        ASMobj.Grand_EstResale_Total = NationawideAddemblydetails.Grand_EstResale_Total;
                                        ASMobj.Type = "NationWide";
                                        ASMobj.Created_By = NationawideAddemblydetails.Created_By;
                                        ASMobj.Created_Date = DateTime.Now;
                                        ASMobj.Updated_By = NationawideAddemblydetails.Created_By;
                                        ASMobj.Updated_Date = DateTime.Now;
                                        ASMobj.Isactive = true;
                                        db.Assemblies_Master.Add(ASMobj);
                                        AssemblyNames.Add(Assemblies_Name);
                                        logs.WriteLog("Adding New Assemnbly - " + Assemblies_Name, item.ID, NationawideAddemblydetails.Created_By);
                                        ASMresultCount = db.SaveChanges();

                                        logs.WriteLog("Successfully Added New Assemnbly - " + Assemblies_Name, item.ID, NationawideAddemblydetails.Created_By);
                                        if (ASMresultCount > 0)
                                        {

                                            using (ElectricEaseEntitiesContext DbOBJ2 = new ElectricEaseEntitiesContext())
                                            {
                                                Assemblies_Parts ASM_PartsTBLobj = new Assemblies_Parts();
                                                if (NationWideAssemblyParts.Count > 0)
                                                {
                                                    foreach (var Nparts in NationWideAssemblyParts)
                                                    {

                                                        Parts_Details PDTblObj = new Parts_Details();
                                                        string stat = "";
                                                        bool isupdate = true;
                                                        //validate User ID 
                                                        var PDTblObj1 = db.Parts_Details.Where(x => x.Part_Number == Nparts.AssmblyPart.Part_Number && x.Client_Id == item.ID).FirstOrDefault();
                                                        if (PDTblObj1 == null)
                                                        {
                                                            if (Nparts.AssmblyPart.Part_Cost == 0 || Nparts.AssmblyPart.Part_Resale == 0)
                                                            {
                                                                PartsNames ppartnumber = new PartsNames();
                                                                ppartnumber.assemblyName = Assemblies_Name;
                                                                ppartnumber.Partnumber = Nparts.AssmblyPart.Part_Number;
                                                                ppartnumber.Category = Nparts.AssmblyPart.Part_Category;
                                                                ppartnumber.Cost = Convert.ToString(Nparts.AssmblyPart.Part_Cost);
                                                                ppartnumber.Resale = Convert.ToString(Nparts.AssmblyPart.Part_Resale);
                                                                partnumber.Add(ppartnumber);
                                                            }
                                                            PDTblObj.Client_Id = item.ID;
                                                            PDTblObj.Part_Category = Nparts.AssmblyPart.Part_Category;
                                                            PDTblObj.Description = Nparts.NationPart.Description;
                                                            PDTblObj.Client_Description = Nparts.NationPart.Client_Description;
                                                            PDTblObj.Cost = Nparts.AssmblyPart.Part_Cost;
                                                            PDTblObj.Resale_Cost = Nparts.AssmblyPart.Part_Resale;
                                                            PDTblObj.Purchased_From = Nparts.NationPart.Purchased_From;
                                                            PDTblObj.UOM = Nparts.NationPart.UOM;
                                                            PDTblObj.LaborUnit = Nparts.AssmblyPart.LaborUnit ?? 0;
                                                            PDTblObj.IsActive = true;
                                                            PDTblObj.Created_By = "Admin";
                                                            PDTblObj.Created_Date = DateTime.Now;
                                                            PDTblObj.Updated_By = "Admin";
                                                            PDTblObj.Updated_Date = DateTime.Now;
                                                            PDTblObj.Part_Number = Nparts.AssmblyPart.Part_Number.Trim();
                                                            PDTblObj.IsActive = true;
                                                            PDTblObj.Type = "NationWide";
                                                            db.Parts_Details.Add(PDTblObj);
                                                            stat = "“New Parts” has been added successfully.";
                                                            logs.WriteLog("Adding the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Created_By);
                                                            int resultcount = db.SaveChanges();
                                                            ASM_PartsTBLobj.Client_ID = item.ID;
                                                            ASM_PartsTBLobj.Assemblies_Name = Assemblies_Name;
                                                            ASM_PartsTBLobj.Assemblies_Category = Nparts.AssmblyPart.Assemblies_Category;
                                                            // ASM_PartsTBLobj.Part_ID = model.Part_ID;
                                                            ASM_PartsTBLobj.Part_Number = Nparts.AssmblyPart.Part_Number.Trim();
                                                            ASM_PartsTBLobj.Part_Category = Nparts.AssmblyPart.Part_Category;
                                                            ASM_PartsTBLobj.Part_Cost = Nparts.AssmblyPart.Part_Cost;
                                                            ASM_PartsTBLobj.Part_Resale = Nparts.AssmblyPart.Part_Resale;
                                                            ASM_PartsTBLobj.LaborUnit = Nparts.AssmblyPart.LaborUnit ?? 0;
                                                            ASM_PartsTBLobj.Estimated_Qty = Nparts.AssmblyPart.Estimated_Qty;
                                                            ASM_PartsTBLobj.EstimatedCost_Total = Nparts.AssmblyPart.EstimatedCost_Total;
                                                            ASM_PartsTBLobj.EstimatedResale_Total = Nparts.AssmblyPart.EstimatedResale_Total;
                                                            ASM_PartsTBLobj.IsActive = true;

                                                            logs.WriteLog("Adding New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, item.ID, NationawideAddemblydetails.Created_By);
                                                            DbOBJ2.Assemblies_Parts.Add(ASM_PartsTBLobj);
                                                            ASMPartsResultCount = DbOBJ2.SaveChanges();
                                                            logs.WriteLog("Successfully Added New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, item.ID, NationawideAddemblydetails.Created_By);//isupdate = true;
                                                            EstcostTotal += Nparts.AssmblyPart.EstimatedCost_Total;
                                                            EstReasaleTotal += Nparts.AssmblyPart.EstimatedResale_Total;
                                                        }
                                                        else
                                                        {
                                                            if (PDTblObj1.Cost == 0 || PDTblObj1.Resale_Cost == 0)
                                                            {
                                                                PartsNames ppartnumber = new PartsNames();
                                                                ppartnumber.assemblyName = Assemblies_Name;
                                                                ppartnumber.Partnumber = Nparts.AssmblyPart.Part_Number;
                                                                ppartnumber.Category = Nparts.AssmblyPart.Part_Category;
                                                                ppartnumber.Cost = PDTblObj1.Cost.ToString();
                                                                ppartnumber.Resale = PDTblObj1.Resale_Cost.ToString();
                                                                partnumber.Add(ppartnumber);
                                                            }
                                                            PDTblObj1.Description = Nparts.NationPart.Description;
                                                            PDTblObj1.Client_Description = Nparts.NationPart.Client_Description;
                                                            PDTblObj1.Purchased_From = Nparts.NationPart.Purchased_From;
                                                            PDTblObj1.UOM = Nparts.NationPart.UOM;
                                                            db.SaveChanges();

                                                            ASM_PartsTBLobj.Client_ID = item.ID;
                                                            ASM_PartsTBLobj.Assemblies_Name = Assemblies_Name;
                                                            ASM_PartsTBLobj.Assemblies_Category = Nparts.AssmblyPart.Assemblies_Category;
                                                            ASM_PartsTBLobj.Part_Number = PDTblObj1.Part_Number.Trim();
                                                            ASM_PartsTBLobj.Part_Category = PDTblObj1.Part_Category ?? Nparts.AssmblyPart.Part_Category;
                                                            ASM_PartsTBLobj.Part_Cost = PDTblObj1.Cost;
                                                            ASM_PartsTBLobj.Part_Resale = PDTblObj1.Resale_Cost ?? 0;
                                                            ASM_PartsTBLobj.LaborUnit = PDTblObj1.LaborUnit ?? 0;
                                                            ASM_PartsTBLobj.Estimated_Qty = Nparts.AssmblyPart.Estimated_Qty;
                                                            ASM_PartsTBLobj.EstimatedCost_Total = PDTblObj1.Cost * Nparts.AssmblyPart.Estimated_Qty;
                                                            ASM_PartsTBLobj.EstimatedResale_Total = ASM_PartsTBLobj.Part_Resale * Nparts.AssmblyPart.Estimated_Qty;
                                                            ASM_PartsTBLobj.IsActive = true;

                                                            logs.WriteLog("Adding New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, item.ID, NationawideAddemblydetails.Created_By);
                                                            DbOBJ2.Assemblies_Parts.Add(ASM_PartsTBLobj);
                                                            ASMPartsResultCount = DbOBJ2.SaveChanges();
                                                            logs.WriteLog("Successfully Added New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, item.ID, NationawideAddemblydetails.Created_By);
                                                            EstcostTotal += ASM_PartsTBLobj.EstimatedCost_Total;
                                                            EstReasaleTotal += ASM_PartsTBLobj.EstimatedResale_Total;
                                                        }

                                                    }
                                                    var assembly = db.Assemblies_Master.Where(x => x.Assemblies_Name == Assemblies_Name && x.Client_Id == item.ID).FirstOrDefault();
                                                    if (assembly != null)
                                                    {
                                                        assembly.Parts_EstimatedCostTotal = EstcostTotal;
                                                        assembly.Parts_EstimatedResaleTotal = EstReasaleTotal;
                                                        assembly.Grand_EstCost_Total = assembly.Labor_EstimatedCostTotal + EstcostTotal;
                                                        assembly.Grand_EstResale_Total = assembly.Lobor_EstimatedResaleTotal + EstReasaleTotal;
                                                        db.SaveChanges();
                                                    }
                                                }

                                                if (ASMPartsResultCount > 0)
                                                {

                                                    status = "Success";
                                                }
                                                else
                                                {
                                                    status = "Fail";
                                                }

                                            }

                                        }
                                        else
                                        {
                                            status = "Success";
                                        }
                                    }
                                    EmailAssembileslist += delimiter;
                                    EmailAssembileslist += Assemblies_Name;
                                    delimiter = ", ";
                                }
                            }
                            if (AssemblyNames.Count > 0 && partnumber.Count > 0)
                            {
                                var client = db.Client_Master.Where(x => x.Client_ID == item.ID && x.IsActive == true).FirstOrDefault();
                                EmailLog _email = new EmailLog();
                                _email.Subject = "Exported Assemblies Names";
                                _email.BodyContent = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/EmailTempLates/AssembliesExp.html"));
                                _email.BodyContent = _email.BodyContent.Replace("##FirstName##", client.Contact_person);
                                string tablebody = "";

                                tablebody = "<table width='96%' border='1' align='center' cellspacing='0' cellpadding='0'><thead><tr><th>Assembly Name</th><th>Part Number</th><th>Category</th><th>Cost</th><th>Resale Cost</th></tr></thead><tbody>";
                                if (AssemblyNames.Count > 0)
                                {
                                    foreach (var aname in AssemblyNames)
                                    {
                                        var paname = partnumber.Where(x => x.assemblyName == aname).FirstOrDefault();
                                        if (paname != null)
                                        {
                                            tablebody += "<tr>";
                                            tablebody += "<td colspan='5' style='color:#0979d8;'>&nbsp;</td></tr>";
                                        }
                                        //tablebody += "<td colspan='5' style='color:#0979d8;'><b>" + aname + "</b></td></tr>";
                                        if (partnumber.Count > 0)
                                        {

                                            foreach (var pnumber in partnumber)
                                            {

                                                //if (pnumber.Cost == "0" && pnumber.Resale == "0")
                                                //{
                                                if (pnumber.assemblyName == aname)
                                                {
                                                    tablebody += "<tr>";
                                                    tablebody += "<td><b>" + aname + "</b></td>";
                                                    tablebody += "<td align='center'>" + pnumber.Partnumber + "</td>";
                                                    tablebody += "<td align='center'>" + pnumber.Category + "</td>";
                                                    if (pnumber.Cost == "0" && pnumber.Resale == "0")
                                                    {
                                                        tablebody += "<td align='center'style='color: #ec0909'>" + pnumber.Cost + "</td>";
                                                        tablebody += "<td align='center'style='color: #ec0909'>" + pnumber.Resale + "</td>";
                                                    }
                                                    else
                                                    {
                                                        tablebody += "<td align='center'>" + pnumber.Cost + "</td>";
                                                        tablebody += "<td align='center'>" + pnumber.Resale + "</td>";
                                                    }
                                                    tablebody += "</tr>";
                                                }
                                                //}
                                            }

                                        }
                                    }
                                }
                                tablebody += "</tbody></table>";
                                _email.BodyContent = _email.BodyContent.Replace("##partslist##", tablebody);
                                _email.EmailTo = client.Email;
                                EmailService em = new EmailService();
                                em.SendEmail(_email);
                            }

                        }

                        //To remove last comma in the list of processed assemblies
                        if (assembliesList.Length > 0)
                        {
                            assembliesList.Remove((assembliesList.Length - 1), 1);
                        }

                        string connStr = ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(connStr))
                        {
                            using (SqlCommand cmd = new SqlCommand("EE_RecalcNationalwideExportClientAssembly", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@AssemblyList", SqlDbType.NVarChar).Value = assembliesList.ToString();
                                cmd.Parameters.Add("@ClientIDList", SqlDbType.NVarChar).Value = clientList.ToString();
                                cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = "Admin Export";

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else if (model.DisbutorId != null)
                    {
                        foreach (var Did in model.DisbutorId)
                        {
                            EmailAssembileslist = "";
                            delimiter = "";
                            partnumber.Clear();
                            AssemblyNames.Clear();
                            if (model.AssemblyId.Count > 0)
                            {
                                foreach (var assemid in model.AssemblyId)
                                {
                                    decimal EstcostTotal = 0;
                                    decimal EstReasaleTotal = 0;
                                    var NationawideAddemblydetails = db.Super_Admin_Assemblies_Master.Where(x => x.Assemblies_Id == assemid.ID && x.Isactive == true).FirstOrDefault();
                                    // var NationWideAssemblyParts = db.Super_Admin_Assemblies_Parts.Where(x => x.Assemblies_Id == assemid.ID && x.IsActive == true).ToList();
                                    var NationWideAssemblyParts = db.Super_Admin_Assemblies_Parts.Join(db.Super_Admin_Parts_Details, a => a.Part_Number, b => b.Part_Number, (a, b) => new { AssmblyPart = a, DistPart = b }).Where(x => x.AssmblyPart.Assemblies_Id == assemid.ID).ToList();
                                    if (NationawideAddemblydetails != null)
                                    {
                                        int ASMresultCount;
                                        Distributor_Assemblies_Master ASMobj = new Distributor_Assemblies_Master();
                                        NationawideAddemblydetails.Assemblies_Name = NationawideAddemblydetails.Assemblies_Name.Trim();
                                        Distributor_Assemblies_Master AssembliesIsExist = db.Distributor_Assemblies_Master.Where(x => x.Assemblies_Name == NationawideAddemblydetails.Assemblies_Name && x.Distributor_Id == Did.ID).FirstOrDefault();
                                        if (AssembliesIsExist != null)
                                        {
                                            //To skip the duplication operation 
                                            status = "Success";
                                            continue;

                                            //var count = db.Distributor_Assemblies_Master.Where(x => x.Assemblies_Name.Contains(NationawideAddemblydetails.Assemblies_Name) && x.Distributor_Id == Did.ID).ToList();
                                            //Assemblies_Name = String.Format("{0}_{1}", NationawideAddemblydetails.Assemblies_Name, count.Count + 1);

                                        }
                                        else
                                        {
                                            Assemblies_Name = NationawideAddemblydetails.Assemblies_Name;

                                            //To store Assembly List for the Parts calculation
                                            assembliesList.Append(NationawideAddemblydetails.Assemblies_Id);
                                            assembliesList.Append(",");

                                            distributorList.Append(Did.ID.ToString());
                                            distributorList.Append(',');
                                        }
                                        ASMobj.Distributor_Id = Did.ID;
                                        ASMobj.Assemblies_Name = Assemblies_Name;
                                        ASMobj.Assemblies_Category = NationawideAddemblydetails.Assemblies_Category;
                                        ASMobj.Assemblies_Description = NationawideAddemblydetails.Assemblies_Description;
                                        ASMobj.severity = NationawideAddemblydetails.severity;
                                        ASMobj.labor_cost = NationawideAddemblydetails.labor_cost;
                                        ASMobj.labor_ResaleCost = NationawideAddemblydetails.labor_ResaleCost;
                                        ASMobj.labor_EstimatedHours = NationawideAddemblydetails.labor_EstimatedHours;
                                        ASMobj.Labor_EstimatedCostTotal = NationawideAddemblydetails.Labor_EstimatedCostTotal;
                                        ASMobj.Lobor_EstimatedResaleTotal = NationawideAddemblydetails.Lobor_EstimatedResaleTotal;
                                        ASMobj.Parts_EstimatedCostTotal = NationawideAddemblydetails.Parts_EstimatedCostTotal;
                                        ASMobj.Parts_EstimatedResaleTotal = NationawideAddemblydetails.Parts_EstimatedResaleTotal;
                                        ASMobj.Estimated_Qty_Total = NationawideAddemblydetails.Estimated_Qty_Total;
                                        ASMobj.Grand_EstCost_Total = NationawideAddemblydetails.Grand_EstCost_Total;
                                        ASMobj.Grand_EstResale_Total = NationawideAddemblydetails.Grand_EstResale_Total;
                                        ASMobj.Type = "NationWide";
                                        ASMobj.Created_By = NationawideAddemblydetails.Created_By;
                                        ASMobj.Created_Date = DateTime.Now;
                                        ASMobj.Updated_By = NationawideAddemblydetails.Created_By;
                                        ASMobj.Updated_Date = DateTime.Now;
                                        ASMobj.Isactive = true;
                                        db.Distributor_Assemblies_Master.Add(ASMobj);
                                        AssemblyNames.Add(Assemblies_Name);
                                        logs.WriteLog("Adding New Assemnbly - " + Assemblies_Name, Did.ID, NationawideAddemblydetails.Created_By);
                                        ASMresultCount = db.SaveChanges();
                                        logs.WriteLog("Successfully Added New Assemnbly - " + Assemblies_Name, Did.ID, NationawideAddemblydetails.Created_By);
                                        if (ASMresultCount > 0)
                                        {

                                            using (ElectricEaseEntitiesContext DbOBJ2 = new ElectricEaseEntitiesContext())
                                            {
                                                Distributor_Assemblies_Parts ASM_PartsTBLobj = new Distributor_Assemblies_Parts();
                                                if (NationWideAssemblyParts.Count > 0)
                                                {
                                                    foreach (var Nparts in NationWideAssemblyParts)
                                                    {

                                                        Distributor_Parts_Details PDTblObj = new Distributor_Parts_Details();
                                                        string stat = "";
                                                        bool isupdate = true;
                                                        //validate User ID 
                                                        var PDTblObj1 = db.Distributor_Parts_Details.Where(x => x.Part_Number == Nparts.AssmblyPart.Part_Number && x.Distributor_ID == Did.ID).FirstOrDefault();
                                                        if (PDTblObj1 == null)
                                                        {
                                                            PartsNames ppartnumber = new PartsNames();
                                                            ppartnumber.assemblyName = Assemblies_Name;
                                                            ppartnumber.Partnumber = Nparts.AssmblyPart.Part_Number;
                                                            ppartnumber.Category = Nparts.AssmblyPart.Part_Category;
                                                            ppartnumber.Cost = "0";
                                                            ppartnumber.Resale = "0";
                                                            partnumber.Add(ppartnumber);
                                                            PDTblObj.Client_Id = 0;
                                                            PDTblObj.Distributor_ID = Did.ID;

                                                            PDTblObj.Part_Category = Nparts.DistPart.Part_Category;
                                                            PDTblObj.Description = Nparts.DistPart.Description;
                                                            PDTblObj.Client_Description = Nparts.DistPart.Client_Description;
                                                            PDTblObj.Cost = 0;
                                                            PDTblObj.Resale_Cost = 0;
                                                            PDTblObj.Purchased_From = Nparts.DistPart.Purchased_From;
                                                            PDTblObj.UOM = Nparts.DistPart.UOM;
                                                            PDTblObj.IsActive = true;
                                                            PDTblObj.Created_By = "Admin";
                                                            PDTblObj.Created_Date = DateTime.Now;
                                                            PDTblObj.Updated_By = "Admin";
                                                            PDTblObj.Updated_Date = DateTime.Now;
                                                            PDTblObj.Part_Number = Nparts.AssmblyPart.Part_Number.Trim();
                                                            PDTblObj.IsActive = true;
                                                            PDTblObj.Type = "NationWide";
                                                            PDTblObj.LaborUnit = 0;
                                                            db.Distributor_Parts_Details.Add(PDTblObj);
                                                            stat = "“New Parts” has been added successfully.";
                                                            logs.WriteLog("Adding the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Created_By);
                                                            int resultcount = db.SaveChanges();
                                                            ASM_PartsTBLobj.Distributor_ID = Did.ID;
                                                            ASM_PartsTBLobj.Assemblies_Name = Assemblies_Name;
                                                            ASM_PartsTBLobj.Assemblies_Category = Nparts.AssmblyPart.Assemblies_Category;
                                                            // ASM_PartsTBLobj.Part_ID = model.Part_ID;
                                                            ASM_PartsTBLobj.Part_Number = Nparts.AssmblyPart.Part_Number.Trim();
                                                            ASM_PartsTBLobj.Part_Category = Nparts.AssmblyPart.Part_Category;
                                                            ASM_PartsTBLobj.Part_Cost = Nparts.AssmblyPart.Part_Cost;
                                                            ASM_PartsTBLobj.Part_Resale = Nparts.AssmblyPart.Part_Resale;
                                                            ASM_PartsTBLobj.Estimated_Qty = Nparts.AssmblyPart.Estimated_Qty;
                                                            ASM_PartsTBLobj.EstimatedCost_Total = Nparts.AssmblyPart.EstimatedCost_Total;
                                                            ASM_PartsTBLobj.EstimatedResale_Total = Nparts.AssmblyPart.EstimatedResale_Total;
                                                            ASM_PartsTBLobj.LaborUnit = Nparts.AssmblyPart.LaborUnit ?? 0;
                                                            ASM_PartsTBLobj.IsActive = true;
                                                            ASM_PartsTBLobj.AssemblyID = ASMobj.AssemblyID;
                                                            logs.WriteLog("Adding New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, Did.ID, NationawideAddemblydetails.Created_By);
                                                            DbOBJ2.Distributor_Assemblies_Parts.Add(ASM_PartsTBLobj);
                                                            ASMPartsResultCount = DbOBJ2.SaveChanges();
                                                            logs.WriteLog("Successfully Added New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, Did.ID, NationawideAddemblydetails.Created_By);
                                                            EstcostTotal += Nparts.AssmblyPart.EstimatedCost_Total;
                                                            EstReasaleTotal += Nparts.AssmblyPart.EstimatedResale_Total;
                                                        }
                                                        else
                                                        {
                                                            //if (PDTblObj1.IsActive == false)
                                                            //{
                                                            //    PDTblObj1.IsActive = true;
                                                            //    db.SaveChanges();
                                                            //}
                                                            if (PDTblObj1.Cost == 0 && PDTblObj1.Resale_Cost == 0)
                                                            {
                                                                PartsNames ppartnumber = new PartsNames();
                                                                ppartnumber.assemblyName = Assemblies_Name;
                                                                ppartnumber.Partnumber = Nparts.AssmblyPart.Part_Number;
                                                                ppartnumber.Category = Nparts.AssmblyPart.Part_Category;
                                                                ppartnumber.Cost = PDTblObj1.Cost.ToString();
                                                                ppartnumber.Resale = PDTblObj1.Resale_Cost.ToString();
                                                                partnumber.Add(ppartnumber);
                                                            }
                                                            PDTblObj1.Description = Nparts.DistPart.Description;
                                                            PDTblObj1.Client_Description = Nparts.DistPart.Client_Description;
                                                            PDTblObj1.Purchased_From = Nparts.DistPart.Purchased_From;
                                                            PDTblObj1.UOM = Nparts.DistPart.UOM;
                                                            PDTblObj1.LaborUnit = Nparts.DistPart.LaborUnit ?? 0;
                                                            db.SaveChanges();
                                                            ASM_PartsTBLobj.Distributor_ID = Did.ID;
                                                            ASM_PartsTBLobj.Assemblies_Name = Assemblies_Name;
                                                            ASM_PartsTBLobj.Assemblies_Category = Nparts.AssmblyPart.Assemblies_Category;
                                                            // ASM_PartsTBLobj.Part_ID = model.Part_ID;
                                                            ASM_PartsTBLobj.Part_Number = PDTblObj1.Part_Number.Trim();
                                                            ASM_PartsTBLobj.Part_Category = PDTblObj1.Part_Category;
                                                            ASM_PartsTBLobj.Part_Cost = PDTblObj1.Cost ?? 0;
                                                            ASM_PartsTBLobj.Part_Resale = PDTblObj1.Resale_Cost ?? 0;
                                                            ASM_PartsTBLobj.Estimated_Qty = Nparts.AssmblyPart.Estimated_Qty;
                                                            ASM_PartsTBLobj.EstimatedCost_Total = PDTblObj1.Cost * Nparts.AssmblyPart.Estimated_Qty ?? 0;
                                                            ASM_PartsTBLobj.EstimatedResale_Total = PDTblObj1.Resale_Cost * Nparts.AssmblyPart.Estimated_Qty ?? 0;
                                                            ASM_PartsTBLobj.IsActive = true;
                                                            ASM_PartsTBLobj.LaborUnit = PDTblObj1.LaborUnit ?? 0;
                                                            ASM_PartsTBLobj.AssemblyID = ASMobj.AssemblyID;
                                                            logs.WriteLog("Adding New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, Did.ID, NationawideAddemblydetails.Created_By);
                                                            DbOBJ2.Distributor_Assemblies_Parts.Add(ASM_PartsTBLobj);
                                                            ASMPartsResultCount = DbOBJ2.SaveChanges();
                                                            logs.WriteLog("Successfully Added New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, Did.ID, NationawideAddemblydetails.Created_By);
                                                            EstcostTotal += ASM_PartsTBLobj.EstimatedCost_Total;
                                                            EstReasaleTotal += ASM_PartsTBLobj.EstimatedResale_Total;

                                                        }


                                                    }
                                                    var assembly = db.Distributor_Assemblies_Master.Where(x => x.Assemblies_Name == Assemblies_Name && x.Distributor_Id == Did.ID).FirstOrDefault();
                                                    if (assembly != null)
                                                    {
                                                        assembly.Parts_EstimatedCostTotal = EstcostTotal;
                                                        assembly.Parts_EstimatedResaleTotal = EstReasaleTotal;
                                                        assembly.Grand_EstCost_Total = assembly.Labor_EstimatedCostTotal + EstcostTotal;
                                                        assembly.Grand_EstResale_Total = assembly.Lobor_EstimatedResaleTotal + EstReasaleTotal;
                                                        db.SaveChanges();
                                                    }
                                                }
                                                if (ASMPartsResultCount > 0)
                                                {
                                                    status = "Success";
                                                }
                                                else
                                                {
                                                    status = "Fail";
                                                }

                                            }

                                        }
                                        else
                                        {
                                            status = "Success";
                                        }
                                    }
                                    EmailAssembileslist += delimiter;
                                    EmailAssembileslist += Assemblies_Name;
                                    delimiter = ", ";
                                }
                            }

                            //To remove last comma in the list of processed assemblies
                            //if (assembliesList.Length > 0)
                            //{
                            //    assembliesList.Remove((assembliesList.Length - 1), 1);
                            //}

                            

                            //if (AssemblyNames.Count > 0 && partnumber.Count > 0)
                            //{
                            //    var client = db.Distributor_Master.Where(x => x.ID == Did.ID && x.IsActive == true).FirstOrDefault();
                            //    EmailLog _email = new EmailLog();
                            //    _email.Subject = "Exported Assemblies Names";
                            //    _email.BodyContent = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/EmailTempLates/AssembliesExp.html"));
                            //    _email.BodyContent = _email.BodyContent.Replace("##FirstName##", client.Company);
                            //    string tablebody = "";

                            //    tablebody = "<table width='96%' border='1' align='center' cellspacing='0' cellpadding='0'><thead><tr><th>Assembly Name</th><th>Part Number</th><th>Category</th><th>Cost</th><th>Resale Cost</th></tr></thead><tbody>";
                            //    if (AssemblyNames.Count > 0)
                            //    {
                            //        foreach (var aname in AssemblyNames)
                            //        {
                            //            var paname = partnumber.Where(x => x.assemblyName == aname).FirstOrDefault();
                            //            if (paname != null)
                            //            {
                            //                tablebody += "<tr>";
                            //                tablebody += "<td colspan='5' style='color:#0979d8;'>&nbsp;</td></tr>";
                            //            }
                            //            //tablebody += "<td colspan='5' style='color:#0979d8;'><b>" + aname + "</b></td></tr>";
                            //            if (partnumber.Count > 0)
                            //            {
                            //                //tablebody += "<tr>";
                            //                //tablebody += "<td colspan='5' style='color:#0979d8;'></td></tr>";
                            //                foreach (var pnumber in partnumber)
                            //                {

                            //                    //if (pnumber.Cost == "0" && pnumber.Resale == "0")
                            //                    //{
                            //                    if (pnumber.assemblyName == aname)
                            //                    {

                            //                        tablebody += "<tr>";
                            //                        tablebody += "<td><b>" + aname + "</b></td>";
                            //                        tablebody += "<td align='center'>" + pnumber.Partnumber + "</td>";
                            //                        tablebody += "<td align='center'>" + pnumber.Category + "</td>";
                            //                        if (pnumber.Cost == "0" && pnumber.Resale == "0")
                            //                        {
                            //                            tablebody += "<td align='center'style='color: #ec0909'>" + pnumber.Cost + "</td>";
                            //                            tablebody += "<td align='center'style='color: #ec0909'>" + pnumber.Resale + "</td>";
                            //                        }
                            //                        else
                            //                        {
                            //                            tablebody += "<td align='center'>" + pnumber.Cost + "</td>";
                            //                            tablebody += "<td align='center'>" + pnumber.Resale + "</td>";
                            //                        }
                            //                        tablebody += "</tr>";
                            //                    }
                            //                    //}
                            //                }

                            //            }
                            //        }
                            //    }
                            //    tablebody += "</tbody></table>";
                            //    _email.BodyContent = _email.BodyContent.Replace("##partslist##", tablebody);
                            //    _email.EmailTo = client.Email;
                            //    EmailService em = new EmailService();
                            //    em.SendEmail(_email);
                            //}

                        }
                        string connStr = ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(connStr))
                        {
                            using (SqlCommand cmd = new SqlCommand("EE_RecalcNationalwideExportDistributorAssembly", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@AssemblyList", SqlDbType.NVarChar).Value = assembliesList.ToString();
                                cmd.Parameters.Add("@DistributorIDList", SqlDbType.NVarChar).Value = distributorList.ToString();
                                cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = "Admin Export";

                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }

                    }
                    
                }
                return status;
            }
            catch (Exception ex)
            {
                return status + ex.Message;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string DistributorAssemblyExport(DistributorAssemblies model)
        {
            string status = "";
            string Assemblies_Name = "";
            string delimiter = "";
            string EmailAssembileslist = "";
            List<string> AssemblyNames = new List<string>();
            List<PartsNames> partnumber = new List<PartsNames>();
            StringBuilder assembliesList = new StringBuilder();
            StringBuilder clientList = new StringBuilder();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    if (model.ClienId.Count > 0)
                    {
                        foreach (var item in model.ClienId)
                        {
                            AssemblyNames.Clear();
                            partnumber.Clear();
                            EmailAssembileslist = "";
                            delimiter = "";
                            if (model.selectedAssembiles.Count > 0)
                            {
                                foreach (var assemid in model.selectedAssembiles)
                                {
                                    decimal EstcostTotal = 0;
                                    decimal EstReasaleTotal = 0;
                                    bool isDigitPresent = assemid.Name.Any(c => char.IsDigit(c));
                                    if (isDigitPresent == true)
                                    {
                                        //   string cLastCharacter = assemid.Name.Substring(assemid.Name.Length - 1);
                                        assemid.Name = assemid.Name;
                                        assemid.Name = assemid.Name.Replace("a_a", " ");
                                        //assemid.Name = assemid.Name.Replace(""+cLastCharacter+"", "("+ cLastCharacter +")");
                                    }
                                    else
                                    {
                                        assemid.Name = assemid.Name.Replace("a_a", " ");
                                    }
                                    var DistributorAddemblydetails = db.Distributor_Assemblies_Master.Where(x => x.Distributor_Id == model.DisbutorId && x.Assemblies_Name == assemid.Name && x.Isactive == true).FirstOrDefault();
                                    var DistributorAssemblyParts1 = db.Distributor_Assemblies_Parts.Where(x => x.Distributor_ID == model.DisbutorId && x.Assemblies_Name == assemid.Name && x.IsActive == true).ToList();
                                    var DistributorAssemblyParts = db.Distributor_Assemblies_Parts.Join(db.Distributor_Parts_Details, a => a.Part_Number, b => b.Part_Number, (a, b) => new { AssmblyPart = a, DistPart = b }).Where(x => x.AssmblyPart.Distributor_ID == model.DisbutorId && x.DistPart.Distributor_ID == model.DisbutorId && x.AssmblyPart.Assemblies_Name == assemid.Name && x.AssmblyPart.IsActive == true).ToList();
                                    if (DistributorAddemblydetails != null)
                                    {
                                        int ASMresultCount;
                                        Assemblies_Master ASMobj = new Assemblies_Master();
                                        DistributorAddemblydetails.Assemblies_Name = DistributorAddemblydetails.Assemblies_Name.Trim();
                                        Assemblies_Master AssembliesIsExist = db.Assemblies_Master.Where(x => x.Assemblies_Name == DistributorAddemblydetails.Assemblies_Name && x.Client_Id == item.ID).FirstOrDefault();
                                        if (AssembliesIsExist != null)
                                        {
                                            status = "Success";
                                            continue;
                                            //var count = db.Assemblies_Master.Where(x => x.Assemblies_Name.Contains(DistributorAddemblydetails.Assemblies_Name) && x.Client_Id == item.ID).ToList();
                                            //Assemblies_Name = String.Format("{0}_{1}", DistributorAddemblydetails.Assemblies_Name, count.Count + 1);
                                        }
                                        else
                                        {
                                            Assemblies_Name = DistributorAddemblydetails.Assemblies_Name;
                                        }
                                        ASMobj.Client_Id = item.ID;
                                        ASMobj.Assemblies_Name = Assemblies_Name;
                                        ASMobj.Assemblies_Category = DistributorAddemblydetails.Assemblies_Category;
                                        ASMobj.Assemblies_Description = DistributorAddemblydetails.Assemblies_Description;
                                        ASMobj.severity = DistributorAddemblydetails.severity;
                                        ASMobj.labor_cost = DistributorAddemblydetails.labor_cost;
                                        ASMobj.labor_ResaleCost = DistributorAddemblydetails.labor_ResaleCost;
                                        ASMobj.labor_EstimatedHours = DistributorAddemblydetails.labor_EstimatedHours;
                                        ASMobj.Labor_EstimatedCostTotal = DistributorAddemblydetails.Labor_EstimatedCostTotal;
                                        ASMobj.Lobor_EstimatedResaleTotal = DistributorAddemblydetails.Lobor_EstimatedResaleTotal;
                                        ASMobj.Parts_EstimatedCostTotal = DistributorAddemblydetails.Parts_EstimatedCostTotal;
                                        ASMobj.Parts_EstimatedResaleTotal = DistributorAddemblydetails.Parts_EstimatedResaleTotal;
                                        ASMobj.Estimated_Qty_Total = DistributorAddemblydetails.Estimated_Qty_Total;
                                        ASMobj.Grand_EstCost_Total = DistributorAddemblydetails.Grand_EstCost_Total;
                                        ASMobj.Grand_EstResale_Total = DistributorAddemblydetails.Grand_EstResale_Total;
                                        ASMobj.Type = "Distributor";
                                        ASMobj.Created_By = DistributorAddemblydetails.Created_By;
                                        ASMobj.Created_Date = DateTime.Now;
                                        ASMobj.Updated_By = DistributorAddemblydetails.Created_By;
                                        ASMobj.Updated_Date = DateTime.Now;
                                        ASMobj.Isactive = true;
                                        db.Assemblies_Master.Add(ASMobj);

                                        //To get the assemblies to be processed
                                        AssemblyNames.Add(Assemblies_Name);


                                        logs.WriteLog("Adding New Assemnbly - " + Assemblies_Name, item.ID, DistributorAddemblydetails.Created_By);
                                        ASMresultCount = db.SaveChanges();

                                        logs.WriteLog("Successfully Added New Assemnbly - " + Assemblies_Name, item.ID, DistributorAddemblydetails.Created_By);
                                        if (ASMresultCount > 0)
                                        {
                                            using (ElectricEaseEntitiesContext DbOBJ2 = new ElectricEaseEntitiesContext())
                                            {
                                                Assemblies_Parts ASM_PartsTBLobj = new Assemblies_Parts();
                                                if (DistributorAssemblyParts.Count > 0)
                                                {
                                                    foreach (var Nparts in DistributorAssemblyParts)
                                                    {

                                                        Parts_Details PDTblObj = new Parts_Details();
                                                        string stat = "";
                                                        bool isupdate = true;
                                                        //validate User ID 
                                                        var PDTblObj1 = db.Parts_Details.Where(x => x.Part_Number == Nparts.AssmblyPart.Part_Number && x.Client_Id == item.ID).FirstOrDefault();

                                                        //If Part not present in the Client
                                                        if (PDTblObj1 == null)
                                                        {
                                                            PartsNames ppnumbe = new PartsNames();
                                                            ppnumbe.assemblyName = Assemblies_Name;
                                                            ppnumbe.Partnumber = Nparts.AssmblyPart.Part_Number;
                                                            ppnumbe.Category = Nparts.AssmblyPart.Part_Category;
                                                            ppnumbe.Cost = "0";
                                                            ppnumbe.Resale = "0";

                                                            partnumber.Add(ppnumbe);
                                                            PDTblObj.Client_Id = item.ID;
                                                            PDTblObj.Part_Category = Nparts.AssmblyPart.Part_Category;
                                                            PDTblObj.Description = Nparts.DistPart.Description;
                                                            PDTblObj.Client_Description = Nparts.DistPart.Client_Description;
                                                            PDTblObj.Cost = 0;
                                                            PDTblObj.Resale_Cost = 0;
                                                            PDTblObj.LaborUnit = Nparts.AssmblyPart.LaborUnit ?? 0;
                                                            PDTblObj.Purchased_From = Nparts.DistPart.Purchased_From;
                                                            PDTblObj.UOM = Nparts.DistPart.UOM;
                                                            PDTblObj.IsActive = true;
                                                            PDTblObj.Created_By = "Admin";
                                                            PDTblObj.Created_Date = DateTime.Now;
                                                            PDTblObj.Updated_By = "Admin";
                                                            PDTblObj.Updated_Date = DateTime.Now;
                                                            PDTblObj.Part_Number = Nparts.AssmblyPart.Part_Number.Trim();
                                                            PDTblObj.IsActive = true;
                                                            PDTblObj.Type = "Distributor";
                                                            //PDTblObj.LaborUnit = 0;
                                                            db.Parts_Details.Add(PDTblObj);
                                                            stat = "“New Parts” has been added successfully.";
                                                            logs.WriteLog("Adding the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Created_By);
                                                            int resultcount = db.SaveChanges();

                                                            //Distributor Client Assembly
                                                            ASM_PartsTBLobj.Client_ID = item.ID;
                                                            ASM_PartsTBLobj.Assemblies_Name = Assemblies_Name;
                                                            ASM_PartsTBLobj.Assemblies_Category = Nparts.AssmblyPart.Assemblies_Category;
                                                            // ASM_PartsTBLobj.Part_ID = model.Part_ID;
                                                            ASM_PartsTBLobj.Part_Number = Nparts.AssmblyPart.Part_Number.Trim();
                                                            ASM_PartsTBLobj.Part_Category = Nparts.AssmblyPart.Part_Category;
                                                            //ASM_PartsTBLobj.Part_Cost = Nparts.Part_Cost;
                                                            //ASM_PartsTBLobj.Part_Resale =Nparts.Part_Resale;
                                                            ASM_PartsTBLobj.Part_Cost = 0;
                                                            ASM_PartsTBLobj.Part_Resale = 0;
                                                            ASM_PartsTBLobj.LaborUnit = Nparts.AssmblyPart.LaborUnit ?? 0;
                                                            ASM_PartsTBLobj.Estimated_Qty = Nparts.AssmblyPart.Estimated_Qty;
                                                            //ASM_PartsTBLobj.EstimatedCost_Total = Nparts.EstimatedCost_Total;
                                                            //ASM_PartsTBLobj.EstimatedResale_Total = Nparts.EstimatedResale_Total;
                                                            ASM_PartsTBLobj.EstimatedCost_Total = Nparts.AssmblyPart.Estimated_Qty * 0;
                                                            ASM_PartsTBLobj.EstimatedResale_Total = Nparts.AssmblyPart.Estimated_Qty * 0;
                                                            ASM_PartsTBLobj.IsActive = true;
                                                            logs.WriteLog("Adding New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, item.ID, DistributorAddemblydetails.Created_By);
                                                            DbOBJ2.Assemblies_Parts.Add(ASM_PartsTBLobj);
                                                            ASMPartsResultCount = DbOBJ2.SaveChanges();
                                                            logs.WriteLog("Successfully Added New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, item.ID, DistributorAddemblydetails.Created_By);
                                                            //EstcostTotal += Nparts.EstimatedCost_Total;
                                                            //EstReasaleTotal += Nparts.EstimatedResale_Total;
                                                            EstcostTotal += ASM_PartsTBLobj.EstimatedCost_Total;
                                                            EstReasaleTotal += ASM_PartsTBLobj.EstimatedResale_Total;

                                                        }
                                                        //If Part present in the client
                                                        else
                                                        {
                                                            if (PDTblObj1.Cost == 0 && PDTblObj1.Resale_Cost == 0)
                                                            {
                                                                PartsNames ppartnumber = new PartsNames();
                                                                ppartnumber.assemblyName = Assemblies_Name;
                                                                ppartnumber.Partnumber = Nparts.AssmblyPart.Part_Number;
                                                                ppartnumber.Category = Nparts.AssmblyPart.Part_Category;
                                                                ppartnumber.Cost = PDTblObj1.Cost.ToString();
                                                                ppartnumber.Resale = PDTblObj1.Resale_Cost.ToString();
                                                                partnumber.Add(ppartnumber);
                                                            }
                                                            PDTblObj1.Description = Nparts.DistPart.Description;
                                                            PDTblObj1.Client_Description = Nparts.DistPart.Client_Description;
                                                            PDTblObj1.Purchased_From = Nparts.DistPart.Purchased_From;
                                                            PDTblObj1.UOM = Nparts.DistPart.UOM;
                                                            ASM_PartsTBLobj.Client_ID = item.ID;
                                                            ASM_PartsTBLobj.Assemblies_Name = Assemblies_Name;
                                                            ASM_PartsTBLobj.Assemblies_Category = Nparts.AssmblyPart.Assemblies_Category;
                                                            ASM_PartsTBLobj.Part_Number = PDTblObj1.Part_Number.Trim();
                                                            ASM_PartsTBLobj.Part_Category = PDTblObj1.Part_Category ?? Nparts.AssmblyPart.Part_Category;
                                                            ASM_PartsTBLobj.Part_Cost = PDTblObj1.Cost;
                                                            ASM_PartsTBLobj.Part_Resale = PDTblObj1.Resale_Cost ?? 0;
                                                            ASM_PartsTBLobj.LaborUnit = PDTblObj1.LaborUnit ?? 0;
                                                            ASM_PartsTBLobj.Estimated_Qty = Nparts.AssmblyPart.Estimated_Qty;
                                                            ASM_PartsTBLobj.EstimatedCost_Total = PDTblObj1.Cost * Nparts.AssmblyPart.Estimated_Qty;
                                                            ASM_PartsTBLobj.EstimatedResale_Total = ASM_PartsTBLobj.Part_Resale * Nparts.AssmblyPart.Estimated_Qty;
                                                            ASM_PartsTBLobj.IsActive = true;


                                                            logs.WriteLog("Adding New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, item.ID, DistributorAddemblydetails.Created_By);
                                                            DbOBJ2.Assemblies_Parts.Add(ASM_PartsTBLobj);
                                                            ASMPartsResultCount = DbOBJ2.SaveChanges();
                                                            logs.WriteLog("Successfully Added New Assemnbly Parts - " + Nparts.AssmblyPart.Part_Number, item.ID, DistributorAddemblydetails.Created_By);
                                                            EstcostTotal += ASM_PartsTBLobj.EstimatedCost_Total;
                                                            EstReasaleTotal += ASM_PartsTBLobj.EstimatedResale_Total;
                                                        }
                                                    }
                                                    var assembly = db.Assemblies_Master.Where(x => x.Assemblies_Name == Assemblies_Name && x.Client_Id == item.ID).FirstOrDefault();
                                                    if (assembly != null)
                                                    {
                                                        assembly.Parts_EstimatedCostTotal = EstcostTotal;
                                                        assembly.Parts_EstimatedResaleTotal = EstReasaleTotal;
                                                        assembly.Grand_EstCost_Total = assembly.Labor_EstimatedCostTotal + EstcostTotal;
                                                        assembly.Grand_EstResale_Total = assembly.Lobor_EstimatedResaleTotal + EstReasaleTotal;
                                                        db.SaveChanges();
                                                    }

                                                }

                                                if (ASMPartsResultCount > 0)
                                                {

                                                    status = "Success";
                                                }
                                                else
                                                {
                                                    status = "Fail";
                                                }

                                            }

                                        }
                                        else
                                        {
                                            status = "Success";
                                        }
                                    }
                                    EmailAssembileslist += delimiter;
                                    EmailAssembileslist += Assemblies_Name;
                                    delimiter = ", ";
                                }
                            }
                            if (AssemblyNames.Count > 0 && partnumber.Count > 0)
                            {
                                var client = db.Client_Master.Where(x => x.Client_ID == item.ID && x.IsActive == true).FirstOrDefault();
                                EmailLog _email = new EmailLog();
                                _email.Subject = "Exported Assemblies Names";
                                _email.BodyContent = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/EmailTempLates/AssembliesExp.html"));
                                _email.BodyContent = _email.BodyContent.Replace("##FirstName##", client.Contact_person);
                                string tablebody = "";

                                tablebody = "<table width='96%' border='1' align='center' cellspacing='0' cellpadding='0'><thead><tr><th>Assembly Name</th><th>Part Number</th><th>Category</th><th>Cost</th><th>Resale Cost</th></tr></thead><tbody>";
                                if (AssemblyNames.Count > 0)
                                {
                                    foreach (var aname in AssemblyNames)
                                    {
                                        var paname = partnumber.Where(x => x.assemblyName == aname).FirstOrDefault();
                                        if (paname != null)
                                        {
                                            tablebody += "<tr>";
                                            tablebody += "<td colspan='5' style='color:#0979d8;'>&nbsp;</td></tr>";
                                        }
                                        //tablebody += "<td colspan='5' style='color:#0979d8;'><b>" + aname + "</b></td></tr>";
                                        if (partnumber.Count > 0)
                                        {
                                            //tablebody += "<tr>";
                                            //tablebody += "<td colspan='5' style='color:#0979d8;'></td></tr>";
                                            foreach (var pnumber in partnumber)
                                            {

                                                //if (pnumber.Cost == "0" && pnumber.Resale == "0")
                                                //{
                                                if (pnumber.assemblyName == aname)
                                                {

                                                    tablebody += "<tr>";
                                                    tablebody += "<td><b>" + aname + "</b></td>";
                                                    tablebody += "<td align='center'>" + pnumber.Partnumber + "</td>";
                                                    tablebody += "<td align='center'>" + pnumber.Category + "</td>";
                                                    if (pnumber.Cost == "0" && pnumber.Resale == "0")
                                                    {
                                                        tablebody += "<td align='center'style='color: #ec0909'>" + pnumber.Cost + "</td>";
                                                        tablebody += "<td align='center'style='color: #ec0909'>" + pnumber.Resale + "</td>";
                                                    }
                                                    else
                                                    {
                                                        tablebody += "<td align='center'>" + pnumber.Cost + "</td>";
                                                        tablebody += "<td align='center'>" + pnumber.Resale + "</td>";
                                                    }
                                                    tablebody += "</tr>";
                                                }
                                                //}
                                            }

                                        }
                                    }
                                }
                                tablebody += "</tbody></table>";
                                _email.BodyContent = _email.BodyContent.Replace("##partslist##", tablebody);
                                _email.EmailTo = client.Email;
                                EmailService em = new EmailService();
                                em.SendEmail(_email);
                            }

                            //To remove the duplicate assembly Ids
                            AssemblyNames = AssemblyNames.Distinct().ToList();
                            foreach (string assemly in AssemblyNames)
                            {
                                assembliesList.Append(assemly);
                                assembliesList.Append(',');
                            }
                            
                            clientList.Append(item.ID.ToString());
                            clientList.Append(',');
                        }
                    }
                }

                //To remove last comma in the list of processed assemblies
                if (assembliesList.Length > 0)
                {
                    assembliesList.Remove((assembliesList.Length - 1), 1);
                }

                string connStr = ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("EE_RecalcDistributorExportClientAssembly", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@AssemblyList", SqlDbType.NVarChar).Value = assembliesList.ToString();
                        cmd.Parameters.Add("@ClientIDList", SqlDbType.NVarChar).Value = clientList.ToString();
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = "Admin Export";

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return status;
            }
            catch (Exception ex)
            {
                return status;
            }
        }

        public ServiceResult<bool> AssemblieNameIsExist(string Assemblies_Name)
        {

            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    bool result = DbOBJ.Assemblies_Master.Any(m => m.Assemblies_Name == Assemblies_Name);

                    if (result == true)
                    {
                        response.ResultCode = 1;
                        response.Message = "“Assemblies Name” does Exist!";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = 0;
                        response.Message = "“Assembly Name” does not exist!";
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
        /// <summary>
        /// NationwideAssemblieNameIsExist
        /// </summary>
        /// <param name="Assemblies_Name"></param>
        /// <returns></returns>
        public ServiceResult<bool> NationwideAssemblieNameIsExist(int AssebliesId)
        {

            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    bool result = DbOBJ.Super_Admin_Assemblies_Master.Any(m => m.Assemblies_Id == AssebliesId);

                    if (result == true)
                    {
                        response.ResultCode = 1;
                        response.Message = "“Assemblies Name” does Exist!";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = 0;
                        response.Message = "“Assembly Name” does not exist!";
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
        /// <summary>
        /// DistributorAssemblieNameIsExist
        /// </summary>
        /// <param name="Assemblies_Name"></param>
        /// <param name="Did"></param>
        /// <returns></returns>
        public ServiceResult<bool> DistributorAssemblieNameIsExist(string Assemblies_Name, int Did)
        {

            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    bool result = DbOBJ.Distributor_Assemblies_Master.Any(m => m.Assemblies_Name == Assemblies_Name && m.Distributor_Id == Did);

                    if (result == true)
                    {
                        response.ResultCode = 1;
                        response.Message = "“Assemblies Name” does Exist!";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = 0;
                        response.Message = "“Assembly Name” does not exist!";
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
        public ServiceResult<int> DeleteAssembliesDetails(string AssembliesName, int Client_ID, string username)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            List<AssembliesParts_DetailsInfoList> ASMPartsListOBJ = new List<AssembliesParts_DetailsInfoList>();
            List<AssembliesLabourDetailsList> ASMLabourListOBJ = new List<AssembliesLabourDetailsList>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    AssembliesName = AssembliesName.Trim();
                    Assemblies_Master ASMtbl = new Assemblies_Master();
                    var partsList = DbOBJ.Assemblies_Parts.Where(a => a.Client_ID == Client_ID && a.Assemblies_Name == AssembliesName).ToList();
                    if (partsList.Any())
                    {
                        foreach (var item in partsList)
                        {
                            DbOBJ.Assemblies_Parts.Remove(item);
                            DbOBJ.SaveChanges();
                        }
                    }
                    ASMtbl = (from m in DbOBJ.Assemblies_Master where m.Client_Id == Client_ID && m.Assemblies_Name == AssembliesName && m.Isactive == true select m).FirstOrDefault();
                    // ASMtbl.Isactive = false;
                    DbOBJ.Assemblies_Master.Remove(ASMtbl);
                    int Asresultcount = DbOBJ.SaveChanges();
                    if (Asresultcount > 0)
                    {
                        //using (ElectricEaseEntitiesContext DbOBJ1 = new ElectricEaseEntitiesContext())
                        //{
                        //    Assemblies_Parts ASPartsTabl = new Assemblies_Parts();
                        //    var partsListData = (from ASMtabl in DbOBJ1.Assemblies_Master
                        //                         join ASparts in DbOBJ1.Assemblies_Parts
                        //                         on ASMtabl.Assemblies_Name equals ASparts.Assemblies_Name
                        //                         where ASMtabl.Client_Id == Client_ID
                        //                         && ASMtabl.Assemblies_Name == AssembliesName
                        //                         && ASMtabl.Client_Id == ASparts.Client_ID
                        //                         && ASparts.IsActive == true
                        //                         select ASparts).ToList();
                        //    ASMtbl = (from m in DbOBJ1.Assemblies_Master where m.Client_Id == Client_ID && m.Assemblies_Name == AssembliesName select m).FirstOrDefault();
                        //    if (ASMtbl.Isactive == false)
                        //    {
                        //        foreach (var item in partsListData)
                        //        {
                        //            item.IsActive = false;
                        //            ASMPartsResultCount = DbOBJ1.SaveChanges();
                        //        }

                        //        if (ASMPartsResultCount > 0)
                        //        {
                        //            logs.WriteLog("Successfully Deleted the Assembly - " + AssembliesName, Client_ID, username);
                        //            response.ResultCode = 1;
                        //            response.Message = "Assemblies Details Deleted Successfully!";
                        //            return response;

                        //        }
                        //        else
                        //        {
                        //            response.ResultCode = -1;
                        //            response.Message = "Error occured while deleteting Asseblies  Parts Details! ";
                        //            return response;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        response.ResultCode = -1;
                        //        response.Message = "Error occured while deleteting Asseblies information! ";
                        //        return response;

                        //    }
                        //}
                        logs.WriteLog("Successfully Deleted the Assembly - " + AssembliesName, Client_ID, username);
                        response.ResultCode = 1;
                        response.Message = "“Assemblies Details” deleted successfully.";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "An error occurred while deleting “Assemblies information”!";
                        return response;
                    }
                }


            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While deleting the Assembly - " + AssembliesName + " [Error Msg - " + ex.Message + " ]", Client_ID, username);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }
        /// <summary>
        /// DeleteNationaWideAssembliesDetails
        /// </summary>
        /// <param name="AssembliesName"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public ServiceResult<int> DeleteNationaWideAssembliesDetails(string AssembliesName, int Asseblies_Id, string username)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            List<AssembliesParts_DetailsInfoList> ASMPartsListOBJ = new List<AssembliesParts_DetailsInfoList>();
            List<AssembliesLabourDetailsList> ASMLabourListOBJ = new List<AssembliesLabourDetailsList>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    AssembliesName = AssembliesName.Trim();
                    Super_Admin_Assemblies_Master ASMtbl = new Super_Admin_Assemblies_Master();
                    var partsList = DbOBJ.Super_Admin_Assemblies_Parts.Where(a => a.Assemblies_Id == Asseblies_Id && a.Assemblies_Name == AssembliesName).ToList();
                    if (partsList.Any())
                    {
                        foreach (var item in partsList)
                        {
                            DbOBJ.Super_Admin_Assemblies_Parts.Remove(item);
                            DbOBJ.SaveChanges();
                        }
                    }
                    ASMtbl = (from m in DbOBJ.Super_Admin_Assemblies_Master where m.Assemblies_Id == Asseblies_Id && m.Assemblies_Name == AssembliesName && m.Isactive == true select m).FirstOrDefault();
                    // ASMtbl.Isactive = false;
                    DbOBJ.Super_Admin_Assemblies_Master.Remove(ASMtbl);
                    int Asresultcount = DbOBJ.SaveChanges();
                    if (Asresultcount > 0)
                    {
                        //using (ElectricEaseEntitiesContext DbOBJ1 = new ElectricEaseEntitiesContext())
                        //{
                        //    Super_Admin_Assemblies_Parts ASPartsTabl = new Super_Admin_Assemblies_Parts();
                        //    var partsListData = (from ASparts in DbOBJ1.Super_Admin_Assemblies_Parts
                        //                         where
                        //                         ASparts.Assemblies_Name == AssembliesName
                        //                         && ASparts.Assemblies_Id == ASparts.Assemblies_Id
                        //                         && ASparts.IsActive == true
                        //                         select ASparts).ToList();
                        //    ASMtbl = (from m in DbOBJ1.Super_Admin_Assemblies_Master where m.Assemblies_Id == Asseblies_Id && m.Assemblies_Name == AssembliesName select m).FirstOrDefault();
                        //    if (ASMtbl.Isactive == false)
                        //    {
                        //        foreach (var item in partsListData)
                        //        {
                        //            item.IsActive = false;
                        //            ASMPartsResultCount = DbOBJ1.SaveChanges();
                        //        }

                        //        if (ASMPartsResultCount > 0)
                        //        {
                        //            logs.WriteLog("Successfully Deleted the Assembly - " + AssembliesName, Asseblies_Id, username);
                        //            response.ResultCode = 1;
                        //            response.Message = "Assemblies Details Deleted Successfully!";
                        //            return response;

                        //        }
                        //        else
                        //        {
                        //            response.ResultCode = -1;
                        //            response.Message = "Error occured while deleteting Asseblies  Parts Details! ";
                        //            return response;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        response.ResultCode = -1;
                        //        response.Message = "Error occured while deleteting Asseblies information! ";
                        //        return response;

                        //    }
                        //}
                        logs.WriteLog("Successfully Deleted the Assembly - " + AssembliesName, Asseblies_Id, username);
                        response.ResultCode = 1;
                        response.Message = "“Assemblies Details” deleted successfully.";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "An error occurred while deleting “Assemblies information”!";
                        return response;
                    }
                }


            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While deleting the Assembly - " + AssembliesName + " [Error Msg - " + ex.Message + " ]", Asseblies_Id, username);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }
        /// <summary>
        /// DeleteDistributorAssembliesDetails
        /// </summary>
        /// <param name="AssembliesName"></param>
        /// <param name="Did"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public ServiceResult<int> DeleteDistributorAssembliesDetails(string AssembliesName, int Did, string username)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            List<AssembliesParts_DetailsInfoList> ASMPartsListOBJ = new List<AssembliesParts_DetailsInfoList>();
            List<AssembliesLabourDetailsList> ASMLabourListOBJ = new List<AssembliesLabourDetailsList>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    AssembliesName = AssembliesName.Trim();
                    Distributor_Assemblies_Master ASMtbl = new Distributor_Assemblies_Master();
                    var parts = DbOBJ.Distributor_Assemblies_Parts.Where(m => m.Distributor_ID == Did && m.Assemblies_Name == AssembliesName).ToList();
                    if (parts.Any())
                    {
                        foreach (var item in parts)
                        {
                            DbOBJ.Distributor_Assemblies_Parts.Remove(item);
                            DbOBJ.SaveChanges();
                        }
                    }
                    ASMtbl = (from m in DbOBJ.Distributor_Assemblies_Master where m.Distributor_Id == Did && m.Assemblies_Name == AssembliesName && m.Isactive == true select m).FirstOrDefault();
                    // ASMtbl.Isactive = false;
                    DbOBJ.Distributor_Assemblies_Master.Remove(ASMtbl);
                    int Asresultcount = DbOBJ.SaveChanges();
                    if (Asresultcount > 0)
                    {
                        //using (ElectricEaseEntitiesContext DbOBJ1 = new ElectricEaseEntitiesContext())
                        //{
                        //    Distributor_Assemblies_Parts ASPartsTabl = new Distributor_Assemblies_Parts();
                        //    var partsListData = (from ASMtabl in DbOBJ1.Distributor_Assemblies_Parts
                        //                         where
                        //                         ASMtabl.Assemblies_Name == AssembliesName
                        //                         && ASMtabl.Distributor_ID == Did
                        //                         && ASMtabl.IsActive == true
                        //                         select ASMtabl).ToList();
                        //    ASMtbl = (from m in DbOBJ1.Distributor_Assemblies_Master where m.Distributor_Id == Did && m.Assemblies_Name == AssembliesName select m).FirstOrDefault();
                        //    if (ASMtbl.Isactive == false)
                        //    {
                        //        foreach (var item in partsListData)
                        //        {
                        //            item.IsActive = false;
                        //            ASMPartsResultCount = DbOBJ1.SaveChanges();
                        //        }

                        //        if (ASMPartsResultCount > 0)
                        //        {
                        //            logs.WriteLog("Successfully Deleted the Assembly - " + AssembliesName, Did, username);
                        //            response.ResultCode = 1;
                        //            response.Message = "Assemblies Details Deleted Successfully!";
                        //            return response;

                        //        }
                        //        else
                        //        {
                        //            response.ResultCode = -1;
                        //            response.Message = "Error occured while deleteting Asseblies  Parts Details! ";
                        //            return response;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        response.ResultCode = -1;
                        //        response.Message = "Error occured while deleteting Asseblies information! ";
                        //        return response;

                        //    }
                        //}
                        logs.WriteLog("Successfully Deleted the Assembly - " + AssembliesName, Did, username);
                        response.ResultCode = 1;
                        response.Message = "“Assemblies Details” deleted successfully.";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "An error occurred while deleting “Assemblies information”!";
                        return response;
                    }
                }


            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While deleting the Assembly - " + AssembliesName + " [Error Msg - " + ex.Message + " ]", Did, username);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResultList<Assembly_MasterInfo> GetNewAssembliesListInJobs(int ClientID)
        {
            ServiceResultList<Assembly_MasterInfo> response = new ServiceResultList<Assembly_MasterInfo>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Assembly_MasterInfo> result = new List<Assembly_MasterInfo>();
                    result = db.Database.SqlQuery<Assembly_MasterInfo>("exec EE_SelectedAssembliesPartsForJobs @ClientID", new SqlParameter("ClientID", ClientID)).ToList();

                    if (result != null)
                    {
                        response.ListData = result;
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
        public ServiceResultList<Assembly_MasterInfo> GetJobAssembliesDetailsList(int ClientID, string Assemblies_Name)
        {
            ServiceResultList<Assembly_MasterInfo> response = new ServiceResultList<Assembly_MasterInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Assembly_MasterInfo> result = new List<Assembly_MasterInfo>();
                    result = db.Database.SqlQuery<Assembly_MasterInfo>("exec EE_GetJobAssembliesDetailList @ClientID, @Assemblies_Name", new SqlParameter("ClientID", ClientID), new SqlParameter("Assemblies_Name", Assemblies_Name)).ToList();
                    if (result != null)
                    {
                        response.ListData = result;
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
        public ServiceResultList<Assembly_MasterInfo> GetJobAssembliesLaborDetailsList(int ClientID, string Assemblies_Name)
        {
            ServiceResultList<Assembly_MasterInfo> response = new ServiceResultList<Assembly_MasterInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Assembly_MasterInfo> result = new List<Assembly_MasterInfo>();
                    result = db.Database.SqlQuery<Assembly_MasterInfo>("exec EE_GetJobAssembliesLaborDetailList @ClientID, @Assemblies_Name", new SqlParameter("ClientID", ClientID), new SqlParameter("Assemblies_Name", Assemblies_Name)).ToList();
                    if (result != null)
                    {
                        response.ListData = result;
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

        public ServiceResultList<Assembly_DescriptionList> GetDescriptionList(int ClientID)
        {
            ServiceResultList<Assembly_DescriptionList> response = new ServiceResultList<Assembly_DescriptionList>();
            List<Assembly_DescriptionList> result = new List<Assembly_DescriptionList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    result = db.Assemblies_Master.Where(m => m.Client_Id == ClientID && m.Isactive == true).Select(m => new Assembly_DescriptionList()
                    {
                        Assemblies_Description = m.Assemblies_Description
                    }).Distinct().ToList();
                    if (result.Count > 0)
                    {
                        response.ListData = result;
                        response.ResultCode = 1;
                        response.Message = "Success";
                        return response;
                    }
                    else
                    {
                        response.ListData = result;
                        response.ResultCode = -1;
                        response.Message = "Data not exist!";
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
        /// <summary>
        /// GetNationDescriptionList
        /// </summary>
        /// <returns></returns>
        public ServiceResultList<Assembly_DescriptionList> GetNationDescriptionList()
        {
            ServiceResultList<Assembly_DescriptionList> response = new ServiceResultList<Assembly_DescriptionList>();
            List<Assembly_DescriptionList> result = new List<Assembly_DescriptionList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    result = db.Super_Admin_Assemblies_Master.Where(m => m.Isactive == true).Select(m => new Assembly_DescriptionList()
                    {
                        Assemblies_Description = m.Assemblies_Description
                    }).Distinct().ToList();
                    if (result.Count > 0)
                    {
                        response.ListData = result;
                        response.ResultCode = 1;
                        response.Message = "Success";
                        return response;
                    }
                    else
                    {
                        response.ListData = result;
                        response.ResultCode = -1;
                        response.Message = "Data not exist!";
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

        /// <summary>
        /// GetDistributorDescriptionList
        /// </summary>
        /// <returns></returns>
        public ServiceResultList<Assembly_DescriptionList> GetDistributorDescriptionList()
        {
            ServiceResultList<Assembly_DescriptionList> response = new ServiceResultList<Assembly_DescriptionList>();
            List<Assembly_DescriptionList> result = new List<Assembly_DescriptionList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    result = db.Distributor_Assemblies_Master.Where(m => m.Isactive == true).Select(m => new Assembly_DescriptionList()
                    {
                        Assemblies_Description = m.Assemblies_Description
                    }).Distinct().ToList();
                    if (result.Count > 0)
                    {
                        response.ListData = result;
                        response.ResultCode = 1;
                        response.Message = "Success";
                        return response;
                    }
                    else
                    {
                        response.ListData = result;
                        response.ResultCode = -1;
                        response.Message = "Data not exist!";
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

        /// <summary>
        /// GetDistributorCategoryList
        /// </summary>
        /// <returns></returns>
        public List<AssembliesCategoryList> GetDistributorCategoryList()
        {

            List<AssembliesCategoryList> result = new List<AssembliesCategoryList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    result = db.Distributor_Assemblies_Master.Where(m => m.Isactive == true && m.Assemblies_Category != null && m.Assemblies_Category != "").Select(m => new AssembliesCategoryList()
                    {
                        Assemblies_Category = m.Assemblies_Category
                    }).Distinct().ToList();

                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public string ValidateLaborUnit(selectedAssembilesIds model)
        {
            string status, assemblyIdsWithComma = string.Empty, assemblyNamesWithComma = string.Empty, clientIdsWithComma = string.Empty, distributorIdsWithComma = string.Empty;

            try
            {
                if (model.AssemblyId != null)
                {
                    foreach (AssemblyIds assemblyID in model.AssemblyId)
                    {
                        assemblyIdsWithComma += assemblyID.ID + ",";

                        using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                        {
                            var assembly = db.Super_Admin_Assemblies_Master.Where(m => m.Assemblies_Id == assemblyID.ID && m.Isactive == true).FirstOrDefault();
                            assemblyNamesWithComma += assembly.Assemblies_Name + ",";
                        }
                    }
                }

                if (model.ClienId != null)
                {
                    foreach (ClienIds clientID in model.ClienId)
                    {
                        clientIdsWithComma += clientID.ID + ",";
                    }
                }

                if (model.DisbutorId != null)
                {
                    foreach (DisbutorIds distributorID in model.DisbutorId)
                    {
                        distributorIdsWithComma += distributorID.ID + ",";
                    }
                }

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 0;

                    //To update the labor unit on Client Assembly Master
                    db.Database.ExecuteSqlCommand("exec MIGRATE_LABORUNIT_WITHASSEMBLYID @ASSEMBLY, @CLIENT, @DISTRIBUTOR", 
                        new SqlParameter("ASSEMBLY", assemblyIdsWithComma), 
                        new SqlParameter("CLIENT", clientIdsWithComma),
                        new SqlParameter("DISTRIBUTOR", distributorIdsWithComma));

                    //To recalculate on Client Assembly Master
                    db.Database.ExecuteSqlCommand("exec EE_RecalcNationalwideExportClientAssembly @AssemblyList, @ClientIDList, @UpdatedBy",
                        new SqlParameter("AssemblyList", assemblyIdsWithComma),
                        new SqlParameter("ClientIDList", clientIdsWithComma),
                        new SqlParameter("UpdatedBy", "Admin"));

                    //To recalculate on Distributor Assembly Master
                    db.Database.ExecuteSqlCommand("exec EE_RecalcNationalwideExportDistributorAssembly @AssemblyList, @DistributorIDList, @UpdatedBy",
                        new SqlParameter("AssemblyList", assemblyIdsWithComma),
                        new SqlParameter("DistributorIDList", distributorIdsWithComma),
                        new SqlParameter("UpdatedBy", "Admin"));

                    status = "Success";
                }
            }
            catch (Exception ex)
            {
                status = ex.Message.ToString();
            }
            return status;
        }

        public string ValidateDistributorClientsLaborUnit(DistributorAssemblies model)
        {
            string status, assemblyIdsWithComma = string.Empty, clientIdsWithComma = string.Empty;

            try
            {
                if (model.selectedAssembiles != null)
                {
                    foreach (AssemblyNames assemblyID in model.selectedAssembiles)
                    {
                        assemblyIdsWithComma += assemblyID.Name.Replace("a_a"," ") + ",";//Replacing a_a should remove after primary key implementation
                    }
                }

                if (model.ClienId != null)
                {
                    foreach (ClienIds clientID in model.ClienId)
                    {
                        clientIdsWithComma += clientID.ID + ",";
                    }
                }

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    db.Database.ExecuteSqlCommand("exec MIGRATE_LABORUNIT_WITHASSEMBLYID_FORDISTIRBUTOR @ASSEMBLY, @CLIENT",
                        new SqlParameter("ASSEMBLY", assemblyIdsWithComma),
                        new SqlParameter("CLIENT", clientIdsWithComma));

                    //To recalculate on Client Assembly Master
                    db.Database.ExecuteSqlCommand("exec EE_RecalcDistributorExportClientAssembly @AssemblyList, @ClientIDList, @UpdatedBy",
                        new SqlParameter("AssemblyList", assemblyIdsWithComma),
                        new SqlParameter("ClientIDList", clientIdsWithComma),
                        new SqlParameter("UpdatedBy", "Admin"));

                    status = "Success";
                }
            }
            catch (Exception ex)
            {
                status = ex.Message.ToString();
            }
            return status;
        }
    }
}
