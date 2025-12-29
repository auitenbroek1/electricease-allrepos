using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Models;
using ElectricEase.Data.JobMaster;
using ElectricEase.Data.DataBase;

namespace ElectricEase.BLL.JobMaster
{
    public class JobMasterBLL
    {
        private readonly JobMasterData jobMasterData;
        public JobMasterBLL()
        {
            jobMasterData = new JobMasterData();
        }

        public ServiceResultList<Job_MasterInfo> GetAssembliesListInfo()
        {
            return jobMasterData.GetAssembliesDetailsList();
        }

        //public ServiceResult<Job_MasterInfo> SaveJobDetails(Job_MasterInfo Model)
        //{
        //    return jobMasterData.SaveJobDetails(Model);
        //}

        public ServiceResult<int> AddJobInformation(Job_MasterInfo Model)
        {
            return jobMasterData.AddJobInformation(Model);
        }


        public ServiceResult<int> AddPartsListDetailsInJob(Job_MasterInfo Model)
        {
            return jobMasterData.AddPartsListDetailsInJob(Model);
        }

        public ServiceResult<int> AddLabouListDetailsInJob(Job_MasterInfo Model)
        {
            return jobMasterData.AddLabouListDetailsInJob(Model);
        }
        public ServiceResult<int> AddAssembliesPartsListInJob(Job_MasterInfo Model)
        {
            return jobMasterData.AddAssembliesPartsListInJob(Model);
        }

        public ServiceResult<int> AddAssemblieslabourListInJob(Job_MasterInfo Model)
        {
            return jobMasterData.AddAssembliesPartsListInJob(Model);
        }
        public ServiceResult<int> AddJobAssembliesPartsList(Job_MasterInfo Model)
        {
            return jobMasterData.AddJobAssembliesPartsList(Model);
        }
        //public ServiceResult<int> AddJobAssembliesLaborList(Job_MasterInfo Model)
        //{
        //    return jobMasterData.AddJobAssembliesLaborList(Model);
        //}
        public ServiceResult<int> AddJobAttachMents(Job_MasterInfo Model)
        {
            return jobMasterData.AddJobAttachMents(Model);

        }
        public ServiceResult<int> AddLegalDetailsInJob(Job_MasterInfo Model)
        {
            return jobMasterData.AddLegalDetailsInJob(Model);

        }


        public ServiceResultList<Job_MasterInfoList> GetJobDetailList(int ClientID)
        {
            return jobMasterData.GetJobDetailList(ClientID); 
        }

        public ServiceResult<Job_MasterInfo> GetJobDetails(int Job_ID, int ClientID)
        {
            return jobMasterData.GetJobDetails(Job_ID, ClientID);
        }
        //public ServiceResult<int> SaveJobPartsDetails(Job_MasterInfo Model)
        //{
        //    return jobMasterData.SaveJobPartsDetails(Model);
        //}

        public ServiceResult<bool> JobIDIsExist(int JobID)
        {
            return jobMasterData.JobIDIsExist(JobID);
        }
        public ServiceResult<JobCalendarinfo> AddJobCalenderInfo(JobCalendarinfo Model)
        {
            return jobMasterData.AddJobCalenderInfo(Model);
        }
        public ServiceResultList<JobCalendarinfo> GetJobCalenderEventsList(int ClientID,int JobID)
        {
            return jobMasterData.GetJobCalenderEventsList(ClientID, JobID);
        }
        //Delete JobEvent in Job Calender
        public ServiceResult<int> DeleteJobCalenderEvent(int ClientID,int EventID,int JobID)
        {
            return jobMasterData.DeleteJobCalenderEvent(ClientID,EventID, JobID);

        }
        public string deleteJobParts(int JobId, string PartId,int ClientID)
        {
            return jobMasterData.deleteJobParts(JobId, PartId, ClientID);
        }
        public string deleteJobLabour(int JobId, string LabourName, int ClientID)
        {
            return jobMasterData.deleteJobLabour(JobId, LabourName, ClientID);
        }
        public string SaveJobDetails(Job_MasterInfo Model)
        {
            return jobMasterData.SaveJobDetails(Model);
        }
        //public ServiceResult<int> UpdateJobDetails(Job_MasterInfo Model)
        //{
        //    return jobMasterData.UpdateJobDetails(Model);
        //}

            public string UpdateJobDetails(int JobID,int ClientId)
        {
            return jobMasterData.UpdateJobDetails(JobID, ClientId);
        }

        public ServiceResult<Job_MasterInfo> GetJobsAssemblies(int Job_ID, int ClientID)
        {
            return jobMasterData.GetJobsAssemblies(Job_ID, ClientID);
        }
        public List<Labor_DetailsInfoList> GetJobsLabour(int Job_ID, int ClientID)
        {
            return jobMasterData.GetJobsLabour(Job_ID, ClientID);
        }
        public List<Parts_DetailsInfoList> GetJobsParts(int Job_ID, int ClientID)
        {
            return jobMasterData.GetJobsParts(Job_ID, ClientID);
        }
        public List<AssembliesParts_DetailsInfoList> GetJobsAssembliesParts(int Job_ID, string AssembliesName, int Client_ID, int AssemblyJobId)
        {
            return jobMasterData.GetJobsAssembliesParts(Job_ID, AssembliesName, Client_ID, AssemblyJobId);
        }
        public Assembly_MasterInfo getjobassemblydetails(int Job_ID, string AssembliesName, int Client_ID, int AssemblyID)
        {
            return jobMasterData.getjobassemblydetais(Job_ID, AssembliesName, Client_ID,AssemblyID);
        }
        public string deleteJobassembly(int Job_ID, int Id, int Client_ID, string username)
        {
            return jobMasterData.deleteJobAssembly(Job_ID, Id, Client_ID, username);
        }
        public List<Legal_DetailsInfoList> GetJobsLegalList(int Jobid, int Client_ID)
        {
            return jobMasterData.getjobLegallist(Jobid, Client_ID);
        }
        public List<clients> getallJobclients(int ClientID)
        {
            return jobMasterData.getallJobClients(ClientID);
        }
        public clients getjobclientdetails(string name)
        {
            return jobMasterData.getjobclientdetails(name);
        }
        public List<clients> getallJobclientsLocation(int ClientID)
        {
            return jobMasterData.getallJobClientsLocation(ClientID);
        }
        public clients getjobclientLocationdetails(string name)
        {
            return jobMasterData.getjobclientlocationdetails(name);
        }
        public string saveattachments(List<Job_Attachments> jobattach)
        {
            return jobMasterData.savejobattachment(jobattach);
        }
        public List<Job_Attachments> getattchments(int Jobid)
        {
            return jobMasterData.getattchments(Jobid);
        }
        public string deleteJobattachment(int attachmentid, int ClientId, string username)
        {
            return jobMasterData.deleteJobattachment(attachmentid,ClientId,username);
        }
        public ServiceResultList<SOWInfoList> GetPreviousSOWList(int ClientID)
        {

            return jobMasterData.GetPreviousSOWList(ClientID);
        }
        public ServiceResultList<SOWInfoList> GetPreviousJobDesc(int ClientID)
        {

            return jobMasterData.GetPreviousJobDesc(ClientID);
        }
        public ServiceResultList<SOWInfoList> GetPreviousDirection(int ClientID)
        {

            return jobMasterData.GetPreviousDirection(ClientID);
        }
        public ServiceResult<int> DeleteJobDetails(int ClientId, int JobId, string username)
        {
            return jobMasterData.DeleteJobDetails(ClientId, JobId, username);
        }

        public ServiceResultList<CalendarEventItem> GetCalenderEvents(int ClientID,string userid,bool CreatedBy_SuperUser)
        {
            return jobMasterData.GetCalenderEvents(ClientID, userid, CreatedBy_SuperUser);

        }

 
        public Sow_Master SaveSowMaster(string sow, string Category, string Subject,int ClientID)
        {
            return jobMasterData.SaveSowMaster(sow, ClientID, Category, Subject);
        }
        public List<Sow_Master> GetSowMasterList(int ClientID)
        {
            return jobMasterData.GetSowMasterList(ClientID);
        }

        public string RemoveSow(int id, int ClientId)
        {
            return jobMasterData.RemoveSow(id, ClientId);
        }

        public Sow_Master SowMasterDetails(int id,int clientId)
        {
            return jobMasterData.SowMasterDetail(id, clientId);
        }
        public List<MyUserList> GetMyUserList(int ClientID,string comment="")
        {
            return jobMasterData.GetMyUserList(ClientID,comment);
        }
        public ServiceResult<string> GetMycolor(int ClientID,string Assigneduser)
        {
            return jobMasterData.GetMycolor(ClientID, Assigneduser);
        }
        public List<Job_DJE_VQ_Detailsinfo> JobDjeDeltiles(int Jobid, int Client_ID)
        {
            return jobMasterData.JobDjeDeltiles(Jobid, Client_ID);
        }
        public string DeleteDjeRow(int StatusID)
        {
            return jobMasterData.DeleteDjeRow(StatusID);
        }
        public List<exportAssemblyAndParts> ExportPartsInJob(int JobId)
        {
            return jobMasterData.ExportPartsInJob(JobId);
        }
    }
}
