using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectricEase;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
//using System.Web.Mvc;
using log4net;
using System.Collections;
using System.Collections.ObjectModel;
using System.Web.UI.WebControls;

namespace ElectricEase.Models
{
    public class Logger
    {
        public static readonly ILog log = LogManager.GetLogger("Log");

        public static void Info(string message)
        {
            log.Info(message);
        }

        public static void Error(string message)
        {
            log.Error(message);
        }

        public static void Debug(string message)
        {
            log.Debug(message);
        }

        public static void Fatal(string message)
        {
            log.Fatal(message);
        }

        public static void Warning(string message)
        {
            log.Warn(message);
        }
    }

    public class LoginInfo
    {
        public int Client_ID { get; set; }
        //[RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string UserName { get; set; }
        //[RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Password { get; set; }
        public string Client_Company { get; set; }
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string NewPassword { get; set; }
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string ConfirmPassword { get; set; }
        public string ReturnUrl { get; set; }
        public string Email { get; set; }
        public string TempEmail { get; set; }


    }
    public class sowdata
    {
        public int Id { get; set; }
        public string Sow_desc { get; set; }
        public Nullable<int> ClientId { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }

    }
    public class CountDetails
    {
        public int Parts_Count { get; set; }
        public int Assembly_Count { get; set; }
        public int JobOpen_Count { get; set; }
        public int JobClosed_Count { get; set; }
    }

    public class CalendarEventItem
    {
        public int ID { get; set; }
        public string User_ID { get; set; }
        public string Job_AssignedUser { get; set; }
        public string JobDisplayId { get; set; }
        public string Client_Name { get; set; }
        public string Title { get; set; }
        public bool IsAllDayEvent { get; set; }
        public string TotalHours { get; set; }
        public bool? Fullday { get; set; }

        public DateTime Start { get; set; }
        public DateTime timeFormat { get; set; }
        public TimeSpan? Starttime { get; set; }
        public TimeSpan? Endtime { get; set; }
        public string sTime { get; set; }
        public DateTime End { get; set; }
        public string eTime { get; set; }
        public string backgroundcolor { get; set; }
        public string border { get; set; }
        public string JobStatus { get; set; }
        public string handleWindowResize { get; set; }
    }
    public class Client_MasterInfolist
    {
        public int Client_ID { get; set; }
        public string Client_Company { get; set; }
        public byte[] Client_Logo { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use text only")]
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Contact_person { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        public string SuperUser { get; set; }
        public string SuperUser_Pwd { get; set; }
        public Boolean? IsActive { get; set; }
        public string urlogo { get; set; }
        public int DistributorID { get; set; }
        public string Distributorname { get; set; }
        public string ContactPerson { get; set; }



    }

    public class DistributorPartsList
    {
        public int Distributor_Id { get; set; }
        public string Part_Number { get; set; }
        public string Part_Category { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Resale_Cost { get; set; }
        public string Purchased_From { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
    }




    public class Client_MasterInfo
    {
        public string FileLocation { get; set; }
        public List<Client_MasterInfolist> ClientList { get; set; }
        public int Client_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Client_Company { get; set; }
        // [Required(ErrorMessage = "Required")]
        public HttpPostedFileBase UploadedFile { get; set; }
        public byte[] Client_Logo { get; set; }
        [MaxLength(200, ErrorMessage = "Max 200 chars length")]
        //[RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        //[RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Invalid Text")]
        //[RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$",
        // ErrorMessage = "special characters are not  allowed.")]
        [RegularExpression(@"[a-zA-Z0-9._^%$#! ,/()~@,-]+", ErrorMessage = "Invalid Text")]
        public string Address { get; set; }
        [MaxLength(200, ErrorMessage = "Max 200 chars length")]
        //[RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        //   [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$",
        //ErrorMessage = "special characters are not  allowed.")]
        [RegularExpression(@"[a-zA-Z0-9._^%$#! ,/()~@,-]+", ErrorMessage = "Invalid Text")]
        public string Address2 { get; set; }
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string City { get; set; }
        //[MaxLength(100, ErrorMessage = "Max 100 chars length")]
        //[RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Client_State { get; set; }
        [MaxLength(10, ErrorMessage = "Max 10 chars length")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Zip")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use text only")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(50, ErrorMessage = "Max 50 chars length")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Invalid “Phone Number”!")]
        public string Phone { get; set; }
        [MaxLength(50, ErrorMessage = "Max 50 chars length")]
        // [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Mobile number")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Invalid “Phone Number”!")]
        public string Mobile { get; set; }
        [MaxLength(50, ErrorMessage = "Max 50 chars length")]
        // [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Invalid Fax !")]
        public string Fax { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(50, ErrorMessage = "Max 50 chars length")]
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use text only")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Contact_person { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string SuperUser { get; set; }
        // [DataType(DataType.Password)]
        //[MaxLength(20, ErrorMessage = "Max 20 chars length")]
        // [Required(ErrorMessage = " Please Enter Password")]
        // [RegularExpression(@"^[\S]*$", ErrorMessage = "White space found")]
        public string SuperUser_Pwd { get; set; }
        public Boolean IsActive { get; set; }


        public string Sender_EmailAddress { get; set; }
        public string Sender_EmailPassword { get; set; }
        public string DomainName { get; set; }
        public Nullable<int> SMTP_Port { get; set; }
        public string SMTP_Host { get; set; }
        [MaxLength(3, ErrorMessage = "Max 3 chars length")]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage = "Use Upper Case letters only please")]
        public string JobIDPreffix { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a Numbers only")]
        public int? AutoSaveTime_InSecs { get; set; }
        //public EditClient_MasterInfo EditClientInfo { get; set; }
        public bool activateStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DistributorID { get; set; }
    }
    public class EditClient_MasterInfo
    {
        public string FileLocation { get; set; }
        public List<Client_MasterInfolist> ClientList { get; set; }
        public int Client_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Client_Company { get; set; }
        //[Required(ErrorMessage = "Required")]
        public HttpPostedFileBase UploadedFile { get; set; }
        public byte[] Client_Logo { get; set; }
        [MaxLength(200, ErrorMessage = "Max 200 chars length")]
        //[RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$",
     ErrorMessage = "special characters are not  allowed.")]
        public string Address { get; set; }
        [MaxLength(200, ErrorMessage = "Max 200 chars length")]
        //[RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$",
     ErrorMessage = "special characters are not  allowed.")]
        public string Address2 { get; set; }
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string City { get; set; }
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string State { get; set; }
        [MaxLength(10, ErrorMessage = "Max 10 chars length")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Zip")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use text only")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(50, ErrorMessage = "Max 50 chars length")]
        // [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Phone number")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Invalid “Phone Number”!")]
        public string Phone { get; set; }
        [MaxLength(50, ErrorMessage = "Max 50 chars length")]
        // [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Mobile number")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Invalid “Phone Number”!")]
        public string Mobile { get; set; }
        [MaxLength(50, ErrorMessage = "Max 50 chars length")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Invalid Fax !")]
        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Fax")]
        public string Fax { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max 50 chars length")]
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use text only")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Contact_person { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        //[Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string SuperUser { get; set; }
        [MaxLength(20, ErrorMessage = "Max 20 chars length")]
        [DataType(DataType.Password)]
        //[StringLength(20, ErrorMessage = "Pass \"{0}\" must have no less {2} chars.", MinimumLength = 8)]
        //[StringLength(20, ErrorMessage = "Password must have no less {2} chars.", MinimumLength = 8)]
        [RegularExpression(@"^[\S]*$", ErrorMessage = "White space found")]
        public string SuperUser_Pwd { get; set; }
        public Boolean IsActive { get; set; }

        public string Sender_EmailAddress { get; set; }
        public string Sender_EmailPassword { get; set; }
        public string DomainName { get; set; }
        public Nullable<int> SMTP_Port { get; set; }
        public string SMTP_Host { get; set; }
        [MaxLength(3, ErrorMessage = "Max 3 chars length")]
        [RegularExpression(@"^[A-Z]+$", ErrorMessage = "Use Upper Case letters only please")]
        public string JobIDPreffix { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a Numbers only")]
        public int? AutoSaveTime_InSecs { get; set; }


    }

    public class Account_MasterInfoList
    {
        public long ID { get; set; }
        public int Client_ID { get; set; }
        public string Client_Company { get; set; }
        public string User_ID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Phone { get; set; }
        public string E_Mail { get; set; }
        public Boolean IsActive { get; set; }
        public string Password { get; set; }
        public string UserColor { get; set; }
        //public HttpPostedFileBase UploadedFile { get; set; }
        //public byte[] Photo { get; set; }
        //public bool Site_Administrator { get; set; }
        //public bool Account_Administrator { get; set; }
        //public bool Job_Administrator { get; set; }
        //public bool Part_Administrator { get; set; }
        //public bool Labor_Administrator { get; set; }
        //public bool Legal_Adminstrator { get; set; }
        //public string Created_By { get; set; }
        //public Nullable<System.DateTime> Created_Date { get; set; }
        //public bool CreatedBy_SuperUser { get; set; }
        //public string Updated_By { get; set; }
        //public Nullable<System.DateTime> Updated_Date { get; set; }

    }

    public class Account_MasterInfo
    {

        public List<Account_MasterInfoList> AMUsersList = new List<Account_MasterInfoList>();
        public string Client_Company { get; set; }
        public int Client_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        public string User_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use text only")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string First_Name { get; set; }
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use text only")]
        //[Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Last_Name { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "Max 30 chars length")]
        // [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Phone number")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Invalid “Phone Number”!")]
        public string Phone { get; set; }
        //[Required(ErrorMessage = "Required")]
        public HttpPostedFileBase UploadedFile { get; set; }
        public byte[] Photo { get; set; }
        public byte[] MyClientlogo { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max 50 chars length")]
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string E_Mail { get; set; }
        [MaxLength(20, ErrorMessage = "Max 20 chars length")]
        [Required(ErrorMessage = " Please Enter Password")]
        //[StringLength(20, ErrorMessage = "Password must have no less {2} chars.", MinimumLength = 8)]
        [RegularExpression(@"^[\S]*$", ErrorMessage = "White space found")]
        public string Password { get; set; }

        public bool Site_Administrator { get; set; }

        public bool Account_Administrator { get; set; }

        public bool Job_Administrator { get; set; }

        public bool Part_Administrator { get; set; }

        public bool Labor_Administrator { get; set; }
        //New Fields added by Arunkumar. K on 12/16/2015.
        public bool Assembly_Administrator { get; set; }

        public bool Job_Description_Report { get; set; }

        public bool Client_Estimation_Report { get; set; }

        public bool Mailsendconformaition { get; set; }

        public bool Legal_Adminstrator { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public bool CreatedBy_SuperUser { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        public Boolean IsActive { get; set; }
        public string UserColor { get; set; }

        public Boolean? isfirst { get; set; }

        public int distId { get; set; }
        public string DistLogo { get; set; }
        public string distCompurl { get; set; }
    }

    public class EditAccount_MasterInfo
    {

        //public List <Account_MasterInfoList> AMUsersList { get; set; }
        public List<Account_MasterInfoList> AMUsersList = new List<Account_MasterInfoList>();
        public string Client_Company { get; set; }
        public int Client_ID { get; set; }
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        public string User_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use text only")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string First_Name { get; set; }
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        //[RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use text only")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Last_Name { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(30, ErrorMessage = "Max 30 chars length")]
        // [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Phone number")]
        [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Invalid “Phone Number”!")]
        public string Phone { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
        public byte[] Photo { get; set; }
        public string UserColor { get; set; }
        public byte[] MyClientlogo { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max 50 chars length")]
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string E_Mail { get; set; }
        [MaxLength(20, ErrorMessage = "Max 20 chars length")]
        //[StringLength(20, ErrorMessage = "Password must have no less {2} chars.", MinimumLength = 8)]
        [RegularExpression(@"^[\S]*$", ErrorMessage = "White space found")]
        public string Password { get; set; }

        public bool Site_Administrator { get; set; }

        public bool Account_Administrator { get; set; }

        public bool Job_Administrator { get; set; }

        public bool Part_Administrator { get; set; }

        public bool Labor_Administrator { get; set; }

        public bool Legal_Adminstrator { get; set; }
        //New Fields added by Arunkumar. K on 12/16/2015.
        public bool Assembly_Administrator { get; set; }

        public bool Job_Description_Report { get; set; }

        public bool Client_Estimation_Report { get; set; }

        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public bool CreatedBy_SuperUser { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        public Boolean IsActive { get; set; }

        public string Address { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Client_State { get; set; }

        public string ZipCode { get; set; }

        public string JobIDPreffix { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }

        public string Contactperson { get; set; }

        public bool isfirst { get; set; }

        public string Imagesrc { get; set; }

    }
    public class Parts_GrandTotal
    {
        public decimal ExpGrandTotal { get; set; }
        public decimal EstGrandTotal { get; set; }
        public decimal ActGrandTotal { get; set; }
    }
    public class Labor_GrandTotal
    {
        public decimal ExpGrandTotal { get; set; }
        public decimal EstGrandTotal { get; set; }
        public decimal ActGrandTotal { get; set; }
    }

    public class Parts_DetailsInfoList
    {
        public long ID { get; set; }
        public int Client_ID { get; set; }
        public int Part_ID { get; set; }
        public string Client_Company { get; set; }
        public string Company { get; set; }
        public int Distributor_ID { get; set; }
        public string Part_Number { get; set; }
        public string UOM { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Part_Category { get; set; }
        public string OtherPart_Category { get; set; }
        public decimal Resale_Cost { get; set; }
        public string Purchased_From { get; set; }
        public decimal Cost { get; set; }
        public decimal Part_Cost { get; set; }
        public bool isChekced { get; set; }
        public decimal estimateQuantity { get; set; }
        public decimal Estimated_Qty { get; set; }
        public decimal Actual_Qty { get; set; }
        public decimal Expected_Total { get; set; }
        public decimal Estimated_Total { get; set; }
        public decimal? LaborUnit { get; set; }
        public decimal Actual_Total { get; set; }
        public bool isdiabled { get; set; }
        public string AssemblyName { get; set; }
        public string PartDescription { get; set; }
        public IList<Parts_DetailsInfoList> result { get; set; }
        //public string Description { get; set; }
        //public string Client_Description { get; set; }
        //public decimal Cost { get; set; }
        //public string Created_By { get; set; }
        // public System.DateTime? Created_Date { get; set; }
        //public string Updated_By { get; set; }
        public DateTime Updated_Date { get; set; }
        public Boolean IsActive { get; set; }
    }

    public class AssembliesParts_DetailsInfoList
    {
        //  public List<Parts_DetailsInfoList> PartsList { get; set; }
        public int AssemblyID { get; set; }
        public List<Parts_GrandTotal> PartsGrandTotalList { get; set; }
        public string Assemblies_Name { get; set; }
        public string Assemblies_Category { get; set; }
        public string Assemblies_Description { get; set; }
        public string Parts_Description { get; set; }
        public int Part_ID { get; set; }
        public int Client_ID { get; set; }
        public string Client_Company { get; set; }
        public string Part_Number { get; set; }
        public string Description { get; set; }
        public string Part_Category { get; set; }
        public string OtherPart_Category { get; set; }
        public decimal Resale_Cost { get; set; }
        public string Purchased_From { get; set; }
        public decimal Cost { get; set; }
        public decimal Estimated_Qty { get; set; }
        public decimal Actual_Qty { get; set; }
        public decimal Expected_Total { get; set; }
        public decimal Estimate_Qty_Total { get; set; }
        public decimal Estimated_Total { get; set; }
        public decimal Actual_Total { get; set; }
        // [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal EstCost_Total { get; set; }
        // [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal EstResaleCost_Total { get; set; }
        public bool isChekced { get; set; }
        public decimal Part_Cost { get; set; }
        public byte[] Parts_Photo { get; set; }
        public Boolean Isactive { get; set; }
        public bool? IsActivePartsDetails { get; set; }
        public int assemblypartsCount { get; set; }
        public decimal? LaborUnit { get; set; }

        //public decimal Estimated_Qty { get; set; }
        //public decimal Actual_Qty { get; set; }
    }
    public class jobassemblyparts
    {
        public List<Parts_GrandTotal> PartsGrandTotalList { get; set; }
        public string Assemblies_Name { get; set; }
        public string Assemblies_Category { get; set; }
        public string Assemblies_Description { get; set; }
        public string severity { get; set; }
        public int Part_ID { get; set; }
        public int Client_ID { get; set; }
        public string Client_Company { get; set; }
        public string Part_Number { get; set; }
        public string Part_Category { get; set; }
        public string OtherPart_Category { get; set; }
        public decimal Resale_Cost { get; set; }
        public string Purchased_From { get; set; }
        public decimal Cost { get; set; }
        public decimal Estimated_Qty { get; set; }
        public decimal Actual_Qty { get; set; }
        public decimal Expected_Total { get; set; }
        public decimal Estimated_Total { get; set; }
        public decimal Actual_Total { get; set; }
        // [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal EstCost_Total { get; set; }
        // [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal EstResaleCost_Total { get; set; }
        public bool isChekced { get; set; }
        public decimal Part_Cost { get; set; }
        public byte[] Parts_Photo { get; set; }
        public Boolean Isactive { get; set; }
        public Nullable<decimal> Multiplier { get; set; }
        public int JobAssembly_Id { get; set; }
        public decimal GrandCostTotal { get; set; }
        public decimal GrandResaleTotal { get; set; }
        public decimal GrandActualTotal { get; set; }
        public decimal PartsActualTotal { get; set; }
        public int assemblypartsCount { get; set; }
        public decimal? LaborUnit { get; set; }
        public decimal PartResaleTotal { get; set; }
        public decimal labor_EstimatedHours { get; set; }
    }
    public class Parts_DetailsInfo
    {
        public List<PartsCategoryList> partcatgoryList { get; set; }
        public IList<distributordropdown> distributorList { get; set; }
        public List<Parts_DetailsInfoList> PartsList { get; set; }
        public int Client_ID { get; set; }
        public int DistributorID { get; set; }
        [Required(ErrorMessage = "Required")]
        // [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Part_Number")]
        // [RegularExpression(@"[a-zA-Z0-9._^%$#!/()~@, - ]+", ErrorMessage = "Invalid Part_Number")]
        [MaxLength(50, ErrorMessage = "Max 50 chars length")]
        public string Part_Number { get; set; }
        public string Part_Category { get; set; }
        public string Part_CategoryCl { get; set; }
        [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        //[RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string OtherPart_Category { get; set; }
        // [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Invalid Text")]
        // [MaxLength(1000, ErrorMessage = "Max 1000 chars length")]
        public string Description { get; set; }
        // [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        [MaxLength(1000, ErrorMessage = "Max 1000 chars length")]
        public string Client_Description { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public decimal Resale_Cost { get; set; }
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        public string Purchased_From { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Updated_By { get; set; }
        public System.DateTime Updated_Date { get; set; }
        public Boolean IsActive { get; set; }
        public decimal Estimated_Qty { get; set; }
        public bool PartsRights { get; set; }
        public string UOM { get; set; }
        public int distid { get; set; }
        public string dtsCompany { get; set; }

        [RegularExpression("([0-9.]+)", ErrorMessage = "Invalid number")]
        public decimal? LaborUnit { get; set; }


    }
    public class EditParts_DetailsInfo
    {
        public List<Parts_DetailsInfoList> PartsList { get; set; }
        public string Client_Company { get; set; }
        public int Client_ID { get; set; }
        //[RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Part_Number")]
        //[RegularExpression(@"[a-zA-Z0-9._^%$#!/()~@,-]+", ErrorMessage = "Invalid Part_Number")]
        [MaxLength(50, ErrorMessage = "Max 50 chars length")]
        public string Part_Number { get; set; }
        public string Part_Category { get; set; }
        public string OtherPart_Category { get; set; }
        // [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Invalid Text")]
        [MaxLength(1000, ErrorMessage = "Max 1000 chars length")]
        public string Description { get; set; }
        // [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        [MaxLength(1000, ErrorMessage = "Max 1000 chars length")]
        public string Client_Description { get; set; }
        [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public decimal Cost { get; set; }
        [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public decimal Resale_Cost { get; set; }
        public string Purchased_From { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Updated_By { get; set; }
        public System.DateTime Updated_Date { get; set; }
        public Boolean IsActive { get; set; }

        public bool PartsRights { get; set; }
    }
    public class Labor_DetailsInfo
    {
        public List<LabourCategoryList> laborcatgorylist { get; set; }
        public int Client_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Laborer_Name { get; set; }
        public string Labor_Category { get; set; }
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        public string otherLabor_Category { get; set; }
        [MaxLength(1000, ErrorMessage = "Max 1000 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Description { get; set; }
        [MaxLength(1000, ErrorMessage = "Max 1000 chars length")]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Invalid Text")]
        public string Client_Description { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public decimal Resale_Cost { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public decimal? Rate { get; set; }
        public string Created_By { get; set; }
        //public decimal Burden { get; set; }
        public decimal? burden { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Updated_By { get; set; }
        public System.DateTime Updated_Date { get; set; }
        public Boolean IsActive { get; set; }
        public byte[] Photo { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
        public bool Gender { get; set; }
    }
    public class Labor_DetailsInfoList
    {
        public string Client_Company { get; set; }
        public int Client_ID { get; set; }
        public int Labour_ID { get; set; }
        public string Laborer_Name { get; set; }
        public string Labor_Category { get; set; }
        public string otherLabor_Category { get; set; }
        public decimal Resale_Cost { get; set; }
        public decimal? Burden { get; set; }
        public bool Gender { get; set; }
        public bool isChekced { get; set; }
        public decimal Estimated_Hour { get; set; }
        public decimal? Actual_Hours { get; set; }
        public decimal Cost { get; set; }
        public decimal? Expected_Total { get; set; }
        public decimal? Estimated_Total { get; set; }
        public decimal? Actual_Total { get; set; }
        public decimal? Rate { get; set; }
        //public string Client_Company { get; set; }
        //public int Client_ID { get; set; }
        //public string Laborer_Name { get; set; }
        //public string Labor_Category { get; set; }
        //public string otherLabor_Category { get; set; }
        //public decimal Resale_Cost { get; set; }
        //public decimal? Burden { get; set; }
        //public bool Gender { get; set; }
        //public string Description { get; set; }
        //public string Client_Description { get; set; }
        //public decimal Cost { get; set; }
        //public string Created_By { get; set; }
        //public System.DateTime Created_Date { get; set; }
        //public string Updated_By { get; set; }
        //public System.DateTime Updated_Date { get; set; }
        //public Boolean IsActive { get; set; }
        //public byte[] Photo { get; set; }
        //public HttpPostedFileBase UploadedFile { get; set; }

    }

    //These two class are created by Arunkumar.K on 12/19/2015.
    public class AssembliesLabourDetailsList
    {
        public List<Labor_GrandTotal> LaborGrand_Total { get; set; }
        public string Laborer_Name { get; set; }
        public string Assemblies_Name { get; set; }
        public string Client_Company { get; set; }
        public int Labour_ID { get; set; }
        public int Client_ID { get; set; }
        public string Labor_Category { get; set; }
        public string otherLabor_Category { get; set; }
        public decimal Resale_Cost { get; set; }
        public decimal Burden { get; set; }
        public decimal Cost { get; set; }
        // public decimal Part_Cost { get; set; }
        public decimal Estimated_Hour { get; set; }
        public decimal Actual_Hours { get; set; }
        public bool Gender { get; set; }
        public bool isChekced { get; set; }
        public byte[] Laborer_Photo { get; set; }
        public decimal Expected_Total { get; set; }
        public decimal Estimated_Total { get; set; }
        public decimal Actual_Total { get; set; }
        public Boolean IsActive { get; set; }
    }

    //public class LaborDetailsInformationList
    //{
    //    public int clientId { get; set; }
    //    public string laborName { get; set; }
    //    public string laborCategory { get; set; }
    //    public decimal Resale_Cost { get; set; }
    //    public decimal Cost { get; set; }
    //}
    public class EditLabor_DetailsInfo
    {

        public string Client_Company { get; set; }
        public int Client_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Laborer_Name { get; set; }

        public string Labor_Category { get; set; }
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        public string otherLabor_Category { get; set; }
        [MaxLength(1000, ErrorMessage = "Max 1000 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Description { get; set; }
        [MaxLength(1000, ErrorMessage = "Max 1000 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Client_Description { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public decimal Resale_Cost { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public decimal? Rate { get; set; }
        public decimal? burden { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Updated_By { get; set; }
        public System.DateTime Updated_Date { get; set; }
        public Boolean IsActive { get; set; }
        public byte[] Photo { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
        public bool Gender { get; set; }
    }
    public class Legal_DetailsInfo
    {
        public List<LegalCategoryList> LegalcatgoryList { get; set; }
        public int Client_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(2000, ErrorMessage = "Max 2000 chars length")]
        // [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Legal_Detail { get; set; }
        public string Legal_Category { get; set; }
        public string OtherLegal_Category { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Updated_By { get; set; }
        public System.DateTime Updated_Date { get; set; }
        public Boolean IsActive { get; set; }
        public int Legal_ID { get; set; }
    }
    public class Legal_DetailsInfoList
    {
        public string Client_Company { get; set; }
        public int Client_ID { get; set; }
        public string Legal_Detail { get; set; }
        public string Legal_Category { get; set; }
        public int? Legal_ID { get; set; }
        public int Job_Legal_ID { get; set; }
        public bool isChekced { get; set; }
        //public string Created_By { get; set; }
        //public System.DateTime Created_Date { get; set; }
        //public string Updated_By { get; set; }
        //public System.DateTime Updated_Date { get; set; }
        //public Boolean IsActive { get; set; }

    }
    public class EditLegal_DetailsInfo
    {
        public string Client_Company { get; set; }
        public int Client_ID { get; set; }
        [MaxLength(2000, ErrorMessage = "Max 2000 chars length")]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "Invalid Text")]
        public string Legal_Detail { get; set; }
        public string Legal_Category { get; set; }
        public int Legal_ID { get; set; }
        public string OtherLegal_Category { get; set; }
        public string Created_By { get; set; }
        public System.DateTime Created_Date { get; set; }
        public string Updated_By { get; set; }
        public System.DateTime Updated_Date { get; set; }
        public Boolean IsActive { get; set; }

    }

    public class JobAssignInfo
    {
        public string AssignedUser { get; set; }
        public string UserColor { get; set; }
    }
    public class Job_MasterInfoList
    {
        public List<JobAssignInfo> JobAssignedList { get; set; }
        public int Job_ID { get; set; }
        public string JobDisplayId { get; set; }
        public string Job_Description { get; set; }
        public string Created_By { get; set; }
        public string Job_Status { get; set; }
        public string Job_AssignedUser { get; set; }
        public string UserColor { get; set; }
        public bool TempPole { get; set; }
        public string Client_Name { get; set; }
        public string Client_Address { get; set; }
        public string Client_City { get; set; }
        public string Work_Address { get; set; }
        public string Work_City { get; set; }
        public string Job_Number { get; set; }
        public DateTime PriceUpdatedDate { get; set; }
        public bool IsPriceUpdate { get; set; }

    }
    public class JobCalendarinfo
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public int Client_Id { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string sDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string eDate { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public Nullable<bool> FullDay { get; set; }
        public string AssignTo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string UserColor { get; set; }
        public string Totalhours { get; set; }
        public string stTime { get; set; }
        public string etTime { get; set; }



    }
    public class JobPartsInfo
    {
        public List<Parts_DetailsInfoList> NewPartsListInJob { get; set; }

        public decimal Parts_GrandExpTotal { get; set; }
        public decimal Parts_GrandEstTotal { get; set; }
        public decimal Parts_GrandActTotal { get; set; }
    }
    public class JobLaborInfo
    {
        public List<Labor_DetailsInfoList> NewLabourListInJob { get; set; }

        public decimal Labor_GrandExpTotal { get; set; }
        public decimal Labor_GrandEstTotal { get; set; }
        public decimal Labor_GrandActTotal { get; set; }
    }

    public class SOWInfoList
    {
        public int id { set; get; }
        public string Doing_What { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }

    }
    public class MyUserList
    {
        public string name { get; set; }
        public string User_ID { get; set; }

    }
    public class Job_MasterInfo
    {
        // public IList<AssembliesParts_DetailsInfoList> PartsListData { get; set; }
        // public IList<AssembliesLabourDetailsList> LabourListData { get; set; }
        public List<Parts_DetailsInfoList> NewPartsListInJob { get; set; }
        public List<Labor_DetailsInfoList> NewLabourListInJob { get; set; }
        public List<Legal_DetailsInfoList> NewLegalListInJob { get; set; }
        public List<Job_DJE_VQ_Detailsinfo> DJEVQDetails { get; set; }
        public List<Assembly_MasterInfo> NewJobAssemblyPartsList { get; set; }
        public List<jobassemblyparts> Jobsassemblyparts { get; set; }
        public List<Assembly_MasterInfo> NewJobAssemblyLaborList { get; set; }
        public List<Assembly_MasterInfo> NewJobAssemblyList { get; set; }
        public List<HttpPostedFileBase> AttachmentList { get; set; }
        public Job_EstimationDetailsinfo Job_EstimationDetails { get; set; }
        public IList<MyUserList> Userlist { get; set; }
        public bool TempPole { get; set; }
        public string Labor_Category { get; set; }
        public string Client_Company { get; set; }
        public int Client_Id { get; set; }
        public int Job_ID { get; set; }
        public string JobDisplayId { get; set; }
        public string Job_Description { get; set; }
        public string Client_Name { get; set; }
        public string Job_AssignedUser { get; set; }
        public string Other_ClientName { get; set; }
        public bool FullDay { get; set; }
        public string Assemblies_Name { get; set; }
        public string Assemblies_Description { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string Job_Number { get; set; }

        // [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        // [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Client_Address { get; set; }
        //  [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        //  [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Client_Address2 { get; set; }
        // [MaxLength(70, ErrorMessage = "Max 70 chars length")]
        // [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Client_City { get; set; }
        // [MaxLength(70, ErrorMessage = "Max 70 chars length")]
        // [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Client_State { get; set; }
        //[MaxLength(10, ErrorMessage = "Max 10 chars length")]
        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Zip code")]
        public string Client_ZipCode { get; set; }
        // [MaxLength(50, ErrorMessage = "Max 25 chars length")]
        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Phone number")]
        // [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Invalid Phone Number!")]
        public string Client_Phone { get; set; }
        // [MaxLength(50, ErrorMessage = "Max 50 chars length")]
        // [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Phone number")]
        //[RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Invalid Phone Number!")]
        public string Client_Mobile { get; set; }
        public string Client_ContactPerson { get; set; }
        // [MaxLength(25, ErrorMessage = "Max 25 chars length")]
        // [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Fax")]
        // [RegularExpression(@"^([0-9\(\)\/\+ \-]*)$", ErrorMessage = "Invalid Fax !")]
        public string Client_Fax { get; set; }
        [MaxLength(50, ErrorMessage = "Max 50 chars length")]
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Client_Email { get; set; }
        public string Previous_Location { get; set; }
        public string Other_Previous_Location { get; set; }
        // [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        // [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Work_Address { get; set; }
        // [MaxLength(70, ErrorMessage = "Max 70 chars length")]
        // [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Work_Address2 { get; set; }
        //  [MaxLength(70, ErrorMessage = "Max 70 chars length")]
        //  [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Work_City { get; set; }
        //[MaxLength(70, ErrorMessage = "Max 70 chars length")]
        // [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Work_State { get; set; }
        // [MaxLength(10, ErrorMessage = "Max 10 chars length")]
        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Zip code")]
        public string Work_ZipCode { get; set; }
        public string Directions_To { get; set; }
        public string Doing_What { get; set; }
        public string Job_Status { get; set; }
        //  public DateTime Start_Date { get; set; }
        //public DateTime End_Date { get; set; }

        //  [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Job_Mileage_Estimated { get; set; }
        //  [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Job_Mileage_Actual { get; set; }
        //  [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Job_Mileage_Cost { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Job_Mileage_ExpTotal { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Job_Mileage_EstTotal { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Job_Mileage_ActTotal { get; set; }

        //  [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> EstimateValue_SubTotal { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> EstimateValue_ExpTotal { get; set; }
        //  [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> EstimateValue_Overhead_Percentage { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> EstimateValue_Overhead_EstTotal { get; set; }
        //  [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> EstimateValue_Overhead_ExpTotal { get; set; }
        //[RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> EstimateValue_Profit_Percentage { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> EstimateValue_Profit_EstTotal { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Estimate_Override { get; set; }

        public Nullable<decimal> SalesTax { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> SalesTax_Perc { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Job_ExpenseTotal { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Job_EstimatedTotal { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Job_ActualTotal { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Profit_Loss_Total { get; set; }
        // [RegularExpression("([0-9.,]+)", ErrorMessage = "Invalid number")]
        public Nullable<decimal> Profit_Loss_Perc { get; set; }
        public Nullable<bool> Isactive { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Updated_By { get; set; }
        public Nullable<System.DateTime> Updated_Date { get; set; }
        public decimal GrandCostTotal { get; set; }
        public decimal GrandResaleTotal { get; set; }
        public decimal GrandActualTotal { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
        public byte[] Job_Attachment { get; set; }
        public string Attachement_Name { get; set; }

        public decimal? Parts_GrandExpTotal { get; set; }
        public decimal? Parts_GrandEstTotal { get; set; }
        public decimal? Parts_GrandActTotal { get; set; }

        public decimal? Labor_GrandExpTotal { get; set; }
        public decimal? Labor_GrandEstTotal { get; set; }
        public decimal? Labor_GrandActTotal { get; set; }
        public string UserColor { get; set; }
        public string Description { get; set; }

    }
    public class Viewfile
    {
        public string filetype { get; set; }
        public System.Drawing.Image imgdata { get; set; }
        public byte[] imagearray { get; set; }
        public int JobID { get; set; }
        public string AttachmentName { get; set; }
    }
    public class Assembly_MasterInfoList
    {
        public long ID { get; set; }
        ///public string Name { get; set; }
        public int Client_ID { get; set; }
        public int Assemblies_Id { get; set; }
        public int Distributor_Id { get; set; }
        public string Assemblies_Name { get; set; }
        public string Assemblies_Description { get; set; }
        public string Assemblies_Category { get; set; }
        public string Type { get; set; }
        public string severity { get; set; }
        public bool isChekced { get; set; }
        public decimal Est_Qty { get; set; }
        public int Act_Qty { get; set; }
        public decimal Est_Cost_Total { get; set; }
        public decimal Est_Resale_Total { get; set; }
        public decimal Actual_Total { get; set; }

        public decimal Parts_GrandExp_Total { get; set; }
        public decimal Parts_GrandEst_Total { get; set; }
        public decimal Parts_GrandAct_Total { get; set; }

        public decimal Labor_GrandExp_Total { get; set; }
        public decimal Labor_GrandEst_Total { get; set; }
        public decimal Labor_GrandAct_Total { get; set; }
        public int assemblypartsCount { get; set; }

        public string distributorName { get; set; }

        public bool IsActive { get; set; }

        public bool Iszero { get; set; }
        public decimal labor_cost { get; set; }
        public decimal labor_ResaleCost { get; set; }
        public DateTime Updated_Date { get; set; }

    }
    public class Assembly_DescriptionList
    {
        public string Assemblies_Description { get; set; }
    }
    //This class is created by Arunkumar.K on 12/16/2015.
    public class Assembly_MasterInfo
    {
        public IList<Assembly_MasterInfoList> asList { get; set; }
        public IList<AssembliesParts_DetailsInfoList> PartsListData { get; set; }
        public int AssebliesId { get; set; }
        // public IList<AssembliesLabourDetailsList> LabourListData { get; set; }
        public AssemblyPartsModel AsDetails { get; set; }
        public List<jobassemblyparts> Jobsassemblyparts { get; set; }
        public int DistributorID { get; set; }
        public string Name { get; set; }
        public int Client_ID { get; set; }
        [Required(ErrorMessage = "Required")]
        [MaxLength(100, ErrorMessage = "Max 100 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Assemblies_Name { get; set; }
        [MaxLength(2000, ErrorMessage = "Max 2000 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string Assemblies_Description { get; set; }
        public int AssemblyId { get; set; }
        public string Assemblies_Category { get; set; }
        [Required(ErrorMessage = "Required")]
        public string severity { get; set; }
        [MaxLength(500, ErrorMessage = "Max 500 chars length")]
        [RegularExpression(@"^(?=.*[a-zA-Z]).+$", ErrorMessage = "Invalid Text")]
        public string OtherAssemblies_Category { get; set; }
        public int Labour_ID { get; set; }
        public string Laborer_Name { get; set; }
        public string Laborer_Phone { get; set; }
        public byte[] Laborer_Photo { get; set; }
        // public decimal Lobor_Cost { get; set; }
        public decimal Burden { get; set; }
        public decimal Lobor_Resale { get; set; }
        public decimal Estimated_Hour { get; set; }
        public decimal LaborEst_CostTotal { get; set; }
        public decimal LaborEst_ResaleTotal { get; set; }
        public decimal PartCostTotal { get; set; }
        public decimal Estimated_Qty_Total { get; set; }
        public decimal PartResaleTotal { get; set; }
        public decimal GrandCostTotal { get; set; }
        public decimal GrandResaleTotal { get; set; }
        public decimal Actual_Hours { get; set; }
        public decimal Expected_Total { get; set; }
        public decimal Estimated_Total { get; set; }
        public decimal Actual_Total { get; set; }
        public bool Isactive { get; set; }
        public string Created_By { get; set; }
        public System.DateTime? Created_Date { get; set; }
        public string Updated_By { get; set; }
        public System.DateTime? Updated_Date { get; set; }
        public int Part_ID { get; set; }
        public string Part_Number { get; set; }
        public string PartDescription { get; set; }
        public decimal Part_Cost { get; set; }
        public byte[] Parts_Photo { get; set; }
        public decimal Estimated_Qty { get; set; }
        public decimal Actual_Qty { get; set; }
        public decimal Part_Resale { get; set; }
        public bool isChekced { get; set; }
        public decimal labor_cost { get; set; }
        public decimal labor_ResaleCost { get; set; }
        public decimal labor_EstimatedHours { get; set; }
        public int labour_actual_hours { get; set; }
        public decimal labour_actual_total { get; set; }
        public decimal Labor_Actual_Qty { get; set; }
        public decimal Est_Cost_Total { get; set; }
        public decimal Est_Resale_Total { get; set; }
        public decimal Est_Qty_Total { get; set; }
        public decimal Multiplier { get; set; }
        public int JobAssembly_Id { get; set; }
        public decimal PartsActualTotal { get; set; }
        public decimal GrandActualTotal { get; set; }
        public int assemblypartsCount { get; set; }
        public string togid { get; set; }
        public bool Active { get; set; }
    }
    public class Assembly_PartsDetailsInfoList
    {
        public int Client_ID { get; set; }
        public string Client_Company { get; set; }
        public string Part_Number { get; set; }
        public string Part_Category { get; set; }
        public string OtherPart_Category { get; set; }
        public decimal Resale_Cost { get; set; }
        public string Purchased_From { get; set; }
        public decimal Cost { get; set; }
    }
    public class ListPart
    {
        public int Clientid { get; set; }
        public string Partnumber { get; set; }
    }
    public class FileUpload
    {
        // public IEnumerable<HttpPostedFileBase> files { get; set; }
        public bool ischecked { get; set; }
        public string fileName { get; set; }
    }

    public class AssemblyPartsModel
    {
        public List<ElectricEase.Models.AssembliesParts_DetailsInfoList> partslist { get; set; }
        public ElectricEase.Models.Assembly_MasterInfo assmeblymasterinfo { get; set; }
        //public Assembly_MasterInfo ass = new Assembly_MasterInfo();
    }
    public class clients
    {
        public string name { get; set; }
        public string Client_Address { get; set; }
        public string Client_Address2 { get; set; }
        public string Client_City { get; set; }
        public string Client_State { get; set; }
        public string Client_Zip { get; set; }
        public string Client_Phone { get; set; }
        public string Client_Mobile { get; set; }
        public string Client_ContactPerson { get; set; }
        public string Client_Fax { get; set; }
        public string Client_Email { get; set; }
        public string Work_Address { get; set; }
        public string Work_Address2 { get; set; }
        public string Work_City { get; set; }
        public string Work_State { get; set; }
        public string Work_Location { get; set; }
        public string Work_ZipCode { get; set; }
    }
    public class Report_Jobs_Parts
    {
        public string Part_Number { get; set; }
        public decimal Estimated_Qty { get; set; }
        public string Category { get; set; }
        public string Part_Desc { get; set; }
        //public int Count { get; set; }
    }
    public class Reports_Attachments
    {
        public string Attachment { get; set; }
    }
    public class Job_DJE_VQ_Detailsinfo
    {
        public int Status_ID { get; set; }
        public int Client_ID { get; set; }
        public int Job_ID { get; set; }
        public string Expense { get; set; }
        public string Vendor_Name { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<decimal> Profit { get; set; }
        public string Job_DJE_VQ_Status { get; set; }
        public Nullable<decimal> Resale_Total { get; set; }
        public Nullable<decimal> Job_DJETotal { get; set; }
        public Nullable<decimal> Job_VQTotal { get; set; }
        public bool IsActive { get; set; }

    }
    public class Job_EstimationDetailsinfo
    {
        public int Client_ID { get; set; }
        public int Job_ID { get; set; }
        public int Job_EstimationID { get; set; }
        public Nullable<int> Selected_Estimation_Type { get; set; }
        public Nullable<decimal> Job_AssemblyTotal { get; set; }
        public Nullable<decimal> Estimation1_AssemblyTax { get; set; }
        public Nullable<decimal> Estimation1_AssemblyTotal { get; set; }
        public Nullable<decimal> Estimation2_AssemblyProfit { get; set; }
        public Nullable<decimal> Estimation2_AssemblySubTotal { get; set; }
        public Nullable<decimal> Estimation2_AssemblyTax { get; set; }
        public Nullable<decimal> Estimation2_AssemblyTotal { get; set; }
        public Nullable<decimal> Job_LaborTotal { get; set; }
        public Nullable<decimal> Estimation1_LaborTax { get; set; }
        public Nullable<decimal> Estimation1_laborTotal { get; set; }
        public Nullable<decimal> Estimation2_LaborProft { get; set; }
        public Nullable<decimal> Estimate2_LaborSubTotal { get; set; }
        public Nullable<decimal> Estimate2_LaborTax { get; set; }
        public Nullable<decimal> Estimate2_LaborTotal { get; set; }
        public Nullable<decimal> Job_PartsTotal { get; set; }
        public Nullable<decimal> Estimation1_PartsTax { get; set; }
        public Nullable<decimal> Estimation1_PartsTotal { get; set; }
        public Nullable<decimal> Estimation2_PartsProfit { get; set; }
        public Nullable<decimal> Estimation2_PartsSubTotal { get; set; }
        public Nullable<decimal> Estimation2_PartsTax { get; set; }
        public Nullable<decimal> Estimation2_PartsTotal { get; set; }
        public Nullable<decimal> Job_DJETotal { get; set; }
        public Nullable<decimal> Estimation1_DJETax { get; set; }
        public Nullable<decimal> Estimation1_DJETotal { get; set; }
        public Nullable<decimal> Estimation2_DJEProfit { get; set; }
        public Nullable<decimal> Estimation2_DJESubTotal { get; set; }
        public Nullable<decimal> Estimation2_DJETax { get; set; }
        public Nullable<decimal> Estimation2_DJETotal { get; set; }
        public Nullable<decimal> Job_VQTotal { get; set; }
        public Nullable<decimal> Estimation1_VQTax { get; set; }
        public Nullable<decimal> Estimation1_VQTotal { get; set; }
        public Nullable<decimal> Estimation2_VQProfit { get; set; }
        public Nullable<decimal> Estimation2_VQSubtotal { get; set; }
        public Nullable<decimal> Estimation2_VQTax { get; set; }
        public Nullable<decimal> Estimation2_VQTotal { get; set; }
        public Nullable<decimal> Estimation1_GrandTotal { get; set; }
        public Nullable<decimal> Estimation1_Total { get; set; }
        public Nullable<decimal> Estimation2_GrandTotal { get; set; }
        public Nullable<decimal> Estimation2_SubTotal { get; set; }
        public Nullable<decimal> Estimation2_Total { get; set; }
        public Nullable<decimal> Estimate3_Override { get; set; }
        public Nullable<decimal> Estimate4_Subtotal { get; set; }
        public Nullable<decimal> Estimate4_Tax { get; set; }
        public Nullable<decimal> Estimate4_Total { get; set; }
        public string Estimate4_Data { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
    public class contactUs
    {
        public string message { get; set; }
    }

    public class LoginResult
    {
        public bool LoginStatus { get; set; }
        public string ClientCompany { get; set; }
        public int ClientID { get; set; }
        public byte[] ClientLogo { get; set; }
        public byte[] Photo { get; set; }
    }

    public class JobDescriptionReportResult
    {
        public int JobId { get; set; }
        public string ClientName { get; set; }
        public string JobNumber { get; set; }
        public string JobDescription { get; set; }
        public string Address { get; set; }
        public string city { get; set; }
        public string Job_Status { get; set; }


    }

    public class GetjobReportPdf
    {
        public int Job_id { get; set; }
        public string Job_Number { get; set; }
        public string CompanyAddress { get; set; }
        public string ClientAddress { get; set; }
        public string WOrkAddress { get; set; }
        public string Email { get; set; }
        public string Dirctionto { get; set; }
        public string JobDiscription { get; set; }
        public string scope_of_work { get; set; }
        public List<Parts_DetailsInfoList> jobPartsData { get; set; }
        public List<Labor_DetailsInfoList> jobLaborData { get; set; }
        public List<AssembliesParts_DetailsInfoList> jobAssemblyPartsData { get; set; }
        public List<Legal_DetailsInfoList> LegalData { get; set; }

        public string PdfURL { get; set; }
    }
    public class Distributor
    {
        public int ID { get; set; }
        public string Company { get; set; }
        public int No_Of_Users { get; set; }
        public string Company_Url { get; set; }
        public DateTime Expiry_Date { get; set; }
        public string Email { get; set; }
        public string CompanyLogo { get; set; }
        public bool IsActive { get; set; }
        public string Logo { get; set; }
        public string LExpiry_Date { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class distributordropdown
    {
        public int value { get; set; }
        public string Name { get; set; }
    }
    public class Standaredclient
    {
        public int value { get; set; }
        public string Name { get; set; }
    }

    public class DisbutorIds
    {
        public int ID { get; set; }
    }
    public class ClienIds
    {
        public int ID { get; set; }
    }
    public class AssemblyIds
    {
        public int ID { get; set; }
    }
    public class selectedAssembilesIds
    {
        public List<DisbutorIds> DisbutorId { get; set; }
        public List<ClienIds> ClienId { get; set; }
        public List<AssemblyIds> AssemblyId { get; set; }
    }
    //public class  AssembliesNames
    //{
    //   public int AssemblyId { get; set; }
    //    public string AssemblyName { get; set; }
    //}
    public class PartsNames
    {
        public string assemblyName { get; set; }
        public string Partnumber { get; set; }
        public string Category { get; set; }
        public string Cost { get; set; }
        public string Resale { get; set; }
        public string LaborUnit { get; set; }
        public string Comments { get; set; }

    }
    public class AssemblyNames
    {
        public string Name { get; set; }
    }

    public class DistributorAssemblies
    {
        public int DisbutorId { get; set; }
        public List<ClienIds> ClienId { get; set; }
        public List<AssemblyNames> selectedAssembiles { get; set; }
    }

    public class TimeSheetMastr
    {
        public long TimeSheetID { get; set; }
        public string TimeSheetCode { get; set; }
        public Nullable<int> Client_ID { get; set; }
        public string User_ID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> TotalHours { get; set; }
        public Nullable<decimal> OTHours { get; set; }
        public string ClientName { get; set; }
        public string ReportingManager { get; set; }
        public string Comments { get; set; }
    }

    public class exportAssemblyAndParts
    {
        public string PartCategory { get; set; }
        public string PartNumber { get; set; }
        public string PartDescription { get; set; }
        public decimal EstimatedQty { get; set; }
    }
}

