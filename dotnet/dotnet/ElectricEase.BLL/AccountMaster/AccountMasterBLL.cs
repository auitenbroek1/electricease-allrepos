using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Models;
using ElectricEase.Data.AccountMaster;
using System.Collections.ObjectModel;

namespace ElectricEase.BLL.AccountMaster
{
    public class AccountMasterBLL
    {
        private readonly AccountMasterData AccountData;
        public AccountMasterBLL()
        {
            AccountData = new AccountMasterData();
        }
        //public ServiceResultList<Account_MasterInfoList> GetAllUserList()
        //{
        //    return AccountData.GetAllUserList();
        //}
        public List<Account_MasterInfoList> GetAllUserList()
        {
            return AccountData.GetAllUserList();
        }
        //public ObservableCollection<Account_MasterInfoList> GetAllUserList()
        //{
        //    return AccountData.GetAllUserList();
        //}

        public ServiceResultList<Account_MasterInfoList> GetUsersList()
        {
            return AccountData.GetUsersList();
        }
        public List<Account_MasterInfoList> GetMyClientUserList(int ClientID)
        {
            return AccountData.GetMyClientUserList(ClientID);
        }
        public ServiceResultList<Account_MasterInfoList> GetMyUserList(int ClientID)
        {
            return AccountData.GetMyUserList(ClientID);
        }

        public ServiceResult<int> AddNewUserBySP(Account_MasterInfo Model)
        {
            return AccountData.AddNewUserBySP(Model);
        }
        public ServiceResult<bool> CheckIsExistUser(string UserID)
        {
            return AccountData.CheckIsExistUser(UserID);
        }
        public ServiceResult<EditAccount_MasterInfo> GetUserDetails(string UserID, int ClientID)
        {
            return AccountData.GetUserDetails(UserID, ClientID);
        }
        public ServiceResult<Account_MasterInfo> GetUserDetail(string UserID, int ClientID)
        {
            return AccountData.GetUserDetail(UserID, ClientID);
        }
        public ServiceResult<int> SaveUserDetails(EditAccount_MasterInfo Model)
        {
            return AccountData.SaveUserDetails(Model);
        }
        public ServiceResult<int> DeleteUserInAM(string UserID, int ClientID)
        {
            return AccountData.DeleteUserInAM(UserID, ClientID);
        }

        public ServiceResult<EditAccount_MasterInfo> GetMyDetailsInAM(string UserID)
        {
            return AccountData.GetMyDetailsInAM(UserID);
        }

        public ServiceResult<int> UpdateMyDetailsInAM(EditAccount_MasterInfo Model)
        {
            return AccountData.UpdateMyDetailsInAM(Model);
        }
    }
}
