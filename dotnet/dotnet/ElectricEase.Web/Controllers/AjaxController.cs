using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.BLL;
using ElectricEase.Models;
using System.IO;
using System.Web.Security;
using System.Web.Configuration;
using ElectricEase.BLL.Master;
using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.PartsMaster;
using ElectricEase.BLL.AccountMaster;
using ElectricEase.BLL.LabourMaster;
using ElectricEase.BLL.LegalMaster;
using ElectricEase.BLL.JobMaster;

namespace ElectricEase.Web.Controllers
{
    public class AjaxController : Controller
    {
        //
        // GET: /Ajax/

        [HttpGet]
        public JsonResult Index()
        {
            return Json("{\"success\":\"1\"}", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetAllClientList()
        {
            MasterServiceBLL MasterResponse = new MasterServiceBLL();
            return Json(MasterResponse.GetMyClientList(), JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetPartsCategoryList()
        {

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            MasterServiceBLL MasterResponse = new MasterServiceBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            if (Loginuser == "Admin")
            {
                return Json(MasterResponse.GetPartsCategoryList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = ClientResponse.GetClientName(Loginuser);
                return Json(MasterResponse.GetMyPartsCategoryList(result.Client_ID), JsonRequestBehavior.AllowGet);
            }


        }
        [HttpGet]
        public ActionResult GetLabourCategoryList()
        {
            MasterServiceBLL MasterResponse = new MasterServiceBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();

            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            if (Loginuser == "Admin")
            {
                return Json(MasterResponse.GetLabourCategoryList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = ClientResponse.GetClientName(Loginuser);
                return Json(MasterResponse.GetMyLabourCategoryList(result.Client_ID), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult GetLegalCategoryList()
        {
            MasterServiceBLL MasterResponse = new MasterServiceBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();

            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            if (Loginuser == "Admin")
            {
                return Json(MasterResponse.GetLegalCategoryList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = ClientResponse.GetClientName(Loginuser);
                return Json(MasterResponse.GetMyLegalCategoryList(result.Client_ID), JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult GetMyUserListinAM()
        {
            // ElectricBLL response = new ElectricBLL();
            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResultList<Account_MasterInfoList> response = new ServiceResultList<Account_MasterInfoList>();
            Account_MasterInfo ModelObj = new Account_MasterInfo();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

            if (Loginuser == "Admin")
            {
                response = AccountResponse.GetUsersList();
                var jsonResult = Json(response.ListData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }
            else
            {

                ModelObj = ClientResponse.GetClientName(Loginuser);
                //To Get Users List Here
                response = AccountResponse.GetMyUserList(ModelObj.Client_ID);
                var jsonResult = Json(response.ListData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
                //return Json(AccountResponse.GetMyUserList(ModelObj.Client_ID), JsonRequestBehavior.AllowGet);
            }


        }


        public JsonResult GetMyClienListinCM()
        {
            //ElectricBLL response = new ElectricBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResultList<Client_MasterInfolist> response = new ServiceResultList<Client_MasterInfolist>();

            response = ClientResponse.GetClientList();
            var jsonResult = Json(response.ListData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult GetMyPartsList()
        {
            PartsMasterBLL PartsResponse = new PartsMasterBLL();
            ClientMasterBLL ClientData = new ClientMasterBLL();
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            if (Loginuser == "Admin")
            {
                response = PartsResponse.GetPartsDetalisList();

                var jsonResult = Json(response.ListData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                Account_MasterInfo ModelObj = new Account_MasterInfo();
                ModelObj = ClientData.GetClientName(Loginuser);

                response = PartsResponse.GetMyPartsList(ModelObj.Client_ID);
                var jsonResult = Json(response.ListData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;

            }

        }


        //public JsonResult GetMyLabourList()
        //{
        //    LabourMasterBLL LabourResponse = new LabourMasterBLL();
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    ServiceResultList<Labor_DetailsInfoList> response = new ServiceResultList<Labor_DetailsInfoList>();
        //    string LoginUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    if (LoginUser == "Admin")
        //    {
        //        response = LabourResponse.GetLabourList();
        //        var jsonResult = Json(response.ListData, JsonRequestBehavior.AllowGet);
        //        jsonResult.MaxJsonLength = int.MaxValue;
        //        return jsonResult;
        //    }
        //    else
        //    {
        //        Account_MasterInfo ModelObj = new Account_MasterInfo();
        //        ModelObj = ClientResponse.GetClientName(LoginUser);

        //        response = LabourResponse.GetMyLabourList(ModelObj.Client_ID);
        //        var jsonResult = Json(response.ListData, JsonRequestBehavior.AllowGet);
        //        jsonResult.MaxJsonLength = int.MaxValue;
        //        return jsonResult;
        //    }
        //}

        public JsonResult GetMyJobAssembliesList()
        {
            JobMasterBLL jobResponse = new JobMasterBLL();
            ServiceResultList<Job_MasterInfo> response = new ServiceResultList<Job_MasterInfo>();
            string LoginUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            Job_MasterInfo objAssembliesNameList = new Job_MasterInfo();
            response = jobResponse.GetAssembliesListInfo();
            var jsonResult = Json(response.ListData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult GetMyLegalList()
        {
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResultList<Legal_DetailsInfoList> response = new ServiceResultList<Legal_DetailsInfoList>();
            string LoginUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            if (LoginUser == "Admin")
            {
                response = LegalResponse.GetLegalList();
                var jsonResult = Json(response.ListData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            else
            {
                Account_MasterInfo ModelObj = new Account_MasterInfo();
                ModelObj = ClientResponse.GetClientName(LoginUser);
                response = LegalResponse.GetMyLegalList(ModelObj.Client_ID);
                var jsonResult = Json(response.ListData, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
        }
        [HttpGet]
        public JsonResult GetMyAssemliesNameList()
        {

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            MasterServiceBLL MasterResponse = new MasterServiceBLL();

            string LoginUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

            var result = ClientResponse.GetClientName(LoginUser);
            var resultdata = MasterResponse.GetMyAssembliesNameCategoryList(result.Client_ID);
            return Json(resultdata.ToList(), JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetAssemliesCategoryList()
        {

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            MasterServiceBLL MasterResponse = new MasterServiceBLL();

            string LoginUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

            var result = ClientResponse.GetClientName(LoginUser);
            var resultdata = MasterResponse.GetAssembliesCategoryList(result.Client_ID);
            return Json(resultdata.ToList(), JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetNationWideAssemliesCategoryList()
        {

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            MasterServiceBLL MasterResponse = new MasterServiceBLL();

            string LoginUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

            var result = ClientResponse.GetClientName(LoginUser);
            var resultdata = MasterResponse.GetNationWideAssembliesCategoryList();
            return Json(resultdata.ToList(), JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetDistributorAssemliesCategoryList(int distributorId = 0)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            MasterServiceBLL MasterResponse = new MasterServiceBLL();
            AssembliesCategoryList parts = new AssembliesCategoryList();
            string LoginUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(LoginUser);
            var resultdata = MasterResponse.GetDistributorAssembliesCategoryList(distributorId);
            resultdata = resultdata.OrderBy(a => a.Assemblies_Category).ToList();
            parts.Assemblies_Category = "other";           
            resultdata.Insert(0, parts);
            return Json(resultdata.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
