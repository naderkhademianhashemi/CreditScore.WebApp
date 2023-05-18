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
    public class sqlFileAVRepository : IFileAvRepository
    {
        private string _connectionString;
        public sqlFileAVRepository()
        {
             _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        public FileCreateResponse Add(FileAV fileAV)
        {
            FileCreateResponse fileCreateResponse = new FileCreateResponse();
            try
            {
                
                string insertSql = "INSERT INTO T_File " +
                                    "(ID, Path, MD5, SHA1, SHA256, Count, Queue) VALUES " +
                                    "(@FileID, @Path, @MD5, @SHA1, @SHA256, @COUNT, @Queue)";
                string insertSql2 = "Insert INTO T_User_File " +
                                    "(ID,ActionDate,ActionType,User_ID,File_ID,IP,Name) VALUES " +
                                    "(@UserFileID,@ActionDate,@ActionType,@UserID,@FileID,@IP, @Name) ";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = insertSql;
                    SetCommandParametersForInsertTo(fileAV, command);
                    connection.Open();
                    command.ExecuteNonQuery();
                    command.CommandText = insertSql2;
                    command.ExecuteNonQuery();
                }
                fileCreateResponse.Success = true;
               
            }
            catch (SqlException exe)
            {
                fileCreateResponse.Message = exe.Message;
            }          
             return fileCreateResponse;          
        }
        public AssessResponse ReAssessFile(AssessRequest request)
        {
            AssessResponse response = new AssessResponse();
            try
            {
                string updateSql = "UPDATE T_File SET Queue = 'True' " + 
                                   "WHERE ID = '" + request.FileId.ToString() + "'";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = updateSql;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                response.Success = true;
            }
            catch (SqlException exe)
            {
                response.Message = exe.Message;
            }          
            return response;
        }
        private static void SetCommandParametersForInsertTo(FileAV fileAV, SqlCommand command)
        {
            fileAV.UserFileID = Guid.NewGuid();
            fileAV.ActionType = 1;
            command.Parameters.Add(new SqlParameter("@FileID", fileAV.FileID));
            command.Parameters.Add(new SqlParameter("@Name", fileAV.Name));
            command.Parameters.Add(new SqlParameter("@Path", fileAV.Path));
            command.Parameters.Add(new SqlParameter("@ActionDate", fileAV.DateAction));
            command.Parameters.Add(new SqlParameter("@MD5", fileAV.sMD5));
            command.Parameters.Add(new SqlParameter("@SHA1", fileAV.sSHA1));
            command.Parameters.Add(new SqlParameter("@SHA256", fileAV.sSHA256));
            command.Parameters.Add(new SqlParameter("@Count", fileAV.Count));
            command.Parameters.Add(new SqlParameter("@Queue", fileAV.Queue));
            command.Parameters.Add(new SqlParameter("@UserFileID", fileAV.UserFileID));
            command.Parameters.Add(new SqlParameter("@UserID", fileAV.UserID));
            command.Parameters.Add(new SqlParameter("@ActionType", fileAV.ActionType));
            command.Parameters.Add(new SqlParameter("@IP", fileAV.IP));
        }


        public FileAV FindBy(FileAV fileAV)
        {
            FileAV fileAv;
            string queryString = "SELECT * FROM dbo.T_File WHERE dbo.T_File.MD5 = @MD5 OR dbo.T_File.SHA1 = @SHA1 OR dbo.T_File.SHA256 = @SHA256 ;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                SqlParameter MD5param = new SqlParameter("@MD5", fileAV.sMD5);
                SqlParameter SHA1param = new SqlParameter("@SHA1", fileAV.sSHA1);
                SqlParameter SHA256param = new SqlParameter("@SHA256", fileAV.sSHA256);
                command.Parameters.Add(MD5param);
                command.Parameters.Add(SHA1param);
                command.Parameters.Add(SHA256param);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        fileAv = new FileAV(new Guid((reader)[0].ToString()), (reader)[2].ToString());
                    }
                    else
                        fileAv = null;
                }
            }
            return fileAv;
        }
        public ChkFileResponse FindAll(ChkFileRequest request)
        {
            ChkFileResponse response = new ChkFileResponse();
            try
            {
                List<FileAV> AVFiles = new List<FileAV>();
                FileAV fileav = new FileAV();
                string id = "";
                string queryString = "SELECT * FROM dbo.V_File WHERE User_ID = '" + request.UserId.ToString() + "' ORDER BY ActionDate;";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = reader["File_ID"].ToString();
                            fileav = new FileAV(new Guid(id), "", reader["Name"].ToString(),
                                DateTime.Parse(reader["ActionDate"].ToString()),
                                reader["MD5"].ToString(), reader["SHA1"].ToString(), reader["SHA256"].ToString());
                            fileav.ActionDesc = reader["Description"].ToString();
                            fileav.UserFileID = new Guid(reader["USERFile_ID"].ToString());
                            AVFiles.Add(fileav);
                        }
                    }
                    fileav._filesUploaded = AVFiles;
                    response.Count = AVFiles.Count;
                    response.Success = true;
                    response.Results = fileav;
                }
            }
            catch (SqlException exc)
            {
                response.Message = exc.Message;
            }
            return response;
        }
        private IEnumerable<FileAV> CreateListOfFiles(IDataReader datareader)
        {
            IList<FileAV> fileavs = new List<FileAV>();
            FileAV fileav;
            string id = "";
            while (datareader.Read())
            {
                id = datareader["File_ID"].ToString();
                fileav = new FileAV(new Guid(id), "", datareader["Name"].ToString(), 
                    DateTime.Parse(datareader["ActionDate"].ToString()),
                    datareader["MD5"].ToString(), datareader["SHA1"].ToString(), datareader["SHA256"].ToString());
                fileavs.Add(fileav);
            }
            return fileavs;
        }
        public FileCreateResponse Save(FileAV fileAV)
        {
            FileCreateResponse fileCreateResponse = new FileCreateResponse();
            return fileCreateResponse;
        }
        public IList<FileAV> FindAll()
        {
            IList<FileAV> accounts = new List<FileAV>();
            string queryString = "SELECT * FROM dbo.Transactions INNER JOIN " + "dbo.BankAccounts ON " +
                                 "dbo.Transactions.BankAccountId = dbo.BankAccounts.BankAccountId " +
                                 "ORDER BY dbo.BankAccounts.BankAccountId;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    //accounts = CreateListOfAccountsFrom(reader);
                }
            }
            return accounts;
        }
        public AVResultsResponse ReportFile(AVResultsRequest request)
        {
            AVResultsResponse response =  new AVResultsResponse();
            try
            {
                string queryString = "SELECT * FROM dbo.V_Report WHERE File_ID = @FileId;";
                AntiV lav = new AntiV();
                List<AntiV> Results = new List<AntiV>();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = queryString;
                    SqlParameter Idparam = new SqlParameter("@FileId", request.FileId);
                    command.Parameters.Add(Idparam);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lav = new AntiV(new Guid(reader["AV_ID"].ToString()), reader["AV_NAME"].ToString(), reader["MalwareName"].ToString(), reader["DetectType"].ToString(),
                                bool.Parse(reader["Result"].ToString()), DateTime.Parse(reader["AssessDate"].ToString()));
                            Results.Add(lav);
                        }
                    }
                    lav._results = Results;
                    response.Results = lav;
                }
            }
            catch (SqlException exc)
            {
                response.Message = exc.Message;
            }
            return response;
        }
    }
}