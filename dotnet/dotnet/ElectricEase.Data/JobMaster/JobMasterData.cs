using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Data;
using ElectricEase.Models;
using ElectricEase.Data.DataBase;
using System.Data.SqlClient;
using ElectricEase.Data.AssembliesMaster;
using System.Globalization;
using System.Diagnostics;

namespace ElectricEase.Data.JobMaster
{
    public class JobMasterData
    {
        int PartsresultCount;
        int LabourresultCount;
        int LegalResulutcount;
        LogsMaster logs = new LogsMaster();
        public ServiceResultList<Job_MasterInfo> GetAssembliesDetailsList()
        {
            ServiceResultList<Job_MasterInfo> response = new ServiceResultList<Job_MasterInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Job_MasterInfo> result = new List<Job_MasterInfo>();

                    result = db.Database.SqlQuery<Job_MasterInfo>("exec EE_GetAssembliesList").ToList();
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
                throw ex;
            }
        }

        public ServiceResult<bool> JobIDIsExist(int JobID)
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    bool result = DbOBJ.Job_Master.Any(m => m.Job_ID == JobID);

                    if (result == true)
                    {
                        response.ResultCode = 1;
                        response.Message = "“Job ID” does exist!";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = 0;
                        response.Message = "“Job ID” not exist!";
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
        //public ServiceResult<Job_MasterInfo> SaveJobDetails(Job_MasterInfo Model)
        //{

        //}

        public ServiceResult<int> AddJobInformation(Job_MasterInfo Model)
        {

            ServiceResult<int> Response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
                {
                    int resultCount;
                    Job_Master JobMasterTbl = new Job_Master();

                    JobMasterTbl.Client_Id = Model.Client_Id;
                    JobMasterTbl.Job_Description = Model.Job_Description;
                    JobMasterTbl.Assemblies_Name = Model.Assemblies_Name;
                    if (Model.Client_Name == null)
                    {
                        JobMasterTbl.Client_Name = Model.Other_ClientName;
                    }
                    else
                    {
                        JobMasterTbl.Client_Name = Model.Client_Name;
                    }

                    JobMasterTbl.Client_Address = Model.Client_Address;
                    JobMasterTbl.Client_Address2 = Model.Client_Address2;
                    JobMasterTbl.Client_City = Model.Client_City;
                    JobMasterTbl.Client_State = Model.Client_State;
                    JobMasterTbl.Client_ZipCode = Model.Client_ZipCode;
                    JobMasterTbl.Client_Phone = Model.Client_Phone;
                    JobMasterTbl.Client_Mobile = Model.Client_Mobile;
                    JobMasterTbl.Client_ContactPerson = Model.Client_ContactPerson;
                    JobMasterTbl.Client_Fax = Model.Client_Fax;
                    JobMasterTbl.Client_Email = Model.Client_Email;

                    JobMasterTbl.Work_Address = Model.Work_Address;
                    JobMasterTbl.Work_Address2 = Model.Work_Address2;
                    JobMasterTbl.Work_City = Model.Work_City;
                    JobMasterTbl.Work_ZipCode = Model.Work_ZipCode;

                    JobMasterTbl.Directions_To = Model.Directions_To;
                    JobMasterTbl.Doing_What = Model.Doing_What;

                    JobMasterTbl.Job_Status = Model.Job_Status;
                    JobMasterTbl.Job_Mileage_Estimated = Model.Job_Mileage_Estimated;
                    JobMasterTbl.Job_Mileage_Actual = Model.Job_Mileage_Actual;
                    JobMasterTbl.Job_Mileage_Cost = Model.Job_Mileage_Cost;
                    JobMasterTbl.Job_Mileage_ExpTotal = Model.Job_Mileage_ExpTotal;
                    JobMasterTbl.Job_Mileage_EstTotal = Model.Job_Mileage_EstTotal;
                    JobMasterTbl.Job_Mileage_ActTotal = Model.Job_Mileage_ActTotal;

                    JobMasterTbl.EstimateValue_SubTotal = Model.EstimateValue_SubTotal;
                    JobMasterTbl.EstimateValue_Overhead_Percentage = Model.EstimateValue_Overhead_Percentage;
                    JobMasterTbl.EstimateValue_Overhead_EstTotal = Model.EstimateValue_Overhead_EstTotal;
                    JobMasterTbl.EstimateValue_Profit_Percentage = Model.EstimateValue_Profit_Percentage;
                    JobMasterTbl.EstimateValue_Profit_EstTotal = Model.EstimateValue_Profit_EstTotal;
                    JobMasterTbl.Estimate_Override = Model.Estimate_Override;

                    JobMasterTbl.SalesTax = Model.SalesTax;
                    JobMasterTbl.SalesTax_Perc = Model.SalesTax_Perc;
                    JobMasterTbl.Job_ExpenseTotal = Model.Job_ExpenseTotal;
                    JobMasterTbl.Job_EstimatedTotal = Model.Job_EstimatedTotal;
                    JobMasterTbl.Job_ActualTotal = Model.Job_ActualTotal;
                    JobMasterTbl.Profit_Loss_Total = Model.Profit_Loss_Total;
                    JobMasterTbl.Profit_Loss_Perc = Model.Profit_Loss_Perc;

                    JobMasterTbl.Created_By = Model.Created_By;
                    JobMasterTbl.Created_Date = DateTime.Now; ;
                    JobMasterTbl.Updated_By = Model.Updated_By;
                    JobMasterTbl.Updated_Date = DateTime.Now;
                    JobMasterTbl.Isactive = true;
                    DbObj.Job_Master.Add(JobMasterTbl);
                    resultCount = DbObj.SaveChanges();
                    if (resultCount > 0)
                    {
                        Response.Message = "“Job Information” saved successfully.";
                        Response.ResultCode = 1;
                        return Response;
                    }
                    else
                    {
                        Response.Message = " Error occurred while adding “Job Information”!";
                        Response.ResultCode = -1;
                        return Response;

                    }

                }

            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                Response.ResultCode = -1;
                return Response;

            }
        }


        public ServiceResult<int> AddPartsListDetailsInJob(Job_MasterInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();

            try
            {
                using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
                {
                    var result = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m).FirstOrDefault();
                    Job_Parts JobPartsTbl = new Job_Parts();

                    foreach (var item in Model.NewPartsListInJob)
                    {

                        JobPartsTbl.Client_ID = Model.Client_Id;
                        JobPartsTbl.Job_ID = result.Job_ID;
                        JobPartsTbl.Part_Number = item.Part_Number;
                        //      JobPartsTbl.Description = item.Description;
                        JobPartsTbl.Part_Cost = item.Part_Cost;
                        JobPartsTbl.Parts_Photo = null;
                        JobPartsTbl.Part_Resale = item.Resale_Cost;
                        JobPartsTbl.Estimated_Qty = item.Estimated_Qty;
                        JobPartsTbl.Actual_Qty = item.Actual_Qty;
                        JobPartsTbl.Expected_Total = item.Expected_Total;
                        JobPartsTbl.Estimated_Total = item.Estimated_Total;
                        JobPartsTbl.Actual_Total = item.Actual_Total;
                        JobPartsTbl.IsActive = true;
                        JobPartsTbl.LaborUnit = item.LaborUnit;
                        DbObj.Job_Parts.Add(JobPartsTbl);
                        PartsresultCount = DbObj.SaveChanges();
                    }
                    if (PartsresultCount > 0)
                    {
                        response.Message = "“Job Parts” saved successfully.";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {
                        response.Message = " Error occurred while saving “Job Parts List Details”!";
                        response.ResultCode = 1;
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


        public ServiceResult<int> AddLabouListDetailsInJob(Job_MasterInfo Model)
        {

            ServiceResult<int> response = new ServiceResult<int>();

            try
            {
                using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
                {
                    Job_Labor JoblabourTbl = new Job_Labor();
                    foreach (var item in Model.NewLabourListInJob)
                    {
                        //JoblabourTbl.Job_ID = Model.Job_ID;
                        var JobID = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m.Job_ID).FirstOrDefault();
                        JoblabourTbl.Job_ID = JobID;
                        JoblabourTbl.Client_Id = Model.Client_Id;
                        JoblabourTbl.Laborer_Name = item.Laborer_Name;
                        JoblabourTbl.Lobor_Cost = item.Cost;
                        // JobPartsTbl.Parts_Photo = item.Parts_Photo;

                        JoblabourTbl.Burden = item.Burden;
                        JoblabourTbl.Lobor_Resale = item.Resale_Cost;
                        JoblabourTbl.Estimated_Hour = item.Estimated_Hour;
                        JoblabourTbl.Estimated_Hour = item.Estimated_Hour;
                        JoblabourTbl.Expected_Total = item.Expected_Total;
                        JoblabourTbl.Estimated_Total = item.Estimated_Total;
                        JoblabourTbl.Actual_Total = item.Actual_Total;
                        JoblabourTbl.IsActive = true;
                        DbObj.Job_Labor.Add(JoblabourTbl);
                        LabourresultCount = DbObj.SaveChanges();
                    }
                    if (LabourresultCount > 0)
                    {
                        response.Message = "“Job Labor” saved successfully.";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {
                        response.Message = " Error occurred while saving “Job Labor List Details”!";
                        response.ResultCode = 1;
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


        public ServiceResult<int> AddAssembliesPartsListInJob(Job_MasterInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
                {
                    Job_Parts JobPartsTbl = new Job_Parts();
                    foreach (var item in Model.NewPartsListInJob)
                    {
                        //JobPartsTbl.Job_ID = Model.Job_ID;
                        var JobID = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m.Job_ID).FirstOrDefault();
                        JobPartsTbl.Job_ID = JobID;

                        JobPartsTbl.Part_Number = item.Part_Number;
                        JobPartsTbl.Part_Cost = item.Part_Cost;
                        // JobPartsTbl.Parts_Photo = item.Parts_Photo;

                        JobPartsTbl.Part_Resale = item.Resale_Cost;
                        JobPartsTbl.Estimated_Qty = item.Estimated_Qty;
                        JobPartsTbl.Actual_Qty = item.Actual_Qty;
                        JobPartsTbl.Expected_Total = item.Expected_Total;
                        JobPartsTbl.Estimated_Total = item.Estimated_Total;
                        JobPartsTbl.Actual_Total = item.Actual_Total;
                        JobPartsTbl.IsActive = true;
                        JobPartsTbl.LaborUnit = item.LaborUnit;
                        DbObj.Job_Parts.Add(JobPartsTbl);
                        DbObj.SaveChanges();
                    }
                    if (PartsresultCount > 0)
                    {
                        response.Message = "“Job Assemblies Parts” saved successfully.";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {
                        response.Message = " Error occurred while saving “Assemblies Parts” in job details”!";
                        response.ResultCode = 1;
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




        public ServiceResult<int> AddAssemblieslabourListInJob(Job_MasterInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();

            try
            {
                using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
                {
                    Job_Labor JoblabourTbl = new Job_Labor();
                    foreach (var item in Model.NewLabourListInJob)
                    {
                        //JoblabourTbl.Job_ID = Model.Job_ID;
                        var JobID = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m.Job_ID).FirstOrDefault();
                        JoblabourTbl.Job_ID = JobID;
                        JoblabourTbl.Laborer_Name = item.Laborer_Name;
                        JoblabourTbl.Lobor_Cost = item.Cost;
                        // JobPartsTbl.Parts_Photo = item.Parts_Photo;

                        JoblabourTbl.Burden = item.Burden;
                        JoblabourTbl.Lobor_Resale = item.Resale_Cost;
                        JoblabourTbl.Estimated_Hour = item.Estimated_Hour;
                        JoblabourTbl.Estimated_Hour = item.Estimated_Hour;
                        JoblabourTbl.Expected_Total = item.Expected_Total;
                        JoblabourTbl.Estimated_Total = item.Estimated_Total;
                        JoblabourTbl.Actual_Total = item.Actual_Total;
                        JoblabourTbl.IsActive = true;
                        DbObj.Job_Labor.Add(JoblabourTbl);
                        DbObj.SaveChanges();
                    }
                    if (LabourresultCount > 0)
                    {
                        response.Message = "“Job Assemblies Labor List” saved Successfully.";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {
                        response.Message = " Error occurred while saving “Assemblies Labor List” in job details!";
                        response.ResultCode = 1;
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

        public ServiceResult<int> AddJobAssembliesPartsList(Job_MasterInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
                {
                    Job_Assembly_Parts JobPartsTbl = new Job_Assembly_Parts();
                    foreach (var item in Model.NewJobAssemblyPartsList)
                    {
                        //JobPartsTbl.Job_ID = Model.Job_ID;
                        var JobID = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m.Job_ID).FirstOrDefault();
                        var Client_ID = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m.Client_Id).FirstOrDefault();
                        JobPartsTbl.Client_ID = Client_ID;
                        JobPartsTbl.Job_ID = JobID;
                        JobPartsTbl.Assemblies_Name = item.Assemblies_Name;
                        JobPartsTbl.Assemblies_Category = item.Assemblies_Category;
                        JobPartsTbl.Part_Number = item.Part_Number;
                        JobPartsTbl.Part_Cost = item.Part_Cost;
                        JobPartsTbl.Parts_Photo = item.Parts_Photo;
                        JobPartsTbl.Part_Resale = item.Part_Resale;
                        JobPartsTbl.Estimated_Qty = item.Estimated_Qty;
                        JobPartsTbl.Actual_Qty = item.Actual_Qty;
                        JobPartsTbl.Expected_Total = item.Expected_Total;
                        JobPartsTbl.Estimated_Total = item.Estimated_Total;
                        JobPartsTbl.Actual_Total = item.Actual_Total;
                        JobPartsTbl.IsActive = true;
                        DbObj.Job_Assembly_Parts.Add(JobPartsTbl);
                        DbObj.SaveChanges();
                    }
                    if (PartsresultCount > 0)
                    {
                        response.Message = "“Job Assemblies Parts” saved successfully.";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {
                        response.Message = " Error occurred while saving “Assemblies Parts” in job details”!";
                        response.ResultCode = 1;
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

        //public ServiceResult<int> AddJobAssembliesLaborList(Job_MasterInfo Model)
        //{
        //    ServiceResult<int> response = new ServiceResult<int>();
        //    try
        //    {
        //        using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
        //        {
        //            Job_Assembly_Labor JobPartsTbl = new Job_Assembly_Labor();
        //            foreach (var item in Model.NewJobAssemblyPartsList)
        //            {
        //                //JobPartsTbl.Job_ID = Model.Job_ID;
        //                var JobID = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m.Job_ID).FirstOrDefault();
        //                var Client_ID = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m.Client_Id).FirstOrDefault();
        //                JobPartsTbl.Client_Id = Client_ID;
        //                JobPartsTbl.Job_ID = JobID;
        //                JobPartsTbl.Assemblies_Name = item.Assemblies_Name;
        //                JobPartsTbl.Assemblies_Category = item.Assemblies_Category;
        //                JobPartsTbl.Lobor_Cost = item.labor_cost;
        //                JobPartsTbl.Burden = item.Burden;
        //                JobPartsTbl.Lobor_Resale = item.labor_ResaleCost;
        //                JobPartsTbl.Estimated_Hour = Convert.ToDecimal(item.Estimated_Hour);
        //                JobPartsTbl.Actual_Hours = item.Actual_Hours;
        //                JobPartsTbl.Expected_Total = item.Expected_Total;
        //                JobPartsTbl.Estimated_Total = item.Estimated_Total;
        //                JobPartsTbl.Actual_Total = item.Actual_Total;
        //                JobPartsTbl.IsActive = true;
        //                DbObj.Job_Assembly_Labor.Add(JobPartsTbl);
        //                DbObj.SaveChanges();
        //            }
        //            if (PartsresultCount > 0)
        //            {
        //                response.Message = "Job Assemblies Parts Saved Succefully!";
        //                response.ResultCode = 1;
        //                return response;
        //            }
        //            else
        //            {
        //                response.Message = " Error Occured while saving Assemblies Parts  in Job Details!";
        //                response.ResultCode = 1;
        //                return response;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        response.ResultCode = -1;
        //        response.Message = ex.Message;
        //        return response;
        //    }
        //}

        public ServiceResult<int> AddJobAttachMents(Job_MasterInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();

            try
            {
                using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
                {
                    Job_Attachments JobAttachMentTbl = new Job_Attachments();
                    //JobAttachMentTbl.Job_ID = Model.Job_ID;
                    var JobID = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m.Job_ID).FirstOrDefault();
                    JobAttachMentTbl.Job_ID = JobID;
                    JobAttachMentTbl.Client_Id = Model.Client_Id;
                    JobAttachMentTbl.Attachement_Name = Model.Attachement_Name;
                    JobAttachMentTbl.Attachment = Model.Job_Attachment;
                    JobAttachMentTbl.IsActive = true;
                    DbObj.Job_Attachments.Add(JobAttachMentTbl);
                    int AtachmentResultCount = DbObj.SaveChanges();

                    if (AtachmentResultCount > 0)
                    {
                        response.Message = "“Job Details” saved successfully.";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {
                        response.Message = " Error occurred while saving “Job Attachment List” in job details!";
                        response.ResultCode = 1;
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
        //       public ServiceResult<int>AddDjeDetailsinfoinjobs(Job_MasterInfo model)
        //       {
        //           ServiceResult<int> responce = new ServiceResult<int>();
        //           try
        //           {
        //               using(ElectricEaseEntitiesContext Dbobj = new ElectricEaseEntitiesContext())
        //               {
        //                   Job_DJE_VQ_Detailsinfo JobDJEtbl = new Job_DJE_VQ_Detailsinfo();
        //foreach(var item in Models.)
        //               }
        //           }

        //       }

        public ServiceResult<int> AddLegalDetailsInJob(Job_MasterInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
                {
                    Job_Legal JobLegalTbl = new Job_Legal();
                    foreach (var item in Model.NewLegalListInJob)
                    {
                        //JobPartsTbl.Job_ID = Model.Job_ID;
                        var JobID = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m.Job_ID).FirstOrDefault();
                        var Client_ID = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m.Client_Id).FirstOrDefault();
                        JobLegalTbl.Client_ID = Client_ID;
                        JobLegalTbl.Job_ID = JobID;
                        JobLegalTbl.Legal_ID = item.Legal_ID;
                        JobLegalTbl.Legal_Detail = item.Legal_Detail;
                        JobLegalTbl.IsActive = true;

                        DbObj.Job_Legal.Add(JobLegalTbl);
                        DbObj.SaveChanges();
                    }
                    if (LegalResulutcount > 0)
                    {
                        response.Message = "“Job Legal” saved successfully.";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {
                        response.Message = " Error occurred while saving “Assemblies Parts” in job details”!";
                        response.ResultCode = 1;
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

        public ServiceResultList<Job_MasterInfoList> GetJobDetailList(int ClientID)
        {
            ServiceResultList<Job_MasterInfoList> response = new ServiceResultList<Job_MasterInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Job_MasterInfoList> result = new List<Job_MasterInfoList>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    result = db.Database.SqlQuery<Job_MasterInfoList>("exec EE_GetJobDetailList @ClientID", para).ToList();


                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            item.JobAssignedList = (from JobcalenderTbl in db.Job_Calendar
                                                    join AcTbl in db.Account_Master on JobcalenderTbl.AssignTo equals AcTbl.User_ID
                                                    where JobcalenderTbl.JobId == item.Job_ID && JobcalenderTbl.IsActive == true && AcTbl.IsActive == true && AcTbl.Client_ID == ClientID
                                                    select new JobAssignInfo()
                                                    {
                                                        AssignedUser = JobcalenderTbl.AssignTo,
                                                        UserColor = AcTbl.UserColor

                                                    }).ToList();

                            // var GetUserColor = (from m in db.Account_Master where m.Client_ID == ClientID && m.IsActive == true && m.User_ID == item.Job_AssignedUser select m.UserColor).FirstOrDefault();
                            //item.UserColor = GetUserColor;
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


        public ServiceResult<Job_MasterInfo> GetJobDetails(int Job_ID, int ClientID)
        {
            ServiceResult<Job_MasterInfo> response = new ServiceResult<Job_MasterInfo>();
            try
            {
                using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                {
                    Job_Master JobTblOBJ = new Job_Master();
                    JobTblOBJ = (from m in DbOBJ.Job_Master where (m.Client_Id == ClientID && m.Job_ID == Job_ID && m.Isactive == true) select m).FirstOrDefault();
                    if (JobTblOBJ == null)
                    {
                        response.Message = "“Job ID” does not exist!";
                        response.ResultCode = -1;
                        return response;
                    }

                    // Here Get Job Info table Information
                    response.Data = new Job_MasterInfo()
                    {
                        Job_ID = JobTblOBJ.Job_ID,
                        Job_Number = JobTblOBJ.Job_Number,
                        JobDisplayId = JobTblOBJ.JobDisplayId,
                        Job_Description = JobTblOBJ.Job_Description,
                        Job_Status = JobTblOBJ.Job_Status,
                        //Start_Date= JobTblOBJ.Start_Date,
                        //End_Date= JobTblOBJ.End_Date,
                        //Start_Date= JobTblOBJ.Start_Date.HasValue? JobTblOBJ.Start_Date.Value.ToString("MM/dd/yyyy"):string.Empty,
                        // End_Date= JobTblOBJ.End_Date.HasValue ? JobTblOBJ.End_Date.Value.ToString("MM/dd/yyyy") : string.Empty,
                        TempPole = JobTblOBJ.TempPole.Value,
                        Job_AssignedUser = JobTblOBJ.Job_AssignedUser,
                        Client_Name = JobTblOBJ.Client_Name,
                        Client_ContactPerson = JobTblOBJ.Client_ContactPerson,
                        Client_Address = JobTblOBJ.Client_Address,
                        Client_Address2 = JobTblOBJ.Client_Address2,
                        Client_City = JobTblOBJ.Client_City,
                        Client_State = JobTblOBJ.Client_State,
                        Client_ZipCode = JobTblOBJ.Client_ZipCode,
                        Client_Phone = JobTblOBJ.Client_Phone,
                        Client_Mobile = JobTblOBJ.Client_Mobile,
                        Client_Fax = JobTblOBJ.Client_Fax,
                        Client_Email = JobTblOBJ.Client_Email,
                        Previous_Location = JobTblOBJ.Work_Location,
                        Work_Address = JobTblOBJ.Work_Address,
                        Work_Address2 = JobTblOBJ.Work_Address2,
                        Work_State = JobTblOBJ.Work_State,
                        Work_City = JobTblOBJ.Work_City,
                        Work_ZipCode = JobTblOBJ.Work_ZipCode,
                        Directions_To = JobTblOBJ.Directions_To,
                        Doing_What = JobTblOBJ.Doing_What,
                        Job_Mileage_ActTotal = JobTblOBJ.Job_Mileage_ActTotal,
                        Job_Mileage_Cost = JobTblOBJ.Job_Mileage_Cost,
                        Job_Mileage_Actual = JobTblOBJ.Job_Mileage_Actual,
                        Job_Mileage_Estimated = JobTblOBJ.Job_Mileage_Estimated,
                        Job_Mileage_EstTotal = JobTblOBJ.Job_Mileage_EstTotal,
                        Job_Mileage_ExpTotal = JobTblOBJ.Job_Mileage_ExpTotal,
                        EstimateValue_SubTotal = JobTblOBJ.EstimateValue_SubTotal,
                        EstimateValue_Profit_Percentage = JobTblOBJ.EstimateValue_Profit_Percentage,
                        EstimateValue_ExpTotal = JobTblOBJ.EstimateValue_ExpTotal,
                        EstimateValue_Overhead_EstTotal = JobTblOBJ.EstimateValue_Overhead_EstTotal,
                        EstimateValue_Overhead_ExpTotal = JobTblOBJ.EstimateValue_Overhead_ExpTotal,
                        EstimateValue_Overhead_Percentage = JobTblOBJ.EstimateValue_Overhead_Percentage,
                        EstimateValue_Profit_EstTotal = JobTblOBJ.EstimateValue_Profit_EstTotal,
                        Profit_Loss_Perc = JobTblOBJ.Profit_Loss_Perc,
                        Profit_Loss_Total = JobTblOBJ.Profit_Loss_Total,
                        Job_ActualTotal = JobTblOBJ.Job_ActualTotal,
                        Job_EstimatedTotal = JobTblOBJ.Job_EstimatedTotal,
                        Job_ExpenseTotal = JobTblOBJ.Job_ExpenseTotal,
                        SalesTax = JobTblOBJ.SalesTax,
                        SalesTax_Perc = JobTblOBJ.SalesTax_Perc,
                        Estimate_Override = JobTblOBJ.Estimate_Override,
                        Created_By = JobTblOBJ.Created_By,
                        Created_Date = JobTblOBJ.Created_Date,
                        Updated_By = JobTblOBJ.Updated_By,
                        Updated_Date = JobTblOBJ.Updated_Date

                    };


                    response.Data.NewPartsListInJob = (from JobTbl in DbOBJ.Job_Master
                                                       join JobPartsTabl in DbOBJ.Job_Parts
                                                        on JobTbl.Job_ID equals JobPartsTabl.Job_ID
                                                       join PartsTbl in DbOBJ.Parts_Details
                                                       on JobPartsTabl.Part_Number equals PartsTbl.Part_Number
                                                       where (JobPartsTabl.Client_ID == ClientID
                                                   && JobPartsTabl.Job_ID == Job_ID
                                                    && JobTbl.Isactive == true
                                                     && JobPartsTabl.IsActive == true)
                                                       select new Parts_DetailsInfoList()
                                                       {
                                                           Client_ID = JobPartsTabl.Client_ID,
                                                           Part_ID = JobPartsTabl.Part_ID,
                                                           Part_Number = JobPartsTabl.Part_Number.Trim(),
                                                           Part_Cost = JobPartsTabl.Part_Cost,
                                                           Resale_Cost = JobPartsTabl.Part_Resale,
                                                           Estimated_Qty = JobPartsTabl.Estimated_Qty,
                                                           Actual_Qty = JobPartsTabl.Actual_Qty,
                                                           Expected_Total = JobPartsTabl.Expected_Total,
                                                           Estimated_Total = JobPartsTabl.Estimated_Total,
                                                           PartDescription = PartsTbl.Description,
                                                           LaborUnit = JobPartsTabl.LaborUnit

                                                       }
                                             ).ToList();

                    response.Data.NewLabourListInJob = (from JobTbl in DbOBJ.Job_Master
                                                        join JobLaborTabl in DbOBJ.Job_Labor
                                                      on JobTbl.Job_ID equals JobLaborTabl.Job_ID
                                                        where (
                                                        JobLaborTabl.Client_Id == ClientID
                                                         && JobLaborTabl.Job_ID == Job_ID
                                                      && JobLaborTabl.IsActive == true
                                                      && JobTbl.Isactive == true)
                                                        select new Labor_DetailsInfoList()
                                                        {
                                                            Client_ID = JobLaborTabl.Client_Id,
                                                            Labour_ID = JobLaborTabl.Labour_ID,
                                                            Burden = JobLaborTabl.Burden,
                                                            Laborer_Name = JobLaborTabl.Laborer_Name,
                                                            Cost = JobLaborTabl.Lobor_Cost,
                                                            Resale_Cost = JobLaborTabl.Lobor_Resale,
                                                            Estimated_Hour = JobLaborTabl.Estimated_Hour,
                                                            Actual_Hours = JobLaborTabl.Actual_Hours,
                                                            Expected_Total = JobLaborTabl.Expected_Total,
                                                            Estimated_Total = JobLaborTabl.Estimated_Total,
                                                            Actual_Total = JobLaborTabl.Actual_Total,

                                                        }
                                                ).ToList();

                    response.Data.NewJobAssemblyPartsList = (from JobTbl in DbOBJ.Job_Master
                                                             join JobASPartsTabl in DbOBJ.Job_Assembly_Parts
                                                              on JobTbl.Job_ID equals JobASPartsTabl.Job_ID
                                                            // join PartsTbl in DbOBJ.Parts_Details
                                                            //on JobASPartsTabl.Part_Number equals PartsTbl.Part_Number
                                                             where (JobASPartsTabl.Client_ID == ClientID
                                                         && JobASPartsTabl.Job_ID == Job_ID
                                                          && JobTbl.Isactive == true
                                                           && JobASPartsTabl.IsActive == true)
                                                             select new Assembly_MasterInfo()
                                                             {
                                                                 Client_ID = JobASPartsTabl.Client_ID,
                                                                 Part_ID = JobASPartsTabl.AssembliesPart_ID,
                                                                 Assemblies_Name = JobASPartsTabl.Assemblies_Name,
                                                                 Assemblies_Category = JobASPartsTabl.Assemblies_Category,
                                                                 Part_Number = JobASPartsTabl.Part_Number.Trim(),
                                                                 Part_Cost = JobASPartsTabl.Part_Cost,
                                                                 Part_Resale = JobASPartsTabl.Part_Resale,
                                                                 Estimated_Qty = JobASPartsTabl.Estimated_Qty,
                                                                 Actual_Qty = JobASPartsTabl.Actual_Qty,
                                                                 Expected_Total = JobASPartsTabl.Expected_Total,
                                                                 Estimated_Total = JobASPartsTabl.Estimated_Total,
                                                                 //PartDescription = PartsTbl.Description
                                                                 // IsActive = true

                                                             }
                                                ).ToList();

                    response.Data.NewJobAssemblyLaborList = (from JobTbl in DbOBJ.Job_Master
                                                             join JobASLaborTabl in DbOBJ.Job_Assembly_Labor
                                                           on JobTbl.Job_ID equals JobASLaborTabl.Job_ID
                                                             where (
                                                             JobASLaborTabl.Client_Id == ClientID
                                                              && JobASLaborTabl.Job_ID == Job_ID
                                                           //&& JobASLaborTabl.IsActive == true
                                                           && JobTbl.Isactive == true)
                                                             select new Assembly_MasterInfo()
                                                             {
                                                                 Client_ID = JobASLaborTabl.Client_Id,
                                                                 Labour_ID = JobASLaborTabl.AssembliesLabour_ID,
                                                                 Burden = JobASLaborTabl.Burden,
                                                                 labor_cost = JobASLaborTabl.Lobor_Cost,
                                                                 labor_ResaleCost = JobASLaborTabl.Lobor_Resale,
                                                                 Estimated_Hour = JobASLaborTabl.Estimated_Hour,
                                                                 Actual_Hours = JobASLaborTabl.Actual_Hours,
                                                                 Expected_Total = JobASLaborTabl.Expected_Total ?? 0,
                                                                 Estimated_Total = JobASLaborTabl.Estimated_Total ?? 0,
                                                                 Actual_Total = JobASLaborTabl.Actual_Total ?? 0,

                                                             }
                                                   ).ToList();
                    response.Data.NewLegalListInJob = (from JobTbl in DbOBJ.Job_Master
                                                       join JobLegalTabl in DbOBJ.Job_Legal
                                                     on JobTbl.Job_ID equals JobLegalTabl.Job_ID
                                                       where (
                                                       JobLegalTabl.Client_ID == ClientID
                                                        && JobLegalTabl.Job_ID == Job_ID
                                                     && JobLegalTabl.IsActive == true
                                                     && JobTbl.Isactive == true)
                                                       select new Legal_DetailsInfoList()
                                                       {
                                                           Client_ID = JobLegalTabl.Client_ID,
                                                           Legal_ID = JobLegalTabl.Legal_ID,
                                                           Job_Legal_ID = JobLegalTabl.Job_Legal_ID,
                                                           Legal_Detail = JobLegalTabl.Legal_Detail


                                                       }
                                                       ).ToList();
                    response.Data.DJEVQDetails = (from jobTbl in DbOBJ.Job_Master
                                                  join jobDJeTbl in DbOBJ.Job_DJE_VQ_Details
                                                      on jobTbl.Job_ID equals jobDJeTbl.Job_ID
                                                  where (
                                                  jobDJeTbl.Client_ID == ClientID
                                                  && jobDJeTbl.Job_ID == Job_ID
                                                  && jobDJeTbl.IsActive == true
                                                  && jobTbl.Isactive == true)
                                                  select new Job_DJE_VQ_Detailsinfo()
                                                  {
                                                      Client_ID = jobDJeTbl.Client_ID,
                                                      Expense = jobDJeTbl.Expense,
                                                      Vendor_Name = jobDJeTbl.Vendor_Name,
                                                      Cost = jobDJeTbl.Cost,
                                                      Profit = jobDJeTbl.Profit,
                                                      Job_DJE_VQ_Status = jobDJeTbl.Job_DJE_VQ_Status,
                                                      Resale_Total = jobDJeTbl.Resale_Total,
                                                      Job_DJETotal = jobDJeTbl.Job_DJETotal,
                                                      Job_VQTotal = jobDJeTbl.Job_VQTotal
                                                  }
                                                    ).ToList();

                    //here getting Estimation Details
                    response.Data.Job_EstimationDetails = (from job in DbOBJ.Job_Master
                                                           join jobEst in DbOBJ.Job_EstimationDetails on job.Job_ID equals jobEst.Job_ID
                                                           where job.Client_Id == ClientID && job.Job_ID == Job_ID
                                                           && job.Isactive == true && jobEst.IsActive == true
                                                           select new Job_EstimationDetailsinfo()
                                                           {

                                                               Selected_Estimation_Type = jobEst.Selected_Estimation_Type,
                                                               Job_AssemblyTotal = jobEst.Job_AssemblyTotal,
                                                               Estimation1_AssemblyTax = jobEst.Estimation1_AssemblyTax,
                                                               Estimation1_AssemblyTotal = jobEst.Estimation1_AssemblyTotal,
                                                               Estimation2_AssemblyProfit = jobEst.Estimation2_AssemblyProfit,
                                                               Estimation2_AssemblySubTotal = jobEst.Estimation2_AssemblySubTotal,
                                                               Estimation2_AssemblyTax = jobEst.Estimation2_AssemblyTax,
                                                               Estimation2_AssemblyTotal = jobEst.Estimation2_AssemblyTotal,
                                                               Job_LaborTotal = jobEst.Job_LaborTotal,
                                                               Estimation1_LaborTax = jobEst.Estimation1_LaborTax,
                                                               Estimation1_laborTotal = jobEst.Estimation1_laborTotal,
                                                               Estimation2_LaborProft = jobEst.Estimation2_LaborProft,
                                                               Estimate2_LaborSubTotal = jobEst.Estimate2_LaborSubTotal,
                                                               Estimate2_LaborTax = jobEst.Estimate2_LaborTax,
                                                               Estimate2_LaborTotal = jobEst.Estimate2_LaborTotal,
                                                               Job_PartsTotal = jobEst.Job_PartsTotal,
                                                               Estimation1_PartsTax = jobEst.Estimation1_PartsTax,
                                                               Estimation1_PartsTotal = jobEst.Estimation1_PartsTotal,
                                                               Estimation2_PartsProfit = jobEst.Estimation2_PartsProfit,
                                                               Estimation2_PartsSubTotal = jobEst.Estimation2_PartsSubTotal,
                                                               Estimation2_PartsTax = jobEst.Estimation2_PartsTax,
                                                               Estimation2_PartsTotal = jobEst.Estimation2_PartsTotal,
                                                               Job_DJETotal = jobEst.Job_DJETotal,
                                                               Estimation1_DJETax = jobEst.Estimation1_DJETax,
                                                               Estimation1_DJETotal = jobEst.Estimation1_DJETotal,
                                                               Estimation2_DJEProfit = jobEst.Estimation2_DJEProfit,
                                                               Estimation2_DJESubTotal = jobEst.Estimation2_DJESubTotal,
                                                               Estimation2_DJETax = jobEst.Estimation2_DJETax,
                                                               Estimation2_DJETotal = jobEst.Estimation2_DJETotal,
                                                               Job_VQTotal = jobEst.Job_VQTotal,
                                                               Estimation1_VQTax = jobEst.Estimation1_VQTax,
                                                               Estimation1_VQTotal = jobEst.Estimation1_VQTotal,
                                                               Estimation2_VQProfit = jobEst.Estimation2_VQProfit,
                                                               Estimation2_VQSubtotal = jobEst.Estimation2_VQSubtotal,
                                                               Estimation2_VQTax = jobEst.Estimation2_VQTax,
                                                               Estimation2_VQTotal = jobEst.Estimation2_VQTotal,
                                                               Estimation1_GrandTotal = jobEst.Estimation1_GrandTotal,
                                                               Estimation1_Total = jobEst.Estimation1_Total,
                                                               Estimation2_GrandTotal = jobEst.Estimation2_GrandTotal,
                                                               Estimation2_SubTotal = jobEst.Estimation2_SubTotal,
                                                               Estimation2_Total = jobEst.Estimation2_Total,
                                                               Estimate3_Override = jobEst.Estimate3_Override,
                                                               Estimate4_Subtotal = jobEst.Estimate4_Subtotal,
                                                               Estimate4_Tax = jobEst.Estimate4_Tax,
                                                               Estimate4_Total = jobEst.Estimate4_Total,
                                                               Estimate4_Data = jobEst.Estimate4_Data,
                                                           }).FirstOrDefault();
                    if (response.Data.Job_EstimationDetails == null)
                    {
                        response.Data.Job_EstimationDetails = new Job_EstimationDetailsinfo();
                    }
                    var result = (from JobTbl in DbOBJ.Job_Master
                                  join JobAttachTabl in DbOBJ.Job_Attachments
                                on JobTbl.Job_ID equals JobAttachTabl.Job_ID
                                  where (
                                  JobAttachTabl.Client_Id == ClientID
                                   && JobAttachTabl.Job_ID == Job_ID
                                && JobAttachTabl.IsActive == true
                                && JobTbl.Isactive == true)
                                  select JobAttachTabl
                                                           ).FirstOrDefault();
                    if (result != null)
                    {
                        response.Data.Attachement_Name = result.Attachement_Name;
                        response.Data.Job_Attachment = result.Attachment;
                    }

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
        /// To save jobs
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        /// 
        public ServiceResult<JobCalendarinfo> AddJobCalenderInfo(JobCalendarinfo Model)
        {
            ServiceResult<JobCalendarinfo> response = new ServiceResult<JobCalendarinfo>();
            try
            {
                using (ElectricEaseEntitiesContext dbobj = new ElectricEaseEntitiesContext())
                {
                    Job_Calendar JobCalenderTable = new Job_Calendar();
                    var checkisExist = dbobj.Job_Calendar.Where(m => m.Client_Id == Model.Client_Id && m.JobId == Model.JobId && m.StartDate == Model.StartDate && m.StartTime == Model.StartTime && m.EndDate == Model.EndDate && m.EndTime == Model.EndTime && m.IsActive == true && m.AssignTo == Model.AssignTo).FirstOrDefault();
                    if (checkisExist != null)
                    {
                        response.ResultCode = 0;
                        response.Message = "This “Event” is already registered!";
                        return response;
                    }
                    else
                    {

                        var checkisExistFulday = dbobj.Job_Calendar.Where(m => m.Client_Id == Model.Client_Id && m.JobId == Model.JobId && m.StartDate == Model.StartDate && m.EndDate == Model.EndDate && m.FullDay == true && m.IsActive == true && m.AssignTo == Model.AssignTo).FirstOrDefault();

                        if (checkisExistFulday != null)
                        {
                            response.ResultCode = 0;
                            response.Message = "This “Event” is already registered!";
                            return response;
                        }
                        else
                        {


                            JobCalenderTable.Client_Id = Model.Client_Id;
                            JobCalenderTable.JobId = Model.JobId;
                            JobCalenderTable.StartDate = Model.StartDate;
                            JobCalenderTable.StartTime = Model.StartTime;
                            JobCalenderTable.EndDate = Model.EndDate;
                            JobCalenderTable.EndTime = Model.EndTime;
                            JobCalenderTable.AssignTo = Model.AssignTo;
                            JobCalenderTable.FullDay = Model.FullDay;
                            JobCalenderTable.IsActive = true;

                            dbobj.Job_Calendar.Add(JobCalenderTable);
                            int resultCount = dbobj.SaveChanges();
                            if (resultCount > 0)
                            {
                                response.ResultCode = 1;
                                response.Message = "“Job Calendar” details are added successfully.";
                                return response;
                            }
                            else
                            {
                                response.ResultCode = 0;
                                response.Message = "Error";
                                return response;
                            }

                        }


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
        public ServiceResultList<JobCalendarinfo> GetJobCalenderEventsList(int ClientID, int JobID)
        {
            ServiceResultList<JobCalendarinfo> response = new ServiceResultList<JobCalendarinfo>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<JobCalendarinfo> result = new List<JobCalendarinfo>();
                    SqlParameter[] para = new SqlParameter[2];
                    para[0] = new SqlParameter() { ParameterName = "@ClientID", Value = ClientID };
                    para[1] = new SqlParameter() { ParameterName = "@JobID", Value = JobID };
                    result = db.Database.SqlQuery<JobCalendarinfo>("exec EE_GetJobCalenderList @ClientID,@JobID", para).ToList();

                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            var GetUserColor = (from m in db.Account_Master where m.Client_ID == ClientID && m.IsActive == true && m.User_ID == item.AssignTo select m.UserColor).FirstOrDefault();
                            item.UserColor = GetUserColor;

                        }


                        response.ListData = result;
                        foreach (var item in response.ListData)
                        {
                            if (item.StartTime != null && item.EndTime != null)
                            {
                                item.Totalhours = (item.StartTime - item.EndTime).Value.TotalHours.ToString();
                            }

                            if (item.StartTime != TimeSpan.MinValue && item.EndTime != TimeSpan.MinValue && item.StartTime != null && item.EndTime != null)
                            {
                                //item.sTime = item.Start.ToString("hh:mm tt");
                                // item.eTime = item.End.ToString("hh:mm tt");
                                //string stime = Convert.ToString(item.StartTime);
                                //DateTime sdateTime = DateTime.ParseExact(Model.stTime,
                                //    "hh:mm tt", CultureInfo.InvariantCulture);

                            }

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
        //Delete JobEvent in Job Calender
        public ServiceResult<int> DeleteJobCalenderEvent(int ClientID, int EventID, int JobID)
        {
            ServiceResult<int> response = new ServiceResult<int>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Job_Calendar JobCalenderTbl = new Job_Calendar();
                    JobCalenderTbl = db.Job_Calendar.Where(m => m.Client_Id == ClientID && m.Id == EventID && m.JobId == JobID).FirstOrDefault();
                    JobCalenderTbl.IsActive = false;
                    int resultCount = db.SaveChanges();
                    if (resultCount > 0)
                    {
                        response.ResultCode = 1;
                        response.Message = "“Job Event” details are deleted successfully.";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = 0;
                        response.Message = "Error";
                        return response;

                    }

                }


            }
            catch (Exception ex)
            {
                response.ResultCode = 1;
                response.Message = ex.Message;
                return response;
            }
        }
        /// <summary>
        /// To delete parts from jobs
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="PartId"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public string deleteJobParts(int JobId, string PartId, int ClientID)
        {
            string stat = "";
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    var parts = db.Job_Parts.Where(x => x.Part_Number == PartId && x.Job_ID == JobId && x.Client_ID == ClientID).FirstOrDefault();
                    if (parts != null)
                    {
                        db.Job_Parts.Remove(parts);
                        db.SaveChanges();
                    }
                    stat = "success";
                }
            }
            catch (Exception ex)
            {
                stat = ex.Message;
            }
            return stat;
        }
        /// <summary>
        /// To delete labour from jobs
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="PartId"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public string deleteJobLabour(int JobId, string LabourName, int ClientID)
        {
            string stat = "";
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    var labour = db.Job_Labor.Where(x => x.Laborer_Name == LabourName && x.Job_ID == JobId && x.Client_Id == ClientID).FirstOrDefault();
                    if (labour != null)
                    {
                        db.Job_Labor.Remove(labour);
                        db.SaveChanges();
                    }
                    stat = "success";
                }
            }
            catch (Exception ex)
            {
                stat = ex.Message;
            }
            return stat;
        }
        public string SaveJobDetails(Job_MasterInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            int resultCount;
            int jobid = 0;
            try
            {
                using (ElectricEaseEntitiesContext JobMastertblDBOBJ = new ElectricEaseEntitiesContext())
                {

                    Job_Master JobMasterTbl = new Job_Master();
                    bool isnew = false;
                    JobMasterTbl = JobMastertblDBOBJ.Job_Master.Where(x => x.Client_Id == Model.Client_Id && x.Job_ID == Model.Job_ID).FirstOrDefault();
                    if (JobMasterTbl == null)
                    {
                        JobMasterTbl = new Job_Master();
                        isnew = true;
                    }
                    JobMasterTbl.Job_Number = Model.Job_Number;
                    JobMasterTbl.Client_Id = Model.Client_Id;
                    JobMasterTbl.Job_Description = Model.Job_Description;
                    JobMasterTbl.Job_AssignedUser = Model.Job_AssignedUser;
                    JobMasterTbl.TempPole = Model.TempPole;
                    JobMasterTbl.Assemblies_Name = Model.Assemblies_Name;
                    if (Model.Client_Name == "New Client")
                    {
                        JobMasterTbl.Client_Name = Model.Other_ClientName;
                    }
                    else
                    {
                        JobMasterTbl.Client_Name = Model.Client_Name;
                    }

                    JobMasterTbl.Client_Address = Model.Client_Address;
                    JobMasterTbl.Client_Address2 = Model.Client_Address2;
                    JobMasterTbl.Client_City = Model.Client_City;
                    JobMasterTbl.Client_State = Model.Client_State;
                    JobMasterTbl.Client_ZipCode = Model.Client_ZipCode;
                    JobMasterTbl.Client_Phone = Model.Client_Phone;
                    JobMasterTbl.Client_Mobile = Model.Client_Mobile;
                    JobMasterTbl.Client_ContactPerson = Model.Client_ContactPerson;
                    JobMasterTbl.Client_Fax = Model.Client_Fax;
                    JobMasterTbl.Client_Email = Model.Client_Email;

                    JobMasterTbl.Work_Address = Model.Work_Address;
                    JobMasterTbl.Work_Address2 = Model.Work_Address2;
                    JobMasterTbl.Work_City = Model.Work_City;
                    JobMasterTbl.Work_State = Model.Work_State;

                    if (Model.Previous_Location == "New Location")
                    {
                        JobMasterTbl.Work_Location = Model.Other_Previous_Location;
                    }
                    else
                    {
                        JobMasterTbl.Work_Location = Model.Previous_Location;
                    }
                    JobMasterTbl.Work_ZipCode = Model.Work_ZipCode;
                    JobMasterTbl.Directions_To = Model.Directions_To;
                    JobMasterTbl.Doing_What = Model.Doing_What;
                    JobMasterTbl.Job_Status = Model.Job_Status;
                    JobMasterTbl.Parts_GrandExpTotal = Model.Parts_GrandExpTotal;
                    JobMasterTbl.Parts_GrandEstTotal = Model.Parts_GrandEstTotal;
                    JobMasterTbl.Parts_GrandActTotal = Model.Parts_GrandActTotal;

                    JobMasterTbl.Labor_GrandExpTotal = Model.Labor_GrandExpTotal;
                    JobMasterTbl.Labor_GrandEstTotal = Model.Labor_GrandEstTotal;
                    JobMasterTbl.Labor_GrandActTotal = Model.Labor_GrandActTotal;

                    JobMasterTbl.Job_Mileage_Estimated = Model.Job_Mileage_Estimated;
                    JobMasterTbl.Job_Mileage_Actual = Model.Job_Mileage_Actual;
                    JobMasterTbl.Job_Mileage_Cost = Model.Job_Mileage_Cost;
                    JobMasterTbl.Job_Mileage_ExpTotal = Model.Job_Mileage_ExpTotal;
                    JobMasterTbl.Job_Mileage_EstTotal = Model.Job_Mileage_EstTotal;
                    JobMasterTbl.Job_Mileage_ActTotal = Model.Job_Mileage_ActTotal;

                    JobMasterTbl.EstimateValue_SubTotal = Model.EstimateValue_SubTotal;
                    JobMasterTbl.EstimateValue_Overhead_Percentage = Model.EstimateValue_Overhead_Percentage;
                    JobMasterTbl.EstimateValue_Overhead_EstTotal = Model.EstimateValue_Overhead_EstTotal;
                    JobMasterTbl.EstimateValue_Profit_Percentage = Model.EstimateValue_Profit_Percentage;
                    JobMasterTbl.EstimateValue_Overhead_ExpTotal = Model.EstimateValue_Overhead_ExpTotal;
                    JobMasterTbl.EstimateValue_Profit_EstTotal = Model.EstimateValue_Profit_EstTotal;
                    JobMasterTbl.Estimate_Override = Model.Estimate_Override;
                    JobMasterTbl.EstimateValue_ExpTotal = Model.EstimateValue_ExpTotal;
                    JobMasterTbl.SalesTax = Model.SalesTax;
                    JobMasterTbl.SalesTax_Perc = Model.SalesTax_Perc;
                    JobMasterTbl.Job_ExpenseTotal = Model.Job_ExpenseTotal;
                    JobMasterTbl.Job_EstimatedTotal = Model.Job_EstimatedTotal;
                    JobMasterTbl.Job_ActualTotal = Model.Job_ActualTotal;
                    JobMasterTbl.Profit_Loss_Total = Model.Profit_Loss_Total;
                    JobMasterTbl.Profit_Loss_Perc = Model.Profit_Loss_Perc;
                    JobMasterTbl.PriceUpdatedDate = DateTime.Now;

                    if (Model.Job_ID == 0)
                    {
                        JobMasterTbl.Created_Date = DateTime.Now;
                        JobMasterTbl.Updated_Date = DateTime.Now;
                        JobMasterTbl.Created_By = Model.Created_By;
                    }
                    else
                    {
                        // JobMasterTbl.Start_Date =Model.Start_Date;
                        //JobMasterTbl.Start_Date = Model.Start_Date==null?(DateTime?)null: Model.Start_Date;
                        // JobMasterTbl.End_Date = Model.End_Date;
                        JobMasterTbl.Updated_Date = DateTime.Now;
                        JobMasterTbl.Updated_By = Model.Updated_By;
                    }

                    JobMasterTbl.Updated_By = Model.Updated_By;
                    JobMasterTbl.Isactive = true;
                    if (Model.Job_ID == 0)
                    {
                        JobMastertblDBOBJ.Job_Master.Add(JobMasterTbl);
                        logs.WriteLog("Adding New Job", JobMasterTbl.Client_Id, Model.Created_By);
                    }
                    else
                    {
                        logs.WriteLog("Updating Job- " + JobMasterTbl.Job_ID, JobMasterTbl.Client_Id, Model.Created_By);
                    }
                    resultCount = JobMastertblDBOBJ.SaveChanges();
                    if (isnew)
                    {
                        logs.WriteLog("Successfully Added New Job - " + JobMasterTbl.Job_ID, JobMasterTbl.Client_Id, Model.Created_By);
                    }
                    else
                    {
                        logs.WriteLog("Successfully Updated Job - " + JobMasterTbl.Job_ID, JobMasterTbl.Client_Id, Model.Created_By);
                    }
                    jobid = JobMasterTbl.Job_ID;
                    string prefix = JobMastertblDBOBJ.Client_Master.Where(x => x.Client_ID == JobMasterTbl.Client_Id).Select(x => x.JobIDPreffix).FirstOrDefault();
                    if (prefix != null && prefix != "")
                        JobMasterTbl.JobDisplayId = JobMastertblDBOBJ.Client_Master.Where(x => x.Client_ID == JobMasterTbl.Client_Id).Select(x => x.JobIDPreffix).FirstOrDefault() + " - " + JobMasterTbl.Job_ID;
                    else
                        JobMasterTbl.JobDisplayId = JobMasterTbl.Job_ID.ToString();
                    JobMastertblDBOBJ.SaveChanges();
                }
                if (resultCount > 0)
                {
                    if (Model.NewJobAssemblyList != null && Model.NewJobAssemblyList.Count > 0)
                    {
                        using (ElectricEaseEntitiesContext JobAsTblDBOBJ = new ElectricEaseEntitiesContext())
                        {
                            try
                            {
                                int ASMresultCount;
                                Job_AssembliesDetails ASMobj = new Job_AssembliesDetails();
                                foreach (var item in Model.NewJobAssemblyList)
                                {
                                    try
                                    {
                                        ASMobj = new Job_AssembliesDetails();
                                        bool update = true;
                                        if (item.Assemblies_Name.Contains("20a  Romex Receptacle"))
                                        {
                                        }
                                        ASMobj = JobAsTblDBOBJ.Job_AssembliesDetails.Where(x => x.Job_ID == jobid && x.JobAssembly_Id == item.JobAssembly_Id).FirstOrDefault();
                                        if (ASMobj == null)
                                        {
                                            ASMobj = new Job_AssembliesDetails();
                                            update = false;
                                        }
                                        ASMobj.Client_Id = Model.Client_Id;
                                        ASMobj.Job_ID = jobid;
                                        ASMobj.Multiplier = item.Multiplier;
                                        ASMobj.Assemblies_Name = item.Assemblies_Name;
                                        ASMobj.Assemblies_Category = item.Assemblies_Category;
                                        ASMobj.severity = item.severity;
                                        ASMobj.Est_Cost_Total = item.Est_Cost_Total;
                                        ASMobj.Est_Resale_Total = item.Est_Resale_Total;
                                        ASMobj.Est_Qty = item.Estimated_Qty;
                                        ASMobj.Act_Qty_Total = item.Actual_Qty;
                                        ASMobj.Actual_Total = item.Actual_Total;
                                        if (item.labor_cost != 0)
                                        {
                                            ASMobj.Labor_Cost = item.labor_cost;
                                        }
                                        if (item.Lobor_Resale != 0)
                                        {
                                            ASMobj.Labor_Resale = item.Lobor_Resale;
                                        }
                                        if (item.Estimated_Hour != 0)
                                        {
                                            ASMobj.Labor_Est_Hours = item.Estimated_Hour;
                                        }
                                        if (item.LaborEst_CostTotal != 0)
                                        {
                                            ASMobj.Labor_Est_CostTotal = item.LaborEst_CostTotal;
                                        }
                                        if (item.LaborEst_ResaleTotal != 0)
                                        {
                                            ASMobj.Labor_Est_ResaleTotal = item.LaborEst_ResaleTotal;
                                        }
                                        if (item.labour_actual_total != 0)
                                        {
                                            ASMobj.Labor_Actual_Total = item.labour_actual_total;
                                        }
                                        if (item.Labor_Actual_Qty != 0)
                                        {
                                            ASMobj.Labor_Actual_Qty = item.Labor_Actual_Qty;
                                        }
                                        if (item.GrandCostTotal != 0)
                                        {
                                            ASMobj.GrandCostTotal = item.GrandCostTotal;
                                        }
                                        if (item.GrandResaleTotal != 0)
                                        {
                                            ASMobj.GrandResaleTotal = item.GrandResaleTotal;
                                        }
                                        if (item.PartCostTotal != 0)
                                        {
                                            ASMobj.PartsCostTotal = item.PartCostTotal;
                                        }
                                        if (item.PartResaleTotal != 0)
                                        {
                                            ASMobj.PartsResaleTotal = item.PartResaleTotal;
                                        }
                                        if (item.GrandActualTotal != 0)
                                        {
                                            ASMobj.GrandActualTotal = item.GrandActualTotal;
                                        }
                                        if (item.PartsActualTotal != 0)
                                        {
                                            ASMobj.PartsActualTotal = item.PartsActualTotal;
                                        }
                                        ASMobj.Created_By = Model.Created_By;
                                        ASMobj.Created_Date = DateTime.Now;
                                        ASMobj.Updated_By = Model.Updated_By;
                                        ASMobj.Isactive = true;
                                        if (!update)
                                        {
                                            //  ASMobj.Actual_Total = 0;
                                            //ASMobj.Act_Qty_Total = 0;
                                            if (item.GrandResaleTotal != 0)
                                            {
                                                ASMobj.GrandActualTotal = item.GrandResaleTotal;
                                            }
                                            if (item.PartResaleTotal != 0)
                                            {
                                                ASMobj.PartsActualTotal = item.PartResaleTotal;
                                            }


                                            JobAsTblDBOBJ.Job_AssembliesDetails.Add(ASMobj);
                                            logs.WriteLog("Adding New Jobs Assembly- " + ASMobj.Assemblies_Name, ASMobj.Client_Id, Model.Created_By);
                                        }
                                        else
                                            logs.WriteLog("Updating Jobs Assembly - " + ASMobj.Assemblies_Name, ASMobj.Client_Id, Model.Created_By);
                                        ASMresultCount = JobAsTblDBOBJ.SaveChanges();
                                        if (!update)
                                        {
                                            logs.WriteLog("Successfully Added New Jobs Assembly- " + ASMobj.Assemblies_Name, ASMobj.Client_Id, Model.Created_By);
                                        }
                                        else
                                        {
                                            logs.WriteLog("Successfully Updated Jobs Assembly- " + ASMobj.Assemblies_Name, ASMobj.Client_Id, Model.Created_By);
                                        }
                                        using (ElectricEaseEntitiesContext JobAsPartsDbOBJ = new ElectricEaseEntitiesContext())
                                        {
                                            Job_Assembly_Parts JobPartsTbl = new Job_Assembly_Parts();
                                            if (item.Jobsassemblyparts != null && item.Jobsassemblyparts.Count > 0)
                                            {

                                                List<Job_Assembly_Parts> lstparts = JobAsPartsDbOBJ.Job_Assembly_Parts.Where(x => x.Assemblies_Name == ASMobj.Assemblies_Name && x.Job_ID == jobid && x.JobAssembly_Id == ASMobj.JobAssembly_Id).ToList();
                                                if (lstparts != null && lstparts.Count > 0)
                                                {
                                                    foreach (var existitem in lstparts)
                                                    {

                                                        JobAsPartsDbOBJ.Job_Assembly_Parts.Remove(existitem);
                                                        int c = JobAsPartsDbOBJ.SaveChanges();
                                                    }
                                                }
                                                foreach (var PartsItem in item.Jobsassemblyparts)
                                                {
                                                    bool isupdate = true;
                                                    JobPartsTbl = new Job_Assembly_Parts();
                                                    JobPartsTbl = JobAsTblDBOBJ.Job_Assembly_Parts.Where(x => x.Assemblies_Name == PartsItem.Assemblies_Name && x.Part_Number == PartsItem.Part_Number && x.Job_ID == jobid && x.JobAssembly_Id == ASMobj.JobAssembly_Id).FirstOrDefault();
                                                    if (JobPartsTbl == null)
                                                    {
                                                        JobPartsTbl = new Job_Assembly_Parts();
                                                        isupdate = false;
                                                    }
                                                    JobPartsTbl.JobAssembly_Id = ASMobj.JobAssembly_Id;
                                                    JobPartsTbl.Job_ID = jobid;
                                                    JobPartsTbl.Assemblies_Name = PartsItem.Assemblies_Name;
                                                    JobPartsTbl.Assemblies_Category = PartsItem.Assemblies_Category;
                                                    JobPartsTbl.Client_ID = ASMobj.Client_Id;
                                                    JobPartsTbl.Part_Number = PartsItem.Part_Number;
                                                    JobPartsTbl.Part_Cost = PartsItem.Part_Cost;
                                                    JobPartsTbl.Part_Resale = PartsItem.Resale_Cost;
                                                    JobPartsTbl.Estimated_Qty = PartsItem.Estimated_Qty;
                                                    JobPartsTbl.Actual_Qty = PartsItem.Actual_Qty;
                                                    JobPartsTbl.Actual_Total = PartsItem.Actual_Total;
                                                    JobPartsTbl.IsActive = true;
                                                    JobPartsTbl.LaborUnit = PartsItem.LaborUnit;
                                                    if (!isupdate)
                                                    {
                                                        JobAsPartsDbOBJ.Job_Assembly_Parts.Add(JobPartsTbl);
                                                        logs.WriteLog("Adding New Jobs Assembly Parts - " + JobPartsTbl.Part_Number, ASMobj.Client_Id, Model.Created_By);
                                                    }
                                                    else
                                                        logs.WriteLog("Updating Jobs Assembly Parts - " + JobPartsTbl.Part_Number, ASMobj.Client_Id, Model.Created_By);
                                                    int count = JobAsPartsDbOBJ.SaveChanges();
                                                    if (!isupdate)
                                                    {
                                                        logs.WriteLog("Successfully Added New Jobs Assembly Parts - " + JobPartsTbl.Part_Number, ASMobj.Client_Id, Model.Created_By);
                                                    }
                                                    else
                                                        logs.WriteLog("Successfully Updated Jobs Assembly Parts - " + JobPartsTbl.Part_Number, ASMobj.Client_Id, Model.Created_By);

                                                }
                                            }
                                            else
                                            {
                                                Job_Master jobtbl = new Job_Master();
                                                var isnotExist = JobAsPartsDbOBJ.Job_Assembly_Parts.Any(x => x.Client_ID == Model.Client_Id && x.Job_ID == Model.Job_ID && x.Assemblies_Name == item.Assemblies_Name && x.JobAssembly_Id == ASMobj.JobAssembly_Id);
                                                if (isnotExist == false)
                                                {
                                                    if (item.Assemblies_Name.Contains("20a  Romex Receptacle"))
                                                    {
                                                    }
                                                    var assemblymaster = JobAsTblDBOBJ.Assemblies_Master.Where(x => x.Assemblies_Name.Trim() == item.Assemblies_Name.Trim() && x.Client_Id == Model.Client_Id).FirstOrDefault();
                                                    ASMobj.Labor_Cost = assemblymaster.labor_cost;
                                                    ASMobj.Labor_Resale = assemblymaster.labor_ResaleCost;
                                                    ASMobj.Labor_Est_Hours = assemblymaster.labor_EstimatedHours;
                                                    ASMobj.Labor_Est_CostTotal = assemblymaster.Labor_EstimatedCostTotal;
                                                    ASMobj.Labor_Est_ResaleTotal = assemblymaster.Lobor_EstimatedResaleTotal;
                                                    ASMobj.Labor_Actual_Total = assemblymaster.Lobor_EstimatedResaleTotal;
                                                    ASMobj.Labor_Actual_Qty = assemblymaster.labor_EstimatedHours;
                                                    //ASMobj.GrandCostTotal = assemblymaster.Grand_EstCost_Total;
                                                    //ASMobj.GrandResaleTotal = assemblymaster.Grand_EstResale_Total;
                                                    ASMobj.PartsCostTotal = assemblymaster.Parts_EstimatedCostTotal;
                                                    ASMobj.PartsResaleTotal = assemblymaster.Parts_EstimatedResaleTotal;
                                                    ASMobj.GrandActualTotal = assemblymaster.Grand_EstResale_Total;
                                                    ASMobj.PartsActualTotal = assemblymaster.Parts_EstimatedResaleTotal;
                                                    JobAsTblDBOBJ.SaveChanges();
                                                    List<Assemblies_Parts> assembliesparts = JobAsTblDBOBJ.Assemblies_Parts.Where(x => x.Assemblies_Name.Trim() == item.Assemblies_Name.Trim() && x.Client_ID == Model.Client_Id).ToList();
                                                    if (assembliesparts != null && assembliesparts.Count > 0)
                                                    {
                                                        foreach (var PartsItem in assembliesparts)
                                                        {
                                                            bool isupdate = true;
                                                            JobPartsTbl = new Job_Assembly_Parts();
                                                            JobPartsTbl = JobAsTblDBOBJ.Job_Assembly_Parts.Where(x => x.Assemblies_Name.Trim() == PartsItem.Assemblies_Name.Trim() && x.Part_Number.Trim() == PartsItem.Part_Number.Trim() && x.Job_ID == jobid && x.JobAssembly_Id == ASMobj.JobAssembly_Id).FirstOrDefault();
                                                            if (JobPartsTbl == null)
                                                            {
                                                                JobPartsTbl = new Job_Assembly_Parts();
                                                                isupdate = false;
                                                                JobPartsTbl.Actual_Qty = PartsItem.Estimated_Qty;
                                                                JobPartsTbl.Actual_Total = PartsItem.Part_Resale;
                                                                //JobPartsTbl.Actual_Total = PartsItem.PartsActualTotal;
                                                            }
                                                            else
                                                            {
                                                                JobPartsTbl.Actual_Qty = PartsItem.Actual_Qty ?? 0;
                                                                JobPartsTbl.Actual_Total = PartsItem.Actual_Total ?? 0;
                                                            }
                                                            JobPartsTbl.Job_ID = jobid;
                                                            JobPartsTbl.Client_ID = ASMobj.Client_Id;
                                                            JobPartsTbl.JobAssembly_Id = ASMobj.JobAssembly_Id;
                                                            JobPartsTbl.Assemblies_Name = PartsItem.Assemblies_Name;
                                                            JobPartsTbl.Assemblies_Category = PartsItem.Assemblies_Category;
                                                            JobPartsTbl.Part_Number = PartsItem.Part_Number;
                                                            JobPartsTbl.Part_Cost = PartsItem.Part_Cost;
                                                            JobPartsTbl.Part_Resale = PartsItem.Part_Resale;
                                                            JobPartsTbl.Estimated_Qty = PartsItem.Estimated_Qty;
                                                            JobPartsTbl.IsActive = true;
                                                            JobPartsTbl.LaborUnit = PartsItem.LaborUnit;
                                                            if (!isupdate)
                                                            {
                                                                JobAsPartsDbOBJ.Job_Assembly_Parts.Add(JobPartsTbl);
                                                                logs.WriteLog("Adding New Jobs Assembly Parts - " + JobPartsTbl.Part_Number, ASMobj.Client_Id, Model.Created_By);
                                                            }
                                                            else
                                                                logs.WriteLog("Updating Jobs Assembly Parts - " + JobPartsTbl.Part_Number, ASMobj.Client_Id, Model.Created_By);
                                                            int count = JobAsPartsDbOBJ.SaveChanges();
                                                            if (!isupdate)
                                                            {
                                                                logs.WriteLog("Successfully Added New Jobs Assembly Parts - " + JobPartsTbl.Part_Number, ASMobj.Client_Id, Model.Created_By);
                                                            }
                                                            else
                                                                logs.WriteLog("Successfully Updated Jobs Assembly Parts - " + JobPartsTbl.Part_Number, ASMobj.Client_Id, Model.Created_By);
                                                        }
                                                    }

                                                }
                                                ////To delete existing parts for the current assembly
                                                //List<Job_Assembly_Parts> lstparts = JobAsPartsDbOBJ.Job_Assembly_Parts.Where(x => x.Assemblies_Name == ASMobj.Assemblies_Name && x.Job_ID == jobid && x.JobAssembly_Id == ASMobj.JobAssembly_Id).ToList();
                                                //if (lstparts != null && lstparts.Count > 0)
                                                //{
                                                //    foreach (var existitem in lstparts)
                                                //    {

                                                //        JobAsPartsDbOBJ.Job_Assembly_Parts.Remove(existitem);
                                                //        int c = JobAsPartsDbOBJ.SaveChanges();
                                                //    }
                                                //}
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    if (Model.NewPartsListInJob != null && Model.NewPartsListInJob.Count > 0)
                    {
                        using (ElectricEaseEntitiesContext JobParttblDbObj = new ElectricEaseEntitiesContext())
                        {
                            var result = (from m in JobParttblDbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m).FirstOrDefault();
                            Job_Parts JobPartsTbl = new Job_Parts();
                            List<Job_Parts> lstparts = JobParttblDbObj.Job_Parts.Where(x => x.Job_ID == jobid).ToList();
                            if (lstparts != null)
                            {
                                foreach (var item in lstparts)
                                {
                                    JobParttblDbObj.Job_Parts.Remove(item);
                                    JobParttblDbObj.SaveChanges();
                                }
                            }
                            foreach (var item in Model.NewPartsListInJob)
                            {
                                bool isjobpartsupdate = true;
                                JobPartsTbl = new Job_Parts();
                                JobPartsTbl = JobParttblDbObj.Job_Parts.Where(x => x.Job_ID == jobid && x.Part_Number.Trim() == item.Part_Number.Trim()).FirstOrDefault();
                                if (JobPartsTbl == null)
                                {
                                    JobPartsTbl = new Job_Parts();
                                    isjobpartsupdate = false;
                                }
                                JobPartsTbl.Client_ID = Model.Client_Id;
                                JobPartsTbl.Job_ID = jobid;
                                JobPartsTbl.Part_Number = item.Part_Number;
                                JobPartsTbl.Part_Cost = item.Part_Cost;
                                JobPartsTbl.Parts_Photo = null;

                                JobPartsTbl.Part_Resale = item.Resale_Cost;
                                JobPartsTbl.Estimated_Qty = item.Estimated_Qty;
                                JobPartsTbl.Actual_Qty = item.Actual_Qty;
                                JobPartsTbl.Expected_Total = item.Expected_Total;
                                JobPartsTbl.Estimated_Total = item.Estimated_Total;
                                JobPartsTbl.Actual_Total = item.Actual_Total;
                                JobPartsTbl.IsActive = true;
                                JobPartsTbl.LaborUnit = item.LaborUnit;
                                if (!isjobpartsupdate)
                                {
                                    JobParttblDbObj.Job_Parts.Add(JobPartsTbl);
                                    logs.WriteLog("Adding Jobs Parts - " + JobPartsTbl.Part_Number, Model.Client_Id, Model.Created_By);
                                }
                                else
                                    logs.WriteLog("Updating Jobs Parts - " + JobPartsTbl.Part_Number, Model.Client_Id, Model.Created_By);
                                PartsresultCount = JobParttblDbObj.SaveChanges();
                                if (!isjobpartsupdate)
                                {
                                    logs.WriteLog("Successfully saved Jobs Parts - " + JobPartsTbl.Part_Number, Model.Client_Id, Model.Created_By);
                                }
                                else
                                    logs.WriteLog("Successfully saved Jobs Parts - " + JobPartsTbl.Part_Number, Model.Client_Id, Model.Created_By);
                            }
                        }
                    }
                    else
                    {
                        using (ElectricEaseEntitiesContext JobParttblDbObj = new ElectricEaseEntitiesContext())
                        {
                            var partslist = JobParttblDbObj.Job_Parts.Where(x => x.Job_ID == jobid && x.Client_ID == Model.Client_Id).ToList();
                            if (partslist != null && partslist.Count > 0)
                            {
                                foreach (var item in partslist)
                                {
                                    JobParttblDbObj.Job_Parts.Remove(item);
                                    JobParttblDbObj.SaveChanges();
                                }
                            }

                        }
                    }

                    using (ElectricEaseEntitiesContext ctx = new ElectricEaseEntitiesContext())
                    {
                        List<Job_Labor> lstjoblabor = ctx.Job_Labor.Where(x => x.Job_ID == jobid).ToList();
                        if (lstjoblabor != null)
                        {
                            foreach (Job_Labor item in lstjoblabor)
                            {
                                ctx.Job_Labor.Remove(item);
                                ctx.SaveChanges();
                            }
                        }
                    }
                    if (Model.NewLabourListInJob != null && Model.NewLabourListInJob.Count > 0)
                    {
                        using (ElectricEaseEntitiesContext JobLabottblDbObj = new ElectricEaseEntitiesContext())
                        {
                            Job_Labor JoblabourTbl = new Job_Labor();
                            foreach (var item in Model.NewLabourListInJob)
                            {
                                bool isjoblabourupdate = true;
                                JoblabourTbl = new Job_Labor();
                                JoblabourTbl = JobLabottblDbObj.Job_Labor.Where(x => x.Job_ID == jobid && x.Laborer_Name.Trim() == item.Laborer_Name.Trim()).FirstOrDefault();
                                if (JoblabourTbl != null)
                                {
                                    JobLabottblDbObj.Job_Labor.Remove(JoblabourTbl);
                                    JoblabourTbl = new Job_Labor();
                                    isjoblabourupdate = false;
                                }
                                else
                                {
                                    JoblabourTbl = new Job_Labor();
                                    isjoblabourupdate = false;
                                }
                                JoblabourTbl.Job_ID = jobid;
                                JoblabourTbl.Client_Id = Model.Client_Id;
                                JoblabourTbl.Laborer_Name = item.Laborer_Name;
                                JoblabourTbl.Lobor_Cost = item.Cost;

                                JoblabourTbl.Burden = item.Burden;
                                JoblabourTbl.Lobor_Resale = item.Resale_Cost;
                                JoblabourTbl.Estimated_Hour = item.Estimated_Hour;
                                JoblabourTbl.Actual_Hours = item.Actual_Hours;
                                JoblabourTbl.Expected_Total = item.Expected_Total;
                                JoblabourTbl.Estimated_Total = item.Estimated_Total;
                                JoblabourTbl.Actual_Total = item.Actual_Total;
                                JoblabourTbl.IsActive = true;
                                if (!isjoblabourupdate)
                                {
                                    JobLabottblDbObj.Job_Labor.Add(JoblabourTbl);
                                    logs.WriteLog("Saving Jobs Parts - " + JoblabourTbl.Laborer_Name, Model.Client_Id, Model.Created_By);
                                }
                                LabourresultCount = JobLabottblDbObj.SaveChanges();
                                logs.WriteLog("Successfully Saved Jobs Parts - " + JoblabourTbl.Laborer_Name, Model.Client_Id, Model.Created_By);
                            }
                        }
                    }
                    else
                    {
                        using (ElectricEaseEntitiesContext JobLabottblDbObj = new ElectricEaseEntitiesContext())
                        {
                            var Laborlist = JobLabottblDbObj.Job_Labor.Where(x => x.Job_ID == jobid && x.Client_Id == Model.Client_Id).ToList();
                            if (Laborlist != null && Laborlist.Count > 0)
                            {
                                foreach (var item in Laborlist)
                                {
                                    JobLabottblDbObj.Job_Labor.Remove(item);
                                    JobLabottblDbObj.SaveChanges();
                                }
                            }

                        }
                    }

                    if (Model.NewLegalListInJob != null && Model.NewLegalListInJob.Count > 0)
                    {
                        using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
                        {
                            List<Job_Legal> legallist = DbObj.Job_Legal.Where(x => x.Job_ID == jobid).ToList();
                            if (legallist != null && legallist.Count > 0)
                            {
                                foreach (Job_Legal joblegal in legallist)
                                {
                                    DbObj.Job_Legal.Remove(joblegal);
                                    DbObj.SaveChanges();
                                }
                            }
                            Job_Legal JobLegalTbl = new Job_Legal();
                            foreach (var item in Model.NewLegalListInJob)
                            {
                                bool isjoblegalupdate = true;
                                JobLegalTbl = new Job_Legal();
                                JobLegalTbl = DbObj.Job_Legal.Where(x => x.Job_ID == jobid && x.Legal_ID == item.Legal_ID).FirstOrDefault();
                                if (JobLegalTbl == null)
                                {
                                    JobLegalTbl = new Job_Legal();
                                    isjoblegalupdate = false;
                                }
                                var Client_ID = (from m in DbObj.Job_Master where m.Client_Email == Model.Client_Email && m.Client_Id == Model.Client_Id && m.Isactive == true select m.Client_Id).FirstOrDefault();
                                JobLegalTbl.Client_ID = Client_ID;
                                JobLegalTbl.Job_ID = jobid;
                                JobLegalTbl.Legal_ID = item.Legal_ID;
                                JobLegalTbl.Legal_Detail = item.Legal_Detail;
                                JobLegalTbl.IsActive = true;
                                if (!isjoblegalupdate)
                                {
                                    DbObj.Job_Legal.Add(JobLegalTbl);
                                    logs.WriteLog("Saving Jobs Legal - " + JobLegalTbl.Legal_ID, Model.Client_Id, Model.Created_By);
                                }
                                else
                                    logs.WriteLog("Updating Jobs Legal - " + JobLegalTbl.Legal_ID, Model.Client_Id, Model.Created_By);
                                int count = DbObj.SaveChanges();
                                if (!isjoblegalupdate)
                                {
                                    logs.WriteLog("Saved Jobs Legal - " + JobLegalTbl.Legal_ID, Model.Client_Id, Model.Created_By);
                                }
                                else
                                    logs.WriteLog("Updated Jobs Legal - " + JobLegalTbl.Legal_ID, Model.Client_Id, Model.Created_By);
                            }
                        }
                    }
                    else
                    {
                        using (ElectricEaseEntitiesContext legalctx = new ElectricEaseEntitiesContext())
                        {
                            List<Job_Legal> lstjoblegal = legalctx.Job_Legal.Where(x => x.Job_ID == jobid).ToList();
                            if (lstjoblegal != null)
                            {
                                foreach (Job_Legal item in lstjoblegal)
                                {
                                    legalctx.Job_Legal.Remove(item);
                                    legalctx.SaveChanges();
                                }
                            }
                        }

                    }
                    if (Model.DJEVQDetails != null && Model.DJEVQDetails.Count > 0)
                    {
                        using (ElectricEaseEntitiesContext DjeDb = new ElectricEaseEntitiesContext())
                        {

                            List<Job_DJE_VQ_Details> Djelist = DjeDb.Job_DJE_VQ_Details.Where(x => x.Job_ID == jobid).ToList();
                            if (Djelist != null && Djelist.Count > 0)
                            {
                                foreach (Job_DJE_VQ_Details jobDJE in Djelist)
                                {
                                    DjeDb.Job_DJE_VQ_Details.Remove(jobDJE);
                                    DjeDb.SaveChanges();
                                }
                            }
                            Job_DJE_VQ_Details jobDjeTbl = new Job_DJE_VQ_Details();
                            foreach (var item in Model.DJEVQDetails)
                            {
                                bool isjobDJEUpdate = true;
                                jobDjeTbl = new Job_DJE_VQ_Details();
                                jobDjeTbl = DjeDb.Job_DJE_VQ_Details.Where(x => x.Job_ID == jobid && x.Status_ID == item.Status_ID).FirstOrDefault();
                                if (jobDjeTbl == null)
                                {
                                    jobDjeTbl = new Job_DJE_VQ_Details();
                                    isjobDJEUpdate = false;
                                }
                                var Client_ID = (from m in DjeDb.Job_Master where m.Client_Id == Model.Client_Id && m.Client_Email == Model.Client_Email && m.Isactive == true select m.Client_Id).FirstOrDefault();
                                jobDjeTbl.Client_ID = Client_ID;
                                jobDjeTbl.Job_ID = jobid;
                                // jobDjeTbl.Status_ID = item.Status_ID;
                                jobDjeTbl.Expense = item.Expense;
                                jobDjeTbl.Vendor_Name = item.Vendor_Name;
                                jobDjeTbl.Cost = item.Cost;
                                jobDjeTbl.Profit = item.Profit;
                                jobDjeTbl.Job_DJE_VQ_Status = item.Job_DJE_VQ_Status;
                                jobDjeTbl.Resale_Total = item.Resale_Total;
                                if (item.Job_DJE_VQ_Status == "DJE")
                                {
                                    jobDjeTbl.Job_DJETotal = item.Job_DJETotal;
                                }
                                else
                                {
                                    jobDjeTbl.Job_VQTotal = item.Job_DJETotal;
                                }
                                jobDjeTbl.IsActive = true;
                                if (!isjobDJEUpdate)
                                {
                                    DjeDb.Job_DJE_VQ_Details.Add(jobDjeTbl);
                                    logs.WriteLog("Saving job Dje/VQ" + jobDjeTbl.Status_ID, Model.Client_Id, Model.Created_By);
                                }
                                else
                                    logs.WriteLog("Update Job Dje/VQ" + jobDjeTbl.Status_ID, Model.Client_Id, Model.Created_By);
                                int count = DjeDb.SaveChanges();
                                if (!isjobDJEUpdate)
                                {
                                    logs.WriteLog("Saving job Dje/VQ" + jobDjeTbl.Status_ID, Model.Client_Id, Model.Created_By);
                                }
                                else
                                {
                                    logs.WriteLog("Update Job Dje/VQ" + jobDjeTbl.Status_ID, Model.Client_Id, Model.Created_By);
                                }

                            }
                        }
                    }
                    if (Model.Job_EstimationDetails != null)
                    {
                        using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                        {
                            List<Job_EstimationDetails> Estimationlist = db.Job_EstimationDetails.Where(x => x.Job_ID == jobid).ToList();
                            if (Estimationlist != null && Estimationlist.Count > 0)
                            {
                                foreach (Job_EstimationDetails JobEstimaTION in Estimationlist)
                                {
                                    db.Job_EstimationDetails.Remove(JobEstimaTION);
                                    db.SaveChanges();
                                }
                            }
                            Job_EstimationDetails JobEastmition = new Job_EstimationDetails();

                            bool IsJob_Esatmationupdate = true;
                            JobEastmition = new Job_EstimationDetails();
                            JobEastmition = db.Job_EstimationDetails.Where(x => x.Job_ID == jobid).FirstOrDefault();
                            if (JobEastmition == null)
                            {
                                JobEastmition = new Job_EstimationDetails();
                                IsJob_Esatmationupdate = false;
                            }
                            JobEastmition.Client_ID = Model.Client_Id;
                            JobEastmition.Job_ID = jobid;
                            JobEastmition.Selected_Estimation_Type = Model.Job_EstimationDetails.Selected_Estimation_Type;
                            JobEastmition.Job_AssemblyTotal = Model.Job_EstimationDetails.Job_AssemblyTotal;
                            JobEastmition.Estimation1_AssemblyTax = Model.Job_EstimationDetails.Estimation1_AssemblyTax;
                            JobEastmition.Estimation1_AssemblyTotal = Model.Job_EstimationDetails.Estimation1_AssemblyTotal;

                            JobEastmition.Estimation2_AssemblyProfit = Model.Job_EstimationDetails.Estimation2_AssemblyProfit;
                            JobEastmition.Estimation2_AssemblySubTotal = Model.Job_EstimationDetails.Estimation2_AssemblySubTotal;
                            JobEastmition.Estimation2_AssemblyTax = Model.Job_EstimationDetails.Estimation2_AssemblyTax;
                            JobEastmition.Estimation2_AssemblyTotal = Model.Job_EstimationDetails.Estimation2_AssemblyTotal;

                            JobEastmition.Job_LaborTotal = Model.Job_EstimationDetails.Job_LaborTotal;
                            JobEastmition.Estimation1_LaborTax = Model.Job_EstimationDetails.Estimation1_LaborTax;
                            JobEastmition.Estimation1_laborTotal = Model.Job_EstimationDetails.Estimation1_laborTotal;

                            JobEastmition.Estimation2_LaborProft = Model.Job_EstimationDetails.Estimation2_LaborProft;
                            JobEastmition.Estimate2_LaborSubTotal = Model.Job_EstimationDetails.Estimate2_LaborSubTotal;
                            JobEastmition.Estimate2_LaborTax = Model.Job_EstimationDetails.Estimate2_LaborTax;
                            JobEastmition.Estimate2_LaborTotal = Model.Job_EstimationDetails.Estimate2_LaborTotal;

                            JobEastmition.Job_PartsTotal = Model.Job_EstimationDetails.Job_PartsTotal;
                            JobEastmition.Estimation1_PartsTax = Model.Job_EstimationDetails.Estimation1_PartsTax;
                            JobEastmition.Estimation1_PartsTotal = Model.Job_EstimationDetails.Estimation1_PartsTotal;

                            JobEastmition.Estimation2_PartsProfit = Model.Job_EstimationDetails.Estimation2_PartsProfit;
                            JobEastmition.Estimation2_PartsSubTotal = Model.Job_EstimationDetails.Estimation2_PartsSubTotal;
                            JobEastmition.Estimation2_PartsTax = Model.Job_EstimationDetails.Estimation2_PartsTax;
                            JobEastmition.Estimation2_PartsTotal = Model.Job_EstimationDetails.Estimation2_PartsTotal;

                            JobEastmition.Job_DJETotal = Model.Job_EstimationDetails.Job_DJETotal;
                            JobEastmition.Estimation1_DJETax = Model.Job_EstimationDetails.Estimation1_DJETax;
                            JobEastmition.Estimation1_DJETotal = Model.Job_EstimationDetails.Estimation1_DJETotal;

                            JobEastmition.Estimation2_DJEProfit = Model.Job_EstimationDetails.Estimation2_DJEProfit;
                            JobEastmition.Estimation2_DJESubTotal = Model.Job_EstimationDetails.Estimation2_DJESubTotal;
                            JobEastmition.Estimation2_DJETax = Model.Job_EstimationDetails.Estimation2_DJETax;
                            JobEastmition.Estimation2_DJETotal = Model.Job_EstimationDetails.Estimation2_DJETotal;

                            JobEastmition.Job_VQTotal = Model.Job_EstimationDetails.Job_VQTotal;
                            JobEastmition.Estimation1_VQTax = Model.Job_EstimationDetails.Estimation1_VQTax;
                            JobEastmition.Estimation1_VQTotal = Model.Job_EstimationDetails.Estimation1_VQTotal;

                            JobEastmition.Estimation2_VQProfit = Model.Job_EstimationDetails.Estimation2_VQProfit;
                            JobEastmition.Estimation2_VQSubtotal = Model.Job_EstimationDetails.Estimation2_VQSubtotal;
                            JobEastmition.Estimation2_VQTax = Model.Job_EstimationDetails.Estimation2_VQTax;
                            JobEastmition.Estimation2_VQTotal = Model.Job_EstimationDetails.Estimation2_VQTotal;


                            JobEastmition.Estimation1_GrandTotal = Model.Job_EstimationDetails.Estimation1_GrandTotal;
                            JobEastmition.Estimation1_Total = Model.Job_EstimationDetails.Estimation1_Total;

                            JobEastmition.Estimation2_GrandTotal = Model.Job_EstimationDetails.Estimation2_GrandTotal;
                            JobEastmition.Estimation2_SubTotal = Model.Job_EstimationDetails.Estimation2_SubTotal;
                            JobEastmition.Estimation2_Total = Model.Job_EstimationDetails.Estimation2_Total;
                            JobEastmition.Estimate3_Override = Model.Job_EstimationDetails.Estimate3_Override;
                            JobEastmition.Estimate4_Subtotal = Model.Job_EstimationDetails.Estimate4_Subtotal;
                            JobEastmition.Estimate4_Tax = Model.Job_EstimationDetails.Estimate4_Tax;
                            JobEastmition.Estimate4_Total = Model.Job_EstimationDetails.Estimate4_Total;
                            JobEastmition.Estimate4_Data = Model.Job_EstimationDetails.Estimate4_Data;
                            JobEastmition.IsActive = true;
                            if (!IsJob_Esatmationupdate)
                            {
                                db.Job_EstimationDetails.Add(JobEastmition);
                                logs.WriteLog("Saving job Eastmition" + JobEastmition.Job_EstimationID, Model.Client_Id, Model.Created_By);
                            }
                            else
                                logs.WriteLog("Update Job Eastmition" + JobEastmition.Job_EstimationID, Model.Client_Id, Model.Created_By);
                            int count = db.SaveChanges();
                            if (IsJob_Esatmationupdate)
                            {
                                logs.WriteLog("Saving job Eastmition" + JobEastmition.Job_EstimationID, Model.Client_Id, Model.Created_By);
                            }
                            else
                            {
                                logs.WriteLog("Update Job Eastmition" + JobEastmition.Job_EstimationID, Model.Client_Id, Model.Created_By);
                            }




                        }
                    }

                    response.Message = "“Job Details” added successfully.";
                    response.ResultCode = 1;
                    return jobid.ToString();
                }
                else
                {
                    response.Message = "Error occurred while adding “Job Details”!";
                    response.ResultCode = 0;
                    return "Job Details not added";
                }


            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While Adding/Updating Job Details [Error Msg - " + ex.Message + " ]", Model.Client_Id, Model.Created_By);
                response.Message = ex.Message;
                response.ResultCode = -1;
                return ex.Message.ToString();
            }
        }




        public string UpdateJobDetails(int JobID, int ClientId)
        {
            string state = "";
            decimal partsexptotal = 0;
            decimal partsEsttotal = 0;
            decimal partsActtotal = 0;
            decimal partsesttwocal = 0;
            decimal Assembleypartcost = 0;
            decimal Assembleypartresalecoset = 0;
            decimal partsEstimatetwotoal = 0;
            decimal Grandcosttotal = 0;
            decimal grandresalecosttotal = 0;
            decimal assemblieEstmateone = 0;
            decimal assemblieEstimatetwo = 0;
            decimal partsexp_total = 0;
            decimal AssembilesExptotal = 0;
            decimal LabExpTotal = 0;
            decimal DJEExptotal = 0;
            decimal VQExpTotal = 0;
            decimal PartsAct_Total = 0;
            decimal AssembileAct_Total = 0;
            decimal LabourAct_Total = 0;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    //Job_Master jobtbl = new Job_Master();
                    var jobtbl = db.Job_Master.Where(x => x.Job_ID == JobID && x.Client_Id == ClientId).FirstOrDefault();
                    var JobParts = db.Job_Parts.Where(x => x.Job_ID == JobID && x.Client_ID == ClientId).ToList();
                    var jobassembleparts = db.Job_Assembly_Parts.Where(x => x.Job_ID == JobID && x.Client_ID == ClientId).ToList();
                    if (JobParts.Count > 0 || jobassembleparts.Count > 0)
                    {
                        if (JobParts.Count > 0)
                        {
                            foreach (var item in JobParts)
                            {
                                var partdetiles = db.Parts_Details.Where(x => x.Client_Id == ClientId && x.Part_Number == item.Part_Number).FirstOrDefault();
                                if (partdetiles != null)
                                {
                                    if ((item.Part_Resale != partdetiles.Resale_Cost) || (item.Part_Cost != partdetiles.Cost))
                                    {
                                        item.Part_Resale = partdetiles.Resale_Cost ?? 0;
                                        item.Part_Cost = partdetiles.Cost;
                                        item.Estimated_Total = item.Part_Resale * item.Estimated_Qty;
                                        item.Expected_Total = item.Part_Cost * item.Actual_Qty;
                                        item.Actual_Total = item.Part_Resale * item.Actual_Qty;
                                        partsesttwocal = item.Part_Cost * item.Estimated_Qty;
                                        partsEsttotal += item.Estimated_Total;
                                        partsEstimatetwotoal += partsesttwocal;
                                        partsexp_total += partdetiles.Cost * item.Actual_Qty;
                                        PartsAct_Total += partdetiles.Resale_Cost * item.Actual_Qty ?? 0;
                                        int res = db.SaveChanges();
                                        if (res > 0)
                                        {
                                            jobtbl.IsPriceUpdate = true;
                                            jobtbl.PriceUpdatedDate = DateTime.Now;
                                            db.SaveChanges();
                                            state = "Success";

                                        }
                                    }
                                    else
                                    {
                                        partsesttwocal = item.Part_Cost * item.Estimated_Qty;
                                        partsEsttotal += item.Estimated_Total;
                                        partsEstimatetwotoal += partsesttwocal;
                                        partsexp_total += item.Part_Cost * item.Actual_Qty;
                                        PartsAct_Total += item.Part_Resale * item.Actual_Qty;
                                    }


                                }

                            }
                        }
                        if (jobassembleparts.Count > 0)
                        {
                            foreach (var item in jobassembleparts)
                            {
                                var partdetiles = db.Parts_Details.Where(x => x.Client_Id == ClientId && x.Part_Number == item.Part_Number).FirstOrDefault();
                                var assembilesparts = db.Assemblies_Parts.Where(x => x.Client_ID == ClientId && x.Assemblies_Name == item.Assemblies_Name && x.Part_Number == item.Part_Number).FirstOrDefault();
                                if (partdetiles != null)
                                {

                                    if ((item.Part_Resale != partdetiles.Resale_Cost) || (item.Part_Cost != partdetiles.Cost))
                                    {
                                        item.Part_Resale = partdetiles.Resale_Cost ?? 0;
                                        item.Part_Cost = partdetiles.Cost;
                                        item.Estimated_Total = partdetiles.Cost * item.Estimated_Qty;
                                        item.Expected_Total = partdetiles.Cost * item.Estimated_Qty;
                                        item.Actual_Total = partdetiles.Resale_Cost * item.Estimated_Qty ?? 0;
                                        int res = db.SaveChanges();
                                        if (assembilesparts != null)
                                        {
                                            assembilesparts.Part_Cost = partdetiles.Cost;
                                            assembilesparts.Part_Resale = partdetiles.Resale_Cost ?? 0;
                                            assembilesparts.EstimatedCost_Total = partdetiles.Cost * assembilesparts.Estimated_Qty;
                                            assembilesparts.EstimatedResale_Total = partdetiles.Resale_Cost * assembilesparts.Estimated_Qty ?? 0;
                                            db.SaveChanges();
                                        }
                                        if (res > 0)
                                        {

                                            jobtbl.IsPriceUpdate = true;
                                            jobtbl.PriceUpdatedDate = DateTime.Now;
                                            db.SaveChanges();
                                            state = "Success";
                                        }
                                        var assembileparts = db.Assemblies_Parts.Where(x => x.Client_ID == ClientId && x.Assemblies_Name == item.Assemblies_Name).ToList();
                                        if (assembileparts.Count > 0)
                                        {
                                            //var labourestcosttotal = 0;
                                            //var laborestresaletotal = 0;
                                            decimal partsestmatedcosttotal = 0;
                                            decimal partestimateresaletotal = 0;
                                            decimal grandestcosttotal = 0;
                                            decimal grandestresaletotal = 0;
                                            foreach (var aitem in assembileparts)
                                            {
                                                partsestmatedcosttotal += aitem.EstimatedCost_Total;
                                                partestimateresaletotal += aitem.EstimatedResale_Total;
                                            }
                                            var assembliemaster = db.Assemblies_Master.Where(x => x.Client_Id == ClientId && x.Assemblies_Name == item.Assemblies_Name).FirstOrDefault();
                                            if (assembliemaster != null)
                                            {
                                                assembliemaster.Parts_EstimatedCostTotal = partsestmatedcosttotal;
                                                assembliemaster.Parts_EstimatedResaleTotal = partestimateresaletotal;
                                                assembliemaster.Grand_EstCost_Total = partsestmatedcosttotal + assembliemaster.Labor_EstimatedCostTotal;
                                                assembliemaster.Grand_EstResale_Total = partestimateresaletotal + assembliemaster.Lobor_EstimatedResaleTotal;
                                                db.SaveChanges();
                                            }
                                        }


                                    }

                                }

                            }

                        }
                        var assembliedetails = db.Job_AssembliesDetails.Where(x => x.Client_Id == ClientId && x.Job_ID == JobID).ToList();
                        if (assembliedetails.Count > 0)
                        {
                            foreach (var assemitem in assembliedetails)
                            {
                                var assembliesparts = db.Job_Assembly_Parts.Where(x => x.Client_ID == ClientId && x.Job_ID == JobID && x.Assemblies_Name == assemitem.Assemblies_Name).ToList();
                                if (assembliesparts.Count > 0)
                                {
                                    Assembleypartcost = 0;
                                    Assembleypartresalecoset = 0;
                                    Grandcosttotal = 0;
                                    grandresalecosttotal = 0;
                                    foreach (var partitems in assembliesparts)
                                    {
                                        Assembleypartcost += partitems.Part_Cost * partitems.Estimated_Qty;
                                        Assembleypartresalecoset += partitems.Part_Resale * partitems.Estimated_Qty;
                                        Grandcosttotal += partitems.Part_Cost * partitems.Estimated_Qty;
                                        grandresalecosttotal += partitems.Part_Resale * partitems.Estimated_Qty;
                                    }

                                }
                                var Assembleypartcost1 = Grandcosttotal + assemitem.Labor_Est_CostTotal ?? 0;
                                var Assembleypartresalecoset1 = grandresalecosttotal + assemitem.Labor_Est_ResaleTotal ?? 0;
                                if (assemitem.PartsCostTotal != Assembleypartcost || assemitem.PartsResaleTotal != Assembleypartresalecoset)
                                {
                                    assemitem.PartsCostTotal = Assembleypartcost;
                                    assemitem.PartsResaleTotal = Assembleypartresalecoset;
                                    assemitem.Est_Cost_Total = assemitem.Multiplier * Assembleypartcost1 * assemitem.Act_Qty_Total;
                                    assemitem.Est_Resale_Total = assemitem.Multiplier * Assembleypartresalecoset1 * assemitem.Est_Qty;
                                    assemitem.Actual_Total = assemitem.Multiplier * Assembleypartresalecoset1 * assemitem.Act_Qty_Total;
                                    assemitem.GrandCostTotal = Assembleypartcost1;
                                    assemitem.GrandResaleTotal = Assembleypartresalecoset1;
                                    assemitem.GrandActualTotal = assemitem.Actual_Total ?? 0;
                                    decimal estimatetwo = assemitem.Multiplier * Assembleypartcost1 * assemitem.Est_Qty ?? 0;
                                    assemblieEstmateone += assemitem.Est_Resale_Total ?? 0;
                                    assemblieEstimatetwo += estimatetwo;
                                    AssembilesExptotal += assemitem.Multiplier * assemitem.Act_Qty_Total * Assembleypartcost1 ?? 0;
                                    AssembileAct_Total += assemitem.Multiplier * assemitem.Act_Qty_Total * Assembleypartresalecoset1 ?? 0;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    decimal estimatetwo = assemitem.Multiplier * Assembleypartcost1 * assemitem.Est_Qty ?? 0;
                                    assemblieEstmateone += assemitem.Est_Resale_Total ?? 0;
                                    assemblieEstimatetwo += estimatetwo;
                                    AssembilesExptotal += assemitem.Multiplier * assemitem.Act_Qty_Total * Assembleypartcost1 ?? 0;
                                    AssembileAct_Total += assemitem.Multiplier * assemitem.Act_Qty_Total * Assembleypartresalecoset1 ?? 0;
                                    assemitem.GrandCostTotal = Assembleypartcost1;
                                    assemitem.GrandResaleTotal = Assembleypartresalecoset1;
                                    db.SaveChanges();
                                }

                            }

                        }
                        var JobEstimation = db.Job_EstimationDetails.Where(x => x.Client_ID == ClientId && x.Job_ID == JobID).FirstOrDefault();
                        if (JobEstimation != null)
                        {
                            //estimate1
                            JobEstimation.Job_AssemblyTotal = assemblieEstmateone;
                            if (JobEstimation.Estimation1_AssemblyTax == null)
                            {
                                JobEstimation.Estimation1_AssemblyTax = 0;
                            }
                            JobEstimation.Estimation1_AssemblyTotal = (assemblieEstmateone * (JobEstimation.Estimation1_AssemblyTax / 100)) + assemblieEstmateone;
                            JobEstimation.Job_PartsTotal = partsEsttotal;
                            if (JobEstimation.Estimation1_PartsTax == null)
                            {
                                JobEstimation.Estimation1_PartsTax = 0;
                            }
                            JobEstimation.Estimation1_PartsTotal = (partsEsttotal * JobEstimation.Estimation1_PartsTax / 100) + partsEsttotal;
                            JobEstimation.Estimation1_GrandTotal = JobEstimation.Job_AssemblyTotal + JobEstimation.Job_PartsTotal + JobEstimation.Job_LaborTotal + JobEstimation.Job_DJETotal + JobEstimation.Job_VQTotal;
                            JobEstimation.Estimation1_Total = JobEstimation.Estimation1_AssemblyTotal + JobEstimation.Estimation1_PartsTotal + JobEstimation.Estimation1_laborTotal + JobEstimation.Estimation1_VQTotal + JobEstimation.Estimation1_DJETotal;
                            //estimate2
                            if (JobEstimation.Estimation2_AssemblyProfit == null)
                            {
                                JobEstimation.Estimation2_AssemblyProfit = 0;
                            }
                            JobEstimation.Estimation2_AssemblySubTotal = (assemblieEstimatetwo * (JobEstimation.Estimation2_AssemblyProfit / 100)) + assemblieEstimatetwo;
                            if (JobEstimation.Estimation2_AssemblyTax == null)
                            {
                                JobEstimation.Estimation2_AssemblyTax = 0;
                            }
                            JobEstimation.Estimation2_AssemblyTotal = (JobEstimation.Estimation2_AssemblySubTotal * (JobEstimation.Estimation2_AssemblyTax / 100)) + JobEstimation.Estimation2_AssemblySubTotal;
                            if (JobEstimation.Estimation2_PartsProfit == null)
                            {
                                JobEstimation.Estimation2_PartsProfit = 0;
                            }
                            JobEstimation.Estimation2_PartsSubTotal = (partsEstimatetwotoal * (JobEstimation.Estimation2_PartsProfit / 100)) + partsEstimatetwotoal;
                            if (JobEstimation.Estimation2_PartsTax == null)
                            {
                                JobEstimation.Estimation2_PartsTax = 0;
                            }
                            JobEstimation.Estimation2_PartsTotal = (JobEstimation.Estimation2_PartsSubTotal * (JobEstimation.Estimation2_PartsTax / 100)) + JobEstimation.Estimation2_PartsSubTotal;
                            JobEstimation.Estimation2_GrandTotal = assemblieEstimatetwo + partsEstimatetwotoal + JobEstimation.Estimation2_DJETotal + JobEstimation.Estimation2_VQTotal + JobEstimation.Estimate2_LaborTotal;
                            JobEstimation.Estimation2_SubTotal = JobEstimation.Estimation2_AssemblySubTotal + JobEstimation.Estimation2_PartsSubTotal + JobEstimation.Estimation2_VQSubtotal + JobEstimation.Estimation2_DJESubTotal + JobEstimation.Estimate2_LaborSubTotal;
                            JobEstimation.Estimation2_Total = JobEstimation.Estimation2_AssemblyTotal + JobEstimation.Estimation2_PartsTotal + JobEstimation.Estimation2_DJETotal + JobEstimation.Estimation2_VQTotal + JobEstimation.Estimate2_LaborTotal;
                            int res = db.SaveChanges();
                            var joblabour = db.Job_Labor.Where(x => x.Client_Id == ClientId && x.Job_ID == JobID).ToList();
                            if (joblabour.Count >= 0)
                            {
                                foreach (var lab in joblabour)
                                {
                                    decimal labexptotal = lab.Lobor_Cost * lab.Actual_Hours ?? 0;
                                    decimal laboractcost = lab.Lobor_Resale * lab.Actual_Hours ?? 0;
                                    LabExpTotal += labexptotal;
                                    LabourAct_Total += laboractcost;
                                }
                            }
                            var DJE = db.Job_DJE_VQ_Details.Where(x => x.Client_ID == ClientId && x.Job_ID == JobID && x.Job_DJE_VQ_Status == "DJE").ToList();
                            if (DJE.Count > 0)
                            {
                                foreach (var ditem in DJE)
                                {
                                    decimal djeexpentotal = ditem.Cost ?? 0;
                                    DJEExptotal += djeexpentotal;
                                }
                            }
                            var VQ = db.Job_DJE_VQ_Details.Where(x => x.Client_ID == ClientId && x.Job_ID == JobID && x.Job_DJE_VQ_Status == "VQ").ToList();
                            if (VQ.Count > 0)
                            {
                                foreach (var ditem in VQ)
                                {
                                    decimal djeexpentotal = ditem.Cost ?? 0;
                                    VQExpTotal += djeexpentotal;
                                }
                            }
                            var jobtotal = db.Job_Master.Where(x => x.Client_Id == ClientId && x.Job_ID == JobID).FirstOrDefault();
                            if (jobtotal != null)
                            {
                                if (JobEstimation.Selected_Estimation_Type == 1)
                                {

                                    decimal jobexpenstotal = AssembilesExptotal + partsexp_total + LabExpTotal + DJEExptotal + VQExpTotal;
                                    jobtotal.Job_EstimatedTotal = JobEstimation.Estimation1_GrandTotal;
                                    jobtotal.Job_ActualTotal = PartsAct_Total + LabourAct_Total + AssembileAct_Total;
                                    jobtotal.Profit_Loss_Total = jobtotal.Job_EstimatedTotal - jobexpenstotal;
                                }
                                else if (JobEstimation.Selected_Estimation_Type == 2)
                                {
                                    decimal jobexpenstotal = AssembilesExptotal + partsexp_total + LabExpTotal + DJEExptotal + VQExpTotal;
                                    jobtotal.Job_EstimatedTotal = JobEstimation.Estimation2_SubTotal;
                                    jobtotal.Job_ActualTotal = PartsAct_Total + LabourAct_Total + AssembileAct_Total;
                                    jobtotal.Profit_Loss_Total = jobtotal.Job_EstimatedTotal - jobexpenstotal;
                                }

                                db.SaveChanges();
                            }
                            if (res > 0 || res == 0)
                            {

                                jobtbl.IsPriceUpdate = true;
                                jobtbl.PriceUpdatedDate = DateTime.Now;
                                db.SaveChanges();
                                state = "Success";
                            }


                        }
                    }
                    else if (JobParts.Count == 0 || jobassembleparts.Count == 0)
                    {
                        state = "No parts for this job";
                    }

                }
                return state;
            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }
        /// <summary>
        /// To get all job assemblies using the jobid
        /// </summary>
        /// <param name="Job_ID"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        /// 
        public ServiceResult<Job_MasterInfo> GetJobsAssemblies(int Job_ID, int ClientID)
        {
            ServiceResult<Job_MasterInfo> model = new ServiceResult<Job_MasterInfo>();
            using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
            {
                model.Data = new Job_MasterInfo();
                model.Data.Jobsassemblyparts = new List<jobassemblyparts>();
                List<Job_AssembliesDetails> lstassemblydetails = DbObj.Job_AssembliesDetails.Where(x => x.Job_ID == Job_ID && x.Client_Id == ClientID && x.Isactive == true).ToList();
                foreach (var item in lstassemblydetails)
                {
                    jobassemblyparts jobassem = new jobassemblyparts();
                    jobassem.severity = item.severity;
                    jobassem.JobAssembly_Id = item.JobAssembly_Id;
                    jobassem.Multiplier = item.Multiplier;
                    jobassem.Actual_Qty = item.Act_Qty_Total == null ? 0 : item.Act_Qty_Total.Value;
                    jobassem.Actual_Total = item.Actual_Total == null ? 0 : item.Actual_Total.Value;
                    jobassem.Assemblies_Category = item.Assemblies_Category;
                    jobassem.Assemblies_Name = item.Assemblies_Name;
                    jobassem.EstCost_Total = item.Est_Cost_Total == null ? 0 : item.Est_Cost_Total.Value;
                    jobassem.Estimated_Qty = item.Est_Qty == null ? 0 : item.Est_Qty.Value;
                    jobassem.EstResaleCost_Total = item.Est_Resale_Total == null ? 0 : item.Est_Resale_Total.Value;
                    jobassem.Part_Cost = item.PartsCostTotal == null ? 0 : item.PartsCostTotal.Value;
                    jobassem.PartResaleTotal = item.PartsResaleTotal == null ? 0 : item.PartsResaleTotal.Value;
                    jobassem.labor_EstimatedHours = item.Labor_Est_Hours == null ? 0 : item.Labor_Est_Hours.Value;
                    jobassem.GrandCostTotal = item.GrandCostTotal ?? 0;
                    jobassem.GrandResaleTotal = item.GrandResaleTotal ?? 0;
                    jobassem.GrandActualTotal = item.GrandActualTotal;
                    jobassem.assemblypartsCount = (from aspartstbl in DbObj.Assemblies_Parts
                                                   join PMtbl in DbObj.Parts_Details on aspartstbl.Part_Number equals PMtbl.Part_Number
                                                   where aspartstbl.Client_ID == ClientID && aspartstbl.Assemblies_Name == item.Assemblies_Name && aspartstbl.IsActive == true && PMtbl.IsActive == true
                                                   select PMtbl
                                             ).Distinct().Count();
                    model.Data.Jobsassemblyparts.Add(jobassem);
                }
            }
            return model;
        }
        /// <summary>
        /// To get all job Labour using the jobid
        /// </summary>
        /// <param name="Job_ID"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public List<Labor_DetailsInfoList> GetJobsLabour(int Job_ID, int ClientID)
        {
            List<Labor_DetailsInfoList> model = new List<Labor_DetailsInfoList>();
            using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
            {
                List<Job_Labor> lstlabour = DbObj.Job_Labor.Where(x => x.Job_ID == Job_ID && x.Client_Id == ClientID).ToList();
                foreach (var item in lstlabour)
                {
                    Labor_DetailsInfoList joblabor = new Labor_DetailsInfoList();
                    joblabor.Laborer_Name = item.Laborer_Name;
                    joblabor.Cost = item.Lobor_Cost;
                    joblabor.Resale_Cost = item.Lobor_Resale;
                    joblabor.Estimated_Hour = item.Estimated_Hour;
                    joblabor.Actual_Hours = item.Actual_Hours;
                    joblabor.Actual_Total = item.Actual_Total;
                    joblabor.isChekced = item.IsActive ?? false;
                    model.Add(joblabor);
                }
            }
            return model;
        }
        /// <summary>
        /// To get all job Parts
        /// </summary>
        /// <param name="Job_ID"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public List<Parts_DetailsInfoList> GetJobsParts(int Job_ID, int ClientID)
        {
            List<Parts_DetailsInfoList> lstparts = new List<Parts_DetailsInfoList>();
            using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
            {
                var parts = db.Job_Parts.Where(x => x.Job_ID == Job_ID).ToList();
                if (parts != null)
                {
                    foreach (var item in parts)
                    {
                        Parts_DetailsInfoList partslst = new Parts_DetailsInfoList();
                        partslst.Actual_Qty = item.Actual_Qty;
                        partslst.Actual_Total = item.Actual_Total;
                        partslst.Part_Cost = item.Part_Cost;
                        partslst.Part_Number = item.Part_Number;
                        partslst.Description = db.Parts_Details.Where(x => x.Part_Number == item.Part_Number && x.Client_Id == item.Client_ID).Select(x => x.Description).FirstOrDefault();
                        partslst.Resale_Cost = item.Part_Resale;
                        partslst.Estimated_Qty = item.Estimated_Qty;
                        partslst.Estimated_Total = item.Estimated_Total;
                        partslst.Expected_Total = item.Expected_Total;
                        partslst.LaborUnit = item.LaborUnit;
                        lstparts.Add(partslst);
                    }
                }
            }

            return lstparts;
        }
        /// <summary>
        /// To get job assembly parts
        /// </summary>
        /// <param name="Job_ID"></param>
        /// <param name="name"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public List<AssembliesParts_DetailsInfoList> GetJobsAssembliesParts(int Job_ID, string name, int ClientID, int AssemblyJobId)
        {
            List<AssembliesParts_DetailsInfoList> lstassemblyparts = new List<AssembliesParts_DetailsInfoList>();
            using (ElectricEaseEntitiesContext DbObj = new ElectricEaseEntitiesContext())
            {
                List<Job_Assembly_Parts> lstjobparts = DbObj.Job_Assembly_Parts.Where(x => x.Client_ID == ClientID && x.Job_ID == Job_ID && x.Assemblies_Name == name && x.JobAssembly_Id == AssemblyJobId).ToList();
                foreach (var item in lstjobparts)
                {
                    AssembliesParts_DetailsInfoList assemparts = new AssembliesParts_DetailsInfoList();
                    assemparts.Part_Number = item.Part_Number;
                    assemparts.Part_Category = DbObj.Parts_Details.Where(x => x.Part_Number == item.Part_Number && x.Client_Id == item.Client_ID).Select(x => x.Part_Category).FirstOrDefault();
                    assemparts.Estimated_Qty = item.Estimated_Qty;
                    assemparts.EstCost_Total = Decimal.Round(item.Part_Cost * item.Estimated_Qty, 2);
                    assemparts.EstResaleCost_Total = Decimal.Round(item.Part_Resale * item.Estimated_Qty, 2);
                    assemparts.Actual_Qty = item.Actual_Qty;
                    assemparts.Actual_Total = item.Actual_Total;
                    assemparts.Part_Cost = item.Part_Cost;
                    assemparts.LaborUnit = item.LaborUnit ?? 0;
                    assemparts.Resale_Cost = item.Part_Resale;
                    assemparts.Assemblies_Name = item.Assemblies_Name;
                    assemparts.Assemblies_Category = item.Assemblies_Category;
                    assemparts.Description = DbObj.Parts_Details.Where(x => x.Part_Number == item.Part_Number && x.Client_Id == item.Client_ID).Select(x => x.Description).FirstOrDefault();

                    lstassemblyparts.Add(assemparts);
                }
                if (lstjobparts == null || lstjobparts.Count == 0)
                {
                    List<Assemblies_Parts> lstparts = DbObj.Assemblies_Parts.Where(x => x.Assemblies_Name == name && x.Client_ID == ClientID && x.IsActive == true).ToList();
                    foreach (var item in lstparts)
                    {
                        AssembliesParts_DetailsInfoList assemparts = new AssembliesParts_DetailsInfoList();
                        assemparts.Part_Number = item.Part_Number;
                        assemparts.Part_Category = DbObj.Parts_Details.Where(x => x.Part_Number == item.Part_Number).Select(x => x.Part_Category).FirstOrDefault();
                        assemparts.Estimated_Qty = item.Estimated_Qty;
                        assemparts.EstCost_Total = Decimal.Round(item.Part_Cost * item.Estimated_Qty, 2);
                        assemparts.EstResaleCost_Total = Decimal.Round(item.Part_Resale * item.Estimated_Qty, 2);
                        assemparts.Actual_Qty = item.Actual_Qty ?? 0;
                        assemparts.Actual_Total = item.Actual_Total ?? 0;
                        assemparts.Part_Cost = item.Part_Cost;
                        assemparts.LaborUnit = item.LaborUnit ?? 0;
                        assemparts.Resale_Cost = item.Part_Resale;
                        assemparts.Assemblies_Name = item.Assemblies_Name;
                        assemparts.Assemblies_Category = item.Assemblies_Category;
                        assemparts.Description = DbObj.Parts_Details.Where(x => x.Part_Number == item.Part_Number && x.Client_Id == item.Client_ID).Select(x => x.Description).FirstOrDefault();

                        lstassemblyparts.Add(assemparts);
                    }
                }
            }
            return lstassemblyparts;
        }
        /// <summary>
        /// To get particular job assembly details
        /// </summary>
        /// <param name="Job_ID"></param>
        /// <param name="name"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public Assembly_MasterInfo getjobassemblydetais(int Job_ID, string name, int ClientID, int AssemblyID)
        {
            Assembly_MasterInfo assemmaster = new Assembly_MasterInfo();
            using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
            {
                var assemblymaster = db.Job_AssembliesDetails.Where(x => x.Client_Id == ClientID && x.Job_ID == Job_ID && x.Assemblies_Name == name && x.JobAssembly_Id == AssemblyID).FirstOrDefault();
                assemmaster.Assemblies_Name = name;
                if (assemblymaster == null)
                {
                    ServiceResult<Assembly_MasterInfo> assem = new AssembliesMasterData().GetAssembliesListDetails(name, ClientID);
                    assemmaster = assem.Data;
                    return assemmaster;
                }
                assemmaster.Assemblies_Category = assemblymaster.Assemblies_Category;
                assemmaster.labor_cost = assemblymaster.Labor_Cost ?? 0;
                assemmaster.Lobor_Resale = assemblymaster.Labor_Resale ?? 0;
                assemmaster.labour_actual_total = assemblymaster.Labor_Actual_Total ?? 0;
                assemmaster.labour_actual_hours = Convert.ToInt32(assemblymaster.Labor_Actual_Qty ?? 0);
                assemmaster.LaborEst_ResaleTotal = assemblymaster.Labor_Est_ResaleTotal ?? 0;
                assemmaster.LaborEst_CostTotal = assemblymaster.Labor_Est_CostTotal ?? 0;
                assemmaster.labor_ResaleCost = assemblymaster.Labor_Resale ?? 0;
                assemmaster.Estimated_Hour = assemblymaster.Labor_Est_Hours ?? 0;
                assemmaster.PartCostTotal = assemblymaster.PartsCostTotal ?? 0;
                assemmaster.PartResaleTotal = assemblymaster.PartsResaleTotal ?? 0;
                assemmaster.GrandCostTotal = assemblymaster.GrandCostTotal ?? 0;
                assemmaster.GrandResaleTotal = assemblymaster.GrandResaleTotal ?? 0;
                assemmaster.PartsActualTotal = assemblymaster.PartsActualTotal;
                assemmaster.GrandActualTotal = assemblymaster.GrandActualTotal;
            }

            return assemmaster;
        }
        /// <summary>
        /// To delete an job assembly
        /// </summary>
        /// <param name="Job_ID"></param>
        /// <param name="Id"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public string deleteJobAssembly(int Job_ID, int Id, int ClientID, string username)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Job_Assembly_Parts> lstassemparts = db.Job_Assembly_Parts.Where(x => x.JobAssembly_Id == Id && x.Job_ID == Job_ID).ToList();
                    foreach (var item in lstassemparts)
                    {
                        db.Job_Assembly_Parts.Remove(item);
                        db.SaveChanges();
                    }
                    db.Job_AssembliesDetails.Remove(db.Job_AssembliesDetails.Where(x => x.JobAssembly_Id == Id && x.Job_ID == Job_ID).FirstOrDefault());
                    db.SaveChanges();
                    logs.WriteLog("Successfully Deleted the Jobs Assembly - " + Id, ClientID, username);
                }
                return "Success";
            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While Deleting the Jobs Assembly - " + Id + " [Error Msg - " + ex.Message + " ]", ClientID, username);
                return "error";
            }
        }
        /// <summary>
        /// To get job legal list
        /// </summary>
        /// <param name="jobid"></param>
        /// <param name="clientid"></param>
        /// <returns></returns>
        public List<Legal_DetailsInfoList> getjobLegallist(int jobid, int clientid)
        {
            List<Legal_DetailsInfoList> legallist = new List<Legal_DetailsInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Job_Legal> joblegal = db.Job_Legal.Where(x => x.Client_ID == clientid && x.Job_ID == jobid).ToList();
                    foreach (var item in joblegal)
                    {
                        Legal_DetailsInfoList legalinfo = new Legal_DetailsInfoList();
                        legalinfo.Legal_ID = item.Legal_ID;
                        legalinfo.Legal_Category = item.Job_Legal_ID.ToString();
                        legalinfo.Legal_Detail = item.Legal_Detail;
                        legalinfo.isChekced = true;
                        legallist.Add(legalinfo);
                    }
                    return legallist;
                }
            }
            catch
            {
                return legallist;
            }
        }
        /// <summary>
        /// To get all the clients from Job master
        /// </summary>
        /// <returns></returns>
        public List<clients> getallJobClients(int ClientID)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<clients> lstclients = (from c in db.Job_Master
                                                where c.Client_Name != null && c.Client_Name != "" && c.Client_Id == ClientID
                                                select new clients
                                                {
                                                    name = c.Client_Name
                                                }).Distinct().ToList();
                    return lstclients;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// To get all the clients from Job master
        /// </summary>
        /// <returns></returns>
        public List<clients> getallJobClientsLocation(int ClientID)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<clients> lstclients = (from c in db.Job_Master
                                                where c.Work_Location != null && c.Work_Location.Trim() != "" && c.Client_Id == ClientID
                                                select new clients
                                                {
                                                    name = c.Work_Location
                                                }).Distinct().ToList();
                    return lstclients;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// To get job client details - On select dropdown
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public clients getjobclientdetails(string name)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Job_Master jobmaster = db.Job_Master.Where(x => x.Client_Name == name).FirstOrDefault();
                    clients client = new clients();
                    client.name = jobmaster.Client_Name;
                    client.Client_Address = jobmaster.Client_Address;
                    client.Client_Address2 = jobmaster.Client_Address2;
                    client.Client_City = jobmaster.Client_City;
                    client.Client_ContactPerson = jobmaster.Client_ContactPerson;
                    client.Client_Email = jobmaster.Client_Email;
                    client.Client_Fax = jobmaster.Client_Fax;
                    client.Client_Mobile = jobmaster.Client_Mobile;
                    client.Client_Phone = jobmaster.Client_Phone;
                    client.Client_State = jobmaster.Client_State;
                    client.Client_Zip = jobmaster.Client_ZipCode;
                    return client;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// To get job clients location - On select dropdown
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public clients getjobclientlocationdetails(string name)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Job_Master jobmaster = db.Job_Master.Where(x => x.Work_Location == name).FirstOrDefault();
                    clients client = new clients();
                    client.name = jobmaster.Client_Name;
                    client.Work_Address = jobmaster.Work_Address;
                    client.Work_Address2 = jobmaster.Work_Address2;
                    client.Work_City = jobmaster.Work_City;
                    client.Work_State = jobmaster.Work_State;
                    client.Work_Location = jobmaster.Work_Location;
                    client.Work_ZipCode = jobmaster.Work_ZipCode;
                    return client;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// To save the job attchment
        /// </summary>
        /// <param name="lstjobattach"></param>
        /// <returns></returns>
        public string savejobattachment(List<Job_Attachments> lstjobattach)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    if (lstjobattach.Count > 0)
                    {
                        int njobid = lstjobattach[0].Job_ID;
                        foreach (Job_Attachments job in lstjobattach)
                        {
                            db.Job_Attachments.Add(job);
                            logs.WriteLog("Uploading Attachment for Job - " + njobid, job.Client_Id, "");
                            db.SaveChanges();
                            logs.WriteLog("Successfully Uploaded Attachment for Job - " + njobid, job.Client_Id, "");
                        }
                    }
                    return "Success";
                }
            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While uploading attachment for Job - " + lstjobattach[0].Job_ID + " [Error Msg - " + ex.Message + " ]", lstjobattach[0].Client_Id, "");
                return "error";
            }
        }

        public List<Job_Attachments> getattchments(int jobid)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    return db.Job_Attachments.Where(x => x.Job_ID == jobid).ToList();
                }
            }
            catch
            {
                return null;
            }
        }
        public string deleteJobattachment(int attachmentid, int ClientId, string username)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    db.Job_Attachments.Remove(db.Job_Attachments.Where(x => x.Attachment_ID == attachmentid).FirstOrDefault());
                    db.SaveChanges();
                    logs.WriteLog("Deleted the Jobs Attachment - " + attachmentid, ClientId, username);
                    return "success";
                }
            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] while deleting the Jobs Attachment - " + attachmentid + " [Error Msg - " + ex.Message + " ]", ClientId, username);
                return "error";
            }
        }
        public ServiceResultList<SOWInfoList> GetPreviousSOWList(int ClientID)
        {
            ServiceResultList<SOWInfoList> response = new ServiceResultList<SOWInfoList>();
            List<SOWInfoList> result = new List<SOWInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    result = db.Job_Master.Where(m => m.Client_Id == ClientID && m.Isactive == true).Select(m => new SOWInfoList()
                    {
                        Doing_What = m.Doing_What
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
        public ServiceResultList<SOWInfoList> GetPreviousJobDesc(int ClientID)
        {
            ServiceResultList<SOWInfoList> response = new ServiceResultList<SOWInfoList>();
            List<SOWInfoList> result = new List<SOWInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    result = db.Job_Master.Where(m => m.Client_Id == ClientID && m.Isactive == true).Select(m => new SOWInfoList()
                    {
                        Doing_What = m.Job_Description
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
        public ServiceResultList<SOWInfoList> GetPreviousDirection(int ClientID)
        {
            ServiceResultList<SOWInfoList> response = new ServiceResultList<SOWInfoList>();
            List<SOWInfoList> result = new List<SOWInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    result = db.Job_Master.Where(m => m.Client_Id == ClientID && m.Isactive == true).Select(m => new SOWInfoList()
                    {
                        Doing_What = m.Directions_To
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
        public ServiceResult<int> DeleteJobDetails(int ClientId, int JobId, string username)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            Job_Master jobtblOBJ = new Job_Master();

            try
            {

                using (ElectricEaseEntitiesContext databaseobj = new ElectricEaseEntitiesContext())
                {
                    jobtblOBJ = (from m in databaseobj.Job_Master where m.Client_Id == ClientId && m.Isactive == true select m).FirstOrDefault();
                    if (jobtblOBJ != null)
                    {

                        //Delete Job Attachments
                        using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                        {
                            List<Job_Attachments> IsExit = db.Job_Attachments.Where(x => x.Client_Id == ClientId && x.Job_ID == JobId).ToList();
                            if (IsExit != null && IsExit.Count > 0)
                            {
                                foreach (Job_Attachments jobattach in IsExit)
                                {
                                    db.Job_Attachments.Remove(jobattach);
                                    db.SaveChanges();
                                }
                            }
                        }
                        //Delete Job Legal Details
                        using (ElectricEaseEntitiesContext dbJobLegal = new ElectricEaseEntitiesContext())
                        {

                            List<Job_Legal> lstJoblegal = dbJobLegal.Job_Legal.Where(x => x.Client_ID == ClientId && x.Job_ID == JobId).ToList();
                            if (lstJoblegal != null && lstJoblegal.Count > 0)
                            {
                                foreach (var item in lstJoblegal)
                                {
                                    dbJobLegal.Job_Legal.Remove(item);
                                    dbJobLegal.SaveChanges();
                                }
                            }


                        }

                        //Delete Job Labor Details
                        using (ElectricEaseEntitiesContext dbJobLabor = new ElectricEaseEntitiesContext())
                        {
                            List<Job_Labor> lstJoblabor = dbJobLabor.Job_Labor.Where(x => x.Client_Id == ClientId && x.Job_ID == JobId).ToList();
                            if (lstJoblabor != null && lstJoblabor.Count > 0)
                            {
                                foreach (var item in lstJoblabor)
                                {
                                    dbJobLabor.Job_Labor.Remove(item);
                                    dbJobLabor.SaveChanges();
                                }
                            }


                        }

                        //Delete Job Parts Details
                        using (ElectricEaseEntitiesContext dbJobParts = new ElectricEaseEntitiesContext())
                        {
                            List<Job_Parts> lstJobparts = dbJobParts.Job_Parts.Where(x => x.Client_ID == ClientId && x.Job_ID == JobId).ToList();
                            if (lstJobparts != null && lstJobparts.Count > 0)
                            {
                                foreach (var item in lstJobparts)
                                {
                                    dbJobParts.Job_Parts.Remove(item);
                                    dbJobParts.SaveChanges();
                                }
                            }


                        }
                        //Delete Job Assemblies parts Details
                        using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                        {
                            List<Job_Assembly_Parts> lstassemparts = db.Job_Assembly_Parts.Where(x => x.Client_ID == ClientId && x.Job_ID == JobId).ToList();
                            if (lstassemparts != null && lstassemparts.Count > 0)
                            {
                                foreach (var item in lstassemparts)
                                {
                                    db.Job_Assembly_Parts.Remove(item);
                                    db.SaveChanges();
                                }

                            }

                        }

                        //Delete job Assemblies Details
                        using (ElectricEaseEntitiesContext ASdb = new ElectricEaseEntitiesContext())
                        {
                            List<Job_AssembliesDetails> lsAsemblies = ASdb.Job_AssembliesDetails.Where(x => x.Client_Id == ClientId && x.Job_ID == JobId).ToList();
                            if (lsAsemblies != null && lsAsemblies.Count > 0)
                            {
                                foreach (var item in lsAsemblies)
                                {
                                    ASdb.Job_AssembliesDetails.Remove(item);
                                    int AsResultCount = ASdb.SaveChanges(); ;
                                }
                                // ASdb.Job_AssembliesDetails.Remove(ASdb.Job_AssembliesDetails.Where(x => x.Client_Id == ClientId && x.Job_ID == JobId).FirstOrDefault());

                            }

                        }
                        using (ElectricEaseEntitiesContext JobCalenderdb = new ElectricEaseEntitiesContext())
                        {
                            List<Job_Calendar> JobCalenderList = JobCalenderdb.Job_Calendar.Where(x => x.Client_Id == ClientId && x.JobId == JobId).ToList();
                            if (JobCalenderList != null && JobCalenderList.Count > 0)
                            {
                                foreach (var item in JobCalenderList)
                                {
                                    JobCalenderdb.Job_Calendar.Remove(item);
                                    int AsResultCount = JobCalenderdb.SaveChanges();
                                }
                                // ASdb.Job_AssembliesDetails.Remove(ASdb.Job_AssembliesDetails.Where(x => x.Client_Id == ClientId && x.Job_ID == JobId).FirstOrDefault());

                            }

                        }
                        using (ElectricEaseEntitiesContext Jobdje = new ElectricEaseEntitiesContext())
                        {
                            List<Job_DJE_VQ_Details> lstjobsjobs = Jobdje.Job_DJE_VQ_Details.Where(x => x.Client_ID == ClientId && x.Job_ID == JobId).ToList();
                            if (lstjobsjobs != null && lstjobsjobs.Count > 0)
                            {
                                foreach (var item in lstjobsjobs)
                                {
                                    Jobdje.Job_DJE_VQ_Details.Remove(item);
                                    Jobdje.SaveChanges();
                                }
                            }

                        }
                        using (ElectricEaseEntitiesContext Jobestimate = new ElectricEaseEntitiesContext())
                        {
                            List<Job_EstimationDetails> lstestimatejobs = Jobestimate.Job_EstimationDetails.Where(x => x.Client_ID == ClientId && x.Job_ID == JobId).ToList();
                            if (lstestimatejobs != null && lstestimatejobs.Count > 0)
                            {
                                foreach (var item in lstestimatejobs)
                                {
                                    Jobestimate.Job_EstimationDetails.Remove(item);
                                    Jobestimate.SaveChanges();
                                }
                            }

                        }
                        //Delete Job information in Job Master Table
                        using (ElectricEaseEntitiesContext DbOBJ = new ElectricEaseEntitiesContext())
                        {
                            Job_Master job = DbOBJ.Job_Master.Where(x => x.Client_Id == ClientId && x.Job_ID == JobId && x.Isactive == true).FirstOrDefault();
                            var chkTimeSheets = DbOBJ.TimeSheet_Details.Where(a => a.Job_ID == JobId).ToList();
                            if (job != null && chkTimeSheets.Any())
                            {
                                string timeSheets = string.Join(",", chkTimeSheets.Select(a => a.TimeSheet_Master.TimeSheetCode).Distinct().ToList());
                                logs.WriteLog("Job Id " + job.Job_ID + " can't be deleted as it is associated to " + timeSheets + " " + JobId, ClientId, username);
                                response.ResultCode = 0;
                                response.Message = "Job Id " + job.Job_ID + " can't be deleted as it is associated to " + timeSheets + " ";
                                return response;
                            }
                            DbOBJ.Job_Master.Remove(job);
                            int resultCount = DbOBJ.SaveChanges();
                        }

                        logs.WriteLog("Successfully Deleted Job - " + JobId, ClientId, username);
                        response.ResultCode = 1;
                        response.Message = "“Job Details” deleted successfully.";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = 0;
                        response.Message = "“Data” doesn't exist!";
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While Deleting Job - " + JobId + " [Error Msg - " + ex.Message + "]", ClientId, username);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;

            }
        }

        //Get Calender Events
        public ServiceResultList<CalendarEventItem> GetCalenderEvents(int ClientID, string userid, bool CreatedBy_SuperUser)
        {
            ServiceResultList<CalendarEventItem> response = new ServiceResultList<CalendarEventItem>();
            List<Job_Calendar> res = new List<Job_Calendar>();
            try
            {

                using (ElectricEaseEntitiesContext Db = new ElectricEaseEntitiesContext())
                {
                    //if (CreatedBy_SuperUser == false)
                    //{ 
                    //response.ListData = (from m in Db.Job_Calendar
                    //                     join jobtbl in Db.Job_Master on m.JobId equals jobtbl.Job_ID
                    //                     where m.Client_Id == ClientID && m.AssignTo == userid && m.StartDate != null && m.EndDate != null && m.IsActive == true
                    //                     select new CalendarEventItem()
                    //                     {
                    //                         ID = m.JobId,
                    //                         Job_AssignedUser = m.AssignTo,
                    //                         JobStatus = jobtbl.Job_Status,
                    //                         Title = jobtbl.Client_Name,
                    //                         Start = m.StartDate ?? DateTime.Now,
                    //                         End = m.EndDate ?? DateTime.Now,
                    //                         Starttime = m.StartTime,
                    //                         Endtime = m.EndTime,
                    //                         IsAllDayEvent = true,
                    //                         Fullday = m.FullDay
                    //                     }).ToList();
                    //}
                    //else {
                    //Db.Job_Calendar.Where(x => x.Client_Id == ClientID && x.AssignTo == userid).ToList();

                    response.ListData = (from m in Db.Job_Master
                                         join jobcalendertbl in Db.Job_Calendar on m.Job_ID equals jobcalendertbl.JobId
                                         join cmtbl in Db.Client_Master on m.Client_Id equals cmtbl.Client_ID
                                         //join Atbl in Db.Account_Master on m.Client_Id equals Atbl.Client_ID
                                         where m.Client_Id == ClientID && m.Isactive == true && cmtbl.IsActive == true && jobcalendertbl.IsActive == true
                                         && jobcalendertbl.StartDate != null && jobcalendertbl.EndDate != null
                                         select new CalendarEventItem()
                                         {
                                             ID = m.Job_ID,
                                             Job_AssignedUser = jobcalendertbl.AssignTo,
                                             JobStatus = m.Job_Status,
                                             Title = m.Client_Name,
                                             Start = jobcalendertbl.StartDate ?? DateTime.Now,
                                             End = jobcalendertbl.EndDate ?? DateTime.Now,
                                             Starttime = jobcalendertbl.StartTime,
                                             Endtime = jobcalendertbl.EndTime,
                                             IsAllDayEvent = true,
                                             Fullday = jobcalendertbl.FullDay

                                             // backgroundcolor=AcUserTbl.UserColor

                                         }).ToList();
                    //}

                    foreach (var item in response.ListData)
                    {

                        if (item.Starttime != TimeSpan.MinValue && item.Endtime != TimeSpan.MinValue && item.Starttime != null && item.Endtime != null)
                        {

                            item.Start = item.Start.Add(item.Starttime ?? TimeSpan.MinValue);
                            item.End = item.End.Add(item.Endtime ?? TimeSpan.MinValue);
                            //DateTime timeform = DateTime.Today.Add(item.Starttime?? TimeSpan.MinValue);
                            item.sTime = item.Start.ToString("hh:mm tt");
                            item.eTime = item.End.ToString("hh:mm tt");



                            if (item.Starttime > item.Endtime && item.Starttime != item.Endtime)
                            {
                                item.TotalHours = (item.Starttime - item.Endtime).Value.TotalHours.ToString("N2");
                                TimeSpan time = (item.Starttime - item.Endtime).Value;
                            }
                            if (item.Endtime > item.Starttime && item.Starttime != item.Endtime)
                            {
                                item.TotalHours = (item.Endtime - item.Starttime).Value.TotalHours.ToString("N2");
                                TimeSpan time = (item.Endtime - item.Starttime).Value;
                                item.TotalHours = time.ToString("hh") + "H :";
                                item.TotalHours += time.ToString("mm") + "M ";
                                // item.TotalHours += time.ToString("ss") + "S";
                            }

                        }
                        if (item.Job_AssignedUser != null)
                        {
                            var mycolor = (from m in Db.Account_Master where m.Client_ID == ClientID && m.User_ID == item.Job_AssignedUser select m.UserColor).FirstOrDefault();
                            item.backgroundcolor = mycolor;


                        }
                        else
                        {
                            item.backgroundcolor = "#231f20";
                        }

                        if (item.JobStatus.Trim() == "Closed")
                        {
                            item.border = "#CC0000";
                        }
                        else
                        {
                            item.border = item.backgroundcolor;
                        }
                    }

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


        //Get My UserList
        public List<MyUserList> GetMyUserList(int ClientID, string comment = "")
        {
            List<MyUserList> response = new List<MyUserList>();
            List<MyUserList> UserList = new List<MyUserList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    if (comment == "")
                    {
                        UserList = db.Account_Master.Where(m => m.IsActive == true && m.CreatedBy_SuperUser != true && m.Client_ID == ClientID).Select(m => new MyUserList()
                        {
                            name = m.User_ID
                        }).Distinct().ToList();
                    }
                    else
                    {
                        UserList = db.Account_Master.Where(m => m.IsActive == true && m.Client_ID == ClientID).Select(m => new MyUserList()
                        {
                            name = m.User_ID
                        }).Distinct().ToList();
                    }
                    if (UserList.Count > 0 && UserList != null)
                    {

                        return UserList;
                    }
                    else
                    {
                        //response.ListData = UserList;

                        return response;
                    }

                }

            }
            catch (Exception ex)
            {

                return response;
            }
        }
        public Sow_Master SaveSowMaster(string sow, int ClientID, string Category, string Subject)
        {
            try
            {
                Sow_Master sowmaster = new Sow_Master();
                using (var db = new ElectricEaseEntitiesContext())
                {
                    if (sow.Trim() != "")
                    {

                        sowmaster.ClientId = ClientID;
                        sowmaster.Sow_desc = sow;
                        sowmaster.Subject = Subject;
                        sowmaster.Category = Category;
                        db.Sow_Master.Add(sowmaster);
                        db.SaveChanges();
                    }
                    return sowmaster;
                }
            }
            catch
            {
                return null;
            }
        }

        public List<Sow_Master> GetSowMasterList(int ClientID)
        {
            try
            {
                using (var db = new ElectricEaseEntitiesContext())
                {
                    var sholist = db.Sow_Master.Where(x => x.ClientId == ClientID).ToList();
                    return sholist;
                }
            }
            catch
            {
                return null;
            }
        }



        public Sow_Master SowMasterDetail(int id, int clientId)
        {
            Sow_Master data = new Sow_Master();
            try
            {
                using (var db = new ElectricEaseEntitiesContext())
                {
                    if (id != 0)
                    {
                        data = db.Sow_Master.Where(x => x.Id == id && x.ClientId == clientId).FirstOrDefault();
                    }
                }
                return data;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string RemoveSow(int id, int ClientId)
        {
            try
            {
                using (var db = new ElectricEaseEntitiesContext())
                {
                    if (id != 0)
                    {

                        db.Sow_Master.Remove(db.Sow_Master.Where(x => x.ClientId == ClientId && x.Id == id).FirstOrDefault());
                        db.SaveChanges();
                    }

                    return "success";
                }
            }
            catch
            {
                return null;
            }
        }
        public ServiceResult<string> GetMycolor(int ClientID, string Assigneduser)
        {
            ServiceResult<string> response = new ServiceResult<string>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    var ColorResult = (from m in db.Account_Master where m.Client_ID == ClientID && m.User_ID == Assigneduser && m.IsActive == true select m.UserColor).FirstOrDefault();
                    if (ColorResult != null)
                    {
                        response.Data = ColorResult;
                        response.Message = "success";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {

                        response.Message = "fail";
                        response.ResultCode = 0;
                        return response;
                    }

                }

            }
            catch (Exception ex)
            {

                response.Message = ex.Message;
                response.ResultCode = 1;
                return response;
            }
        }

        public List<Job_DJE_VQ_Detailsinfo> JobDjeDeltiles(int Jobid, int Client_ID)
        {
            List<Job_DJE_VQ_Detailsinfo> jobDJE = new List<Job_DJE_VQ_Detailsinfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    jobDJE = (from m in db.Job_DJE_VQ_Details
                              where m.Client_ID == Client_ID && m.Job_ID == Jobid && m.IsActive == true
                              select new Job_DJE_VQ_Detailsinfo()
                              {
                                  Client_ID = m.Client_ID,
                                  Job_ID = m.Job_ID,
                                  Status_ID = m.Status_ID,
                                  Expense = m.Expense,
                                  Vendor_Name = m.Vendor_Name,
                                  Cost = m.Cost,
                                  Profit = m.Profit,
                                  Job_DJE_VQ_Status = m.Job_DJE_VQ_Status,
                                  Resale_Total = m.Resale_Total,
                                  Job_DJETotal = m.Job_DJETotal,
                                  Job_VQTotal = m.Job_VQTotal,
                                  IsActive = true
                              }).ToList();

                    return jobDJE;


                }
            }
            catch (Exception ex)
            {
                return jobDJE;
            }
        }
        public string DeleteDjeRow(int StatusID)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    db.Job_DJE_VQ_Details.Remove(db.Job_DJE_VQ_Details.Where(x => x.Status_ID == StatusID).FirstOrDefault());
                    db.SaveChanges();
                    //logs.WriteLog("successfully deleted the Job DjeROw-" + JobID,StatusID);
                }
                return "success";
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        public List<exportAssemblyAndParts> ExportPartsInJob(int JobId)
        {
            List<exportAssemblyAndParts> exportList = new List<exportAssemblyAndParts>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "JobID", Value = JobId };
                    exportList = db.Database.SqlQuery<exportAssemblyAndParts>("exec EE_ExportPartsInJob @JobID", para).ToList();
                }
                return exportList;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return exportList;
            }
        }
    }
}
