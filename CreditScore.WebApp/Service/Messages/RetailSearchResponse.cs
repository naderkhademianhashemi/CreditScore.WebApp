using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using AlpsCreditScoring.Models;

namespace AlpsCreditScoring.Service.Messages
{
    public class RetailSearchResponse : ResponseBase
    {
        public Cls_Loan Loan { get; set; }
        public Cls_Loan[] Loans { get; set; }
        public Cls_Job Job { get; set; }
        public Cls_Meta[] Meta { get; set; }
        public DataTable Table { get; set; }
        public DataRow Row { get; set; }
        public Int64 count { get; set; }
        public Int64 Id { get; set; }
    }
}
