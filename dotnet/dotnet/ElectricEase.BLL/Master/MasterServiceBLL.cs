using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Models;
using ElectricEase.Data;

namespace ElectricEase.BLL.Master
{
   public  class MasterServiceBLL
    {
       private readonly MasterServiceData MasterData;
       public MasterServiceBLL()
       {
           MasterData = new MasterServiceData();
       }
        public List<ClientList> GetMyClientList()
        {
            List<ClientList> clientlist = new List<ClientList>();
            clientlist = MasterData.GetMyClientList();
            return clientlist;
        }


        public List<PartsCategoryList> GetPartsCategoryList()
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            PartsList = MasterData.GetPartsCategoryList();
            return PartsList;
        }

        public List<PartsCategoryList> GetDistributorPartsCategoryList()
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            PartsList = MasterData.GetDistributorPartsCategoryList();
            return PartsList;
        }

        public List<PartsCategoryList> GetNationalwidePartsCategoryList()
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            PartsList = MasterData.GetNationalwidePartsCategoryList();
            return PartsList;
        }

        

        public List<PartsCategoryList> GetMyPartsCategoryList(int ClientID)
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            PartsList = MasterData.GetMyPartsCategoryList(ClientID);
            return PartsList;
        }
        public List<PartsCategoryList> GetMyDistributorPartsCategoryList(int Did)
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            PartsList = MasterData.GetMyDistributorPartsCategoryList(Did);
            return PartsList;
        }
        public List<PartsCategoryList> GetMyDistributorPartsCategoryList()
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            PartsList = MasterData.GetMyDistributorPartsCategoryList();
            return PartsList;
        }
        public List<PartsCategoryList> GetNationwidepartscategory()
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            PartsList = MasterData.GetNationwidepartscategory();
            return PartsList;
        }
        public List<LabourCategoryList> GetLabourCategoryList()
        {
            List<LabourCategoryList> LabourList = new List<LabourCategoryList>();
            return LabourList = MasterData.GetLabourCategoryList();

        }
        public List<LabourCategoryList> GetMyLabourCategoryList(int ClientID)
        {
            List<LabourCategoryList> LabourList = new List<LabourCategoryList>();
             LabourList = MasterData.GetMyLabourCategoryList(ClientID).OrderBy(x=>x.Labor_Category).ToList();
            return LabourList;
        }
        public List<LegalCategoryList> GetLegalCategoryList()
        {
            List<LegalCategoryList> LegalList = new List<LegalCategoryList>();
            return LegalList = MasterData.GetLegalCategoryList();

        }
        public List<LegalCategoryList>GetMyLegalCategoryList(int ClientID)
        {
            List<LegalCategoryList> LegalList = new List<LegalCategoryList>();
            return LegalList = MasterData.GetMyLegalCategoryList(ClientID);

        }
        public List<AssembliesNameList> GetMyAssembliesNameCategoryList(int ClientID)
        {
            List<AssembliesNameList> AssembliesNameListOBJ = new List<AssembliesNameList>();
            return AssembliesNameListOBJ = MasterData.GetMyAssembliesNameCategoryList(ClientID);

        }

        public List<AssembliesCategoryList> GetAssembliesCategoryList(int ClientID)
        {
            List<AssembliesCategoryList> AsCatgoryList = new List<AssembliesCategoryList>();
            AsCatgoryList = MasterData.GetAssembliesCategoryList(ClientID);
            return AsCatgoryList;
        }
        public List<AssembliesCategoryList> GetNationWideAssembliesCategoryList()
        {
            List<AssembliesCategoryList> AsCatgoryList = new List<AssembliesCategoryList>();
            AsCatgoryList = MasterData.GetNationWideAssembliesCategoryList();
            return AsCatgoryList;
        }
        public List<AssembliesCategoryList> GetDistributorAssembliesCategoryList(int distributorId)
        {
            List<AssembliesCategoryList> AsCatgoryList = new List<AssembliesCategoryList>();
            AsCatgoryList = MasterData.GetDistributorAssembliesCategoryList(distributorId);
            return AsCatgoryList;
        }
    }
}
