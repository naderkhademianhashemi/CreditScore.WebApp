using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;

namespace AlpsCreditScoring.Service.Messages
{
    public class ChkFileRequest
    {
        public Guid UserId { get; set; }

        public IList<FileAV> Results { get; set; }
    }
}