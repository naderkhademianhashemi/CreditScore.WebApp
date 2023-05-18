using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Repository;



namespace AlpsCreditScoring.Service.Messages
{
    public class ScoreRequest
    {
        public bool bLivingStatus { get; set; }
        public bool bEmployee { get; set; }
        public bool bGuaranty { get; set; }
        public bool bEconomic { get; set; }
        public bool bCustomerGroup { get; set; }
        public bool bResident { get; set; }
        public bool bShohrat { get; set; }
        public bool bOzviat { get; set; }
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
        public bool bNiroou { get; set; }
        public string AccountNo { get; set; }
        public string Amount { get; set; }
        public string National { get; set; }
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
        public string Guaranty { get; set; }
        public string Resident { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public Int64 BloID { get; set; }
        public string LoanTypeId { get; set; }
        public string SexId { get; set; }
        public string MaritalId { get; set; }
        public string LivingStatusId { get; set; }
        public string EducationId { get; set; }
        public string EmployeeId { get; set; }
        public string GuaranteeTypeId { get; set; }
        public string EconomicSectorTypeId { get; set; }
        public string ContractTypeId { get; set; }
        public string CustomerGroupTypeId { get; set; }
        public string ResidentId { get; set; }
        public string OzviatId { get; set; }
        public string ShohratId { get; set; }
        public string NiroouId { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public string strDataBaseName { get; set; }
        public string strUserName { get; set; }
        public string strPassWord { get; set; }
        public string sOlapDataSource { get; set; }
        public string strInitCatalog { get; set; }
        public string strMiningStructNameHaghighy { get; set; }
        public string strMiningStructNameHoghoghy { get; set; }
        public string strJudgeHaghighy { get; set; }
        public string strJudgeHoghoghy { get; set; }

    }
}