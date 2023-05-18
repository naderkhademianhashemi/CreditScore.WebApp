using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;


namespace AlpsCreditScoring.Service.Messages
{
    public class AVResultsRequest
    {
        public Guid FileId { get; set; }

        public Guid UserId { get; set; }

        public IList<AntiV> Results { get; set; }
    }
}