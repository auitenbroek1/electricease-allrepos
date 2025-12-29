using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.BLL.PartsMaster;
using ElectricEase.Models;
using ElectricEase.Helpers;
using System.IO;
using System.Web.Security;
using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.LabourMaster;
using System.Web.Script.Serialization;
using ElectricEase.Data.DataBase;
using ElectricEase.BLL.Master;

namespace ElectricEase.Web.Controllers
{
    public class Labour_MasterController : Controller
    {
        //
        // GET: /Labour_Master/
        //Labour Master index Page

        [Authorize(Roles = "Admin, SiteAdmin, LaborAdmin")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetMyLaborCatgory(string ClientID = "",string laborname="")
        {

            List<LabourCategoryList> laborlist = new List<LabourCategoryList>();
            LabourCategoryList labor = new LabourCategoryList();
            ServiceResult<Labor_DetailsInfo> response = new ServiceResult<Labor_DetailsInfo>();
            LabourMasterBLL LabourResponse = new LabourMasterBLL();
            // PartsCategoryList parts2 = new PartsCategoryList();
            // parts2.Part_Category = "Select";

            if (ClientID != "" && ClientID != "0")
            {
                laborlist = new MasterServiceBLL().GetMyLabourCategoryList(Convert.ToInt32(ClientID));
                //partslist.Add(parts2);               
                laborlist = laborlist.OrderBy(a => a.Labor_Category).ToList();
                labor.Labor_Category = "other";
                laborlist.Insert(0, labor);

                ViewBag.Labor_Category = new SelectList(laborlist, "Labor_Category", "Labor_Category");
                if(laborname!="")
                {
                    response = LabourResponse.GetLabourDetail(laborname, Convert.ToInt32(ClientID));
                    if(response.Data!=null)
                    {
                        response.Data.laborcatgorylist = laborlist;
                        return Json(response.Data, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(laborlist, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {

                    return Json(laborlist, JsonRequestBehavior.AllowGet);
                } 
            }
            else
            {
                laborlist = laborlist.OrderBy(a => a.Labor_Category).ToList();
                labor.Labor_Category = "other";
                laborlist.Insert(0, labor);
                return Json(laborlist, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EditLabourDetails(string LabourName, string ClientID)
        {
            ServiceResult<Labor_DetailsInfo> response = new ServiceResult<Labor_DetailsInfo>();
            LabourMasterBLL LabourResponse = new LabourMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            ViewBag.Clientid = new SelectList(new MasterServiceBLL().GetMyClientList(), "Client_ID", "Client_Company");
            List<LabourCategoryList> laborlist = new List<LabourCategoryList>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            var result = ClientResponse.GetClientName(Loginuser);
            if (Loginuser == "Admin")
            {
                laborlist = new MasterServiceBLL().GetLabourCategoryList(); ;
            }
            else
            {
                laborlist = new MasterServiceBLL().GetMyLabourCategoryList(result.Client_ID);
            }
            LabourCategoryList labor = new LabourCategoryList();
            laborlist = laborlist.OrderBy(a => a.Labor_Category).ToList();
            labor.Labor_Category = "other";
            laborlist.Insert(0, labor);
            ViewBag.LaborCategory = new SelectList(laborlist, "Labor_Category", "Labor_Category");
            // here to Get LabourDetails To Edit
            if (ClientID != "0" && LabourName != "0")
            {
                response = LabourResponse.GetLabourDetail(LabourName, Convert.ToInt32(ClientID));

                if (response.ResultCode > 0)
                {
                    return View("_AddLabourDetails", response.Data);
                }
                else
                {
                    return View("_AddLabourDetails");
                }
            }
            else
            {
                Labor_DetailsInfo model = new Labor_DetailsInfo();
                model.Gender = true;
                return View("_AddLabourDetails", model);
            }

        }

        public ActionResult CheckLaborNameIsvalid(string ClientID = "", string LaborName = "")
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            LabourMasterBLL LabourResponse = new LabourMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            int Client_ID = 0;

            if (Loginuser != "Admin")
            {
                Client_ID = result.Client_ID;
            }
            else
            {
                Client_ID = Convert.ToInt32(ClientID);
            }
            response = LabourResponse.CheckIsExistingLabor(LaborName.Trim(), Client_ID);
            if (response.Data == true)
            {
                return Json("Already Exists", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("New", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveLaborDetails()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            string model = Request.Form["Model"];
            Labor_DetailsInfo Model = jss.Deserialize<Labor_DetailsInfo>(model);

            var uploadfile = Request.Files;
            LabourMasterBLL LabourResponse = new LabourMasterBLL();
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
                        if(file.ContentType != "application/pdf")
                        {
                            file.InputStream.CopyTo(ms);
                            Model.Photo = ms.ToArray();
                        }
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

                response = LabourResponse.AddLabourDetail(Model);
                if (response.ResultCode > 0)
                {
                    // TempData["AddSuccessMsg"] = response.Message;
                    return Json(response.Message);
                }
                else
                {
                    // TempData["AddFailMsg"] = response.Message;
                    return Json(response.Message);
                }


            }
            else
            {
                return Json("Error");
            }

        }

        public ActionResult DeleteLaborDetail(string LaborName = "", string ClientID = "")
        {
            LabourMasterBLL laborResponse = new LabourMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();

            if (LaborName.Trim() != "0" && ClientID.Trim() != "0")
            {
                string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                ViewBag.LoginUser = Loginuser;
                response = laborResponse.DeleteLabourDetails(LaborName.Trim(), Convert.ToInt32(ClientID));

                return Json(response.Message, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult LMIndex()
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


        //here to Show LabourList Only If no Labour Master Rights
        public ActionResult LabourListIndex()
        {
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            return View();
        }



        //Here To Add Labour Details in LabourMaster
        public ActionResult AddLabourDetails(Labor_DetailsInfo Model)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            LabourMasterBLL LabourResponse = new LabourMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;

            if (Model.Labor_Category == null)
            {
                Model.Labor_Category = Model.otherLabor_Category;
            }
            //if (Model.Burden==0)
            //{
            //    Model.Burden =1;
            //}
            if (ModelState.IsValid)
            {

                if (Model.UploadedFile != null)
                {
                    if (Model.otherLabor_Category != null)
                    {
                        Model.Labor_Category = Model.otherLabor_Category;
                    }
                    string img = System.IO.Path.GetFileName(Model.UploadedFile.FileName);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Model.UploadedFile.InputStream.CopyTo(ms);
                        Model.Photo = ms.ToArray();
                    }
                }
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
                }

                response = LabourResponse.AddLabourDetails(Model);
                if (response.ResultCode > 0)
                {
                    TempData["ADDSuccessMsg"] = response.Message;
                    return RedirectToAction("LMIndex");
                }
                else
                {
                    TempData["ADDFailMsg"] = response.Message;
                    return RedirectToAction("LMIndex", Model);
                }
            }
            else
            {
                return View("LMIndex", Model);
            }


        }

        //Here to Get Labou Details For  Edit


        //Here to update Labour Information Details
        [HttpPost]
        public ActionResult UpdateLabourDetails(EditLabor_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            LabourMasterBLL LabourResponse = new LabourMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;

            //save modified client details after model validation
            if (ModelState.IsValid)
            {
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
                if (Model.UploadedFile != null)
                {

                    string img = System.IO.Path.GetFileName(Model.UploadedFile.FileName);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Model.UploadedFile.InputStream.CopyTo(ms);
                        Model.Photo = ms.ToArray();
                    }
                }
                response = LabourResponse.UpdateLabourDetails(Model);
                if (response.ResultCode > 0)
                {
                    TempData["UpdateSuccessMsg"] = response.Message;
                    return RedirectToAction("LMIndex");
                }
                else
                {
                    ViewBag.Edit = "EditLabourDetails";
                    ViewBag.UpdateErrordMsg = response.Message;
                    return View("_EditLabourDetails", Model);
                }
            }
            else
            {
                ViewBag.Edit = "EditLabourDetails";
                return View("_EditLabourDetails", Model);
            }

        }

        //Here To Delete  Labour Information 
        public ActionResult DeleteLabourDetails(string LabourName, int ClientID)
        {
            LabourMasterBLL LabourResponse = new LabourMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            ViewBag.LoginUser = Loginuser;
            response = LabourResponse.DeleteLabourDetails(LabourName, ClientID);
            if (response.ResultCode > 0)
            {
                // ViewBag.DeleteSuccessMsg = response.Message;
                TempData["DeleteSuccessMsg"] = response.Message;
                return RedirectToAction("LMIndex");
            }
            else
            {
                TempData["DeleteFailMsg"] = response.Message;
                return RedirectToAction("LMIndex");
            }
        }

        public ActionResult LabourList()
        {
            LabourMasterBLL LabourResponse = new LabourMasterBLL();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            List<Labor_DetailsInfoList> response = new List<Labor_DetailsInfoList>();
            string LoginUser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            if (LoginUser == "Admin")
            {
                response = LabourResponse.GetLabourList();
                return PartialView("_LabourList", response);
            }
            else
            {
                Account_MasterInfo ModelObj = new Account_MasterInfo();
                ModelObj = ClientResponse.GetClientName(LoginUser);
                response = LabourResponse.GetLabourListbyId(ModelObj.Client_ID);
                return PartialView("_LabourList", response);
            }
        }

    }
}
