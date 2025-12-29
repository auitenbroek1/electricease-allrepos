using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Models;
using ElectricEase.Data.LabourMaster;
using ElectricEase.Data.DataBase;

namespace ElectricEase.BLL.LabourMaster
{
    public class LabourMasterBLL
    {
        private readonly LabourMasterData LabourData;
        public LabourMasterBLL()
        {
            LabourData = new LabourMasterData();

        }

        public List<Labor_DetailsInfoList> GetLabourList()
        {
            return LabourData.GetLabourList().OrderBy(x=>x.Labour_ID).ToList();
        }

        public ServiceResultList<Labor_DetailsInfoList> GetMyLabourList(int ClientID)
        {
            return LabourData.GetMyLabourList(ClientID);
        }

        public List<Labor_DetailsInfoList> GetLabourListbyId(int ClientID)
        {
            return LabourData.GetMyLabourListbyId(ClientID);
        }
        public ServiceResult<int> AddLabourDetails(Labor_DetailsInfo Model)
        {
            return LabourData.AddLabourDetails(Model);

        }

        //check is existing Labor
        public ServiceResult<bool> CheckIsExistingLabor(string Laborname, int ClientID)
        {
            return LabourData.CheckIsExistingLabor(Laborname, ClientID);
        }
        public ServiceResult<int> AddLabourDetail(Labor_DetailsInfo Model)
        {
            return LabourData.AddLabourDetail(Model);

        }

        //Here To Get Labour Details To Edit Labour Details
        public ServiceResult<EditLabor_DetailsInfo> GetLabourDetails(string LabourName, int ClientID)
        {
            return LabourData.GetLabourDetails(LabourName, ClientID);
        }
        public ServiceResult<Labor_DetailsInfo> GetLabourDetail(string LabourName, int ClientID)
        {
            return LabourData.GetLabourDetail(LabourName, ClientID);
        }
        public EditLabor_DetailsInfo GetLabourDetailsbyname(string LabourName, int ClientID)
        {
            return LabourData.GetLabourDetailsbyname(LabourName, ClientID);
        }

        //Here To Update Labour details 
        public ServiceResult<int> UpdateLabourDetails(EditLabor_DetailsInfo Model)
        {
            return LabourData.UpdateLabourDetails(Model);
        }

        //Here To Delete labour details
        public ServiceResult<int> DeleteLabourDetails(string LabourName, int ClientID)
        {
            return LabourData.DeleteLabourDetails(LabourName, ClientID);
        }

        public ServiceResultList<Labor_DetailsInfoList> GetMyJobLaborerList(int clientId, string laborerName)
        {
            return LabourData.GetMyJobLaborerList(clientId, laborerName);
        }
    }
}
