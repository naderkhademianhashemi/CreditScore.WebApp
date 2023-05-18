using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;

namespace AlpsCreditScoring.Service.Messages
{
    public class CorpBaseResponse : ResponseBase
    {
        public Dictionary<int, string> CorpType = new Dictionary<int, string>();
        public Dictionary<int, string> CorpActivity = new Dictionary<int, string>();
        public Dictionary<int, string> LoanState = new Dictionary<int, string>();
        public Dictionary<int, string> Answer = new Dictionary<int, string>();
        public Dictionary<int, string> PermitType = new Dictionary<int, string>();
        public Dictionary<int, string> PermitSource = new Dictionary<int, string>();
        public Dictionary<int, string> Account = new Dictionary<int, string>();
        public Dictionary<int, string> LoanRequest = new Dictionary<int, string>();
        public Dictionary<int, string> LoanType = new Dictionary<int, string>();
        public Dictionary<int, string> Guaranty = new Dictionary<int, string>();
        public Dictionary<int, string> EduDegree = new Dictionary<int, string>();
        public Dictionary<int, string> ShareEduDegree = new Dictionary<int, string>();
        public Dictionary<int, string> Currency = new Dictionary<int, string>();
        public Dictionary<int, string> Bank = new Dictionary<int, string>();
        public Dictionary<int, string> Bank2 = new Dictionary<int, string>();
        public Dictionary<int, string> Position = new Dictionary<int, string>();
        public Dictionary<int, string> AimLoan = new Dictionary<int, string>();
        public Dictionary<int, string> Collateral = new Dictionary<int, string>();
        public Dictionary<int, string> EconomicSector = new Dictionary<int, string>();
        public Dictionary<int, string> ContractType = new Dictionary<int, string>();
        public Dictionary<int, string> CustomerGroup = new Dictionary<int, string>();
    }
}