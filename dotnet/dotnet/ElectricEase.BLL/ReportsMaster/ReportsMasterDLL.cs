using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Data.ReportsMaster;
using ElectricEase.Models;

namespace ElectricEase.BLL.ReportsMaster
{
    
    public class ReportsMasterDLL
    {
        private readonly ReportsMasterData ReportsData;
        public ReportsMasterDLL()
        {
            ReportsData = new ReportsMasterData();
        }

        public ServiceResultList<Job_MasterInfo> GetMyDescriptionReport(int Client_ID)
        {
            return ReportsData.GetDescriptionReportsList(Client_ID);
        }
    }
}
