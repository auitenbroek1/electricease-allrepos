using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectricEase.Data;
using System.Threading.Tasks;
using ElectricEase.Data.DataBase;
using ElectricEase.Models;

namespace ElectricEase.Data
{
    public class ElectricData
    {
       


      

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
                    model.Client_ID=AMTblObj.Client_ID;
                    model.Account_Administrator = AMTblObj.Account_Administrator;
                    model.CreatedBy_SuperUser = AMTblObj.CreatedBy_SuperUser;
                    model.Part_Administrator = AMTblObj.Part_Administrator;

                    model.MyClientlogo = (from m in db.Client_Master where m.Client_ID == model.Client_ID select m.Client_Logo).FirstOrDefault();
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
            bool uservalid;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    uservalid = db.Account_Master.Any(m => m.User_ID == model.UserName && m.Password == model.Password && m.IsActive == true);

                    return uservalid;
                }
            }
            catch (Exception ex)
            {
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



                    if(uservalid==true)
                    {
                        Account_Master AMtblObj = new Account_Master();
                        AMtblObj = (from m in db.Account_Master where m.E_Mail == Email && m.IsActive == true select m).FirstOrDefault();


                        response.Data = new ForgotPasswordModel() { 
                        
                            NAME=AMtblObj.First_Name,
                            User_ID=AMtblObj.User_ID,
                            PASSWORDTOKEN=AMtblObj.Password,
                            EMAIL=AMtblObj.E_Mail
                        };

                        response.Message="ValidUser";
                        response.ResultCode = 1;
                        return response;
                    }
                    else{
                        
                        response.Message = "User Not Exist";
                        response.ResultCode = -1;
                        return response;
                    }
                    
                }
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                response.ResultCode = -1;
                return response;
            }


        }
        

        public  ServiceResult<int> AddClientMaster(Client_MasterInfo model)
        {

            ServiceResult<int> response = new ServiceResult<int>();
            int resultcount;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    
                    Client_Master CMtableObj = new Client_Master();
                    Account_Master AMtTblObj = new Account_Master();

                    // here To check email is is exist already
                    Client_Master CheckEmailID = db.Client_Master.Where(x => x.Email == model.Email).FirstOrDefault();
                    if (CheckEmailID != null)
                    {
                        response.Message = "Email Id is already exists!";
                        return response;
                    }
                    AMtTblObj = db.Account_Master.Where(x => x.User_ID == model.SuperUser).FirstOrDefault();
                    if (AMtTblObj!=null)
                    {
                        response.Message = "UserId is Already Exists!";
                        return response;
                    }
                    

                    CMtableObj.Client_Company = model.Client_Company;
                    CMtableObj.Client_Logo = model.Client_Logo;
                    CMtableObj.Contact_person = model.Contact_person;
                    CMtableObj.Address = model.Address;
                    CMtableObj.Address2 = model.Address2;
                    CMtableObj.City = model.City;
                    CMtableObj.State = model.State;
                    CMtableObj.ZipCode = model.ZipCode;
                    CMtableObj.Phone = model.Phone;
                    CMtableObj.Mobile = model.Mobile;
                    CMtableObj.Fax = model.Fax;
                    CMtableObj.Email = model.Email;
                    CMtableObj.Created_By = "Admin";
                    CMtableObj.Created_Date = DateTime.Now;
                    CMtableObj.Updated_By = "Admin";
                    CMtableObj.Updated_Date = DateTime.Now;
                    CMtableObj.IsActive = true;

                    db.Client_Master.Add(CMtableObj);
                    resultcount = db.SaveChanges();

                    if (resultcount > 0)
                    {
                        int clientID = (from m in db.Client_Master where m.Client_Company == model.Client_Company && m.IsActive == true select m.Client_ID).FirstOrDefault();

                        using (ElectricEaseEntitiesContext database = new ElectricEaseEntitiesContext())
                        {
                            Account_Master AMTblObj = new Account_Master();

                            AMTblObj.Client_ID = clientID;
                            AMTblObj.User_ID = model.SuperUser;
                            AMTblObj.Password = model.SuperUser_Pwd;
                            AMTblObj.First_Name = model.Contact_person;
                            AMTblObj.Phone = model.Phone;
                            AMTblObj.Photo = model.Client_Logo;
                            AMTblObj.E_Mail = model.Email;
                            AMTblObj.Site_Administrator = true;
                            AMTblObj.Account_Administrator = true;
                            AMTblObj.Job_Administrator = true;
                            AMTblObj.Part_Administrator = true;
                            AMTblObj.Labor_Administrator = true;
                            AMTblObj.Legal_Adminstrator = true;
                            AMTblObj.Created_By = "Admin";
                            AMTblObj.Created_Date = DateTime.Now;
                            AMTblObj.Updated_By = "Admin";
                            AMTblObj.Updated_Date = DateTime.Now;
                            AMTblObj.IsActive = true;
                            AMTblObj.CreatedBy_SuperUser = true;
                            

                            database.Account_Master.Add(AMTblObj);
                            int result = database.SaveChanges();

                            if (result > 0)
                            {
                                response.Data = result;
                                response.Message = "New Client Has been Added Successfully!";
                                response.ResultCode = result;
                                return response;
                            }
                            else
                            {
                                response.Data = -1;
                                response.Message = "Error while Client registration";
                                response.ResultCode = result;
                                return response;
                            }
                        }


                    }
                    else
                    {
                        response.Data = -1;
                        response.Message = "Error while Client registration";
                        response.ResultCode = resultcount;
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


        public List<Client_MasterInfolist> GetClientList()
        {
            // List<Client_MasterInfo> listObj = new List<Client_MasterInfo>();
            Client_MasterInfo listobj = new Client_MasterInfo();
            List<Client_MasterInfolist> listObj1 = new List<Client_MasterInfolist>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    listObj1 = (from m in db.Client_Master
                                where m.IsActive == true
                                select new Client_MasterInfolist()
                                {
                                    Client_ID = m.Client_ID,
                                    Client_Company = m.Client_Company,
                                    Address = m.Address,
                                    Address2 = m.Address2,
                                    City = m.City,
                                    State = m.State,
                                    ZipCode = m.ZipCode,
                                    Phone = m.Phone,
                                    Mobile = m.Mobile,
                                    Fax = m.Fax,
                                    Email = m.Email,
                                    Contact_person = m.Contact_person,
                                    Created_By = m.Created_By,
                                    Created_Date = m.Created_Date,
                                    Updated_By = m.Updated_By,
                                    Updated_Date = m.Updated_Date
                                }).ToList();

                    // listobj.ClientList = listObj1;

                    if (listObj1 != null)
                    {
                        return listObj1;

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

        //To Delete a Client in ClienMaster
        public ServiceResult<int> DeleteClient(int ClientID)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            int resultcount;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Client_Master tableObj = new Client_Master();
                    tableObj = db.Client_Master.Where(m => m.Client_ID == ClientID).FirstOrDefault();
                    tableObj.IsActive = false;
                    resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        response.Data = resultcount;
                        response.ResultCode = resultcount;
                        response.Message = "Deleted Successfully";
                        return response;

                    }
                    else
                    {
                        response.Data = -1;
                        response.ResultCode = -1;
                        response.Message = " Error occured while Delete process!";
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
                        response.Message = "Client doesn't exists";
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
                        Created_By=CMtblobj.Created_By,
                        Created_Date=CMtblobj.Created_Date,
                        Updated_By=CMtblobj.Updated_By,
                        Updated_Date=CMtblobj.Updated_Date
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

        //Here to Update a Client Information
        public ServiceResult<int> SaveClientDetails(EditClient_MasterInfo model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Client_Master CMtblobj = new Client_Master();
                   
                    CMtblobj = (from m in db.Client_Master where m.Client_ID == model.Client_ID && m.IsActive == true select m).FirstOrDefault();
                    if (CMtblobj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "Client doesn't exists";
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
                    CMtblobj.Updated_By = "Admin";
                    CMtblobj.Updated_Date=DateTime.Now;
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
                        AMtblobj.Updated_By = "Admin";
                        AMtblobj.Updated_Date = DateTime.Now;
                        int resultcount2 = db.SaveChanges();

                        if (resultcount2 > 0 || resultcount>0)
                        {
                            response.Data = resultcount2;
                            response.ResultCode = 1;
                            response.Message = "Profile Updated Successfully";
                        }
                        else
                        {
                            response.Data = resultcount2;
                            response.ResultCode = 0;
                            response.Message = "No changes found in your profile!";
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


        //--------------------------------------------------------------------------------------
        //Account_Master 

        public List<Account_MasterInfoList> GetAllUserList()
        {
            List<Account_MasterInfoList> listObj1 = new List<Account_MasterInfoList>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    listObj1 = (from m in db.Account_Master
                                where m.IsActive == true 
                                select new Account_MasterInfoList()
                                {
                                    Client_ID = m.Client_ID,
                                    User_ID = m.User_ID,
                                    First_Name = m.First_Name,
                                    Last_Name = m.Last_Name,
                                    Phone = m.Phone,
                                    Photo = m.Photo,
                                    E_Mail = m.E_Mail,
                                    Password = m.Password,
                                    Site_Administrator = m.Site_Administrator,
                                    Account_Administrator = m.Account_Administrator,
                                    Job_Administrator = m.Job_Administrator,
                                    Part_Administrator = m.Part_Administrator,
                                    Labor_Administrator = m.Labor_Administrator,
                                    Legal_Adminstrator = m.Legal_Adminstrator,
                                    CreatedBy_SuperUser = m.CreatedBy_SuperUser,
                                    Created_By = m.Created_By,
                                    Created_Date = m.Created_Date,
                                    Updated_By = m.Updated_By,
                                    Updated_Date = m.Updated_Date
                                }).ToList();

                    // listobj.ClientList = listObj1;
                    if (listObj1 != null)
                    {
                        return listObj1;

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
        public List<Account_MasterInfoList> GetMyUserList( int ClientID)
        {
           
            List<Account_MasterInfoList> listObj1 = new List<Account_MasterInfoList>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    listObj1 = (from m in db.Account_Master
                                where m.IsActive == true && m.Client_ID == ClientID && m.CreatedBy_SuperUser!=true
                                select new Account_MasterInfoList()
                                {
                                    Client_ID = m.Client_ID,
                                    User_ID = m.User_ID,
                                    First_Name = m.First_Name,
                                    Last_Name = m.Last_Name,
                                    Phone = m.Phone,
                                    Photo = m.Photo,
                                    E_Mail = m.E_Mail,
                                    Password = m.Password,
                                    Site_Administrator = m.Site_Administrator,
                                    Account_Administrator = m.Account_Administrator,
                                    Job_Administrator = m.Job_Administrator,
                                    Part_Administrator = m.Part_Administrator,
                                    Labor_Administrator = m.Labor_Administrator,
                                    Legal_Adminstrator = m.Legal_Adminstrator,
                                    CreatedBy_SuperUser=m.CreatedBy_SuperUser,
                                    Created_By = m.Created_By,
                                    Created_Date = m.Created_Date,
                                    Updated_By = m.Updated_By,
                                    Updated_Date = m.Updated_Date
                                }).ToList();

                    // listobj.ClientList = listObj1;
                    if (listObj1 != null)
                    {
                        return listObj1;

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

        public ServiceResult<int> AddNewUserBySP(Account_MasterInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {

                    Account_Master tableObj = new Account_Master();

                    
                    //validate User ID 
                    Account_Master UserIsExit = db.Account_Master.Where(x => x.User_ID == Model.User_ID).FirstOrDefault();
                    if (UserIsExit!=null)
                    {
                        response.Message = "User Id Is Already Registered";
                        return response;
                    }
                    

                    tableObj.Client_ID = Model.Client_ID;
                    tableObj.User_ID = Model.User_ID;
                    tableObj.Password = Model.Password;
                    tableObj.First_Name = Model.First_Name;
                    tableObj.Last_Name = Model.Last_Name;
                    tableObj.Phone = Model.Phone;
                    tableObj.Photo = Model.Photo;
                    tableObj.E_Mail = Model.E_Mail;
                    tableObj.Password = Model.Password;
                    tableObj.Site_Administrator = Model.Site_Administrator ;
                    tableObj.Phone = Model.Phone;
                    tableObj.Account_Administrator = Model.Account_Administrator;
                    tableObj.Job_Administrator = Model.Job_Administrator;
                    tableObj.Part_Administrator = Model.Part_Administrator;
                    tableObj.Labor_Administrator = Model.Labor_Administrator;
                    tableObj.Legal_Adminstrator = Model.Legal_Adminstrator;
                    tableObj.Part_Administrator = Model.Part_Administrator;
                    tableObj.Created_By = Model.Created_By;
                    tableObj.CreatedBy_SuperUser = false;
                    tableObj.Created_Date = DateTime.Now;
                    tableObj.Updated_By = Model.Created_By;
                    tableObj.Updated_Date = DateTime.Now;
                    tableObj.IsActive = true;

                    db.Account_Master.Add(tableObj);
                    int resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        response.ResultCode = 1;
                        response.Message = "New User Has been Added Succefully!";
                        return response;
                    }
                    else
                    {
                       // response.ResultCode = 1;
                        response.Message = "Error while New User Register!";
                        return response;
                    }
                }

            }
            catch(Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResult<EditAccount_MasterInfo> GetUserDetails(string UserID)
        {
            ServiceResult<EditAccount_MasterInfo> response = new ServiceResult<EditAccount_MasterInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Account_Master AMtblobj = new Account_Master();
                    AMtblobj = (from m in db.Account_Master where m.User_ID == UserID && m.IsActive == true select m).FirstOrDefault();
                    if (AMtblobj==null)
                    {
                        response.ResultCode = 0;
                        response.Message = "User dosen't Exists";
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
                        E_Mail = AMtblobj.E_Mail,
                        Password = AMtblobj.Password,
                        Site_Administrator = AMtblobj.Site_Administrator,
                        Account_Administrator = AMtblobj.Account_Administrator,
                        Job_Administrator = AMtblobj.Job_Administrator,
                        Part_Administrator = AMtblobj.Part_Administrator,
                        Labor_Administrator = AMtblobj.Labor_Administrator,
                        Legal_Adminstrator = AMtblobj.Legal_Adminstrator,
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
                    //Check whether given Email Id is already Exists
                   
                    AMtblobj = (from m in db.Account_Master where m.User_ID == Model.User_ID && m.IsActive == true  select m).FirstOrDefault();

                    
                    //AMtblobj.Client_ID = Model.Client_ID;
                    if(Model.User_ID!=null)
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
                   
                    AMtblobj.Site_Administrator = Model.Site_Administrator;
                    AMtblobj.Account_Administrator = Model.Account_Administrator;
                    AMtblobj.Job_Administrator = Model.Job_Administrator;
                    AMtblobj.Part_Administrator = Model.Part_Administrator;
                    AMtblobj.Labor_Administrator = Model.Labor_Administrator;
                    AMtblobj.Legal_Adminstrator = Model.Legal_Adminstrator;
                    AMtblobj.Updated_By = Model.Updated_By;
                    AMtblobj.Updated_Date = DateTime.Now;
                    int resultcount = db.SaveChanges();

                    if (resultcount > 0)
                    {
                        response.ResultCode = resultcount;
                        response.ResultCode = 1;
                        response.Message = "Your Changes Has been Modified Succesfully!";
                        return response;
                    }
                    else
                    {
                        response.Data = resultcount;
                        response.ResultCode = -1;
                        response.Message = "No changes found in your profile!";
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

        public ServiceResult<int> DeleteUserInAM(string UserID)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            int resultcount;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Account_Master tableObj = new Account_Master();
                    tableObj = db.Account_Master.Where(m => m.User_ID == UserID).FirstOrDefault();
                    tableObj.IsActive = false;
                    resultcount = db.SaveChanges();
                    if (resultcount > 0)
                    {
                        response.ResultCode = resultcount;
                        response.Data = resultcount;
                        response.Message = "Deleted Succefully!";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Data = resultcount;
                        response.Message = "Deleted Failed!";
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


        public ServiceResult<EditAccount_MasterInfo> GetMyDetailsInAM(string UserID)
        {
            ServiceResult<EditAccount_MasterInfo> response = new ServiceResult<EditAccount_MasterInfo>();
            try
            {
                using(ElectricEaseEntitiesContext db=new ElectricEaseEntitiesContext())
                {
                    Account_Master AMtblObj = new Account_Master();
                    AMtblObj = (from m in db.Account_Master where m.User_ID == UserID select m).FirstOrDefault();

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
                        Updated_Date = AMtblObj.Updated_Date

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
                    //Check whether given Email Id is already Exists
                    
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
                    AMtblobj.E_Mail = Model.E_Mail;

                    //AMtblobj.Site_Administrator = Model.Site_Administrator;
                    //AMtblobj.Account_Administrator = Model.Account_Administrator;
                    //AMtblobj.Job_Administrator = Model.Job_Administrator;
                    //AMtblobj.Part_Administrator = Model.Part_Administrator;
                    //AMtblobj.Labor_Administrator = Model.Labor_Administrator;
                    //AMtblobj.Legal_Adminstrator = Model.Legal_Adminstrator;
                  //  AMtblobj.CreatedBy_SuperUser = Model.CreatedBy_SuperUser;
                    AMtblobj.Updated_By = Model.Updated_By;
                    AMtblobj.Updated_Date = DateTime.Now;
                    int resultcount = db.SaveChanges();

                    if (resultcount > 0)
                    {
                        response.ResultCode = resultcount;
                        response.ResultCode = 1;
                        response.Message = "Your Changes Has been Modified Succesfully!";
                        return response;
                    }
                    else
                    {
                        response.Data = resultcount;
                        response.ResultCode = -1;
                        response.Message = "No changes found in your profile!";
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
