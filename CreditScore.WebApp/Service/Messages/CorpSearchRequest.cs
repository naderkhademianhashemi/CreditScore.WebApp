using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;


namespace AlpsCreditScoring.Service.Messages
{
    public class CorpSearchRequest
    {
        public bool bCorpName { get; set; }
        public bool bRegisterNo { get; set; }
        public bool bNational { get; set; }
        public bool bBranch { get; set; }
        public bool bLoanCode { get; set; }
        public bool bCapitalKickoff { get; set; }
        public bool bCustomerCode { get; set; }
        public bool bCreateDate1 { get; set; }
        public bool bCreateDate2 { get; set; }
        public bool bCorpType { get; set; }
        public bool bCorpActivity { get; set; }
        public bool bLoanType { get; set; }
        public bool bLoanState { get; set; }
        public bool bBourse { get; set; }
        public bool bAmount { get; set; }
        public bool bAccountNo { get; set; }
        public string CorpName { get; set; }
        public string CorpType { get; set; }
        public string RegisterNo { get; set; }
        public string National { get; set; }
        public string Branch { get; set; }
        public string LoanCode { get; set; }
        public string CapitalKickoff { get; set; }
        public string CustomerCode { get; set; }
        public string CorpActivity { get; set; }
        public string CreateDate1 { get; set; }
        public string CreateDate2 { get; set; }
        public string LoanType { get; set; }
        public string LoanState { get; set; }
        public string Bourse { get; set; }
        public string Amount { get; set; }
        public string AccountNo { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public Int64 BloID { get; set; }

        //Permit Section
        public string PermitNo { get; set; }
        public string IssueDate { get; set; }
        public string ExpireDate { get; set; }
        public string IssuePlace { get; set; }
        public Int32 PermitTypeId { get; set; }
        public Int32 IssuePlaceId { get; set; }
        public Int32 ValidMonth { get; set; }
        public Int64 BpeID { get; set; }

        //Board Section
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string BirthDate { get; set; }
        public string NationalId { get; set; }
        public string CertificationId { get; set; }
        public string BoardIssuePlace { get; set; }
        public Int32 EducationId { get; set; }
        public Int32 PositionId { get; set; }
        public Int64 BprID { get; set; }
        public Int64 CorpID { get; set; }
        public Int64 BboID { get; set; }
        public string BprCode { get; set; }
        public Int32 BrpTypeID { get; set; }

        //Share Section
        public Int64 BshID { get; set; }
        public string Percent { get; set; }


    }
}