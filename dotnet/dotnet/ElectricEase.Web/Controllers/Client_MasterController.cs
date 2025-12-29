using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.BLL;
using ElectricEase.Models;
using ElectricEase.Helpers;
using System.IO;
using ElectricEase.BLL.ClientMaster;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Data;

namespace ElectricEase.Web.Controllers
{
    //[AllowAnonymous]
    public class Client_MasterController : Controller
    {
        //
        // GET: /Client_Master/

        [Authorize(Roles = "Admin")]
        public ActionResult CMindex()
        {
            return View();
        }
        public ActionResult GetClientDetails(string ClientID = "")
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<Client_MasterInfo> response = new ServiceResult<Client_MasterInfo>();
            ServiceResultList<distributordropdown> distributorlist = new ServiceResultList<distributordropdown>();
            distributorlist = ClientResponse.distributorList();
            ViewBag.State = new SelectList(Utility.GetStates(), "Value", "Name");
            ViewBag.Distributor = new SelectList(distributorlist.ListData, "value", "Name");
            if (ClientID != "" && ClientID != "0")
            {
                response = ClientResponse.GetClientDetails(Convert.ToInt32(ClientID));
                if (response.ResultCode > 0)
                {

                    return PartialView("_AddClientDetails", response.Data);
                }
                else
                {
                    return PartialView("_AddClientDetails");
                }

            }
            else
            {
                return PartialView("_AddClientDetails", new Client_MasterInfo());
            }

        }
        public ActionResult GetClientListDetails()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResultList<Client_MasterInfolist> response = new ServiceResultList<Client_MasterInfolist>();
            response = ClientResponse.GetClientList();
            if (response.ListData != null && response.ListData.Count > 0)
            {
                return PartialView("_ClientList", response.ListData);
            }
            else
            {
                return PartialView("_ClientList", response.ListData);
            }
        }
        public ActionResult CheckClientisvalid(string ClientCompany = "", string SuperUser = "")
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();

            ServiceResult<bool> response = new ServiceResult<bool>();
            response = ClientResponse.CheckIsExisCompany(ClientCompany, SuperUser);
            if (response.ResultCode == 1)
            {
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }



        }
        public ActionResult SaveClientDetails()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            string model = Request.Form["Model"];
            Client_MasterInfo Model = jss.Deserialize<Client_MasterInfo>(model);
            var uploadfile = Request.Files;

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            //ViewBag.tempfile = Model.UploadedFile;
            if (ModelState.IsValid)
            {
                if (uploadfile.Count > 0)
                {
                    HttpPostedFileBase file = uploadfile[0];
                    // string img = System.IO.Path.GetFileName(files.FileName);
                    using (MemoryStream ms = new MemoryStream())
                    {

                        file.InputStream.CopyTo(ms);
                        Model.Client_Logo = ms.ToArray();
                    }
                }
                else if (Model.Client_ID == 0)
                {
                    string path = System.IO.Path.Combine(Server.MapPath("/Images/default_logo.png"));
                    Model.Client_Logo = System.IO.File.ReadAllBytes(path);
                }

                response = ClientResponse.AddClientMaster(Model);
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
        [AllowAnonymous]
        public ActionResult Register(string FirstName = "", string LastName = "", string Phone = "", string Mobile = "", string Email = "", string CompanyName = "")
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            Client_MasterInfo model = new Client_MasterInfo();
            string newuser = "";
            if (FirstName != "" || LastName != "" || Phone != "" || Mobile != "" || Email != "" || CompanyName != "")
            {
                if (CompanyName != "")
                {
                    model.Client_Company = CompanyName;
                }
                else
                {
                    model.Client_Company = FirstName;
                }
                model.Contact_person = FirstName;
                model.FirstName = FirstName;
                model.LastName = LastName;
                model.Phone = Phone;
                model.Mobile = Mobile;
                model.Email = Email;
                string path = System.IO.Path.Combine(Server.MapPath("/Images/default_logo.png"));
                model.Client_Logo = System.IO.File.ReadAllBytes(path);
                string checkCompany = ClientResponse.checkisexistcomapnyanduser(model.Client_Company, Email);
                if (checkCompany != "")
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('" + checkCompany + "');</script>");
                }
                else
                {
                    newuser = ClientResponse.RegisterNewuser(model);
                }
                TempData["Message"] = "Please check your register email";
                return RedirectToAction("Login", "Login");
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }

        //Here To delete   Client Details
        public ActionResult DeleteClientDetails(int ClientID)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            response = ClientResponse.DeleteClient(ClientID);
            if (response.ResultCode > 0)
            {
                TempData["DeleteSucessMsg"] = response.Message;
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["DeleteFailMsg"] = response.Message;
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }

        }
        //Client Master Index
        public ActionResult Index()
        {


            Client_MasterInfo model = new Client_MasterInfo();
            // model.EditClientInfo = new EditClient_MasterInfo();

            if (TempData["AddSuccessMsg"] != null)
            {
                ViewBag.SuccessMsg = TempData["AddSuccessMsg"];
            }
            if (TempData["AddFailMsg"] != null)
            {
                ViewBag.ADDFailMsg = TempData["AddFailMsg"];
            }

            if (TempData["DeleteSucessMsg"] != null)
            {
                ViewBag.DeleteSucessMsg = TempData["DeleteSucessMsg"];
            }
            if (TempData["DeleteFailMsg"] != null)
            {
                ViewBag.DeleteFailMsg = TempData["DeleteFailMsg"];
            }
            if (TempData["UpdateSucessMsg"] != null)
            {
                ViewBag.UpdateSucessMsg = TempData["UpdateSucessMsg"];
            }
            if (TempData["UpdateErrorMsg"] != null)
            {
                ViewBag.EditSavedErrorMsg = TempData["UpdateErrorMsg"];
            }
            return View();
        }

        //Here To Add New Client Details
        [HttpPost]
        public ActionResult AddClient(Client_MasterInfo model)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            ViewBag.tempfile = model.UploadedFile;
            if (ModelState.IsValid)
            {
                if (model.UploadedFile != null)
                {
                    string img = System.IO.Path.GetFileName(model.UploadedFile.FileName);
                    //string path = System.IO.Path.Combine(Server.MapPath("~/AppImges/Client_Master"), img);
                    //model.UploadedFile.SaveAs(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        model.UploadedFile.InputStream.CopyTo(ms);
                        model.Client_Logo = ms.ToArray();
                    }
                }


                response = ClientResponse.AddClientMaster(model);
                if (response.ResultCode > 0)
                {
                    TempData["AddSuccessMsg"] = response.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["AddFailMsg"] = response.Message;
                    return RedirectToAction("Index");
                }

            }
            else
            {
                if (model.UploadedFile != null)
                {
                    string img = System.IO.Path.GetFileName(model.UploadedFile.FileName);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        model.UploadedFile.InputStream.CopyTo(ms);
                        model.Client_Logo = ms.ToArray();
                        ViewBag.file = Convert.ToBase64String(model.Client_Logo);
                        ViewBag.upfile = model.UploadedFile;
                        model.UploadedFile = ViewBag.tempfile;
                    }
                }

                return View("Index", model);
            }

        }

        //Here To delete   Client Details
        public ActionResult DeleteClient(int id)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            response = ClientResponse.DeleteClient(id);
            if (response.ResultCode > 0)
            {
                TempData["DeleteSucessMsg"] = response.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["DeleteFailMsg"] = response.Message;
                return RedirectToAction("Index");
            }

        }

        //Here to get ClientDetails For Edit 
        public ActionResult EditClientMaster(int id)
        {

            ServiceResult<EditClient_MasterInfo> response = new ServiceResult<EditClient_MasterInfo>();
            Client_MasterInfo modelobj = new Client_MasterInfo();
            //response.Data.EditClientInfo = new EditClient_MasterInfo();
            //response.Data.EditClientInfo = new EditClient_MasterInfo();

            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            response = ClientResponse.GetClientDetail(id);
            //modelobj.EditClientInfo = response.Data;
            TempData["Logo"] = response.Data.Client_Logo;

            if (response.ResultCode > 0)
            {
                ViewBag.Edit = "Edit";
                //return View("_EditClientMaster", response.Data);
                return View("_EditClientMaster", response.Data);
            }
            else
            {
                ViewBag.Edit = "Edit";
                ViewBag.EditErrorMsg = response.Message;
                return View(response.Data);
            }

            //TempData["logo"] = modelobj.Client_Logo;



        }

        //Here to  Update Client Information 
        [HttpPost]
        public ActionResult EditClientMaster(EditClient_MasterInfo model)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();

            ServiceResult<int> response = new ServiceResult<int>();


            //byte[] logo = ElectricHelper.ObjectToByteArray(TempData["Logo"]);


            //save modified client details after model validation
            if (ModelState.IsValid)
            {
                if (model.UploadedFile != null)
                {
                    string img = System.IO.Path.GetFileName(model.UploadedFile.FileName);
                    //string path = System.IO.Path.Combine(Server.MapPath("~/AppImges/Client_Master"), img);
                    //model.UploadedFile.SaveAs(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        model.UploadedFile.InputStream.CopyTo(ms);
                        model.Client_Logo = ms.ToArray();
                    }
                }

                response = ClientResponse.SaveClientDetails(model);
                if (response.ResultCode > 0)
                {
                    //ViewBag.EditSuccesMsg = ResultCount.Message;
                    TempData["UpdateSucessMsg"] = response.Message;
                    return RedirectToAction("Index");
                }
                else
                {

                    ViewBag.Edit = "Edit";
                    ViewBag.UpdateErrorMsg = response.Message;
                    return View("_EditClientMaster", model);
                }

            }
            else
            {

                ViewBag.Edit = "Edit";

                return View("_EditClientMaster", model);

            }


        }

        //Here to Get My Client Image
        [HttpGet]
        public FileResult GetImage()
        {
            // Client_MasterInfo modelobj = new Client_MasterInfo();
            // List<Client_MasterInfolist> listobj = new List<Client_MasterInfolist>();
            //EstimBLL response = new EstimBLL();
            //modelobj = response.GetClientDetail(id);
            byte[] fileContents = ElectricHelper.ObjectToByteArray(TempData["logo"]);
            string contentType = "image/png";
            return File(fileContents, contentType);
        }


        public ActionResult check()
        {
            return View();
        }

        //

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Populate(int ClientID = 0)
        {
            var results = new List<Dictionary<string, object>>();

            string queryString = @"
                SELECT
	                (SELECT COUNT(*) FROM Assemblies_Master WHERE Client_ID = @ClientID) AS [Assemblies],
	                (SELECT COUNT(*) FROM Assemblies_Parts WHERE Client_ID = @ClientID) AS [Assemblies_Parts],
	                (SELECT COUNT(*) FROM Parts_Details WHERE Client_ID = @ClientID) AS [Parts]
                ;
            ";

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@ClientID", ClientID);

                //

                DataTable dt = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(dt);

                var sampleResults = dt.AsEnumerable();

                foreach (DataRow row in sampleResults)
                {
                    var result = new Dictionary<string, object>();

                    result.Add("Assemblies", row["Assemblies"]);
                    result.Add("Assemblies_Parts", row["Assemblies_Parts"]);
                    result.Add("Parts", row["Parts"]);

                    results.Add(result);
                }
            }

            ServiceResult<EditClient_MasterInfo> response = new ServiceResult<EditClient_MasterInfo>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            response = ClientResponse.GetClientDetail(ClientID);

            ViewBag.ClientID = ClientID;
            ViewBag.Client = response.Data;
            ViewBag.Results = results;

            // return Json(results, JsonRequestBehavior.AllowGet);

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Populate(int clientID = 0, string action = "")
        {
            string query = "";
            int affected = 0;

            if (action == "reset")
            {
                query = @"
                    DELETE FROM Assemblies_Master WHERE Client_Id = @ClientID;
                    DELETE FROM Assemblies_Parts WHERE Client_Id = @ClientID;
                    DELETE FROM Parts_Details WHERE Client_Id = @ClientID;
                ";
            }

            if (action == "submit")
            {
                query = @"
                    INSERT INTO
	                    Assemblies_Master
                    SELECT
	                    @ClientID AS Client_Id,
	                    CONCAT(am.AssemblyID, ': ', am.Assemblies_Name) AS Assemblies_Name,
	                    am.Assemblies_Category,
	                    am.Assemblies_Description,
	                    am.severity,
	                    am.labor_cost,
	                    am.labor_ResaleCost,
	                    am.labor_EstimatedHours,
	                    am.Labor_EstimatedCostTotal,
	                    am.Lobor_EstimatedResaleTotal,
	                    am.Parts_EstimatedCostTotal,
	                    am.Parts_EstimatedResaleTotal,
	                    am.Estimated_Qty_Total,
	                    am.Grand_EstCost_Total,
	                    am.Grand_EstResale_Total,
	                    am.Created_By,
	                    GETDATE() AS Created_Date,
	                    am.Updated_By,
	                    GETDATE() AS Updated_Date,
	                    am.Isactive,
	                    NULL AS [Type]
                    FROM
	                    Distributor_Assemblies_Master AS am
                    ;

                    INSERT INTO
	                    Parts_Details
                    SELECT
	                    @ClientID AS Client_Id,
	                    pd.Part_Number,
	                    pd.Part_Category,
	                    pd.Description,
	                    pd.Client_Description,
	                    pd.Cost,
	                    pd.Resale_Cost,
	                    pd.Purchased_From,
	                    pd.Created_By,
	                    GETDATE() AS Created_Date,
	                    pd.Updated_By,
	                    GETDATE() AS Updated_Date,
	                    pd.IsActive,
	                    pd.UOM,
	                    NULL AS [Type],
	                    pd.LaborUnit
                    FROM
	                    Distributor_Parts_Details AS pd
                    ;

                    INSERT INTO
                        Assemblies_Parts
                    SELECT
                        @ClientID AS Client_Id,
                        CONCAT(ap.AssemblyID, ': ', ap.Assemblies_Name) AS Assemblies_Name,
                        ap.Assemblies_Category,
                        ap.Part_Number,
                        ap.Part_Category,
                        ap.Part_Cost,
                        ap.Parts_Photo,
                        ap.Part_Resale,
                        ap.Estimated_Qty,
                        ap.Actual_Qty,
                        ap.Expected_Total,
                        ap.Estimated_Total,
                        ap.EstimatedCost_Total,
                        ap.EstimatedResale_Total,
                        ap.Actual_Total,
                        ap.IsActive,
                        ap.LaborUnit
                    FROM
                        Distributor_Assemblies_Parts AS ap
                    ;
                ";
            }

            if (!String.IsNullOrEmpty(query))
            {
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ClientID", clientID);

                    connection.Open();
                    affected = command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            Response.Redirect(Request.RawUrl);

            var response = new {
                clientID = clientID,
                action = action,
                affected = affected
            };

            return Json(response);
        }
    }
}
