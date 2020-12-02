using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Diagnostics;


namespace ViewModels.EquipmentIn
{




    public partial class EquipmentInViewModel
    {

        public EquipmentInViewModel()
        {

            Detail = new List<EquipmentInDetailViewModel>();


            //SelectListItem selectListItem = new SelectListItem();

            //selectListItem.Text = "Please Select";
            //selectListItem.Value = "0";

            //// IEnumerable<SelectListItem> iESelectListItems = (IEnumerable<SelectListItem>)selectListItem;

            //List<SelectListItem> list = new List<SelectListItem>();

            //list.Add(selectListItem);

            // CustomerName = new System.Web.Mvc.SelectList(list.ToList(), "Value", "Text");



        }


        //[Required(AllowEmptyStrings = false, ErrorMessage = "Issue description cannot be empty")]
        //[MaxLength(500, ErrorMessage = "Issue decription cannot be longer than 500 charecters")]
        //[DisplayName("Issue description")]
        //[DataType(DataType.MultilineText)]
        //public string IssueDescription { get; set; }

        [MaxLength(255, ErrorMessage = "Customers important instructions cannot be longer than 255 charecters")]
        [Required(ErrorMessage =
            "Customers important instructions cannot be empty (Enter 'NIL' in case of no instructions)")]
        [DisplayName("Customers important instructions")]
        [DataType(DataType.MultilineText)]
        public string CustomersImportantInstructions { get; set; }



        //public string CustomerCode { get; set; }






        //public string CustomerName { get; set; }

        //public string CustomerBillingAddress { get; set; }

        //public string CustomerDelivaryAddress { get; set; }

        //public string EquipmentTypeCode { get; set; }

        [DisplayName("In warranty")] public bool InWarranty { get; set; }

        [MaxLength(50, ErrorMessage = "Warranty job ID cannot be longer than 50 charecters")]
        [DisplayName("Warranty job ID")]
        public string WarrantyJobID { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Internal ID cannot be empty")]
        [MaxLength(250, ErrorMessage = "Internal ID cannot be longer than 250 charecters")]
        [DisplayName("Internal ID")]
        public string InternalID { get; set; }

        [DisplayName("Public Tag ID")]
        [MaxLength(250, ErrorMessage = "Public Tag ID cannot be longer than 250 charecters")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Public Tag cannot be empty")]
        public string PublicTagID { get; set; }

        [DisplayName("Tag ID")]
        [MaxLength(250, ErrorMessage = "Tag ID cannot be longer than 250 charecters")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Tag ID cannot be empty")]
        public string TagID { get; set; }



        //[Required(AllowEmptyStrings = false, ErrorMessage = "Financial year cannot be empty")]
        //[MaxLength(5, ErrorMessage = "Financial year cannot be longer than 5 charecters")]
        //[DisplayName("Financial Year")]
        //public string FinYear { get; set; }



        [MaxLength(250, ErrorMessage = "Manufacturers serial number cannot be longer than 250 charecters")]
        [DisplayName("Manufacturers Serial Number")]
        //[CustomValidation(typeof(EquipmentInCustomValidator),"ValidateManufacturerSerialNumber")] //added by soni on 17th july 2020
        public string ProductMfrSerialNumber { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Equipment manufacturer name cannot be empty")]
        [MaxLength(250, ErrorMessage = "Equipment manufacturer name cannot be longer than 250 charecters")]
        [DisplayName("Equipment Manufacturers Name")]
        //[CustomValidation(typeof(EquipmentInCustomValidator),"ValidateProductMfrName")] //modified by aishwarya on 10th oct 2019
        public string ProductMfrName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Equipment model name cannot be empty")]
        [MaxLength(250, ErrorMessage = "Equipment model name cannot be longer than 250 charecters")]
        [DisplayName("Equipment Model Name")] 
        //[CustomValidation(typeof(EquipmentInCustomValidator),"ValidateProductModelName")] //modified by aishwarya on 10th oct 2019
        public string ProductModelName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Equipment model number cannot be empty")]
        [MaxLength(250, ErrorMessage = "Equipment model number cannot be longer than 250 charecters")]
        [DisplayName("Equipment Model Number")]
        public string ProductModelNumber { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Configuration cannot be empty (Enter 'ST' or 'STANDARD' in case of standard configuration")]
        [MaxLength(1000, ErrorMessage = "Configuration cannot be longer than 1000 charecters")]
        [DataType(DataType.MultilineText)]
        public string Configuration { get; set; }


        [Required(AllowEmptyStrings = false,
            ErrorMessage = "Accessories list cannot be empty (Enter 'NIL' in case of no accessories")]
        [MaxLength(500, ErrorMessage = "Accessories list cannot be longer than 500 charecters")]
        [DisplayName("Accessories List")]
        [DataType(DataType.MultilineText)]
        public string AccessoriesList { get; set; }

        //[DisplayName("Needs diagnosis")]
        //public bool NeedsDiagnosis { get; set; }


        //[DisplayName("Needs Estimate")]
        //public bool NeedsEstimate { get; set; }

        [Required(ErrorMessage =
            "Equipment physical damage(s) cannot be empty (Enter 'None' or 'NA' in case of no physical damages)")]
        [DisplayName("Equipment Physical Damage(s)")]
        [DataType(DataType.MultilineText)]
        public string PhysicalDamages { get; set; }

        [Required(ErrorMessage =
            "Equipment other damage(s) cannot be empty (Enter 'None' or 'NA' in case of no other damages)")]
        [DisplayName("Equipment Other Damage(s)")]
        [DataType(DataType.MultilineText)]
        public string OtherDamages { get; set; }

        [Required(ErrorMessage = "Checked by personnel name cannot be empty")]
        [DisplayName("Checked by personnel name")]
        //[CustomValidation(typeof(EquipmentInCustomValidator), "ValidateCheckedBy")] //modified by aishwarya on 10th oct 2019]
        public string CheckedBy { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Internal instructions cannot be empty")]
        [MaxLength(500, ErrorMessage = "Internal instructions cannot be longer than 500 charecters")]
        [DisplayName("Internal instructions")]
        [DataType(DataType.MultilineText)]
        public string EquipINInternalInstructions { get; set; }


        [Required(ErrorMessage = "Company Code cannot be empty")]
        [StringLength(50)]
        public string CompanyCode { get; set; }


        [DisplayName("Equipment In Id")]
        [Required(ErrorMessage = "Equipment In Id cannot be empty")]
        [StringLength(50)]
        public string EquipmentInId { get; set; }

        //[DataType(DataType.DateTime)]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy hh:mm:ss tt}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Equipment In date-time cannot be empty")]
        //[DisplayName("Equipment In Date-Time")]
        //[DateRangeMinMax(ErrorMessage = "Date is out of range")]
        //[DateGreaterThanCurrent(ErrorMessage = "Date greater than todays date")]
        [Required(ErrorMessage = "Arrival Date Time Cannot be empty")]
        public DateTime ArrivalDateTime { get; set; }

        [MaxLength(50, ErrorMessage = "Delivered by - personnel name cannot be longer than 50 charecters")]
        [DisplayName("Delivered by - Personnel Name")]
        //[CustomValidation(typeof(EquipmentInCustomValidator), "ValidateDeliveredByPersonnelName")]//modified by aishwarya on 10th oct 2019
        public string DeliveredByPersonnelName { get; set; }

        [MaxLength(15, ErrorMessage = "Delivered by - personnel phone number cannot be longer than 15 charecters")]
        [DisplayName("Delivered by - Personnel phone number")]
        public string DeliveredByPersonPhoneNumber { get; set; }

        [MaxLength(150, ErrorMessage = "Work order number cannot be longer than 150 charecters")]
        [DisplayName("Work order number")]
        public string WorkOrderNumber { get; set; }

        [DisplayName("Work order date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //[DateRangeMinMax(ErrorMessage = "Date is out of range")]
        //[DateGreaterThanCurrent(ErrorMessage = "Date greater than todays date")]
        public DateTime? WorkOrderDate { get; set; }

        [MaxLength(50, ErrorMessage = "Work order authorized by cannot be longer than 50 charecters")]
        [DisplayName("Work order authorized by")]
        public string WorkOrderAuthorizedBy { get; set; }

        [DisplayName("Is Public")] public bool IsPublic { get; set; }

        [MaxLength(250, ErrorMessage = "Customer text cannot be longer than 250 charecters")]
        [DisplayName("Customer text")]
        public string CustomerText { get; set; }

        [DisplayName("Is cancelled")] public bool IsCancelled { get; set; }

        [MaxLength(50, ErrorMessage = "Cancelled by - name cannot be longer than 50 charecters")]
        [DisplayName("Cancelled by - Name")]
        //[CustomValidation(typeof(EquipmentInCustomValidator), "ValidateCancelledBy")] //modified by aishwarya on 10th oct 2019]
        public string CancelledBy { get; set; }

        [MaxLength(150, ErrorMessage = "Cancelled reason cannot be longer than 150 charecters")]
        [DisplayName("Cancelled Reason")]
        public string CancelledReason { get; set; }

        public DateTime? CancelledDateTime { get; set; }

        [MaxLength(50, ErrorMessage = "Equipment IN by personnel name cannot be longer than 50 charecters")]
        [DisplayName("Equipment IN by personnel name")]
        //[CustomValidation(typeof(EquipmentInCustomValidator), "ValidateEquipmentINByName")]//modified by aishwarya on 10th oct 2019
        public string EquipmentINByName { get; set; }


        [MaxLength(15, ErrorMessage = "Rack ID cannot be longer than 15 charecters")]
        [DisplayName("Assigned Rack ID")]
        public string AssignedRackID { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Point of contact - personnel name cannot be longer than 50 charecters")]
        [DisplayName("Point of Contact - Personnel name")]
        //[CustomValidation(typeof(EquipmentInCustomValidator), "ValidatePointOfContactPersonnelName")] //modified by aishwarya on 10th oct 2019
        public string PointOfContactPersonnelName { get; set; }

        [Required(ErrorMessage = "Please select customer name")]
        [DisplayName("Customer Name / Identifier")]
        //[CustomValidation(typeof(EquipmentInCustomValidator), "ValidateCustomerName")] //modified by aishwarya on 10th oct 2019
        public string CustomerName { get; set; }


        [DisplayName("Customer Address")]
        [DataType(DataType.MultilineText)]
        public string CustomerBillingAddress { get; set; }

        [Required] [StringLength(50)] public string EnteredByPersonnelID { get; set; }
        
        public IList<EquipmentInDetailViewModel> Detail { get; set; }
        
    }

    public class EquipmentInDetailViewModel
    {


        [Required(ErrorMessage = "Company Code cannot be empty")]
        [StringLength(50)]
        public string CompanyCode { get; set; }


        [DisplayName("Equipment In Id")]
        [Required(ErrorMessage = "Equipment In Id cannot be empty")]
        [StringLength(50)]
        public string EquipmentInId { get; set; }


        [DisplayName("Sl. No.")] [Required] public int SlNo { get; set; }

        [Key, Column(Order = 3)]
        [DisplayName("Issue Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Issue description cannot be empty")]
        [StringLength(500, ErrorMessage = "Issue description cannot be longer than 500 charecters")]
        //[DataType(DataType.MultilineText)]
        //[CustomValidation(typeof(EquipmentInCustomValidator), "ValidateIssueDescription")] //modified by aishwarya on 30th oct 2019
        public string IssueDescription { get; set; }

        [DisplayName("Reported By")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Reported By cannot be empty")]
        [StringLength(10, ErrorMessage = "Reported By cannot be longer than 10 charecters")]
        public string ReportedBy { get; set; }
        
    }
}