using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectricEase.BLL;
using ElectricEase.Models;
using ElectricEase.Helpers;
using System.IO;

namespace ElectricEase.Web.Controllers
{
    public class MasterController : Controller
    {
        public ActionResult Client_Master()
        {
            return View();
        }

        public ActionResult Account_Master()
        {
            return View();
        }
        public ActionResult Parts_Master()
        {
            return View();
        }
        public ActionResult NR_Parts_Master()
        {
            return View();
        }

        public ActionResult Labour_Master()
        {
            return View();
        }
        public ActionResult Legal_Master()
        {
            return View();
        }


        public ActionResult Jobs()
        {
            return View();
        }
        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult getimage(Client_MasterInfo model)
        {
            //HttpPostedFileBase photo = Request.Files["photo"];
            HttpPostedFileBase assignmentFile = Request.Files[0];
            if (assignmentFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(assignmentFile.FileName);
                model.FileLocation = Path.Combine(
                    Server.MapPath("~/App_Data/uploads"), fileName);
                //assignmentFile.SaveAs(model.FileLocation);
            }
            return View();
        }

    }
}
