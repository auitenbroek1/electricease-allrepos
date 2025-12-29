using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.BLL;
using ElectricEase.Models;
using ElectricEase.Helpers;
using System.IO;
using System.Web.Security;
using ElectricEase.BLL.AccountMaster;
using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.Master;
using System.Web.Script.Serialization;
using System.Drawing;

namespace ElectricEase.Web.Controllers
{

    public class Account_MasterController : Controller
    {
        //
        // GET: /Account_Master/

        [Authorize(Roles = "Admin, SiteAdmin, AccountAdmin")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserDetails(string ClientID = "", string UserID = "")
        {
            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ServiceResult<Account_MasterInfo> response = new ServiceResult<Account_MasterInfo>();
            ViewBag.Clientid = new SelectList(new MasterServiceBLL().GetMyClientList(), "Client_ID", "Client_Company");
            if (ClientID != "0" && UserID != "0")
            {
                response = AccountResponse.GetUserDetail(UserID, Convert.ToInt32(ClientID));
                if (response.ResultCode > 0)
                {

                    return PartialView("_AddNewUserDetails", response.Data);
                }
                else
                {
                    return PartialView("_AddNewUserDetails");
                }

            }
            else
            {
                return PartialView("_AddNewUserDetails", new Account_MasterInfo());
            }
        }

        public ActionResult GetUserListDetails()
        {
            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ClientMasterBLL ClientData = new ClientMasterBLL();
            ServiceResultList<Account_MasterInfoList> response = new ServiceResultList<Account_MasterInfoList>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            if (Loginuser == "Admin")
            {
                response = AccountResponse.GetUsersList();
                return PartialView("_UsersList", response.ListData);
            }
            else
            {
                Account_MasterInfo ModelObj = new Account_MasterInfo();
                ModelObj = ClientData.GetClientName(Loginuser);
                response = AccountResponse.GetMyUserList(ModelObj.Client_ID);
                return PartialView("_UsersList", response.ListData);
            }
        }

        public ActionResult CheckUserIDisvalid(string UserId)
        {
            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<bool> response = new ServiceResult<bool>();
            response = AccountResponse.CheckIsExistUser(UserId);
            if (response.ResultCode == 1)
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Fasle", JsonRequestBehavior.AllowGet);
            }



        }
        public ActionResult SaveUserDetails()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            string model = Request.Form["Model"];
            Account_MasterInfo Model = jss.Deserialize<Account_MasterInfo>(model);
            var uploadfile = Request.Files;
            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            if (ModelState.IsValid)
            {
                if (uploadfile.Count > 0)
                {
                    HttpPostedFileBase file = uploadfile[0];
                    // string img = System.IO.Path.GetFileName(files.FileName);
                    using (MemoryStream ms = new MemoryStream())
                    {

                        file.InputStream.CopyTo(ms);
                        Model.Photo = ms.ToArray();
                    }
                }
                string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                if (Loginuser != "Admin")
                {
                    var result = ClientResponse.GetClientName(Loginuser);
                    Model.Client_ID = result.Client_ID;
                    Model.Created_By = Loginuser;
                    Model.Updated_By = Loginuser;
                }
                else
                {
                    Model.Created_By = Loginuser;
                    Model.Updated_By = Loginuser;
                }

                response = AccountResponse.AddNewUserBySP(Model);
                if (response.ResultCode > 0)
                {
                    TempData["AddSuccessMsg"] = response.Message;
                    return Json(response.Message);
                }
                else
                {
                    TempData["AddFailMsg"] = response.Message;
                    return Json(response.Message);
                }


            }
            else
            {
                return Json("Error");
            }

        }

        public ActionResult SendEmail(Account_MasterInfo model)
        {
            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<bool> response = new ServiceResult<bool>();
            response = AccountResponse.CheckIsExistUser(model.User_ID);
            if(response.Data==true)
            {
                EmailLog _email = new EmailLog();
                _email.BodyContent = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/EmailTempLates/Registration.html"));
                _email.BodyContent = _email.BodyContent.Replace("##FirstName##", model.First_Name);
                _email.BodyContent = _email.BodyContent.Replace("##LastName##", model.Last_Name);
                _email.BodyContent = _email.BodyContent.Replace("##Email##", model.User_ID);
                _email.BodyContent = _email.BodyContent.Replace("##Password##", model.Password);
                _email.EmailTo = model.E_Mail;
                _email.Subject = "Electric-ease login credentials";
                EmailService _emailService = new EmailService();
                string Status = _emailService.SendEmail(_email);
                return Json(Status, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("User doesn’t exist. please save and send mail", JsonRequestBehavior.AllowGet);
            }
        }
        //To Delete a user in AM
        public ActionResult DeleteUserDetails(string ClientID = "", string UserID = "")
        {

            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            response = AccountResponse.DeleteUserInAM(UserID, Convert.ToInt32(ClientID));
            if (response.ResultCode > 0)
            {

                return Json(response.Message, JsonRequestBehavior.AllowGet);

            }
            else
            {
                TempData["DeleteFailMsg"] = response.Message;
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult AMIndex()
        {
            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            //ServiceResult<Account_MasterInfoList> response = new ServiceResult<Account_MasterInfoList>();
            Account_MasterInfo ModelOBJ = new Account_MasterInfo();
            if (Loginuser == "Admin")
            {
                ModelOBJ.AMUsersList = AccountResponse.GetAllUserList();
            }
            else
            {
                var result = ClientResponse.GetClientName(Loginuser);
                ModelOBJ.AMUsersList = AccountResponse.GetMyClientUserList(result.Client_ID);
            }

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
            return View(ModelOBJ);
        }

        //to Get Profile Detail if no AcccountMaster Rights 
        public ActionResult MyProfileDetail()
            {
            ServiceResult<EditAccount_MasterInfo> response = new ServiceResult<EditAccount_MasterInfo>();
            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            ViewBag.State = new SelectList(Utility.GetStates(), "Value", "Name");
            response = AccountResponse.GetMyDetailsInAM(Loginuser);
             if (TempData["MyUpdateSuccessMsg"] != null)
            {
                ViewBag.MyUpdateSuccessMsg = TempData["MyUpdateSuccessMsg"];
            }

            return View(response.Data);
        }

        //Here to update MyProfile detail
        [HttpPost]
        public ActionResult MyProfileDetails()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer(); 
            string model = Request.Form["Model"];
            EditAccount_MasterInfo Model = jss.Deserialize<EditAccount_MasterInfo>(model);
            var uploadfile = Request.Files; 
            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            //To Get Login User Details
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
           
            EditAccount_MasterInfo ModelObj2 = new EditAccount_MasterInfo();
            var result = ClientResponse.GetClientName(Loginuser);
            Model.Client_ID = result.Client_ID;
            Model.Updated_By = result.User_ID; 
            //save modified client details after model validation
            if (ModelState.IsValid)
            {
                if (uploadfile.Count > 0)
                {
                    //string img = System.IO.Path.GetFileName(Model.UploadedFile.FileName);
                    //string path = System.IO.Path.Combine(Server.MapPath("~/AppImges/Account_Master"), img);
                    //Model.UploadedFile.SaveAs(path);
                    HttpPostedFileBase file = uploadfile[0];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        Model.Photo = ms.ToArray();
                    }
                }
                //else if(Model.Imagesrc ==""|| Model.Imagesrc == null)
                //{
                //    string path = System.IO.Path.Combine(Server.MapPath("/Images/default_logo.png"));
                //    Model.Photo = System.IO.File.ReadAllBytes(path);
                   
                //}
                response = AccountResponse.UpdateMyDetailsInAM(Model);
                if (response.ResultCode > 0)
                {
                    //TempData["MyUpdateSuccessMsg"] = response.Message;
                    ////ViewBag.UpdateSuccessMsg = response.Message;
                    //return RedirectToAction("MyProfileDetail");
                    return Json(response.Message);
                }
                else
                {
                    //ViewBag.MyUpdateErorMsg = response.Message;
                    ////ViewBag.UpdateErorMsg = response.Message;
                    //return View(Model);
                     return Json(response.Message);
                }
            }
            else
            {

                return View(Model);
            }
            
        }

        //Here to Add New User Details
        public ActionResult AddNewUser()
        {

            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            //ServiceResult<Account_MasterInfoList> response = new ServiceResult<Account_MasterInfoList>();
            Account_MasterInfo ModelOBJ = new Account_MasterInfo();
            if (Loginuser == "Admin")
            {
                ModelOBJ.AMUsersList = AccountResponse.GetAllUserList();
            }
            else
            {
                var result = ClientResponse.GetClientName(Loginuser);
                ModelOBJ.AMUsersList = AccountResponse.GetMyClientUserList(result.Client_ID);
            }

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
            return View(ModelOBJ);
        }
        //Here to Add New User Details
        [HttpPost]
        public ActionResult AddNewUser(Account_MasterInfo ModelOBJ, string Gender)
        {
            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<int> ResultCount = new ServiceResult<int>();
            // Here To Get Client ID By Login User 
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            if (Loginuser == "Admin")
            {
                ModelOBJ.AMUsersList = AccountResponse.GetAllUserList();
            }
            else
            {
                var result = ClientResponse.GetClientName(Loginuser);
                ModelOBJ.AMUsersList = AccountResponse.GetMyClientUserList(result.Client_ID);
            }
            if (Loginuser == "Admin")
            {
                ModelOBJ.Created_By = "Admin";
            }
            else
            {
                Account_MasterInfo ModelObj2 = new Account_MasterInfo();
                ModelObj2 = ClientResponse.GetClientName(Loginuser);
                ModelOBJ.Client_ID = ModelObj2.Client_ID;
                ModelOBJ.Created_By = ModelObj2.User_ID;
            }

            if (ModelState.IsValid)
            {
                //here upload file convert into binary[]
                if (ModelOBJ.UploadedFile != null)
                {
                    string img = System.IO.Path.GetFileName(ModelOBJ.UploadedFile.FileName);
                    //string path = System.IO.Path.Combine(Server.MapPath("~/AppImges/Account_Master"), img);
                    //ModelObj1.UploadedFile.SaveAs(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ModelOBJ.UploadedFile.InputStream.CopyTo(ms);
                        ModelOBJ.Photo = ms.ToArray();
                    }
                }
                ResultCount = AccountResponse.AddNewUserBySP(ModelOBJ);
                if (ResultCount.ResultCode > 0)
                {

                    TempData["ADDSuccessMsg"] = ResultCount.Message;
                    return RedirectToAction("AMIndex");
                }
                else
                {
                    TempData["ADDFailMsg"] = ResultCount.Message;
                    //return View("AMIndex", ModelObj1);
                    return RedirectToAction("AMIndex");
                }

            }
            else
            {
                if (ModelOBJ.UploadedFile != null)
                {
                    string img = System.IO.Path.GetFileName(ModelOBJ.UploadedFile.FileName);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ModelOBJ.UploadedFile.InputStream.CopyTo(ms);
                        ModelOBJ.Photo = ms.ToArray();
                        ViewBag.file = Convert.ToBase64String(ModelOBJ.Photo);
                        ViewBag.upfile = ModelOBJ.UploadedFile;
                        ModelOBJ.UploadedFile = ViewBag.tempfile;
                    }
                }
                return View("AMIndex", ModelOBJ);
                //return View("_AddNewUser", ModelObj1);
            }


        }

        //To Get Details for Edit
        public ActionResult EditUserDetails(string UserID, int ClientID)
        {
            ServiceResult<EditAccount_MasterInfo> response = new ServiceResult<EditAccount_MasterInfo>();
            EditAccount_MasterInfo model = new EditAccount_MasterInfo();
            ServiceResultList<Account_MasterInfoList> ListObJ = new ServiceResultList<Account_MasterInfoList>();

            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;

            //Get UserDetails To Edit
            response = AccountResponse.GetUserDetails(UserID, ClientID);
            if (Loginuser == "Admin")
            {
                var result = AccountResponse.GetAllUserList();
                model.AMUsersList = AccountResponse.GetAllUserList();
                // response.Data.AMUsersList = model.AMUsersList;
                foreach (Account_MasterInfoList item in model.AMUsersList)
                {
                    response.Data.AMUsersList.Add(new Account_MasterInfoList
                    {
                        Client_Company = item.Client_Company,
                        Client_ID = item.Client_ID,
                        E_Mail = item.E_Mail,
                        First_Name = item.First_Name,
                        Last_Name = item.Last_Name,
                        Phone = item.Phone,
                        User_ID = item.User_ID,
                        UserColor = item.UserColor
                    });
                    //model.AMUsersList.Add(item);
                }
            }
            else
            {
                var loginfo = ClientResponse.GetClientName(Loginuser);
                //var result = AccountResponse.GetMyClientUserList(loginfo.Client_ID);
                model.AMUsersList = AccountResponse.GetMyClientUserList(loginfo.Client_ID);
                // response.Data.AMUsersList = model.AMUsersList;
                foreach (Account_MasterInfoList item in model.AMUsersList)
                {
                    response.Data.AMUsersList.Add(new Account_MasterInfoList
                    {
                        Client_Company = item.Client_Company,
                        Client_ID = item.Client_ID,
                        E_Mail = item.E_Mail,
                        First_Name = item.First_Name,
                        Last_Name = item.Last_Name,
                        Phone = item.Phone,
                        User_ID = item.User_ID,
                        UserColor = item.UserColor
                    });
                    //model.AMUsersList.Add(item);
                }
            }
            if (response.ResultCode > 0)
            {
                ViewBag.Edit = "EditUserDetails";

                return View("_EditUserDetails", response.Data);
            }
            else
            {
                ViewBag.EditSavedErrorMsg = response.Message;
                ViewBag.Edit = "EditUserDetails";
                return View("_EditUserDetails", response.Data);
            }

        }

        //to save edited information
        [HttpPost]
        public ActionResult EditUserDetails(EditAccount_MasterInfo ModelObj1)
        {
            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<EditAccount_MasterInfo> ListOBJ = new ServiceResult<EditAccount_MasterInfo>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

            ServiceResult<int> response = new ServiceResult<int>();
            if (Loginuser == "Admin")
            {
                var result = AccountResponse.GetAllUserList();
                ModelObj1.AMUsersList = AccountResponse.GetAllUserList();
            }
            else
            {
                var result = ClientResponse.GetClientName(Loginuser);
                ModelObj1.AMUsersList = AccountResponse.GetMyClientUserList(result.Client_ID);
            }
            //To Get Login User Details
            ViewBag.LoginUser = Loginuser;
            if (Loginuser == "Admin")
            {
                ModelObj1.Updated_By = "Admin";
            }
            else
            {
                EditAccount_MasterInfo ModelObj2 = new EditAccount_MasterInfo();
                var result = ClientResponse.GetClientName(Loginuser);
                ModelObj1.Client_ID = result.Client_ID;
                ModelObj1.Updated_By = result.User_ID;
            }

            //save modified client details after model validation
            if (ModelState.IsValid)
            {
                if (ModelObj1.UploadedFile != null)
                {
                    string img = System.IO.Path.GetFileName(ModelObj1.UploadedFile.FileName);
                    //string path = System.IO.Path.Combine(Server.MapPath("~/AppImges/Account_Master"), img);
                    //ModelObj1.UploadedFile.SaveAs(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ModelObj1.UploadedFile.InputStream.CopyTo(ms);
                        ModelObj1.Photo = ms.ToArray();
                    }
                }
                response = AccountResponse.SaveUserDetails(ModelObj1);
                if (response.ResultCode > 0)
                {
                    TempData["UpdateSuccessMsg"] = response.Message;
                    return RedirectToAction("AMIndex");
                }
                else
                {
                    ViewBag.Edit = "EditUserDetails";
                    ViewBag.UpdateErrordMsg = response.Message;
                    return View("_EditUserDetails", ModelObj1);
                }
            }
            else
            {
                ViewBag.Edit = "EditUserDetails";
                return View("_EditUserDetails", ModelObj1);
            }

        }

        //To Delete a user in AM
        public ActionResult DeleteUser(string UserID, int ClientID)
        {

            AccountMasterBLL AccountResponse = new AccountMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            response = AccountResponse.DeleteUserInAM(UserID, ClientID);
            if (response.ResultCode > 0)
            {
                // ViewBag.DeleteSuccessMsg = response.Message;
                TempData["DeleteSuccessMsg"] = response.Message;
                return RedirectToAction("AMIndex");

            }
            else
            {
                TempData["DeleteFailMsg"] = response.Message;
                return RedirectToAction("AMIndex");
            }


        }

    }
}
