using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;

namespace AlpsCreditScoring.Service.Messages
{
    public class AVResultsResponse : ResponseBase
    {
        public AntiV Results { get; set; } 
    }
}