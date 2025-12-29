using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Models;
using ElectricEase.Data.ClientMaster;

namespace ElectricEase.BLL.ClientMaster
{
    public class ClientMasterBLL
    {
        private readonly ClientMasterData ClientData;

        public ClientMasterBLL()
        {
            ClientData = new ClientMasterData();

        }


        //To Get ClienName for Login Based Rights
        public Account_MasterInfo GetClientName(string username)
        {
            return ClientData.GetClientName(username);
        }

        //To  check whether login user is valid or not
        public bool CheckValidUser(LoginInfo model)
        {
            return ClientData.CheckValidUser(model);

        }

        public ServiceResult<ForgotPasswordModel> CheckIsExistingUser(string Email)
        {
            return ClientData.CheckIsExistingUser(Email);
        }
        public ServiceResult<bool> CheckIsExisCompany(string ClientCompany, string UserID)
        {
            return ClientData.CheckIsExisCompany(ClientCompany, UserID);
        }

        public string checkisexistcomapnyanduser(string ClientCompany, string UserID)
        {
            return ClientData.checkisexistcomapnyanduser(ClientCompany, UserID);
        }
        public ServiceResult<int> AddClientMaster(Client_MasterInfo model)
        {
            return ClientData.AddClientMaster(model);
        }

        public string RegisterNewuser(Client_MasterInfo model)
        {
            return ClientData.RegisterNewuser(model);
        }

        public ServiceResultList<Client_MasterInfolist> GetClientList()
        {
            return ClientData.GetClientList();
        }

        public ServiceResult<int> DeleteClient(int ClientID)
        {
            return ClientData.DeleteClient(ClientID);
        }

        //public Client_MasterInfo GetClientDetail(int ClientID)
        //{
        //    return ElectricAppData.GetClientDetail(ClientID);
        //}

        public ServiceResult<EditClient_MasterInfo> GetClientDetail(int ClientID)
        {
            return ClientData.GetClientDetail(ClientID);

        }
        public ServiceResult<Client_MasterInfo> GetClientDetails(int ClientID)
        {
            return ClientData.GetClientDetails(ClientID);

        }

        public ServiceResult<int> SaveClientDetails(EditClient_MasterInfo model)
        {
            return ClientData.SaveClientDetails(model);
        }
        //public int SaveClientDetails(Client_MasterInfo model)
        //{
        //    return ElectricAppData.SaveClientDetails(model);
        //}
        //here to  update  Login new password
        public ServiceResult<int> UpdatePassword(string newpassword, string confirmpassword, string Email)
        {
            return ClientData.UpdatePassword(newpassword, confirmpassword, Email);
        }

        //Here to get Nimber of pars,Job open ,close count  Details
        public ServiceResult<CountDetails> GetCountDetails(int ClientID)
        {
            return ClientData.GetCountDetails(ClientID);
        }

        public ServiceResult<int> SaveDistributor(Distributor model)
        {
            return ClientData.SaveDistributor(model);
        }

        public ServiceResultList<Distributor> GetDistributorlist()
        {
            return ClientData.GetDistributorlist();
        }
        public ServiceResult<Distributor>EditDistributor(int id)
        {
            return ClientData.EditDistributor(id);
        }
        public ServiceResultList<distributordropdown> distributorList()
        {
            return ClientData.distributorList();
        }
        public ServiceResultList<Standaredclient> StandaredUsers()
        {
            return ClientData.StandaredUsers();
        }
        public ServiceResultList<Standaredclient> DistributorClients(int Did)
        {
            return ClientData.DistributorClients(Did);
        }

        public ServiceResult<bool> IsDistributorExiste(Distributor model)
        {
            return ClientData.IsDistributorExiste(model);
        }
    }
}
