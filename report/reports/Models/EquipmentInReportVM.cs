using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewModels.EquipmentIn;

namespace ReportViewModels.TRANSACTIONS
{
    public class EquipmentInReportViewModel : EquipmentInViewModel
    {






        public static IList<EquipmentInReportViewModel> GetData(string CompanyCode, DateTime StartDate,
            DateTime EndDate)
        {
            try
            {
                IList<EquipmentInReportViewModel> equipmentInViewModel = new List<EquipmentInReportViewModel>();


                //EquipmentInReportViewModel equipmentInViewModel1 = new EquipmentInReportViewModel() { AccessoriesList = "", ArrivalDateTime = Convert.ToDateTime("01-01-2018"), AssignedRackID = "1a", CancelledBy = "", CancelledDateTime = Convert.ToDateTime("6-11-2018"), CancelledReason = "", CheckedBy = "Arun", CompanyCode = "", CustomerBillingAddress = "qpq", CustomerName = "Lohith", CustomersImportantInstructions = "", CustomerText = "", DeliveredByPersonnelName = "", DeliveredByPersonPhoneNumber = "", EquipINInternalInstructions = "", EquipmentINByName = "Suhas", EquipmentInId = "1",  InternalID = "111", PublicTagID = "11", InWarranty = true, IsCancelled = true, IsPublic = false, OtherDamages = "", PhysicalDamages = "", ProductMfrName = "Lenovo", ProductMfrSerialNumber = "1EE-01", ProductModelName = "Yoga 730", Configuration = "Intel i5-8250U, 8GB DDR4 RAM, 256GB PCIe SSD, Thunderbolt, Fingerprint Reader, Backlit Keyboard", TagID = "1q1", WarrantyJobID = "", WorkOrderAuthorizedBy = "", WorkOrderDate = Convert.ToDateTime("6-11-2018"), WorkOrderNumber = "" };
                //EquipmentInDetailViewModel equipmentInDetailViewModel1 = new EquipmentInDetailViewModel() { IssueDescription = "No Screen Display", ReportedBy = "Personnel" };
                //equipmentInViewModel1.Detail.Add(equipmentInDetailViewModel1);

                //equipmentInViewModel.Add(equipmentInViewModel1);

                //EquipmentInReportViewModel equipmentInViewModel2 = new EquipmentInReportViewModel() { AccessoriesList = "", ArrivalDateTime = Convert.ToDateTime("01-01-2018"), AssignedRackID = "2a", CancelledBy = "", CancelledDateTime = Convert.ToDateTime("6-11-2018"), CancelledReason = "", CheckedBy = "Avinash", CompanyCode = "", CustomerBillingAddress = "ala", CustomerName = "Praveen", CustomersImportantInstructions = "", CustomerText = "", DeliveredByPersonnelName = "", DeliveredByPersonPhoneNumber = "", EquipINInternalInstructions = "", EquipmentINByName = "Sharan", EquipmentInId = "2",  InternalID = "222", PublicTagID = "22", InWarranty = true, IsCancelled = false, IsPublic = false, OtherDamages = "", PhysicalDamages = "", ProductMfrName = "Dell", ProductMfrSerialNumber = "UI3221-NI0", ProductModelName = "Vostro 3578", Configuration = "Intel Core i5 8th Gen,Quad Core 1.6 GHz Clock Speed,8 GB DDR4 RAM,1 TB Hard Disk,2 GB AMD Graphics Card,15.6 inches, 1920 x 1080 pixels,Windows 10 OS,1 Year Warranty", TagID = "1e1", WarrantyJobID = "", WorkOrderAuthorizedBy = "", WorkOrderDate = Convert.ToDateTime("6-11-2018"), WorkOrderNumber = "" };
                //EquipmentInDetailViewModel equipmentInDetailViewModel2 = new EquipmentInDetailViewModel() { IssueDescription = "System Hanging", ReportedBy = "Personnel" };
                //equipmentInViewModel2.Detail.Add(equipmentInDetailViewModel2);

                //equipmentInViewModel.Add(equipmentInViewModel2);

                //EquipmentInReportViewModel equipmentInViewModel3 = new EquipmentInReportViewModel() { AccessoriesList = "", ArrivalDateTime = Convert.ToDateTime("01-02-2018"), AssignedRackID = "3a", CancelledBy = "", CancelledDateTime = Convert.ToDateTime("6-11-2018"), CancelledReason = "", CheckedBy = "Pradeep", CompanyCode = "", CustomerBillingAddress = "zmz", CustomerName = "Karan", CustomersImportantInstructions = "", CustomerText = "", DeliveredByPersonnelName = "", DeliveredByPersonPhoneNumber = "", EquipINInternalInstructions = "", EquipmentINByName = "Karthik", EquipmentInId = "3",  InternalID = "333", PublicTagID = "33", InWarranty = true, IsCancelled = true, IsPublic = false, OtherDamages = "", PhysicalDamages = "", ProductMfrName = "HP", ProductMfrSerialNumber = "1RR41-00", ProductModelName = "OMEN X", Configuration = "Intel Core i7 processor,NVIDIA GeForce GTX 960M or 965M Graphics Card,Optional 4K IPS Display,8 GB DDR4 RAM,1 TB Hard Disk", WarrantyJobID = "", WorkOrderAuthorizedBy = "", WorkOrderDate = Convert.ToDateTime("6-11-2018"), WorkOrderNumber = "" };
                //EquipmentInDetailViewModel equipmentInDetailViewModel3 = new EquipmentInDetailViewModel() { IssueDescription = "KeyBoard Not Working", ReportedBy = "Customer" };
                //equipmentInViewModel3.Detail.Add(equipmentInDetailViewModel3);

                //equipmentInViewModel.Add(equipmentInViewModel3);

                //EquipmentInReportViewModel equipmentInViewModel4 = new EquipmentInReportViewModel() { AccessoriesList = "", ArrivalDateTime = Convert.ToDateTime("01-02-2018"), AssignedRackID = "4a", CancelledBy = "", CancelledDateTime = Convert.ToDateTime("6-11-2018"), CancelledReason = "", CheckedBy = "Suresh", CompanyCode = "", CustomerBillingAddress = "qoq", CustomerName = "Mathew", CustomersImportantInstructions = "", CustomerText = "", DeliveredByPersonnelName = "", DeliveredByPersonPhoneNumber = "", EquipINInternalInstructions = "", EquipmentINByName = "Amal", EquipmentInId = "4",  InternalID = "444", PublicTagID = "44", InWarranty = true, IsCancelled = true, IsPublic = false, OtherDamages = "", PhysicalDamages = "", ProductMfrName = "Sony", ProductMfrSerialNumber = "2UI8IO", ProductModelName = "VAIO T Series SVT13134CXS", Configuration = "13.3-Inch Touchscreen Ultrabook,1.9 GHz Intel Core i3-3227U Processor, 4GB DDR3, 500GB HDD, Windows 8", TagID = "3er3", WarrantyJobID = "", WorkOrderAuthorizedBy = "", WorkOrderDate = Convert.ToDateTime("6-11-2018"), WorkOrderNumber = "" };
                //EquipmentInDetailViewModel equipmentInDetailViewModel4 = new EquipmentInDetailViewModel() { IssueDescription = "Touch Screen Not Working", ReportedBy = "Customer" };
                //equipmentInViewModel4.Detail.Add(equipmentInDetailViewModel4);

                //equipmentInViewModel.Add(equipmentInViewModel4);

                //EquipmentInReportViewModel equipmentInViewModel5 = new EquipmentInReportViewModel() { AccessoriesList = "", ArrivalDateTime = Convert.ToDateTime("01-02-2018"), AssignedRackID = "5a", CancelledBy = "", CancelledDateTime = Convert.ToDateTime("6-11-2018"), CancelledReason = "", CheckedBy = "Ratheesh", CompanyCode = "", CustomerBillingAddress = "sks", CustomerName = "Joel", CustomersImportantInstructions = "", CustomerText = "", DeliveredByPersonnelName = "", DeliveredByPersonPhoneNumber = "", EquipINInternalInstructions = "", EquipmentINByName = "Kamal", EquipmentInId = "5",  InternalID = "555", PublicTagID = "55", InWarranty = false, IsCancelled = false, IsPublic = false, OtherDamages = "", PhysicalDamages = "", ProductMfrName = "Asus", ProductMfrSerialNumber = "3URIIO", ProductModelName = "VivoBook Pro", Configuration = "17.3 inches Full HD, Intel i7, 16GB DDR4 RAM, 256GB M.2 SSD + 1TB HDD, GeForce GTX 1050 4GB, Backlit KB, Windows 10 - N705UD-EH76, Star Gray, Casual Gaming", TagID = "4rt4", WarrantyJobID = "", WorkOrderAuthorizedBy = "", WorkOrderDate = Convert.ToDateTime("6-11-2018"), WorkOrderNumber = "" };
                //EquipmentInDetailViewModel equipmentInDetailViewModel5 = new EquipmentInDetailViewModel() { IssueDescription = "System Not switching on", ReportedBy = "Customer" };
                //equipmentInViewModel5.Detail.Add(equipmentInDetailViewModel5);

                //equipmentInViewModel.Add(equipmentInViewModel5);

                //EquipmentInReportViewModel equipmentInViewModel6 = new EquipmentInReportViewModel() { AccessoriesList = "", ArrivalDateTime = Convert.ToDateTime("01-03-2018"), AssignedRackID = "6a", CancelledBy = "", CancelledDateTime = Convert.ToDateTime("6-11-2018"), CancelledReason = "", CheckedBy = "Abhi", CompanyCode = "", CustomerBillingAddress = "dld", CustomerName = "Abel", CustomersImportantInstructions = "", CustomerText = "", DeliveredByPersonnelName = "", DeliveredByPersonPhoneNumber = "", EquipINInternalInstructions = "", EquipmentINByName = "Joshi", EquipmentInId = "6",  InternalID = "666", PublicTagID = "66", InWarranty = false, IsCancelled = false, IsPublic = false, OtherDamages = "", PhysicalDamages = "", ProductMfrName = "Acer", ProductMfrSerialNumber = "3REE-11", ProductModelName = "Aspire E 15", Configuration = "8th Gen Intel Core i3-8130U, 6GB RAM Memory, 1TB HDD, 8X DVD", TagID = "7yt6", WarrantyJobID = "", WorkOrderAuthorizedBy = "", WorkOrderDate = Convert.ToDateTime("6-11-2018"), WorkOrderNumber = "" };
                //EquipmentInDetailViewModel equipmentInDetailViewModel6 = new EquipmentInDetailViewModel() { IssueDescription = "System getting heated", ReportedBy = "Customer" };
                //equipmentInViewModel6.Detail.Add(equipmentInDetailViewModel6);

                //equipmentInViewModel.Add(equipmentInViewModel6);

                //EquipmentInReportViewModel equipmentInViewModel7 = new EquipmentInReportViewModel() { AccessoriesList = "", ArrivalDateTime = Convert.ToDateTime("01-04-2018"), AssignedRackID = "7a", CancelledBy = "", CancelledDateTime = Convert.ToDateTime("6-11-2018"), CancelledReason = "", CheckedBy = "Ravi", CompanyCode = "", CustomerBillingAddress = "ucu", CustomerName = "Sabin", CustomersImportantInstructions = "", CustomerText = "", DeliveredByPersonnelName = "", DeliveredByPersonPhoneNumber = "", EquipINInternalInstructions = "", EquipmentINByName = "Aashique", EquipmentInId = "7",  InternalID = "777", PublicTagID = "77", InWarranty = false, IsCancelled = false, IsPublic = false, OtherDamages = "", PhysicalDamages = "", ProductMfrName = "Toshiba", ProductMfrSerialNumber = "5CEE-233", ProductModelName = "Tecra A50", Configuration = "Intel Core i7-7500U up to 3.50GHz, 16GB DDR4, 256GB M.2 SSD, DVD±RW, HDMI, 802.11ac, Bluetooth, TPM 2.0, USB 3.0, Windows 10 Professional", TagID = "4t44", WarrantyJobID = "", WorkOrderAuthorizedBy = "", WorkOrderDate = Convert.ToDateTime("6-11-2018"), WorkOrderNumber = "" };
                //EquipmentInDetailViewModel equipmentInDetailViewModel7 = new EquipmentInDetailViewModel() { IssueDescription = "No Battery BackUp", ReportedBy = "Personnel" };
                //equipmentInViewModel7.Detail.Add(equipmentInDetailViewModel7);

                //equipmentInViewModel.Add(equipmentInViewModel7);

                //EquipmentInReportViewModel equipmentInViewModel8 = new EquipmentInReportViewModel() { AccessoriesList = "", ArrivalDateTime = Convert.ToDateTime("01-04-2018"), AssignedRackID = "8a", CancelledBy = "", CancelledDateTime = Convert.ToDateTime("6-11-2018"), CancelledReason = "", CheckedBy = "Manu", CompanyCode = "", CustomerBillingAddress = "vpv", CustomerName = "Abin", CustomersImportantInstructions = "", CustomerText = "", DeliveredByPersonnelName = "", DeliveredByPersonPhoneNumber = "", EquipINInternalInstructions = "", EquipmentINByName = "Roshan", EquipmentInId = "8",  InternalID = "888", PublicTagID = "88", InWarranty = false, IsCancelled = true, IsPublic = false, OtherDamages = "", PhysicalDamages = "", ProductMfrName = "Fujitsu", ProductMfrSerialNumber = "12DCEE-003", ProductModelName = "Lifebook A555", Configuration = "Intel Core i3 5th Gen,2 GHz Clock Speed,8 GB DDR3 RAM,500 GB Hard Disk,Intel HD Graphics,15.6 inches, 1366 x 768 pixels,DOS OS, 1 Year Warranty", TagID = "5r5", WarrantyJobID = "", WorkOrderAuthorizedBy = "", WorkOrderDate = Convert.ToDateTime("6-11-2018"), WorkOrderNumber = "" };
                //EquipmentInDetailViewModel equipmentInDetailViewModel8 = new EquipmentInDetailViewModel() { IssueDescription = "USB not detecting", ReportedBy = "Personnel" };
                //equipmentInViewModel8.Detail.Add(equipmentInDetailViewModel8);

                //equipmentInViewModel.Add(equipmentInViewModel8);

                //EquipmentInReportViewModel equipmentInViewModel9 = new EquipmentInReportViewModel() { AccessoriesList = "", ArrivalDateTime = Convert.ToDateTime("01-05-2018"), AssignedRackID = "9a", CancelledBy = "", CancelledDateTime = Convert.ToDateTime("6-11-2018"), CancelledReason = "", CheckedBy = "Benny", CompanyCode = "", CustomerBillingAddress = "rlr", CustomerName = "Sathish", CustomersImportantInstructions = "", CustomerText = "", DeliveredByPersonnelName = "", DeliveredByPersonPhoneNumber = "", EquipINInternalInstructions = "", EquipmentINByName = "Pavan", EquipmentInId = "9",  InternalID = "999", PublicTagID = "99", InWarranty = false, IsCancelled = true, IsPublic = false, OtherDamages = "", PhysicalDamages = "", ProductMfrName = "MSI", ProductMfrSerialNumber = "4msi31", ProductModelName = "Stealth GS73", Configuration = "Display resolution1920x1080 pixels,OSWindows 10 Pro,RAM8GB,Hard disk2TB,,GraphicsNVIDIA Geforce GTX 1070,Weight2.43kg", TagID = "3rr4", WarrantyJobID = "", WorkOrderAuthorizedBy = "", WorkOrderDate = Convert.ToDateTime("6-11-2018"), WorkOrderNumber = "" };
                //EquipmentInDetailViewModel equipmentInDetailViewModel9 = new EquipmentInDetailViewModel() { IssueDescription = "No Battery BackUp", ReportedBy = "Personnel" };
                //equipmentInViewModel9.Detail.Add(equipmentInDetailViewModel9);

                //equipmentInViewModel.Add(equipmentInViewModel9);



                IUnitOfWork UoW = new UnitOfWork(CompanyCode);

                equipmentInViewModel = EquipmentInReportMapper.ToReportViewModel(UoW.EquipmentIn.GetList(CompanyCode,
                    new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, 00, 00, 00),
                    new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59)).ReturnValue);



                return equipmentInViewModel;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }




        }
    }
}