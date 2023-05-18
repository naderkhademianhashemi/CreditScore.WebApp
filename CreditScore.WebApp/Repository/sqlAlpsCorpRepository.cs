using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using AlpsCreditScoring.Models;
using AlpsCreditScoring.Service.Messages;
using CreditScore.WebApp;

namespace AlpsCreditScoring.Repository
{
    public class sqlAlpsCorpRepository : IAlpsCorpRepository
    {
        private string _connectionString;
        private System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("fa-IR");
        public sqlAlpsCorpRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["AlpsConnection"].ConnectionString;
        }
        public CorpSearchResponse FindAll(int pageIndex, int pageSize)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                #region Query
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("[GetCorpPageWise]"))
                    //using (SqlCommand cmd = new SqlCommand("[GetCorpPageWiseFA]"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                        cmd.Parameters.AddWithValue("@PageSize", pageSize);
                        cmd.Parameters.Add("@PageCount", SqlDbType.BigInt, 4).Direction = ParameterDirection.Output;
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            con.Open();
                            sda.SelectCommand = cmd;
                            using (DataSet ds = new DataSet())
                            {
                                sda.Fill(ds, "Loans");
                                response.count = Convert.ToInt64(cmd.Parameters["@PageCount"].Value);
                                response.Table = ds.Tables["Loans"];
                            }
                            con.Close();
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
        public CorpSearchResponse FindTableBy(CorpSearchRequest Request)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                #region Query
                string queryString = "SELECT * FROM dbo.V_BNK_LOANSEARCHCORPFA WHERE BCP_BPR_ID > 0  ";
                if (Request.bNational)
                    queryString += "AND BCP_IDENTIFICATION_ID = " + Request.National;
                if (Request.bCorpName)
                    queryString += "AND BCP_NAME LIKE N'%" + Request.CorpName + "% ' ";
                if (Request.bBranch)
                    queryString += "AND BLO_BRANCH_CODE LIKE '%" + Request.Branch + "% ' ";
                if (Request.bLoanCode)
                    queryString += "AND BLO_CODE LIKE '" + Request.LoanCode + "' ";
                if (Request.bAccountNo)
                    queryString += "AND BLO_ACCOUNTNO LIKE '" + Request.AccountNo + "' ";
                if (Request.bCustomerCode)
                    queryString += "AND BPR_CODE LIKE '" + Request.CustomerCode + "' ";
                if (Request.bRegisterNo)
                    queryString += "AND BCP_REGISTER_NO LIKE N'%" + Request.RegisterNo + "%'";
                if (Request.bCapitalKickoff)
                    queryString += "AND BCP_CAPITAL_KICKOFF >= " + Request.CapitalKickoff + " AND BCP_CAPITAL_KICKOFF < " + ((Convert.ToDouble(Request.CapitalKickoff) + Convert.ToDouble(Request.CapitalKickoff) * 0.1)).ToString();
                if (Request.bCreateDate1 && !Request.bCreateDate2)
                    queryString += "AND BLO_CREATE_DATE > '" + Request.bCreateDate1 + "'";
                if (Request.bCreateDate1 && Request.bCreateDate2)
                    queryString += "AND BLO_CREATE_DATE > '" + Request.bCreateDate1 + "'" +
                         "' AND BLO_CREATE_DATE < '" + Request.bCreateDate2;
                if (Request.bAmount)
                    queryString += "AND BLO_AMOUNT >= " + Request.Amount;
                if (Request.bCorpType)
                    queryString += "AND BCP_TYPE_ID = " + Request.CorpType;
                if (Request.bCorpActivity)
                    queryString += "AND BCP_ACTIVITY_ID = " + Request.CorpActivity;
                if (Request.bLoanType)
                    queryString += "AND BLO_LOANTYPE_ID = " + Request.LoanType;
                if (Request.bLoanState)
                    queryString += "AND BLO_LOANSTATE = " + Request.LoanState;
                if (Request.bBourse)
                    queryString += "AND BCP_IN_BOURSE = " + Request.Bourse;
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
        public CorpSearchResponse FindBy(CorpSearchRequest Request)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            Cls_Loan loan = new Cls_Loan();
            try
            {
                #region FindQuery
                //string queryString = "SELECT * FROM dbo.V_BNK_LOANSEARCHCORPFA WHERE BCP_BPR_ID > 0 AND BLO_ID = " + Request.BloID.ToString();
                string queryString = "SELECT * FROM dbo.V_BNK_LOANSEARCHCORP WHERE BCP_BPR_ID > 0 AND BLO_ID = " + Request.BloID.ToString();
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
                            loan.BloStatus = Convert.ToByte(reader["BLO_STATUS"]);
                            loan.bktCorp = new Cls_CorpPerson();
                            loan.bktCorp.bktPerson = new Cls_Person();
                            loan.bktCorp.bktPerson.BprId = Convert.ToInt64(reader["BCP_BPR_ID"]);
                            loan.BloId = Convert.ToInt64(reader["BLO_ID"]);
                            loan.BloCode = reader["BLO_CODE"].ToString();
                            loan.bktCorp.BcpName = reader["BCP_NAME"].ToString(); //till here
                            if (reader["BLO_CREATE_DATE"] != DBNull.Value && reader["BLO_CREATE_DATE"].ToString() != "1/1/0001")
                            {
                                loan.BloCreateDate = Convert.ToDateTime(reader["BLO_CREATE_DATE"].ToString());
                            }
                            if (reader["BLO_ECONOMICSECTOR_ID"] != DBNull.Value)
                            {
                                loan.BLO_EconomicSector = Convert.ToInt32(reader["BLO_ECONOMICSECTOR_ID"]);
                            }
                            if (reader["BLO_CONTRACTTYPE_ID"] != DBNull.Value)
                            {
                                loan.BLO_ContractType = Convert.ToInt32(reader["BLO_CONTRACTTYPE_ID"]);
                            }
                            if (reader["BLO_CUSTOMERGROUP_ID"] != DBNull.Value)
                            {
                                loan.BLO_CustomerGroup = Convert.ToInt32(reader["BLO_CUSTOMERGROUP_ID"]);
                            }
                            if (reader["BLO_LOANAIM"] != DBNull.Value)
                            {
                                loan.BLO_LoanAim = Convert.ToInt32(reader["BLO_LOANAIM"]);
                            }
                            if (reader["BLO_LOANSTATE"] != DBNull.Value)
                            {
                                loan.BLO_LoanState = Convert.ToInt32(reader["BLO_LOANSTATE"]);
                            }
                            if (reader["BLO_LOANTYPE_ID"] != DBNull.Value)
                            {
                                loan.BLO_LoanTypeId = Convert.ToInt32(reader["BLO_LOANTYPE_ID"]);
                            }
                            if (reader["BLO_LOANTYPE2_ID"] != DBNull.Value)
                            {
                                loan.BLO_LoanType2Id = Convert.ToInt32(reader["BLO_LOANTYPE2_ID"]);
                            }
                            if (reader["BLO_LOANREQUEST_ID"] != DBNull.Value)
                            {
                                loan.BLO_LoanRequest = Convert.ToInt32(reader["BLO_LOANREQUEST_ID"]);
                            }

                            if (reader["BLO_BRANCH_CODE"] != DBNull.Value)
                            {
                                loan.BloBranchCode = reader["BLO_BRANCH_CODE"].ToString();
                                //if (reader["State_Of_Branch"] != DBNull.Value)
                                    //loan.StateCode = reader["State_Of_Branch"].ToString();
                            }
                            if (reader["BLO_AMOUNT"] != DBNull.Value)
                                loan.BloAmount = Convert.ToDouble(reader["BLO_AMOUNT"]);
                            if (reader["BLO_ASSURANCE1"] != DBNull.Value)
                                loan.BloAssurance1 = Convert.ToByte(reader["BLO_ASSURANCE1"]);
                            if (reader["BLO_ASSURANCE2"] != DBNull.Value)
                                loan.BloAssurance1 = Convert.ToByte(reader["BLO_ASSURANCE2"]);
                            if (reader["BLO_ACCOUNTNO"] != DBNull.Value)
                                loan.BloAccountNo = reader["BLO_ACCOUNTNO"].ToString();
                            if (reader["BLO_PAYMENT"] != DBNull.Value)
                                loan.BloPayment = Convert.ToDouble(reader["BLO_PAYMENT"]);
                            if (reader["BLO_PAYMENT_NBR"] != DBNull.Value)
                                loan.BloPaymentNbr = Convert.ToInt32(reader["BLO_PAYMENT_NBR"]);
                            if (reader["BLO_PAYMENT_PERIOD"] != DBNull.Value)
                                loan.BloPaymentPeriod = Convert.ToByte(reader["BLO_PAYMENT_PERIOD"]);
                            if (reader["BLO_PAYMENT_DATE"] != DBNull.Value && reader["BLO_PAYMENT_DATE"].ToString() != "1/1/0001")
                                //Convert.ToDateTime(
                                //loan.BloPaymentDate = DateTime.ParseExact(reader["BLO_PAYMENT_DATE"].ToString(), "yyyy/mm/dd", culture);
                                loan.BloPaymentDate = Convert.ToDateTime(reader["BLO_PAYMENT_DATE"].ToString());
                            if (reader["BLO_MATURITY_DATE"] != DBNull.Value && reader["BLO_MATURITY_DATE"].ToString() != "1/1/0001")
                                //Convert.ToDateTime(
                                loan.BloMaturityDate = Convert.ToDateTime(reader["BLO_MATURITY_DATE"].ToString());
                                //loan.BloMaturityDate = DateTime.ParseExact(reader["BLO_MATURITY_DATE"].ToString(), "yyyy/mm/dd", culture);
                            if (reader["BLO_BCU_ID"] != DBNull.Value)
                                loan.BLO_BCU_ID = Convert.ToInt32(reader["BLO_BCU_ID"]);
                            loan.bktCorp.bktPerson.BprCode = reader["BPR_CODE"].ToString();
                            loan.bktCorp.bktPerson.BprType = 2;
                            loan.bktCorp.BcpName = reader["BCP_NAME"].ToString();
                            loan.bktCorp.BcpRegisterNo = reader["BCP_REGISTER_NO"].ToString();
                            loan.bktCorp.BcpIdentificationID = Convert.ToInt64(reader["BCP_IDENTIFICATION_ID"]);
                            loan.bktCorp.BcpRegisterPlace = reader["BCP_REGISTER_PLACE"].ToString();
                            if (reader["BCP_ESTABLISH_DATE"] != DBNull.Value && reader["BCP_ESTABLISH_DATE"].ToString() != "1/1/0001")
                                //Convert.ToDateTime(
                                //loan.bktCorp.BcpEstablishDate = DateTime.ParseExact(reader["BCP_ESTABLISH_DATE"].ToString(), "yyyy/mm/dd", culture);
                                loan.bktCorp.BcpEstablishDate = Convert.ToDateTime(reader["BCP_ESTABLISH_DATE"].ToString());
                            if (reader["BCP_INBOURSE_DATE"] != DBNull.Value && reader["BCP_INBOURSE_DATE"].ToString() != "1/1/0001")
                                //Convert.ToDateTime(
                                //loan.bktCorp.BcpInbourseDate = DateTime.ParseExact(reader["BCP_INBOURSE_DATE"].ToString(), "yyyy/mm/dd", culture);
                                loan.bktCorp.BcpInbourseDate = Convert.ToDateTime(reader["BCP_INBOURSE_DATE"].ToString());
                            if (reader["BCP_TYPE_ID"] != DBNull.Value)
                                loan.bktCorp.BCP_TYPE_ID = Convert.ToInt32(reader["BCP_TYPE_ID"]);
                            if (reader["BCP_TYPE_ID"] != DBNull.Value)
                            {
                                loan.bktCorp.BCP_TYPE_ID = Convert.ToInt32(reader["BCP_TYPE_ID"]);
                                loan.bktCorp.Type = reader["TYPE"].ToString();
                            }
                            if (reader["BCP_ACTIVITY_ID"] != DBNull.Value)
                            {
                                loan.bktCorp.BCP_ACTIVITY_ID = Convert.ToInt32(reader["BCP_ACTIVITY_ID"]);
                                loan.bktCorp.Activity = reader["ACTIVITY"].ToString();
                            }
                            if (reader["BCP_STAFF_NBR"] != DBNull.Value)
                                loan.bktCorp.BcpStaffNbr = Convert.ToInt32(reader["BCP_STAFF_NBR"]);
                            if (reader["BCP_BRANCHES_NBR"] != DBNull.Value)
                                loan.bktCorp.BcpBranchesNbr = Convert.ToInt16(reader["BCP_BRANCHES_NBR"]);
                            loan.bktCorp.BcpBranchesNbr = Convert.ToInt16(reader["BCP_BRANCHES_NBR"]);
                            loan.bktCorp.BcpCapitalKickoff = Convert.ToDouble(reader["BCP_CAPITAL_KICKOFF"]);
                            loan.bktCorp.BcpCapitalActual = Convert.ToDouble(reader["BCP_CAPITAL_ACTUAL"]);
                            loan.bktCorp.BcpInBourse = Convert.ToBoolean(reader["BCP_IN_BOURSE"]);
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
        public CorpSearchResponse FindByCustomerID(CorpSearchRequest Request)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            Cls_Loan loan = new Cls_Loan();
            try
            {
                #region QueryFind
                string queryString = "SELECT count(*) FROM dbo.V_BNK_LOANSEARCHCORPFA WHERE BPR_CODE Like '" + Request.CustomerCode + "'";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        queryString = "SELECT * FROM dbo.V_BNK_LOANSEARCHCORPFA WHERE BPR_CODE Like '" + Request.CustomerCode + "'";
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
                                    loan.bktCorp = new Cls_CorpPerson();
                                    loan.bktCorp.bktPerson = new Cls_Person();
                                    loan.bktCorp.bktPerson.BprId = Convert.ToInt64(reader["BCP_BPR_ID"]);
                                    loan.BloId = Convert.ToInt64(reader["BLO_ID"]);
                                    loan.BloCode = reader["BLO_CODE"].ToString();
                                    loan.bktCorp.BcpName = reader["BCP_NAME"].ToString(); //till here
                                    if (reader["BLO_CREATE_DATE"] != DBNull.Value && reader["BLO_CREATE_DATE"].ToString() != "1/1/0001")
                                    {
                                        loan.BloCreateDate = DateTime.ParseExact(reader["BLO_CREATE_DATE"].ToString(), "yyyy/mm/dd", culture);
                                    }
                                    if (reader["BLO_ECONOMICSECTOR_ID"] != DBNull.Value)
                                    {
                                        loan.BLO_EconomicSector = Convert.ToInt32(reader["BLO_ECONOMICSECTOR_ID"]);
                                    }
                                    if (reader["BLO_CONTRACTTYPE_ID"] != DBNull.Value)
                                    {
                                        loan.BLO_ContractType = Convert.ToInt32(reader["BLO_CONTRACTTYPE_ID"]);
                                    }
                                    if (reader["BLO_CUSTOMERGROUP_ID"] != DBNull.Value)
                                    {
                                        loan.BLO_CustomerGroup = Convert.ToInt32(reader["BLO_CUSTOMERGROUP_ID"]);
                                    }
                                    if (reader["BLO_LOANAIM"] != DBNull.Value)
                                    {
                                        loan.BLO_LoanAim = Convert.ToInt32(reader["BLO_LOANAIM"]);
                                    }
                                    if (reader["BLO_LOANSTATE"] != DBNull.Value)
                                    {
                                        loan.BLO_LoanState = Convert.ToInt32(reader["BLO_LOANSTATE"]);
                                    }
                                    if (reader["BLO_LOANTYPE_ID"] != DBNull.Value)
                                    {
                                        loan.BLO_LoanTypeId = Convert.ToInt32(reader["BLO_LOANTYPE_ID"]);
                                    }
                                    if (reader["BLO_LOANTYPE2_ID"] != DBNull.Value)
                                    {
                                        loan.BLO_LoanType2Id = Convert.ToInt32(reader["BLO_LOANTYPE2_ID"]);
                                    }
                                    if (reader["BLO_LOANREQUEST_ID"] != DBNull.Value)
                                    {
                                        loan.BLO_LoanRequest = Convert.ToInt32(reader["BLO_LOANREQUEST_ID"]);
                                    }

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
                                        loan.BloAssurance1 = Convert.ToByte(reader["BLO_ASSURANCE2"]);
                                    if (reader["BLO_ACCOUNTNO"] != DBNull.Value)
                                        loan.BloAccountNo = reader["BLO_ACCOUNTNO"].ToString();
                                    if (reader["BLO_PAYMENT"] != DBNull.Value)
                                        loan.BloPayment = Convert.ToDouble(reader["BLO_PAYMENT"]);
                                    if (reader["BLO_PAYMENT_NBR"] != DBNull.Value)
                                        loan.BloPaymentNbr = Convert.ToInt32(reader["BLO_PAYMENT_NBR"]);
                                    if (reader["BLO_PAYMENT_PERIOD"] != DBNull.Value)
                                        loan.BloPaymentPeriod = Convert.ToByte(reader["BLO_PAYMENT_PERIOD"]);
                                    if (reader["BLO_PAYMENT_DATE"] != DBNull.Value && reader["BLO_PAYMENT_DATE"].ToString() != "1/1/0001")
                                        loan.BloPaymentDate = DateTime.ParseExact(reader["BLO_PAYMENT_DATE"].ToString(), "yyyy/mm/dd", culture);
                                    if (reader["BLO_MATURITY_DATE"] != DBNull.Value && reader["BLO_MATURITY_DATE"].ToString() != "1/1/0001")
                                    {
                                        loan.BloMaturityDate = DateTime.ParseExact(reader["BLO_MATURITY_DATE"].ToString(), "yyyy/mm/dd", culture);
                                    }
                                    if (reader["BLO_BCU_ID"] != DBNull.Value)
                                        loan.BLO_BCU_ID = Convert.ToInt32(reader["BLO_BCU_ID"]);
                                    loan.bktCorp.bktPerson.BprCode = reader["BPR_CODE"].ToString();
                                    loan.bktCorp.bktPerson.BprType = 2;
                                    loan.bktCorp.BcpName = reader["BCP_NAME"].ToString();
                                    loan.bktCorp.BcpRegisterNo = reader["BCP_REGISTER_NO"].ToString();
                                    loan.bktCorp.BcpIdentificationID = Convert.ToInt64(reader["BCP_IDENTIFICATION_ID"]);
                                    loan.bktCorp.BcpRegisterPlace = reader["BCP_REGISTER_PLACE"].ToString();
                                    if (reader["BCP_ESTABLISH_DATE"] != DBNull.Value && reader["BCP_ESTABLISH_DATE"].ToString() != "1/1/0001")
                                        loan.bktCorp.BcpEstablishDate = DateTime.ParseExact(reader["BCP_ESTABLISH_DATE"].ToString(), "yyyy/mm/dd", culture);
                                    if (reader["BCP_INBOURSE_DATE"] != DBNull.Value && reader["BCP_INBOURSE_DATE"].ToString() != "1/1/0001")
                                        loan.bktCorp.BcpInbourseDate = DateTime.ParseExact(reader["BCP_INBOURSE_DATE"].ToString(), "yyyy/mm/dd", culture);
                                    if (reader["BCP_TYPE_ID"] != DBNull.Value)
                                        loan.bktCorp.BCP_TYPE_ID = Convert.ToInt32(reader["BCP_TYPE_ID"]);
                                    if (reader["BCP_TYPE_ID"] != DBNull.Value)
                                    {
                                        loan.bktCorp.BCP_TYPE_ID = Convert.ToInt32(reader["BCP_TYPE_ID"]);
                                        loan.bktCorp.Type = reader["TYPE"].ToString();
                                    }
                                    if (reader["BCP_ACTIVITY_ID"] != DBNull.Value)
                                    {
                                        loan.bktCorp.BCP_ACTIVITY_ID = Convert.ToInt32(reader["BCP_ACTIVITY_ID"]);
                                        loan.bktCorp.Activity = reader["ACTIVITY"].ToString();
                                    }
                                    if (reader["BCP_STAFF_NBR"] != DBNull.Value)
                                        loan.bktCorp.BcpStaffNbr = Convert.ToInt32(reader["BCP_STAFF_NBR"]);
                                    if (reader["BCP_BRANCHES_NBR"] != DBNull.Value)
                                        loan.bktCorp.BcpBranchesNbr = Convert.ToInt16(reader["BCP_BRANCHES_NBR"]);
                                    loan.bktCorp.BcpBranchesNbr = Convert.ToInt16(reader["BCP_BRANCHES_NBR"]);
                                    loan.bktCorp.BcpCapitalKickoff = Convert.ToDouble(reader["BCP_CAPITAL_KICKOFF"]);
                                    loan.bktCorp.BcpCapitalActual = Convert.ToDouble(reader["BCP_CAPITAL_ACTUAL"]);
                                    loan.bktCorp.BcpInBourse = Convert.ToBoolean(reader["BCP_IN_BOURSE"]);
                                    response.Loan = new Cls_Loan();
                                                                        
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
        public CorpSearchResponse FindByAddress(long BprID)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            DataSet dsAddress = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            Cls_Loan loan = new Cls_Loan();
            loan.bktCorp = new Cls_CorpPerson();
            loan.bktCorp.bktPerson = new Cls_Person();
            Cls_Address address = new Cls_Address();
            try
            {
                #region QueryFind
                string queryString = "SELECT * FROM dbo.V_BNK_ADDRESSCORPFA WHERE BCP_BPR_ID = " + BprID;
                        //+ " AND BAP_CREATE_DATE IN (SELECT MAX(BAP_CREATE_DATE) FROM dbo.BKT_ADDRESS_PERSON where BAP_BPR_ID = " + BprID + ")";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    da = new SqlDataAdapter(command);
                    da.Fill(dsAddress, "Address");
                    loan.bktCorp.bktPerson.bktAddress = new Cls_Address[dsAddress.Tables[0].Rows.Count];
                    for (int j = 0; j < dsAddress.Tables[0].Rows.Count; j++)
                    {
                        address = new Cls_Address();
                        address.Bap_TypeID = Convert.ToInt32(dsAddress.Tables[0].Rows[j]["BAP_BBD_ID"]);
                        address.BapId = Convert.ToInt32(dsAddress.Tables[0].Rows[j]["BAP_ID"]);
                        address.BapPriority = Convert.ToByte(dsAddress.Tables[0].Rows[j]["BAP_PRIORITY"]);
                        address.BapValue = dsAddress.Tables[0].Rows[j]["BAP_VALUE"].ToString();
                        loan.bktCorp.bktPerson.bktAddress[j] = address;
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
        public CorpBaseResponse GetBase()
        {
            string strSQL;
            CorpBaseResponse response = new CorpBaseResponse();
            try
            {
                #region BaseQuery
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    strSQL = "SELECT * FROM dbo.V_BAS_CORPTYPE";
                    SqlCommand command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.CorpType.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_CORPACTIVITY";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.CorpActivity.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
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
                    strSQL = "SELECT * FROM dbo.V_BAS_ANSWER";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Answer.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
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
        public CorpBaseResponse GetEditBase()
        {
            string strSQL;
            CorpBaseResponse response = new CorpBaseResponse();
            try {
                   #region QueryBaseData
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    strSQL = "SELECT * FROM dbo.V_BAS_CORPTYPE";
                    SqlCommand command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.CorpType.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_CORPACTIVITY";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.CorpActivity.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
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
                    strSQL = "SELECT * FROM dbo.V_BAS_ANSWER";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Answer.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_PERMIT";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.PermitType.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_PERMITSOURCE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.PermitSource.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_ACCOUNT";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Account.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
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
                    strSQL = "SELECT * FROM dbo.V_BAS_LOANTYPE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.LoanType.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_LOANGUARANTY";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Guaranty.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_EDUDEGREE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.EduDegree.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                            response.ShareEduDegree.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT BCU_ID,BCU_TITLE FROM dbo.BKT_CURRENCY ORDER BY BCU_SEQ";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Currency.Add(Convert.ToInt32(reader["BCU_ID"]), reader["BCU_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT BBN_ID,BBN_TITLE FROM dbo.BKT_BANKS";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Bank.Add(Convert.ToInt32(reader["BBN_ID"]), reader["BBN_TITLE"].ToString());
                            response.Bank2.Add(Convert.ToInt32(reader["BBN_ID"]), reader["BBN_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_POSITION";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Position.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_AIMLOAN";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.AimLoan.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_COLLATERAL";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.Collateral.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_ECONOMICSECTEUR";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.EconomicSector.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_CONTRACTTYPE";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.ContractType.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
                        }
                    }
                    strSQL = "SELECT * FROM dbo.V_BAS_CUSTOMERGROUP";
                    command = new SqlCommand(strSQL, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            response.CustomerGroup.Add(Convert.ToInt32(reader["BBD_ID"]), reader["BBD_TITLE"].ToString());
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
        public CorpSearchResponse GetPermits(CorpSearchRequest Request)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string queryString = "SELECT * FROM [AlpsPdb].[DBO].[V_BNK_PERMITFA] WHERE [BPE_BLO_ID] = " + Request.BloID.ToString() ;

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
                    connection.Close();
                }
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public CorpSearchResponse InsertPermit(CorpSearchRequest request)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string insertSql = "INSERT INTO [AlpsPdb].[dbo].BKT_PERMIT " +
                                      "([BPE_BLO_ID],[BPE_TYPE_ID],[BPE_PERMIT_NO],[BPE_ISSUE_DATE],[BPE_EXPIRE_DATE]," +
                                      "[BPE_ISSUEPLACE_ID],[BPE_VALID_MONTH],[BPE_CREATE_DATE],[BPE_STATUS]) " +
                                      "VALUES (@BloID, @TypeID, @PermitNo, @IssueDate, @ExpireDate , @IssuePlace, @ValidMonth, @CreateDT, @Status)";
                string updateSql = "UPDATE [AlpsPdb].[dbo].[BKT_LOANS] " +
                                   "SET [BLO_FLGPERMIT] = 'True',[BLO_CREATE_DATE] = @CreateDT, [BLO_STATUS] = 2 " +
                                   "WHERE BLO_ID = @BloID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    SqlParameter TypeID = new SqlParameter("@TypeID", request.PermitTypeId);
                    SqlParameter BloID = new SqlParameter("@BloID", request.BloID);
                    SqlParameter PermitNo = new SqlParameter("@PermitNo", request.PermitNo);
                    SqlParameter IssueDate = new SqlParameter("@IssueDate", Convert.ToDateTime(request.IssueDate));
                    SqlParameter ExpireDate = new SqlParameter("@ExpireDate", Convert.ToDateTime(request.ExpireDate));
                    SqlParameter IssuePlace = new SqlParameter("@IssuePlace", request.IssuePlaceId);
                    SqlParameter ValidMonth = new SqlParameter("@ValidMonth", request.ValidMonth);
                    SqlParameter CreateDT = new SqlParameter("@CreateDT", System.DateTime.Now);
                    SqlParameter Status = new SqlParameter("@Status", 1);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(BloID);
                    command.Parameters.Add(PermitNo);
                    command.Parameters.Add(IssueDate);
                    command.Parameters.Add(ExpireDate);
                    command.Parameters.Add(IssuePlace);
                    command.Parameters.Add(ValidMonth);
                    command.Parameters.Add(CreateDT);
                    command.Parameters.Add(Status);
                    connection.Open();
                    command.ExecuteNonQuery();
                    command.CommandText = updateSql;
                    //command.Parameters.Add(CreateDT);
                    //command.Parameters.Add(BloID);
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
        public CorpSearchResponse DeletePermit(long BpeID)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string deletetSql = "Delete [AlpsPdb].[dbo].BKT_PERMIT Where BPE_ID = @BpeID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deletetSql;
                    SqlParameter Permit = new SqlParameter("@BpeID", BpeID);
                    command.Parameters.Add(Permit);
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
        public CorpSearchResponse UpdatePermit(CorpSearchRequest request)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            Global g = new Global();
            try
            {
                string updateSql = "UPDATE [AlpsPdb].[dbo].BKT_PERMIT " +
                                       "SET [BPE_TYPE_ID] = @TypeID," +
                                       "[BPE_ISSUEPLACE_ID] = @IssueID," +
                                       "[BPE_PERMIT_NO] = @PermitNo," +
                                       "[BPE_ISSUE_DATE] = @IssueDate," +
                                       "[BPE_EXPIRE_DATE] = @ExpireDate," +
                                       "[BPE_VALID_MONTH] = @ValidMonth," +
                                       "[BPE_CREATE_DATE] = @CreateDate," +
                                       "[BPE_STATUS] = @Status " +
                                       "WHERE BPE_ID = @BpeID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    SqlParameter TypeID = new SqlParameter("@TypeID", request.PermitTypeId);
                    SqlParameter IssueID = new SqlParameter("@IssueID", request.IssuePlaceId);
                    SqlParameter PermitNo = new SqlParameter("@PermitNo", request.PermitNo);
                    SqlParameter IssueDate = new SqlParameter("@IssueDate", g.CheckDate(request.IssueDate));
                    SqlParameter ExpireDate = new SqlParameter("@ExpireDate", g.CheckDate(request.ExpireDate));
                    SqlParameter ValidMonth = new SqlParameter("@ValidMonth", request.ValidMonth);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    SqlParameter BpeID = new SqlParameter("@BpeID", request.BpeID);
                    SqlParameter Status = new SqlParameter("@Status", 2);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(IssueID);
                    command.Parameters.Add(PermitNo);
                    command.Parameters.Add(IssueDate);
                    command.Parameters.Add(ExpireDate);
                    command.Parameters.Add(ValidMonth);
                    command.Parameters.Add(CreateDate);
                    command.Parameters.Add(BpeID);
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
        public CorpSearchResponse GetBoard(CorpSearchRequest Request)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string queryString = "SELECT * FROM [AlpsPdb].[DBO].[V_BNK_BOARDFA] WHERE [CorpId] = " + Request.BprID.ToString(); 

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
                    connection.Close();
                }
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public CorpSearchResponse DeleteBoard(long BboID)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string deletetSql = "Delete [AlpsPdb].[dbo].BKT_BOARD Where BBO_ID = @BboID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deletetSql;
                    SqlParameter Board = new SqlParameter("@BboID", BboID);
                    command.Parameters.Add(Board);
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
        public CorpSearchResponse UpdateBoard(CorpSearchRequest request)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string updateSql = "UPDATE [AlpsPdb].[dbo].[BKT_REAL_PERSON] " +
                                   "SET [BRP_FIRST_NAME] = @Fname, [BRP_LAST_NAME] = @Lname," +
                                   "[BRP_BIRTH_DATE] = @BirthDate, [BRP_FATHER_NAME] = @Father," +
                                   "[BRP_BIRTH_CERTID] = @Certid, [BRP_IDENTIFICATION_ID] = @NationalId + ," +
                                   "[BRP_EDU_DEGREE_ID] = @EDU_DEGREE_ID , [BRP_ISSUE_PLACE] = @IssuePlace '" +
                                   " WHERE [BRP_BPR_ID] = " + request.BprID.ToString();

                string updateSql2 = "UPDATE [AlpsPdb].[dbo].[BKT_BOARD] " +
                                    "SET [BBO_POSITION_ID] = @PositionID ,[BLO_CREATE_DATE] = @Createdt ,[BLO_STATUS] = 2 " +
                                    "WHERE BBO_ID = " + request.BboID.ToString();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    SqlParameter Fname = new SqlParameter("@Fname", request.FirstName);
                    SqlParameter Lname = new SqlParameter("@Lname", request.LastName);
                    SqlParameter BirthDate = new SqlParameter("@BirthDate", request.BirthDate);
                    SqlParameter Father = new SqlParameter("@Father", request.FatherName);
                    SqlParameter Certid = new SqlParameter("@Certid", request.CertificationId);
                    SqlParameter NationalId = new SqlParameter("@NationalId", request.NationalId);
                    SqlParameter EDU_DEGREE_ID = new SqlParameter("@EDU_DEGREE_ID", request.EducationId);
                    SqlParameter IssuePlace = new SqlParameter("@IssuePlace", request.IssuePlace);
                    command.Parameters.Add(Fname);
                    command.Parameters.Add(Lname);
                    command.Parameters.Add(BirthDate);
                    command.Parameters.Add(Father);
                    command.Parameters.Add(Certid);
                    command.Parameters.Add(NationalId);
                    command.Parameters.Add(EDU_DEGREE_ID);
                    command.Parameters.Add(IssuePlace);
                    connection.Open();
                    command.ExecuteNonQuery();
                    command.CommandText = updateSql2;
                    SqlParameter PositionID = new SqlParameter("@PositionID", request.PositionId);
                    SqlParameter CreateDate = new SqlParameter("@CreateDate", System.DateTime.Now);
                    command.Parameters.Add(PositionID);
                    command.Parameters.Add(CreateDate);
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
        public CorpSearchResponse InsertBoard(CorpSearchRequest request)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            request.BrpTypeID = 227; //ID for board member
            SqlConnection CnnSql = new SqlConnection(_connectionString);
            SqlTransaction TrSql;
            if (CnnSql.State == ConnectionState.Closed)
                CnnSql.Open();
            TrSql = CnnSql.BeginTransaction();
            SqlCommand cmd = new SqlCommand("", CnnSql, TrSql);
            cmd.Connection = CnnSql;
            try
            {
                cmd.CommandText = "SELECT BPR_ID FROM dbo.BKT_PERSON WHERE BPR_CODE LIKE '" + request.BprCode + "'";
                if (Convert.ToInt64(cmd.ExecuteScalar()) > 0)
                {
                    request.BprID = Convert.ToInt64(cmd.ExecuteScalar());
                    cmd.CommandText = "UPDATE [AlpsPdb].[dbo].[BKT_REAL_PERSON] " +
                                      "SET [BRP_FIRST_NAME] = N'" + request.FirstName + "'," + "[BRP_LAST_NAME] = N'" + request.LastName + "'," +
                                      "[BRP_BIRTH_DATE] = '" + request.BirthDate + "'," + "[BRP_FATHER_NAME] = N'" + request.FatherName + "'," +
                                      "[BRP_BIRTH_CERTID] = " + request.CertificationId + "," + "[BRP_IDENTIFICATION_ID] = " + request.NationalId + "," +
                                      "[BRP_EDU_DEGREE_ID] = " + request.EducationId.ToString() + "," + "[BRP_ISSUE_PLACE] = N'" + request.IssuePlace + "'" +
                                      " WHERE [BRP_BPR_ID] = " + request.BprID.ToString();
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = "INSERT INTO dbo.BKT_PERSON " +
                       "(BPR_CODE, BPR_TYPE, BPR_STATUS, BPR_DATE) " +
                       "VALUES ('" + request.BprCode + "',1,1,'" + System.DateTime.Now.ToString() + "')";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "SELECT BPR_ID FROM dbo.BKT_PERSON WHERE BPR_CODE LIKE '" + request.BprCode + "'";
                    request.BprID = Convert.ToInt64(cmd.ExecuteScalar());

                    cmd.CommandText = "INSERT INTO [AlpsPdb].[dbo].BKT_REAL_PERSON " +
                                      "([BRP_BPR_ID],[BRP_FIRST_NAME],[BRP_LAST_NAME],[BRP_FATHER_NAME],[BRP_BIRTH_DATE]," +
                                      "[BRP_BIRTH_CERTID],[BRP_IDENTIFICATION_ID],[BRP_EDU_DEGREE_ID],[BRP_TYPE_ID]," +
                                      "[BRP_ISSUE_PLACE]) " +
                                      "VALUES (" + request.BprID + ",N'" + request.FirstName + "',N'" + request.LastName + "',N'" +
                                      request.FatherName + "','" + request.BirthDate + "'," + request.CertificationId + "," +
                                      request.NationalId + "," + request.EducationId.ToString() + "," + request.BrpTypeID.ToString() + ",N'" +
                                      request.IssuePlace + "')";
                    cmd.ExecuteNonQuery();
                }
                cmd.CommandText = "INSERT INTO [AlpsPdb].[dbo].BKT_BOARD " +
                    "([BBO_BPR_ID],[BBO_PERSON_ID],[BBO_POSITION_ID])" +
                    "VALUES (" + request.BprID.ToString() + "," + request.CorpID.ToString() + "," + request.PositionId.ToString() + ")";
                cmd.ExecuteNonQuery();
      
                cmd.CommandText = "UPDATE [AlpsPdb].[dbo].[BKT_LOANS] " +
                                  "SET [BLO_FLGBOARD] = 'True',[BLO_CREATE_DATE] = " + System.DateTime.Now.ToString() +
                                  ",[BLO_STATUS] = 2 " +
                                  "WHERE BLO_ID = " + request.BloID.ToString();
            cmd.ExecuteNonQuery();
            TrSql.Commit();
            response.Success = true;
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
                response.Success = false;
            }
            return response;
        }
        public CorpSearchResponse GetShare(CorpSearchRequest Request)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string queryString = "SELECT * FROM [AlpsPdb].[DBO].[V_BNK_SHAREHOLDERFA] WHERE [CorpId] = " + Request.BprID.ToString();

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
                    connection.Close();
                }
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public CorpSearchResponse DeleteShare(long BshID)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string deletetSql = "Delete [AlpsPdb].[dbo].BKT_SHAREHOLDER Where BSH_ID = @BshID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = deletetSql;
                    SqlParameter Share = new SqlParameter("@BshID", BshID);
                    command.Parameters.Add(Share);
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
        public CorpSearchResponse UpdateLoan(Cls_Loan loan)
        {
            CorpSearchResponse response = new CorpSearchResponse();
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
        public CorpSearchResponse UpdateCorp(Cls_CorpPerson Corporate)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string updateSql = "UPDATE [AlpsPdb].[dbo].[BKT_CORP_PERSON] " +
                    "SET [BCP_TYPE_ID] = @TypeId , [BCP_ACTIVITY_ID] = @ActivityId ," +
                    "[BCP_NAME] = @Name, [BCP_REGISTER_NO] = @RegisterNo ," +
                    "[BCP_REGISTER_PLACE] = @RegisterPlace , [BCP_ESTABLISH_DATE] = @EstablishDate ," +
                    "[BCP_CAPITAL_KICKOFF] = @CapitalKickoff , [BCP_CAPITAL_ACTUAL] = @CapitalActual ," +
                    "[BCP_IN_BOURSE] = @InBourse , [BCP_INBOURSE_DATE] = @InBourseDate ," +
                    "[BCP_BRANCHES_NBR] = @BranchesNbr , [BCP_STAFF_NBR] = @StaffNbr ," +
                    "[BCP_STATUS] = @Status , [BCP_STAT_DATE] = @Date " +
                    "WHERE [BCP_BPR_ID] = " + Corporate.bktPerson.BprId.ToString();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    SqlParameter TypeId = new SqlParameter("@TypeId", Corporate.BCP_TYPE_ID);
                    SqlParameter ActivityId = new SqlParameter("@ActivityId", Corporate.BCP_ACTIVITY_ID);
                    SqlParameter Name = new SqlParameter("@Name", Corporate.BcpName);
                    SqlParameter RegisterNo = new SqlParameter("@RegisterNo", Corporate.BcpRegisterNo);
                    SqlParameter RegisterPlace = new SqlParameter("@RegisterPlace", Corporate.BcpRegisterPlace);
                    SqlParameter EstablishDate = new SqlParameter("@EstablishDate", Corporate.BcpEstablishDate);
                    SqlParameter CapitalKickoff = new SqlParameter("@CapitalKickoff", Corporate.BcpCapitalKickoff);
                    SqlParameter CapitalActual = new SqlParameter("@CapitalActual", Corporate.BcpCapitalActual);
                    SqlParameter InBourse = new SqlParameter("@InBourse", Corporate.BcpInBourse);
                    SqlParameter InBourseDate = new SqlParameter("@InBourseDate", Corporate.BcpInbourseDate);
                    SqlParameter BranchesNbr = new SqlParameter("@BranchesNbr", Corporate.BcpBranchesNbr);
                    SqlParameter StaffNbr = new SqlParameter("@StaffNbr", Corporate.BcpStaffNbr);
                    SqlParameter Status = new SqlParameter("@Status", 2);
                    SqlParameter Date = new SqlParameter("@Date", DateTime.Now);
                    command.Parameters.Add(TypeId);
                    command.Parameters.Add(ActivityId);
                    command.Parameters.Add(Name);
                    command.Parameters.Add(RegisterNo);
                    command.Parameters.Add(RegisterPlace);
                    command.Parameters.Add(EstablishDate);
                    command.Parameters.Add(CapitalKickoff);
                    command.Parameters.Add(CapitalActual);
                    command.Parameters.Add(InBourse);
                    command.Parameters.Add(InBourseDate);
                    command.Parameters.Add(BranchesNbr);
                    command.Parameters.Add(StaffNbr);
                    command.Parameters.Add(Status);
                    command.Parameters.Add(Date);
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
        public CorpSearchResponse InsertOffice(Cls_Loan loan)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string insertSql = "INSERT INTO [AlpsPdb].[dbo].BKT_CENTRAL_OFFICE " +
                                  "([BCO_BLO_ID],[BCO_SANADSTATUS_ID],[BCO_PROPSTATUS_ID],[BCO_TYPE_ID],[BCO_VALUE]," +
                                  "[BCO_SURFACE],[BCO_ARENA],[BCO_STDPROPERTY],[BCO_PERSONAL],[BCO_BBN_ID]," +
                                  "[BCO_RAHN_DURATION],[BCO_RAHN_AMOUNT],[BCO_RAHN_DATE],[BCO_RENT]) " +
                                  "VALUES (@BloID, @SanadID, @PropID, @TypeID, @Value, @Surface, " +
                                  "@Arena, @StdProperty, @Personal, @BbnID, @RahanDuration, @RahnAmount, " +
                                  "@RahnDate, @Rent)";
                string updateSql = "UPDATE [AlpsPdb].[dbo].BKT_CENTRAL_OFFICE " +
                                  "SET [BCO_SANADSTATUS_ID] = @SanadID ," +
                                  "[BCO_PROPSTATUS_ID] = @PropID ," +
                                  "[BCO_VALUE] = @Value ," +
                                  "[BCO_SURFACE] = @Surface ," +
                                  "[BCO_ARENA] = @Arena ," +
                                  "[BCO_STDPROPERTY] = @StdProperty ," +
                                  "[BCO_PERSONAL] = @Personal ," +
                                  "[BCO_BBN_ID] = @BbnID ," +
                                  "[BCO_RAHN_DURATION] = @RahanDuration ," +
                                  "[BCO_RAHN_AMOUNT] = @RahnAmount ," +
                                  "[BCO_RAHN_DATE] = @RahnDate ," +
                                  "[BCO_RENT] = @Rent WHERE [BCO_ID] = @BcoID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    if (loan.bktCorp.bktFactory.BcoId > 0)
                    {
                        command.CommandText = updateSql;
                        loan.bktCorp.bktPerson.BprStatus = 2;
                    }
                    else
                    {
                        command.CommandText = insertSql;
                        loan.bktCorp.bktPerson.BprStatus = 1;
                    }

                    SqlParameter BloID = new SqlParameter("@BloID", loan.BloId);
                    SqlParameter SanadID = new SqlParameter("@SanadID", loan.bktCorp.bktCentralOffice.BCO_SANADSTATUS_ID);
                    SqlParameter PropID = new SqlParameter("@PropID", loan.bktCorp.bktCentralOffice.BCO_PROPSTATUS_ID);
                    SqlParameter TypeID = new SqlParameter("@TypeID", loan.bktCorp.bktCentralOffice.BCO_TYPE_ID);
                    SqlParameter Value = new SqlParameter("@Value", loan.bktCorp.bktCentralOffice.BcoValue);
                    SqlParameter Surface = new SqlParameter("@Surface", loan.bktCorp.bktCentralOffice.BcoSurface);
                    SqlParameter Arena = new SqlParameter("@Arena", loan.bktCorp.bktCentralOffice.BcoArena);
                    SqlParameter StdProperty = new SqlParameter("@StdProperty", loan.bktCorp.bktCentralOffice.BcoStdproperty);
                    SqlParameter Personal = new SqlParameter("@Personal", loan.bktCorp.bktCentralOffice.BcoPersonal);
                    SqlParameter BbnID = new SqlParameter("@BbnID", loan.bktCorp.bktCentralOffice.BCO_BBN_ID);
                    SqlParameter RahanDuration = new SqlParameter("@RahanDuration", loan.bktCorp.bktCentralOffice.BcoRahnDuration);
                    SqlParameter RahnAmount = new SqlParameter("@RahnAmount", loan.bktCorp.bktCentralOffice.BcoRahnAmount);
                    SqlParameter RahnDate = new SqlParameter("@RahnDate", loan.bktCorp.bktCentralOffice.BcoRahnDate);
                    SqlParameter Rent = new SqlParameter("@Rent", loan.bktCorp.bktCentralOffice.BcoRent);
                    SqlParameter BcoID = new SqlParameter("@BcoID", loan.bktCorp.bktCentralOffice.BcoId);
                    command.Parameters.Add(BloID);
                    command.Parameters.Add(SanadID);
                    command.Parameters.Add(PropID);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(Value);
                    command.Parameters.Add(Surface);
                    command.Parameters.Add(Arena);
                    command.Parameters.Add(StdProperty);
                    command.Parameters.Add(Personal);
                    command.Parameters.Add(BbnID);
                    command.Parameters.Add(RahanDuration);
                    command.Parameters.Add(RahnAmount);
                    command.Parameters.Add(RahnDate);
                    command.Parameters.Add(Rent);
                    command.Parameters.Add(BcoID);
                    connection.Open();
                    command.ExecuteNonQuery();
                    if (loan.bktCorp.bktPerson.BprStatus == 1)
                    {
                        command.CommandText = "SELECT BCO_ID FROM dbo.BKT_CENTRAL_OFFICE WHERE BCO_BLO_ID @BloId AND [BCO_TYPE_ID] = 231";
                        response.Id = Convert.ToInt64(command.ExecuteScalar());
                        loan.bktCorp.bktFactory.BcoId = response.Id;
                    }
                    response.Loan = loan;
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
        public CorpSearchResponse InsertFactory(Cls_Loan loan)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string insertSql = "INSERT INTO [AlpsPdb].[dbo].BKT_CENTRAL_OFFICE " +
                                  "([BCO_BLO_ID],[BCO_SANADSTATUS_ID],[BCO_PROPSTATUS_ID],[BCO_TYPE_ID],[BCO_VALUE]," +
                                  "[BCO_SURFACE],[BCO_ARENA],[BCO_STDPROPERTY],[BCO_PERSONAL],[BCO_BBN_ID]," +
                                  "[BCO_RAHN_DURATION],[BCO_RAHN_AMOUNT],[BCO_RAHN_DATE],[BCO_RENT],[BCO_MACHINERY]) " +
                                    "VALUES (@BloID, @SanadID, @PropID, @TypeID, @Value, @Surface, " +
                                    "@Arena, @StdProperty, @Personal, @BbnID, @RahanDuration, @RahnAmount, " +
                                    "@RahnDate, @Rent, @Machinery)";
                string updateSql = "UPDATE [AlpsPdb].[dbo].BKT_CENTRAL_OFFICE " +
                                  "SET [BCO_SANADSTATUS_ID] = @SanadID ," +
                                  "[BCO_PROPSTATUS_ID] = @PropID ," +
                                  "[BCO_VALUE] = @Value ," +
                                  "[BCO_SURFACE] = @Surface ," +
                                  "[BCO_ARENA] = @Arena ," +
                                  "[BCO_STDPROPERTY] = @StdProperty ," +
                                  "[BCO_PERSONAL] = @Personal ," +
                                  "[BCO_BBN_ID] = @BbnID ," +
                                  "[BCO_RAHN_DURATION] = @RahanDuration ," +
                                  "[BCO_RAHN_AMOUNT] = @RahnAmount ," +
                                  "[BCO_RAHN_DATE] = @RahnDate ," +
                                  "[BCO_RENT] = @Rent WHERE [BCO_ID] = @BcoID";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    if (loan.bktCorp.bktFactory.BcoId > 0)
                    {
                        command.CommandText = updateSql;
                        loan.bktCorp.bktPerson.BprStatus = 2;
                    }
                    else
                    {
                        command.CommandText = insertSql;
                        loan.bktCorp.bktPerson.BprStatus = 1;
                    }
                    SqlParameter BloID = new SqlParameter("@BloID", loan.BloId);
                    SqlParameter SanadID = new SqlParameter("@SanadID", loan.bktCorp.bktFactory.BCO_SANADSTATUS_ID);
                    SqlParameter PropID = new SqlParameter("@PropID", loan.bktCorp.bktFactory.BCO_PROPSTATUS_ID);
                    SqlParameter TypeID = new SqlParameter("@TypeID", loan.bktCorp.bktFactory.BCO_TYPE_ID);
                    SqlParameter Value = new SqlParameter("@Value", loan.bktCorp.bktFactory.BcoValue);
                    SqlParameter Surface = new SqlParameter("@Surface", loan.bktCorp.bktFactory.BcoSurface);
                    SqlParameter Arena = new SqlParameter("@Arena", loan.bktCorp.bktFactory.BcoArena);
                    SqlParameter StdProperty = new SqlParameter("@StdProperty", loan.bktCorp.bktFactory.BcoStdproperty);
                    SqlParameter Personal = new SqlParameter("@Personal", loan.bktCorp.bktFactory.BcoPersonal);
                    SqlParameter BbnID = new SqlParameter("@BbnID", loan.bktCorp.bktFactory.BCO_BBN_ID);
                    SqlParameter RahanDuration = new SqlParameter("@RahanDuration", loan.bktCorp.bktFactory.BcoRahnDuration);
                    SqlParameter RahnAmount = new SqlParameter("@RahnAmount", loan.bktCorp.bktFactory.BcoRahnAmount);
                    SqlParameter RahnDate = new SqlParameter("@RahnDate", loan.bktCorp.bktFactory.BcoRahnDate);
                    SqlParameter Rent = new SqlParameter("@Rent", loan.bktCorp.bktFactory.BcoRent);
                    SqlParameter BcoID = new SqlParameter("@BcoID", loan.bktCorp.bktFactory.BcoId);
                    SqlParameter Machinery = new SqlParameter("@Machinery", loan.bktCorp.bktFactory.BcoMachinery);
                    command.Parameters.Add(BloID);
                    command.Parameters.Add(SanadID);
                    command.Parameters.Add(PropID);
                    command.Parameters.Add(TypeID);
                    command.Parameters.Add(Value);
                    command.Parameters.Add(Surface);
                    command.Parameters.Add(Arena);
                    command.Parameters.Add(StdProperty);
                    command.Parameters.Add(Personal);
                    command.Parameters.Add(BbnID);
                    command.Parameters.Add(RahanDuration);
                    command.Parameters.Add(RahnAmount);
                    command.Parameters.Add(RahnDate);
                    command.Parameters.Add(Rent);
                    command.Parameters.Add(BcoID);
                    command.Parameters.Add(Machinery);
                    connection.Open();
                    command.ExecuteNonQuery();
                    if (loan.bktCorp.bktPerson.BprStatus == 1)
                    {
                        command.CommandText = "SELECT BCO_ID FROM dbo.BKT_CENTRAL_OFFICE WHERE BCO_BLO_ID @BloId AND [BCO_TYPE_ID] = 232";
                        response.Id = Convert.ToInt64(command.ExecuteScalar());
                        loan.bktCorp.bktFactory.BcoId = response.Id;
                    }
                    response.Loan = loan;
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
        public CorpSearchResponse GetOffice(long BloId)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string queryString = "SELECT * FROM [AlpsPdb].[DBO].[V_BNK_OFFICEFA] WHERE [BCO_BLO_ID] = " + BloId.ToString() + " AND [BCO_TYPE_ID] = 231";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    Cls_Loan loan = new Cls_Loan();
                    loan.bktCorp = new Cls_CorpPerson();
                    loan.bktCorp.bktCentralOffice = new Cls_CentralOffice();
                    SqlDataReader sdr;
                    sdr = command.ExecuteReader(CommandBehavior.Default);
                    while (sdr.Read())
                    {
                        loan.bktCorp.bktCentralOffice.BcoId = Convert.ToInt64(sdr["BCO_ID"]);
                        loan.bktCorp.bktCentralOffice.BCO_SANADSTATUS_ID = Convert.ToInt32(sdr["BCO_SANADSTATUS_ID"]);
                        loan.bktCorp.bktCentralOffice.BCO_PROPSTATUS_ID = Convert.ToInt32(sdr["BCO_PROPSTATUS_ID"]);
                        loan.bktCorp.bktCentralOffice.BcoValue = Convert.ToDouble(sdr["BCO_VALUE"]);
                        loan.bktCorp.bktCentralOffice.BcoSurface = Convert.ToDecimal(sdr["BCO_SURFACE"]);
                        loan.bktCorp.bktCentralOffice.BcoStdproperty = Convert.ToDecimal(sdr["BCO_STDPROPERTY"]);
                        loan.bktCorp.bktCentralOffice.BcoRent = Convert.ToDouble(sdr["BCO_RENT"]);
                        loan.bktCorp.bktCentralOffice.BcoRahnDuration = Convert.ToInt32(sdr["BCO_RAHN_DURATION"]);
                        if (sdr["BCO_RAHN_DATE"] != DBNull.Value && sdr["BCP_RAHN_DATE"].ToString() != "1/1/0001")
                            loan.bktCorp.bktCentralOffice.BcoRahnDate = DateTime.ParseExact(sdr["BCO_RAHN_DATE"].ToString(), "yyyy/mm/dd", culture);
                        //loan.bktCorp.bktCentralOffice.BcoRahnDate = Convert.ToDateTime(sdr["BCO_RAHN_DATE"]);
                        loan.bktCorp.bktCentralOffice.BcoRahnAmount = Convert.ToDouble(sdr["BCO_RAHN_AMOUNT"]);
                        loan.bktCorp.bktCentralOffice.BcoPersonal = Convert.ToInt32(sdr["BCO_PERSONAL"]);
                        loan.bktCorp.bktCentralOffice.BcoArena = Convert.ToDecimal(sdr["BCO_ARENA"]);
                        loan.bktCorp.bktCentralOffice.BCO_BBN_ID = Convert.ToInt64(sdr["BCO_BBN_ID"]);
                    }
                    sdr.Close();
                    connection.Close();
                    response.Loan = loan;
                    response.Success = true;
                }
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
        public CorpSearchResponse GetFactory(long BloId)
        {
            CorpSearchResponse response = new CorpSearchResponse();
            try
            {
                string queryString = "SELECT * FROM [AlpsPdb].[DBO].[V_BNK_OFFICEFA] WHERE [BCO_BLO_ID] = " + BloId.ToString() + " AND [BCO_TYPE_ID] = 232"; ;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    Cls_Loan loan = new Cls_Loan();
                    loan.bktCorp = new Cls_CorpPerson();
                    loan.bktCorp.bktFactory = new Cls_CentralOffice();
                    SqlDataReader sdr;
                    sdr = command.ExecuteReader(CommandBehavior.Default);
                    while (sdr.Read())
                    {
                        loan.bktCorp.bktFactory.BcoId = Convert.ToInt64(sdr["BCO_ID"]);
                        loan.bktCorp.bktFactory.BCO_SANADSTATUS_ID = Convert.ToInt32(sdr["BCO_SANADSTATUS_ID"]);
                        loan.bktCorp.bktFactory.BCO_PROPSTATUS_ID = Convert.ToInt32(sdr["BCO_PROPSTATUS_ID"]);
                        if (sdr["BCO_MACHINERY"] != DBNull.Value)
                            loan.bktCorp.bktFactory.BcoMachinery = Convert.ToDouble(sdr["BCO_MACHINERY"]);
                        loan.bktCorp.bktFactory.BcoValue = Convert.ToDouble(sdr["BCO_VALUE"]);
                        loan.bktCorp.bktFactory.BcoSurface = Convert.ToDecimal(sdr["BCO_SURFACE"]);
                        loan.bktCorp.bktFactory.BcoStdproperty = Convert.ToDecimal(sdr["BCO_STDPROPERTY"]);
                        loan.bktCorp.bktFactory.BcoRent = Convert.ToDouble(sdr["BCO_RENT"]);
                        loan.bktCorp.bktFactory.BcoRahnDuration = Convert.ToInt32(sdr["BCO_RAHN_DURATION"]);
                        //loan.bktCorp.bktFactory.BcoRahnDate = Convert.ToDateTime(sdr["BCO_RAHN_DATE"]);
                        if (sdr["BCO_RAHN_DATE"] != DBNull.Value && sdr["BCP_RAHN_DATE"].ToString() != "1/1/0001")
                            loan.bktCorp.bktCentralOffice.BcoRahnDate = DateTime.ParseExact(sdr["BCO_RAHN_DATE"].ToString(), "yyyy/mm/dd", culture);
                        loan.bktCorp.bktFactory.BcoRahnAmount = Convert.ToDouble(sdr["BCO_RAHN_AMOUNT"]);
                        loan.bktCorp.bktFactory.BcoPersonal = Convert.ToInt32(sdr["BCO_PERSONAL"]);
                        loan.bktCorp.bktFactory.BcoArena = Convert.ToDecimal(sdr["BCO_ARENA"]);
                        loan.bktCorp.bktFactory.BCO_BBN_ID = Convert.ToInt64(sdr["BCO_BBN_ID"]);
                        response.Loan = loan;
                    }
                    sdr.Close();
                    connection.Close();
                    response.Success = true;
                }
            }
            catch (SqlException exp)
            {
                response.Message = exp.ToString();
            }
            return response;
        }
    }
 }