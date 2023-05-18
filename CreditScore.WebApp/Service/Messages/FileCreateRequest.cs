using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AlpsCreditScoring.Service.Messages
{
    public class FileCreateRequest
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string path { get; set; }
        
        

    }
}