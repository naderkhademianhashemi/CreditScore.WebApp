using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;
using AlpsCreditScoring.Service.Messages;


namespace AlpsCreditScoring.Service
{
    public class UserService
    {
        private IAlpsRepository _AlpsRepository;
        public UserService(IAlpsRepository AlpsRepository)
        {
            _AlpsRepository = AlpsRepository;
        }
        public UserResponse GetGroup()
        {
            return _AlpsRepository.BaseGroup();
        }
        public UserResponse GetAccesses()
        {
            return _AlpsRepository.BaseAccesses();
        }
        public UserResponse InsertNewUser(UserRequest request)
        {
            return _AlpsRepository.InsertUser(request);
        }
        public UserResponse LookUpUser()
        {
             string query = "Select [User_Id],[User_Name],[GroupName],[User_Branch],[User_State],[User_Group_Id] from V_USER";
             return _AlpsRepository.Lookup(query);
        }
        public UserResponse LookUpBranch(UserRequest request)
        {
            string query = "";
            if (!request.bBranch)
                query = "Select Branch, Branch_Description,State_Id,State_Name from [dbo].[Organization] O inner join " +
                           "[dbo].[State] S On O.State_Of_Branch = S.State_Id";
            else
                query = "Select Branch, Branch_Description,State_Id,State_Name from [dbo].[Organization] O inner join " +
                           "[dbo].[State] S On O.State_Of_Branch = S.State_Id Where Branch Like '%" + request.Branch.ToString() + "%'";
            return _AlpsRepository.Lookup(query);
        }
        public UserResponse LookUpBranches(UserRequest request)
        {
            string query = "";
            if (request.bMehr)
            {
                query = "SELECT SHOBE FROM dbo.V_SHOBE ORDER BY SHOBE";
            }
            else
            {
                query = "SELECT SHOBE FROM dbo.T_SHOBE_NEGIN ORDER BY SHOBE";
            }
            return _AlpsRepository.Lookup(query);
        }
        
        public UserResponse LookUpCustomer(UserRequest request)
        {
            string query = "";
            if (!request.bCode)
                query = "SELECT* FROM dbo.V_BNK_REAL";
            else
                query = "SELECT * FROM dbo.V_BNK_REAL WHERE BPR_CODE Like '%" + request.Code + "%'";
            return _AlpsRepository.Lookup(query);
        }
        public UserResponse LookUpCorp(UserRequest request)
        {
            string query = "";
            if (!request.bCode)
                query = "SELECT* FROM dbo.V_BNK_CORP";
            else
                query = "SELECT * FROM dbo.V_BNK_CORP WHERE BPR_CODE LIKE '%" + request.Code + "%'";
            return _AlpsRepository.Lookup(query);
        }
        public UserResponse LookUpProfile()
        {
            string query = "Select [ID],[Profile_Id],[DMR_Model],[DMB_Model],[GMR_Model],[GMB_Model] from [dbo].[Profile]";
            return _AlpsRepository.Lookup(query);
        }
        public UserResponse LookUpJudgFields(RetailSearchRequest request)
        {
            string query = "SELECT * FROM dbo.V_AHP_MODEL WHERE AHP_MODEL_NAME LIKE N'" + request.Model + "'";
            return _AlpsRepository.Lookup(query);
        }
        public UserResponse LookUpJudgBuildTree(RetailSearchRequest request)
        {
            string query = "with ReportingTree(ID, AHP_Element_Name, Parent_Id, PARENT, Amount_Score, AHP_Map_Id, Seq, lvl, sort) As(" +
                    "Select ID, AHP_Element_Name, Parent_Id, PARENT, Amount_Score, AHP_Map_Id, Seq, 0 as lvl, cast(AHP_Element_Name as nvarchar(max)) as sort " +
                    "From dbo.V_AHP_MODEL where Parent_Id = -1 and AHP_MODEL_NAME LIKE N'" + request.Model + "'" +
                    "UNION ALL " +
                    "select A.ID, A.AHP_Element_Name, A.Parent_Id, A.PARENT, A.Amount_Score, A.AHP_Map_Id, A.Seq, ReportingTree.lvl + 1, ReportingTree.sort + '/' + cast(A.AHP_Element_Name as nvarchar(max)) as sort " +
                    "From dbo.V_AHP_MODEL A INNER JOIN ReportingTree ON A.Parent_Id = ReportingTree.ID ) " +
                    "SELECT ID, AHP_Element_Name, PARENT, Amount_Score,Parent_Id,AHP_Map_Id,lvl FROM ReportingTree order by ReportingTree.sort";
            return _AlpsRepository.Lookup(query);
        }
        public UserResponse LookUpJudgFieldsPerson(RetailSearchRequest request)
        {
            string query = "Select * from dbo.AHP_Customer_Judge where Cust_BPR_ID = " + request.BprID.ToString() +
                " And Create_DT = (Select Max(Create_DT) from dbo.AHP_Customer_Judge where Cust_BPR_ID = " + request.BprID.ToString() + ")";
            return _AlpsRepository.Lookup(query);
        }
        public UserResponse LookUpByQuery(string query)
        {
            return _AlpsRepository.Lookup(query);
        }
        public UserResponse DeleteUser(string UserName)
        {
            return _AlpsRepository.DeleteUser(UserName);
        }
        public UserResponse InsertProfile(UserRequest request)
        {
            return _AlpsRepository.InsertProfile(request);
        }
        public UserResponse UpdateProfile(UserRequest request)
        {
            return _AlpsRepository.UpdateProfile(request);
        }
        public UserResponse UpdateGroup(UserRequest request)
        {
            return _AlpsRepository.UpdateGroup(request);
        }
        public UserResponse SetGroupAccess(UserRequest request)
        {
            return _AlpsRepository.GroupAccess(request);
        }
        public UserResponse DeleteGroup(int GroupID)
        {
            return _AlpsRepository.DeleteGroup(GroupID);
        }
        public UserResponse InsertGroup(UserRequest request)
        {
            return _AlpsRepository.InsertGroup(request);
        }
    }
}