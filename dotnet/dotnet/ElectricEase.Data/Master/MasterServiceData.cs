using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Models;
using ElectricEase.Data.DataBase;

namespace ElectricEase.Data
{
    public class MasterServiceData
    {
        public List<ClientList> GetMyClientList()
        {
            List<ClientList> clientlist = new List<ClientList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    clientlist = db.Client_Master.Where(m => m.IsActive == true).OrderBy(a => a.Client_Company).Select(m => new ClientList()
                    {
                        Client_ID = m.Client_ID,
                        Client_Company = m.Client_Company

                    }).ToList();

                    return clientlist;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<PartsCategoryList> GetPartsCategoryList()
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {

                    PartsList = (from PartsTbl in db.Parts_Details
                                 join CMtbl
                                 in db.Client_Master
                                 on PartsTbl.Client_Id equals CMtbl.Client_ID
                                 where (PartsTbl.IsActive == true && CMtbl.IsActive == true && PartsTbl.Part_Category != "")
                                 select new PartsCategoryList()
                                 {
                                     Part_Category = PartsTbl.Part_Category

                                 }).Distinct().ToList();

                    //PartsList = db.Parts_Details.Where(m => m.IsActive == true && m.Part_Category!=null).Select(m => new PartsCategoryList()
                    //{
                    //    Part_Category = m.Part_Category

                    //}).Distinct().ToList();
                }
                return PartsList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// GetDistributorPartsCategoryList
        /// </summary>
        /// <returns></returns>
        public List<PartsCategoryList> GetDistributorPartsCategoryList()
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {

                    PartsList = (from PartsTbl in db.Distributor_Parts_Details
                                 where (PartsTbl.IsActive == true && PartsTbl.Part_Category != "")
                                 select new PartsCategoryList()
                                 {
                                     Part_Category = PartsTbl.Part_Category
                                 }).Distinct().ToList();

                    //PartsList = db.Parts_Details.Where(m => m.IsActive == true && m.Part_Category!=null).Select(m => new PartsCategoryList()
                    //{
                    //    Part_Category = m.Part_Category

                    //}).Distinct().ToList();
                }
                return PartsList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public List<PartsCategoryList> GetNationalwidePartsCategoryList()
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {

                    PartsList = (from PartsTbl in db.Super_Admin_Parts_Details
                                 where (PartsTbl.IsActive == true && PartsTbl.Part_Category != "")
                                 select new PartsCategoryList()
                                 {
                                     Part_Category = PartsTbl.Part_Category
                                 }).Distinct().ToList();

                    //PartsList = db.Parts_Details.Where(m => m.IsActive == true && m.Part_Category!=null).Select(m => new PartsCategoryList()
                    //{
                    //    Part_Category = m.Part_Category

                    //}).Distinct().ToList();
                }
                return PartsList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public List<PartsCategoryList> GetMyPartsCategoryList(int ClientID)
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    PartsList = (from PartsTbl in db.Parts_Details
                                 join CMtbl
                                 in db.Client_Master
                                 on PartsTbl.Client_Id equals CMtbl.Client_ID
                                 where (PartsTbl.IsActive == true && CMtbl.IsActive == true && PartsTbl.Client_Id == ClientID && PartsTbl.Part_Category != "")
                                 select new PartsCategoryList()
                                 {
                                     Part_Category = PartsTbl.Part_Category,
                                     Value = PartsTbl.Part_Category,
                                     Name = PartsTbl.Part_Category
                                 }).Distinct().ToList();

                    //PartsList = db.Parts_Details.Where(m => m.IsActive == true && m.Client_Id == ClientID).Select(m => new PartsCategoryList()
                    //{
                    //    Part_Category = m.Part_Category

                    //}).Distinct().ToList();
                }
                return PartsList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// GetMyDistributorPartsCategoryList
        /// </summary>
        /// <param name="DistributorID"></param>
        /// <returns></returns>
        public List<PartsCategoryList> GetMyDistributorPartsCategoryList(int Did)
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    PartsList = (from PartsTbl in db.Distributor_Parts_Details
                                 where (PartsTbl.IsActive == true && PartsTbl.Distributor_ID == Did && PartsTbl.Part_Category != "")
                                 select new PartsCategoryList()
                                 {
                                     Part_Category = PartsTbl.Part_Category

                                 }).Distinct().ToList();
                }
                return PartsList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// GetMyDistributorPartsCategoryList
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public List<PartsCategoryList> GetMyDistributorPartsCategoryList()
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    PartsList = (from PartsTbl in db.Distributor_Parts_Details
                                 where (PartsTbl.IsActive == true && PartsTbl.Part_Category != "")
                                 select new PartsCategoryList()
                                 {
                                     Part_Category = PartsTbl.Part_Category

                                 }).Distinct().ToList();

                    //PartsList = db.Parts_Details.Where(m => m.IsActive == true && m.Client_Id == ClientID).Select(m => new PartsCategoryList()
                    //{
                    //    Part_Category = m.Part_Category

                    //}).Distinct().ToList();
                }
                return PartsList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// GetNationwidepartscategory
        /// </summary>
        /// <returns></returns>
        public List<PartsCategoryList> GetNationwidepartscategory()
        {
            List<PartsCategoryList> PartsList = new List<PartsCategoryList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    PartsList = (from PartsTbl in db.Super_Admin_Parts_Details
                                 select new PartsCategoryList()
                                 {
                                     Part_Category = PartsTbl.Part_Category
                                 }).Distinct().ToList();

                    //PartsList = db.Parts_Details.Where(m => m.IsActive == true && m.Client_Id == ClientID).Select(m => new PartsCategoryList()
                    //{
                    //    Part_Category = m.Part_Category

                    //}).Distinct().ToList();
                }
                return PartsList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<LabourCategoryList> GetLabourCategoryList()
        {
            List<LabourCategoryList> LabourList = new List<LabourCategoryList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    LabourList = (from labortbl in db.Labor_Details
                                  join CMtbl
                                  in db.Client_Master
                                  on labortbl.Client_Id equals CMtbl.Client_ID
                                  where (labortbl.IsActive == true && CMtbl.IsActive == true && labortbl.Labor_Category != "")
                                  select new LabourCategoryList()
                                  {
                                      Labor_Category = labortbl.Labor_Category

                                  }).Distinct().ToList();

                    //LabourList = db.Labor_Details.Where(m => m.IsActive == true && m.Labor_Category != null).Select(m => new LabourCategoryList()
                    //{ 



                    //}).Distinct().ToList();
                }
                return LabourList;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public List<LabourCategoryList> GetMyLabourCategoryList(int ClientID)
        {
            List<LabourCategoryList> LabourList = new List<LabourCategoryList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    LabourList = (from labortbl in db.Labor_Details
                                  join CMtbl
                                  in db.Client_Master
                                  on labortbl.Client_Id equals CMtbl.Client_ID
                                  where (labortbl.IsActive == true && CMtbl.IsActive == true && labortbl.Client_Id == ClientID && labortbl.Labor_Category != "")
                                  select new LabourCategoryList()
                                  {
                                      Labor_Category = labortbl.Labor_Category

                                  }).Distinct().ToList();


                    //LabourList = db.Labor_Details.Where(m => m.IsActive == true && m.Client_Id == ClientID && m.Labor_Category != null).Select(m => new LabourCategoryList()
                    //{

                    //    Labor_Category = m.Labor_Category

                    //}).Distinct().ToList();
                }
                return LabourList;
            }
            catch (Exception ex)
            {
                return null;

            }

        }
        public List<LegalCategoryList> GetLegalCategoryList()
        {
            List<LegalCategoryList> LegalList = new List<LegalCategoryList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    LegalList = (from legaltbl in db.Legal_Details
                                 join CMtbl
                                 in db.Client_Master
                                 on legaltbl.Client_Id equals CMtbl.Client_ID
                                 where (legaltbl.IsActive == true && CMtbl.IsActive == true && legaltbl.Legal_Category != "")
                                 select new LegalCategoryList()
                                 {
                                     Legal_Category = legaltbl.Legal_Category

                                 }).Distinct().ToList();

                    //LegalList = db.Legal_Details.Where(m => m.IsActive == true && m.Legal_Category != null).Select(m => new LegalCategoryList()
                    //{

                    //    Legal_Category = m.Legal_Category

                    //}).Distinct().ToList();
                }
                return LegalList;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        public List<LegalCategoryList> GetMyLegalCategoryList(int ClientID)
        {
            List<LegalCategoryList> LegalList = new List<LegalCategoryList>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    LegalList = (from legaltbl in db.Legal_Details
                                 join CMtbl
                                 in db.Client_Master
                                 on legaltbl.Client_Id equals CMtbl.Client_ID
                                 where (legaltbl.IsActive == true && CMtbl.IsActive == true && legaltbl.Client_Id == ClientID && legaltbl.Legal_Category != "")
                                 select new LegalCategoryList()
                                 {
                                     Legal_Category = legaltbl.Legal_Category

                                 }).Distinct().ToList();

                    //LegalList = db.Legal_Details.Where(m => m.IsActive == true && m.Client_Id == ClientID && m.Legal_Category != null).Select(m => new LegalCategoryList()
                    //{

                    //    Legal_Category = m.Legal_Category

                    //}).Distinct().ToList();
                }


                return LegalList;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public List<AssembliesNameList> GetMyAssembliesNameCategoryList(int ClientID)
        {
            List<AssembliesNameList> AssembliesNameListOBJ = new List<AssembliesNameList>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {

                    AssembliesNameListOBJ = (from AStbl in db.Assemblies_Master
                                             join CMtbl
                                             in db.Client_Master
                                             on AStbl.Client_Id equals CMtbl.Client_ID
                                             where (AStbl.Isactive == true && CMtbl.IsActive == true && AStbl.Client_Id == ClientID && AStbl.Assemblies_Name != "")
                                             select new AssembliesNameList()
                                             {
                                                 Assemblies_Name = AStbl.Assemblies_Name

                                             }).Distinct().ToList();
                    //AssembliesNameListOBJ = db.Assemblies_Master.Where(m => m.Isactive == true && m.Client_Id == ClientID && m.Assemblies_Name != null).Select(m => new AssembliesNameList()
                    //{

                    //    Assemblies_Name = m.Assemblies_Name

                    //}).Distinct().ToList();
                }


                return AssembliesNameListOBJ;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public List<AssembliesCategoryList> GetAssembliesCategoryList(int ClientID)
        {
            List<AssembliesCategoryList> AssembliesNameListOBJ = new List<AssembliesCategoryList>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    AssembliesNameListOBJ = (from AStbl in db.Assemblies_Master
                                             join CMtbl
                                             in db.Client_Master
                                             on AStbl.Client_Id equals CMtbl.Client_ID
                                             where (CMtbl.IsActive == true && AStbl.Client_Id == ClientID && AStbl.Assemblies_Category != "")
                                             select new AssembliesCategoryList()
                                             {
                                                 Assemblies_Category = AStbl.Assemblies_Category

                                             }).Distinct().ToList();

                    //AssembliesNameListOBJ = db.Assemblies_Master.Where(m => m.Isactive == true && m.Client_Id == ClientID && m.Assemblies_Name != null).Select(m => new AssembliesCategoryList()
                    //{

                    //    Assemblies_Category = m.Assemblies_Category

                    //}).Distinct().ToList();
                }


                return AssembliesNameListOBJ;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
        /// <summary>
        /// GetNationWideAssembliesCategoryList
        /// </summary>
        /// <returns></returns>
        public List<AssembliesCategoryList> GetNationWideAssembliesCategoryList()
        {
            List<AssembliesCategoryList> AssembliesNameListOBJ = new List<AssembliesCategoryList>();

            //try
            //{
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    AssembliesNameListOBJ = (from AStbl in db.Super_Admin_Assemblies_Master
                                             where AStbl.Assemblies_Category != ""
                                             select new AssembliesCategoryList()
                                             {
                                                 Assemblies_Category = AStbl.Assemblies_Category

                                             }).Distinct().OrderBy(row => row.Assemblies_Category).ToList();

                    //AssembliesNameListOBJ = db.Assemblies_Master.Where(m => m.Isactive == true && m.Client_Id == ClientID && m.Assemblies_Name != null).Select(m => new AssembliesCategoryList()
                    //{

                    //    Assemblies_Category = m.Assemblies_Category

                    //}).Distinct().ToList();
                }


                return AssembliesNameListOBJ;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }
        /// <summary>
        /// GetDistributorAssembliesCategoryList
        /// </summary>
        /// <returns></returns>
        public List<AssembliesCategoryList> GetDistributorAssembliesCategoryList(int distributorId)
        {
            List<AssembliesCategoryList> AssembliesNameListOBJ = new List<AssembliesCategoryList>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    AssembliesNameListOBJ = (from AStbl in db.Distributor_Assemblies_Master
                                             where AStbl.Assemblies_Category != "" && AStbl.Isactive == true && AStbl.Distributor_Id == distributorId
                                             select new AssembliesCategoryList() { Assemblies_Category = AStbl.Assemblies_Category }).Distinct().ToList();

                    //AssembliesNameListOBJ = db.Assemblies_Master.Where(m => m.Isactive == true && m.Client_Id == ClientID && m.Assemblies_Name != null).Select(m => new AssembliesCategoryList()
                    //{

                    //    Assemblies_Category = m.Assemblies_Category

                    //}).Distinct().ToList();
                }


                return AssembliesNameListOBJ;
            }
            catch (Exception ex)
            {
                return null;

            }
        }
    }
}
