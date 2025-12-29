using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Data;
using ElectricEase.Models;
using ElectricEase.Data.DataBase;
using System.Data.SqlClient;
using ElectricEase.Helpers;

namespace ElectricEase.Data.LegalMaster
{
    public class LegalMasterData
    {
        LogsMaster logs = new LogsMaster();
        public ServiceResultList<Legal_DetailsInfoList> GetLegalList()
        {
            ServiceResultList<Legal_DetailsInfoList> response = new ServiceResultList<Legal_DetailsInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Legal_DetailsInfoList> result = new List<Legal_DetailsInfoList>();
                    result = db.Database.SqlQuery<Legal_DetailsInfoList>("exec EE_GetLegalDetailList").OrderBy(x=>x.Legal_ID).ToList();

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

        public ServiceResultList<Legal_DetailsInfoList> GetMyLegalList(int ClientID)
        {
            ServiceResultList<Legal_DetailsInfoList> response = new ServiceResultList<Legal_DetailsInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Legal_DetailsInfoList> result = new List<Legal_DetailsInfoList>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    result = db.Database.SqlQuery<Legal_DetailsInfoList>("exec EE_GetMyLegalDetailList @ClientID", para).ToList();

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

        //check is existing Legal Detail
        public ServiceResult<bool> CheckIsExistingLegal(string LegalDetail, int ClientID)
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    //validate User ID 
                    Legal_Details LegalIsExit = db.Legal_Details.Where(x => x.Client_Id == ClientID && x.Legal_Detail == LegalDetail && x.IsActive == true).FirstOrDefault();
                    if (LegalIsExit != null)
                    {
                        response.Data = true;
                        response.ResultCode = 1;
                        response.Message = "“Legal Detail” is already registered.";
                        return response;
                    }
                    else
                    {
                        response.Data = false;
                        response.ResultCode = 0;
                        response.Message = "“Legal Detail” is invalid.";
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
        //Here To Add New Legal details To Legal Details Table From Database
        public ServiceResult<int> AddLegalDetails(Legal_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {

                    Legal_Details LDTblObj = new Legal_Details();
                    string stat = "";
                    bool isupdate = true;
                    //validate Legal Detail
                    if (Model.Legal_ID == 0)
                        LDTblObj = db.Legal_Details.Where(x => x.Client_Id == Model.Client_ID && x.Legal_Detail == Model.Legal_Detail && x.IsActive == true).FirstOrDefault();
                    else
                        LDTblObj = db.Legal_Details.Where(x => x.Client_Id == Model.Client_ID && x.Legal_ID == Model.Legal_ID && x.IsActive == true).FirstOrDefault();
                    if (LDTblObj != null)
                    {
                        isupdate = true;
                    }
                    else
                    {
                        LDTblObj = new Legal_Details();
                        isupdate = false;
                    }
                    LDTblObj.Client_Id = Model.Client_ID;
                    if (Model.OtherLegal_Category != null && Model.Legal_Category == null)
                    {
                        Model.Legal_Category = Model.OtherLegal_Category;
                    }
                    if (Model.Legal_Category == "other" && Model.OtherLegal_Category != null)
                    {
                        Model.Legal_Category = Model.OtherLegal_Category;
                    }
                    LDTblObj.Legal_Category = Model.Legal_Category;
                    LDTblObj.Legal_Detail = Model.Legal_Detail;

                    LDTblObj.IsActive = true;
                    if (!isupdate)
                    {
                        LDTblObj.Created_By = Model.Created_By;
                        LDTblObj.Created_Date = DateTime.Now;
                        LDTblObj.Updated_By = Model.Created_By;
                        LDTblObj.Updated_Date = DateTime.Now;

                        LDTblObj.Legal_ID = Convert.ToInt32(Model.Legal_ID.ToString());
                        db.Legal_Details.Add(LDTblObj);
                        stat = "“Legal Details” are added successfully.";
                        logs.WriteLog("Successfully Added the New Legal - " + Model.Legal_Detail, Model.Client_ID, Model.Created_By);
                    }
                    else
                    {
                        LDTblObj.Updated_By = Model.Created_By;
                        LDTblObj.Updated_Date = DateTime.Now;
                        stat = "Legal Details are updated successfully.";
                        logs.WriteLog("Successfully Updated the  Legal - " + Model.Legal_Detail, Model.Client_ID, Model.Updated_By);
                    }
                    int resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        response.ResultCode = 1;
                        response.Message = stat;
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "Error while “Legal” registering!";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                logs.WriteLog("[Exception] While Add/Update Legal - " + Model.Legal_ID + "[Error Msg- " + ex.Message + " ]", Model.Client_ID, Model.Created_By);
                return response;
            }
        }

        //here to Get Legal details For Edit and Update
        public ServiceResult<EditLegal_DetailsInfo> GetLegalDetails(int LegalID, int ClientID)
        {
            ServiceResult<EditLegal_DetailsInfo> response = new ServiceResult<EditLegal_DetailsInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Legal_Details LDTblObj = new Legal_Details();
                    LDTblObj = (from m in db.Legal_Details where m.Legal_ID == LegalID && m.IsActive == true && m.Client_Id == ClientID select m).FirstOrDefault();
                    if (LDTblObj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“Legal ID” doesn't exist!";
                        return response;
                    }
                    response.Data = new EditLegal_DetailsInfo()
                    {

                        Client_ID = LDTblObj.Client_Id,
                        Legal_ID = LDTblObj.Legal_ID,
                        Legal_Category = LDTblObj.Legal_Category,
                        Legal_Detail = LDTblObj.Legal_Detail,
                        Created_By = LDTblObj.Created_By,
                        Created_Date = LDTblObj.Created_Date,
                        Updated_By = LDTblObj.Updated_By,
                        Updated_Date = LDTblObj.Updated_Date,
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
        public ServiceResult<Legal_DetailsInfo> GetLegalDetail(int LegalID, int ClientID)
        {
            ServiceResult<Legal_DetailsInfo> response = new ServiceResult<Legal_DetailsInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Legal_Details LDTblObj = new Legal_Details();
                    LDTblObj = (from m in db.Legal_Details where m.Legal_ID == LegalID && m.IsActive == true && m.Client_Id == ClientID select m).FirstOrDefault();
                    if (LDTblObj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“Legal ID” doesn't exist!";
                        return response;
                    }
                    response.Data = new Legal_DetailsInfo()
                    {

                        Client_ID = LDTblObj.Client_Id,
                        Legal_ID = LDTblObj.Legal_ID,
                        Legal_Category = LDTblObj.Legal_Category,
                        Legal_Detail = LDTblObj.Legal_Detail,
                        Created_By = LDTblObj.Created_By,
                        Created_Date = LDTblObj.Created_Date,
                        Updated_By = LDTblObj.Updated_By,
                        Updated_Date = LDTblObj.Updated_Date,
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
        public string contactus(contactUs model)
        {
            string Status = "";
            try
            {
                EmailLog _email = new EmailLog();
                _email.EmailTo = model.message;
                _email.Subject = "Thank You- ElectricEase";
                _email.BodyContent = "Hi <br/>Thank you for Contacting us, Our team will contact you soon";
                _email.BodyContent += "<br/><br/>Thanks & Regards,<br/> Electricease Team";
                _email.SentBy = "ElectricEase Admin";
                _email.SentDate = DateTime.Now;
                EmailService _emailservices = new EmailService();
                Status = _emailservices.SendEmail(_email);
                if (Status == "success")
                {
                    string EmailSentTo = System.Configuration.ConfigurationManager.AppSettings["To"];
                    _email.EmailTo = EmailSentTo;
                    _email.Subject = "Product Demo & Pricing";
                    _email.BodyContent = "Hi <br/>Client has requested for demo and pricing details.<br/>Email : " + model.message;
                    _email.BodyContent += "<br/><br/>Thanks & Regards";
                    _email.SentDate = DateTime.Now;
                    string Status2 = _emailservices.SendEmail(_email);
                    string stat = Status;
                }
                else
                {
                    string stat = Status;
                }
                return "success";


            }
            catch (Exception ex)
            {
                return "fail";
            }

        }
        //Here To Update Legal Details
        public ServiceResult<int> UpdateLegalDetails(EditLegal_DetailsInfo Model)
        {

            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Legal_Details LDTblObj = new Legal_Details();


                    LDTblObj = (from m in db.Legal_Details where m.Legal_ID == Model.Legal_ID && m.Client_Id == Model.Client_ID && m.IsActive == true && m.Client_Id == Model.Client_ID select m).FirstOrDefault();
                    if (Model.OtherLegal_Category != null && Model.Legal_Category == null)
                    {
                        Model.Legal_Category = Model.OtherLegal_Category;
                    }

                    if (Model.Legal_Category == "1" && Model.OtherLegal_Category != null)
                    {
                        Model.Legal_Category = Model.OtherLegal_Category;
                    }
                    if (Model.Legal_Category != null)
                    {
                        LDTblObj.Legal_Category = Model.Legal_Category;
                    }
                    LDTblObj.Legal_Detail = Model.Legal_Detail;
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
        public ServiceResult<int> UpdateLegalDetail(Legal_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Legal_Details LDTblObj = new Legal_Details();


                    LDTblObj = (from m in db.Legal_Details where m.Legal_ID == Model.Legal_ID && m.Client_Id == Model.Client_ID && m.IsActive == true && m.Client_Id == Model.Client_ID select m).FirstOrDefault();
                    if (Model.OtherLegal_Category != null && Model.Legal_Category == null)
                    {
                        Model.Legal_Category = Model.OtherLegal_Category;
                    }

                    if (Model.Legal_Category == "1" && Model.OtherLegal_Category != null)
                    {
                        Model.Legal_Category = Model.OtherLegal_Category;
                    }
                    if (Model.Legal_Category != null)
                    {
                        LDTblObj.Legal_Category = Model.Legal_Category;
                    }
                    LDTblObj.Legal_Detail = Model.Legal_Detail;
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

        //Here To Delete Legal Details\
        public ServiceResult<int> DeleteLegalDetails(int LegalID, int ClientID)
        {

            ServiceResult<int> response = new ServiceResult<int>();
            int resultcount;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Legal_Details LDtableObj = new Legal_Details();
                    LDtableObj = db.Legal_Details.Where(m => m.Legal_ID == LegalID && m.Client_Id == ClientID).FirstOrDefault();
                    LDtableObj.IsActive = false;
                    resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        response.ResultCode = resultcount;
                        response.Data = resultcount;
                        response.Message = "“Deleted” successfully.";
                        logs.WriteLog("Successfully Updated the  Legal - " + LegalID, ClientID, LegalID.ToString());
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
                logs.WriteLog("[Exception] While Deleting Legal - " + LegalID + "[Error Msg- " + ex.Message + " ]", ClientID, LegalID.ToString());
                return response;

            }
        }

        //Here Get Selected Legal List Details
        public ServiceResultList<Legal_DetailsInfoList> GetSelectedLegalListDetails(int ClientID, int LegalID)
        {
            ServiceResultList<Legal_DetailsInfoList> response = new ServiceResultList<Legal_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Legal_DetailsInfoList> result = new List<Legal_DetailsInfoList>();
                    result = db.Database.SqlQuery<Legal_DetailsInfoList>("exec EE_GetSelectedLegalList @ClientID, @legalID", new SqlParameter("ClientID", ClientID), new SqlParameter("LegalID", LegalID)).ToList();

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
