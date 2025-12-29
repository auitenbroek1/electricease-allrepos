using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.Models;
using ElectricEase.Helpers;
using System.IO;
using System.Web.Security;
using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.AccountMaster;
using ElectricEase.BLL.PartsMaster;
using ElectricEase.BLL.LabourMaster;
using ElectricEase.BLL.LegalMaster;
using ElectricEase.BLL.Master;
using System.Data.OleDb;
using ElectricEase.Data.DataBase;

namespace ElectricEase.Web.Controllers
{
    public class Legal_MasterController : Controller
    {
        //
        // GET: /Legal_Master/
        /// <summary>
        //Legal master index Page
        /// </summary>
        /// <returns></returns>
        /// 

        [Authorize(Roles = "Admin, SiteAdmin, LegalAdmin")]
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult LGMIndex()
        {
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            if (TempData["ADDSuccessMsg"] != null)
            {
                ViewBag.ADDSuccessMsg = TempData["ADDSuccessMsg"];
            }
            if (TempData["UpdateSuccessMsg"] != null)
            {
                ViewBag.UpdateSuccessMsg = TempData["UpdateSuccessMsg"];
            }
            if (TempData["DeleteSuccessMsg"] != null)
            {
                ViewBag.DeleteSuccessMsg = TempData["DeleteSuccessMsg"];
            }
            if (TempData["DeleteFailMsg"] != null)
            {
                ViewBag.DeleteFailMsg = TempData["DeleteFailMsg"];
            }
            if (TempData["ADDFailMsg"] != null)
            {
                ViewBag.ADDFailMsg = TempData["ADDFailMsg"];
            }

            return View();
        }

        //Legal list Index Page If No Legal Rights
        public ActionResult LegalListIndex()
        {

            //  ViewBag.LoginUser = Loginuser;
            LegalMasterBLL legalResponce = new LegalMasterBLL();
            ClientMasterBLL clientdata = new ClientMasterBLL();
            ServiceResultList<Legal_DetailsInfoList> response = new ServiceResultList<Legal_DetailsInfoList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            if (Loginuser == "Admin")
            {
                response = legalResponce.GetLegalList();
                return PartialView("_LegalList", response.ListData);
            }
            else
            {
                Account_MasterInfo modelobj = new Account_MasterInfo();
                modelobj = clientdata.GetClientName(Loginuser);
                response = legalResponce.GetMyLegalList(modelobj.Client_ID);
                return PartialView("_LegalList", response.ListData);
            }
            // return View();

        }
        public ActionResult CheckLegalDetailIsvalid(Legal_DetailsInfo Model)
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            int Client_ID = 0;

            if (Loginuser != "Admin")
            {
                Model.Client_ID = result.Client_ID;
            }

            response = LegalResponse.CheckIsExistingLegal(Model.Legal_Detail, Model.Client_ID);
            if (response.Data == true)
            {
                return Json("Already Exists");
            }
            else
            {
                return Json("New");
            }
        }

        public ActionResult GetMyLegalCatgory(string ClientID = "",string legalID="")
        {

            List<LegalCategoryList> legallist = new List<LegalCategoryList>();
            LegalCategoryList legal = new LegalCategoryList();
            ServiceResult<Legal_DetailsInfo> response = new ServiceResult<Legal_DetailsInfo>();
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            // PartsCategoryList parts2 = new PartsCategoryList();
            // parts2.Part_Category = "Select";

            if (ClientID != "" && ClientID != "0")
            {
                legallist = new MasterServiceBLL().GetMyLegalCategoryList(Convert.ToInt32(ClientID));
                             
                legallist = legallist.OrderBy(a => a.Legal_Category).ToList();
                legal.Legal_Category = "other";
                legallist.Insert(0, legal);

                ViewBag.Legal_Category = new SelectList(legallist, "Legal_Category", "Legal_Category");
                if(legalID!="0")
                {
                    response = LegalResponse.GetLegalDetail(Convert.ToInt32(legalID), Convert.ToInt32(ClientID));
                    if(response.Data!=null)
                    {
                        response.Data.LegalcatgoryList = legallist;
                        return Json(response.Data, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(legallist, JsonRequestBehavior.AllowGet);

                    }
                   
                }
                else
                {
                    return Json(legallist, JsonRequestBehavior.AllowGet);
                }
               
            }
            else
            {
                legallist = legallist.OrderBy(a => a.Legal_Category).ToList();
                legal.Legal_Category = "other";
                legallist.Insert(0, legal);
                return Json(legallist, JsonRequestBehavior.AllowGet);
            }
        }
        //Here To Add  New Legal Details
        public ActionResult AddLegalDetails(Legal_DetailsInfo Model)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            if (ModelState.IsValid)
            {

                // Here To Get Client ID By Login User 

                if (Loginuser == "Admin")
                {
                    Model.Created_By = Loginuser;

                }
                else
                {
                    var result = ClientResponse.GetClientName(Loginuser);
                    Model.Client_ID = result.Client_ID;
                    Model.Created_By = Loginuser;
                    Model.Updated_By = Loginuser;
                }

                response = LegalResponse.AddLegalDetails(Model);
                if (response.ResultCode > 0)
                {
                    TempData["ADDSuccessMsg"] = response.Message;
                    return Json(response.Message);
                }
                else
                {
                    TempData["ADDFailMsg"] = response.Message;
                    return Json(response.Message);
                }
            }
            else
            {
                string status = "";
                int i = 0;
                foreach (var item in ModelState.Values)
                {
                    if (item.Errors.Count > 0)
                    {
                        int j = 0;
                        foreach (var keys in ModelState.Keys)
                        {
                            if (j == i)
                            {
                                status += keys.ToString();
                                break;
                            }
                            j++;
                        }
                        status += " - " + item.Errors[0].ErrorMessage + ",";
                    }
                    i++;
                }
                return Json(response.Message);
            }
        }
        //Here To Get a Legal Details For Edit and Update 
        public ActionResult EditLegalDetails(string LegalID = "", string ClientID = "")
        {
            ServiceResult<Legal_DetailsInfo> response = new ServiceResult<Legal_DetailsInfo>();
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            var result = ClientResponse.GetClientName(Loginuser);
            List<LegalCategoryList> LegalList = new List<LegalCategoryList>();
            if (Loginuser == "Admin")
                LegalList = new MasterServiceBLL().GetLegalCategoryList();
            else
                LegalList = new MasterServiceBLL().GetMyLegalCategoryList(result.Client_ID);
            ViewBag.ClientID = new SelectList(new MasterServiceBLL().GetMyClientList(), "Client_ID", "Client_Company");
            LegalCategoryList Legal = new LegalCategoryList();
            LegalList = LegalList.OrderBy(a => a.Legal_Category).ToList();
            Legal.Legal_Category = "other";
            LegalList.Insert(0, Legal);
            ViewBag.LegalCategory = new SelectList(LegalList, "Legal_Category", "Legal_Category");
            if (LegalID.Trim() != "0" && ClientID.Trim() != "0")
            {
                response = LegalResponse.GetLegalDetail(Convert.ToInt32(LegalID), Convert.ToInt32(ClientID));
                if (response.ResultCode > 0)
                {

                    return PartialView("_AddLegalDetails1", response.Data);
                }
                else
                {
                    ViewBag.EditSavedErrorMsg = response.Message;
                    ViewBag.Edit = "EditLegalDetails";
                    return PartialView("_AddLegalDetails1", response.Data);
                }
            }
            else
            {
                Legal_DetailsInfo model = new Legal_DetailsInfo();
                return PartialView("_AddLegalDetails1", new Legal_DetailsInfo());
            }
        }

        //here To Update and Save Legal Details
        [HttpPost]
        public ActionResult UpdateLegalDetail(Legal_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            //save modified client details after model validation

            //To Get Login User Details
            if (Loginuser == "Admin")
            {
                Model.Updated_By = Loginuser;
            }
            else
            {
                var result = ClientResponse.GetClientName(Loginuser);
                Model.Client_ID = result.Client_ID;
                Model.Updated_By = Loginuser;
            }
            if (ModelState.IsValid)
            {

                response = LegalResponse.UpdateLegalDetail(Model);
                if (response.ResultCode > 0)
                {
                    TempData["UpdateSuccessMsg"] = response.Message;
                    return Json(response.Message);
                }
                else
                {
                    ViewBag.Edit = "EditLegalDetails";
                    ViewBag.UpdateErrordMsg = response.Message;
                    return Json(response.Message);
                }
            }
            else
            {
                ViewBag.Edit = "EditLegalDetails";
                return Json(response.Message);
            }

        }


        //Here To Delete Legal Details
        public ActionResult DeleteLegalDetails(string LegalID, string ClientID)
        {
            LegalMasterBLL LegalResponse = new LegalMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            if (LegalID.Trim() != "0" && ClientID.Trim() != "0")
            {
                string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                ViewBag.LoginUser = Loginuser;
                response = LegalResponse.DeleteLegalDetails(Convert.ToInt32(LegalID), Convert.ToInt32(ClientID));
                if (response.ResultCode > 0)
                {
                    TempData["DeleteSuccesMsg"] = response.Message;
                    return Json(response.Message);
                }
                else
                {
                    TempData["DeleteFailMessage"] = response.Message;
                    return Json(response.Message);
                }

            }
            else
            {

                return Json(response.Message);
            }
        }
       
    }
}