using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Repository;



namespace AlpsCreditScoring.Service.Messages
{
    public class UserRequest
    {
        
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Group { get; set; }
        public string Profile { get; set; }
        public string AccessLevel { get; set; }
        public int AccessId { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public int ProfileId { get; set; }
        public int SectionId { get; set; }
        public int CreatedBy { get; set; }
        public int Branch { get; set; }
        public bool bBranch { get; set; }
        public string Code { get; set; }
        public bool bCode { get; set; }
        public bool bMehr { get; set; }
        public bool bRetail { get; set; }
    }
}