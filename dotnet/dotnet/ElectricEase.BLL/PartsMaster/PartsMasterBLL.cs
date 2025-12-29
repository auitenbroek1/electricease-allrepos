using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Models;
using ElectricEase.Data.PartsMaster;
using ElectricEase.Data.DataBase;
using System.Data;

namespace ElectricEase.BLL.PartsMaster
{

    public class PartsMasterBLL
    {
        private readonly PartsMasterData PartsData;
        public PartsMasterBLL()
        {
            PartsData = new PartsMasterData();
        }

        public ServiceResultList<Parts_DetailsInfoList> GetPartsDetalisList(string PartCatgory = "")
        {
            return PartsData.GetPartsDetalisList(PartCatgory);
        }
        public ServiceResultList<Parts_DetailsInfoList> GetDistributorPartsDetalisList(string PartCatgory = "")
        {
            return PartsData.GetDistributorPartsDetalisList(PartCatgory);
        }

        public ServiceResultList<Parts_DetailsInfoList> GetSelectedDistributorPartsDetalisList(string PartCatgory = "", int Did = 0)
        {
            return PartsData.GetSelectedDistributorPartsDetalisList(PartCatgory, Did);
        }
        public ServiceResultList<Parts_DetailsInfoList> GetNationalWidePartsDetalisList(string PartCatgory = "")
        {
            return PartsData.GetNationalWidePartsDetalisList(PartCatgory);
        }

        public List<Super_Admin_Parts_Details> SearchNationalWidePartsData(string searchBy, string extra_search)
        {
            return PartsData.SearchNationalWidePartsData(searchBy, extra_search);
        }

        public List<Super_Admin_Parts_Details> GetNationalWidePartsData(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, string extra_search)
        {
            return PartsData.GetNationalWidePartsData(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, extra_search);
        }

        public List<Parts_DetailsInfoList> GetDistributorPartsData(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, string extra_search)
        {
            return PartsData.GetDistributorPartsData(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, extra_search);
        }

        public List<Parts_DetailsInfoList> GetDistributorPartsByDistId(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, string extra_search, int DistributorId)
        {
            return PartsData.GetDistributorPartsByDistId(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, extra_search, DistributorId);
        }


        public List<Parts_DetailsInfoList> GetPartsDetalis(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, string extra_search, int? Client_ID)
        {
            return PartsData.GetPartsDetalis(searchBy, take, skip, sortBy, sortDir, out filteredResultsCount, out totalResultsCount, extra_search, Client_ID);
        }

        public ServiceResult<int> AddNewPartsDetail(Parts_DetailsInfo Model)
        {
            return PartsData.AddNewPartsDetail(Model);
        }
        public ServiceResult<int> AddNewDistributorPartsDetail(Parts_DetailsInfo Model)
        {
            return PartsData.AddNewDistributorPartsDetail(Model);
        }

        public ServiceResult<int> AddNationwidePartsDetail(Parts_DetailsInfo Model)
        {
            return PartsData.AddNationwidePartsDetail(Model);
        }
        public ServiceResultList<Parts_DetailsInfoList> GetMyPartsList(int ClientID, string PartCatgory = "")
        {
            return PartsData.GetMyPartsList(ClientID, PartCatgory);

        }

        public ServiceResult<Parts_DetailsInfo> GetMyPartsDetails(string PartNumber, int ClientID)
        {
            return PartsData.GetMyPartsDetails(PartNumber, ClientID);
        }
        public ServiceResult<Parts_DetailsInfo> GetMyDistbutorPartsDetails(string PartNumber, int D_Id)
        {
            return PartsData.GetMyDistbutorPartsDetails(PartNumber, D_Id);
        }

        public ServiceResult<Parts_DetailsInfo> GetNationawidePartsDetails(string PartNumber)
        {
            return PartsData.GetNationawidePartsDetails(PartNumber);
        }
        public ServiceResult<int> SavePartsDetails(EditParts_DetailsInfo Model)
        {
            return PartsData.SavePartsDetails(Model);
        }

        public ServiceResult<int> DeletePartsDetails(string PartNumber, int ClientID, string Username)
        {
            return PartsData.DeletePartsDetails(PartNumber, ClientID, Username);
        }

        public ServiceResult<int> DeleteDistributorPartsDetails(string PartNumber, int DistributorID, string Username)
        {
            return PartsData.DeleteDistributorPartsDetails(PartNumber, DistributorID, Username);
        }
        public ServiceResult<int> DeleteNotionaWidePartsDetails(string PartNumber)
        {
            return PartsData.DeleteNotionaWidePartsDetails(PartNumber);
        }
        public ServiceResultList<Parts_DetailsInfoList> GetJobPartsList(int clientId, string partsNumber)
        {
            return PartsData.GetJobPartsList(clientId, partsNumber);
        }

        public ServiceResultList<Labor_DetailsInfoList> GetJobLaborDetails(int clientId)
        {
            return PartsData.GetMyJobLaborDetailsList(clientId);
        }

        public string UploadParts(List<Parts_Details> lstparts)
        {
            return PartsData.UploadParts(lstparts);
        }
        public ServiceResult<bool> CheckIsNewPartNumber(int ClientID, string PartNumber)
        {
            return PartsData.CheckIsNewPartNumber(ClientID, PartNumber);
        }
        public ServiceResult<bool> CheckIsDistributorNewPartNumber(int Distributor, string PartNumber)
        {
            return PartsData.CheckIsDistributorNewPartNumber(Distributor, PartNumber);
        }
        public ServiceResult<bool> SuperAdminCheckIsNewPartNumber(string PartNumber)
        {
            return PartsData.SuperAdminCheckIsNewPartNumber(PartNumber);
        }

        public List<Parts_DetailsInfoList> GetPartsListByClientID(int ClientID)
        {
            return PartsData.GetPartsListByClientID(ClientID);
        }

        public List<DistributorPartsList> GetDistributorPartsByDistID(int distributorID)
        {
            return PartsData.GetDistributorPartsByDistID(distributorID);
        }

        public DataTable ImportParts(DataTable excelParts, int clientID)
        {
            return PartsData.ImportParts(excelParts, clientID);
        }

        public DataTable ImportNationWideParts(DataTable excelParts)
        {
            return PartsData.ImportNationWideParts(excelParts);
        }

        public DataTable ImportDistributorParts(DataTable excelParts, int distributorID)
        {
            return PartsData.ImportDistributorParts(excelParts, distributorID);
        }
    }
}

