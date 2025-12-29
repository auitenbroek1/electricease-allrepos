using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Data;
using ElectricEase.Models;
using ElectricEase.Data.DataBase;
using System.Data.SqlClient;

namespace ElectricEase.Data.LabourMaster
{
    public class LabourMasterData
    {
        LogsMaster logs = new LogsMaster();
        public List<Labor_DetailsInfoList> GetLabourList()
        {
            List<Labor_DetailsInfoList> response = new List<Labor_DetailsInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Labor_DetailsInfoList> result = new List<Labor_DetailsInfoList>();
                    result = db.Database.SqlQuery<Labor_DetailsInfoList>("exec EE_GetLabourDetailList").ToList();

                    if (result != null)
                    {
                        //response.ListData = result;
                        //response.Message = "Success";
                        //response.ResultCode = 1;
                        response = result;
                        return response;
                    }
                    else
                    {
                        //response.Message = "No Records Found";
                        //response.ResultCode = -1;
                        return response;

                    }
                }
            }
            catch (Exception ex)
            {
                //response.Message = ex.Message;
                //response.ResultCode = -1;
                return null;
            }
        }

        public ServiceResultList<Labor_DetailsInfoList> GetMyLabourList(int ClientID)
        {

            ServiceResultList<Labor_DetailsInfoList> response = new ServiceResultList<Labor_DetailsInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Labor_DetailsInfoList> result = new List<Labor_DetailsInfoList>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    result = db.Database.SqlQuery<Labor_DetailsInfoList>("exec EE_GetMyLabourDetailList @ClientID", para).ToList();
                    if (result != null)
                    {
                        response.ListData = result;
                        response.Message = "Success";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {
                        response.Message = "No “Records” found!";
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

        public List<Labor_DetailsInfoList> GetMyLabourListbyId(int ClientID)
        {

            List<Labor_DetailsInfoList> response = new List<Labor_DetailsInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Labor_DetailsInfoList> result = new List<Labor_DetailsInfoList>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    result = db.Database.SqlQuery<Labor_DetailsInfoList>("exec EE_GetMyLabourDetailList @ClientID", para).ToList();
                    if (result != null)
                    {

                        return result;
                    }
                    else
                    {

                        return result;

                    }
                }
            }
            catch (Exception ex)
            {
                return response;
            }
        }
        //Here To add Labour Details to LabourDetails Table from Database
        public ServiceResult<int> AddLabourDetails(Labor_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {

                    Labor_Details LDTblObj = new Labor_Details();


                    //validate User ID 
                    Labor_Details UserIsExit = db.Labor_Details.Where(x => x.Laborer_Name == Model.Laborer_Name && x.Client_Id == Model.Client_ID && x.IsActive == true).FirstOrDefault();
                    if (UserIsExit != null)
                    {
                        response.Message = "“Labor Name” is already registered.";
                        return response;
                    }

                    if (Model.Labor_Category == "1" && Model.otherLabor_Category != null)
                    {
                        Model.Labor_Category = Model.otherLabor_Category;
                    }

                    LDTblObj.Client_Id = Model.Client_ID;
                    LDTblObj.Laborer_Name = Model.Laborer_Name.Trim();
                    LDTblObj.Labor_Category = Model.Labor_Category;
                    LDTblObj.Description = Model.Description;
                    LDTblObj.Client_Description = Model.Client_Description;
                    LDTblObj.Rate = Model.Rate;
                    LDTblObj.Cost = Model.Cost;
                    LDTblObj.Resale_Cost = Model.Resale_Cost;
                    LDTblObj.Burden = Model.burden;
                    LDTblObj.Created_By = Model.Created_By;
                    LDTblObj.Created_Date = DateTime.Now;
                    LDTblObj.Updated_By = Model.Created_By;
                    LDTblObj.Updated_Date = DateTime.Now;
                    LDTblObj.IsActive = true;
                    LDTblObj.Photo = Model.Photo;
                    LDTblObj.Gender = Model.Gender;

                    db.Labor_Details.Add(LDTblObj);
                    int resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        response.ResultCode = 1;
                        response.Message = "“New Labor” has been added successfully.";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "Error while “Labor Registering”!";
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

        //check is existing Labor
        public ServiceResult<bool> CheckIsExistingLabor(string Laborname, int ClientID)
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    //validate User ID 
                    Labor_Details UserIsExit = db.Labor_Details.Where(x => x.Client_Id == ClientID && x.Laborer_Name == Laborname).FirstOrDefault();
                    if (UserIsExit != null)
                    {
                        response.Data = true;
                        response.ResultCode = 1;
                        response.Message = "“Labor Name” is already registered.";
                        return response;
                    }
                    else
                    {
                        response.Data = false;
                        response.ResultCode = 0;
                        response.Message = "“Labor Name” is valid.";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }
        public ServiceResult<int> AddLabourDetail(Labor_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    bool isupdate = true;

                    //validate User ID 
                    Labor_Details LDTblObj = db.Labor_Details.Where(x => x.Laborer_Name == Model.Laborer_Name && x.Client_Id == Model.Client_ID).FirstOrDefault();
                    if (LDTblObj == null)
                    {
                        LDTblObj = new Labor_Details();
                        Labor_Details UserIsExit = db.Labor_Details.Where(x => x.Laborer_Name == Model.Laborer_Name && x.Client_Id == Model.Client_ID && x.IsActive == true).FirstOrDefault();
                        if (UserIsExit != null)
                        {
                            response.Message = "“Labor Name” is already registered.";
                            return response;
                        }
                        isupdate = false;
                    }
                    else
                    {
                        isupdate = true;
                    }
                    if (Model.otherLabor_Category != null && Model.Labor_Category == null)
                    {
                        Model.Labor_Category = Model.otherLabor_Category;
                    }
                    if (Model.Labor_Category == "other" && Model.otherLabor_Category != null)
                    {
                        Model.Labor_Category = Model.otherLabor_Category;
                    }
                    LDTblObj.Laborer_Name = Model.Laborer_Name.Trim();
                    LDTblObj.Labor_Category = Model.Labor_Category;
                    LDTblObj.Description = Model.Description;
                    LDTblObj.Client_Description = Model.Client_Description;
                    LDTblObj.Cost = Model.Cost;
                    LDTblObj.Rate = Model.Rate;
                    LDTblObj.Resale_Cost = Model.Resale_Cost;
                    LDTblObj.Burden = Model.burden;
                    LDTblObj.IsActive = true;
                    LDTblObj.Gender = Model.Gender;
                    LDTblObj.Client_Id = Model.Client_ID;
                    if (isupdate == false)
                    {
                        LDTblObj.Created_By = Model.Created_By;
                        LDTblObj.Created_Date = DateTime.Now;
                        LDTblObj.Updated_By = Model.Created_By;
                        LDTblObj.Updated_Date = DateTime.Now;
                        LDTblObj.Photo = Model.Photo;
                        db.Labor_Details.Add(LDTblObj);
                       
                    }
                    else
                    {
                        LDTblObj.Updated_By = Model.Created_By;
                        LDTblObj.Updated_Date = DateTime.Now;

                        if (Model.Photo != null)
                        {
                            LDTblObj.Photo = Model.Photo;
                        }
                    }

                    int resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        response.ResultCode = 1;
                        if (isupdate == false)
                        {
                            response.Message = "“Labor details” are added successfully.";
                            logs.WriteLog("Successfully Added the New Labor - " + Model.Laborer_Name, Model.Client_ID, Model.Created_By);

                        }
                        else
                        {
                            response.Message = "“Labor details” are updated successfully.";
                            logs.WriteLog("Successfully Updated the  Labor - " + Model.Laborer_Name, Model.Client_ID, Model.Updated_By);
                        }

                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "Error while “Labor Registering”!";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                logs.WriteLog("[Exception] While adding Labor - " + Model.Laborer_Name + "[Error Msg- " + ex.Message + " ]", Model.Client_ID, Model.Created_By);
                return response;
            }
        }
        //Here To Get Labour Details To Edit Labour Details
        public ServiceResult<EditLabor_DetailsInfo> GetLabourDetails(string LabourName, int ClientID)
        {
            ServiceResult<EditLabor_DetailsInfo> response = new ServiceResult<EditLabor_DetailsInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Labor_Details LDTblObj = new Labor_Details();
                    LDTblObj = (from m in db.Labor_Details where m.Laborer_Name == LabourName && m.IsActive == true && m.Client_Id == ClientID select m).FirstOrDefault();
                    if (LDTblObj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“Labor” doesn't exist!";
                        return response;
                    }
                    response.Data = new EditLabor_DetailsInfo()
                    {

                        Client_ID = LDTblObj.Client_Id,
                        Laborer_Name = LDTblObj.Laborer_Name,
                        Gender = LDTblObj.Gender == true ? true : false,
                        Labor_Category = LDTblObj.Labor_Category,
                        Description = LDTblObj.Description,
                        Client_Description = LDTblObj.Client_Description,
                        Cost = LDTblObj.Cost,
                        Rate = LDTblObj.Rate ?? 0,
                        Resale_Cost = LDTblObj.Resale_Cost ?? 0,
                        burden = LDTblObj.Burden ?? 0,
                        Created_By = LDTblObj.Created_By,
                        Created_Date = LDTblObj.Created_Date,
                        Updated_By = LDTblObj.Updated_By,
                        Updated_Date = LDTblObj.Updated_Date,
                        Photo = LDTblObj.Photo


                    };

                    response.ResultCode = 1;
                    response.Message = "Success";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;

            }
        }

        public ServiceResult<Labor_DetailsInfo> GetLabourDetail(string LabourName, int ClientID)
        {
            ServiceResult<Labor_DetailsInfo> response = new ServiceResult<Labor_DetailsInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Labor_Details LDTblObj = new Labor_Details();
                    LDTblObj = (from m in db.Labor_Details where m.Laborer_Name == LabourName && m.IsActive == true && m.Client_Id == ClientID select m).FirstOrDefault();
                    if (LDTblObj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“Labor” doesn't exist!";
                        return response;
                    }
                    response.Data = new Labor_DetailsInfo()
                    {

                        Client_ID = LDTblObj.Client_Id,
                        Laborer_Name = LDTblObj.Laborer_Name,
                        Gender = LDTblObj.Gender == true ? true : false,
                        Labor_Category = LDTblObj.Labor_Category,
                        Description = LDTblObj.Description,
                        Client_Description = LDTblObj.Client_Description,
                        Rate = LDTblObj.Rate ?? 0,
                        Cost = LDTblObj.Cost,
                        Resale_Cost = LDTblObj.Resale_Cost ?? 0,
                        burden = LDTblObj.Burden ?? 0,
                        Created_By = LDTblObj.Created_By,
                        Created_Date = LDTblObj.Created_Date,
                        Updated_By = LDTblObj.Updated_By,
                        Updated_Date = LDTblObj.Updated_Date,
                        Photo = LDTblObj.Photo


                    };

                    response.ResultCode = 1;
                    response.Message = "Success";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;

            }
        }

        public EditLabor_DetailsInfo GetLabourDetailsbyname(string LabourName, int ClientID)
        {
            EditLabor_DetailsInfo response = new EditLabor_DetailsInfo();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Labor_Details LDTblObj = new Labor_Details();
                    LDTblObj = (from m in db.Labor_Details where m.Laborer_Name == LabourName && m.IsActive == true && m.Client_Id == ClientID select m).FirstOrDefault();
                    if (LDTblObj == null)
                    {

                        return response;
                    }
                    response = new EditLabor_DetailsInfo()
                    {

                        Client_ID = LDTblObj.Client_Id,
                        Laborer_Name = LDTblObj.Laborer_Name,
                        Gender = LDTblObj.Gender == true ? true : false,
                        Labor_Category = LDTblObj.Labor_Category,
                        Description = LDTblObj.Description,
                        Client_Description = LDTblObj.Client_Description,
                        Cost = LDTblObj.Cost,
                        Rate = LDTblObj.Rate ?? 0,
                        Resale_Cost = LDTblObj.Resale_Cost ?? 0,
                        burden = LDTblObj.Burden ?? 0,
                        Created_By = LDTblObj.Created_By,
                        Created_Date = LDTblObj.Created_Date,
                        Updated_By = LDTblObj.Updated_By,
                        Updated_Date = LDTblObj.Updated_Date,
                        Photo = LDTblObj.Photo


                    };


                    return response;
                }
            }
            catch (Exception ex)
            {

                return null;

            }
        }
        //Here To Update Labour details 
        public ServiceResult<int> UpdateLabourDetails(EditLabor_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Labor_Details LDTblObj = new Labor_Details();


                    LDTblObj = (from m in db.Labor_Details where m.Laborer_Name == Model.Laborer_Name && m.Client_Id == Model.Client_ID && m.IsActive == true select m).FirstOrDefault();
                    //PDTblObj.Part_Number = Model.Part_Number;

                    if (Model.Labor_Category == null && Model.otherLabor_Category != null)
                    {
                        Model.Labor_Category = Model.otherLabor_Category;
                    }
                    if (Model.Labor_Category == "1" && Model.otherLabor_Category != null)
                    {
                        Model.Labor_Category = Model.otherLabor_Category;
                    }
                    if (Model.Labor_Category != null)
                    {
                        LDTblObj.Labor_Category = Model.Labor_Category;
                    }
                    if (Model.Photo != null)
                    {
                        LDTblObj.Photo = Model.Photo;
                    }

                    LDTblObj.Burden = Model.burden;
                    LDTblObj.Gender = Model.Gender;
                    LDTblObj.Description = Model.Description;
                    LDTblObj.Client_Description = Model.Client_Description;
                    LDTblObj.Cost = Model.Cost;
                    LDTblObj.Rate = Model.Rate;
                    LDTblObj.Resale_Cost = Model.Resale_Cost;
                    LDTblObj.Updated_By = Model.Updated_By;
                    LDTblObj.Updated_Date = DateTime.Now; ;
                    LDTblObj.Updated_By = Model.Updated_By;


                    int resultcount = db.SaveChanges();

                    if (resultcount > 0)
                    {
                        response.ResultCode = resultcount;
                        response.ResultCode = 1;
                        response.Message = "Your “Changes” have been modified successfully.";
                        return response;
                    }
                    else
                    {
                        response.Data = resultcount;
                        response.ResultCode = -1;
                        response.Message = "“No” changes found in your profile!";
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

        //Here To Delete labour details
        public ServiceResult<int> DeleteLabourDetails(string LabourName, int ClientID)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            int resultcount;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Labor_Details LDtableObj = new Labor_Details();
                    LDtableObj = db.Labor_Details.Where(m => m.Laborer_Name == LabourName && m.IsActive == true && m.Client_Id == ClientID).FirstOrDefault();
                    LDtableObj.IsActive = false;
                    resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        response.ResultCode = resultcount;
                        response.Data = resultcount;
                        response.Message = "“Deleted” successfully.";
                        logs.WriteLog("Successfully Deleted the Labor - " + LabourName, ClientID, LabourName);
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Data = resultcount;
                        response.Message = "Delete \"Failed\"!";

                        return response;
                    }

                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                logs.WriteLog("[Exception] While Deleting labor - " + LabourName + "[Error Msg- " + ex.Message + " ]", ClientID, LabourName);
                return response;

            }
        }

        public ServiceResultList<Labor_DetailsInfoList> GetMyJobLaborerList(int ClientID, string Laborer_Name)
        {
            ServiceResultList<Labor_DetailsInfoList> response = new ServiceResultList<Labor_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Labor_DetailsInfoList> result = new List<Labor_DetailsInfoList>();
                    result = db.Database.SqlQuery<Labor_DetailsInfoList>("exec EE_GetMylabourDetails @ClientID, @Laborer_Name", new SqlParameter("ClientID", ClientID), new SqlParameter("Laborer_Name", Laborer_Name)).ToList();

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
    }
}
