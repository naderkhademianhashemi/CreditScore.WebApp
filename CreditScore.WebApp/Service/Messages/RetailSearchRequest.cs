using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;


namespace AlpsCreditScoring.Service.Messages
{
    public class RetailSearchRequest
    {
        public bool bAccountNo { get; set; }
        public bool bAmount { get; set; }
        public bool bNational { get; set; }
        public bool bBranch { get; set; }
        public bool bLoanCode { get; set; }
        public bool bName { get; set; }
        public bool bFamily { get; set; }
        public bool bFather { get; set; }
        public bool bCustomerCode { get; set; }
        public bool bBirthDate { get; set; }
        public bool bCreateDate1 { get; set; }
        public bool bCreateDate2 { get; set; }
        public bool bSex { get; set; }
        public bool bMarital { get; set; }
        public bool bLoanType { get; set; }
        public bool bLoanState { get; set; }
        public bool bEduDegree { get; set; }
        public string AccountNo { get; set; }
        public string Amount { get; set; }
        public string National { get; set; }
        public string CertId { get; set; }
        public string Branch { get; set; }
        public string LoanCode { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Father { get; set; }
        public string CustomerCode { get; set; }
        public string birthDate { get; set; }
        public string CreateDate1 { get; set; }
        public string CreateDate2 { get; set; }
        public string Sex { get; set; }
        public string Marital { get; set; }
        public string LoanType { get; set; }
        public string LoanState { get; set; }
        public string EduDegree { get; set; }
        public string Health { get; set; }
        public string Employ { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public Int64 BloID { get; set; }
        public Int64 BprID { get; set; }
        public Int64 SpouseID { get; set; }
        public int Type { get; set; }
        public string Model { get; set; }
        public string Desc { get; set; }

        //Facility Section
        public string Institution { get; set; }
        public double FacilityAmount { get; set; }
        public Int32? PaymentNumber { get; set; }
        public double Payment { get; set; }
        public double? Remain { get; set; }
        public Int32 LoanTypeId { get; set; }
        public Int32? FacilityTypeId { get; set; }
        public Int64 BriID { get; set; }
        //Bank Section
        public double AccountTurnOver { get; set; }
        public double AccountAmount { get; set; }
        public string OpeningDate { get; set; }
        public DateTime Opening_Date { get; set; }
        public string AccountType { get; set; }
        public string BankName { get; set; }
        public Int32 AccountTypeId { get; set; }
        public Int32 BankId { get; set; }
        public Int64 BkaID { get; set; }

        //Income Section
        public Int32 IncomeTypeId { get; set; }
        public double IncomeAmount { get; set; }
        public string IncomeTitle { get; set; }
        public Int64 BinID { get; set; }

        //Expend Section
        public Int32 ExpendTypeId { get; set; }
        public double ExpendAmount { get; set; }
        public string ExpendTitle { get; set; }
        public Int64 BexID { get; set; }

        //Asset Section
        public Int32 AssetTypeId { get; set; }
        public double AssetAmount { get; set; }
        public string AssetTitle { get; set; }
        public Int64 BssID { get; set; }

        //Judg Section
        public Int32 UserId { get; set; }
        public Int32 ElementId { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
    }
}