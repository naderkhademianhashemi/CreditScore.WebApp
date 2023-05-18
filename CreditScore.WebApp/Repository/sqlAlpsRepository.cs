using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using AlpsCreditScoring.Models;
using AlpsCreditScoring.Service.Messages;


namespace AlpsCreditScoring.Repository
{
    public class sqlAlpsRepository : IAlpsRepository
    {
        private string _connectionString;
        private System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("fa-IR");
        public sqlAlpsRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AlpsConnection"].ConnectionString;
        }
        public RetailSearchResponse FindAll(int pageIndex, int pageSize)
        {
            var response = new RetailSearchResponse();
            try
            {
                #region Query
                using (var con = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("[GetRetailPageWise]"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (var dt = new DataTable())
                            {
                                sda.Fill(dt);
                                response.Table = dt;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse FindTableBy(RetailSearchRequest Request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                #region Query
                string queryString = "SELECT * FROM dbo.V_BNK_LOANSEARCH WHERE 1 = 1 ";
                if (Request.bNational)
                    queryString += "AND BRP_IDENTIFICATION_ID = " + Request.National;
                if (Request.bName)
                    queryString += "AND BRP_FIRST_NAME LIKE N'" + Request.Name + "'";
                if (Request.bBranch)
                    queryString += "AND BLO_BRANCH_CODE LIKE '" + Request.Branch + "'";
                if (Request.bLoanCode)
                    queryString += "AND BLO_CODE LIKE '" + Request.LoanCode + "'";
                if (Request.bAccountNo)
                    queryString += "AND BLO_ACCOUNTNO LIKE '" + Request.AccountNo + "'";
                if (Request.bCustomerCode)
                    queryString += "AND BPR_CODE LIKE '" + Request.CustomerCode + "'";
                if (Request.bFamily)
                    queryString += "AND BRP_LAST_NAME LIKE N'" + Request.Family + "'";
                if (Request.bFather)
                    queryString += "AND BRP_FATHER_NAME LIKE N'" + Request.Father + "'";
                if (Request.bBirthDate)
                    queryString += "AND BRP_BIRTH_DATE = '" + Request.birthDate + "'";
                if (Request.bCreateDate1 && !Request.bCreateDate2)
                    queryString += "AND BLO_CREATE_DATE > '" + Request.bCreateDate1 + "'";
                if (Request.bCreateDate1 && Request.bCreateDate2)
                    queryString += "AND BLO_CREATE_DATE > '" + Request.bCreateDate1 + "'" +
                         "' AND BLO_CREATE_DATE < '" + Request.bCreateDate2;
                if (Request.bSex)
                    queryString += "AND BRP_SEX_ID = " + Request.Sex;
                if (Request.bMarital)
                    queryString += "AND BRP_MARIAGE_ID = " + Request.Marital;
                if (Request.bLoanType)
                    queryString += "AND BLO_LOANTYPE_ID = " + Request.LoanType;
                if (Request.bLoanState)
                    queryString += "AND BLO_LOANSTATE = " + Request.LoanState;
                if (Request.bEduDegree)
                    queryString += "AND BRP_EDU_DEGREE_ID = " + Request.EduDegree;
                if (Request.bAmount)
                    queryString += "AND BLO_AMOUNT >= " + Request.Amount;
                queryString += " ORDER BY BPR_CODE";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        command.Connection = connection;
                        sda.SelectCommand = command;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds, "Loans");
                            //response.count = Convert.ToInt64(cmd.Parameters["@PageCount"].Value);
                            response.Table = ds.Tables["Loans"];
                        }
                    }
                }
                #endregion
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse FindByAddress(long BprID)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            DataSet dsAddress = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            Cls_Loan loan = new Cls_Loan();
            Cls_Address address = new Cls_Address();
            try
            {
                #region QueryFind
                string queryString = "SELECT * FROM dbo.V_BNK_ADDRESSFA WHERE BRP_BPR_ID = " + BprID;
                // + " AND BAP_CREATE_DATE IN (SELECT MAX(BAP_CREATE_DATE) FROM dbo.BKT_ADDRESS_PERSON where BAP_BPR_ID = " + BprID + ")";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    da = new SqlDataAdapter(command);
                    da.Fill(dsAddress, "Address");
                    loan.bktCustomer.bktPerson.bktAddress = new Cls_Address[dsAddress.Tables[0].Rows.Count];
                    for (int j = 0; j < dsAddress.Tables[0].Rows.Count; j++)
                    {
                        address = new Cls_Address();
                        address.Bap_TypeID = Convert.ToInt32(dsAddress.Tables[0].Rows[j]["BBD_ID"]);
                        address.BapId = Convert.ToInt32(dsAddress.Tables[0].Rows[j]["BAP_ID"]);
                        address.BapPriority = Convert.ToByte(dsAddress.Tables[0].Rows[j]["BAP_PRIORITY"]);
                        address.BapValue = dsAddress.Tables[0].Rows[j]["BAP_VALUE"].ToString();
                        loan.bktCustomer.bktPerson.bktAddress[j] = address;
                    }
                    response.Loan = new Cls_Loan();
                    response.Loan = loan;
                }

                #endregion
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse FindByJob(long BprID)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            Cls_Loan loan = new Cls_Loan();
            Cls_Job job = new Cls_Job();
            Cls_Meta meta = new Cls_Meta();
            try
            {
                #region QueryFind
                string queryString = "SELECT * FROM dbo.V_BNK_JOBFA WHERE BRP_BPR_ID = " + BprID.ToString() +
                             " Order By BJO_SEQ";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    int i = 0;
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            job = new Cls_Job();
                            job.BjoId = Convert.ToInt64(reader["BJO_ID"]);
                            job.BjoPlaceTitle = reader["BJO_PLACE_TITLE"].ToString();
                            if (reader["BJO_DURATION"] != DBNull.Value)
                                job.BjoDuration = Convert.ToInt32(reader["BJO_DURATION"]);
                            if (reader["BJO_POSITION_ID"] != DBNull.Value)
                                job.Bjo_PositionId = Convert.ToInt32(reader["BJO_POSITION_ID"]);
                            loan.bktCustomer.bktJobs[i] = job;
                            i++;
                        }
                    }
                    response.Loan = new Cls_Loan();
                    response.Loan = loan;
                    reader.Close();
                }

                #endregion
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse FindBySpouse(long BprID)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            Cls_Loan loan = new Cls_Loan();
            Cls_Job job = new Cls_Job();
            Cls_Meta meta = new Cls_Meta();
            try
            {
                #region QueryFind
                string queryString = "SELECT * FROM V_BNK_SPOUSEFA WHERE BRP_SPOUSE_ID =  " + BprID.ToString();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            loan.bktCustomer.bktSpouse = new Cls_RealPerson();
                            loan.bktCustomer.bktSpouse.bktPerson.BprId = Convert.ToInt64(reader["BRP_BPR_ID"]);
                            loan.bktCustomer.bktSpouse.BRP_EDU_DEGREE_ID = Convert.ToInt32(reader["BRP_EDU_DEGREE_ID"]);
                            loan.bktCustomer.bktSpouse.BRP_EMPLOY_ID = Convert.ToInt32(reader["BRP_EMPLOY_ID"]);
                            loan.bktCustomer.bktSpouse.BRP_HEALTHSTATE_ID = Convert.ToInt32(reader["BRP_HEALTHSTATE_ID"]);
                            loan.bktCustomer.bktSpouse.BrpFirstName = reader["BRP_FIRST_NAME"].ToString();
                            loan.bktCustomer.bktSpouse.BrpLastName = reader["BRP_LAST_NAME"].ToString();
                            loan.bktCustomer.bktSpouse.BrpFatherName = reader["BRP_FATHER_NAME"].ToString();
                            loan.bktCustomer.bktSpouse.BrpIdentificationId = Convert.ToInt64(reader["BRP_IDENTIFICATION_ID"]);
                            loan.bktCustomer.bktSpouse.BrpBirthCertid = Convert.ToInt64(reader["BRP_BIRTH_CERTID"]);
                            if (reader["BRP_BIRTH_DATE"] != DBNull.Value && reader["BRP_BIRTH_DATE"].ToString() != "1/1/0001")
                                loan.bktCustomer.bktSpouse.BrpBirthDate = DateTime.ParseExact(reader["BRP_BIRTH_DATE"].ToString(), "yyyy/mm/dd", culture);
                            //loan.bktCustomer.bktSpouse.BrpBirthDate = Convert.ToDateTime(reader["BRP_BIRTH_DATE"]);
                            loan.bktCustomer.bktSpouse.BrpIssuePlace = reader["BRP_ISSUE_PLACE"].ToString();
                            job.BjoId = Convert.ToInt64(reader["BJO_ID"]);
                            job.BjoPlaceTitle = reader["BJO_PLACE_TITLE"].ToString();
                            job.BjoDuration = Convert.ToInt32(reader["BJO_DURATION"]);
                            job.Bjo_PositionId = Convert.ToInt32(reader["BJO_POSITION_ID"]);
                            loan.bktCustomer.bktSpouse.bktJobs[0] = job;
                            response.Loan = new Cls_Loan();
                            response.Loan = loan;
                            response.Job = new Cls_Job();
                            response.Job = job;
                            reader.Close();
                        }
                        else
                        {
                            response.Success = false;
                            return response;
                        }
                    }

                    queryString = "SELECT * FROM dbo.V_BNK_SPOUSEDATA WHERE BRP_BPR_ID = " + loan.bktCustomer.bktSpouse.bktPerson.BprId.ToString() +
                                        " AND [FOR] LIKE N'شخص حقيقی'";
                    command.CommandText = queryString;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        int i = 0;
                        if (reader.HasRows)
                        {
                            response.Meta = new AlpsCreditScoring.Models.Cls_Meta[2];
                            while (reader.Read())
                            {
                                meta = new Cls_Meta();
                                meta.BmdId = Convert.ToInt64(reader["BMD_ID"]);
                                meta.BmdKey = reader["BMD_KEY"].ToString();
                                meta.BmdValue = reader["BMD_VALUE"].ToString();
                                response.Meta[i] = meta;
                                i++;
                            }
                        }
                        reader.Close();
                    }
                }

                #endregion
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse FindBy(RetailSearchRequest Request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            Cls_Loan loan = new Cls_Loan();
            try
            {
                #region QueryFind
                string queryString = "SELECT * FROM dbo.V_BNK_LOANSEARCH WHERE BPR_TYPE = 1 and BLO_ID = " + Request.BloID.ToString();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            if (reader["BLO_STATUS"] != DBNull.Value)
                                loan.BloStatus = Convert.ToByte(reader["BLO_STATUS"]);
                            loan.bktCustomer.bktPerson.BprId = Convert.ToInt64(reader["BPR_ID"]);
                            loan.BloId = Convert.ToInt64(reader["BLO_ID"]);
                            loan.BloCode = reader["BLO_CODE"].ToString();
                            loan.bktCustomer.BRP_SEX_ID = Convert.ToInt32(reader["BRP_SEX_ID"]);
                            if (reader["BRP_IDENTIFICATION_ID"] != DBNull.Value)
                                loan.bktCustomer.BrpIdentificationId = Convert.ToInt64(reader["BRP_IDENTIFICATION_ID"]);
                            if (reader["BRP_BIRTH_CERTID"] != DBNull.Value)
                                loan.bktCustomer.BrpBirthCertid = Convert.ToInt64(reader["BRP_BIRTH_CERTID"]);
                            if (reader["BRP_FATHER_NAME"] != DBNull.Value)
                                loan.bktCustomer.BrpFatherName = reader["BRP_FATHER_NAME"].ToString();
                            if (reader["BLO_CREATE_DATE"] != DBNull.Value)
                            {
                                loan.BloCreateDate = Convert.ToDateTime(reader["BLO_CREATE_DATE"]);
                            }
                            if (reader["BLO_ECONOMICSECTOR_ID"] != DBNull.Value)
                                loan.BLO_EconomicSector = Convert.ToInt32(reader["BLO_ECONOMICSECTOR_ID"]);
                            if (reader["BLO_CONTRACTTYPE_ID"] != DBNull.Value)
                                loan.BLO_ContractType = Convert.ToInt32(reader["BLO_CONTRACTTYPE_ID"]);
                            if (reader["BLO_CUSTOMERGROUP_ID"] != DBNull.Value)
                                loan.BLO_CustomerGroup = Convert.ToInt32(reader["BLO_CUSTOMERGROUP_ID"]);
                            if (reader["BRP_RESIDENT_ID"] != DBNull.Value)
                                loan.bktCustomer.BRP_RESIDENT_ID = Convert.ToInt32(reader["BRP_RESIDENT_ID"]);
                            if (reader["BRP_OZVIAT_ID"] != DBNull.Value)
                                loan.bktCustomer.BRP_OZVIAT_ID = Convert.ToInt32(reader["BRP_OZVIAT_ID"]);
                            if (reader["BRP_SHOHRAT_ID"] != DBNull.Value)
                                loan.bktCustomer.BRP_SHOHRAT_ID = Convert.ToInt32(reader["BRP_SHOHRAT_ID"]);
                            if (reader["BRP_NIROOU_ID"] != DBNull.Value)
                                loan.bktCustomer.BRP_NIROOU_ID = Convert.ToInt32(reader["BRP_NIROOU_ID"]);
                            if (reader["BLO_LOANAIM"] != DBNull.Value)
                                loan.BLO_LoanAim = Convert.ToInt32(reader["BLO_LOANAIM"]);
                            if (reader["BLO_LOANSTATE"] != DBNull.Value)
                                loan.BLO_LoanState = Convert.ToInt32(reader["BLO_LOANSTATE"]);
                            if (reader["BLO_LOANTYPE_ID"] != DBNull.Value)
                                loan.BLO_LoanTypeId = Convert.ToInt32(reader["BLO_LOANTYPE_ID"]);
                            if (reader["BLO_LOANTYPE_ID"] != DBNull.Value)
                                loan.BLO_LoanType2Id = Convert.ToInt32(reader["BLO_LOANTYPE2_ID"]);
                            if (reader["BLO_LOANREQUEST_ID"] != DBNull.Value)
                                loan.BLO_LoanRequest = Convert.ToInt32(reader["BLO_LOANREQUEST_ID"]);
                            if (reader["BLO_BCU_ID"] != DBNull.Value)
                                loan.BLO_BCU_ID = Convert.ToInt32(reader["BLO_BCU_ID"]);
                            if (reader["BLO_BRANCH_CODE"] != DBNull.Value)
                            {
                                loan.BloBranchCode = reader["BLO_BRANCH_CODE"].ToString();
                                loan.StateCode = reader["State_Of_Branch"].ToString();
                            }
                            if (reader["BLO_AMOUNT"] != DBNull.Value)
                                loan.BloAmount = Convert.ToDouble(reader["BLO_AMOUNT"]);
                            if (reader["BLO_ASSURANCE1"] != DBNull.Value)
                                loan.BloAssurance1 = Convert.ToByte(reader["BLO_ASSURANCE1"]);
                            if (reader["BLO_ASSURANCE2"] != DBNull.Value)
                                loan.BloAssurance2 = Convert.ToByte(reader["BLO_ASSURANCE2"]);
                            if (reader["BLO_ACCOUNTNO"] != DBNull.Value)
                                loan.BloAccountNo = reader["BLO_ACCOUNTNO"].ToString();
                            if (reader["BLO_PAYMENT"] != DBNull.Value)
                                loan.BloPayment = Convert.ToDouble(reader["BLO_PAYMENT"]);
                            if (reader["BLO_PAYMENT_NBR"] != DBNull.Value)
                                loan.BloPaymentNbr = Convert.ToInt32(reader["BLO_PAYMENT_NBR"]);
                            if (reader["BLO_PAYMENT_DATE"] != DBNull.Value)
                                loan.BloPaymentDate = Convert.ToDateTime(reader["BLO_PAYMENT_DATE"]);
                            if (reader["BLO_PAYMENT_PERIOD"] != DBNull.Value)
                                loan.BloPaymentPeriod = Convert.ToByte(reader["BLO_PAYMENT_PERIOD"]);
                            if (loan.BloStatus != 3)
                            {
                                loan.bktCustomer.bktPerson.BprCode = reader["BPR_CODE"].ToString();
                                loan.bktCustomer.bktPerson.BprType = 1;
                                loan.bktCustomer.BrpFirstName = reader["BRP_FIRST_NAME"].ToString();
                                loan.bktCustomer.BrpLastName = reader["BRP_LAST_NAME"].ToString();
                                if (reader["BRP_BIRTH_DATE"] != DBNull.Value)
                                {
                                    loan.bktCustomer.BrpBirthDate = Convert.ToDateTime(reader["BRP_BIRTH_DATE"]);
                                }
                                if (reader["BLO_MATURITY_DATE"] != DBNull.Value)
                                    loan.BloMaturityDate = Convert.ToDateTime(reader["BLO_MATURITY_DATE"]);
                                if (reader["BRP_ISSUE_PLACE"] != DBNull.Value)
                                    loan.bktCustomer.BrpIssuePlace = reader["BRP_ISSUE_PLACE"].ToString();
                                if (reader["BRP_MARIAGE_ID"] != DBNull.Value)
                                    loan.bktCustomer.BRP_MARIAGE_ID = Convert.ToInt32(reader["BRP_MARIAGE_ID"]);
                                if (reader["BRP_HOME_ID"] != DBNull.Value)
                                    loan.bktCustomer.BRP_HOME_ID = Convert.ToInt32(reader["BRP_HOME_ID"]);
                                if (reader["BRP_HEALTHSTATE_ID"] != DBNull.Value)
                                    loan.bktCustomer.BRP_HEALTHSTATE_ID = Convert.ToInt32(reader["BRP_HEALTHSTATE_ID"]);
                                if (reader["BRP_SERVICEDUE_ID"] != DBNull.Value)
                                    loan.bktCustomer.BRP_SERVICEDUE_ID = Convert.ToInt32(reader["BRP_SERVICEDUE_ID"]);
                                if (reader["BRP_EDU_DEGREE_ID"] != DBNull.Value)
                                    loan.bktCustomer.BRP_EDU_DEGREE_ID = Convert.ToInt32(reader["BRP_EDU_DEGREE_ID"]);
                                if (reader["BRP_EMPLOY_ID"] != DBNull.Value)
                                    loan.bktCustomer.BRP_EMPLOY_ID = Convert.ToInt32(reader["BRP_EMPLOY_ID"]);
                                if (reader["BRP_CHILDREN"] != DBNull.Value)
                                    loan.bktCustomer.BrpChildren = Convert.ToByte(reader["BRP_CHILDREN"]);
                                if (reader["BRP_PERSON_INCHARGE"] != DBNull.Value)
                                    loan.bktCustomer.BrpPersonIncharge = Convert.ToByte(reader["BRP_PERSON_INCHARGE"]);
                                if (reader["BRP_EDU_LASTFIELD"] != DBNull.Value)
                                    loan.bktCustomer.BrpEduLastField = reader["BRP_EDU_LASTFIELD"].ToString();


                            }
                            response.Loan = new Cls_Loan();
                            response.Loan = loan;
                            response.Message = loan.BloId.ToString();
                        }
                        else
                            loan = null;
                    }
                }
                #endregion
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse FindByCustomerID(RetailSearchRequest Request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            Cls_Loan loan = new Cls_Loan();
            try
            {
                #region QueryFind
                string queryString = "SELECT count(*) FROM dbo.V_BNK_LOANSEARCH WHERE BPR_TYPE = 1 and BPR_CODE Like '" + Request.CustomerCode + "'";
                //string queryString = "SELECT count(*) FROM dbo.V_BNK_LOANSEARCHFA WHERE BPR_TYPE = 1 and BPR_CODE Like '" + Request.CustomerCode + "'";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        queryString = "SELECT * FROM dbo.V_BNK_LOANSEARCH WHERE BPR_TYPE = 1 and BPR_CODE Like '" + Request.CustomerCode + "'";
                        //queryString = "SELECT * FROM dbo.V_BNK_LOANSEARCHFA WHERE BPR_TYPE = 1 and BPR_CODE Like '" + Request.CustomerCode + "'";
                        command.CommandText = queryString;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int i = 0;
                            if (reader.HasRows)
                            {
                                response.Loans = new Cls_Loan[count];
                                while (reader.Read())
                                {
                                    if (reader["BLO_STATUS"] != DBNull.Value)
                                        loan.BloStatus = Convert.ToByte(reader["BLO_STATUS"]);
                                    loan.bktCustomer.bktPerson.BprId = Convert.ToInt64(reader["BPR_ID"]);
                                    loan.BloId = Convert.ToInt64(reader["BLO_ID"]);
                                    loan.BloCode = reader["BLO_CODE"].ToString();
                                    loan.bktCustomer.BRP_SEX_ID = Convert.ToInt32(reader["BRP_SEX_ID"]);
                                    if (reader["BRP_IDENTIFICATION_ID"] != DBNull.Value)
                                        loan.bktCustomer.BrpIdentificationId = Convert.ToInt64(reader["BRP_IDENTIFICATION_ID"]);
                                    if (reader["BRP_BIRTH_CERTID"] != DBNull.Value)
                                        loan.bktCustomer.BrpBirthCertid = Convert.ToInt64(reader["BRP_BIRTH_CERTID"]);
                                    if (reader["BRP_FATHER_NAME"] != DBNull.Value)
                                        loan.bktCustomer.BrpFatherName = reader["BRP_FATHER_NAME"].ToString();
                                    if (reader["BLO_CREATE_DATE"] != DBNull.Value && reader["BLO_CREATE_DATE"].ToString() != "1/1/0001")
                                    {
                                        loan.BloCreateDate = DateTime.ParseExact(reader["BLO_CREATE_DATE"].ToString(), "yyyy/mm/dd", culture);
                                    }
                                    if (reader["BLO_ECONOMICSECTOR_ID"] != DBNull.Value)
                                        loan.BLO_EconomicSector = Convert.ToInt32(reader["BLO_ECONOMICSECTOR_ID"]);
                                    if (reader["BLO_CONTRACTTYPE_ID"] != DBNull.Value)
                                        loan.BLO_ContractType = Convert.ToInt32(reader["BLO_CONTRACTTYPE_ID"]);
                                    if (reader["BLO_CUSTOMERGROUP_ID"] != DBNull.Value)
                                        loan.BLO_CustomerGroup = Convert.ToInt32(reader["BLO_CUSTOMERGROUP_ID"]);
                                    if (reader["BRP_RESIDENT_ID"] != DBNull.Value)
                                        loan.bktCustomer.BRP_RESIDENT_ID = Convert.ToInt32(reader["BRP_RESIDENT_ID"]);
                                    if (reader["BRP_OZVIAT_ID"] != DBNull.Value)
                                        loan.bktCustomer.BRP_OZVIAT_ID = Convert.ToInt32(reader["BRP_OZVIAT_ID"]);
                                    if (reader["BRP_SHOHRAT_ID"] != DBNull.Value)
                                        loan.bktCustomer.BRP_SHOHRAT_ID = Convert.ToInt32(reader["BRP_SHOHRAT_ID"]);
                                    if (reader["BRP_NIROOU_ID"] != DBNull.Value)
                                        loan.bktCustomer.BRP_NIROOU_ID = Convert.ToInt32(reader["BRP_NIROOU_ID"]);
                                    if (reader["BLO_LOANAIM"] != DBNull.Value)
                                        loan.BLO_LoanAim = Convert.ToInt32(reader["BLO_LOANAIM"]);
                                    if (reader["BLO_LOANSTATE"] != DBNull.Value)
                                        loan.BLO_LoanState = Convert.ToInt32(reader["BLO_LOANSTATE"]);
                                    if (reader["BLO_LOANTYPE_ID"] != DBNull.Value)
                                        loan.BLO_LoanTypeId = Convert.ToInt32(reader["BLO_LOANTYPE_ID"]);
                                    if (reader["BLO_LOANTYPE_ID"] != DBNull.Value)
                                        loan.BLO_LoanType2Id = Convert.ToInt32(reader["BLO_LOANTYPE2_ID"]);
                                    if (reader["BLO_LOANREQUEST_ID"] != DBNull.Value)
                                        loan.BLO_LoanRequest = Convert.ToInt32(reader["BLO_LOANREQUEST_ID"]);
                                    if (reader["BLO_BCU_ID"] != DBNull.Value)
                                        loan.BLO_BCU_ID = Convert.ToInt32(reader["BLO_BCU_ID"]);
                                    if (reader["BLO_BRANCH_CODE"] != DBNull.Value)
                                    {
                                        loan.BloBranchCode = reader["BLO_BRANCH_CODE"].ToString();
                                        if (reader["State_Of_Branch"] != DBNull.Value)
                                            loan.StateCode = reader["State_Of_Branch"].ToString();
                                    }

                                    if (reader["BLO_AMOUNT"] != DBNull.Value)
                                        loan.BloAmount = Convert.ToDouble(reader["BLO_AMOUNT"]);
                                    if (reader["BLO_ASSURANCE1"] != DBNull.Value)
                                        loan.BloAssurance1 = Convert.ToByte(reader["BLO_ASSURANCE1"]);
                                    if (reader["BLO_ASSURANCE2"] != DBNull.Value)
                                        loan.BloAssurance2 = Convert.ToByte(reader["BLO_ASSURANCE2"]);
                                    if (reader["BLO_ACCOUNTNO"] != DBNull.Value)
                                        loan.BloAccountNo = reader["BLO_ACCOUNTNO"].ToString();
                                    if (reader["BLO_PAYMENT"] != DBNull.Value)
                                        loan.BloPayment = Convert.ToDouble(reader["BLO_PAYMENT"]);
                                    if (reader["BLO_PAYMENT_NBR"] != DBNull.Value)
                                        loan.BloPaymentNbr = Convert.ToInt32(reader["BLO_PAYMENT_NBR"]);
                                    if (reader["BLO_PAYMENT_DATE"] != DBNull.Value && reader["BLO_PAYMENT_DATE"].ToString() != "1/1/0001")
                                        loan.BloPaymentDate = DateTime.ParseExact(reader["BLO_PAYMENT_DATE"].ToString(), "yyyy/mm/dd", culture);
                                    //loan.BloPaymentDate = Convert.ToDateTime(reader["BLO_PAYMENT_DATE"]);
                                    if (reader["BLO_PAYMENT_PERIOD"] != DBNull.Value)
                                        loan.BloPaymentPeriod = Convert.ToByte(reader["BLO_PAYMENT_PERIOD"]);
                                    if (loan.BloStatus != 3)
                                    {
                                        loan.bktCustomer.bktPerson.BprCode = reader["BPR_CODE"].ToString();
                                        loan.bktCustomer.bktPerson.BprType = 1;
                                        loan.bktCustomer.BrpFirstName = reader["BRP_FIRST_NAME"].ToString();
                                        loan.bktCustomer.BrpLastName = reader["BRP_LAST_NAME"].ToString();
                                        if (reader["BRP_BIRTH_DATE"] != DBNull.Value && reader["BRP_BIRTH_DATE"].ToString() != "1/1/0001")
                                        {
                                            loan.bktCustomer.BrpBirthDate = DateTime.ParseExact(reader["BRP_BIRTH_DATE"].ToString(), "yyyy/mm/dd", culture);
                                            //loan.bktCustomer.BrpBirthDate = Convert.ToDateTime(reader["BRP_BIRTH_DATE"]);
                                        }
                                        if (reader["BLO_MATURITY_DATE"] != DBNull.Value && reader["BLO_MATURITY_DATE"].ToString() != "1/1/0001")
                                            loan.BloMaturityDate = DateTime.ParseExact(reader["BLO_MATURITY_DATE"].ToString(), "yyyy/mm/dd", culture);
                                        if (reader["BRP_ISSUE_PLACE"] != DBNull.Value)
                                            loan.bktCustomer.BrpIssuePlace = reader["BRP_ISSUE_PLACE"].ToString();
                                        if (reader["BRP_MARIAGE_ID"] != DBNull.Value)
                                            loan.bktCustomer.BRP_MARIAGE_ID = Convert.ToInt32(reader["BRP_MARIAGE_ID"]);
                                        if (reader["BRP_HOME_ID"] != DBNull.Value)
                                            loan.bktCustomer.BRP_HOME_ID = Convert.ToInt32(reader["BRP_HOME_ID"]);
                                        if (reader["BRP_HEALTHSTATE_ID"] != DBNull.Value)
                                            loan.bktCustomer.BRP_HEALTHSTATE_ID = Convert.ToInt32(reader["BRP_HEALTHSTATE_ID"]);
                                        if (reader["BRP_CHILDSTATE_ID"] != DBNull.Value)
                                            loan.bktCustomer.BRP_CHILDSTATE_ID = Convert.ToInt32(reader["BRP_CHILDSTATE_ID"]);
                                        if (reader["BRP_SERVICEDUE_ID"] != DBNull.Value)
                                            loan.bktCustomer.BRP_SERVICEDUE_ID = Convert.ToInt32(reader["BRP_SERVICEDUE_ID"]);
                                        if (reader["BRP_EDU_DEGREE_ID"] != DBNull.Value)
                                            loan.bktCustomer.BRP_EDU_DEGREE_ID = Convert.ToInt32(reader["BRP_EDU_DEGREE_ID"]);
                                        if (reader["BRP_EMPLOY_ID"] != DBNull.Value)
                                            loan.bktCustomer.BRP_EMPLOY_ID = Convert.ToInt32(reader["BRP_EMPLOY_ID"]);
                                        if (reader["BRP_CHILDREN"] != DBNull.Value)
                                            loan.bktCustomer.BrpChildren = Convert.ToByte(reader["BRP_CHILDREN"]);
                                        if (reader["BRP_PERSON_INCHARGE"] != DBNull.Value)
                                            loan.bktCustomer.BrpPersonIncharge = Convert.ToByte(reader["BRP_PERSON_INCHARGE"]);
                                        if (reader["BRP_EDU_LASTFIELD"] != DBNull.Value)
                                            loan.bktCustomer.BrpEduLastField = reader["BRP_EDU_LASTFIELD"].ToString();
                                    }
                                    response.Loans[i] = loan;
                                    i++;
                                }
                            }
                            else
                                loan = null;
                        }
                    }
                }
                response.Success = true;
                #endregion
            }
            catch (SqlException exp)
            {
                response.Success = false;
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailBaseResponse GetBase()
        {
            string strSQL;
            RetailBaseResponse response = new RetailBaseResponse();
            try
            {
                #region QueryBase
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    strSQL = "SELECT * FROM dbo.V_BAS_SEX";
                    SqlCommand command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Gender.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_MARITAL";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Marital.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_EDUDEGREE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.EduDegree.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_LOANTYPE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.LoanType.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_LOANSTATE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.LoanState.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    connection.Close();
                }
                #endregion
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailBaseResponse GetBaseEdit()
        {
            string strSQL;
            RetailBaseResponse response = new RetailBaseResponse();
            try
            {
                #region QueryBase
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    strSQL = "SELECT * FROM dbo.V_BAS_SEX";
                    SqlCommand command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Gender.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_MARITAL";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Marital.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_EDUDEGREE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.EduDegree.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                            response.EduDegreeSpouse.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    //--***********************************************************************loan
                    //--select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 47)
                    strSQL = "select * from Mehr_Base_Data WHERE   (BBD_BBD_ID = 47)";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.LoanType.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    //LoanState
                    //select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 247)
                    strSQL = "select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 247)";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.LoanState.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    //--******************************************************************Account
                    //select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 69)
                    strSQL = "select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 69)";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Account.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_ANSWER";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Answer.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_CHILDREN";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Children.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                            response.PrsInCharge.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    //--*************************************************************Expense
                    //--select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 81)
                    strSQL = "select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 81)";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Expenditure.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_HEALTHSTATE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.HealthState.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                            response.HealthFamily.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                            response.HealthSpouse.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_HOUSESTATE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.HouseState.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    //--**************************************************************Incomes
                    //select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 75)
                    strSQL = "select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 75)";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Income.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_JOBSTATE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.JobState.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                            response.JobStatSpouse.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_LOANREQUEST";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.LoanRequest.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    //--***************************************************************LoanGuaranty
                    //select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 59)
                    strSQL = "select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 59)";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.LoanGuaranty.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_SERVICEDUE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.ServiceDue.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_POSITION";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Position.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                            response.PosSpouse.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    //--*************************************************************BBD_BBD_ID = 222 CURRENCY 
                    //--select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 222)
                    strSQL = "select BBD_ID,BBD_TITLE from Mehr_Base_Data   WHERE     (BBD_BBD_ID = 222)";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Currency.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
                        }
                    }
                    //--***************************************************************BBD_BBD_ID = 111 Banks 
                    //--select BBD_ID,BBD_TITLE from Mehr_Base_Data WHERE(BBD_BBD_ID = 111)
                    strSQL = "SELECT * FROM dbo.BKT_BANKS";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Bank.Add(Convert.ToInt32(reader["BBN_ID"]), reader["BBN_TITLE"].ToString());
                        }
                    }
                    //--********************************************************************AimLoan
                    //--select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 273)
                    strSQL = "select * from Mehr_Base_Data WHERE (BBD_BBD_ID = 273)";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Aim.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    //--******************************************************************collat 
                    //--select * from Mehr_Base_Data WHERE  (BBD_BBD_ID = 280)
                    strSQL = "select * from Mehr_Base_Data WHERE  (BBD_BBD_ID = 280) ";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Collateral.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    //--**************************************************************EconomicSector
                    //--select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 440)
                    strSQL = "select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 440)";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.EconomicSector.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_RESIDENT";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Resident.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_OZVIAT";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Ozviat.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_SHOHRAT";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Shohrat.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_NIROOU";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Niroou.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_ITEMTYPE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.FacilityType.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    //--*********************************************************************CustomerGroup
                    //--select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 433)
                    strSQL = "select * from Mehr_Base_Data WHERE  (BBD_BBD_ID = 433)";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.CustomerGroup.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    //--********************************************************************contract
                    //--select * from Mehr_Base_Data WHERE     (BBD_BBD_ID = 458)
                    strSQL = "select * from Mehr_Base_Data WHERE   (BBD_BBD_ID = 458)";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.ContractType.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }

                    connection.Close();
                }
                #endregion
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse GetClientFacility(RetailSearchRequest Request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string queryString = "SELECT * FROM dbo.V_BNK_ITEMS_mehr WHERE BRI_BLO_ID = " + Request.BloID.ToString() +
                                        " AND TYPE LIKE N'تسهيلات'";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        command.Connection = connection;
                        sda.SelectCommand = command;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds, "Loans");
                            response.Table = ds.Tables["Loans"];
                        }
                    }

                    //} //string queryString = "SELECT * FROM dbo.V_BNK_ITEMSFA WHERE BRI_BLO_ID = " + Request.BloID.ToString() +
                    //                        " AND TYPE LIKE N'تسهيلات'";
                    //using (SqlConnection connection = new SqlConnection(_connectionString))
                    //{
                    //    SqlCommand command = connection.CreateCommand();
                    //    command.CommandText = queryString;
                    //    connection.Open();
                    //    using (SqlDataAdapter sda = new SqlDataAdapter())
                    //    {
                    //        command.Connection = connection;
                    //        sda.SelectCommand = command;
                    //        using (DataSet ds = new DataSet())
                    //        {
                    //            sda.Fill(ds, "Loans");
                    //            response.Table = ds.Tables["Loans"];
                    //        }
                    //    }

                }
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse GetClientAssets(RetailSearchRequest Request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                //string queryString = "SELECT SELECT [BSS_ID],[BSS_TYPE_ID],[BSS_BLO_ID],[BSS_CODE]," +
                //                "[BSS_TITLE],[BSS_AMOUNT],[BSS_DESC],dbo.ToPersianDateTime(BSS_CREATE_DATE) AS BSS_CREATE_DATE," +
                //                ",[BSS_STATUS] FROM dbo.BKT_ASSETS WHERE BSS_BLO_ID = " + Request.BloID.ToString();
                //using (SqlConnection connection = new SqlConnection(_connectionString))
                //{
                //    SqlCommand command = connection.CreateCommand();
                //    command.CommandText = queryString;
                //    connection.Open();
                //    using (SqlDataAdapter sda = new SqlDataAdapter())
                //    {
                //        command.Connection = connection;
                //        sda.SelectCommand = command;
                //        using (DataSet ds = new DataSet())
                //        {
                //            sda.Fill(ds, "Loans");
                //            response.Table = ds.Tables["Loans"];
                //        }
                //    }
                //}

                string queryString = @"SELECT   
                                            [BSS_ID],
                                            [BSS_TYPE_ID],
                                            [BSS_BLO_ID],
                                            [BSS_CODE],
                                            [BSS_TITLE],
                                            [BSS_AMOUNT],
                                            [BSS_DESC],
                                            BSS_CREATE_DATE AS BSS_CREATE_DATE,
                                            [BSS_STATUS]
                                                    FROM 
                                                    dbo.BKT_ASSETS 
                                                    WHERE BSS_BLO_ID = " + Request.BloID.ToString();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        command.Connection = connection;
                        sda.SelectCommand = command;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds, "Loans");
                            response.Table = ds.Tables["Loans"];
                        }
                    }
                }
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse GetClientAccounts(RetailSearchRequest Request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                //string queryString = "SELECT [BKA_ID],[BKA_TYPE_ID],[BKA_BLO_ID],[BKA_BBN_ID],[BKA_ACCOUNT_NO],[BKA_CAPITAL]," +
                //                     "[BKA_TURNOVER],[BBD_TITLE],[BBN_TITLE],dbo.ToPersianDateTime(BKA_OPEN_DATE) AS BKA_OPEN_DATE,[BKA_STATUS] " +
                //                     "FROM dbo.V_BNK_ACCOUNT  WHERE BKA_BLO_ID = " + Request.BloID.ToString();
                string queryString = @"SELECT [BKA_ID],
                                                [BKA_TYPE_ID],
                                                [BKA_BLO_ID],
                                                [BKA_BBN_ID],
                                                [BKA_ACCOUNT_NO],
                                                [BKA_CAPITAL],
                                                [BKA_TURNOVER],
                                                [BBD_TITLE],
                                                [BBN_TITLE],
                                                BKA_OPEN_DATE AS BKA_OPEN_DATE,
                                                [BKA_STATUS]
                                      FROM dbo.V_BNK_ACCOUNT 
                                        WHERE BKA_BLO_ID = " + Request.BloID.ToString();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        command.Connection = connection;
                        sda.SelectCommand = command;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds, "Loans");
                            response.Table = ds.Tables["Loans"];
                        }
                    }
                }
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse GetClientGuaranty(RetailSearchRequest Request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string queryString = "SELECT * FROM dbo.V_BNK_ITEMS_Mehr WHERE BRI_BLO_ID = " + Request.BloID.ToString() +
                                        " AND TYPE LIKE N'ضمانت'";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        command.Connection = connection;
                        sda.SelectCommand = command;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds, "Loans");
                            response.Table = ds.Tables["Loans"];
                        }
                    }
                }
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse GetClientCost(RetailSearchRequest Request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string queryString = @"SELECT [BEX_ID],
                                                [BEX_TYPE_ID],
                                                [BEX_BLO_ID],
                                                [BEX_CODE],
                                                [BEX_TITLE],
                                                [BEX_DESC],
                                                [BEX_AMOUNT],
                                                [BEX_CREATE_DATE] ,
                                                [BEX_STATUS] 
                                            FROM dbo.BKT_EXPENDITURES INNER JOIN
                                     Mehr_Base_Data 
                                        ON dbo.BKT_EXPENDITURES.BEX_TYPE_ID = dbo.Mehr_Base_Data.BBD_ID
                                     WHERE BEX_BLO_ID = " + Request.BloID.ToString();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        command.Connection = connection;
                        sda.SelectCommand = command;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds, "Loans");
                            response.Table = ds.Tables["Loans"];
                        }
                    }
                }

                //string queryString = "SELECT [BEX_ID],[BEX_TYPE_ID],[BEX_BLO_ID],[BEX_CODE],[BEX_TITLE]," +
                //                    "[BEX_DESC],[BEX_AMOUNT],dbo.ToPersianDateTime([BEX_CREATE_DATE]) AS BEX_CREATE_DATE," +
                //                    "[BEX_STATUS] FROM dbo.BKT_EXPENDITURES INNER JOIN " +
                //                    "dbo.BKT_BASE_DATA ON dbo.BKT_EXPENDITURES.BEX_TYPE_ID = dbo.BKT_BASE_DATA.BBD_ID " +
                //                    "WHERE BEX_BLO_ID = " + Request.BloID.ToString();

                //dbo.BKT_BASE_DATA ON dbo.BKT_EXPENDITURES.BEX_TYPE_ID = dbo.BKT_BASE_DATA.BBD_ID 
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public RetailSearchResponse GetClientIncome(RetailSearchRequest Request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string queryString = @"SELECT [BIN_ID],
                                        [BIN_TYPE_ID],
                                        [BIN_BLO_ID],
                                        [BIN_CODE],
                                        Mehr_Base_Data.BBD_TITLE AS [BIN_TITLE],
                                        [BIN_DESC],
                                        [BIN_AMOUNT],
                                        [BIN_CREATE_DATE] AS BIN_CREATE_DATE,
                                        [BIN_STATUS] 
                                        FROM dbo.BKT_INCOMES INNER JOIN
                                    dbo.Mehr_Base_Data 
                                            ON dbo.BKT_INCOMES.BIN_TYPE_ID = dbo.Mehr_Base_Data.BBD_ID
                                    WHERE BIN_BLO_ID = " + Request.BloID.ToString();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        command.Connection = connection;
                        sda.SelectCommand = command;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds, "Loans");
                            response.Table = ds.Tables["Loans"];
                        }
                    }
                }
            }
            //string queryString = "SELECT [BIN_ID],[BIN_TYPE_ID],[BIN_BLO_ID],[BIN_CODE],[BIN_TITLE],[BIN_DESC]," +
            //                    "[BIN_AMOUNT],dbo.ToPersianDateTime([BIN_CREATE_DATE]) AS BIN_CREATE_DATE,[BIN_STATUS] FROM dbo.BKT_INCOMES INNER JOIN " +
            //                    "dbo.BKT_BASE_DATA ON dbo.BKT_INCOMES.BIN_TYPE_ID = dbo.BKT_BASE_DATA.BBD_ID " +
            //                    "WHERE BIN_BLO_ID = " + Request.BloID.ToString();
            //using (SqlConnection connection = new SqlConnection(_connectionString))
            //{
            //    SqlCommand command = connection.CreateCommand();
            //    command.CommandText = queryString;
            //    connection.Open();
            //    using (SqlDataAdapter sda = new SqlDataAdapter())
            //    {
            //        command.Connection = connection;
            //        sda.SelectCommand = command;
            //        using (DataSet ds = new DataSet())
            //        {
            //            sda.Fill(ds, "Loans");
            //            response.Table = ds.Tables["Loans"];
            //        }
            //    }
            //}

            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public UserResponse BaseGroup()
        {
            UserResponse response = new UserResponse();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string strSQL = "SELECT * FROM dbo.Groups";
                SqlCommand command = new SqlCommand(strSQL, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        response.Groups.Add(Convert.ToInt32(reader["ID"]), reader["GroupName"].ToString());
                    }
                }
                connection.Close();
            }
            return response;
        }
        public UserResponse BaseAccesses()
        {
            UserResponse response = new UserResponse();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string strSQL = "SELECT * FROM [dbo].[AccessLevel]";
                SqlCommand command = new SqlCommand(strSQL, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        response.Accesses.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
                    }
                }
                connection.Close();
            }
            return response;
        }
        public UserResponse InsertUser(UserRequest request)
        {
            UserResponse response = new UserResponse();
            try
            {
                string insertSql = "INSERT INTO Users " +
                                    "([User_Name], [User_Password], [User_Group_Id],[User_Branch]) VALUES " +
                                    "(@UserName, @Password, @GroupId, @Branch)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    SqlParameter User = new SqlParameter("@UserName", request.UserName);
                    SqlParameter Pass = new SqlParameter("@Password", request.Password);
                    SqlParameter GroupId = new SqlParameter("@GroupId", request.GroupId);
                    SqlParameter Branch = new SqlParameter("@Branch", request.Branch);
                    command.Parameters.Add(User);
                    command.Parameters.Add(Pass);
                    command.Parameters.Add(GroupId);
                    command.Parameters.Add(Branch);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public RetailSearchResponse InsertPerson(string BprCode)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string insertSql = "INSERT INTO dbo.BKT_PERSON " +
                               "(BPR_CODE, BPR_TYPE, BPR_STATUS, BPR_DATE) " +
                               "VALUES (@Code, @Type, @Status, @Date)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    SqlParameter Code = new SqlParameter("@Code", BprCode);
                    SqlParameter Type = new SqlParameter("@Type", 1);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    SqlParameter Date = new SqlParameter("@Date", System.DateTime.Now);
                    command.Parameters.Add(Code);
                    command.Parameters.Add(Type);
                    command.Parameters.Add(Status);
                    command.Parameters.Add(Date);
                    connection.Open();
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT BPR_ID FROM dbo.BKT_PERSON WHERE BPR_CODE LIKE @Code";
                    response.Id = Convert.ToInt64(command.ExecuteScalar());
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public RetailSearchResponse InsertRetailSpouse(Cls_Loan loan)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                //Retail ID here is in fact the Spouse ID and the SpouseID is the ID of the requester for the loan
                string insertSql = "INSERT INTO [AlpsPdb].[dbo].[BKT_REAL_PERSON] " +
                    "([BRP_BPR_ID],[BRP_SPOUSE_ID],[BRP_FIRST_NAME],[BRP_LAST_NAME],[BRP_BIRTH_DATE],[BRP_FATHER_NAME], " +
                    "[BRP_BIRTH_CERTID],[BRP_IDENTIFICATION_ID]," +
                    "[BRP_MARIAGE_ID],[BRP_SEX_ID],[BRP_EMPLOY_ID],[BRP_EDU_DEGREE_ID]," +
                    "[BRP_HEALTHSTATE_ID],[BRP_TYPE_ID],[BRP_ISSUE_PLACE]) " +
                    "VALUES (@RetailID, @SpouseID, @FirstName, @LastName, @BirthDate, @FatherName, " +
                    "@BirthCertid, @IdentificationId, @MARIAGE_ID, @SEX_ID, @EMPLOY_ID, @EDU_DEGREE_ID, " +
                    "@HEALTHSTATE_ID, @TYPE_ID, @IssuePlace)";
                string updateSql = "UPDATE [AlpsPdb].[dbo].[BKT_REAL_PERSON] " +
                    "SET [BRP_FIRST_NAME] = @FirstName, [BRP_LAST_NAME] = @LastName," +
                    "[BRP_BIRTH_DATE] = @BirthDate, [BRP_FATHER_NAME] = @FatherName," +
                    "[BRP_BIRTH_CERTID] = @BirthCertid, [BRP_IDENTIFICATION_ID] = @IdentificationId, " +
                    "[BRP_MARIAGE_ID] = @MARIAGE_ID, [BRP_SEX_ID] = @SEX_ID," +
                    "[BRP_EMPLOY_ID] = @EMPLOY_ID, [BRP_EDU_DEGREE_ID] = @EDU_DEGREE_ID, " +
                    "[BRP_HEALTHSTATE_ID] = @HEALTHSTATE_ID, [BRP_ISSUE_PLACE] = @IssuePlace " +
                    " WHERE [BRP_BPR_ID] = @RetailID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    if (loan.bktCustomer.bktSpouse.bktPerson.BprId > 0)
                    {
                        command.CommandText = updateSql;
                    }
                    else
                    {
                        command.CommandText = insertSql;
                    }

                    SqlParameter RetailID = new SqlParameter("@RetailID", loan.bktCustomer.bktSpouse.bktPerson.BprId);
                    SqlParameter SpouseID = new SqlParameter("@SpouseID", loan.bktCustomer.bktPerson.BprId);
                    SqlParameter FirstName = new SqlParameter("@FirstName", loan.bktCustomer.bktSpouse.BrpFirstName);
                    SqlParameter LastName = new SqlParameter("@LastName", loan.bktCustomer.bktSpouse.BrpLastName);
                    SqlParameter BirthDate = new SqlParameter("@BirthDate", loan.bktCustomer.bktSpouse.BrpBirthDate);
                    SqlParameter FatherName = new SqlParameter("@FatherName", loan.bktCustomer.bktSpouse.BrpFatherName);
                    SqlParameter BirthCertid = new SqlParameter("@BirthCertid", loan.bktCustomer.bktSpouse.BrpBirthCertid);
                    SqlParameter IdentificationId = new SqlParameter("@IdentificationId", loan.bktCustomer.bktSpouse.BrpIdentificationId);
                    SqlParameter MARIAGE_ID = new SqlParameter("@MARIAGE_ID", loan.bktCustomer.bktSpouse.BRP_MARIAGE_ID);
                    SqlParameter SEX_ID = new SqlParameter("@SEX_ID", loan.bktCustomer.bktSpouse.BRP_SEX_ID);
                    SqlParameter EMPLOY_ID = new SqlParameter("@EMPLOY_ID", loan.bktCustomer.bktSpouse.BRP_EMPLOY_ID);
                    SqlParameter EDU_DEGREE_ID = new SqlParameter("@EDU_DEGREE_ID", loan.bktCustomer.bktSpouse.BRP_EDU_DEGREE_ID);
                    SqlParameter HEALTHSTATE_ID = new SqlParameter("@HEALTHSTATE_ID", loan.bktCustomer.bktSpouse.BRP_HEALTHSTATE_ID);
                    SqlParameter TYPE_ID = new SqlParameter("@TYPE_ID", loan.bktCustomer.bktSpouse.BRP_TYPE_ID);
                    SqlParameter IssuePlace = new SqlParameter("@IssuePlace", loan.bktCustomer.bktSpouse.BrpIssuePlace);
                    SqlParameter BprID = new SqlParameter("@BprID", loan.bktCustomer.bktSpouse.bktPerson.BprId.ToString());
                    command.Parameters.Add(RetailID);
                    command.Parameters.Add(SpouseID);
                    command.Parameters.Add(FirstName);
                    command.Parameters.Add(LastName);
                    command.Parameters.Add(BirthDate);
                    command.Parameters.Add(FatherName);
                    command.Parameters.Add(BirthCertid);
                    command.Parameters.Add(IdentificationId);
                    command.Parameters.Add(MARIAGE_ID);
                    command.Parameters.Add(SEX_ID);
                    command.Parameters.Add(EMPLOY_ID);
                    command.Parameters.Add(EDU_DEGREE_ID);
                    command.Parameters.Add(HEALTHSTATE_ID);
                    command.Parameters.Add(TYPE_ID);
                    command.Parameters.Add(IssuePlace);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse InsertAdditioanlData(Cls_Meta meta)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string selectSql = "Select [BMD_ID] From [AlpsPdb].[dbo].[BKT_META_DATA] WHERE [BMD_FOR_ID] = @ForId AND [BMD_SUBJECT_ID] = @PersonId AND [BMD_KEY] LIKE @Key";
                string insertSql = "INSERT INTO [AlpsPdb].[dbo].[BKT_META_DATA] " +
                    "([BMD_FOR_ID],[BMD_KEY],[BMD_VALUE],[BMD_SUBJECT_ID],[BMD_CREATE_DATE],[BMD_STATUS]) " +
                    "VALUES (@ForId, @Key, @Value, @PersonId, @Date, @Status)";
                string updateSql = "UPDATE [AlpsPdb].[dbo].[BKT_META_DATA] " +
                    "SET [BMD_VALUE] = @Value, [BMD_CREATE_DATE] = @Date, [BMD_STATUS] = 2 " +
                    "WHERE [BMD_FOR_ID] = @ForId AND [BMD_SUBJECT_ID] = @PersonId AND [BMD_KEY] LIKE @Key";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = selectSql;
                    SqlParameter ForId = new SqlParameter("@ForId", meta.BMD_FOR_ID);
                    SqlParameter Key = new SqlParameter("@Key", meta.BmdKey);
                    SqlParameter Value = new SqlParameter("@Value", meta.BmdValue);
                    SqlParameter PersonId = new SqlParameter("@PersonId", meta.BmdSubjectId);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    SqlParameter Date = new SqlParameter("@Date", System.DateTime.Now);
                    command.Parameters.Add(ForId);
                    command.Parameters.Add(Key);
                    command.Parameters.Add(Value);
                    command.Parameters.Add(PersonId);
                    command.Parameters.Add(Status);
                    command.Parameters.Add(Date);
                    connection.Open();
                    long BmdId = Convert.ToInt64(command.ExecuteScalar());
                    if (BmdId > 0)
                        command.CommandText = updateSql;
                    else
                        command.CommandText = insertSql;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse InsertJob(Cls_Job job, long BprID)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string selectSql = "SELECT BJO_ID FROM [AlpsPdb].[dbo].[BKT_JOBS] " +
                    " WHERE [BJO_BPR_ID] = " + BprID.ToString() +
                    " AND [BJO_SEQ] = " + job.BjoSeq.ToString();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = selectSql;
                    connection.Open();
                    if (Convert.ToInt64(command.ExecuteScalar()) > 0)
                    {
                        command.CommandText = "UPDATE [AlpsPdb].[dbo].[BKT_JOBS] " +
                                    "SET [BJO_POSITION_ID] = " + job.Bjo_PositionId.ToString() + "," +
                                    "[BJO_PLACE_TITLE] = N'" + job.BjoPlaceTitle + "'," +
                                    "[BJO_DURATION] = " + job.BjoDuration.ToString() + "," +
                                    "[BJO_CREATE_DATE] = '" + job.BjoCreateDate.ToString() + "'," +
                                    "[BJO_STATUS] = " + job.BjoStatus +
                                    " WHERE [BJO_BPR_ID] = " + BprID.ToString() + " AND [BJO_SEQ] = " + job.BjoSeq.ToString();
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO [AlpsPdb].[dbo].[BKT_JOBS] " +
                    "([BJO_POSITION_ID],[BJO_BPR_ID],[BJO_PLACE_TITLE],[BJO_SEQ],[BJO_DURATION],[BJO_CREATE_DATE],[BJO_STATUS]) " +
                    "VALUES (" + job.Bjo_PositionId.ToString() + "," + BprID.ToString() + ",N'" + job.BjoPlaceTitle + "'," +
                    job.BjoSeq.ToString() + "," + job.BjoDuration.ToString() + ",'" + job.BjoCreateDate.ToString() + "'," + job.BjoStatus + ")";
                    }
                    command.ExecuteNonQuery();
                    connection.Open();
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT BJO_ID FROM dbo.BKT_JOBS WHERE BJO_BPR_ID = " + BprID.ToString() +
                                  " AND BJO_SEQ = 1 AND BJO_STATUS = 1";
                    response.Id = Convert.ToInt64(command.ExecuteScalar());
                    connection.Close();
                }

                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public UserResponse Lookup(string query)
        {
            UserResponse response = new UserResponse();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = query;
                    connection.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        command.Connection = connection;
                        sda.SelectCommand = command;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds, "lkTable");
                            response.Table = ds.Tables["lkTable"];
                        }
                    }
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.Message;
                response.Success = false;
            }
            return response;
        }
        public UserResponse DeleteUser(string UserName)
        {
            UserResponse response = new UserResponse();
            try
            {
                string deletetSql = "Delete Users Where User_Name = @UserName";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deletetSql;
                    SqlParameter User = new SqlParameter("@UserName", UserName);
                    command.Parameters.Add(User);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public UserResponse InsertProfile(UserRequest request)
        {
            UserResponse response = new UserResponse();
            try
            {
                string InsertSql = "INSERT INTO Profile (Profile_Id, CreatedBy)" +
                                    "Values (@ProfileId, @CreatedBy)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = InsertSql;
                    SqlParameter Profile = new SqlParameter("@ProfileId", request.Profile);
                    SqlParameter CreatedBy = new SqlParameter("@CreatedBy", request.CreatedBy);
                    command.Parameters.Add(Profile);
                    command.Parameters.Add(CreatedBy);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public UserResponse UpdateProfile(UserRequest request)
        {
            UserResponse response = new UserResponse();
            try
            {
                string UpdateSql = "Update Users set Act_Profile_Id = @Profile Where User_Name = @UserName ";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = UpdateSql;
                    SqlParameter Profile = new SqlParameter("@Profile", request.Profile);
                    SqlParameter User = new SqlParameter("@UserName", request.UserName);
                    command.Parameters.Add(Profile);
                    command.Parameters.Add(User);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public UserResponse UpdateGroup(UserRequest request)
        {
            UserResponse response = new UserResponse();
            try
            {
                string UpdateSql = "Update Users set User_Group_Id = @GroupID Where User_Id = @UserID ";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = UpdateSql;
                    SqlParameter Group = new SqlParameter("@GroupID", request.Group);
                    SqlParameter User = new SqlParameter("@UserID", request.UserName);
                    command.Parameters.Add(Group);
                    command.Parameters.Add(User);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public UserResponse GroupAccess(UserRequest request)
        {
            UserResponse response = new UserResponse();
            try
            {
                string UpdateSql = "Update Groups set AccessLevel = @Access Where ID = @Group ";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = UpdateSql;
                    SqlParameter Access = new SqlParameter("@Access", request.AccessId);
                    SqlParameter Group = new SqlParameter("@Group", request.GroupId);
                    command.Parameters.Add(Group);
                    command.Parameters.Add(Access);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public UserResponse DeleteGroup(int GroupID)
        {
            UserResponse response = new UserResponse();
            try
            {
                string deletetSql = "Delete Groups Where ID = @GroupID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deletetSql;
                    SqlParameter Group = new SqlParameter("@GroupID", GroupID);
                    command.Parameters.Add(Group);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public UserResponse InsertGroup(UserRequest request)
        {
            UserResponse response = new UserResponse();
            try
            {
                string deletetSql = "Insert Into Groups (GroupName, AccessLevel) Values (@GroupName,@Access)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deletetSql;
                    SqlParameter Group = new SqlParameter("@GroupName", request.Group);
                    SqlParameter Access = new SqlParameter("@Access", request.AccessId);
                    command.Parameters.Add(Group);
                    command.Parameters.Add(Access);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse InsertActualLoans(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string insertSql = "INSERT INTO [AlpsPdb].[dbo].BKT_RELATED_ITEMS " +
                                        "([BRI_TYPE_ID],[BRI_KIND_ID],[BRI_BLO_ID],[BRI_INSTITUTION_NAME]," +
                                        "[BRI_AMOUNT],[BRI_PAYMENT_NBR],[BRI_PAYMENT],[BRI_REMAIN],[BRI_CREATE_DATE],[BRI_STATUS]) " +
                                        "VALUES (@TypeID,@KindID,@BloID,@Institution,@Amount,@PaymentNbr,@Payment,@Remain,@CreateDate,@Status)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    SqlParameter TypeID = new SqlParameter("@TypeID", 112);
                    SqlParameter KindID = new SqlParameter("@KindID", request.LoanTypeId);
                    SqlParameter BloID = new SqlParameter("@BloID", request.BloID);
                    SqlParameter Institution = new SqlParameter("@Institution", request.Institution);
                    SqlParameter Amount = new SqlParameter("@Amount", request.FacilityAmount);
                    SqlParameter PaymentNbr = new SqlParameter("@PaymentNbr", request.PaymentNumber);
                    SqlParameter Payment = new SqlParameter("@Payment", request.Payment);
                    SqlParameter Remain = new SqlParameter("@Remain", request.Remain);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(KindID);
                    command.Parameters.Add(BloID);
                    command.Parameters.Add(Institution);
                    command.Parameters.Add(Amount);
                    command.Parameters.Add(PaymentNbr);
                    command.Parameters.Add(Payment);
                    command.Parameters.Add(Remain);
                    command.Parameters.Add(CreateDate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse UpdateActualLoans(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string updateSql = "UPDATE [AlpsPdb].[dbo].BKT_RELATED_ITEMS " +
                                   "SET [BRI_KIND_ID] = @KindID,[BRI_INSTITUTION_NAME] = @Institution," +
                                   "[BRI_AMOUNT]= @Amount,[BRI_PAYMENT_NBR]=@PaymentNbr,[BRI_PAYMENT]=@Payment, " +
                                   "[BRI_REMAIN]= @Remain,[BRI_CREATE_DATE]=@CreateDate,[BRI_STATUS]=@Status " +
                                   "WHERE BRI_ID=@BriID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    SqlParameter KindID = new SqlParameter("@KindID", request.LoanTypeId);
                    SqlParameter BriID = new SqlParameter("@BriID", request.BriID);
                    SqlParameter Institution = new SqlParameter("@Institution", request.Institution);
                    SqlParameter Amount = new SqlParameter("@Amount", request.FacilityAmount);
                    SqlParameter PaymentNbr = new SqlParameter("@PaymentNbr", request.PaymentNumber);
                    SqlParameter Payment = new SqlParameter("@Payment", request.Payment);
                    SqlParameter Remain = new SqlParameter("@Remain", request.Remain);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 2);
                    command.Parameters.Add(KindID);
                    command.Parameters.Add(BriID);
                    command.Parameters.Add(Institution);
                    command.Parameters.Add(Amount);
                    command.Parameters.Add(PaymentNbr);
                    command.Parameters.Add(Payment);
                    command.Parameters.Add(Remain);
                    command.Parameters.Add(CreateDate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse DeleteActualLoans(long BriID)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string deletetSql = "Delete [AlpsPdb].[dbo].BKT_RELATED_ITEMS Where BRI_ID = @BriID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deletetSql;
                    SqlParameter Loan = new SqlParameter("@BriID", BriID);
                    command.Parameters.Add(Loan);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public RetailSearchResponse InsertExpenses(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string insertSql = "INSERT INTO [AlpsPdb].[dbo].BKT_EXPENDITURES " +
                                   "([BEX_TYPE_ID],[BEX_BLO_ID],[BEX_TITLE],[BEX_AMOUNT],[BEX_CREATE_DATE],[BEX_STATUS]) " +
                                   "VALUES (@TypeID, @BloID,@Title,@Amount,@CreateDate,@Status)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    SqlParameter TypeID = new SqlParameter("@TypeID", request.ExpendTypeId);
                    SqlParameter BloID = new SqlParameter("@BloID", request.BloID);
                    SqlParameter Amount = new SqlParameter("@Amount", request.ExpendAmount);
                    SqlParameter Title = new SqlParameter("@Title", request.ExpendTitle);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(BloID);
                    command.Parameters.Add(Amount);
                    command.Parameters.Add(Title);
                    command.Parameters.Add(CreateDate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                    command.CommandText = "UPDATE [AlpsPdb].[dbo].[BKT_LOANS] " +
                                    "SET [BLO_FLGEXPEND] = 'True',[BLO_CREATE_DATE] = " + System.DateTime.Now.ToString() +
                                    ",[BLO_STATUS] = 2 " +
                                    "WHERE BLO_ID = " + request.BloID.ToString();
                    command.ExecuteNonQuery();

                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse UpdateExpenses(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string updateSql = "UPDATE [AlpsPdb].[dbo].BKT_EXPENDITURES " +
                                        "SET [BEX_TYPE_ID] = @TypeID ," +
                                        "[BEX_TITLE] = @Title ," +
                                        "[BEX_DESC] = @Desc ," +
                                        "[BEX_AMOUNT] = @Amount ," +
                                        "[BEX_CREATE_DATE] = @CreateDate," +
                                        "[BEX_STATUS] = @Status " +
                                        "WHERE BEX_ID = @BexID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    SqlParameter TypeID = new SqlParameter("@TypeID", request.ExpendTypeId);
                    SqlParameter BexID = new SqlParameter("@BexID", request.BexID);
                    SqlParameter Title = new SqlParameter("@Title", request.ExpendTitle);
                    SqlParameter Amount = new SqlParameter("@Amount", request.ExpendAmount);
                    SqlParameter Desc = new SqlParameter("@Desc", request.Desc);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 2);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(BexID);
                    command.Parameters.Add(Title);
                    command.Parameters.Add(Amount);
                    command.Parameters.Add(Desc);
                    command.Parameters.Add(CreateDate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse DeleteExpenses(long BexID)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string deletetSql = "Delete [AlpsPdb].[dbo].BKT_EXPENDITURES Where BEX_ID = @BexID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deletetSql;
                    SqlParameter Expend = new SqlParameter("@BexID", BexID);
                    command.Parameters.Add(Expend);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public RetailSearchResponse InsertIncome(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string insertSql = "INSERT INTO [AlpsPdb].[dbo].BKT_INCOMES " +
                                         "([BIN_TYPE_ID],[BIN_BLO_ID],[BIN_TITLE],[BIN_AMOUNT],[BIN_CREATE_DATE],[BIN_STATUS]) " +
                                         "VALUES (@TypeID, @BloID, @Title, @Amount, @CreateDate, @Status)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    SqlParameter TypeID = new SqlParameter("@TypeID", request.IncomeTypeId);
                    SqlParameter BloID = new SqlParameter("@BloID", request.BloID);
                    SqlParameter Amount = new SqlParameter("@Amount", request.IncomeAmount);
                    SqlParameter Title = new SqlParameter("@Title", request.Desc);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(BloID);
                    command.Parameters.Add(Amount);
                    command.Parameters.Add(Title);
                    command.Parameters.Add(CreateDate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                    command.CommandText = "UPDATE [AlpsPdb].[dbo].[BKT_LOANS] " +
                                    "SET [BLO_FLGINCOME] = 'True',[BLO_CREATE_DATE] = " + System.DateTime.Now.ToString() +
                                    ",[BLO_STATUS] = 2 " +
                                    "WHERE BLO_ID = " + request.BloID.ToString();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse UpdateIncome(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string updateSql = "UPDATE [AlpsPdb].[dbo].BKT_INCOMES " +
                                        "SET [BIN_TYPE_ID] = @TypeID ," +
                                        "[BIN_TITLE] = @Title," +
                                        "[BIN_AMOUNT] = @Amount ," +
                                        "[BIN_DESC] = @Desc," +
                                        "[BIN_CREATE_DATE] = @CreateDate," +
                                        "[BIN_STATUS] = 2 " +
                                        "WHERE BIN_ID = @BinId";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    SqlParameter TypeID = new SqlParameter("@TypeID", request.IncomeTypeId);
                    SqlParameter BinID = new SqlParameter("@BinID", request.BinID);
                    SqlParameter Title = new SqlParameter("@Title", request.IncomeTitle);
                    SqlParameter Amount = new SqlParameter("@Amount", request.IncomeAmount);
                    SqlParameter Desc = new SqlParameter("@Desc", request.Desc);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 2);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(BinID);
                    command.Parameters.Add(Title);
                    command.Parameters.Add(Amount);
                    command.Parameters.Add(Desc);
                    command.Parameters.Add(CreateDate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse DeleteIncome(long BinID)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string deletetSql = "Delete [AlpsPdb].[dbo].BKT_INCOMES Where BIN_ID = @BinID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deletetSql;
                    SqlParameter Income = new SqlParameter("@BinID", BinID);
                    command.Parameters.Add(Income);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public RetailSearchResponse UpdateAsset(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string updateSql = "UPDATE [AlpsPdb].[dbo].BKT_ASSETS " +
                                       "SET [BSS_TITLE] = @Title, " +
                                       "[BSS_DESC] = @Desc, " +
                                       "[BSS_AMOUNT] = @Amount, " +
                                       "[BSS_CREATE_DATE] = @CreateDate," +
                                       "[BSS_STATUS] = 2" +
                                       "WHERE BSS_ID = @BssID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    //SqlParameter TypeID = new SqlParameter("@TypeID", request.IncomeTypeId);
                    SqlParameter BssID = new SqlParameter("@BssID", request.BssID);
                    SqlParameter Title = new SqlParameter("@Title", request.AssetTitle);
                    SqlParameter Amount = new SqlParameter("@Amount", request.AssetAmount);
                    SqlParameter Desc = new SqlParameter("@Desc", request.Desc);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 2);
                    //command.Parameters.Add(TypeID);
                    command.Parameters.Add(BssID);
                    command.Parameters.Add(Title);
                    command.Parameters.Add(Amount);
                    command.Parameters.Add(Desc);
                    command.Parameters.Add(CreateDate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse DeleteAsset(long BssID)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string deletetSql = "Delete [AlpsPdb].[dbo].BKT_ASSETS Where BSS_ID = @BssID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deletetSql;
                    SqlParameter Asset = new SqlParameter("@BssID", BssID);
                    command.Parameters.Add(Asset);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }
        public RetailSearchResponse InsertAssets(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string insertSql = "INSERT INTO [AlpsPdb].[dbo].BKT_ASSETS " +
                                   "([BSS_BLO_ID],[BSS_TITLE],[BSS_DESC],[BSS_AMOUNT],[BSS_CREATE_DATE],[BSS_STATUS]) " +
                                   "VALUES (@BloID, @Title, @Desc, @Amount, @CreateDate, @Status)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    SqlParameter Desc = new SqlParameter("@Desc", request.Desc);
                    SqlParameter BloID = new SqlParameter("@BloID", request.BloID);
                    SqlParameter Amount = new SqlParameter("@Amount", request.AssetAmount);
                    SqlParameter Title = new SqlParameter("@Title", request.AssetTitle);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    command.Parameters.Add(Desc);
                    command.Parameters.Add(BloID);
                    command.Parameters.Add(Amount);
                    command.Parameters.Add(Title);
                    command.Parameters.Add(CreateDate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                    command.CommandText = " UPDATE [AlpsPdb].[dbo].[BKT_LOANS] " +
                                    "SET [BLO_FLGASSET] = 'True',[BLO_CREATE_DATE] = getdate() " +
                                    ",[BLO_STATUS] = 2 " +
                                    "WHERE BLO_ID = " + request.BloID.ToString();
                    command.ExecuteNonQuery();

                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse InsertGuaranty(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string insertSql = "INSERT INTO [AlpsPdb].[dbo].BKT_RELATED_ITEMS " +
                                   "([BRI_TYPE_ID],[BRI_BLO_ID],[BRI_INSTITUTION_NAME]," +
                                   "[BRI_AMOUNT],[BRI_PAYMENT_NBR],[BRI_PAYMENT],[BRI_REMAIN],[BRI_CREATE_DATE],[BRI_STATUS]) " +
                                   "VALUES (@TypeID,@BloID,@Institution,@Amount,@PaymentNbr,@Payment,@Remain,@CreateDate,@Status)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    SqlParameter TypeID = new SqlParameter("@TypeID", 113);
                    SqlParameter BloID = new SqlParameter("@BloID", request.BloID);
                    SqlParameter Institution = new SqlParameter("@Institution", request.Institution);
                    SqlParameter Amount = new SqlParameter("@Amount", request.FacilityAmount);
                    SqlParameter PaymentNbr = new SqlParameter("@PaymentNbr", request.PaymentNumber);
                    SqlParameter Payment = new SqlParameter("@Payment", request.Payment);
                    SqlParameter Remain = new SqlParameter("@Remain", request.Remain);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(BloID);
                    command.Parameters.Add(Institution);
                    command.Parameters.Add(Amount);
                    command.Parameters.Add(PaymentNbr);
                    command.Parameters.Add(Payment);
                    command.Parameters.Add(Remain);
                    command.Parameters.Add(CreateDate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse UpdateGuaranty(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string updateSql = "UPDATE [AlpsPdb].[dbo].BKT_RELATED_ITEMS " +
                                   "SET [BRI_INSTITUTION_NAME] = @Institution," +
                                   "[BRI_AMOUNT]= @Amount,[BRI_PAYMENT_NBR]=@PaymentNbr,[BRI_PAYMENT]=@Payment, " +
                                   "[BRI_REMAIN]= @Remain,[BRI_CREATE_DATE]=@CreateDate,[BRI_STATUS]=@Status " +
                                   "WHERE BRI_ID=@BriID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    SqlParameter BriID = new SqlParameter("@BriID", request.BriID);
                    SqlParameter Institution = new SqlParameter("@Institution", request.Institution);
                    SqlParameter Amount = new SqlParameter("@Amount", request.FacilityAmount);
                    SqlParameter PaymentNbr = new SqlParameter("@PaymentNbr", request.PaymentNumber);
                    SqlParameter Payment = new SqlParameter("@Payment", request.Payment);
                    SqlParameter Remain = new SqlParameter("@Remain", request.Remain);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 2);
                    command.Parameters.Add(BriID);
                    command.Parameters.Add(Institution);
                    command.Parameters.Add(Amount);
                    command.Parameters.Add(PaymentNbr);
                    command.Parameters.Add(Payment);
                    command.Parameters.Add(Remain);
                    command.Parameters.Add(CreateDate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse InsertAccount(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string insertSql = "INSERT INTO [AlpsPdb].[dbo].BKT_ACCOUNTS " +
                                   "([BKA_TYPE_ID],[BKA_BBN_ID],[BKA_BLO_ID]," +
                                   "[BKA_CAPITAL],[BKA_TURNOVER],[BKA_OPEN_DATE],[BKA_STATUS]) " +
                                   "VALUES (@TypeID, @BankID, @BloID, @Capital, @Turnover, @Opendate, @Status) ";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    SqlParameter TypeID = new SqlParameter("@TypeID", request.AccountTypeId);
                    SqlParameter BloID = new SqlParameter("@BloID", request.BloID);
                    SqlParameter BankID = new SqlParameter("@BankID", request.BankId);
                    SqlParameter Capital = new SqlParameter("@Capital", request.AccountAmount);
                    SqlParameter Turnover = new SqlParameter("@Turnover", request.AccountTurnOver);
                    SqlParameter Opendate = new SqlParameter("@Opendate", request.OpeningDate);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(BloID);
                    command.Parameters.Add(BankID);
                    command.Parameters.Add(Capital);
                    command.Parameters.Add(Turnover);
                    command.Parameters.Add(Opendate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse UpdateAccount(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string updateSql = "UPDATE [AlpsPdb].[dbo].BKT_ACCOUNTS " +
                                       "SET [BKA_TYPE_ID] = @TypeID , [BKA_BBN_ID] = @BankID," +
                                       "[BKA_CAPITAL] = @Capital , [BKA_TURNOVER] = @Turnover," +
                                       "[BKA_OPEN_DATE] = @Opendate, [BKA_STATUS] = 2 " +
                                       "WHERE BKA_ID = @BkaID ";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    SqlParameter TypeID = new SqlParameter("@TypeID", request.AccountTypeId);
                    SqlParameter BkaID = new SqlParameter("@BkaID", request.BkaID);
                    SqlParameter BankID = new SqlParameter("@BankID", request.BankId);
                    SqlParameter Capital = new SqlParameter("@Capital", request.AccountAmount);
                    SqlParameter Turnover = new SqlParameter("@Turnover", request.AccountTurnOver);
                    SqlParameter Opendate = new SqlParameter("@Opendate", request.OpeningDate);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(BkaID);
                    command.Parameters.Add(BankID);
                    command.Parameters.Add(Capital);
                    command.Parameters.Add(Turnover);
                    command.Parameters.Add(Opendate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse UpdateLoan(Cls_Loan loan)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string updateSql = "UPDATE [AlpsPdb].[dbo].[BKT_LOANS] " +
                                "SET BLO_LOANTYPE_ID = @LoanMajorType ," +
                                "BLO_LOANTYPE2_ID = @LoanDepositType ," +
                                "BLO_LOANREQUEST_ID = @RequestType ," +
                                "BLO_ECONOMICSECTOR_ID = @EconomicSector ," +
                                "BLO_CONTRACTTYPE_ID = @LoanType ," +
                                "BLO_CUSTOMERGROUP_ID = @CustomerGroup ," +
                                "BLO_LOANSTATE = @LoanState ," +
                                "BLO_LOANAIM = @LoanAim ," +
                                "BLO_BCU_ID = @Currency ," +
                                "BLO_BRANCH_CODE = @Branch ," +
                                "BLO_AMOUNT = @Amount ," +
                                "BLO_PAYMENT = @Payment ," +
                                "BLO_PAYMENT_NBR = @PaymentNbr," +
                                "BLO_PAYMENT_PERIOD = @PaymentPeriod ," +
                                "BLO_ACCOUNTNO = @AccountNo ," +
                                "BLO_ASSURANCE1 = @Assurance1 ," +
                                "BLO_ASSURANCE2 = @Assurance2 ," +
                                "BLO_CREATE_DATE = @Now ," +
                                "BLO_MATURITY_DATE = @MaturityDate ," +
                                "BLO_STATUS = @Status, BLO_FLGLOAN = 'True'";
                if (loan.BloCode != null)
                {
                    updateSql = updateSql + ", BLO_CODE = @loanCode " +
                        " WHERE BLO_ID = @BloId ";
                }
                else
                {
                    updateSql = updateSql + " WHERE BLO_ID = @BloId ";
                }
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    SqlParameter LoanCode = new SqlParameter("@LoanCode", loan.BloCode);
                    SqlParameter BloId = new SqlParameter("@BloId", loan.BloId);
                    SqlParameter LoanMajorType = new SqlParameter("@LoanMajorType", loan.BLO_LoanTypeId);
                    SqlParameter LoanDepositType = new SqlParameter("@LoanDepositType", loan.BLO_LoanType2Id);
                    SqlParameter RequestType = new SqlParameter("@RequestType", loan.BLO_LoanRequest);
                    SqlParameter EconomicSector = new SqlParameter("@EconomicSector", loan.BLO_EconomicSector);
                    SqlParameter LoanType = new SqlParameter("@LoanType", loan.BLO_ContractType);
                    SqlParameter CustomerGroup = new SqlParameter("@CustomerGroup", loan.BLO_CustomerGroup);
                    SqlParameter LoanState = new SqlParameter("@LoanState", loan.BLO_LoanState);
                    SqlParameter LoanAim = new SqlParameter("@LoanAim", loan.BLO_LoanAim);
                    SqlParameter Currency = new SqlParameter("@Currency", loan.BLO_BCU_ID);
                    SqlParameter Branch = new SqlParameter("@Branch", loan.BloBranchCode);
                    SqlParameter Amount = new SqlParameter("@Amount", loan.BloAmount);
                    SqlParameter Payment = new SqlParameter("@Payment", loan.BloPayment);
                    SqlParameter PaymentNbr = new SqlParameter("@PaymentNbr", loan.BloPaymentNbr);
                    SqlParameter PaymentPeriod = new SqlParameter("@PaymentPeriod", loan.BloPaymentPeriod);
                    SqlParameter AccountNo = new SqlParameter("@AccountNo", loan.BloAccountNo);
                    SqlParameter Assurance1 = new SqlParameter("@Assurance1", loan.BloAssurance1);
                    SqlParameter Assurance2 = new SqlParameter("@Assurance2", loan.BloAssurance2);
                    SqlParameter Now = new SqlParameter("@Now", DateTime.Now);
                    SqlParameter MaturityDate = new SqlParameter("@MaturityDate", loan.BloMaturityDate);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    command.Parameters.Add(LoanCode);
                    command.Parameters.Add(BloId);
                    command.Parameters.Add(LoanMajorType);
                    command.Parameters.Add(LoanDepositType);
                    command.Parameters.Add(RequestType);
                    command.Parameters.Add(EconomicSector);
                    command.Parameters.Add(LoanType);
                    command.Parameters.Add(CustomerGroup);
                    command.Parameters.Add(LoanState);
                    command.Parameters.Add(LoanAim);
                    command.Parameters.Add(Currency);
                    command.Parameters.Add(Branch);
                    command.Parameters.Add(Amount);
                    command.Parameters.Add(Payment);
                    command.Parameters.Add(PaymentNbr);
                    command.Parameters.Add(PaymentPeriod);
                    command.Parameters.Add(AccountNo);
                    command.Parameters.Add(Assurance1);
                    command.Parameters.Add(Assurance2);
                    command.Parameters.Add(Now);
                    command.Parameters.Add(MaturityDate);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse DeleteAccount(long BkaID)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string deletetSql = "Delete [AlpsPdb].[dbo].BKT_ACCOUNTS Where BKA_ID = @BkaID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deletetSql;
                    SqlParameter Account = new SqlParameter("@BkaID", BkaID);
                    command.Parameters.Add(Account);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse UpdateFlag(int What, long BloID)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string UpdateSql = "";

                switch (What)
                {
                    case 1:
                        {
                            UpdateSql = "UPDATE [AlpsPdb].[dbo].[BKT_LOANS] SET [BLO_FLGSPOUSE] = @Flag,[BLO_CREATE_DATE] = @Date,[BLO_STATUS] = @Status WHERE BLO_ID = @BloID";
                            break;
                        }
                    case 21: //Beginning with 2 is for corporate
                        {
                            UpdateSql = "UPDATE [AlpsPdb].[dbo].[BKT_LOANS] SET [BLO_FLGFACTORY] = @Flag,[BLO_CREATE_DATE] = @Date,[BLO_STATUS] = @Status WHERE BLO_ID = @BloID";
                            break;
                        }

                }
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = UpdateSql;
                    SqlParameter Flag = new SqlParameter("@Flag", true);
                    SqlParameter Date = new SqlParameter("@Date", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 2);
                    SqlParameter BloId = new SqlParameter("@BloID", BloID);
                    command.Parameters.Add(Flag);
                    command.Parameters.Add(Date);
                    command.Parameters.Add(Status);
                    command.Parameters.Add(BloId);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse UpdateRetail(Cls_RealPerson Customer)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string updateSql = "UPDATE [AlpsPdb].[dbo].[BKT_REAL_PERSON] " +
                    "SET [BRP_FIRST_NAME] = @FirstName , [BRP_LAST_NAME] = @LastName ," +
                    "[BRP_BIRTH_DATE] = @BirthDate, [BRP_FATHER_NAME] = @FatherName ," +
                    "[BRP_BIRTH_CERTID] = @BirthCertid , [BRP_IDENTIFICATION_ID] = @IdentificationId ," +
                    "[BRP_PERSON_INCHARGE] = @PERSONINCHARGEID , [BRP_CHILDREN] = @CHILDRENID ," +
                    "[BRP_MARIAGE_ID] = @MARIAGEID , [BRP_SEX_ID] = @SEXID ," +
                    "[BRP_HOME_ID] = @HOMEID , [BRP_EMPLOY_ID] = @EMPLOYID ," +
                    "[BRP_EDU_DEGREE_ID] = @EDUDEGREEID , [BRP_SERVICEDUE_ID] = @SERVICEDUEID ," +
                    "[BRP_HEALTHSTATE_ID] = @HEALTHSTATEID , [BRP_CHILDSTATE_ID] = @CHILDSTATEID ," +
                    "[BRP_ISSUE_PLACE] = @IssuePlace , [BRP_RESIDENT_ID] = @RESIDENTID ," +
                    "[BRP_OZVIAT_ID] = @OZVIATID , [BRP_SHOHRAT_ID] = @SHOHRATID ," +
                    "[BRP_NIROOU_ID] = @NIROOUID  WHERE [BRP_BPR_ID] = " + Customer.bktPerson.BprId.ToString();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    SqlParameter FirstName = new SqlParameter("@FirstName", Customer.BrpFirstName);
                    SqlParameter LastName = new SqlParameter("@LastName", Customer.BrpLastName);
                    SqlParameter FatherName = new SqlParameter("@FatherName", Customer.BrpFatherName);
                    SqlParameter BirthDate = new SqlParameter("@BirthDate", Customer.BrpBirthDate);
                    SqlParameter BirthCertid = new SqlParameter("@BirthCertid", Customer.BrpBirthCertid);
                    SqlParameter IdentificationId = new SqlParameter("@IdentificationId", Customer.BrpIdentificationId);
                    SqlParameter PERSONINCHARGEID = new SqlParameter("@PERSONINCHARGEID", Customer.BRP_PERSONINCHARGE_ID);
                    SqlParameter CHILDRENID = new SqlParameter("@CHILDRENID", Customer.BRP_CHILDREN_ID);
                    SqlParameter MARIAGEID = new SqlParameter("@MARIAGEID", Customer.BRP_MARIAGE_ID);
                    SqlParameter SEXID = new SqlParameter("@SEXID", Customer.BRP_SEX_ID);
                    SqlParameter HOMEID = new SqlParameter("@HOMEID", Customer.BRP_HOME_ID);
                    SqlParameter EMPLOYID = new SqlParameter("@EMPLOYID", Customer.BRP_EMPLOY_ID);
                    SqlParameter EDUDEGREEID = new SqlParameter("@EDUDEGREEID", Customer.BRP_EDU_DEGREE_ID);
                    SqlParameter SERVICEDUEID = new SqlParameter("@SERVICEDUEID", Customer.BRP_SERVICEDUE_ID);
                    SqlParameter HEALTHSTATEID = new SqlParameter("@HEALTHSTATEID", Customer.BRP_HEALTHSTATE_ID);
                    SqlParameter CHILDSTATEID = new SqlParameter("@CHILDSTATEID", Customer.BRP_CHILDSTATE_ID);
                    SqlParameter IssuePlace = new SqlParameter("@IssuePlace", Customer.BrpIssuePlace);
                    SqlParameter RESIDENTID = new SqlParameter("@RESIDENTID", Customer.BRP_RESIDENT_ID);
                    SqlParameter OZVIATID = new SqlParameter("@OZVIATID", Customer.BRP_OZVIAT_ID);
                    SqlParameter SHOHRATID = new SqlParameter("@SHOHRATID", Customer.BRP_SHOHRAT_ID);
                    SqlParameter NIROOUID = new SqlParameter("@NIROOUID", Customer.BRP_NIROOU_ID);
                    command.Parameters.Add(FirstName);
                    command.Parameters.Add(LastName);
                    command.Parameters.Add(FatherName);
                    command.Parameters.Add(BirthDate);
                    command.Parameters.Add(BirthCertid);
                    command.Parameters.Add(IdentificationId);
                    command.Parameters.Add(PERSONINCHARGEID);
                    command.Parameters.Add(CHILDRENID);
                    command.Parameters.Add(MARIAGEID);
                    command.Parameters.Add(SEXID);
                    command.Parameters.Add(HOMEID);
                    command.Parameters.Add(EMPLOYID);
                    command.Parameters.Add(EDUDEGREEID);
                    command.Parameters.Add(SERVICEDUEID);
                    command.Parameters.Add(HEALTHSTATEID);
                    command.Parameters.Add(CHILDSTATEID);
                    command.Parameters.Add(IssuePlace);
                    command.Parameters.Add(RESIDENTID);
                    command.Parameters.Add(OZVIATID);
                    command.Parameters.Add(SHOHRATID);
                    command.Parameters.Add(NIROOUID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse InsertAddress(Cls_Address Address, Int64 BprID)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string selectSql = "SELECT BAP_ID FROM [AlpsPdb].[dbo].[BKT_ADDRESS_PERSON] " +
                    " WHERE [BAP_BPR_ID] = " + BprID.ToString() +
                    " AND [BAP_BBD_ID] = " + Address.Bap_TypeID.ToString() +
                    " AND [BAP_PRIORITY] = " + Address.BapPriority.ToString();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = selectSql;
                    connection.Open();
                    if (Convert.ToInt64(command.ExecuteScalar()) > 0)
                    {
                        command.CommandText = "UPDATE [AlpsPdb].[dbo].[BKT_ADDRESS_PERSON] " +
                            "SET [BAP_VALUE] = N'" + Address.BapValue + "'," +
                            "[BAP_STATUS] = " + Address.BapStatus +
                            " WHERE [BAP_BPR_ID] = " + BprID.ToString() +
                            " AND [BAP_BBD_ID] = " + Address.Bap_TypeID.ToString() +
                            " AND [BAP_PRIORITY] = " + Address.BapPriority.ToString();
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO [AlpsPdb].[dbo].[BKT_ADDRESS_PERSON] " +
                        "([BAP_BBD_ID],[BAP_BPR_ID],[BAP_VALUE],[BAP_PRIORITY],[BAP_CREATE_DATE],[BAP_STATUS]) " +
                        "VALUES (" + Address.Bap_TypeID + "," + BprID + ",N'" + Address.BapValue + "'," + Address.BapPriority + ",'" + DateTime.Now + "'," + Address.BapStatus + ")";
                    }
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse InsertJudg(RetailSearchRequest request)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string insertSql = "INSERT INTO dbo.AHP_Customer_Judge " +
                               "([Element_ID],[Value],[Cust_BPR_ID],[User_ID],[Create_DT],[Status]) " +
                               "VALUES (@ElementID,@Value,@BprId,@UserID,@Date,@Status)";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    SqlParameter ElementID = new SqlParameter("@ElementID", request.ElementId);
                    SqlParameter Value = new SqlParameter("@Value", request.Value);
                    SqlParameter BprId = new SqlParameter("@BprId", request.BprID);
                    SqlParameter UserID = new SqlParameter("@UserID", request.UserId);
                    SqlParameter Date = new SqlParameter("@Date", request.Date);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    command.Parameters.Add(ElementID);
                    command.Parameters.Add(Value);
                    command.Parameters.Add(BprId);
                    command.Parameters.Add(UserID);
                    command.Parameters.Add(Date);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public RetailSearchResponse UpdatePersonCode(Cls_Person Person)
        {
            RetailSearchResponse response = new RetailSearchResponse();
            try
            {
                string UpdateSql = "UPDATE dbo.BKT_PERSON " +
                             "SET BPR_CODE = @Code ,BPR_STATUS = 2, BPR_DATE = @Date " +
                             "WHERE BPR_ID = @BprID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = UpdateSql;
                    SqlParameter Code = new SqlParameter("@Code", Person.BprCode);
                    SqlParameter Date = new SqlParameter("@Date", Person.BprDate);
                    SqlParameter BprID = new SqlParameter("@BprID", Person.BprId);
                    command.Parameters.Add(Code);
                    command.Parameters.Add(Date);
                    command.Parameters.Add(BprID);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;

        }


    }
}