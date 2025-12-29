using ElectricEase.Data.AssembliesMaster;
using ElectricEase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricEase.BLL.AssembliesMaster
{
    public class AssembliesMasterBLL
    {
        public readonly AssembliesMasterData AssembliesData;
        public AssembliesMasterBLL()
        {
            AssembliesData = new AssembliesMasterData();
        }

        public ServiceResultList<AssembliesParts_DetailsInfoList> GetMyAssembliesPartsList(int clientId, string partsNumber)
        {
            return AssembliesData.GetMyAssembliesPartsList(clientId, partsNumber);
        }
        public ServiceResultList<AssembliesParts_DetailsInfoList> GetMyNationWideAssembliesPartsList(string partsNumber)
        {
            return AssembliesData.GetMyNationWideAssembliesPartsList(partsNumber);
        }

        public List<AssembliesCategoryList> GetDistributorCategoryList()
        {
            return AssembliesData.GetDistributorCategoryList();
        }

        public ServiceResultList<AssembliesParts_DetailsInfoList> GetMyDistributorAssembliesPartsList(int Did, string partsNumber)
        {
            return AssembliesData.GetMyDistributorAssembliesPartsList(Did,partsNumber);
        }

        public ServiceResultList<AssembliesLabourDetailsList> GetMyAssembliesLaborDetailsList(int clientId)
        {
            return AssembliesData.GetMyAssembliesLaborDetailsList(clientId);
        }

        public ServiceResultList<AssembliesLabourDetailsList> GetMyAssembliesLaborerList(int clientId, string laborerName)
        {
            return AssembliesData.GetMyAssembliesLaborerList(clientId, laborerName);
        }

        public ServiceResult<int> AddNewAssembliesInformation(Assembly_MasterInfo model)
        {
            return AssembliesData.AddNewAssembliesInformation(model);
        }
        public ServiceResult<int> AddNewAssembliesPartsDeatails(Assembly_MasterInfo model)
        {
            return AssembliesData.AddNewAssembliesPartsDeatails(model);
        }
        public ServiceResultList<Assembly_MasterInfo> GetJobAssembliesDetailsList(int Client_ID, string Assemblies_Name)
        {
            return AssembliesData.GetJobAssembliesDetailsList(Client_ID, Assemblies_Name);
        }
        public ServiceResultList<Assembly_MasterInfo> GetJobAssembliesLaborDetailsList(int Client_ID, string Assemblies_Name)
        {
            return AssembliesData.GetJobAssembliesLaborDetailsList(Client_ID, Assemblies_Name);
        }
        public ServiceResult<int> AddNewAssembliesLabourDeatails(Assembly_MasterInfo model)
        {
            return AssembliesData.AddNewAssembliesLabourDeatails(model);
        }

        public ServiceResult<Assembly_MasterInfo> GetAssembliesListDetails(string AssembliesName, int Client_ID)
        {
            return AssembliesData.GetAssembliesListDetails(AssembliesName, Client_ID);
        }

        public ServiceResult<Assembly_MasterInfo> GetNationWideAssembliesListDetails(int AssebliesId)
        {
            return AssembliesData.GetNationWideAssembliesListDetails(AssebliesId);
        }

        public ServiceResult<Assembly_MasterInfo> GetDistributorAssembliesListDetails(string AssembliesName, int Did)
        {
            return AssembliesData.GetDistributorAssembliesListDetails(AssembliesName, Did);
        }

        public ServiceResultList<Assembly_MasterInfo> GetNewAssembliesListInJobs(int Client_ID)
        {
            return AssembliesData.GetNewAssembliesListInJobs(Client_ID);
        }

        public List<Assembly_MasterInfoList> GetAssembliesList(int Client_ID)
        {
            return AssembliesData.GetAssembliesList(Client_ID);
        }
        public List<Assembly_MasterInfoList> GetNationWideAssembliesList()
        {
            return AssembliesData.GetNationWideAssembliesList();
        }

        

        public List<Assembly_MasterInfoList> GetDistributorAssembliesList()
        {
            return AssembliesData.GetDistributorAssembliesList();
        }

        public List<Assembly_MasterInfoList> GetDistributorsAssembliesList(int distributorID)
        {
            return AssembliesData.GetDistributorsAssembliesList(distributorID);
        }
        public ServiceResult<int> UpdateAssembliesDetails(Assembly_MasterInfo model)
        {
            return AssembliesData.UpdateAssembliesDetails(model);
        }

        public ServiceResult<int> UpdateNationWideAssembliesDetails(Assembly_MasterInfo model)
        {
            return AssembliesData.UpdateNationWideAssembliesDetails(model);
        }
        public ServiceResult<int> UpdateDistribotorAssembliesDetails(Assembly_MasterInfo model)
        {
            return AssembliesData.UpdateDistribotorAssembliesDetails(model);
        }
        public ServiceResult<int> DeleteAssembliesDetails(string AssembliesName, int Client_ID, string username)
        {
            return AssembliesData.DeleteAssembliesDetails(AssembliesName, Client_ID, username);
        }
        public ServiceResult<int> DeleteNationaWideAssembliesDetails(string AssembliesName,int Asseblies_Id,string username)
        {
            return AssembliesData.DeleteNationaWideAssembliesDetails(AssembliesName, Asseblies_Id, username);
        }
        public ServiceResult<int> DeleteDistributorAssembliesDetails(string AssembliesName, int Did, string username)
        {
            return AssembliesData.DeleteDistributorAssembliesDetails(AssembliesName, Did, username);
        }
        public ServiceResult<int> AddNewAssembliesDetails(Assembly_MasterInfo model)
        {
            return AssembliesData.AddNewAssembliesDetails(model);
        }

        public ServiceResult<int> AddNewNationWideAssembliesDetails(Assembly_MasterInfo model)
        {
            return AssembliesData.AddNewNationWideAssembliesDetails(model);
        }
        public ServiceResult<int> AddNewDistributorAssembliesDetails(Assembly_MasterInfo model)
        {
            return AssembliesData.AddNewDistributorAssembliesDetails(model);
        }

        public ServiceResult<bool> AssemblieNameIsExist(string Assemblies_Name)
        {
            return AssembliesData.AssemblieNameIsExist(Assemblies_Name);
        }

        public ServiceResult<bool> NationwideAssemblieNameIsExist(int AssebliesId)
        {
            return AssembliesData.NationwideAssemblieNameIsExist(AssebliesId);
        }
        public ServiceResult<bool> DistributorAssemblieNameIsExist(string Assemblies_Name, int Did)
        {
            return AssembliesData.DistributorAssemblieNameIsExist(Assemblies_Name, Did);
        }
        //Get Previous Description List
        public ServiceResultList<Assembly_DescriptionList> GetDescriptionList(int ClientID)
        {

            return AssembliesData.GetDescriptionList(ClientID);
        }
        public ServiceResultList<Assembly_DescriptionList> GetNationDescriptionList()
        {

            return AssembliesData.GetNationDescriptionList();
        }
        public ServiceResultList<Assembly_DescriptionList> GetDistributorDescriptionList()
        {

            return AssembliesData.GetDistributorDescriptionList();
        }
        public ServiceResult<bool> AssemblieNameIsactive(string Assemblies_Name)
        {
            return AssembliesData.AssemblieNameIsactive(Assemblies_Name);
        }
        public string AssemblyExport(selectedAssembilesIds model)
        {
            return AssembliesData.AssemblyExport(model);
        }

        public string DistributorAssemblyExport(DistributorAssemblies model)
        {
            return AssembliesData.DistributorAssemblyExport(model);
        }

        public List<Assembly_MasterInfoList> GetDistributorAssembliesDetalis(string searchBy, int take, int skip, string sortBy, string sortDir, out int filteredResultsCount, out int totalResultsCount, string extra_search)
        {
            return AssembliesData.GetDistributorAssembliesDetalis(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, extra_search);
        }

        public List<Assembly_MasterInfoList> GetNationWideAssembliesGrid()
        {
            return AssembliesData.GetNationWideAssembliesGrid();
        }

        public List<Assembly_MasterInfoList> GetDistributorAssembliesGrid()
        {
            return AssembliesData.GetDistributorAssembliesGrid();
        }

        public string ValidateLaborUnit(selectedAssembilesIds model)
        {
            return AssembliesData.ValidateLaborUnit(model);
        }

        public string ValidateDistributorClientsLaborUnit(DistributorAssemblies model)
        {
            return AssembliesData.ValidateDistributorClientsLaborUnit(model);
        }
    }
}
