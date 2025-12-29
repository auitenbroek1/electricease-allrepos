using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.BLL;
using ElectricEase.Models;
using ElectricEase.Helpers;
using System.Web.Security;
using ElectricEase.BLL.ClientMaster;
using ElectricEase.Data;
using ElectricEase.BLL.LegalMaster;
using System.Configuration;

namespace ElectricEase.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        LogsMaster logs = new LogsMaster();
        public ActionResult WelcomeIndex()
        {
            return View("Index");
        }
        public ActionResult ContactUs(contactUs model)
        {
            LegalMasterBLL response = new LegalMasterBLL();
            var result = response.contactus(model);
            if (result == "success")
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            return Json("Fail", JsonRequestBehavior.AllowGet);

        }
        public ActionResult Login(string returnurl = "")
        {
            if (Session["UserName"] != null)
            {
                return RedirectToAction("Index", "App");
            }

            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            ViewBag.ReturnUrl = returnurl;
            LoginInfo model = new LoginInfo();
            model.ReturnUrl = returnurl;
            return View(model);
        }
        public ActionResult SessionTimout(string returnurl = "")
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginInfo model)
        {
            Logger.Info("Login");


            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string username = model.UserName;
            string pasword = model.Password;
            string adminusername = ConfigurationManager.AppSettings["username"];
            string adminpwd = ConfigurationManager.AppSettings["pwd"];
            if (ModelState.IsValid)
            {
                if (Session["UserName"] == null)
                {
                    if (username == adminusername && pasword == adminpwd)
                    {
                        Session["UserName"] = username;
                        FormsAuthentication.SetAuthCookie(username, false);
                        TimeSpan ts = new TimeSpan(24, 0, 0);
                        FormsAuthentication.Timeout.Add(ts);
                        
                        Request.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(1);
                        if (model.ReturnUrl == null || model.ReturnUrl == "")
                            return RedirectToAction("Index", "App");
                        else
                            return RedirectToAction(".." + model.ReturnUrl);
                    }
                    else
                    {
                        bool uservalid = ClientResponse.CheckValidUser(model);
                        Account_MasterInfo modelobj = new Account_MasterInfo();
                        modelobj = ClientResponse.GetClientName(username);
                        if (modelobj != null)
                        {
                            if (modelobj.Client_Company != null)
                            {
                                Session["CompanyName"] = modelobj.Client_Company;
                                Session["DCompanyLogo"] = modelobj.DistLogo;
                                Session["DCompanyUrl"] = modelobj.distCompurl;
                            }
                        }

                        if (uservalid)
                        {
                            Session["UserName"] = username;
                            Session["UserRole"] = "Client";
                            FormsAuthentication.SetAuthCookie(username, false);
                                                       
                            Request.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(1);
                            if (model.ReturnUrl == null || model.ReturnUrl == "")
                                if (modelobj.isfirst == true)
                                {
                                    return RedirectToAction("MyProfileDetail", "Account_Master");
                                }
                                else
                                {
                                    return RedirectToAction("Index", "App");
                                }

                            else
                                return RedirectToAction(".." + model.ReturnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError("", "We do not recognize your “Username” and / or “Password”. ");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Another User is already logged in, kindly close the current session or use another browser.");
                }
            }
            return View(model);
        }

        //To logout from  the Application
        [OutputCache(NoStore = true, Duration = 0, Location = System.Web.UI.OutputCacheLocation.Any)]
        public ActionResult Logout()
        {
            //Delete the Session details
            Session.Abandon();
            //Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();

            //Avoid back button press after logout....
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Expires = -1;
            Response.Cache.SetNoStore();

            //clear Authentication Cookie
            HttpCookie Cookie = new HttpCookie(".ASPXAUTH", "");
            Cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(Cookie);
            return RedirectToAction("Login");


        }

        public ActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<ForgotPasswordModel> response = new ServiceResult<ForgotPasswordModel>();
            response = ClientResponse.CheckIsExistingUser(Email);
            ViewBag.fpErrorMsg = "";
            if (response.Data != null)
            {
                ServiceResult<EditClient_MasterInfo> ClientDetailResponse = new ServiceResult<EditClient_MasterInfo>();
                // Account_MasterInfo modelobj = new Account_MasterInfo();
                //string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                // modelobj = ClientResponse.GetClientName(username);
                ClientDetailResponse = ClientResponse.GetClientDetail(response.Data.Client_ID);
                string senderMail = ClientDetailResponse.Data.Sender_EmailAddress;
                string SenderMailPassword = ClientDetailResponse.Data.Sender_EmailPassword;
                string DomainName = ClientDetailResponse.Data.DomainName;
                string smtpHost = ClientDetailResponse.Data.SMTP_Host;
                int? smtpPort = ClientDetailResponse.Data.SMTP_Port;
                //string result = Utility.SendEmailForgotPassword(response.Data.EMAIL, response.Data.NAME, senderMail, SenderMailPassword, DomainName, smtpHost, smtpPort);

                //Settings.SenderEmailAddress = senderMail;
                //Settings.SenderEmailPassword = SenderMailPassword;
                //Settings.DomainName = DomainName;
                //Settings.CompanyName = ClientDetailResponse.Data.Client_Company;
                Settings.SmtpHost = smtpHost;
                Settings.SmtpPort = smtpPort;
                string result = Utility.SendEmailForgotPassword(response.Data.EMAIL, response.Data.NAME, senderMail, SenderMailPassword, DomainName, smtpHost, smtpPort);


                if (result != null)
                {
                    ViewBag.Message = result;
                    ViewBag.Name = response.Data.NAME;
                    return PartialView("_ForgotPasswordSent");

                }
                else
                {
                    ViewBag.fpErrorMsg = response.Message;
                    return PartialView("_ForgotPasswordSent");
                }

            }
            else
            {
                ViewBag.fpErrorMsg = "NotExist";
                return Json("NotExist");
            }


        }

        public ActionResult ChangePassword(string Email, string Expire)
        {
            string dcryptMail = ElectricHelper.Decrypt(Email);
            string dcyptdate = ElectricHelper.Decrypt(Expire);
            LoginInfo model = new LoginInfo();
            model.TempEmail = dcryptMail;

            if (Convert.ToDateTime(dcyptdate) >= DateTime.Now)
            {
                ViewBag.ValidKey = " Link is “Valid”.";
                return View("ChagePassword", model);
            }
            else
            {
                ViewBag.InValidKey = " Link is “Expired”!";
                return View("ChagePassword");
            }

        }

        public ActionResult SaveNewPassword(string newpassword, string confirmpassword, string Email)
        {
            //string dcryptMail = ElectricHelper.Decrypt(Email);
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            response = ClientResponse.UpdatePassword(newpassword, confirmpassword, Email);
            if (response.ResultCode > 0)
            {
                ViewBag.SuccessMsg = response.Message;
                return Json("Success");
            }
            else
            {
                ViewBag.FailMsg = response.Message;
                return Json("Fail");
            }

        }



    }
}
