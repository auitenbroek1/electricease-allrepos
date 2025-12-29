using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase;

namespace ElectricEase.Models
{
    public class ClientList
    {
        public int Client_ID { get; set; }
        public string Client_Company { get; set; }
        public string User_ID { get; set; }
        //public string State { get; set; }
    }


    public class PartsCategoryList
    {
        public string Part_Category { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }

    public class LabourCategoryList
    {
        public string Labor_Category { get; set; }
    }

    public class LegalCategoryList
    {
        public string Legal_Category { get; set; }
    }

    public class JobCategoryList
    {
        public string jobCategory { get; set; }
    }

    public class AssembliesNameList
    {
        public string Assemblies_Name { get; set; }
    }
    public class AssembliesCategoryList
    {
        public string Assemblies_Category { get; set; }
    }
}


