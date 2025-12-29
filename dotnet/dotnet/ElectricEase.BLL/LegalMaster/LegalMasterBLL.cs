using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Models;
using ElectricEase.Data.LabourMaster;
using ElectricEase.Data.LegalMaster;

namespace ElectricEase.BLL.LegalMaster
{
    public class LegalMasterBLL
    {
        private readonly LegalMasterData LegalData;
        public LegalMasterBLL()
        {
            LegalData = new LegalMasterData();
        }
        public ServiceResultList<Legal_DetailsInfoList> GetLegalList()
        {
            return LegalData.GetLegalList();
        }

        public ServiceResultList<Legal_DetailsInfoList> GetMyLegalList(int ClientID)
        {
            return LegalData.GetMyLegalList(ClientID);
        }

        //check is existing Legal Detail
        public ServiceResult<bool> CheckIsExistingLegal(string LegalDetail, int ClientID)
        {
            return LegalData.CheckIsExistingLegal(LegalDetail, ClientID);
        }
        //Here To Add New Legal details To Legal Details Table From Database
        public ServiceResult<int> AddLegalDetails(Legal_DetailsInfo Model)
        {
            return LegalData.AddLegalDetails(Model);
        }

        //here to Get Legal details For Edit and Update
        public ServiceResult<EditLegal_DetailsInfo> GetLegalDetails(int LegalID, int ClientID)
        {
            return LegalData.GetLegalDetails(LegalID, ClientID);
        }
        public ServiceResult<Legal_DetailsInfo> GetLegalDetail(int LegalID, int ClientID)
        {
            return LegalData.GetLegalDetail(LegalID, ClientID);
        }

        public string contactus(contactUs model)
        {
            return LegalData.contactus(model);
        }
       


        //Here To Update Legal Details
        public ServiceResult<int> UpdateLegalDetail(Legal_DetailsInfo Model)
        {
            return LegalData.UpdateLegalDetail(Model);
        }
        public ServiceResult<int> UpdateLegalDetails(EditLegal_DetailsInfo Model)
        {
            return LegalData.UpdateLegalDetails(Model);
        }

        //Here To Delete Legal Details\
        public ServiceResult<int> DeleteLegalDetails(int LegalID, int ClientID)
        {
            return LegalData.DeleteLegalDetails(LegalID, ClientID);

        }

        //Here Get Selected Legal List Details
        public ServiceResultList<Legal_DetailsInfoList> GetSelectedLegalListDetails(int ClientID, int LegalID)
        {
            return LegalData.GetSelectedLegalListDetails(ClientID, LegalID);
        }
        //public ServiceResult<int> DeleteLegalDetails(int LegalID, int ClientID)
        //{
        //    return LegalData.DeleteLegalDetails(LegalID, ClientID);
        //} 
        //public ServiceResult<EditLegal_DetailsInfo> GetMyLegalDetails(int LegalID, int p)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
