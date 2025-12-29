using ElectricEase.Data.DataBase;
using ElectricEase.Data.TimeSheetMaster;
using ElectricEase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricEase.BLL.TimeSheetMaster
{
    public class TimeSheetMasterBLL
    {
        private readonly TimeSheetMasterData TimeSheetData;
        public TimeSheetMasterBLL()
        {
            TimeSheetData = new TimeSheetMasterData();
        }

        public ServiceResult<TimeSheetDTO> GetTimeSheetMasterList(TimeSheetDTO modal,int from)
        {
            return TimeSheetData.GetTimeSheetMasterList(modal,from);
        }

        public ServiceResult<TimeSheetDTO> TimeSheetDetails(long? timeSheetID,string Loginuser,bool isView)
        {
            return TimeSheetData.TimeSheetDetails(timeSheetID, Loginuser,isView);
        }

        public ServiceResult<string> SaveTimeSheet(TimeSheetDTO modal)
        {
            return TimeSheetData.SaveTimeSheet(modal);
        }

        public ServiceResult<TimeSheetDTO> ChangeDates(TimeSheetDTO modal, string loginuser)
        {
            return TimeSheetData.ChangeDates(modal, loginuser);
        }

        public ServiceResult<string> ApproveOrRejectTimeSheet(TimeSheetDTO modal, string loginuser)
        {
            return TimeSheetData.ApproveOrRejectTimeSheet(modal);
        }

        public ServiceResult<TimeSheetDTO> DeleteTimeSheet(TimeSheetDTO modal)
        {
            return TimeSheetData.DeleteTimeSheet(modal);
        }
    }
}
