using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Models;
using ElectricEase.Data;
using ElectricEase.Data.DataBase;
using ElectricEase.Helpers;

namespace ElectricEase.Data.ClientMaster
{
    public class ClientMasterData
    {
        LogsMaster logs = new LogsMaster();
        public Account_MasterInfo GetClientName(string username)
        {
            Account_MasterInfo model = new Account_MasterInfo();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {

                    Account_Master AMTblObj = db.Account_Master.SingleOrDefault(u => u.User_ID == username);
                    model.User_ID = AMTblObj.User_ID;
                    model.Photo = AMTblObj.Photo;
                    model.Client_ID = AMTblObj.Client_ID;
                    model.Account_Administrator = AMTblObj.Account_Administrator;
                    model.CreatedBy_SuperUser = AMTblObj.CreatedBy_SuperUser;
                    model.Part_Administrator = AMTblObj.Part_Administrator;
                    model.Labor_Administrator = AMTblObj.Labor_Administrator;
                    model.Job_Administrator = AMTblObj.Job_Administrator;
                    model.Legal_Adminstrator = AMTblObj.Legal_Adminstrator;
                    model.Assembly_Administrator = AMTblObj.Assembly_Administrator;
                    model.Job_Description_Report = AMTblObj.Job_Description_Report;
                    model.Client_Estimation_Report = AMTblObj.Client_Estimation_Report;
                    model.IsActive = (bool)AMTblObj.IsActive;
                    model.First_Name = AMTblObj.First_Name;
                    model.Last_Name = AMTblObj.Last_Name;
                    model.isfirst = AMTblObj.IsFirst;
                    model.Site_Administrator = AMTblObj.Site_Administrator;
                    var result = (from m in db.Client_Master where m.Client_ID == model.Client_ID select m).FirstOrDefault();
                    if (result != null)
                    {
                        model.MyClientlogo = result.Client_Logo;
                        model.Client_Company = result.Client_Company;
                        model.distId = result.Distributor_ID ?? 0;
                        if (result.Distributor_ID != -1)
                        {
                            model.DistLogo = (from d in db.Distributor_Master where d.ID == model.distId select d.CompanyLogo).FirstOrDefault();
                            model.distCompurl = (from d in db.Distributor_Master where d.ID == model.distId select d.Company_Url).FirstOrDefault();
                        }
                    }
                    return model;
                }
            }
            catch (Exception ex)
            {
                return null;
            }


        }


        public bool CheckValidUser(LoginInfo model)
        {
            logs.WriteLog("Enters CheckValidUser", 0, "Admin");
            bool uservalid;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    var result = db.Account_Master.Where(m => m.User_ID.Equals(model.UserName) && m.Password.Equals(model.Password) && m.IsActive == true).FirstOrDefault();
                    if (result.User_ID == model.UserName && result.Password == model.Password)
                    {
                        uservalid = db.Client_Master.Any(m => m.Client_ID == result.Client_ID && m.IsActive == true);
                        if (uservalid == true)
                        {
                            logs.WriteLog("returning true", 0, "Admin");
                            logs.WriteLog("Exits CheckValidUser", 0, "Admin");
                            return true;
                        }
                        else
                        {
                            logs.WriteLog("returning false", 0, "Admin");
                            logs.WriteLog("Exits CheckValidUser", 0, "Admin");
                            return false;
                        }

                    }
                    else
                    {
                        logs.WriteLog("returning false", 0, "Admin");
                        logs.WriteLog("Exits CheckValidUser", 0, "Admin");
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                logs.WriteLog(ex.Message, 0, "Admin");
                logs.WriteLog("returning false", 0, "Admin");
                logs.WriteLog("Exits CheckValidUser", 0, "Admin");
                return false;
            }


        }
        public ServiceResult<ForgotPasswordModel> CheckIsExistingUser(string Email)
        {
            bool uservalid;
            ServiceResult<ForgotPasswordModel> response = new ServiceResult<ForgotPasswordModel>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    uservalid = db.Account_Master.Any(m => m.E_Mail == Email && m.IsActive == true);



                    if (uservalid == true)
                    {
                        Account_Master AMtblObj = new Account_Master();
                        AMtblObj = (from m in db.Account_Master where m.E_Mail == Email && m.IsActive == true select m).FirstOrDefault();


                        response.Data = new ForgotPasswordModel()
                        {
                            Client_ID = AMtblObj.Client_ID,
                            NAME = AMtblObj.First_Name,
                            User_ID = AMtblObj.User_ID,
                            PASSWORDTOKEN = AMtblObj.Password,
                            EMAIL = AMtblObj.E_Mail
                        };

                        response.Message = "Valid “User”.";
                        response.ResultCode = 1;
                        return response;
                    }
                    else
                    {

                        response.Message = "User doesn’t exist.";
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

        public ServiceResult<bool> CheckIsExisCompany(string ClientCompany, string UserID)
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    // Client_Master ClientIsExit = new Client_Master();

                    var ClientIsExit = db.Client_Master.Any(x => x.Client_Company == ClientCompany);

                    var UserIsExit = db.Account_Master.Any(x => x.User_ID == UserID);
                    if (ClientIsExit == true || UserIsExit == true)
                    {
                        response.Data = true;
                        response.ResultCode = 1;
                        if (ClientIsExit == true)
                        {
                            // response.Message = "Client Company Is Already Registered,Please Contact your Admistrator";
                            response.Message = "Are you sure, do you want to update?";
                        }
                        if (UserIsExit == true)
                        {
                            response.Message = "“Client Admin ID” already registered, please change your ID.";
                        }

                        return response;
                    }
                    else
                    {
                        response.Data = false;
                        response.ResultCode = 0;
                        response.Message = "“Client Company” does not exist!";
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
        public ServiceResult<int> AddClientMaster(Client_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    string stat = "";
                    bool isupdate = true;
                    //  Client_Master CMtableObj = new Client_Master();

                    Client_Master CMtableObj = db.Client_Master.Where(x => x.Client_ID == model.Client_ID).FirstOrDefault();
                    if (CMtableObj != null)
                    {
                        isupdate = true;
                    }
                    else
                    {
                        CMtableObj = new Client_Master();
                        isupdate = false;

                    }
                    if (model.DistributorID > 0)
                    {
                        var clientlist = db.Client_Master.Where(x => x.Distributor_ID == model.DistributorID && x.IsActive == true).ToList();
                        var DistributorNoOfuser = (from dt in db.Distributor_Master where dt.ID == model.DistributorID select dt.No_Of_Users).FirstOrDefault();
                        if (isupdate == false)
                        {
                            if (clientlist.Count == DistributorNoOfuser)
                            {
                                response.Message = "Distributor exceeds the number of user “Limit”!";
                                response.ResultCode = -1;
                                return response;
                            }
                        }

                    }
                    // here To check email is is exist already
                    if (isupdate == false)
                    {
                        Client_Master CheckEmailID = db.Client_Master.Where(x => x.Email == model.Email && x.IsActive == true).FirstOrDefault();
                        Account_Master CheckacEmailID = db.Account_Master.Where(x => x.E_Mail == model.Email && x.IsActive == true).FirstOrDefault();
                        if (CheckEmailID != null || CheckacEmailID != null)
                        {
                            response.Message = "Email ID already exists";
                            response.ResultCode = -1;
                            return response;
                        }
                        Account_Master AMtTblObj = new Account_Master();
                        AMtTblObj = db.Account_Master.Where(x => x.User_ID == model.SuperUser).FirstOrDefault();
                        if (AMtTblObj != null)
                        {
                            response.Message = "“User ID” already exists!";
                            response.ResultCode = -1;
                            return response;
                        }
                    }
                    else
                    {
                        CMtableObj.IsActive = model.activateStatus;
                        //if (model.activateStatus == true)
                        //{
                        //    var wholeresult = db.Account_Master.Where(m => m.Client_ID == model.Client_ID);
                        //    if (wholeresult != null)
                        //    {
                        //        foreach (var record in wholeresult)
                        //        {
                        //            record.IsActive = true;
                        //        }
                        //    }

                        //}
                        //else
                        //if (model.activateStatus == false)
                        //{
                        //    var wholeresult = db.Account_Master.Where(m => m.Client_ID == model.Client_ID);
                        //    if (wholeresult != null)
                        //    {
                        //        foreach (var record in wholeresult)
                        //        {
                        //            record.IsActive = false;
                        //        }

                        //    }

                        //}
                       
                        Client_Master CheckEmailID = db.Client_Master.Where(x => x.Email == model.Email && x.IsActive == true && x.Client_ID != model.Client_ID).FirstOrDefault();
                        Account_Master CheckAcEmailID = db.Account_Master.Where(x => x.E_Mail == model.Email && x.IsActive == true && x.Client_ID != model.Client_ID).FirstOrDefault();
                        if (CheckEmailID != null || CheckAcEmailID != null)
                        {
                            response.Message = "Email ID already exists";
                            response.ResultCode = -1;
                            return response;
                        }
                    }

                    CMtableObj.Client_Company = model.Client_Company;
                    CMtableObj.Contact_person = model.Contact_person;
                    CMtableObj.Address = model.Address;
                    CMtableObj.Address2 = model.Address2;
                    CMtableObj.City = model.City;
                    CMtableObj.State = model.Client_State;
                    CMtableObj.ZipCode = model.ZipCode;
                    CMtableObj.Phone = model.Phone;
                    CMtableObj.Mobile = model.Mobile;
                    CMtableObj.Fax = model.Fax;
                    CMtableObj.Email = model.Email;
                    CMtableObj.Distributor_ID = model.DistributorID;

                    CMtableObj.Sender_EmailAddress = model.Sender_EmailAddress;
                    CMtableObj.Sender_EmailPassword = model.Sender_EmailPassword;
                    CMtableObj.DomainName = model.DomainName;
                    CMtableObj.SMTP_Host = model.SMTP_Host;
                    CMtableObj.SMTP_Port = model.SMTP_Port;
                    CMtableObj.JobIDPreffix = model.JobIDPreffix;
                    CMtableObj.AutoSaveTime_InSecs = model.AutoSaveTime_InSecs;
                    if (isupdate == false)
                    {
                        CMtableObj.IsActive = true;
                        CMtableObj.Client_Logo = model.Client_Logo;
                        CMtableObj.Created_By = "Admin";
                        CMtableObj.Created_Date = DateTime.Now;

                        db.Client_Master.Add(CMtableObj);

                    }
                    else
                    {
                        if (model.Client_Logo != null)
                        {
                            CMtableObj.Client_Logo = model.Client_Logo;
                        }
                        CMtableObj.Updated_By = "Admin";
                        CMtableObj.Updated_Date = DateTime.Now;
                    }
                    int resultcount = db.SaveChanges();

                    if (resultcount > 0)
                    {
                        int clientID = (from m in db.Client_Master where m.Client_Company == model.Client_Company && m.IsActive == true select m.Client_ID).FirstOrDefault();

                        //using (ElectricEaseEntitiesContext database = new ElectricEaseEntitiesContext())
                        //{
                        bool Isupdate = true;

                        Account_Master AMTblObj = db.Account_Master.Where(x => x.Client_ID == CMtableObj.Client_ID && x.CreatedBy_SuperUser == true).FirstOrDefault();

                        if (AMTblObj != null)
                        {
                            Isupdate = true;
                        }
                        else
                        {
                            Isupdate = false;
                            AMTblObj = new Account_Master();
                        }
                        AMTblObj.First_Name = model.Contact_person;
                        AMTblObj.Phone = model.Phone;
                        AMTblObj.E_Mail = model.Email;
                        AMTblObj.Site_Administrator = true;
                        AMTblObj.Account_Administrator = true;
                        AMTblObj.Job_Administrator = true;
                        AMTblObj.Part_Administrator = true;
                        AMTblObj.Labor_Administrator = true;
                        AMTblObj.Legal_Adminstrator = true;
                        AMTblObj.Assembly_Administrator = true;
                        AMTblObj.Job_Description_Report = true;
                        AMTblObj.Client_Estimation_Report = true;
                        AMTblObj.UserColor = "#5b94c7";
                        if (Isupdate == false)
                        {
                            AMTblObj.User_ID = model.SuperUser;
                            //AMTblObj.Client_ID = clientID;
                            AMTblObj.Client_ID = CMtableObj.Client_ID;
                            AMTblObj.Photo = model.Client_Logo;
                            AMTblObj.Password = model.SuperUser_Pwd;
                            AMTblObj.Created_By = "Admin";
                            AMTblObj.Created_Date = DateTime.Now;
                            AMTblObj.Updated_By = "Admin";
                            AMTblObj.Updated_Date = DateTime.Now;
                            AMTblObj.IsActive = true;
                            AMTblObj.CreatedBy_SuperUser = true;
                            db.Account_Master.Add(AMTblObj);
                        }
                        else
                        {
                            if (model.SuperUser_Pwd != null)
                            {
                                AMTblObj.Password = model.SuperUser_Pwd;
                            }
                            if (model.Client_Logo != null)
                            {
                                AMTblObj.Photo = model.Client_Logo;
                            }
                            AMTblObj.Updated_By = "Admin";
                            AMTblObj.Updated_Date = DateTime.Now;
                        }
                        int result = db.SaveChanges();
                        if (result > 0)
                        {
                            response.Data = result;
                            if (isupdate == false)
                            {
                                response.Message = "“Client details” are added successfully.";
                                logs.WriteLog("New Client Added Successfully - Admin", 0, "Admin");
                            }
                            else
                            {
                                response.Message = "“Client details” are updated successfully.";
                                logs.WriteLog("New Client Updated Successfully - Admin", 0, "Admin");
                            }
                            response.ResultCode = result;
                            return response;
                        }
                        else
                        {
                            int failureClientID = (from m in db.Client_Master where m.Client_Company == model.Client_Company && m.IsActive == true select m.Client_ID).FirstOrDefault();
                            var failrecord = from m in db.Client_Master where m.Client_ID == failureClientID select m;

                            foreach (var detail in failrecord)
                            {
                                db.Client_Master.Remove(detail);
                            }
                            int deletecount = db.SaveChanges();
                            response.Data = -1;
                            if (isupdate == false)
                            {
                                response.Message = "Error while updating “Client Details”!";
                            }
                            else
                            {
                                response.Message = "Error while “Client registration”!";
                            }

                            response.ResultCode = result;
                            return response;
                        }
                        //}


                    }
                    else
                    {
                        response.Data = -1;
                        response.Message = "Error while “Client registration”!";

                        response.ResultCode = resultcount;
                        return response;
                    }


                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                logs.WriteLog("[Exception] While Add/update Client -Admin " + "[Error Msg- " + ex.Message + " ]", 0, "Admin");
                return response;

            }

        }


        public ServiceResultList<Client_MasterInfolist> GetClientList()
        {

            //Client_MasterInfo listobj = new Client_MasterInfo();
            ServiceResultList<Client_MasterInfolist> response = new ServiceResultList<Client_MasterInfolist>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    response.ListData = (from m in db.Client_Master
                                             //  where m.IsActive == true
                                         select new Client_MasterInfolist()
                                         {
                                             Client_ID = m.Client_ID,
                                             Client_Company = m.Client_Company,
                                             ContactPerson = m.Contact_person,
                                             Address = m.Address,
                                             Address2 = m.Address2,
                                             City = m.City,
                                             State = m.State,
                                             ZipCode = m.ZipCode,
                                             Phone = m.Phone,
                                             Mobile = m.Mobile,
                                             Fax = m.Fax,
                                             Email = m.Email,
                                             DistributorID = m.Distributor_ID ?? 0,
                                             Distributorname = (from dt in db.Distributor_Master where dt.ID == m.Distributor_ID select dt.Company).FirstOrDefault(),
                                             Contact_person = m.Contact_person,
                                             Created_By = m.Created_By,
                                             Created_Date = m.Created_Date,
                                             Updated_By = m.Updated_By,
                                             Updated_Date = m.Updated_Date,
                                             IsActive = m.IsActive

                                         }).ToList();

                    // listobj.ClientList = listObj1;

                    if (response.ListData != null)
                    {
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

        //To Delete a Client in ClienMaster
        public ServiceResult<int> DeleteClient(int ClientID)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            int resultcount;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Client_Master CMtblObj = new Client_Master();
                    CMtblObj = db.Client_Master.Where(m => m.Client_ID == ClientID).FirstOrDefault();
                    CMtblObj.IsActive = false;
                    resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {

                        using (ElectricEaseEntitiesContext data = new ElectricEaseEntitiesContext())
                        {
                            var result = (from m in data.Client_Master where m.Client_ID == ClientID select m).FirstOrDefault();
                            if (result.IsActive == false)
                            {
                                Account_Master AMtblObj = new Account_Master();
                                var wholeresult = data.Account_Master.Where(m => m.Client_ID == ClientID);
                                foreach (var record in wholeresult)
                                {
                                    record.IsActive = false;

                                }

                            }

                            int changecount = data.SaveChanges();

                            if (changecount > 0)
                            {
                                response.Data = resultcount;
                                response.ResultCode = resultcount;
                                response.Message = "Success";
                                logs.WriteLog("New Client Deleted Successfully- " + "Admin", 0, "Admin");
                                return response;
                            }
                            else
                            {
                                response.Data = -1;
                                response.ResultCode = -1;
                                response.Message = "An error occurred while “Delete process”!";
                                return response;

                            }

                        }



                    }
                    else
                    {
                        response.Data = -1;
                        response.ResultCode = -1;
                        response.Message = " An error occurred while “Delete process”!";

                        return response;
                    }

                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                logs.WriteLog("[Exception] While Delete Client -Admin " + "[Error Msg- " + ex.Message + " ]", 0, "Admin");
                return response;

            }

        }
        //Here to Get Client Detail For Update Profile
        public ServiceResult<EditClient_MasterInfo> GetClientDetail(int ClientID)
        {
            ServiceResult<EditClient_MasterInfo> response = new ServiceResult<EditClient_MasterInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Client_Master CMtblobj = new Client_Master();
                    CMtblobj = (from m in db.Client_Master where m.Client_ID == ClientID && m.IsActive == true select m).FirstOrDefault();
                    if (CMtblobj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“Client” doesn't exist!";
                        return response;
                    }
                    response.Data = new EditClient_MasterInfo()
                    {
                        Client_ID = CMtblobj.Client_ID,
                        Client_Company = CMtblobj.Client_Company,
                        Client_Logo = CMtblobj.Client_Logo,
                        Contact_person = CMtblobj.Contact_person,
                        Address = CMtblobj.Address,
                        Address2 = CMtblobj.Address2,
                        City = CMtblobj.City,
                        State = CMtblobj.State,
                        ZipCode = CMtblobj.ZipCode,
                        Phone = CMtblobj.Phone,
                        Mobile = CMtblobj.Mobile,
                        Fax = CMtblobj.Fax,
                        Email = CMtblobj.Email,

                        Sender_EmailAddress = CMtblobj.Sender_EmailAddress,
                        Sender_EmailPassword = CMtblobj.Sender_EmailPassword,
                        DomainName = CMtblobj.DomainName,
                        SMTP_Host = CMtblobj.SMTP_Host,
                        SMTP_Port = CMtblobj.SMTP_Port,
                        JobIDPreffix = CMtblobj.JobIDPreffix,
                        AutoSaveTime_InSecs = CMtblobj.AutoSaveTime_InSecs,
                        Created_By = CMtblobj.Created_By,
                        Created_Date = CMtblobj.Created_Date,
                        Updated_By = CMtblobj.Updated_By,
                        Updated_Date = CMtblobj.Updated_Date
                    };
                    using (ElectricEaseEntitiesContext data = new ElectricEaseEntitiesContext())
                    {
                        Account_Master AMtblobj = new Account_Master();
                        AMtblobj = (from m in db.Account_Master where m.Client_ID == ClientID && m.IsActive == true select m).FirstOrDefault();
                        response.Data.SuperUser = AMtblobj.User_ID;
                        response.Data.SuperUser_Pwd = AMtblobj.Password;
                    }

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

        public ServiceResult<Client_MasterInfo> GetClientDetails(int ClientID)
        {
            ServiceResult<Client_MasterInfo> response = new ServiceResult<Client_MasterInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Client_Master CMtblobj = new Client_Master();
                    CMtblobj = (from m in db.Client_Master where m.Client_ID == ClientID select m).FirstOrDefault();
                    if (CMtblobj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“Client” doesn't exist!";
                        return response;
                    }
                    response.Data = new Client_MasterInfo()
                    {
                        Client_ID = CMtblobj.Client_ID,
                        Client_Company = CMtblobj.Client_Company,
                        Client_Logo = CMtblobj.Client_Logo,
                        Contact_person = CMtblobj.Contact_person,
                        Address = CMtblobj.Address,
                        Address2 = CMtblobj.Address2,
                        City = CMtblobj.City,
                        Client_State = CMtblobj.State,
                        ZipCode = CMtblobj.ZipCode,
                        Phone = CMtblobj.Phone,
                        Mobile = CMtblobj.Mobile,
                        Fax = CMtblobj.Fax,
                        Email = CMtblobj.Email,
                        DistributorID = CMtblobj.Distributor_ID ?? 0,
                        IsActive = CMtblobj.IsActive ?? false,
                        activateStatus = CMtblobj.IsActive ?? false,

                        Sender_EmailAddress = CMtblobj.Sender_EmailAddress,
                        Sender_EmailPassword = CMtblobj.Sender_EmailPassword,
                        DomainName = CMtblobj.DomainName,
                        SMTP_Host = CMtblobj.SMTP_Host,
                        SMTP_Port = CMtblobj.SMTP_Port,
                        JobIDPreffix = CMtblobj.JobIDPreffix,
                        AutoSaveTime_InSecs = CMtblobj.AutoSaveTime_InSecs,
                        Created_By = CMtblobj.Created_By,
                        Created_Date = CMtblobj.Created_Date,
                        Updated_By = CMtblobj.Updated_By,
                        Updated_Date = CMtblobj.Updated_Date
                    };
                    using (ElectricEaseEntitiesContext data = new ElectricEaseEntitiesContext())
                    {
                        Account_Master AMtblobj = new Account_Master();
                        AMtblobj = (from m in db.Account_Master where m.Client_ID == ClientID && m.CreatedBy_SuperUser == true select m).FirstOrDefault();
                        response.Data.SuperUser = AMtblobj.User_ID;
                        response.Data.SuperUser_Pwd = AMtblobj.Password;
                    }

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

        //Here to Update a Client Information
        public ServiceResult<int> SaveClientDetails(EditClient_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Client_Master CMtblobj = new Client_Master();

                    CMtblobj = (from m in db.Client_Master where m.Client_ID == model.Client_ID select m).FirstOrDefault();
                    if (CMtblobj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“Client” doesn't exist!";
                    }

                    CMtblobj.Client_ID = model.Client_ID;
                    CMtblobj.Client_Company = model.Client_Company;
                    if (model.Client_Logo != null)
                    {
                        CMtblobj.Client_Logo = model.Client_Logo;
                    }

                    CMtblobj.Contact_person = model.Contact_person;
                    CMtblobj.Address = model.Address;
                    CMtblobj.Address2 = model.Address2;
                    CMtblobj.City = model.City;
                    CMtblobj.State = model.State;
                    CMtblobj.ZipCode = model.ZipCode;
                    CMtblobj.Phone = model.Phone;
                    CMtblobj.Mobile = model.Mobile;
                    CMtblobj.Fax = model.Fax;
                    CMtblobj.Email = model.Email;
                    CMtblobj.IsActive = true;
                    CMtblobj.Sender_EmailAddress = model.Sender_EmailAddress;
                    CMtblobj.Sender_EmailPassword = model.Sender_EmailPassword;
                    CMtblobj.DomainName = model.DomainName;
                    CMtblobj.SMTP_Host = model.SMTP_Host;
                    CMtblobj.SMTP_Port = model.SMTP_Port;
                    CMtblobj.JobIDPreffix = model.JobIDPreffix;
                    CMtblobj.AutoSaveTime_InSecs = model.AutoSaveTime_InSecs;
                    CMtblobj.Updated_By = "Admin";
                    CMtblobj.Updated_Date = DateTime.Now;
                    int resultcount = db.SaveChanges();

                    using (ElectricEaseEntitiesContext data = new ElectricEaseEntitiesContext())
                    {
                        Account_Master AMtblobj = new Account_Master();
                        AMtblobj = (from m in db.Account_Master where m.Client_ID == model.Client_ID && m.IsActive == true select m).FirstOrDefault();
                        AMtblobj.User_ID = model.SuperUser;
                        if (model.SuperUser_Pwd != null)
                        {
                            AMtblobj.Password = model.SuperUser_Pwd;
                        }
                        if (model.Client_Logo != null)
                        {
                            AMtblobj.Photo = model.Client_Logo;
                        }
                        AMtblobj.First_Name = model.Contact_person;
                        AMtblobj.Phone = model.Phone;
                        AMtblobj.E_Mail = model.Email;
                        AMtblobj.Site_Administrator = true;
                        AMtblobj.Account_Administrator = true;
                        AMtblobj.Job_Administrator = true;
                        AMtblobj.Part_Administrator = true;
                        AMtblobj.Labor_Administrator = true;
                        AMtblobj.Legal_Adminstrator = true;
                        AMtblobj.Assembly_Administrator = true;
                        AMtblobj.Job_Description_Report = true;
                        AMtblobj.Client_Estimation_Report = true;
                        AMtblobj.Updated_By = "Admin";
                        AMtblobj.Updated_Date = DateTime.Now;
                        int resultcount2 = db.SaveChanges();

                        if (resultcount2 > 0 || resultcount > 0)
                        {
                            response.Data = resultcount2;
                            response.ResultCode = 1;
                            response.Message = "“Profile” updated successfully.";
                        }
                        else
                        {
                            response.Data = resultcount2;
                            response.ResultCode = 0;
                            response.Message = "“No” changes found in your profile!";
                        }

                    }
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

        //here to  update  Login new password
        public ServiceResult<int> UpdatePassword(string newpassword, string confirmpassword, string Email)
        {
            ServiceResult<int> response = new ServiceResult<int>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Account_Master ActblOBJ = new Account_Master();
                    ActblOBJ = (from m in db.Account_Master where m.E_Mail == Email && m.IsActive == true select m).FirstOrDefault();
                    ActblOBJ.Password = newpassword;
                    int resultcount = db.SaveChanges();

                    if (resultcount > 0)
                    {
                        response.ResultCode = 1;
                        response.Message = "“Password” updated successfully.";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = 0;
                        response.Message = "Please provide a new password!";
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

        //Here to get Nimber of pars,Job open ,close count  Details
        public ServiceResult<CountDetails> GetCountDetails(int ClientID)
        {
            ServiceResult<CountDetails> response = new ServiceResult<CountDetails>();
            CountDetails model = new CountDetails();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    int PartsCount = (from m in db.Parts_Details where m.Client_Id == ClientID && m.IsActive == true && m.Part_Category != null select m).Count();
                    int AssembliesCount = (from m in db.Assemblies_Master where m.Client_Id == ClientID && m.Isactive == true && m.Assemblies_Category != null select m).Count();
                    // model.Parts_Count = Parts_Count;
                    response.Data = new CountDetails()
                    {
                        Parts_Count = PartsCount,
                        Assembly_Count = AssembliesCount
                    };



                    List<Job_Master> obj = db.Job_Master.Where(x => x.Client_Id == ClientID && x.Isactive == true).ToList();
                    var data = (from o in obj
                                group o by o.Job_Status into grp
                                select new
                                {
                                    name = grp.Key,
                                    total = grp.Count()
                                }).ToList();
                    foreach (var item in data)
                    {
                        if (item.name == "Open")
                        {
                            response.Data.JobOpen_Count = item.total;
                        }

                        if (item.name == "Closed")
                        {
                            response.Data.JobClosed_Count = item.total;
                        }
                    }
                    response.ResultCode = 1;
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

        public string checkisexistcomapnyanduser(string ClientCompany, string UserID)
        {
            string message = "";
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    // Client_Master ClientIsExit = new Client_Master();

                    var ClientIsExit = db.Client_Master.Any(x => x.Client_Company == ClientCompany || x.Email == UserID);

                    var UserIsExit = db.Account_Master.Any(x => x.User_ID == UserID);
                    if (ClientIsExit == true || UserIsExit == true)
                    {
                        if (ClientIsExit == true)
                        {
                            message = "Company or Email ID Is Already Registered Please change Name";
                        }
                        else if (UserIsExit == true)
                        {
                            message = "Email ID is Already Registerd,please change your ID";
                        }

                    }
                    return message;



                }
            }
            catch (Exception ex)
            {
                return message;
            }
        }

        public string RegisterNewuser(Client_MasterInfo model)
        {
            //string stat = "";
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Client_Master CMtableObj = new Client_Master();
                    Account_Master AMTblObj = new Account_Master();
                    String randomPassword = GeneratePassword(3, 3, 3);
                    CMtableObj.Client_Company = model.Client_Company;
                    CMtableObj.Contact_person = model.Contact_person;
                    CMtableObj.Phone = model.Phone;
                    CMtableObj.Mobile = model.Mobile;
                    CMtableObj.Email = model.Email;
                    CMtableObj.Distributor_ID = -1;
                    CMtableObj.Client_Logo = model.Client_Logo;
                    CMtableObj.IsActive = true;
                    CMtableObj.Created_By = "Admin";
                    CMtableObj.Created_Date = DateTime.Now;
                    db.Client_Master.Add(CMtableObj);
                    int resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        AMTblObj.First_Name = model.FirstName;
                        AMTblObj.Last_Name = model.LastName;
                        AMTblObj.Phone = model.Phone;
                        AMTblObj.E_Mail = model.Email;
                        AMTblObj.User_ID = model.Email;
                        //AMTblObj.Client_ID = clientID;
                        AMTblObj.Client_ID = CMtableObj.Client_ID;
                        AMTblObj.Photo = model.Client_Logo;
                        AMTblObj.Password = randomPassword;
                        AMTblObj.Created_By = "Admin";
                        AMTblObj.Created_Date = DateTime.Now;
                        AMTblObj.Updated_By = "Admin";
                        AMTblObj.Updated_Date = DateTime.Now;
                        AMTblObj.IsActive = true;
                        AMTblObj.CreatedBy_SuperUser = true;
                        AMTblObj.IsFirst = true;
                        AMTblObj.Subscribe = true;
                        db.Account_Master.Add(AMTblObj);
                        db.SaveChanges();
                    }
                    EmailLog _email = new EmailLog();
                    _email.EmailTo = model.Email;
                    _email.Subject = "Thank you for registering with Electric Ease!";
                    _email.BodyContent = "<html><body>";
                    _email.BodyContent += "<center><img src='https://electric-ease.com/wp-content/uploads/logo.png' width='300px'/></center>";
                    _email.BodyContent += "<p>Hello<strong> " + model.Contact_person + " " + model.LastName + "</strong></p> ";
                    _email.BodyContent += "<p>Thank you for registering with Electric Ease!</p> ";
                    _email.BodyContent += "<p>Here are your account login credentials:</p><br/>";
                    _email.BodyContent += "<p>Username : <strong>" + model.Email + "</strong></p> ";
                    _email.BodyContent += "<p>Password : <strong>" + randomPassword + "</strong></p><br/>";
                    _email.BodyContent += "<p>Your password can be changed any time after you log in by going into your User Profile.</p><br/>";
                    _email.BodyContent += "<p style='color: rgb(98,98,98);font-size: 14px; '>***Please note that your Parts & Assemblies databases will not show any info for roughly 24 hours.  Once your account is fully activated on our end, you will see parts & assemblies in your database for you to work with.  Please contact us if you have any questions regarding this setup.***<br/>";
                    _email.BodyContent += "<p>Please contact us at <a href='Support@Electric-Ease.com'>Support@Electric-Ease.com</a> if you have any issues at all regarding your login or user setup.</p><br/>";
                    _email.BodyContent += "<p>Sincerely,</p> ";
                    _email.BodyContent += "<p>All of us at Electric Ease!</p><br/>";
                    _email.BodyContent += "<center><a target='_blank' href='www.Electric-Ease.com'>www.Electric-Ease.com</a></center>";
                    _email.BodyContent += "</html></body>";
                    EmailService email = new EmailService();
                    email.SendEmail(_email);

                }
                return "success";

            }
            catch (Exception ex)
            {
                return "Fail";
            }
        }
        public static string GeneratePassword(int lowercase, int uppercase, int numerics)
        {
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";

            Random random = new Random();

            string generated = "!";
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            return generated.Replace("!", string.Empty);

        }
        /// <summary>
        /// SaveDistributor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ServiceResult<int> SaveDistributor(Distributor model)
        {
            bool isupdate = false;
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Distributor_Master dstbl = new Distributor_Master();
                    if (model.ID == 0)
                    {
                        dstbl.Company = model.Company;
                        dstbl.No_Of_Users = model.No_Of_Users;
                        dstbl.Company_Url = model.Company_Url;
                        dstbl.Email = model.Email;
                        dstbl.Expiry_Date = model.Expiry_Date;
                        dstbl.IsActive = model.IsActive;
                        dstbl.CompanyLogo = model.CompanyLogo;
                        dstbl.Created_Date = DateTime.Now;
                        db.Distributor_Master.Add(dstbl);
                    }
                    else
                    {
                        var distributors = db.Distributor_Master.Where(x => x.ID == model.ID).FirstOrDefault();
                        if (distributors != null)
                        {
                            distributors.Company = model.Company;
                            distributors.No_Of_Users = model.No_Of_Users;
                            distributors.Company_Url = model.Company_Url;
                            distributors.Email = model.Email;
                            distributors.Expiry_Date = model.Expiry_Date;
                            distributors.IsActive = model.IsActive;
                            distributors.CompanyLogo = model.CompanyLogo;
                            dstbl.Updated_Date = DateTime.Now;
                            isupdate = true;
                        }
                    }
                    int result = db.SaveChanges();
                    if (result > 0)
                    {
                        response.Data = result;
                        if (isupdate == false)
                        {
                            response.Message = "“Distributor Details” are added successfully.";
                            logs.WriteLog("New Distributor Added Successfully - Admin", 0, "Admin");
                        }
                        else
                        {
                            response.Message = "“Distributor Details” are updated successfully.";
                            logs.WriteLog("New Distributor Updated Successfully - Admin", 0, "Admin");
                        }
                        response.ResultCode = result;

                    }
                    else
                    {
                        response.Message = "“Distributor Details” are updated successfully.";
                        logs.WriteLog("New Distributor Updated Successfully - Admin", 0, "Admin");
                        response.ResultCode = result;
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                return response;
            }
        }

        /// <summary>
        /// IsDistributorExiste
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ServiceResult<bool> IsDistributorExiste(Distributor model)
        {
            ServiceResult<bool> isexist = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    if (model.Company != "")
                    {
                        isexist.Data = db.Distributor_Master.Where(x => x.Company == model.Company).Any();
                        if (isexist.Data == true)
                        {
                            isexist.Message = "“Distributor Name” is repeated that “Distributor Name” is taken. Try another.";
                            return isexist;
                        }

                    }
                    if (model.Email != "")
                    {
                        isexist.Data = db.Distributor_Master.Where(x => x.Email == model.Email).Any();
                        if (isexist.Data == true)
                        {
                            isexist.Message = "“Distributor Email Id” is repeated that “Distributor Email ID” is taken. Try another.";
                            return isexist;
                        }

                    }
                    if (model.Company_Url != "")
                    {
                        isexist.Data = db.Distributor_Master.Where(x => x.Email == model.Email).Any();
                        if (isexist.Data == true)
                        {
                            isexist.Message = "Distributor Company URL is repeated that “Distributor Company URL” is taken. Try another.";
                            return isexist;
                        }

                    }

                }
                return isexist;
            }
            catch
            {
                return isexist;
            }
        }

        /// <summary>
        /// GetDistributorlist
        /// </summary>
        /// <returns></returns>
        public ServiceResultList<Distributor> GetDistributorlist()
        {
            ServiceResultList<Distributor> model = new ServiceResultList<Distributor>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    model.ListData = (from dt in db.Distributor_Master
                                      select new Distributor()
                                      {
                                          ID = dt.ID,
                                          Company = dt.Company,
                                          No_Of_Users = dt.No_Of_Users,
                                          Email = dt.Email,
                                          Company_Url = dt.Company_Url,
                                          IsActive = dt.IsActive ?? false,
                                          Expiry_Date = dt.Expiry_Date ?? DateTime.Now,
                                          CreatedDate = dt.Created_Date ?? DateTime.Now,

                                      }).OrderByDescending(x => x.ID).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                return model;
            }
        }
        /// <summary>
        /// EditDistributor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult<Distributor> EditDistributor(int id)
        {
            ServiceResult<Distributor> responce = new ServiceResult<Distributor>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    responce.Data = (from dt in db.Distributor_Master
                                     where dt.ID == id
                                     select new Distributor()
                                     {
                                         ID = dt.ID,
                                         Company = dt.Company,
                                         No_Of_Users = dt.No_Of_Users,
                                         Company_Url = dt.Company_Url,
                                         Expiry_Date = dt.Expiry_Date ?? DateTime.Now,
                                         Email = dt.Email,
                                         CompanyLogo = dt.CompanyLogo,
                                         IsActive = dt.IsActive ?? false
                                     }).FirstOrDefault();
                }
                return responce;
            }
            catch (Exception ex)
            {
                return responce;
            }
        }

        public ServiceResultList<distributordropdown> distributorList()
        {

            ServiceResultList<distributordropdown> model = new ServiceResultList<distributordropdown>();
            //try
            //{
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    model.ListData = (from dt in db.Distributor_Master
                                      where dt.IsActive == true
                                      select new distributordropdown()
                                      {
                                          value = dt.ID,
                                          Name = dt.Company
                                      }).ToList();
                    model.ListData = model.ListData.OrderBy(x => x.Name).ToList();
                    //var item = (from client in db.Client_Master
                    //            where client.IsActive == true && (client.Distributor_ID == 0 || client.Distributor_ID == -1)
                    //            select new distributordropdown()
                    //            {
                    //                value = client.Distributor_ID ?? 0,
                    //                Name = "STANDARD"
                    //            }).FirstOrDefault();
                    //if (item != null)
                    //{
                    var item = new distributordropdown()
                    {
                        value = -1,
                        Name = "STANDARD"
                    };
                    model.ListData.Insert(0, item);
                    //  }
                }

                return model;
            //}
            //catch (Exception ex)
            //{
            //    return model;
            //}
        }

        public ServiceResultList<Standaredclient> StandaredUsers()
        {

            ServiceResultList<Standaredclient> model = new ServiceResultList<Standaredclient>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    model.ListData = (from dt in db.Client_Master
                                      where dt.IsActive == true && (dt.Distributor_ID == 0 || dt.Distributor_ID == -1)
                                      select new Standaredclient()
                                      {
                                          value = dt.Client_ID,
                                          Name = dt.Client_Company
                                      }).OrderBy(x => x.Name).ToList();
                }
                model.ListData = model.ListData.OrderBy(x => x.Name).ToList();
                return model;
            }
            catch (Exception ex)
            {
                return model;
            }
        }
        /// <summary>
        /// DistributorClients
        /// </summary>
        /// <param name="Did"></param>
        /// <returns></returns>
        public ServiceResultList<Standaredclient> DistributorClients(int Did)
        {

            ServiceResultList<Standaredclient> model = new ServiceResultList<Standaredclient>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    model.ListData = (from dt in db.Client_Master
                                      where dt.Distributor_ID == Did && dt.IsActive == true
                                      select new Standaredclient()
                                      {
                                          value = dt.Client_ID,
                                          Name = dt.Client_Company
                                      }).ToList();
                }
                model.ListData = model.ListData.OrderBy(x => x.value).ToList();
                return model;
            }
            catch (Exception ex)
            {
                return model;
            }
        }

    }
}
