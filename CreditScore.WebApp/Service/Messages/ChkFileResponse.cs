using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;


namespace AlpsCreditScoring.Service.Messages
{
    public class ChkFileResponse : ResponseBase
    {
        public FileAV Results { get; set; }
        public int Count { get; set; }
    }
}