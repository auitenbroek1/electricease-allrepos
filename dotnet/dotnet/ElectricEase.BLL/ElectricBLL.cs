using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Data;
using ElectricEase.Models;


namespace ElectricEase.BLL
{
    public class ElectricBLL
    {

         private readonly ElectricData ElectricAppData;

        public ElectricBLL()
        {
            ElectricAppData = new ElectricData();

        }

        
        //To Get ClienName for Login Based Rights
        public Account_MasterInfo GetClientName(string username)
        {
            return ElectricAppData.GetClientName(username);
        }

        //To  check whether login user is valid or not
        public  bool CheckValidUser(LoginInfo model)
        {
            return ElectricAppData.CheckValidUser(model); 

        }

        public ServiceResult<ForgotPasswordModel> CheckIsExistingUser(string Email)
        {
            return ElectricAppData.CheckIsExistingUser(Email); 
        }
        public ServiceResult<int> AddClientMaster(Client_MasterInfo model)
        {
            return ElectricAppData.AddClientMaster(model);
        }
        public List<Client_MasterInfolist> GetClientList()
        {
            return ElectricAppData.GetClientList();
        }

        public  ServiceResult<int> DeleteClient(int ClientID)
        {
            return ElectricAppData.DeleteClient(ClientID);
        }
        
        //public Client_MasterInfo GetClientDetail(int ClientID)
        //{
        //    return ElectricAppData.GetClientDetail(ClientID);
        //}

        public ServiceResult<EditClient_MasterInfo> GetClientDetail(int ClientID)
        {
            return ElectricAppData.GetClientDetail(ClientID);

        }

        public ServiceResult<int> SaveClientDetails(EditClient_MasterInfo model)
        {
            return ElectricAppData.SaveClientDetails(model);
        }
        //public int SaveClientDetails(Client_MasterInfo model)
        //{
        //    return ElectricAppData.SaveClientDetails(model);
        //}
//--------------------------------------------------------------------------------------
        //Account_Master 

        public List<Account_MasterInfoList> GetAllUserList()
        {
            return ElectricAppData.GetAllUserList();
        }
        public List<Account_MasterInfoList> GetMyUserList(int ClientID)
        {
            return ElectricAppData.GetMyUserList(ClientID);
        }

        public ServiceResult<int> AddNewUserBySP(Account_MasterInfo Model)
        {
            return ElectricAppData.AddNewUserBySP(Model);
        }

        public ServiceResult<EditAccount_MasterInfo> GetUserDetails(string UserID)
        {
            return ElectricAppData.GetUserDetails(UserID);
        }

        public ServiceResult<int> SaveUserDetails(EditAccount_MasterInfo Model)
        {
            return ElectricAppData.SaveUserDetails(Model);
        }
        public ServiceResult<int> DeleteUserInAM(string UserID)
        {
            return ElectricAppData.DeleteUserInAM(UserID);
        }

        public ServiceResult<EditAccount_MasterInfo> GetMyDetailsInAM(string UserID)
        {
            return ElectricAppData.GetMyDetailsInAM(UserID);
        }

        public ServiceResult<int> UpdateMyDetailsInAM(EditAccount_MasterInfo Model)
        {
            return ElectricAppData.UpdateMyDetailsInAM(Model);
        }
    }
}
