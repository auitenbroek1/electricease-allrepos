using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Data;
using ElectricEase.Models;
using ElectricEase.Data.DataBase;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using ElectricEase.Helpers;

namespace ElectricEase.Data.AccountMaster
{
    public class AccountMasterData
    {
        LogsMaster logs = new LogsMaster();
        //public ServiceResultList<Account_MasterInfoList> GetAllUserList()
        //{
        //    ServiceResultList<Account_MasterInfoList> response = new ServiceResultList<Account_MasterInfoList>();

        //    try
        //    {
        //        using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
        //        {
        //            response.ListData = (from m in db.Account_Master
        //                        where m.IsActive == true
        //                        select new Account_MasterInfoList()
        //                        {
        //                            Client_ID = m.Client_ID,
        //                            User_ID = m.User_ID,
        //                            First_Name = m.First_Name,
        //                            Last_Name = m.Last_Name,
        //                            Phone = m.Phone,
        //                            Photo = m.Photo,
        //                            E_Mail = m.E_Mail,
        //                            Password = m.Password,
        //                            Site_Administrator = m.Site_Administrator,
        //                            Account_Administrator = m.Account_Administrator,
        //                            Job_Administrator = m.Job_Administrator,
        //                            Part_Administrator = m.Part_Administrator,
        //                            Labor_Administrator = m.Labor_Administrator,
        //                            Legal_Adminstrator = m.Legal_Adminstrator,
        //                            CreatedBy_SuperUser = m.CreatedBy_SuperUser,
        //                            Created_By = m.Created_By,
        //                            Created_Date = m.Created_Date,
        //                            Updated_By = m.Updated_By,
        //                            Updated_Date = m.Updated_Date
        //                        }).ToList();

        //            // listobj.ClientList = listObj1;
        //            if (response.ListData != null)
        //            {
        //                response.ResultCode = 1;
        //                response.Message = "Success";
        //                return response;

        //            }
        //            else
        //            {
        //                response.ResultCode = -1;
        //                response.Message = "Fail";
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
        public List<Account_MasterInfoList> GetAllUserList()
        {
            List<Account_MasterInfoList> response = new List<Account_MasterInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Account_MasterInfoList> result = new List<Account_MasterInfoList>();
                    response = db.Database.SqlQuery<Account_MasterInfoList>("exec EE_GetAllUsersList").ToList();
                    if (result != null)
                    {

                        return response;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            catch (Exception ex)
            {

                return null;
            }
        }
        //public ObservableCollection<Account_MasterInfoList> GetAllUserList()
        //{

        //    ObservableCollection<Account_MasterInfoList> response = new ObservableCollection<Account_MasterInfoList>();
        //    try
        //    {
        //        using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
        //        {
        //            List<Account_MasterInfoList> result = new List<Account_MasterInfoList>();
        //            response = db.Database.SqlQuery<Account_MasterInfoList>("exec EE_GetAllUsersList").;
        //            if (result != null)
        //            {

        //                return response;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        return null;
        //    }
        //}

        public ServiceResultList<Account_MasterInfoList> GetUsersList()
        {
            ServiceResultList<Account_MasterInfoList> response = new ServiceResultList<Account_MasterInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Account_MasterInfoList> result = new List<Account_MasterInfoList>();
                    result = db.Database.SqlQuery<Account_MasterInfoList>("exec EE_GetAllUsersList").ToList();
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
                        response.Message = "Fail";
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
        public List<Account_MasterInfoList> GetMyClientUserList(int ClientID)
        {

            List<Account_MasterInfoList> response = new List<Account_MasterInfoList>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Account_MasterInfoList> result = new List<Account_MasterInfoList>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    response = db.Database.SqlQuery<Account_MasterInfoList>("exec EE_GetMyUserList @ClientID", para).ToList();

                    // listobj.ClientList = listObj1;
                    if (result != null)
                    {
                        //response.ListData = result;
                        //response.ResultCode = 1;
                        //response.Message = "success";
                        return response;
                    }
                    else
                    {
                        //response.ResultCode = -1;
                        //response.Message = "Fail";
                        return null;
                    }

                }

            }
            catch (Exception ex)
            {
                return null;

            }
        }
        public ServiceResultList<Account_MasterInfoList> GetMyUserList(int ClientID)
        {

            ServiceResultList<Account_MasterInfoList> response = new ServiceResultList<Account_MasterInfoList>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Account_MasterInfoList> result = new List<Account_MasterInfoList>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    result = db.Database.SqlQuery<Account_MasterInfoList>("exec EE_GetMyUserList @ClientID", para).ToList();

                    // listobj.ClientList = listObj1;
                    if (result != null)
                    {
                        response.ListData = result;
                        response.ResultCode = 1;
                        response.Message = "success";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "Fail";
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
        public ServiceResult<bool> CheckIsExistUser(string UserID)
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    //validate User ID 
                    var UserIsExit = db.Account_Master.Any(x => x.User_ID == UserID);
                    if (UserIsExit == true)
                    {
                        response.Data = true;
                        response.ResultCode = 1;
                        response.Message = "“User Id” already registered!";
                        return response;
                    }
                    else
                    {
                        response.Data = false;
                        response.ResultCode = 0;
                        response.Message = "“User Id” does not exist!";
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
        public ServiceResult<int> AddNewUserBySP(Account_MasterInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    bool isupdate = true;
                    //  Client_Master CMtableObj = new Client_Master();

                    Account_Master tableObj = db.Account_Master.Where(x => x.Client_ID == Model.Client_ID && x.User_ID == Model.User_ID && x.IsActive == true).FirstOrDefault();
                    if (tableObj == null)
                    {
                        tableObj = new Account_Master();
                        isupdate = false;


                        Account_Master CheckEmailID = db.Account_Master.Where(x => x.E_Mail == Model.E_Mail && x.IsActive == true && x.Client_ID == Model.Client_ID).FirstOrDefault();
                        if (CheckEmailID != null)
                        {
                            response.Message = "Email ID already exists";
                            response.ResultCode = -1;
                            return response;
                        }
                        bool CheckIsExistColor = db.Account_Master.Any(x => x.UserColor == Model.UserColor && x.Client_ID == Model.Client_ID && x.IsActive == true);

                        if (CheckIsExistColor == true)
                        {

                            response.Message = "The \"Selected Color\" is already existing!";
                            response.ResultCode = -1;
                            return response;
                        }
                        if (Model.UserColor == "#231f20" || Model.UserColor == "#000")
                        {
                            response.Message = "The \"Selected Color\" is already existing!";
                            response.ResultCode = -1;
                            return response;
                        }

                    }
                    else
                    {
                        if (Model.Client_ID == 0)
                        {
                            response.Message = "“Client Company” is required!";
                            response.ResultCode = -1;
                            return response;
                        }
                        Account_Master CheckUserColor = db.Account_Master.Where(x => x.User_ID == Model.User_ID && x.IsActive == true && x.Client_ID == Model.Client_ID).FirstOrDefault();
                        if (CheckUserColor.UserColor != Model.UserColor)
                        {
                            bool CheckIsExistColor = db.Account_Master.Any(x => x.UserColor == Model.UserColor && x.Client_ID == Model.Client_ID && x.IsActive == true);

                            if (CheckIsExistColor == true)
                            {

                                response.Message = "The \"Selected Color\" is already existing!";
                                response.ResultCode = -1;
                                return response;
                            }
                            if (Model.UserColor == "#231f20" || Model.UserColor == "#000" || Model.UserColor == "#CC0000")
                            {
                                response.Message = "The \"Selected Color\" is already existing!";
                                response.ResultCode = -1;
                                return response;
                            }
                        }
                        Account_Master CheckUserMail = db.Account_Master.Where(x => x.User_ID == Model.User_ID && x.IsActive == true && x.Client_ID == Model.Client_ID).FirstOrDefault();
                        if (CheckUserMail.E_Mail != Model.E_Mail)
                        {
                            bool CheckIsExistMail = db.Account_Master.Any(x => x.E_Mail == Model.E_Mail && x.Client_ID == Model.Client_ID && x.IsActive == true);
                            if (CheckIsExistMail == true)
                            {
                                response.Message = "Email ID already exists!";
                                response.ResultCode = -1;
                                return response;
                            }
                        }

                        isupdate = true;
                    }

                    tableObj.Client_ID = Model.Client_ID;
                    tableObj.User_ID = Model.User_ID;
                    tableObj.First_Name = Model.First_Name;
                    tableObj.Last_Name = Model.Last_Name;
                    tableObj.Phone = Model.Phone;
                    tableObj.E_Mail = Model.E_Mail;
                    tableObj.Password = Model.Password;
                    tableObj.Site_Administrator = Model.Site_Administrator;
                    tableObj.Phone = Model.Phone;
                    tableObj.Account_Administrator = Model.Account_Administrator;
                    tableObj.Job_Administrator = Model.Job_Administrator;
                    tableObj.Part_Administrator = Model.Part_Administrator;
                    tableObj.Labor_Administrator = Model.Labor_Administrator;
                    tableObj.Legal_Adminstrator = Model.Legal_Adminstrator;
                    tableObj.Part_Administrator = Model.Part_Administrator;
                    tableObj.Job_Description_Report = Model.Job_Description_Report;
                    tableObj.Assembly_Administrator = Model.Assembly_Administrator;
                    tableObj.Client_Estimation_Report = Model.Client_Estimation_Report;
                    tableObj.CreatedBy_SuperUser = false;
                    tableObj.IsActive = true;
                    if (isupdate == false)
                    {
                        tableObj.Password = Model.Password;
                        tableObj.Photo = Model.Photo;
                        tableObj.UserColor = Model.UserColor;
                        tableObj.Created_Date = DateTime.Now;
                        tableObj.Created_By = Model.Created_By;
                        db.Account_Master.Add(tableObj);
                    }
                    else
                    {
                        tableObj.Updated_By = Model.Created_By;
                        tableObj.Updated_Date = DateTime.Now;
                        if (Model.Password != null)
                        {
                            tableObj.Password = Model.Password;
                        }
                        if (Model.Photo != null)
                        {
                            tableObj.Photo = Model.Photo;
                        }

                        tableObj.UserColor = Model.UserColor;


                    }

                    int resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        response.ResultCode = 1;
                        if (isupdate == false)
                        {
                            if (Model.Mailsendconformaition == true)
                            {
                                EmailLog _email = new EmailLog();
                                _email.BodyContent = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/EmailTempLates/Registration.html"));
                                _email.BodyContent = _email.BodyContent.Replace("##FirstName##", Model.First_Name);
                                _email.BodyContent = _email.BodyContent.Replace("##LastName##", Model.Last_Name);
                                _email.BodyContent = _email.BodyContent.Replace("##Email##", Model.User_ID);
                                _email.BodyContent = _email.BodyContent.Replace("##Password##", Model.Password);
                                _email.EmailTo = Model.E_Mail;
                                _email.Subject = "Electric-ease login credentials";
                                EmailService _emailService = new EmailService();
                                string Status = _emailService.SendEmail(_email);
                            }
                            response.Message = "“User Details” are added successfully.";
                            logs.WriteLog("Successfully Added the New User - " + Model.User_ID, Model.Client_ID, Model.Created_By);
                        }
                        else
                        {
                            if (Model.Mailsendconformaition == true)
                            {
                                EmailLog _email = new EmailLog();
                                _email.BodyContent = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/EmailTempLates/Registration.html"));
                                _email.BodyContent = _email.BodyContent.Replace("##FirstName##", Model.First_Name);
                                _email.BodyContent = _email.BodyContent.Replace("##LastName##", Model.Last_Name);
                                _email.BodyContent = _email.BodyContent.Replace("##Email##", Model.User_ID);
                                _email.BodyContent = _email.BodyContent.Replace("##Password##", Model.Password);
                                _email.EmailTo = Model.E_Mail;
                                _email.Subject = "Electric-ease login credentials";
                                EmailService _emailService = new EmailService();
                                string Status = _emailService.SendEmail(_email);
                            }
                            response.Message = "“User Details” are updated successfully.";
                            logs.WriteLog("Successfully Updated the  User - " + Model.User_ID, Model.Client_ID, Model.Updated_By);
                        }

                        return response;
                    }
                    else
                    {
                        // response.ResultCode = 1;
                        if (isupdate == false)
                        {
                            response.Message = "Error while “New User” registering!";
                        }
                        else
                        {
                            response.Message = "Error while updating “User”!";
                        }

                        return response;
                    }
                }
            }
            catch (Exception ex)
            {

                logs.WriteLog("[Exception] While Add/update User - " + Model.User_ID + "[Error Msg- " + ex.Message + " ]", Model.Client_ID, Model.Created_By);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResult<EditAccount_MasterInfo> GetUserDetails(string UserID, int ClientID)
        {
            ServiceResult<EditAccount_MasterInfo> response = new ServiceResult<EditAccount_MasterInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Account_Master AMtblobj = new Account_Master();
                    AMtblobj = (from m in db.Account_Master where m.User_ID == UserID && m.IsActive == true && m.Client_ID == ClientID select m).FirstOrDefault();
                    if (AMtblobj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“User” doesn't exist!";
                        return response;
                    }
                    response.Data = new EditAccount_MasterInfo()
                    {
                        Client_ID = AMtblobj.Client_ID,
                        User_ID = AMtblobj.User_ID,
                        First_Name = AMtblobj.First_Name,
                        Last_Name = AMtblobj.Last_Name,
                        Phone = AMtblobj.Phone,
                        Photo = AMtblobj.Photo,
                        UserColor = AMtblobj.UserColor,
                        E_Mail = AMtblobj.E_Mail,
                        Password = AMtblobj.Password,
                        Site_Administrator = AMtblobj.Site_Administrator,
                        Account_Administrator = AMtblobj.Account_Administrator,
                        Job_Administrator = AMtblobj.Job_Administrator,
                        Part_Administrator = AMtblobj.Part_Administrator,
                        Labor_Administrator = AMtblobj.Labor_Administrator,
                        Legal_Adminstrator = AMtblobj.Legal_Adminstrator,
                        Assembly_Administrator = AMtblobj.Assembly_Administrator,
                        Job_Description_Report = AMtblobj.Job_Description_Report,
                        Client_Estimation_Report = AMtblobj.Client_Estimation_Report,
                        Created_By = AMtblobj.Created_By,
                        Created_Date = AMtblobj.Created_Date,
                        CreatedBy_SuperUser = AMtblobj.CreatedBy_SuperUser,
                        Updated_By = AMtblobj.Updated_By,
                        Updated_Date = AMtblobj.Updated_Date
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
        public ServiceResult<Account_MasterInfo> GetUserDetail(string UserID, int ClientID)
        {
            ServiceResult<Account_MasterInfo> response = new ServiceResult<Account_MasterInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Account_Master AMtblobj = new Account_Master();
                    AMtblobj = (from m in db.Account_Master where m.User_ID == UserID && m.IsActive == true && m.Client_ID == ClientID select m).FirstOrDefault();
                    if (AMtblobj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“User” doesn't exist!";
                        return response;
                    }
                    response.Data = new Account_MasterInfo()
                    {
                        Client_ID = AMtblobj.Client_ID,
                        User_ID = AMtblobj.User_ID,
                        First_Name = AMtblobj.First_Name,
                        Last_Name = AMtblobj.Last_Name,
                        Phone = AMtblobj.Phone,
                        Photo = AMtblobj.Photo,
                        UserColor = AMtblobj.UserColor,
                        E_Mail = AMtblobj.E_Mail,
                        Password = AMtblobj.Password,
                        Site_Administrator = AMtblobj.Site_Administrator,
                        Account_Administrator = AMtblobj.Account_Administrator,
                        Job_Administrator = AMtblobj.Job_Administrator,
                        Part_Administrator = AMtblobj.Part_Administrator,
                        Labor_Administrator = AMtblobj.Labor_Administrator,
                        Legal_Adminstrator = AMtblobj.Legal_Adminstrator,
                        Assembly_Administrator = AMtblobj.Assembly_Administrator,
                        Job_Description_Report = AMtblobj.Job_Description_Report,
                        Client_Estimation_Report = AMtblobj.Client_Estimation_Report,
                        Created_By = AMtblobj.Created_By,
                        Created_Date = AMtblobj.Created_Date,
                        CreatedBy_SuperUser = AMtblobj.CreatedBy_SuperUser,
                        Updated_By = AMtblobj.Updated_By,
                        Updated_Date = AMtblobj.Updated_Date
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

        //To Save Edit Information In User Details
        public ServiceResult<int> SaveUserDetails(EditAccount_MasterInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Account_Master AMtblobj = new Account_Master();

                    Account_Master CheckUserColor = db.Account_Master.Where(x => x.User_ID == Model.User_ID && x.IsActive == true && x.Client_ID == Model.Client_ID).FirstOrDefault();
                    if (CheckUserColor.UserColor != Model.UserColor)
                    {
                        bool CheckIsExistColor = db.Account_Master.Any(x => x.UserColor == Model.UserColor && x.Client_ID == Model.Client_ID && x.IsActive == true);

                        if (CheckIsExistColor == true)
                        {

                            response.Message = "The \"Selected Color\" is already existing!";
                            response.ResultCode = -1;
                            return response;
                        }
                        if (Model.UserColor == "#231f20" || Model.UserColor == "#000" || Model.UserColor == "#CC0000")
                        {
                            response.Message = "The \"Selected Color\" is already existing!";
                            response.ResultCode = -1;
                            return response;
                        }
                    }
                    Account_Master CheckUserMail = db.Account_Master.Where(x => x.User_ID == Model.User_ID && x.IsActive == true && x.Client_ID == Model.Client_ID).FirstOrDefault();
                    if (CheckUserMail.E_Mail != Model.E_Mail)
                    {
                        bool CheckIsExistMail = db.Account_Master.Any(x => x.E_Mail == Model.E_Mail && x.Client_ID == Model.Client_ID && x.IsActive == true);
                        if (CheckIsExistMail == true)
                        {
                            response.Message = "Email ID already exists!";
                            response.ResultCode = -1;
                            return response;
                        }
                    }
                    AMtblobj = (from m in db.Account_Master where m.User_ID == Model.User_ID && m.IsActive == true select m).FirstOrDefault();


                    //AMtblobj.Client_ID = Model.Client_ID;
                    if (Model.User_ID != null)
                    {
                        AMtblobj.User_ID = Model.User_ID;
                    }
                    if (Model.Password != null)
                    {
                        AMtblobj.Password = Model.Password;
                    }
                    //AMtblobj.Password = Model.Password;
                    if (Model.First_Name != null)
                    {
                        AMtblobj.First_Name = Model.First_Name;
                    }
                    AMtblobj.First_Name = Model.First_Name;
                    AMtblobj.Last_Name = Model.Last_Name;
                    if (Model.Phone != null)
                    {
                        AMtblobj.Phone = Model.Phone;
                    }
                    //AMtblobj.Phone = Model.Phone;
                    if (Model.Photo != null)
                    {
                        AMtblobj.Photo = Model.Photo;
                    }
                    if (Model.UserColor != "#000000")
                    {
                        AMtblobj.UserColor = Model.UserColor;
                    }
                    AMtblobj.E_Mail = Model.E_Mail;

                    AMtblobj.Site_Administrator = Model.Site_Administrator;
                    AMtblobj.Account_Administrator = Model.Account_Administrator;
                    AMtblobj.Job_Administrator = Model.Job_Administrator;
                    AMtblobj.Part_Administrator = Model.Part_Administrator;
                    AMtblobj.Labor_Administrator = Model.Labor_Administrator;
                    AMtblobj.Legal_Adminstrator = Model.Legal_Adminstrator;
                    AMtblobj.Assembly_Administrator = Model.Assembly_Administrator;
                    AMtblobj.Job_Description_Report = Model.Job_Description_Report;
                    AMtblobj.Client_Estimation_Report = Model.Client_Estimation_Report;
                    AMtblobj.Updated_By = Model.Updated_By;
                    AMtblobj.Updated_Date = DateTime.Now;
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

        public ServiceResult<int> DeleteUserInAM(string UserID, int ClientID)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            int resultcount;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Account_Master tableObj = new Account_Master();
                    tableObj = db.Account_Master.Where(m => m.User_ID == UserID && m.IsActive == true && m.Client_ID == ClientID).FirstOrDefault();
                    tableObj.IsActive = false;
                    resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        response.ResultCode = resultcount;
                        response.Data = resultcount;
                        response.Message = "Success";
                        logs.WriteLog("Successfully Deleted the User - " + UserID, ClientID, UserID);
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
                logs.WriteLog("[Exception] While adding User - " + UserID + "[Error Msg- " + ex.Message + " ]", ClientID, UserID);
                return response;

            }
        }


        public ServiceResult<EditAccount_MasterInfo> GetMyDetailsInAM(string UserID)
        {
            ServiceResult<EditAccount_MasterInfo> response = new ServiceResult<EditAccount_MasterInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Account_Master AMtblObj = new Account_Master();
                    Client_Master CMtblobj = new Client_Master();
                    AMtblObj = (from m in db.Account_Master where m.User_ID == UserID select m).FirstOrDefault();
                    CMtblobj = (from m in db.Client_Master where m.Client_ID == AMtblObj.Client_ID select m).FirstOrDefault();
                    response.Data = new EditAccount_MasterInfo()
                    {
                        Client_ID = AMtblObj.Client_ID,
                        User_ID = AMtblObj.User_ID,
                        First_Name = AMtblObj.First_Name,
                        Last_Name = AMtblObj.Last_Name,
                        Phone = AMtblObj.Phone,
                        Photo = AMtblObj.Photo,
                        E_Mail = AMtblObj.E_Mail,
                        Password = AMtblObj.Password,
                        Site_Administrator = AMtblObj.Site_Administrator,
                        Account_Administrator = AMtblObj.Account_Administrator,
                        Job_Administrator = AMtblObj.Job_Administrator,
                        Part_Administrator = AMtblObj.Part_Administrator,
                        Labor_Administrator = AMtblObj.Labor_Administrator,
                        Legal_Adminstrator = AMtblObj.Legal_Adminstrator,
                        Created_By = AMtblObj.Created_By,
                        Created_Date = AMtblObj.Created_Date,
                        CreatedBy_SuperUser = AMtblObj.CreatedBy_SuperUser,
                        Updated_By = AMtblObj.Updated_By,
                        Updated_Date = AMtblObj.Updated_Date,
                        Address = CMtblobj.Address,
                        Address2 = CMtblobj.Address2,
                        City = CMtblobj.City,
                        Client_State = CMtblobj.State,
                        ZipCode = CMtblobj.ZipCode,
                        Client_Company = CMtblobj.Client_Company,
                        Mobile = CMtblobj.Mobile,
                        Fax = CMtblobj.Fax,
                        Contactperson = CMtblobj.Contact_person,
                        isfirst = AMtblObj.IsFirst ?? false,
                        JobIDPreffix = CMtblobj.JobIDPreffix


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

        public ServiceResult<int> UpdateMyDetailsInAM(EditAccount_MasterInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Account_Master AMtblobj = new Account_Master();
                    Client_Master CMTblobj = new Client_Master();
                    //Check whether given Email Id is already Exists
                    // var CheckEmailID = db.Client_Master.Any(x =>x.Client_ID==Model.Client_ID &&x.Email == Model.E_Mail && x.IsActive == true);
                    var CheckacEmailID = db.Account_Master.Any(x => x.Client_ID == Model.Client_ID && x.E_Mail == Model.E_Mail && x.User_ID != Model.User_ID && x.IsActive == true);
                    if (CheckacEmailID == true)
                    {
                        response.Message = "Email ID already exists";
                        response.ResultCode = -1;
                        return response;
                    }
                    AMtblobj = (from m in db.Account_Master where m.User_ID == Model.User_ID && m.IsActive == true select m).FirstOrDefault();
                    CMTblobj = (from m in db.Client_Master where m.Client_ID == Model.Client_ID && m.IsActive == true select m).FirstOrDefault();

                    //AMtblobj.Client_ID = Model.Client_ID;
                    if (Model.User_ID != null)
                    {
                        AMtblobj.User_ID = Model.User_ID;
                    }
                    if (Model.Password != null)
                    {
                        AMtblobj.Password = Model.Password;
                    }
                    //AMtblobj.Password = Model.Password;
                    if (Model.First_Name != null)
                    {
                        AMtblobj.First_Name = Model.First_Name;
                    }
                    AMtblobj.First_Name = Model.First_Name;
                    AMtblobj.Last_Name = Model.Last_Name;
                    if (Model.Phone != null)
                    {
                        AMtblobj.Phone = Model.Phone;
                    }
                    //AMtblobj.Phone = Model.Phone;
                    if (Model.Photo != null)
                    {
                        AMtblobj.Photo = Model.Photo;
                    }
                    AMtblobj.E_Mail = Model.E_Mail;
                    if (AMtblobj.IsFirst == true)
                    {
                        AMtblobj.Site_Administrator = true;
                        AMtblobj.Account_Administrator = true;
                        AMtblobj.Job_Administrator = true;
                        AMtblobj.Part_Administrator = true;
                        AMtblobj.Labor_Administrator = true;
                        AMtblobj.Legal_Adminstrator = true;
                        AMtblobj.Assembly_Administrator = true;
                        AMtblobj.Job_Description_Report = true;
                        AMtblobj.Client_Estimation_Report = true;
                        AMtblobj.CreatedBy_SuperUser = true;
                        AMtblobj.IsFirst = false;
                        CMTblobj.Client_Company = Model.Client_Company;
                    }
                    //AMtblobj.Site_Administrator = Model.Site_Administrator;
                    //AMtblobj.Account_Administrator = Model.Account_Administrator;
                    //AMtblobj.Job_Administrator = Model.Job_Administrator;
                    //AMtblobj.Part_Administrator = Model.Part_Administrator;
                    //AMtblobj.Labor_Administrator = Model.Labor_Administrator;
                    //AMtblobj.Legal_Adminstrator = Model.Legal_Adminstrator;
                    //AMtblobj.CreatedBy_SuperUser = Model.CreatedBy_SuperUser;
                    AMtblobj.Updated_By = Model.Updated_By;
                    AMtblobj.Updated_Date = DateTime.Now;
                    int resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        if (AMtblobj.CreatedBy_SuperUser == true)
                        {
                            CMTblobj.Address = Model.Address;
                            CMTblobj.Address2 = Model.Address2;
                            CMTblobj.City = Model.City;
                            CMTblobj.ZipCode = Model.ZipCode;
                            CMTblobj.JobIDPreffix = Model.JobIDPreffix;
                            CMTblobj.State = Model.Client_State;
                            CMTblobj.Fax = Model.Fax;
                            CMTblobj.Phone = Model.Phone;
                            CMTblobj.Mobile = Model.Mobile;
                            if (Model.Photo != null)
                            {
                                CMTblobj.Client_Logo = Model.Photo;
                            }
                            CMTblobj.Updated_By = "Admin";
                            CMTblobj.Updated_Date = DateTime.Now;
                            CMTblobj.Contact_person = Model.Contactperson;

                            db.SaveChanges();
                        }
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

    }
}
