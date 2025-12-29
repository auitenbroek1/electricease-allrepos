using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase.Data;
using ElectricEase.Models;
using ElectricEase.Data.DataBase;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Data;
using System.Configuration;
using log4net;
using System.Data.Entity.Infrastructure;

namespace ElectricEase.Data.PartsMaster
{
    public class PartsMasterData
    {
        LogsMaster logs = new LogsMaster();

        public List<DistributorPartsList> GetDistributorPartsByDistID(int distributorID)
        {
            List<DistributorPartsList> partsList = new List<DistributorPartsList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    var objectContext = (db as IObjectContextAdapter).ObjectContext;
                    objectContext.CommandTimeout = 300; // 5 minutes

                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter() { ParameterName = "Distributor_ID", Value = distributorID };
                    partsList = db.Database.SqlQuery<DistributorPartsList>("exec EE_MyDistributorPartsDetailList @Distributor_ID", parameters).ToList();
                    return partsList;
                }
                
            }
            catch (Exception ex)
            {
                return partsList;
            }
        }

        public ServiceResultList<Parts_DetailsInfoList> GetPartsDetalisList(string PartCatgory = "")
        {
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Parts_DetailsInfoList> result = new List<Parts_DetailsInfoList>();
                    result = db.Database.SqlQuery<Parts_DetailsInfoList>("exec EE_GetAllPartsDetailList").ToList();

                    if (result != null)
                    {
                        if (PartCatgory == "" || PartCatgory == "ALL PartsCategory")
                        {
                            response.ListData = result;
                            response.ResultCode = 1;
                            response.Message = "Success";
                            return response;
                        }
                        else
                        {
                            List<Parts_DetailsInfoList> selectedList = new List<Parts_DetailsInfoList>();
                            selectedList = result.Where(m => m.Part_Category == PartCatgory).Select(m => new Parts_DetailsInfoList()
                            {
                                Client_Company = m.Client_Company,
                                Client_ID = m.Client_ID,
                                Part_Category = m.Part_Category,
                                Part_Number = m.Part_Number,
                                Cost = m.Cost,
                                Resale_Cost = m.Resale_Cost,
                                Purchased_From = m.Purchased_From,
                                Description = m.Description,
                                UOM = m.UOM,
                                Type = m.Type
                            }).Distinct().ToList();
                            response.ListData = selectedList;
                            response.ResultCode = 1;
                            response.Message = "Success";
                            return response;
                        }
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "No “Records” found!";
                        return response;

                    }
                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        public List<Parts_DetailsInfoList> GetPartsListByClientID(int ClientID)
        {
            List<Parts_DetailsInfoList> partsList = new List<Parts_DetailsInfoList>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    SqlParameter[] parameters = new SqlParameter[1];
                    parameters[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    partsList = db.Database.SqlQuery<Parts_DetailsInfoList>("exec EE_GetPartsListByClientID  @ClientID", parameters).ToList();
                    return partsList;
                }
            }
            catch (Exception ex)
            {
                return partsList;
            }
        }

        /// <summary>
        /// GetPartsDetalis
        /// </summary>
        /// <param name="PartCatgory"></param>
        /// <returns></returns>
        public List<Parts_DetailsInfoList> GetPartsDetalis(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, string extra_search, int? Client_ID)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Parts_Details> res = new List<Parts_Details>();
                    var searchTerms = (searchBy ?? "").Split(' ').ToList().ConvertAll(x => x.ToLower());
                    if (String.IsNullOrWhiteSpace(searchBy) == false)
                    {
                        res = db.Parts_Details.Where(a => a.IsActive == true && a.Client_Id == (Client_ID ?? a.Client_Id) && a.Part_Category != null && a.Part_Category == (extra_search ?? a.Part_Category) &&
                       (searchTerms.Any(srch => a.Part_Category != null && a.Part_Category.ToLower().Contains(srch))
                       || searchTerms.Any(srch => a.Description != null && a.Description.ToLower().Contains(srch))
                       || searchTerms.Any(srch => a.Part_Number != null && a.Part_Number.ToLower().Contains(srch))
                       || searchTerms.Any(srch => a.Purchased_From != null && a.Purchased_From.ToLower().Contains(srch))
                       || searchTerms.Any(srch => a.UOM != null && a.UOM.ToLower().Contains(srch))
                       || searchTerms.Any(srch => a.Type != null && a.Type.ToLower().Contains(srch))
                       // || searchTerms.Any(srch => a.Cost.ToString().Contains(srch))
                       ))
                     .OrderBy(sortBy, sortDir) // have to give a default order when skipping .. so use the PK
                              .Skip(skip)
                              .Take(take)
                              .ToList();
                        filteredResultsCount = db.Parts_Details.Where(a => a.IsActive == true && a.Client_Id == (Client_ID ?? a.Client_Id) && a.Part_Category != null && a.Part_Category == (extra_search ?? a.Part_Category) &&
                     (searchTerms.Any(srch => a.Part_Category != null && a.Part_Category.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Description != null && a.Description.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Part_Number != null && a.Part_Number.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Purchased_From != null && a.Purchased_From.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.UOM != null && a.UOM.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Type != null && a.Type.ToLower().Contains(srch))
                     // || searchTerms.Any(srch => a.Cost.ToString().Contains(srch))
                     )).Count();
                    }
                    else
                    {
                        res = db.Parts_Details.Where(a => a.IsActive == true && a.Client_Id == (Client_ID ?? a.Client_Id) && a.Part_Category != null && a.Part_Category == (extra_search ?? a.Part_Category))
                                         .OrderBy(sortBy, sortDir) // have to give a default order when skipping .. so use the PK
                                                  .Skip(skip)
                                                  .Take(take)
                                                  .ToList();
                        filteredResultsCount = db.Parts_Details.Where(a => a.IsActive == true && a.Client_Id == (Client_ID ?? a.Client_Id) && a.Part_Category != null && a.Part_Category == (extra_search ?? a.Part_Category)).Count();
                    }

                    // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
                    //filteredResultsCount = db.Super_Admin_Parts_Details.Where(whereClause).Where(a => a.IsActive == true).Count();
                    totalResultsCount = db.Parts_Details.Where(a => a.IsActive == true && a.Part_Category != null && a.Client_Id == (Client_ID ?? a.Client_Id)).Count();
                    int i = 1;
                    var result = new List<Parts_DetailsInfoList>(res.Count);
                    foreach (var m in res)
                    {
                        result.Add(new Parts_DetailsInfoList
                        {
                            Client_ID = m.Client_Id,
                            Part_Category = m.Part_Category,
                            Part_Number = m.Part_Number,
                            Cost = m.Cost,
                            Resale_Cost = m.Resale_Cost ?? 0,
                            Purchased_From = m.Purchased_From ?? "",
                            Description = m.Description ?? "",
                            UOM = m.UOM ?? "",
                            Type = m.Type,
                            ID = i
                        });
                        i++;
                    };
                    return result;
                }

            }
            catch (Exception ex)
            {
                filteredResultsCount = 0;
                totalResultsCount = 0;
                return new List<Parts_DetailsInfoList>();
            }
        }


        /// <summary>
        /// GetDistributorPartsData
        /// </summary>
        /// <param name="PartCatgory"></param>
        /// <returns></returns>
        public List<Parts_DetailsInfoList> GetDistributorPartsData(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, string extra_search)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Distributor_Parts_Details> res = new List<Distributor_Parts_Details>();
                    var searchTerms = (searchBy ?? "").Split(' ').ToList().ConvertAll(x => x.ToLower());
                    if (String.IsNullOrWhiteSpace(searchBy) == false)
                    {
                        res = db.Distributor_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category) &&
                   (searchTerms.Any(srch => a.Part_Category != null && a.Part_Category.ToLower().Contains(srch))
                    || searchTerms.Any(srch => a.Distributor_Master != null && a.Distributor_Master.Company.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Description != null && a.Description.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Part_Number != null && a.Part_Number.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Purchased_From != null && a.Purchased_From.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.UOM != null && a.UOM.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Type != null && a.Type.ToLower().Contains(srch))
                   // || searchTerms.Any(srch => a.Cost.ToString().Contains(srch))
                   ))
                 .OrderBy(sortBy, sortDir) // have to give a default order when skipping .. so use the PK
                          .Skip(skip)
                          .Take(take)
                          .ToList();
                        filteredResultsCount = db.Distributor_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category) &&
                     (searchTerms.Any(srch => a.Part_Category != null && a.Part_Category.ToLower().Contains(srch))
                     || searchTerms.Any(srch => a.Distributor_Master != null && a.Distributor_Master.Company.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Description != null && a.Description.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Part_Number != null && a.Part_Number.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Purchased_From != null && a.Purchased_From.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.UOM != null && a.UOM.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Type != null && a.Type.ToLower().Contains(srch))
                     // || searchTerms.Any(srch => a.Cost.ToString().Contains(srch))
                     )).Count();
                    }
                    else
                    {
                        res = db.Distributor_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category))
                                         .OrderBy(sortBy, sortDir) // have to give a default order when skipping .. so use the PK
                                                  .Skip(skip)
                                                  .Take(take)
                                                  .ToList();
                        filteredResultsCount = db.Distributor_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category)).Count();
                    }

                    // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
                    //filteredResultsCount = db.Super_Admin_Parts_Details.Where(whereClause).Where(a => a.IsActive == true).Count();
                    totalResultsCount = db.Distributor_Parts_Details.Where(a => a.IsActive == true).Count();
                    int i = 1;
                    var result = new List<Parts_DetailsInfoList>(res.Count);
                    foreach (var m in res)
                    {
                        result.Add(new Parts_DetailsInfoList
                        {
                            Company = m.Distributor_Master.Company,
                            Client_ID = m.Client_Id,
                            Part_Category = m.Part_Category,
                            Part_Number = m.Part_Number,
                            Cost = m.Cost ?? 0,
                            Resale_Cost = m.Resale_Cost ?? 0,
                            Purchased_From = m.Purchased_From ?? "",
                            Description = m.Description ?? "",
                            UOM = m.UOM ?? "",
                            Type = m.Type ?? "Distributor",
                            Distributor_ID = m.Distributor_ID,
                            ID = i
                        });
                        i++;
                    };
                    return result;
                }

            }
            catch (Exception ex)
            {
                filteredResultsCount = 0;
                totalResultsCount = 0;
                return new List<Parts_DetailsInfoList>();
            }
        }

        /// <summary>
        /// GetDistributorPartsData
        /// </summary>
        /// <param name="PartCatgory"></param>
        /// <returns></returns>
        public List<Parts_DetailsInfoList> GetDistributorPartsByDistId(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, string extra_search, int DistributorId)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Distributor_Parts_Details> res = new List<Distributor_Parts_Details>();
                    var searchTerms = (searchBy ?? "").Split(' ').ToList().ConvertAll(x => x.ToLower());
                    if (String.IsNullOrWhiteSpace(searchBy) == false)
                    {
                        res = db.Distributor_Parts_Details.Where(a => a.IsActive == true && a.Distributor_ID == DistributorId && a.Part_Category == (extra_search ?? a.Part_Category) &&
                   (searchTerms.Any(srch => a.Part_Category != null && a.Part_Category.ToLower().Contains(srch))
                    || searchTerms.Any(srch => a.Distributor_Master != null && a.Distributor_Master.Company.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Description != null && a.Description.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Part_Number != null && a.Part_Number.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Purchased_From != null && a.Purchased_From.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.UOM != null && a.UOM.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Type != null && a.Type.ToLower().Contains(srch))
                   // || searchTerms.Any(srch => a.Cost.ToString().Contains(srch))
                   ))
                 .OrderBy(sortBy, sortDir) // have to give a default order when skipping .. so use the PK
                          .Skip(skip)
                          .Take(take)
                          .ToList();
                        filteredResultsCount = db.Distributor_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category) &&
                     (searchTerms.Any(srch => a.Part_Category != null && a.Part_Category.ToLower().Contains(srch))
                     || searchTerms.Any(srch => a.Distributor_Master != null && a.Distributor_Master.Company.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Description != null && a.Description.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Part_Number != null && a.Part_Number.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Purchased_From != null && a.Purchased_From.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.UOM != null && a.UOM.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Type != null && a.Type.ToLower().Contains(srch))
                     // || searchTerms.Any(srch => a.Cost.ToString().Contains(srch))
                     )).Count();
                    }
                    else
                    {
                        res = db.Distributor_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category) && a.Distributor_ID == DistributorId)
                                         .OrderBy(sortBy, sortDir) // have to give a default order when skipping .. so use the PK
                                                  .Skip(skip)
                                                  .Take(take)
                                                  .ToList();
                        filteredResultsCount = db.Distributor_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category) && a.Distributor_ID == DistributorId).Count();
                    }

                    // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
                    //filteredResultsCount = db.Super_Admin_Parts_Details.Where(whereClause).Where(a => a.IsActive == true).Count();
                    totalResultsCount = db.Distributor_Parts_Details.Where(a => a.IsActive == true && a.Distributor_ID == DistributorId).Count();
                    int i = 1;
                    var result = new List<Parts_DetailsInfoList>(res.Count);
                    foreach (var m in res)
                    {
                        result.Add(new Parts_DetailsInfoList
                        {
                            Company = m.Distributor_Master.Company,
                            Client_ID = m.Client_Id,
                            Part_Category = m.Part_Category,
                            Part_Number = m.Part_Number,
                            Cost = m.Cost ?? 0,
                            Resale_Cost = m.Resale_Cost ?? 0,
                            Purchased_From = m.Purchased_From ?? "",
                            Description = m.Description ?? "",
                            UOM = m.UOM ?? "",
                            Type = m.Type ?? "Distributor",
                            Distributor_ID = m.Distributor_ID,
                            ID = i
                        });
                        i++;
                    };
                    return result;
                }

            }
            catch (Exception ex)
            {
                filteredResultsCount = 0;
                totalResultsCount = 0;
                return new List<Parts_DetailsInfoList>();
            }
        }

        /// <summary>
        /// GetPartsDetalisList
        /// </summary>
        /// <param name="PartCatgory"></param>
        /// <returns></returns>
        public ServiceResultList<Parts_DetailsInfoList> GetDistributorPartsDetalisList(string PartCatgory = "")
        {
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Parts_DetailsInfoList> result = new List<Parts_DetailsInfoList>();
                    result = db.Database.SqlQuery<Parts_DetailsInfoList>("exec EE_GetAllDistributorPartsDetailList").ToList();

                    if (result != null)
                    {
                        if (PartCatgory == "" || PartCatgory == "ALL PartsCategory")
                        {
                            response.ListData = result;
                            response.ResultCode = 1;
                            response.Message = "Success";
                            return response;
                        }
                        else
                        {
                            List<Parts_DetailsInfoList> selectedList = new List<Parts_DetailsInfoList>();
                            selectedList = result.Where(m => m.Part_Category == PartCatgory).Select(m => new Parts_DetailsInfoList()
                            {
                                // Client_Company = m.Client_Company,
                                Company = m.Company,
                                //Client_ID = m.Client_ID,dis   
                                Distributor_ID = m.Distributor_ID,
                                Part_Category = m.Part_Category,
                                Part_Number = m.Part_Number,
                                Cost = m.Cost,
                                Resale_Cost = m.Resale_Cost,
                                Purchased_From = m.Purchased_From,
                                Description = m.Description,
                                UOM = m.UOM,
                                Type = m.Type
                            }).Distinct().ToList();
                            response.ListData = selectedList;
                            response.ResultCode = 1;
                            response.Message = "Success";
                            return response;
                        }
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "No “Records” found!";
                        return response;

                    }
                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }
        private Expression<Func<Super_Admin_Parts_Details, bool>> BuildDynamicWhereClause(ElectricEaseEntitiesContext entities, string searchValue)
        {
            // simple method to dynamically plugin a where clause
            var predicate = PredicateBuilder.True<Super_Admin_Parts_Details>(); // true -where(true) return all
            if (String.IsNullOrWhiteSpace(searchValue) == false)
            {
                // as we only have 2 cols allow the user type in name 'firstname lastname' then use the list to search the first and last name of dbase
                var searchTerms = searchValue.Split(' ').ToList().ConvertAll(x => x.ToLower());
                predicate = predicate.Or(s => searchTerms.Any(srch => s.Part_Category != null && s.Part_Category.ToLower().Contains(srch)));
                //predicate = predicate.Or(s => searchTerms.Any(srch => s.Description != null && s.Description.ToLower().Contains(srch)));
                //predicate = predicate.Or(s => searchTerms.Any(srch => s.Part_Number != null && s.Part_Number.ToLower().Contains(srch)));
                ////predicate = predicate.Or(s => searchTerms.Any(srch => s.Cost.Contains(srch)));
                //predicate = predicate.Or(s => searchTerms.Any(srch => s.Purchased_From != null && s.Purchased_From.ToLower().Contains(srch)));
                //predicate = predicate.Or(s => searchTerms.Any(srch => s.UOM != null && s.UOM.ToLower().Contains(srch)));

            }
            return predicate;
        }
        /// <summary>
        /// GetNationalWidePartsData
        /// </summary>
        /// <param name="PartCatgory"></param>
        /// <returns></returns>
        public List<Super_Admin_Parts_Details> GetNationalWidePartsData(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount, string extra_search)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Super_Admin_Parts_Details> result = new List<Super_Admin_Parts_Details>();
                    // the example datatable used is not supporting multi column ordering
                    // so we only need get the column order from the first column passed to us.        
                    // var whereClause = BuildDynamicWhereClause(db, searchBy);
                    //if (String.IsNullOrEmpty(searchBy))
                    //{
                    //    // if we have an empty search then just order the results by Id ascending
                    //    sortBy = "Created_Date";
                    //    sortDir = true;
                    //}
                    var searchTerms = (searchBy ?? "").Split(' ').ToList().ConvertAll(x => x.ToLower());
                    if (String.IsNullOrWhiteSpace(searchBy) == false)
                    {
                        filteredResultsCount = db.Super_Admin_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category) &&
                     (searchTerms.Any(srch => a.Part_Category != null && a.Part_Category.ToLower().Contains(srch))
                     || searchTerms.Any(srch => a.Description != null && a.Description.ToLower().Contains(srch))
                     || searchTerms.Any(srch => a.Part_Number != null && a.Part_Number.ToLower().Contains(srch))
                     || searchTerms.Any(srch => a.Purchased_From != null && a.Purchased_From.ToLower().Contains(srch))
                     || searchTerms.Any(srch => a.UOM != null && a.UOM.ToLower().Contains(srch))
                     // || searchTerms.Any(srch => a.Cost.ToString().Contains(srch))
                     )).Count();
                        result = db.Super_Admin_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category) &&
                   (searchTerms.Any(srch => a.Part_Category != null && a.Part_Category.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Description != null && a.Description.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Part_Number != null && a.Part_Number.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Purchased_From != null && a.Purchased_From.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.UOM != null && a.UOM.ToLower().Contains(srch))
                   // || searchTerms.Any(srch => a.Cost.ToString().Contains(srch))
                   ))
                 .OrderBy(sortBy, sortDir) // have to give a default order when skipping .. so use the PK
                          .Skip(skip)
                          .Take(take)
                          .ToList();

                    }
                    else
                    {
                        filteredResultsCount = db.Super_Admin_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category)).Count();
                        result = db.Super_Admin_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category))
                  .OrderBy(sortBy, sortDir) // have to give a default order when skipping .. so use the PK
                           .Skip(skip)
                           .Take(take)
                           .ToList();

                    }

                    // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
                    //filteredResultsCount = db.Super_Admin_Parts_Details.Where(whereClause).Where(a => a.IsActive == true).Count();
                    totalResultsCount = db.Super_Admin_Parts_Details.Where(a => a.IsActive == true).Count();

                    return result;
                }

            }
            catch (Exception ex)
            {
                filteredResultsCount = 0;
                totalResultsCount = 0;
                return new List<Super_Admin_Parts_Details>();
            }
        }

        /// <summary>
        /// SearchNationalWidePartsData
        /// </summary>
        /// <param name="PartCatgory"></param>
        /// <returns></returns>
        public List<Super_Admin_Parts_Details> SearchNationalWidePartsData(string searchBy, string extra_search)
        {
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Super_Admin_Parts_Details> result = new List<Super_Admin_Parts_Details>();
                    var searchTerms = (searchBy ?? "").Split(' ').ToList().ConvertAll(x => x.ToLower());
                    if (String.IsNullOrWhiteSpace(searchBy) == false)
                    {
                        result = db.Super_Admin_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category) &&
                   (searchTerms.Any(srch => a.Part_Category != null && a.Part_Category.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Description != null && a.Description.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Part_Number != null && a.Part_Number.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.Purchased_From != null && a.Purchased_From.ToLower().Contains(srch))
                   || searchTerms.Any(srch => a.UOM != null && a.UOM.ToLower().Contains(srch))
                   // || searchTerms.Any(srch => a.Cost.ToString().Contains(srch))
                   )).ToList();

                    }
                    else
                    {

                        result = db.Super_Admin_Parts_Details.Where(a => a.IsActive == true && a.Part_Category == (extra_search ?? a.Part_Category)).ToList();

                    }

                    return result;
                }

            }
            catch (Exception ex)
            {
                return new List<Super_Admin_Parts_Details>();
            }
        }

        /// <summary>
        /// GetNationalWidePartsDetalisList
        /// </summary>
        /// <param name="PartCatgory"></param>
        /// <returns></returns>
        public ServiceResultList<Parts_DetailsInfoList> GetNationalWidePartsDetalisList(string PartCatgory = "")
        {
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Parts_DetailsInfoList> result = new List<Parts_DetailsInfoList>();
                    result = db.Database.SqlQuery<Parts_DetailsInfoList>("exec EE_GetNationWideAllPartsDetailList").ToList();

                    if (result != null)
                    {
                        if (PartCatgory == "" || PartCatgory == "ALL PartsCategory")
                        {
                            response.ListData = result.ToList();
                            response.ResultCode = 1;
                            response.Message = "Success";
                            return response;
                        }
                        else
                        {
                            List<Parts_DetailsInfoList> selectedList = new List<Parts_DetailsInfoList>();
                            selectedList = result.Where(m => m.Part_Category == PartCatgory).Select(m => new Parts_DetailsInfoList()
                            {
                                Client_Company = m.Client_Company,
                                Client_ID = m.Client_ID,
                                Part_Category = m.Part_Category,
                                Part_Number = m.Part_Number,
                                Cost = m.Cost,
                                Resale_Cost = m.Resale_Cost,
                                Purchased_From = m.Purchased_From,
                                Description = m.Description,
                                UOM = m.UOM
                            }).OrderBy(x => x.Part_Category).Distinct().ToList();
                            response.ListData = selectedList;
                            response.ResultCode = 1;
                            response.Message = "Success";
                            return response;
                        }
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "No “Records” found!";
                        return response;

                    }
                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        //public ServiceResultList<Parts_DetailsInfoList> GetPartsDetalisList()
        //{
        //    ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
        //    try
        //    {

        //        using (ElectricEaseEntitiesContext db=new ElectricEaseEntitiesContext())
        //        {
        //            List<Parts_DetailsInfoList> result = new List<Parts_DetailsInfoList>();
        //            result = db.Database.SqlQuery<Parts_DetailsInfoList>("exec EE_GetAllPartsDetailList").ToList();

        //            if (result != null)
        //            {
        //                response.ListData = result;
        //                response.ResultCode = 1;
        //                response.Message = "Success";
        //                return response;
        //            }
        //            else
        //            {
        //                response.ResultCode = -1;
        //                response.Message = "No Records Found";
        //                return response;

        //            }
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        response.ResultCode = -1;
        //        response.Message = ex.Message;
        //        return response;
        //    }
        //}
        public ServiceResultList<Parts_DetailsInfoList> GetMyPartsList(int ClientID, string PartCatgory = "")
        {
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Parts_DetailsInfoList> result = new List<Parts_DetailsInfoList>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    result = db.Database.SqlQuery<Parts_DetailsInfoList>("exec EE_MyPartsDetailList @ClientID", para).ToList();

                    if (result != null)
                    {
                        if (PartCatgory == "" || PartCatgory == "ALL PartsCategory")
                        {
                            response.ListData = result;
                            response.ResultCode = 1;
                            response.Message = "Success";
                            return response;
                        }
                        else
                        {
                            List<Parts_DetailsInfoList> selectedList = new List<Parts_DetailsInfoList>();
                            selectedList = result.Where(m => m.Part_Category == PartCatgory).Select(m => new Parts_DetailsInfoList()
                            {
                                Part_Category = m.Part_Category,
                                Client_ID = m.Client_ID,
                                Part_Number = m.Part_Number,
                                Cost = m.Cost,
                                Resale_Cost = m.Resale_Cost,
                                Description = m.Description,
                                Type = m.Type,
                                Purchased_From = m.Purchased_From,
                                UOM = m.UOM,
                                LaborUnit = m.LaborUnit
                            }).Distinct().ToList();
                            response.ListData = selectedList;
                            response.ResultCode = 1;
                            response.Message = "Success";
                            return response;
                        }

                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "No “Records” found!";
                        return response;

                    }
                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }


        public ServiceResultList<Parts_DetailsInfoList> GetSelectedDistributorPartsDetalisList(string PartCatgory = "", int Did = 0)
        {
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Parts_DetailsInfoList> result = new List<Parts_DetailsInfoList>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "Distributor_ID", Value = Did };
                    result = db.Database.SqlQuery<Parts_DetailsInfoList>("exec EE_MyDistributorPartsDetailList @Distributor_ID", para).ToList();

                    if (result != null)
                    {
                        if (PartCatgory == "" || PartCatgory == "ALL PartsCategory")
                        {
                            response.ListData = result.OrderBy(x => x.Part_Category).ToList(); ;
                            response.ResultCode = 1;
                            response.Message = "Success";
                            return response;
                        }
                        else
                        {
                            List<Parts_DetailsInfoList> selectedList = new List<Parts_DetailsInfoList>();
                            selectedList = result.Where(m => m.Part_Category == PartCatgory).Select(m => new Parts_DetailsInfoList()
                            {
                                Part_Category = m.Part_Category,
                                Client_ID = m.Client_ID,
                                Part_Number = m.Part_Number,
                                Cost = m.Cost,
                                Resale_Cost = m.Resale_Cost,
                                Description = m.Description,
                                Purchased_From = m.Purchased_From,
                                UOM = m.UOM
                            }).Distinct().ToList();
                            response.ListData = selectedList;
                            response.ResultCode = 1;
                            response.Message = "Success";
                            return response;
                        }

                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "No “Records” found!";
                        return response;

                    }
                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }
        //To Add Parts Details to  parts details Table in Database
        public ServiceResult<int> AddNewPartsDetail(Parts_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            List<string> assemblyToProcess = new List<string>();
            StringBuilder assembliesList = new StringBuilder();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {

                    Parts_Details PDTblObj = new Parts_Details();
                    string stat = "";
                    bool isupdate = true;
                    //validate User ID 
                    PDTblObj = db.Parts_Details.Where(x => x.Part_Number == Model.Part_Number && x.Client_Id == Model.Client_ID && x.IsActive == true).FirstOrDefault();
                    if (PDTblObj != null)
                    {
                        isupdate = true;
                    }
                    else
                    {
                        PDTblObj = new Parts_Details();
                        isupdate = false;
                    }
                    if (Model.Part_Category == "1" && Model.OtherPart_Category != null)
                    {
                        Model.Part_Category = Model.OtherPart_Category;
                    }

                    PDTblObj.Client_Id = Model.Client_ID;

                    PDTblObj.Part_Category = Model.Part_Category;
                    PDTblObj.Description = Model.Description;
                    PDTblObj.Client_Description = Model.Client_Description;
                    PDTblObj.Cost = Model.Cost;
                    PDTblObj.Resale_Cost = Model.Resale_Cost;
                    PDTblObj.Purchased_From = Model.Purchased_From;
                    PDTblObj.UOM = Model.UOM;
                    PDTblObj.LaborUnit = Model.LaborUnit;
                    logs.WriteLog("Labor Unit - " + PDTblObj.LaborUnit, PDTblObj.Client_Id, PDTblObj.Created_By);
                    PDTblObj.IsActive = true;
                    if (!isupdate)
                    {
                        PDTblObj.Created_By = Model.Created_By;
                        PDTblObj.Created_Date = DateTime.Now;
                        PDTblObj.Updated_By = Model.Created_By;
                        PDTblObj.Updated_Date = DateTime.Now;
                        PDTblObj.Part_Number = Model.Part_Number.Trim();
                        PDTblObj.IsActive = true;
                        db.Parts_Details.Add(PDTblObj);
                        stat = "“New Parts” has been added successfully.";
                        logs.WriteLog("Adding the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Created_By);
                    }
                    else
                    {
                        PDTblObj.Updated_By = Model.Created_By;
                        PDTblObj.Updated_Date = DateTime.Now;
                        stat = "“Parts” are updated successfully.";
                        logs.WriteLog("Updating the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Updated_By);

                        //To update the Client Assembly Parts when the Parts get updated
                        List<Assemblies_Parts> ClientAssemblyParts = db.Assemblies_Parts.Where(x => x.Client_ID == Model.Client_ID && x.Part_Number == Model.Part_Number && x.IsActive == true).ToList();

                        foreach (Assemblies_Parts part in ClientAssemblyParts)
                        {
                            part.Part_Category = Model.Part_Category;
                            part.Part_Cost = Model.Cost;
                            part.Part_Resale = Model.Resale_Cost;
                            part.LaborUnit = Model.LaborUnit;

                            //To get the assemblies to be processed
                            assemblyToProcess.Add(part.Assemblies_Name);
                        }

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

                    }
                    int resultcount = db.SaveChanges();

                    string connStr = ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        using (SqlCommand cmd = new SqlCommand("EE_RecalcClientAssembly", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@AssemblyList", SqlDbType.NVarChar).Value = assembliesList.ToString();
                            cmd.Parameters.Add("@ClientID", SqlDbType.Int).Value = Model.Client_ID;
                            cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = PDTblObj.Updated_By;

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    //db.Database.SqlQuery<int>("exec EE_RecalcClientAssembly @AssemblyList @ClientID, @UpdatedBy",
                    //        new SqlParameter("AssemblyList", assembliesList.ToString()),
                    //        new SqlParameter("ClientID", Model.Client_ID),
                    //        new SqlParameter("UpdatedBy", PDTblObj.Updated_By));

                    if (resultcount > 0)
                    {
                        if (!isupdate)
                        {
                            logs.WriteLog("Successfully Added the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Created_By);
                        }
                        else
                            logs.WriteLog("Successfully Updated the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Updated_By);

                        response.ResultCode = 1;
                        response.Message = stat;
                        return response;
                    }
                    else
                    {
                        // response.ResultCode = 1;
                        response.Message = "Error while “Parts Registering”!";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While adding Parts - " + Model.Part_Number + "[Error Msg- " + ex.Message + " ]", Model.Client_ID, Model.Created_By);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }
        /// <summary>
        /// AddNewDistributorPartsDetail    
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ServiceResult<int> AddNewDistributorPartsDetail(Parts_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            List<int> assemblyToProcess = new List<int>();
            StringBuilder assembliesList = new StringBuilder();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {

                    Distributor_Parts_Details PDTblObj = new Distributor_Parts_Details();
                    string stat = "";
                    bool isupdate = true;
                    //validate User ID 
                    PDTblObj = db.Distributor_Parts_Details.Where(x => x.Part_Number == Model.Part_Number && x.Distributor_ID == Model.DistributorID && x.IsActive == true).FirstOrDefault();
                    if (PDTblObj != null)
                    {
                        isupdate = true;
                    }
                    else
                    {
                        PDTblObj = new Distributor_Parts_Details();
                        isupdate = false;
                    }
                    if (Model.Part_Category == "1" && Model.OtherPart_Category != null)
                    {
                        Model.Part_Category = Model.OtherPart_Category;
                    }

                    PDTblObj.Client_Id = 1;

                    PDTblObj.Part_Category = Model.Part_Category;
                    PDTblObj.Distributor_ID = Model.DistributorID;
                    PDTblObj.Description = Model.Description;
                    PDTblObj.Client_Description = Model.Client_Description;
                    PDTblObj.Cost = Model.Cost;
                    PDTblObj.Resale_Cost = Model.Resale_Cost;
                    PDTblObj.Purchased_From = Model.Purchased_From;
                    PDTblObj.UOM = Model.UOM;
                    PDTblObj.LaborUnit = Model.LaborUnit;
                    PDTblObj.IsActive = true;
                    if (!isupdate)
                    {
                        PDTblObj.Created_By = Model.Created_By;
                        PDTblObj.Created_Date = DateTime.Now;
                        PDTblObj.Updated_By = Model.Created_By;
                        PDTblObj.Updated_Date = DateTime.Now;
                        PDTblObj.Part_Number = Model.Part_Number.Trim();
                        PDTblObj.IsActive = true;
                        db.Distributor_Parts_Details.Add(PDTblObj);
                        stat = "“New Parts” has been added successfully.";
                        logs.WriteLog("Adding the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Created_By);
                    }
                    else
                    {
                        PDTblObj.Updated_By = Model.Created_By;
                        PDTblObj.Updated_Date = DateTime.Now;
                        stat = "“Parts” are updated successfully.";
                        logs.WriteLog("Updating the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Updated_By);

                        //To update the Distributor Assembly Parts when the Parts get updated
                        List<Distributor_Assemblies_Parts> DistAssemblyParts = db.Distributor_Assemblies_Parts.Where(x => x.Part_Number == Model.Part_Number && x.IsActive == true).ToList();

                        foreach (Distributor_Assemblies_Parts part in DistAssemblyParts)
                        {
                            part.Part_Category = Model.Part_Category;
                            part.Part_Cost = Model.Cost;
                            part.Part_Resale = Model.Resale_Cost;
                            part.LaborUnit = Model.LaborUnit;

                            //To get the assemblies to be processed
                            assemblyToProcess.Add(part.AssemblyID);
                        }

                        //To remove the duplicate assembly Ids
                        assemblyToProcess = assemblyToProcess.Distinct().ToList();
                        foreach (int assembly in assemblyToProcess)
                        {
                            assembliesList.Append(assembly);
                            assembliesList.Append(',');
                        }

                        //To remove last comma in the list of processed assemblies
                        if (assembliesList.Length > 0)
                        {
                            assembliesList.Remove((assembliesList.Length - 1), 1);
                        }
                    }
                    int resultcount = db.SaveChanges();

                    //To update the labor unit on Client Assembly Master
                    string connStr = ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        using (SqlCommand cmd = new SqlCommand("EE_RecalcDistributorAssembly", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@AssemblyList", SqlDbType.NVarChar).Value = assembliesList.ToString();
                            cmd.Parameters.Add("@DistributorID", SqlDbType.Int).Value = Model.DistributorID;
                            cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = Model.Created_By;

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    if (resultcount > 0)
                    {
                        if (!isupdate)
                        {
                            logs.WriteLog("Successfully Added the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Created_By);
                        }
                        else
                            logs.WriteLog("Successfully Updated the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Updated_By);

                        response.ResultCode = 1;
                        response.Message = stat;
                        return response;
                    }
                    else
                    {
                        // response.ResultCode = 1;
                        response.Message = "Error while “Parts Registering”!";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While adding Parts - " + Model.Part_Number + "[Error Msg- " + ex.Message + " ]", Model.Client_ID, Model.Created_By);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }


        /// <summary>
        /// AddNationwidePartsDetail
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public ServiceResult<int> AddNationwidePartsDetail(Parts_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            List<int> assemblyToProcess = new List<int>();
            StringBuilder assembliesList = new StringBuilder();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Super_Admin_Parts_Details PDTblObj = new Super_Admin_Parts_Details();
                    string stat = "";
                    bool isupdate = true;
                    //validate User ID 
                    PDTblObj = db.Super_Admin_Parts_Details.Where(x => x.Part_Number == Model.Part_Number && x.IsActive == true).FirstOrDefault();
                    if (PDTblObj != null)
                    {
                        isupdate = true;
                    }
                    else
                    {
                        PDTblObj = new Super_Admin_Parts_Details();
                        isupdate = false;
                    }
                    if (Model.Part_Category == "1" && Model.OtherPart_Category != null)
                    {
                        Model.Part_Category = Model.OtherPart_Category;
                    }
                    PDTblObj.Part_Category = Model.Part_Category;
                    PDTblObj.Description = Model.Description;
                    PDTblObj.Client_Description = Model.Client_Description;
                    PDTblObj.Cost = Model.Cost;
                    PDTblObj.Resale_Cost = Model.Resale_Cost;
                    PDTblObj.Purchased_From = Model.Purchased_From;
                    PDTblObj.UOM = Model.UOM;
                    PDTblObj.LaborUnit = Model.LaborUnit;
                    PDTblObj.IsActive = true;
                    if (!isupdate)
                    {
                        PDTblObj.Created_By = Model.Created_By;
                        PDTblObj.Created_Date = DateTime.Now;
                        PDTblObj.Updated_By = Model.Created_By;
                        PDTblObj.Updated_Date = DateTime.Now;
                        PDTblObj.Part_Number = Model.Part_Number.Trim();
                        PDTblObj.IsActive = true;
                        db.Super_Admin_Parts_Details.Add(PDTblObj);
                        stat = "“New Parts” has been added successfully.";
                        logs.WriteLog("Adding the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Created_By);
                    }
                    else
                    {
                        PDTblObj.Updated_By = Model.Created_By;
                        PDTblObj.Updated_Date = DateTime.Now;
                        stat = "“Parts” are updated successfully.";
                        logs.WriteLog("Updating the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Updated_By);

                        //To update the Nationalwide Assembly Parts when the Parts get updated
                        List<Super_Admin_Assemblies_Parts> NatAssemblyParts = db.Super_Admin_Assemblies_Parts.Where(x => x.Part_Number == Model.Part_Number && x.IsActive == true).ToList();

                        foreach (Super_Admin_Assemblies_Parts part in NatAssemblyParts)
                        {
                            part.Part_Category = Model.Part_Category;
                            part.Part_Cost = Model.Cost;
                            part.Part_Resale = Model.Resale_Cost;
                            part.LaborUnit = Model.LaborUnit;

                            //To get the assemblies to be processed
                            assemblyToProcess.Add(part.Assemblies_Id);
                        }

                        //To remove the duplicate assembly Ids
                        assemblyToProcess = assemblyToProcess.Distinct().ToList();
                        foreach (int assemly in assemblyToProcess)
                        {
                            assembliesList.Append(assemly);
                            assembliesList.Append(',');
                        }

                        //To remove last comma in the list of processed assemblies
                        if (assembliesList.Length > 0)
                        {
                            assembliesList.Remove((assembliesList.Length - 1), 1);
                        }
                        //db.Database.SqlQuery<int>("exec EE_RecalcNationwideAssembly @AssemblyList", new SqlParameter("AssemblyList", assembliesList));
                    }
                    int resultcount = db.SaveChanges();

                    string connStr = ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        using (SqlCommand cmd = new SqlCommand("EE_RecalcNationwideAssembly", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@AssemblyList", SqlDbType.NVarChar).Value = assembliesList.ToString();
                            cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = PDTblObj.Updated_By;

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    if (resultcount > 0)
                    {
                        if (!isupdate)
                        {
                            logs.WriteLog("Successfully Added the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Created_By);
                        }
                        else
                            logs.WriteLog("Successfully Updated the New Parts - " + PDTblObj.Part_Number, PDTblObj.Client_Id, PDTblObj.Updated_By);

                        response.ResultCode = 1;
                        response.Message = stat;
                        return response;
                    }
                    else
                    {
                        // response.ResultCode = 1;
                        response.Message = "Error while “Parts Registering”!";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While adding Parts - " + Model.Part_Number + "[Error Msg- " + ex.Message + " ]", Model.Client_ID, Model.Created_By);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        public ServiceResult<Parts_DetailsInfo> GetMyPartsDetails(string PartNumber, int ClientID)
        {
            ServiceResult<Parts_DetailsInfo> response = new ServiceResult<Parts_DetailsInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Parts_Details PDTblObj = new Parts_Details();
                    PDTblObj = (from m in db.Parts_Details where m.Part_Number == PartNumber && m.IsActive == true && m.Client_Id == ClientID select m).FirstOrDefault();
                    if (PDTblObj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“User” doesn't exist!";
                        return response;
                    }
                    response.Data = new Parts_DetailsInfo()
                    {
                        Client_ID = PDTblObj.Client_Id,
                        Part_Number = PDTblObj.Part_Number,
                        Part_Category = PDTblObj.Part_Category,
                        Description = PDTblObj.Description,
                        Client_Description = PDTblObj.Client_Description,
                        Cost = PDTblObj.Cost,
                        Resale_Cost = PDTblObj.Resale_Cost ?? 0,
                        Purchased_From = PDTblObj.Purchased_From,
                        UOM = PDTblObj.UOM,
                        LaborUnit = PDTblObj.LaborUnit ?? 0,
                        Created_By = PDTblObj.Created_By,
                        Created_Date = PDTblObj.Created_Date,
                        Updated_By = PDTblObj.Updated_By,
                        Updated_Date = PDTblObj.Updated_Date,
                        Part_CategoryCl = PDTblObj.Part_Category
                    };

                    response.ResultCode = 1;
                    response.Message = "Success";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        /// <summary>
        /// GetMyDistbutorPartsDetails
        /// </summary>
        /// <param name="PartNumber"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public ServiceResult<Parts_DetailsInfo> GetMyDistbutorPartsDetails(string PartNumber, int D_Id)
        {
            ServiceResult<Parts_DetailsInfo> response = new ServiceResult<Parts_DetailsInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Distributor_Parts_Details PDTblObj = new Distributor_Parts_Details();
                    PDTblObj = (from m in db.Distributor_Parts_Details where m.Part_Number == PartNumber && m.IsActive == true && m.Distributor_ID == D_Id select m).FirstOrDefault();
                    if (PDTblObj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“User” doesn't exist!";
                        return response;
                    }
                    response.Data = new Parts_DetailsInfo()
                    {

                        Client_ID = PDTblObj.Client_Id,
                        Part_Number = PDTblObj.Part_Number,
                        Part_Category = PDTblObj.Part_Category,
                        Description = PDTblObj.Description,
                        Client_Description = PDTblObj.Client_Description,
                        Cost = PDTblObj.Cost ?? 0,
                        DistributorID = PDTblObj.Distributor_ID,
                        Resale_Cost = PDTblObj.Resale_Cost ?? 0,
                        Purchased_From = PDTblObj.Purchased_From,
                        UOM = PDTblObj.UOM,
                        LaborUnit = PDTblObj.LaborUnit ?? 0,
                        Created_By = PDTblObj.Created_By,
                        Created_Date = PDTblObj.Created_Date,
                        Updated_By = PDTblObj.Updated_By,
                        Updated_Date = PDTblObj.Updated_Date

                    };

                    response.ResultCode = 1;
                    response.Message = "Success";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;

            }
        }
        /// <summary>
        /// GetNationawidePartsDetails
        /// </summary>
        /// <param name="PartNumber"></param>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public ServiceResult<Parts_DetailsInfo> GetNationawidePartsDetails(string PartNumber)
        {
            ServiceResult<Parts_DetailsInfo> response = new ServiceResult<Parts_DetailsInfo>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Super_Admin_Parts_Details PDTblObj = new Super_Admin_Parts_Details();
                    PDTblObj = (from m in db.Super_Admin_Parts_Details where m.Part_Number == PartNumber && m.IsActive == true select m).FirstOrDefault();
                    if (PDTblObj == null)
                    {
                        response.ResultCode = 0;
                        response.Message = "“User” doesn't exist!";
                        return response;
                    }
                    response.Data = new Parts_DetailsInfo()
                    {

                        Part_Number = PDTblObj.Part_Number,
                        Part_Category = PDTblObj.Part_Category,
                        Description = PDTblObj.Description,
                        Client_Description = PDTblObj.Client_Description,
                        Cost = PDTblObj.Cost,
                        Resale_Cost = PDTblObj.Resale_Cost ?? 0,
                        Purchased_From = PDTblObj.Purchased_From,
                        UOM = PDTblObj.UOM,
                        LaborUnit = PDTblObj.LaborUnit ?? 0,
                        Created_By = PDTblObj.Created_By,
                        Created_Date = PDTblObj.Created_Date,
                        Updated_By = PDTblObj.Updated_By,
                        Updated_Date = PDTblObj.Updated_Date

                    };

                    response.ResultCode = 1;
                    response.Message = "Success";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;

            }
        }


        public ServiceResult<int> SavePartsDetails(EditParts_DetailsInfo Model)
        {
            ServiceResult<int> response = new ServiceResult<int>();

            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Parts_Details PDTblObj = new Parts_Details();
                    //Check whether given Email Id is already Exists

                    PDTblObj = (from m in db.Parts_Details where m.Part_Number == Model.Part_Number && m.Client_Id == Model.Client_ID && m.IsActive == true select m).FirstOrDefault();
                    //PDTblObj.Part_Number = Model.Part_Number;
                    if (Model.OtherPart_Category != null)
                    {
                        Model.Part_Category = Model.OtherPart_Category;
                    }
                    if (Model.Part_Category == "1" && Model.OtherPart_Category != null)
                    {
                        Model.Part_Category = Model.OtherPart_Category;
                    }
                    if (Model.Part_Category != null)
                    {
                        PDTblObj.Part_Category = Model.Part_Category;
                    }

                    PDTblObj.Description = Model.Description;
                    PDTblObj.Client_Description = Model.Client_Description;
                    PDTblObj.Cost = Model.Cost;
                    PDTblObj.Resale_Cost = Model.Resale_Cost;
                    PDTblObj.Purchased_From = Model.Purchased_From;
                    PDTblObj.Updated_By = Model.Updated_By;
                    PDTblObj.Updated_Date = DateTime.Now; ;
                    PDTblObj.Updated_By = Model.Updated_By;


                    int resultcount = db.SaveChanges();

                    if (resultcount > 0)
                    {
                        response.ResultCode = resultcount;
                        response.ResultCode = 1;
                        response.Message = "Your “Changes” have been modified successfully.";
                        return response;
                    }
                    else
                    {
                        response.Data = resultcount;
                        response.ResultCode = -1;
                        response.Message = "“No” changes found in your profile!";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }


        public ServiceResult<int> DeletePartsDetails(string PartNumber, int ClientID, string Username)
        {
            ServiceResult<int> response = new ServiceResult<int>();
            int resultcount;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    //To validate whether deleting parts are assosiated with any Job Assembly Parts and Job Parts
                    var chkJobPart = db.Job_Parts.Where(m => m.Part_Number == PartNumber && m.IsActive == true && m.Client_ID == ClientID).FirstOrDefault();
                    var chkJobAssemblyPart = db.Job_Assembly_Parts.Where(m => m.Part_Number == PartNumber && m.IsActive == true && m.Client_ID == ClientID).FirstOrDefault();
                    if (chkJobPart != null || chkJobAssemblyPart != null)
                    {
                        response.ResultCode = -1;
                        response.Data = 0;
                        response.Message = "Part '" + PartNumber + "' can't be deleted as it is associated to job!";
                        return response;
                    }

                    Parts_Details tableObj = new Parts_Details();
                    List<Assemblies_Parts> assemblyParts = new List<Assemblies_Parts>();
                    StringBuilder assemblyList = new StringBuilder();


                    tableObj = db.Parts_Details.Where(m => m.Part_Number == PartNumber && m.IsActive == true && m.Client_Id == ClientID).FirstOrDefault();
                    assemblyParts = db.Assemblies_Parts.Where(m => m.Part_Number == PartNumber && m.Client_ID == ClientID).ToList();

                    db.Parts_Details.Remove(tableObj);

                    foreach(Assemblies_Parts obj in assemblyParts)
                    {
                        db.Assemblies_Parts.Remove(obj);
                        assemblyList.Append(obj.Assemblies_Name + ",");
                    }

                    resultcount = db.SaveChanges();

                    string connStr = ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        using (SqlCommand cmd = new SqlCommand("EE_RecalcClientAssembly", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@AssemblyList", SqlDbType.NVarChar).Value = assemblyList.ToString();
                            cmd.Parameters.Add("@ClientID", SqlDbType.Int).Value = ClientID;
                            cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = Username;

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    logs.WriteLog("Successfully deleted the Parts Details - " + PartNumber, ClientID, Username);

                    if (resultcount > 0)
                    {
                        response.ResultCode = resultcount;
                        response.Data = resultcount;
                        response.Message = "“Deleted” successfully.";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Data = resultcount;
                        response.Message = "Delete \"Failed\"!";
                        return response;
                    }

                }

            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While deleting the Parts - " + PartNumber + " [Error Msg - " + ex.Message + " ]", ClientID, Username);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;

            }
        }


        public ServiceResult<int> DeleteDistributorPartsDetails(string PartNumber, int DistributorID, string Username)
        {

            ServiceResult<int> response = new ServiceResult<int>();
            List<Distributor_Assemblies_Parts> assemblyPartsList = new List<Distributor_Assemblies_Parts>();
            StringBuilder assemblyList = new StringBuilder();

            int resultcount;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Distributor_Parts_Details tableObj = new Distributor_Parts_Details();

                    tableObj = db.Distributor_Parts_Details.Where(m => m.Part_Number == PartNumber && m.IsActive == true && m.Distributor_ID == DistributorID).FirstOrDefault();
                    assemblyPartsList = db.Distributor_Assemblies_Parts.Where(m => m.Part_Number == PartNumber && m.Distributor_ID == DistributorID).ToList();

                    db.Distributor_Parts_Details.Remove(tableObj);

                    foreach (Distributor_Assemblies_Parts obj in assemblyPartsList)
                    {
                        db.Distributor_Assemblies_Parts.Remove(obj);
                        assemblyList.Append(Convert.ToInt32(obj.AssemblyID) + ",");
                    }

                    resultcount = db.SaveChanges();

                    //To update the labor unit on Client Assembly Master
                    string connStr = ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        using (SqlCommand cmd = new SqlCommand("EE_RecalcDistributorAssembly", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@AssemblyList", SqlDbType.NVarChar).Value = assemblyList.ToString();
                            cmd.Parameters.Add("@DistributorID", SqlDbType.Int).Value = DistributorID;
                            cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = Username;

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    logs.WriteLog("Successfully deleted the Parts Details - " + PartNumber, DistributorID, Username);
                    if (resultcount > 0)
                    {
                        response.ResultCode = resultcount;
                        response.Data = resultcount;
                        response.Message = "“Deleted” successfully.";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Data = resultcount;
                        response.Message = "Delete \"Failed\"!";
                        return response;
                    }

                }

            }
            catch (Exception ex)
            {
                logs.WriteLog("[Exception] While deleting the Parts - " + PartNumber + " [Error Msg - " + ex.Message + " ]", DistributorID, Username);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;

            }
        }

        /// <summary>
        /// DeleteNotionaWidePartsDetails
        /// </summary>
        /// <param name="PartNumber"></param> 
        /// <returns></returns>
        public ServiceResult<int> DeleteNotionaWidePartsDetails(string PartNumber)
        {

            ServiceResult<int> response = new ServiceResult<int>();
            int resultcount;
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Super_Admin_Parts_Details tableObj = new Super_Admin_Parts_Details();
                    List<Super_Admin_Assemblies_Parts> assemblyPartsList = new List<Super_Admin_Assemblies_Parts>();
                    StringBuilder assemblyList = new StringBuilder();

                    tableObj = db.Super_Admin_Parts_Details.Where(m => m.Part_Number == PartNumber && m.IsActive == true).FirstOrDefault();
                    assemblyPartsList = db.Super_Admin_Assemblies_Parts.Where(m => m.Part_Number == PartNumber).ToList();

                    db.Super_Admin_Parts_Details.Remove(tableObj);

                    foreach(Super_Admin_Assemblies_Parts obj in assemblyPartsList)
                    {
                        db.Super_Admin_Assemblies_Parts.Remove(obj);
                        assemblyList.Append(Convert.ToInt32(obj.Assemblies_Id) + ",");
                    }

                    resultcount = db.SaveChanges();

                    string connStr = ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        using (SqlCommand cmd = new SqlCommand("EE_RecalcNationwideAssembly", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@AssemblyList", SqlDbType.NVarChar).Value = assemblyList.ToString();
                            cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = "Admin";

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    if (resultcount > 0)
                    {
                        response.ResultCode = resultcount;
                        response.Data = resultcount;
                        response.Message = "“Deleted” successfully.";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Data = resultcount;
                        response.Message = "Delete \"Failed\"!";
                        return response;
                    }

                }

            }
            catch (Exception ex)
            {
                //logs.WriteLog("[Exception] While deleting the Parts - " + PartNumber + " [Error Msg - " + ex.Message + " ]", ClientID, Username);
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;

            }
        }

        public ServiceResultList<Parts_DetailsInfoList> GetJobPartsList(int ClientID, string Part_Number)
        {
            ServiceResultList<Parts_DetailsInfoList> response = new ServiceResultList<Parts_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Parts_DetailsInfoList> result = new List<Parts_DetailsInfoList>();
                    result = db.Database.SqlQuery<Parts_DetailsInfoList>("exec EE_SelectedAssembliesPartsDetailList @ClientID, @Part_Number", new SqlParameter("ClientID", ClientID), new SqlParameter("Part_Number", Part_Number)).ToList();

                    if (result != null)
                    {
                        response.ListData = result;
                        response.ResultCode = 1;
                        response.Message = "Success";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "No “Records” found!";
                        return response;

                    }
                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }
        public ServiceResultList<Labor_DetailsInfoList> GetMyJobLaborDetailsList(int ClientID)
        {
            ServiceResultList<Labor_DetailsInfoList> response = new ServiceResultList<Labor_DetailsInfoList>();
            try
            {

                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    List<Labor_DetailsInfoList> result = new List<Labor_DetailsInfoList>();
                    SqlParameter[] para = new SqlParameter[1];
                    para[0] = new SqlParameter() { ParameterName = "ClientID", Value = ClientID };
                    result = db.Database.SqlQuery<Labor_DetailsInfoList>("exec EE_GetMyLabourDetailList @ClientID", para).ToList();

                    if (result != null)
                    {
                        response.ListData = result;
                        response.ResultCode = 1;
                        response.Message = "Success";
                        return response;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.Message = "No “Records” found!";
                        return response;

                    }
                }

            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        public string UploadParts(List<Parts_Details> lstparts)
        {
            try
            {
                if (lstparts.Count > 0)
                {
                    foreach (Parts_Details parts in lstparts)
                    {
                        using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                        {
                            Parts_Details dbparts = db.Parts_Details.Where(x => x.Client_Id == parts.Client_Id && x.Part_Number == parts.Part_Number && x.Part_Category != null).FirstOrDefault();
                            if (dbparts != null)
                            {
                                dbparts.Part_Category = parts.Part_Category;
                                dbparts.Cost = parts.Cost;
                                dbparts.Resale_Cost = parts.Resale_Cost;
                                dbparts.Client_Description = parts.Client_Description;
                                dbparts.Updated_Date = DateTime.Now;
                                dbparts.Updated_By = parts.Updated_By;
                                dbparts.Purchased_From = parts.Purchased_From;
                                dbparts.Description = parts.Description;
                                dbparts.UOM = parts.UOM;
                                db.SaveChanges();
                            }
                            else
                            {
                                parts.IsActive = true;
                                parts.Created_Date = DateTime.Now;
                                parts.Updated_Date = DateTime.Now;
                                db.Parts_Details.Add(parts);
                                db.SaveChanges();
                            }
                        }
                    }
                    return "\"Parts\" are uploaded successfully.";
                }
                else
                {
                    return "No parts are there to upload";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public ServiceResult<bool> CheckIsNewPartNumber(int ClientID, string PartNumber)
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Parts_Details partstbl = new Parts_Details();
                    bool CheckIsExist = db.Parts_Details.Any(x => x.Part_Number == PartNumber && x.Client_Id == ClientID && x.IsActive == true);
                    if (CheckIsExist == true)
                    {
                        response.Data = true;
                        response.ResultCode = 1;
                        response.Message = "“Part Number” already exists. Do you want to update?";
                        return response;

                    }
                    else
                    {
                        response.Data = false;
                        response.ResultCode = 0;
                        response.Message = "“Part Number” does not exist!";
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        /// <summary>
        /// CheckIsDistributorNewPartNumber
        /// </summary>
        /// <param name="Distributor"></param>
        /// <param name="PartNumber"></param>
        /// <returns></returns>
        public ServiceResult<bool> CheckIsDistributorNewPartNumber(int Distributor, string PartNumber)
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Distributor_Parts_Details partstbl = new Distributor_Parts_Details();
                    bool CheckIsExist = db.Distributor_Parts_Details.Any(x => x.Part_Number == PartNumber && x.Distributor_ID == Distributor && x.IsActive == true);
                    if (CheckIsExist == true)
                    {
                        response.Data = true;
                        response.ResultCode = 1;
                        response.Message = "“Part Number” already exists. Do you want to update?";
                        return response;

                    }
                    else
                    {
                        response.Data = false;
                        response.ResultCode = 0;
                        response.Message = "“Part Number” does not exist!";
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        /// <summary>
        /// SuperAdminCheckIsNewPartNumber
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="PartNumber"></param>
        /// <returns></returns>
        public ServiceResult<bool> SuperAdminCheckIsNewPartNumber(string PartNumber)
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            try
            {
                using (ElectricEaseEntitiesContext db = new ElectricEaseEntitiesContext())
                {
                    Parts_Details partstbl = new Parts_Details();
                    bool CheckIsExist = db.Super_Admin_Parts_Details.Any(x => x.Part_Number == PartNumber && x.IsActive == true);
                    if (CheckIsExist == true)
                    {
                        response.Data = true;
                        response.ResultCode = 1;
                        response.Message = "“Part Number” already exists. Do you want to update?";
                        return response;

                    }
                    else
                    {
                        response.Data = false;
                        response.ResultCode = 0;
                        response.Message = "“Part Number” does not exist!";
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.ResultCode = -1;
                response.Message = ex.Message;
                return response;
            }
        }

        public DataTable ImportParts(DataTable dtExcelParts, int clientID)
        {
            DataTable statusList = new DataTable();

            try
            {
                //Temp table insertion
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString))
                {

                    cn.Open();
                    using (SqlBulkCopy copy = new SqlBulkCopy(cn))
                    {
                        copy.ColumnMappings.Add(0, 0);
                        copy.ColumnMappings.Add(1, 1);
                        copy.ColumnMappings.Add(2, 2);
                        copy.ColumnMappings.Add(3, 3);
                        copy.ColumnMappings.Add(4, 4);
                        copy.ColumnMappings.Add(5, 5);
                        copy.ColumnMappings.Add(6, 6);
                        copy.ColumnMappings.Add(7, 7);
                        copy.ColumnMappings.Add(8, 8);
                        copy.ColumnMappings.Add(9, 9);
                        copy.ColumnMappings.Add(10, 10);
                        copy.DestinationTableName = "ImportParts";
                        copy.WriteToServer(dtExcelParts);
                    }

                    //Actual table insertion
                    using (var cmd = new SqlCommand("EE_ImportClientParts", cn))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@ProcessID", SqlDbType.VarChar, 50).Value = dtExcelParts.Rows[0][0];
                        cmd.Parameters.Add("@ClientID", SqlDbType.Int).Value = clientID;
                        da.Fill(statusList);
                    }
                }
            }
            catch
            {
                throw;
            }
            return statusList;
        }

        public DataTable ImportNationWideParts(DataTable dtExcelParts)
        {
            DataTable statusList = new DataTable();

            try
            {
                //Temp table insertion
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString))
                {

                    cn.Open();
                    using (SqlBulkCopy copy = new SqlBulkCopy(cn))
                    {
                        copy.ColumnMappings.Add(0, 0);
                        copy.ColumnMappings.Add(1, 1);
                        copy.ColumnMappings.Add(2, 2);
                        copy.ColumnMappings.Add(3, 3);
                        copy.ColumnMappings.Add(4, 4);
                        copy.ColumnMappings.Add(5, 5);
                        copy.ColumnMappings.Add(6, 6);
                        copy.ColumnMappings.Add(7, 7);
                        copy.ColumnMappings.Add(8, 8);
                        copy.ColumnMappings.Add(9, 9);
                        copy.ColumnMappings.Add(10, 10);
                        copy.DestinationTableName = "ImportParts";
                        copy.WriteToServer(dtExcelParts);
                    }

                    //Actual table insertion
                    using (var cmd = new SqlCommand("EE_ImportNationwideParts", cn))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@ProcessID", SqlDbType.VarChar, 50).Value = dtExcelParts.Rows[0][0];
                        da.Fill(statusList);
                    }
                }
            }
            catch
            {
                throw;
            }
            return statusList;
        }

        public DataTable ImportDistributorParts(DataTable dtExcelParts, int distributorID)
        {
            DataTable statusList = new DataTable();

            try
            {
                //Temp table insertion
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["ElectricEaseConnectionString"].ConnectionString))
                {

                    cn.Open();
                    using (SqlBulkCopy copy = new SqlBulkCopy(cn))
                    {
                        copy.ColumnMappings.Add(0, 0);
                        copy.ColumnMappings.Add(1, 1);
                        copy.ColumnMappings.Add(2, 2);
                        copy.ColumnMappings.Add(3, 3);
                        copy.ColumnMappings.Add(4, 4);
                        copy.ColumnMappings.Add(5, 5);
                        copy.ColumnMappings.Add(6, 6);
                        copy.ColumnMappings.Add(7, 7);
                        copy.ColumnMappings.Add(8, 8);
                        copy.ColumnMappings.Add(9, 9);
                        copy.ColumnMappings.Add(10, 10);
                        copy.DestinationTableName = "ImportParts";
                        copy.WriteToServer(dtExcelParts);
                    }

                    //Actual table insertion
                    using (var cmd = new SqlCommand("EE_ImportDistributorParts", cn))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@ProcessID", SqlDbType.VarChar, 50).Value = dtExcelParts.Rows[0][0];
                        cmd.Parameters.Add("@DistributorID", SqlDbType.Int).Value = distributorID;


                        da.Fill(statusList);
                    }
                }
            }
            catch
            {
                throw;
            }
            return statusList;
        }
    }




    // Start - JSon class sent from Datatables


    public class DataTableAjaxPostModel
    {
        // properties are not capital due to json mapping
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<Column> columns { get; set; }
        public Search search { get; set; }
        public List<Order> order { get; set; }
        public string extra_search { get; set; }
        public string existing_data { get; set; }
        public int distributorId { get; set; }
        public int clientId { get; set; }
    }



    public class Column
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; }
    }

    public class Search
    {
        public string value { get; set; }
        public string regex { get; set; }
    }

    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }

    public static class OrderData
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string memberName, bool asc = true)
        {
            ParameterExpression[] typeParams = new ParameterExpression[] { Expression.Parameter(typeof(T), "") };

            PropertyInfo pi = typeof(T).GetProperty(memberName);
            return (IOrderedQueryable<T>)query.Provider.CreateQuery(
                Expression.Call(
                    typeof(Queryable),
                    asc ? "OrderBy" : "OrderByDescending",
                    new Type[] { typeof(T), pi.PropertyType },
                    query.Expression,
                    Expression.Lambda(Expression.Property(typeParams[0], pi), typeParams))
                );
        }

        public static PropertyInfo GetPropValue<T>(this T obj, String propName)
        {
            PropertyInfo info = null;
            //string[] nameParts = propName.Split('.');
            //if (nameParts.Length == 1)
            //{
            //    info= obj.GetType().GetProperty(propName);
            //}

            //foreach (String part in nameParts)
            //{
            //   // if (obj == null) { return null; }

            //    Type type = obj.GetType();
            //     info = type.GetProperty(part);
            //  //  if (info == null) { return null; }
            //  //  return info;
            //   // obj = info.GetValue(obj, null);
            //}
            var parts = propName.Split('.').ToList();
            var currentPart = parts[0];
            info = typeof(T).GetProperty(currentPart);
            if (info == null) { return null; }
            if (propName.IndexOf(".") > -1)
            {
                parts.Remove(currentPart);
                return GetPropValue(info.GetValue(obj, null), String.Join(".", parts));
            }
            else
            {
                return info;
            }

        }
    }

    /// <summary>    
    /// Enables the efficient, dynamic composition of query predicates.    
    /// </summary>    
    public static class PredicateBuilder
    {
        /// <summary>    
        /// Creates a predicate that evaluates to true.    
        /// </summary>    
        public static Expression<Func<T, bool>> True<T>() { return param => true; }

        /// <summary>    
        /// Creates a predicate that evaluates to false.    
        /// </summary>    
        public static Expression<Func<T, bool>> False<T>() { return param => false; }

        /// <summary>    
        /// Creates a predicate expression from the specified lambda expression.    
        /// </summary>    
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

        /// <summary>    
        /// Combines the first predicate with the second using the logical "and".    
        /// </summary>    
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>    
        /// Combines the first predicate with the second using the logical "or".    
        /// </summary>    
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>    
        /// Negates the predicate.    
        /// </summary>    
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
        {
            var negated = Expression.Not(expression.Body);
            return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
        }

        /// <summary>    
        /// Combines the first expression with the second using the specified merge function.    
        /// </summary>    
        static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)    
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first    
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression    
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        class ParameterRebinder : ExpressionVisitor
        {
            readonly Dictionary<ParameterExpression, ParameterExpression> map;

            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }
    /// End- JSon class sent from Datatables
}
