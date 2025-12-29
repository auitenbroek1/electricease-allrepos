using ElectricEase.BLL.ClientMaster;
using ElectricEase.BLL.PartsMaster;
using ElectricEase.Data.DataBase;
using ElectricEase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using log4net;
using ClosedXML.Excel;

namespace ElectricEase.Web.Controllers
{
    public class PartsExcelExportController : Controller
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(DistributorPartsController));
        PartsMasterBLL PartsResponse = new PartsMasterBLL();
        //
        // GET: /PartsExcelExport/

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            ServiceResultList<distributordropdown> distributorlist = new ServiceResultList<distributordropdown>();
            distributorlist = ClientResponse.distributorList();
            if (distributorlist.ListData.Count > 0)
            {
                var itemToRemove = distributorlist.ListData.FirstOrDefault(r => r.value == -1);
                if (itemToRemove != null)
                {
                    distributorlist.ListData.Remove(itemToRemove);
                }
            }
            ViewBag.Distributor = new SelectList(distributorlist.ListData, "value", "Name");
            ServiceResultList<Standaredclient> standareduser = new ServiceResultList<Standaredclient>();
            standareduser = ClientResponse.StandaredUsers();
            ViewBag.Standared = new SelectList(standareduser.ListData, "value", "Name");
            return View();
        }

        public ActionResult GetClients(int DistId)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            List<Standaredclient> clients = new List<Standaredclient>();
            ServiceResultList<Standaredclient> standareduser = new ServiceResultList<Standaredclient>();
            standareduser = ClientResponse.DistributorClients(DistId);
            clients.AddRange(standareduser.ListData);
            return Json(clients, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DistributorImportparts(int Did)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            //int Client_ID = 0;
            List<PartsNames> partnumber = new List<PartsNames>();
            List<PartsNames> NewParts = new List<PartsNames>();
            string status = "";
            try
            {
                PartsMasterBLL PartsResponse = new PartsMasterBLL();
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string filepath = Path.Combine(Server.MapPath("../Upload"), file.FileName);
                    string ext = Path.GetExtension(filepath);
                    string invalidparts = "";
                    int totalpartsimported = 0;
                    int totalpartsupdated = 0;
                    int totalnewparts = 0;
                    int totalfaildparts = 0;
                    DataTable dat = new DataTable();
                    if (ext == ".xlsx")
                    {
                        file.SaveAs(filepath);
                        String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0; HDR=YES; IMEX=1\"", filepath);
                        using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
                        {
                            excelConnection.Open();
                            dat = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string Excelsheet = "";
                            foreach (DataRow row in dat.Rows)
                            {
                                Excelsheet = row["TABLE_NAME"].ToString();
                            }
                            excelConnection.Close();
                            if (Excelsheet == "Sheet1$")
                            {
                                List<Parts_Details> partslist = new List<Parts_Details>();
                                string countcmd = "Select count(*)from [Sheet1$]";
                                OleDbCommand cocmd = new OleDbCommand(countcmd, excelConnection);
                                excelConnection.Open();
                                int rowcount = (int)cocmd.ExecuteScalar();
                                excelConnection.Close();
                                if (rowcount > 0)
                                {
                                    using (OleDbCommand cmd = new OleDbCommand("Select * from [Sheet1$]", excelConnection))
                                    {
                                        DataTable dt = new DataTable();
                                        excelConnection.Open();

                                        OleDbDataReader dr = cmd.ExecuteReader();
                                        dt.Load(dr);
                                        excelConnection.Close();
                                        excelConnection.Open();
                                        using (OleDbDataReader dReader = cmd.ExecuteReader())
                                        {
                                            var count = dt.Columns.Count;
                                            if (count == 8)
                                            {
                                                string Temp1 = dt.Columns[0].ToString();
                                                string Temp2 = dt.Columns[1].ToString();
                                                string Temp3 = dt.Columns[2].ToString();
                                                string Temp4 = dt.Columns[3].ToString();
                                                string Temp5 = dt.Columns[4].ToString();
                                                string Temp6 = dt.Columns[5].ToString();
                                                string Temp7 = dt.Columns[6].ToString();
                                                // string Temp8 = dt.Columns[7].ToString();
                                                string Temp9 = dt.Columns[7].ToString();
                                                if (Temp1 == "Part Number" && Temp2 == "Category" && Temp3 == "My Cost" && Temp4 == "Markup " && Temp5 == "Resale" && Temp6 == "Purchased From " && Temp7 == "My Description " && Temp9 == "UOM")
                                                {
                                                    //logs.WriteLog("Uploading of Parts Started", Client_ID, Loginuser);
                                                    while (dReader.Read())
                                                    {

                                                        totalpartsimported++;
                                                        // Parts_Details parts = new Parts_Details();
                                                        Distributor_Parts_Details Dparts = new Distributor_Parts_Details();
                                                        Parts_Audit partsAudit = new Parts_Audit();
                                                        Dparts.Part_Number = dReader.GetValue(0).ToString();
                                                        Dparts.Part_Category = dReader.GetValue(1).ToString();
                                                        string number = dReader.GetValue(2).ToString();
                                                        bool isNumber = number.Any(char.IsDigit);
                                                        string Rumber = dReader.GetValue(4).ToString();
                                                        bool RisNumber = Rumber.Any(char.IsDigit);
                                                        if (Dparts.Part_Number != "")
                                                        {
                                                            if (dReader.GetValue(2).ToString() != "" && dReader.GetValue(4).ToString() != "" && isNumber == true && RisNumber == true)
                                                            {
                                                                //if (Convert.ToDecimal(dReader.GetValue(2)) < Convert.ToDecimal(dReader.GetValue(3)))
                                                                //{
                                                                Dparts.Distributor_ID = Did;
                                                                //Dparts.Cost = Convert.ToDecimal(dReader.GetValue(2));
                                                                //Dparts.Resale_Cost = Convert.ToDecimal(dReader.GetValue(3));
                                                                if (dReader.GetValue(2).ToString() == "")
                                                                {
                                                                    Dparts.Cost = 0;
                                                                }
                                                                else
                                                                {
                                                                    Dparts.Cost = Convert.ToDecimal(dReader.GetValue(2));
                                                                }
                                                                if (dReader.GetValue(4).ToString() == "")
                                                                {
                                                                    Dparts.Resale_Cost = 0;
                                                                }
                                                                else
                                                                {
                                                                    Dparts.Resale_Cost = Convert.ToDecimal(dReader.GetValue(4));
                                                                }
                                                                Dparts.Purchased_From = dReader.GetValue(5).ToString();
                                                                Dparts.Description = dReader.GetValue(6).ToString();
                                                                //Dparts.Client_Description = dReader.GetValue(7).ToString();
                                                                Dparts.UOM = dReader.GetValue(7).ToString();
                                                                //parts.UOM = dReader.GetValue(7).ToString();
                                                                Dparts.Client_Id = 0;
                                                                if (Loginuser != "Admin")
                                                                {
                                                                    Dparts.Created_By = result.User_ID;
                                                                    Dparts.Updated_By = result.User_ID;
                                                                }
                                                                else
                                                                {
                                                                    Dparts.Created_By = "Admin";
                                                                    Dparts.Updated_By = "Admin";

                                                                }

                                                                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                                                                {
                                                                    Distributor_Parts_Details dbparts = db.Distributor_Parts_Details.Where(x => x.Distributor_ID == Did && x.Part_Number == Dparts.Part_Number).FirstOrDefault();
                                                                    if (dbparts != null)
                                                                    {

                                                                        partsAudit.Client_ID = dbparts.Client_Id;
                                                                        partsAudit.Part_Category = dbparts.Part_Category;
                                                                        partsAudit.Part_Number = dbparts.Part_Number;
                                                                        partsAudit.Cost = dbparts.Cost ?? 0;
                                                                        partsAudit.Resale_Cost = dbparts.Resale_Cost ?? 0;
                                                                        partsAudit.Purchased_From = dbparts.Purchased_From;
                                                                        partsAudit.Description = dbparts.Description;
                                                                        partsAudit.Client_Description = dbparts.Client_Description;
                                                                        partsAudit.Type = "Distributor Parts";
                                                                        partsAudit.UOM = dbparts.UOM;
                                                                        partsAudit.Distributor_ID = Did;
                                                                        partsAudit.Created_Date = DateTime.Now;
                                                                        db.Parts_Audit.Add(partsAudit);
                                                                        db.SaveChanges();
                                                                        Dparts.Distributor_ID = Did;
                                                                        dbparts.Part_Category = Dparts.Part_Category;
                                                                        dbparts.Cost = Dparts.Cost ?? 0;
                                                                        dbparts.Resale_Cost = Dparts.Resale_Cost;
                                                                        dbparts.Client_Description = Dparts.Client_Description;
                                                                        dbparts.Updated_Date = DateTime.Now;
                                                                        dbparts.Updated_By = Dparts.Updated_By;
                                                                        dbparts.Purchased_From = Dparts.Purchased_From;
                                                                        dbparts.Description = Dparts.Description;
                                                                        dbparts.UOM = Dparts.UOM;
                                                                        dbparts.Type = "Admin Excel";

                                                                        //dbparts.UOM = parts.UOM;
                                                                        if (Dparts.Part_Category != "" && Dparts.Part_Category != null && Dparts.Part_Number != "")
                                                                        {
                                                                            PartsNames updatepartname = new PartsNames();
                                                                            updatepartname.Partnumber = Dparts.Part_Number;
                                                                            updatepartname.Category = Dparts.Part_Category;
                                                                            updatepartname.Cost = Convert.ToString(Dparts.Cost);
                                                                            updatepartname.Resale = Convert.ToString(Dparts.Resale_Cost);
                                                                            updatepartname.Comments = "<span style='color:#4081bd'>“Part” is updated successfully.</span>";
                                                                            partnumber.Add(updatepartname);
                                                                            db.SaveChanges();
                                                                            totalpartsupdated++;
                                                                        }
                                                                        else
                                                                        {
                                                                            invalidparts += Dparts.Part_Number + "<br/>";
                                                                            PartsNames invalid = new PartsNames();
                                                                            invalid.Partnumber = Dparts.Part_Number;
                                                                            invalid.Category = Dparts.Part_Category;
                                                                            invalid.Cost = Convert.ToString(dReader.GetValue(2));
                                                                            invalid.Resale = Convert.ToString(dReader.GetValue(4));
                                                                            invalid.Comments = "<span style='color:#f00'>Failed:Invalid \"Part number\" or \"Part Category\" missing!</span>";
                                                                            partnumber.Add(invalid);
                                                                            totalfaildparts++;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Dparts.IsActive = true;
                                                                        Dparts.Created_Date = DateTime.Now;
                                                                        Dparts.Updated_Date = DateTime.Now;
                                                                        Dparts.Type = "Admin Excel";
                                                                        if (Dparts.Part_Category != "" && Dparts.Part_Category != null && Dparts.Part_Number != "")
                                                                        {

                                                                            //To validate, whether this new parts already inserted in current import process
                                                                            PartsNames oldPartToInsert = NewParts.SingleOrDefault(s => s.Partnumber == Dparts.Part_Number);
                                                                            if (oldPartToInsert != null)
                                                                            {
                                                                                _log.Info("Import Failed for the Part Number - " + Dparts.Part_Number + " due to the duplication when it is inserting for the first time");
                                                                                totalfaildparts++;
                                                                                continue;
                                                                            }

                                                                            db.Distributor_Parts_Details.Add(Dparts);
                                                                            db.SaveChanges();
                                                                            totalnewparts++;
                                                                            PartsNames newpart = new PartsNames();
                                                                            newpart.Partnumber = Dparts.Part_Number;
                                                                            newpart.Category = Dparts.Part_Category;
                                                                            newpart.Cost = Convert.ToString(Dparts.Cost);
                                                                            newpart.Resale = Convert.ToString(Dparts.Resale_Cost);
                                                                            newpart.Comments = "<span style='color:#1eca0b'>“New Part” is added successfully.</span>";
                                                                            partnumber.Add(newpart);
                                                                            NewParts.Add(newpart);
                                                                        }
                                                                        else
                                                                        {
                                                                            invalidparts += Dparts.Part_Number + "<br/>";
                                                                            PartsNames invalid = new PartsNames();
                                                                            invalid.Partnumber = Dparts.Part_Number;
                                                                            invalid.Category = Dparts.Part_Category;
                                                                            invalid.Cost = Convert.ToString(dReader.GetValue(2));
                                                                            invalid.Resale = Convert.ToString(dReader.GetValue(4));
                                                                            invalid.Comments = "<span style='color:#f00'>Failed:Invalid \"Part number\" or \"Part Category\" missing!</span>";
                                                                            partnumber.Add(invalid);
                                                                            totalfaildparts++;
                                                                        }

                                                                    }
                                                                }
                                                                //}
                                                                //else
                                                                //{
                                                                //    invalidparts += Dparts.Part_Number + "(check the cost and resale cost), ";
                                                                //    PartsNames invalids = new PartsNames();
                                                                //    invalids.Partnumber = Dparts.Part_Number;
                                                                //    invalids.Category = Dparts.Part_Category;
                                                                //    invalids.Cost = Convert.ToString(Dparts.Cost);
                                                                //    invalids.Resale = Convert.ToString(Dparts.Resale_Cost);
                                                                //    invalids.Comments = "cost and resale cost check";
                                                                //    partnumber.Add(invalids);
                                                                //}

                                                            }
                                                            else
                                                            {
                                                                invalidparts += Dparts.Part_Number + "(Check the “Cost” and “Resale Cost” have values.), ";
                                                                PartsNames rinvalids = new PartsNames();
                                                                rinvalids.Partnumber = Dparts.Part_Number;
                                                                rinvalids.Category = Dparts.Part_Category;
                                                                rinvalids.Cost = dReader.GetValue(2).ToString();
                                                                rinvalids.Resale = dReader.GetValue(4).ToString();
                                                                rinvalids.Comments = "<span style='color:#f00'>Failed:Check the “Cost” and “Resale Cost” have values.</span>";
                                                                partnumber.Add(rinvalids);
                                                                totalfaildparts++;
                                                            }

                                                        }
                                                        else
                                                        {
                                                            PartsNames parnumbeinvalids = new PartsNames();
                                                            parnumbeinvalids.Partnumber = dReader.GetValue(0).ToString();
                                                            parnumbeinvalids.Category = dReader.GetValue(1).ToString();
                                                            parnumbeinvalids.Cost = dReader.GetValue(2).ToString();
                                                            parnumbeinvalids.Resale = dReader.GetValue(4).ToString();
                                                            parnumbeinvalids.Comments = "<span style='color:#f00'>Failed:“Part Number” is missing!</span>";
                                                            partnumber.Add(parnumbeinvalids);
                                                            totalfaildparts++;
                                                        }

                                                        //partslist.Add(partsdetail);
                                                    }
                                                }
                                                else
                                                {
                                                    status = "Invalid parts template!";
                                                    return Json(status);
                                                }

                                            }
                                            else
                                            {
                                                status = "Invalid parts template!";
                                                return Json(status);
                                            }
                                        }
                                    }
                                    string partreporttable = "";

                                    //if (partnumber.Count > 0)
                                    //{
                                    //    partreporttable += "<div class='table-responsive' id='partsreport'><table class='table table-bordered' cellspacing='0' width='100%'><thead><tr><th>PartNumber</th><th>Category</th><th>Cost</th><th>ResaleCost</th><th>Comment</th></tr></thead><tbody>";
                                    //    foreach (var par in partnumber)
                                    //    {
                                    //        partreporttable += "<tr>";
                                    //        partreporttable += "<td>" + par.Partnumber + "</td>";
                                    //        partreporttable += "<td>" + par.Category + "</td>";
                                    //        partreporttable += "<td>" + par.Cost + "</td>";
                                    //        partreporttable += "<td>" + par.Resale + "</td>";
                                    //        partreporttable += "<td>" + par.Comments + "</td>";
                                    //        partreporttable += "</tr>";
                                    //    }
                                    //    partreporttable += "</tbody></table></div>";
                                    //}



                                    status = "<u>\"Parts\" are imported successfully.</u>: <br> Total Parts Imported :" + totalpartsimported + " ;  Newly added Parts :" + totalnewparts + ";  Parts Updated :" + totalpartsupdated + "; Failed Parts:" + totalfaildparts + " <br><br>" + partreporttable + "";
                                    //if (invalidparts != "" && invalidparts.Length > 1)
                                    //{
                                    //    invalidparts = invalidparts.Substring(0, invalidparts.Length - 1);
                                    //    status += "<br/><br/> (Parts Category is missing for some parts:<b> <br/> " + invalidparts + "<br/></b>So please check these parts and try again, other parts are added successfully)";
                                    //}
                                }
                                else
                                {
                                    status = "In excel \"No\" records are available!";
                                }
                            }
                            else
                            {
                                status = "Invalid parts template!";
                                return Json(status);
                            }
                        }
                        System.IO.File.Delete(filepath);
                    }
                    else
                    {
                        status = "Please import the valid template, the file should be \"xlsx\" format only!";
                    }
                }
                return Json(status);
            }
            catch (Exception ex)
            {
                // logs.WriteLog("[Exception] While uploading the Parts [Error Msg - " + ex.Message, Client_ID, Loginuser);
                return Json(ex.Message);
            }
        }

        //[HttpPost]
        //public ActionResult StClientImportparts(int ClientId)
        //{
        //    ClientMasterBLL ClientResponse = new ClientMasterBLL();
        //    string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //    var result = ClientResponse.GetClientName(Loginuser);
        //    //int Client_ID = 0;
        //    List<PartsNames> partnumber = new List<PartsNames>();
        //    StringBuilder assembliesList = new StringBuilder();
        //    List<string> assemblyToProcess = new List<string>();
        //    List<PartsNames> NewParts = new List<PartsNames>();
        //    string status = "";
        //    try
        //    {
        //        PartsMasterBLL PartsResponse = new PartsMasterBLL();
        //        if (Request.Files.Count > 0)
        //        {
        //            HttpPostedFileBase file = Request.Files[0];
        //            string filepath = Path.Combine(Server.MapPath("../Upload"), file.FileName);
        //            string ext = Path.GetExtension(filepath);
        //            string invalidparts = "";
        //            int totalpartsimported = 0;
        //            int totalpartsupdated = 0;
        //            int totalnewparts = 0;
        //            int totalfailed = 0;
        //            DataTable dat = new DataTable();
        //            if (ext == ".xlsx")
        //            {
        //                file.SaveAs(filepath);

        //                String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0; HDR=YES; IMEX=1\"", filepath);
        //                using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
        //                {
        //                    excelConnection.Open();
        //                    dat = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //                    string Excelsheet = "";
        //                    foreach (DataRow row in dat.Rows)
        //                    {
        //                        Excelsheet = row["TABLE_NAME"].ToString();
        //                    }
        //                    excelConnection.Close();
        //                    if (Excelsheet == "Sheet1$")
        //                    {
        //                        List<Parts_Details> partslist = new List<Parts_Details>();
        //                        string countcmd = "Select count(*)from [Sheet1$]";
        //                        OleDbCommand cocmd = new OleDbCommand(countcmd, excelConnection);
        //                        excelConnection.Open();
        //                        int rowcount = (int)cocmd.ExecuteScalar();
        //                        excelConnection.Close();
        //                        if (rowcount > 0)
        //                        {
        //                            using (OleDbCommand cmd = new OleDbCommand("Select * from [Sheet1$]", excelConnection))
        //                            {
        //                                DataTable dt = new DataTable();
        //                                excelConnection.Open();

        //                                OleDbDataReader dr = cmd.ExecuteReader();
        //                                dt.Load(dr);
        //                                excelConnection.Close();
        //                                excelConnection.Open();

        //                                using (OleDbDataReader dReader = cmd.ExecuteReader())
        //                                {
        //                                    var count = dt.Columns.Count;
        //                                    if (count == 9)
        //                                    {
        //                                        string Temp1 = dt.Columns[0].ToString();
        //                                        string Temp2 = dt.Columns[1].ToString();
        //                                        string Temp3 = dt.Columns[2].ToString();
        //                                        string Temp4 = dt.Columns[3].ToString();
        //                                        string Temp5 = dt.Columns[4].ToString();
        //                                        string Temp6 = dt.Columns[5].ToString();//Labor Unit
        //                                        string Temp7 = dt.Columns[6].ToString();//Purchased Form
        //                                        // string Temp8 = dt.Columns[7].ToString();
        //                                        string Temp9 = dt.Columns[7].ToString();//My Description
        //                                        string Temp10 = dt.Columns[8].ToString();//UOM
        //                                        if (Temp1 == "Part Number" && Temp2 == "Category" && Temp3 == "My Cost" && Temp4 == "Markup " && Temp5 == "Resale" && Temp6 == "Labor Unit" && Temp7 == "Purchased From " && Temp9 == "My Description " && Temp10 == "UOM")
        //                                        {
        //                                            using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
        //                                            {
        //                                                db.Configuration.AutoDetectChangesEnabled = false;
        //                                                db.Configuration.ValidateOnSaveEnabled = false;
        //                                                while (dReader.Read())
        //                                                {
        //                                                    totalpartsimported++;
        //                                                    Parts_Details parts = new Parts_Details();
        //                                                    Parts_Audit partsAudit = new Parts_Audit();
        //                                                    parts.Client_Id = ClientId;
        //                                                    parts.Part_Number = dReader.GetValue(0).ToString().Trim();
        //                                                    parts.Part_Category = dReader.GetValue(1).ToString().Trim();
        //                                                    if (parts.Part_Number != "" && parts.Part_Category != "")
        //                                                    {
        //                                                        if (dReader.GetValue(2) != null && dReader.GetValue(4) != null && dReader.GetValue(2) != DBNull.Value && dReader.GetValue(4) != DBNull.Value)
        //                                                        {
        //                                                            //if (Convert.ToDecimal(dReader.GetValue(2)) < Convert.ToDecimal(dReader.GetValue(4)))
        //                                                            //{
        //                                                            parts.Client_Id = ClientId;
        //                                                            //parts.Cost = Convert.ToDecimal(dReader.GetValue(2))/*;*/
        //                                                            if (dReader.GetValue(2).ToString().Trim() == "")
        //                                                            {
        //                                                                parts.Cost = 0;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                parts.Cost = Convert.ToDecimal(dReader.GetValue(2));
        //                                                            }
        //                                                            if (dReader.GetValue(4).ToString().Trim() == "")
        //                                                            {
        //                                                                parts.Resale_Cost = 0;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                parts.Resale_Cost = Convert.ToDecimal(dReader.GetValue(4));
        //                                                            }

        //                                                            //Labor Unit Calculation
        //                                                            if (string.IsNullOrEmpty(dReader.GetValue(5).ToString().Trim()))
        //                                                            {
        //                                                                parts.LaborUnit = 0;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                parts.LaborUnit = Convert.ToDecimal(dReader.GetValue(5));
        //                                                            }

        //                                                            parts.Purchased_From = dReader.GetValue(6).ToString().Trim();
        //                                                            parts.Description = dReader.GetValue(7).ToString().Trim();
        //                                                            /// parts.Client_Description = dReader.GetValue(7).ToString();
        //                                                            parts.UOM = dReader.GetValue(8).ToString().Trim();
        //                                                            // parts.Client_Id = 0;
        //                                                            if (Loginuser != "Admin")
        //                                                            {
        //                                                                parts.Created_By = result.User_ID;
        //                                                                parts.Updated_By = result.User_ID;
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                parts.Created_By = "Admin";
        //                                                                parts.Updated_By = "Admin";

        //                                                            }

        //                                                            //using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
        //                                                            //{
        //                                                            Parts_Details dbparts = db.Parts_Details.Where(x => x.Client_Id == ClientId && x.Part_Number == parts.Part_Number && x.Part_Category != null).FirstOrDefault();
        //                                                            if (dbparts != null)
        //                                                            {
        //                                                                partsAudit.Client_ID = dbparts.Client_Id;
        //                                                                partsAudit.Part_Category = dbparts.Part_Category;
        //                                                                partsAudit.Part_Number = dbparts.Part_Number;
        //                                                                partsAudit.Cost = dbparts.Cost;
        //                                                                partsAudit.Resale_Cost = dbparts.Resale_Cost ?? 0;
        //                                                                //partsAudit.LaborUnit = dbparts.LaborUnit ?? 0;
        //                                                                partsAudit.Purchased_From = dbparts.Purchased_From;
        //                                                                partsAudit.Description = dbparts.Description;
        //                                                                partsAudit.Client_Description = dbparts.Client_Description;
        //                                                                partsAudit.Type = "Standared Client Parts";
        //                                                                partsAudit.UOM = dbparts.UOM;
        //                                                                partsAudit.Created_Date = DateTime.Now;
        //                                                                db.Parts_Audit.Add(partsAudit);

        //                                                                parts.Client_Id = ClientId;
        //                                                                dbparts.Part_Category = parts.Part_Category;
        //                                                                dbparts.Cost = parts.Cost;
        //                                                                dbparts.Resale_Cost = parts.Resale_Cost;
        //                                                                dbparts.LaborUnit = parts.LaborUnit;
        //                                                                dbparts.Client_Description = parts.Client_Description;
        //                                                                dbparts.Updated_Date = DateTime.Now;
        //                                                                dbparts.Updated_By = parts.Updated_By;
        //                                                                dbparts.Purchased_From = parts.Purchased_From;
        //                                                                dbparts.Description = parts.Description;
        //                                                                dbparts.IsActive = true;
        //                                                                dbparts.Type = "Admin Excel";
        //                                                                dbparts.UOM = parts.UOM;


        //                                                                if (parts.Part_Category != "" && parts.Part_Category != null && parts.Part_Number != "")
        //                                                                {
        //                                                                    PartsNames updatepartname = new PartsNames();
        //                                                                    updatepartname.Partnumber = parts.Part_Number;
        //                                                                    updatepartname.Category = parts.Part_Category;
        //                                                                    updatepartname.Cost = Convert.ToString(parts.Cost);
        //                                                                    updatepartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                                    updatepartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                                    updatepartname.Comments = "<span style='color:#4081bd'>“Part” is updated successfully.</span>";
        //                                                                    partnumber.Add(updatepartname);

        //                                                                    //To updated the existing Client Assembly Parts
        //                                                                    List<Assemblies_Parts> assemblyPartsList = db.Assemblies_Parts.Where(x => x.Part_Number == parts.Part_Number).ToList();

        //                                                                    foreach (Assemblies_Parts assemblyPart in assemblyPartsList)
        //                                                                    {
        //                                                                        assemblyPart.Part_Category = parts.Part_Category;
        //                                                                        assemblyPart.Part_Cost = parts.Cost;
        //                                                                        assemblyPart.Part_Resale = parts.Resale_Cost ?? 0;
        //                                                                        assemblyPart.LaborUnit = parts.LaborUnit;
        //                                                                        assemblyPart.EstimatedCost_Total = (parts.Cost * assemblyPart.Estimated_Qty);
        //                                                                        assemblyPart.EstimatedResale_Total = (parts.Resale_Cost * assemblyPart.Estimated_Qty) ?? 0;

        //                                                                        //To get the assemblies to be processed
        //                                                                        assemblyToProcess.Add(assemblyPart.Assemblies_Name);
        //                                                                    }

        //                                                                    totalpartsupdated++;
        //                                                                }
        //                                                                else
        //                                                                {
        //                                                                    invalidparts += parts.Part_Number + "<br/>";
        //                                                                    PartsNames Addpartname = new PartsNames();
        //                                                                    Addpartname.Partnumber = parts.Part_Number;
        //                                                                    Addpartname.Category = parts.Part_Category;
        //                                                                    Addpartname.Cost = Convert.ToString(parts.Cost);
        //                                                                    Addpartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                                    Addpartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                                    Addpartname.Comments = "<span style='color:#f00'>Failed:Invalid \"Part number\" or \"Part Category\" missing!</span>";
        //                                                                    partnumber.Add(Addpartname);
        //                                                                    totalfailed++;
        //                                                                }
        //                                                            }
        //                                                            else
        //                                                            {
        //                                                                parts.IsActive = true;
        //                                                                parts.Created_Date = DateTime.Now;
        //                                                                parts.Updated_Date = DateTime.Now;
        //                                                                parts.Type = "Admin Excel";
        //                                                                if (parts.Part_Category != "" && parts.Part_Category != null && parts.Part_Number != "")
        //                                                                {
        //                                                                    //To validate, whether this new parts already inserted in current import process
        //                                                                    PartsNames oldPartToInsert = NewParts.SingleOrDefault(s => s.Partnumber.ToLower() == parts.Part_Number.ToLower());
        //                                                                    if (oldPartToInsert != null)
        //                                                                    {
        //                                                                        _log.Info("Import Failed for the Part Number - " + parts.Part_Number + " due to the duplication when it is inserting for the first time");
        //                                                                        totalfailed++;
        //                                                                        continue;
        //                                                                    }

        //                                                                    db.Parts_Details.Add(parts);

        //                                                                    totalnewparts++;
        //                                                                    PartsNames Addpartname = new PartsNames();
        //                                                                    Addpartname.Partnumber = parts.Part_Number;
        //                                                                    Addpartname.Category = parts.Part_Category;
        //                                                                    Addpartname.Cost = Convert.ToString(parts.Cost);
        //                                                                    Addpartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                                    Addpartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                                    Addpartname.Comments = "<span style='color:#1eca0b'>“New Part” is added successfully.</span>";
        //                                                                    partnumber.Add(Addpartname);
        //                                                                    NewParts.Add(Addpartname);
        //                                                                }
        //                                                                else
        //                                                                {
        //                                                                    invalidparts += parts.Part_Number + "<br/>";
        //                                                                    PartsNames Addpartname = new PartsNames();
        //                                                                    Addpartname.Partnumber = parts.Part_Number;
        //                                                                    Addpartname.Category = parts.Part_Category;
        //                                                                    Addpartname.Cost = Convert.ToString(parts.Cost);
        //                                                                    Addpartname.Resale = Convert.ToString(parts.Resale_Cost);
        //                                                                    Addpartname.LaborUnit = Convert.ToString(parts.LaborUnit);
        //                                                                    Addpartname.Comments = "<span style='color:#f00'>Failed:Invalid \"Part number\" or \"Part Category\" missing!</span>";
        //                                                                    partnumber.Add(Addpartname);
        //                                                                    totalfailed++;
        //                                                                }
        //                                                            }
        //                                                            //}
        //                                                            //}
        //                                                            //else
        //                                                            //{
        //                                                            //    invalidparts += parts.Part_Number + "(Check the “Cost” and “Resale Cost” have values.), ";
        //                                                            //    PartsNames Addpartname = new PartsNames();
        //                                                            //    Addpartname.Partnumber = parts.Part_Number;
        //                                                            //    Addpartname.Category = parts.Part_Category;
        //                                                            //    Addpartname.Cost = Convert.ToString(dReader.GetValue(2));
        //                                                            //    Addpartname.Resale = Convert.ToString(dReader.GetValue(4));
        //                                                            //    Addpartname.Comments = "<span style='color:#f00'>Failed:Make sure \"Resale Cost\" is higher than \"Cost\".</span>";
        //                                                            //    partnumber.Add(Addpartname);
        //                                                            //    totalfailed++;
        //                                                            //}
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            invalidparts += parts.Part_Number + "(Check the “Cost” and “Resale Cost” have values.), ";
        //                                                            PartsNames Addpartname = new PartsNames();
        //                                                            Addpartname.Partnumber = parts.Part_Number;
        //                                                            Addpartname.Category = parts.Part_Category;
        //                                                            Addpartname.Cost = Convert.ToString(dReader.GetValue(2));
        //                                                            Addpartname.Resale = Convert.ToString(dReader.GetValue(4));
        //                                                            Addpartname.LaborUnit = Convert.ToString(dReader.GetValue(5));
        //                                                            Addpartname.Comments = "<span style='color:#f00'>Failed:Check the “Cost” and “Resale Cost” have values Not Null</span>";
        //                                                            partnumber.Add(Addpartname);
        //                                                            totalfailed++;
        //                                                        }
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        _log.Info(parts.Part_Number + " - Failed due to Part Number or Part Cateogry is null.");
        //                                                        PartsNames Addpartname = new PartsNames();
        //                                                        Addpartname.Partnumber = parts.Part_Number;
        //                                                        Addpartname.Category = parts.Part_Category;
        //                                                        Addpartname.Cost = Convert.ToString(dReader.GetValue(2));
        //                                                        Addpartname.Resale = Convert.ToString(dReader.GetValue(4));
        //                                                        Addpartname.LaborUnit = Convert.ToString(dReader.GetValue(5));
        //                                                        Addpartname.Comments = "<span style='color:#f00'>Failed:“Part Number” is missing!</span>";
        //                                                        partnumber.Add(Addpartname);
        //                                                        totalfailed++;
        //                                                    }
        //                                                    //partslist.Add(partsdetail);
        //                                                }

        //                                                //To save all the database operations at final
        //                                                db.Configuration.AutoDetectChangesEnabled = true;
        //                                                db.SaveChanges();

        //                                                //To remove the duplicate assembly Ids
        //                                                assemblyToProcess = assemblyToProcess.Distinct().ToList();
        //                                                foreach (string assemly in assemblyToProcess)
        //                                                {
        //                                                    assembliesList.Append(assemly);
        //                                                    assembliesList.Append(',');
        //                                                }

        //                                                //To remove last comma in the list of processed assemblies
        //                                                if (assembliesList.Length > 0)
        //                                                {
        //                                                    assembliesList.Remove((assembliesList.Length - 1), 1);
        //                                                }
        //                                                //db.Database.SqlQuery<int>("exec EE_RecalcClientAssembly @AssemblyList", new SqlParameter("AssemblyList", assembliesList));

        //                                                //To recalculate on Client Assembly Master
        //                                                db.Database.ExecuteSqlCommand("exec EE_RecalcClientAssembly @AssemblyList, @ClientID, @UpdatedBy",
        //                                                    new SqlParameter("AssemblyList", assembliesList.ToString()),
        //                                                    new SqlParameter("ClientID", ClientId),
        //                                                    new SqlParameter("UpdatedBy", "Admin Excel"));
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            status = "Invalid parts template!";
        //                                            return Json(status);
        //                                            //return Json(status);
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        status = "Invalid parts template!";
        //                                        return Json(status);
        //                                    }
        //                                }
        //                            }
        //                            string partreporttable = "";
        //                            //if (partnumber.Count > 0)
        //                            //{
        //                            //    partreporttable += "<div class='table-responsive' id='partsreport'><table class='table table-bordered' cellspacing='0' width='100%'><thead><tr><th>PartNumber</th><th>Category</th><th>Cost</th><th>ResaleCost</th><th>Comment</th></tr></thead><tbody>";
        //                            //    foreach (var par in partnumber)
        //                            //    {
        //                            //        partreporttable += "<tr>";
        //                            //        partreporttable += "<td>" + par.Partnumber + "</td>";
        //                            //        partreporttable += "<td>" + par.Category + "</td>";
        //                            //        partreporttable += "<td>" + par.Cost + "</td>";
        //                            //        partreporttable += "<td>" + par.Resale + "</td>";
        //                            //        partreporttable += "<td>" + par.Comments + "</td>";
        //                            //        partreporttable += "</tr>";
        //                            //    }
        //                            //    partreporttable += "</tbody></table></div>";
        //                            //}
        //                            status = "<u>\"Parts\" are imported successfully.</u>: <br> Total Parts Imported :" + totalpartsimported + " ;  Newly added Parts :" + totalnewparts + ";  Parts Updated :" + totalpartsupdated + "; Failed Parts:" + totalfailed + " <br><br>" + partreporttable + "";
        //                            //if (invalidparts != "" && invalidparts.Length > 1)
        //                            //{
        //                            //    invalidparts = invalidparts.Substring(0, invalidparts.Length - 1);
        //                            //    status += "<br/><br/> (Parts Category is missing for some parts:<b> <br/> " + invalidparts + "<br/></b>So please check these parts and try again, other parts are added successfully)";
        //                            //}
        //                        }
        //                        else
        //                        {
        //                            status = "In excel \"No\" records are available!";
        //                        }
        //                        //Create OleDbCommand to fetch data from Excel 
        //                        //logs.WriteLog("Parts are Uploaded Successfully", Client_ID, Loginuser);
        //                    }
        //                    else
        //                    {
        //                        status = "Invalid parts template!";
        //                        return Json(status);
        //                    }
        //                }

        //                System.IO.File.Delete(filepath);
        //            }
        //            else
        //            {
        //                status = "Please import the valid template, the file should be \"xlsx\" format only!";
        //            }
        //        }
        //        return Json(status);
        //    }
        //    catch (Exception ex)
        //    {
        //        // logs.WriteLog("[Exception] While uploading the Parts [Error Msg - " + ex.Message, Client_ID, Loginuser);
        //        return Json(ex.Message);
        //    }
        //}

        [HttpPost]
        public ActionResult DistributorClientImportparts(int Did, int ClientId)
        {
            ClientMasterBLL ClientResponse = new ClientMasterBLL();
            string Loginuser = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            var result = ClientResponse.GetClientName(Loginuser);
            //int Client_ID = 0;
            List<PartsNames> partnumber = new List<PartsNames>();
            List<PartsNames> NewParts = new List<PartsNames>();
            StringBuilder assembliesList = new StringBuilder();
            List<string> assemblyToProcess = new List<string>();
            string status = "";
            try
            {
                PartsMasterBLL PartsResponse = new PartsMasterBLL();
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string filepath = Path.Combine(Server.MapPath("../Upload"), file.FileName);
                    string ext = Path.GetExtension(filepath);
                    string invalidparts = "";
                    int totalpartsimported = 0;
                    int totalpartsupdated = 0;
                    int totalnewparts = 0;
                    int totalfaildpats = 0;
                    DataTable dat = new DataTable();
                    if (ext == ".xlsx")
                    {
                        file.SaveAs(filepath);
                        String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0; HDR=YES; IMEX=1\"", filepath);
                        using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
                        {
                            excelConnection.Open();
                            dat = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string Excelsheet = "";
                            foreach (DataRow row in dat.Rows)
                            {
                                Excelsheet = row["TABLE_NAME"].ToString();
                            }
                            excelConnection.Close();
                            if (Excelsheet == "Sheet1$")
                            {
                                List<Parts_Details> partslist = new List<Parts_Details>();
                                string countcmd = "Select count(*)from [Sheet1$]";
                                OleDbCommand cocmd = new OleDbCommand(countcmd, excelConnection);
                                excelConnection.Open();
                                int rowcount = (int)cocmd.ExecuteScalar();
                                excelConnection.Close();
                                if (rowcount > 0)
                                {
                                    using (OleDbCommand cmd = new OleDbCommand("Select * from [Sheet1$]", excelConnection))
                                    {
                                        DataTable dt = new DataTable();
                                        excelConnection.Open();
                                        OleDbDataReader dr = cmd.ExecuteReader();
                                        dt.Load(dr);
                                        excelConnection.Close();
                                        excelConnection.Open();
                                        using (OleDbDataReader dReader = cmd.ExecuteReader())
                                        {
                                            //logs.WriteLog("Uploading of Parts Started", Client_ID, Loginuser);
                                            var count = dt.Columns.Count;
                                            if (count == 9)
                                            {
                                                string Temp1 = dt.Columns[0].ToString();//Part Number
                                                string Temp2 = dt.Columns[1].ToString();//Category
                                                string Temp3 = dt.Columns[2].ToString();//My Cost
                                                string Temp4 = dt.Columns[3].ToString();//Markup
                                                string Temp5 = dt.Columns[4].ToString();//Resale
                                                string Temp6 = dt.Columns[5].ToString();//Labor Unit
                                                string Temp7 = dt.Columns[6].ToString();//Purchased Form
                                                // string Temp8 = dt.Columns[7].ToString();
                                                string Temp9 = dt.Columns[7].ToString();//My Description
                                                string Temp10 = dt.Columns[8].ToString();//UOM
                                                if (Temp1 == "Part Number" && Temp2 == "Category" && Temp3 == "My Cost" && Temp4 == "Markup " && Temp5 == "Resale" && Temp6 == "Labor Unit" && Temp7 == "Purchased From " && Temp9 == "My Description " && Temp10 == "UOM")
                                                {
                                                    using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                                                    {
                                                        db.Configuration.AutoDetectChangesEnabled = false;
                                                        db.Configuration.ValidateOnSaveEnabled = false;
                                                        while (dReader.Read())
                                                        {

                                                            totalpartsimported++;
                                                            Parts_Details parts = new Parts_Details();
                                                            Parts_Audit partsAudit = new Parts_Audit();
                                                            parts.Part_Number = dReader.GetValue(0).ToString().Trim();
                                                            parts.Part_Category = dReader.GetValue(1).ToString().Trim();
                                                            string number = dReader.GetValue(2).ToString();
                                                            bool isNumber = number.Any(char.IsDigit);
                                                            string Rumber = dReader.GetValue(4).ToString();
                                                            bool RisNumber = Rumber.Any(char.IsDigit);
                                                            if (parts.Part_Number != "" && parts.Part_Category != "")
                                                            {
                                                                if (dReader.GetValue(2).ToString() != "" && dReader.GetValue(4).ToString() != "" && isNumber == true && RisNumber == true)
                                                                {
                                                                    parts.Client_Id = ClientId;
                                                                    //parts.Cost = Convert.ToDecimal(dReader.GetValue(2));
                                                                    //parts.Resale_Cost = Convert.ToDecimal(dReader.GetValue(3));
                                                                    if (dReader.GetValue(2).ToString().Trim() == "")
                                                                    {
                                                                        parts.Cost = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        parts.Cost = Convert.ToDecimal(dReader.GetValue(2));
                                                                    }
                                                                    if (dReader.GetValue(4).ToString().Trim() == "")
                                                                    {
                                                                        parts.Resale_Cost = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        parts.Resale_Cost = Convert.ToDecimal(dReader.GetValue(4));
                                                                    }

                                                                    //Labor Unit Calculation
                                                                    if (string.IsNullOrEmpty(dReader.GetValue(5).ToString().Trim()))
                                                                    {
                                                                        parts.LaborUnit = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        parts.LaborUnit = Convert.ToDecimal(dReader.GetValue(5));
                                                                    }

                                                                    parts.Purchased_From = dReader.GetValue(6).ToString().Trim();
                                                                    parts.Description = dReader.GetValue(7).ToString().Trim();
                                                                    // parts.Client_Description = dReader.GetValue(7).ToString();
                                                                    parts.UOM = dReader.GetValue(8).ToString().Trim();
                                                                    //parts.Client_Id = 0;
                                                                    if (Loginuser != "Admin")
                                                                    {
                                                                        parts.Created_By = result.User_ID;
                                                                        parts.Updated_By = result.User_ID;
                                                                    }
                                                                    else
                                                                    {
                                                                        parts.Created_By = "Admin";
                                                                        parts.Updated_By = "Admin";

                                                                    }

                                                                    //using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                                                                    //{
                                                                    Parts_Details dbparts = db.Parts_Details.Where(x => x.Client_Id == ClientId && x.Part_Number == parts.Part_Number && x.Part_Category != null).FirstOrDefault();
                                                                    if (dbparts != null)
                                                                    {
                                                                        partsAudit.Client_ID = dbparts.Client_Id;
                                                                        partsAudit.Part_Category = dbparts.Part_Category;
                                                                        partsAudit.Part_Number = dbparts.Part_Number;
                                                                        partsAudit.Cost = dbparts.Cost;
                                                                        partsAudit.Resale_Cost = dbparts.Resale_Cost ?? 0;
                                                                        //partsAudit.LaborUnit = dbparts.LaborUnit ?? 0;
                                                                        partsAudit.Purchased_From = dbparts.Purchased_From;
                                                                        partsAudit.Description = dbparts.Description;
                                                                        partsAudit.Client_Description = dbparts.Client_Description;
                                                                        partsAudit.Distributor_ID = Did;
                                                                        partsAudit.Type = "Distributor Client Parts";
                                                                        partsAudit.UOM = dbparts.UOM;
                                                                        partsAudit.Created_Date = DateTime.Now;
                                                                        db.Parts_Audit.Add(partsAudit);

                                                                        parts.Client_Id = ClientId;
                                                                        dbparts.Part_Category = parts.Part_Category;
                                                                        dbparts.Cost = parts.Cost;
                                                                        dbparts.Resale_Cost = parts.Resale_Cost;
                                                                        dbparts.LaborUnit = parts.LaborUnit;
                                                                        dbparts.Client_Description = parts.Client_Description;
                                                                        dbparts.Updated_Date = DateTime.Now;
                                                                        dbparts.Updated_By = parts.Updated_By;
                                                                        dbparts.Purchased_From = parts.Purchased_From;
                                                                        dbparts.Description = parts.Description;
                                                                        dbparts.IsActive = true;
                                                                        dbparts.Type = "Admin Excel";
                                                                        dbparts.UOM = parts.UOM;

                                                                        if (parts.Part_Category != "" && parts.Part_Category != null && parts.Part_Number != "")
                                                                        {
                                                                            PartsNames updatepartname = new PartsNames();
                                                                            updatepartname.Partnumber = parts.Part_Number;
                                                                            updatepartname.Category = parts.Part_Category;
                                                                            updatepartname.Cost = Convert.ToString(parts.Cost);
                                                                            updatepartname.Resale = Convert.ToString(parts.Resale_Cost);
                                                                            updatepartname.LaborUnit = Convert.ToString(parts.LaborUnit);
                                                                            updatepartname.Comments = "<span style='color:#4081bd'>“Part” is updated successfully.</span>";

                                                                            partnumber.Add(updatepartname);

                                                                            //To updated the existing Client Assembly Parts
                                                                            List<Assemblies_Parts> assemblyPartsList = db.Assemblies_Parts.Where(x => x.Part_Number == parts.Part_Number && x.Client_ID == parts.Client_Id).ToList();

                                                                            foreach (Assemblies_Parts assemblyPart in assemblyPartsList)
                                                                            {
                                                                                assemblyPart.Part_Category = parts.Part_Category;
                                                                                assemblyPart.Part_Cost = parts.Cost;
                                                                                assemblyPart.Part_Resale = parts.Resale_Cost ?? 0;
                                                                                assemblyPart.LaborUnit = parts.LaborUnit;
                                                                                assemblyPart.EstimatedCost_Total = (parts.Cost * assemblyPart.Estimated_Qty);
                                                                                assemblyPart.EstimatedResale_Total = (parts.Resale_Cost * assemblyPart.Estimated_Qty) ?? 0;

                                                                                //To get the assemblies to be processed
                                                                                assemblyToProcess.Add(assemblyPart.Assemblies_Name);
                                                                            }

                                                                            totalpartsupdated++;
                                                                        }
                                                                        else
                                                                        {
                                                                            totalfaildpats++;
                                                                            invalidparts += parts.Part_Number + "<br/>";
                                                                            PartsNames pdatepartname = new PartsNames();
                                                                            pdatepartname.Partnumber = parts.Part_Number;
                                                                            pdatepartname.Category = parts.Part_Category;
                                                                            pdatepartname.Cost = Convert.ToString(parts.Cost);
                                                                            pdatepartname.Resale = Convert.ToString(parts.Resale_Cost);
                                                                            pdatepartname.LaborUnit = Convert.ToString(parts.LaborUnit);
                                                                            pdatepartname.Comments = "<span style='color:#f00'>Failed:Invalid \"Part number\" or \"Part Category\" missing!</span>";
                                                                            partnumber.Add(pdatepartname);

                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        parts.Client_Id = ClientId;
                                                                        parts.IsActive = true;
                                                                        parts.Created_Date = DateTime.Now;
                                                                        parts.Updated_Date = DateTime.Now;
                                                                        parts.Type = "Admin Excel";
                                                                        if (parts.Part_Category != "" && parts.Part_Category != null && parts.Part_Number != "")
                                                                        {

                                                                            //To validate, whether this new parts already inserted in current import process
                                                                            PartsNames oldPartToInsert = NewParts.SingleOrDefault(s => s.Partnumber.ToLower() == parts.Part_Number.ToLower());
                                                                            if (oldPartToInsert != null)
                                                                            {
                                                                                _log.Info("Import Failed for the Part Number - " + parts.Part_Number + " due to the duplication when it is inserting for the first time");
                                                                                totalfaildpats++;
                                                                                continue;
                                                                            }

                                                                            db.Parts_Details.Add(parts);

                                                                            totalnewparts++;
                                                                            PartsNames updatepartname = new PartsNames();
                                                                            updatepartname.Partnumber = parts.Part_Number;
                                                                            updatepartname.Category = parts.Part_Category;
                                                                            updatepartname.Cost = Convert.ToString(parts.Cost);
                                                                            updatepartname.Resale = Convert.ToString(parts.Resale_Cost);
                                                                            updatepartname.LaborUnit = Convert.ToString(parts.LaborUnit);
                                                                            updatepartname.Comments = "<span style='color:#1eca0b'>“New Part” is added successfully.</span>";
                                                                            partnumber.Add(updatepartname);
                                                                            NewParts.Add(updatepartname);
                                                                        }
                                                                        else
                                                                        {
                                                                            invalidparts += parts.Part_Number + "<br/>";
                                                                            PartsNames updatepartname = new PartsNames();
                                                                            updatepartname.Partnumber = parts.Part_Number;
                                                                            updatepartname.Category = parts.Part_Category;
                                                                            updatepartname.Cost = Convert.ToString(parts.Cost);
                                                                            updatepartname.Resale = Convert.ToString(parts.Resale_Cost);
                                                                            updatepartname.LaborUnit = Convert.ToString(parts.LaborUnit);
                                                                            updatepartname.Comments = "<span style='color:#f00'>Failed:Invalid \"Part number\" or \"Part Category\" missing!</span>";
                                                                            partnumber.Add(updatepartname);
                                                                            totalfaildpats++;
                                                                        }

                                                                    }
                                                                    //}

                                                                }
                                                                else
                                                                {
                                                                    invalidparts += parts.Part_Number + "<br/>";
                                                                    PartsNames updatepartname = new PartsNames();
                                                                    updatepartname.Partnumber = parts.Part_Number;
                                                                    updatepartname.Category = parts.Part_Category;
                                                                    updatepartname.Cost = dReader.GetValue(2).ToString();
                                                                    updatepartname.Resale = dReader.GetValue(4).ToString();
                                                                    updatepartname.LaborUnit = dReader.GetValue(5).ToString();
                                                                    updatepartname.Comments = "<span style='color:#f00'>Failed:check part Cost or Resale cost</span>";
                                                                    partnumber.Add(updatepartname);
                                                                    totalfaildpats++;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                _log.Info(parts.Part_Number + " - Failed due to Part Number or Part Cateogry is null.");
                                                                PartsNames updatepartname = new PartsNames();
                                                                updatepartname.Partnumber = parts.Part_Number;
                                                                updatepartname.Category = parts.Part_Category;
                                                                updatepartname.Cost = dReader.GetValue(2).ToString();
                                                                updatepartname.Resale = dReader.GetValue(4).ToString();
                                                                updatepartname.LaborUnit = dReader.GetValue(5).ToString();
                                                                updatepartname.Comments = "<span style='color:#f00'>Failed:“Part Number” is missing!</span>";
                                                                partnumber.Add(updatepartname);
                                                                totalfaildpats++;
                                                            }

                                                            //partslist.Add(partsdetail);
                                                        }

                                                        //To save all the database operations at final
                                                        db.Configuration.AutoDetectChangesEnabled = true;
                                                        db.SaveChanges();

                                                        //To remove the duplicate assembly Ids
                                                        assemblyToProcess = assemblyToProcess.Distinct().ToList();
                                                        foreach (string assemly in assemblyToProcess)
                                                        {
                                                            assembliesList.Append(assemly);
                                                            assembliesList.Append(',');
                                                        }

                                                        //To remove last comma in the list of processed assemblies
                                                        if (assembliesList.Length > 0)
                                                        {
                                                            assembliesList.Remove((assembliesList.Length - 1), 1);
                                                        }
                                                        //db.Database.SqlQuery<int>("exec EE_RecalcClientAssembly @AssemblyList", new SqlParameter("AssemblyList", assembliesList));

                                                        //To recalculate on Client Assembly Master
                                                        db.Database.ExecuteSqlCommand("exec EE_RecalcClientAssembly @AssemblyList, @ClientID, @UpdatedBy",
                                                            new SqlParameter("AssemblyList", assembliesList.ToString()),
                                                            new SqlParameter("ClientID", ClientId),
                                                            new SqlParameter("UpdatedBy", "Admin Excel"));
                                                    }
                                                }
                                                else
                                                {
                                                    status = "Invalid parts template!";
                                                    return Json(status);
                                                }
                                            }
                                            else
                                            {
                                                status = "Invalid parts template!";
                                                return Json(status);
                                            }
                                        }
                                    }

                                    string partreporttable = "";

                                    //if (partnumber.Count > 0)
                                    //{
                                    //    partreporttable += "<div class='table-responsive' id='partsreport'><table class='table table-bordered' cellspacing='0' width='100%'><thead><tr><th>PartNumber</th><th>Category</th><th>Cost</th><th>ResaleCost</th><th>Comment</th></tr></thead><tbody>";
                                    //    foreach (var par in partnumber)
                                    //    {
                                    //        partreporttable += "<tr>";
                                    //        partreporttable += "<td>" + par.Partnumber + "</td>";
                                    //        partreporttable += "<td>" + par.Category + "</td>";
                                    //        partreporttable += "<td>" + par.Cost + "</td>";
                                    //        partreporttable += "<td>" + par.Resale + "</td>";
                                    //        partreporttable += "<td>" + par.Comments + "</td>";
                                    //        partreporttable += "</tr>";
                                    //    }
                                    //    partreporttable += "</tbody></table></div>";
                                    //}



                                    status = "<u>\"Parts\" are imported successfully.</u>: <br> Total Parts Imported :" + totalpartsimported + " ;  Newly added Parts :" + totalnewparts + ";  Parts Updated :" + totalpartsupdated + "; Failed Parts:" + totalfaildpats + " <br><br> " + partreporttable + "";
                                    //if (invalidparts != "" && invalidparts.Length > 1)
                                    //{
                                    //    invalidparts = invalidparts.Substring(0, invalidparts.Length - 1);
                                    //    status += "<br/><br/> (Parts Category is missing for some parts:<b> <br/> " + invalidparts + "<br/></b>So please check these parts and try again, other parts are added successfully)";
                                    //}
                                }
                                else
                                {
                                    status = "In excel \"No\" records are available!";
                                }
                            }
                            else
                            {
                                status = "Invalid parts template!";
                                return Json(status);
                            }
                        }
                        System.IO.File.Delete(filepath);
                    }
                    else
                    {
                        status = "Please import the valid template, the file should be \"xlsx\" format only!";
                    }
                }
                return Json(status);
            }
            catch (Exception ex)

            {
                // logs.WriteLog("[Exception] While uploading the Parts [Error Msg - " + ex.Message, Client_ID, Loginuser);
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult ImportParts(int clientID)
        {
            _log.Info("ImportParts - Starts");

            //declarations
            string returnText = string.Empty;
            DataTable dt = new DataTable();
            List<string> assemblyToProcess = new List<string>();
            StringBuilder assembliesList = new StringBuilder();
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    string filepath = Path.Combine(Server.MapPath("../Upload"), file.FileName);
                    string ext = Path.GetExtension(filepath);

                    if (ext == ".xlsx")
                    {
                        file.SaveAs(filepath);
                        String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0; HDR=YES; IMEX=1\"", filepath);
                        using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
                        {
                            //Create OleDbCommand to fetch data from Excel 
                            using (OleDbDataAdapter da = new OleDbDataAdapter("Select * from [Sheet1$]", excelConnection))
                            {
                                excelConnection.Open();

                                dt.Columns.Add("Part Number", typeof(string));
                                dt.Columns.Add("Category", typeof(string));
                                dt.Columns.Add("My Cost", typeof(string));
                                dt.Columns.Add("Markup ", typeof(string));
                                dt.Columns.Add("Resale", typeof(string));
                                dt.Columns.Add("Labor Unit", typeof(string));
                                dt.Columns.Add("Purchased From", typeof(string));
                                dt.Columns.Add("My Description", typeof(string));
                                dt.Columns.Add("UOM", typeof(string));
                                da.Fill(dt);

                                //Adding Row Number
                                DataTable dtExcelParts = new DataTable(dt.TableName);
                                DataColumn dc = new DataColumn("RowNumber");
                                dc.AutoIncrement = true;
                                dc.AutoIncrementSeed = 1;
                                dc.AutoIncrementStep = 1;
                                dc.DataType = typeof(Int32);
                                dtExcelParts.Columns.Add(dc);
                                dc.SetOrdinal(0);
                                dtExcelParts.BeginLoadData();
                                DataTableReader dtReader = new DataTableReader(dt);
                                dtExcelParts.Load(dtReader);
                                dtExcelParts.EndLoadData();

                                //Adding Guid
                                Guid id = Guid.NewGuid();
                                dc = new DataColumn("ProcessID", typeof(String));
                                dc.DefaultValue = id;
                                dtExcelParts.Columns.Add(dc);
                                dc.SetOrdinal(0);

                                DataTable dtImportPartsReport = PartsResponse.ImportParts(dtExcelParts, clientID);
                                Session.Add("dtImportPartsReport", dtImportPartsReport);

                                int totalRows = dtImportPartsReport.Rows.Count;
                                int failedRows = dtImportPartsReport.Select("Status = 'Error'").Length;
                                int insertedRows = dtImportPartsReport.Select("Status = 'Inserted'").Length;
                                int updatedRows = dtImportPartsReport.Select("Status = 'Updated'").Length;

                                returnText = "Total Parts Imported: " + totalRows + "; Newly added Parts: " + insertedRows + "; Parts Updated : "
                                    + updatedRows + "; Failed Parts: " + failedRows;
                            }
                        }
                    }
                    else
                    {
                        returnText = "Error: File extension is not correct!";
                        _log.Error(returnText);
                        _log.Info("ImportParts - Exits");
                    }
                }
                else
                {
                    returnText = "Error: No file exists!";
                    _log.Error(returnText);
                    _log.Info("ImportParts - Exits");
                }
                return Json(returnText);
            }
            catch (Exception ex)
            {
                returnText = "Error: " + ex.Message;
                _log.Error(ex.Message);
                return Json(returnText);
            }
        }

        public void GenerateExcelReport()
        {
            _log.Info("GenerateExcelReport - Enters");
            try
            {
                DataTable dt = Session["dtImportPartsReport"] as DataTable;

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Customers");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=PartsExcelImport-Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
            }
            _log.Info("GenerateExcelReport - Exits");
        }
    }
}
