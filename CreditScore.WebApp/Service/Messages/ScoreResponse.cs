using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Repository;

namespace AlpsCreditScoring.Service.Messages
{
    public class ScoreResponse : ResponseBase
    {
        
        public string RetailModelSM { get; set; }
        public string RetailModelJG { get; set; }
        public string BuisModelSM { get; set; }
        public string BuisModelJG { get; set; }
        public double PD { get; set; }
        public int UserID { get; set; }


    }
}