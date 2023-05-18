using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;

namespace AlpsCreditScoring.Service.Messages
{
    public class RetailBaseResponse : ResponseBase
    {
        public Dictionary<int, string> Gender = new Dictionary<int, string>();
        public Dictionary<int, string> Marital = new Dictionary<int, string>();
        public Dictionary<int, string> EduDegree = new Dictionary<int, string>();
        public Dictionary<int, string> LoanType = new Dictionary<int, string>();
        public Dictionary<int, string> LoanState = new Dictionary<int, string>();
        public Dictionary<int, string> Account = new Dictionary<int, string>();
        public Dictionary<int, string> Answer = new Dictionary<int, string>();
        public Dictionary<int, string> Children = new Dictionary<int, string>();
        public Dictionary<int, string> PrsInCharge = new Dictionary<int, string>();
        public Dictionary<int, string> Expenditure = new Dictionary<int, string>();
        public Dictionary<int, string> EduDegreeSpouse = new Dictionary<int, string>();
        public Dictionary<int, string> HealthState = new Dictionary<int, string>();
        public Dictionary<int, string> HealthFamily = new Dictionary<int, string>();
        public Dictionary<int, string> HealthSpouse = new Dictionary<int, string>();
        public Dictionary<int, string> HouseState = new Dictionary<int, string>();
        public Dictionary<int, string> Income = new Dictionary<int, string>();
        public Dictionary<int, string> JobState = new Dictionary<int, string>();
        public Dictionary<int, string> JobStatSpouse = new Dictionary<int, string>();
        public Dictionary<int, string> LoanRequest = new Dictionary<int, string>();
        public Dictionary<int, string> ServiceDue = new Dictionary<int, string>();
        public Dictionary<int, string> LoanGuaranty = new Dictionary<int, string>();
        public Dictionary<int, string> Position = new Dictionary<int, string>();
        public Dictionary<int, string> PosSpouse = new Dictionary<int, string>();
        public Dictionary<int, string> Currency = new Dictionary<int, string>();
        public Dictionary<int, string> Bank = new Dictionary<int, string>();
        public Dictionary<int, string> Aim = new Dictionary<int, string>();
        public Dictionary<int, string> Collateral = new Dictionary<int, string>();
        public Dictionary<int, string> EconomicSector = new Dictionary<int, string>();
        public Dictionary<int, string> Resident = new Dictionary<int, string>();
        public Dictionary<int, string> Ozviat = new Dictionary<int, string>();
        public Dictionary<int, string> Shohrat = new Dictionary<int, string>();
        public Dictionary<int, string> Niroou = new Dictionary<int, string>();
        public Dictionary<int, string> CustomerGroup = new Dictionary<int, string>();
        public Dictionary<int, string> ContractType = new Dictionary<int, string>();
        public Dictionary<int, string> FacilityType = new Dictionary<int, string>();
    }
}