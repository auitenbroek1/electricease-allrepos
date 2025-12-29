using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Data;
using ElectricEase.Models;
using ElectricEase.Data.DataBase;
using System.Data.SqlClient;

namespace ElectricEase.Data.ReportsMaster
{
    public class ReportsMasterData
    {
        public ServiceResultList<Job_MasterInfo> GetDescriptionReportsList(int Client_ID)
        {
            ServiceResultList<Job_MasterInfo> response = new ServiceResultList<Job_MasterInfo>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Job_MasterInfo> result = new List<Job_MasterInfo>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "ClientID", Value = Client_ID };

                    result = db.Database.SqlQuery<Job_MasterInfo>("exec JobReportDetails @ClientID",para).ToList();

                    if (result != null)
                    {
                        response.ListData = result;
                        response.ResultCode = 1;
                        response.Message = "Success";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "No “Records” found!";
                        return response;

                    }
                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
