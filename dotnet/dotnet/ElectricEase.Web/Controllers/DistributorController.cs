using ElectricEase.BLL.ClientMaster;
using ElectricEase.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ElectricEase.Web.Controllers
{
    //[AllowAnonymous]
    public class DistributorController : Controller
    {
        //
        // GET: /Distributor/

        [Authorize(Roles = "Admin")]
        public ActionResult Index(int id=0)
        {
            if (id > 0)
            {
                ClientMasterBLL ClientResponse = new ClientMasterBLL();
                ServiceResult<Distributor> model = new ServiceResult<Distributor>();
                model = ClientResponse.EditDistributor(id);
                return View(model.Data);
            }
            return View(new Distributor());
        }

        public ActionResult SaveDistributor()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();

            string model = Request.Form["Model"];
              Distributor Model= jss.Deserialize<Distributor>(model);
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResult<int> response = new ServiceResult<int>();
            ServiceResult<bool> isexists = new ServiceResult<bool>();  
            // Model.Company.First().ToString().ToUpper() + Model.Company.Substring(1);
            var uploadfile = Request.Files;
            //bool isexists = false;
            if (Model.ID==0)
            {
                isexists = ClientResponse.IsDistributorExiste(Model);
            }
         
            if (ModelState.IsValid)
            {
               
                if(isexists.Data==false)
                {
                    if (uploadfile.Count > 0)
                    {
                        HttpPostedFileBase file = uploadfile[0];
                        using (MemoryStream ms = new MemoryStream())
                        {

                            file.InputStream.CopyTo(ms);
                            string Directorypath = HttpContext.Server.MapPath("~/DistributorImages/");
                            System.IO.Directory.CreateDirectory(Directorypath);
                            var filename = Directorypath + file.FileName;
                            file.SaveAs(filename);
                            var path = "";
                            path = "http://" + Request.Url.Authority + "/DistributorImages/" + file.FileName;
                            Model.CompanyLogo = path;
                        }


                    }
                    else if (Model.ID != 0)
                    {
                        Model.CompanyLogo = Model.Logo;
                    }

                    response = ClientResponse.SaveDistributor(Model);
                }
                
            }
            if (response.ResultCode > 0)
            {
                TempData["AddSuccessMsg"] = response.Message;
                return Json(response.Message);
            }
            else
            {
                if(isexists.Data==true)
                {
                    response.Message = isexists.Message;
                }
                TempData["AddFailMsg"] = response.Message;
                return Json(response.Message);
            }
        }

        public ActionResult GetDistributorList()
        {
            //List<Distributor> model = new List<Distributor>();
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResultList<Distributor> response = new ServiceResultList<Distributor>();
            response = ClientResponse.GetDistributorlist();
            return PartialView("_GetDistributorList", response.ListData);  
        }

        //public ActionResult EditDistributor(int id)
        //{
            
        //    return RedirectToAction("Index", model.Data);
        //}
    }
}
