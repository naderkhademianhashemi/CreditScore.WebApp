using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlpsCreditScoring.Models
{
    public class AntiV
    {
        private Guid _avid;
        private Guid _userfileid;
        private Guid _userid;
        private Guid _fileid;
        private bool _result;
        private bool _active;
        private string _malware;
        private string _avname;
        private string _detecttype;
        private DateTime _date;
        public IList<AntiV> _results;

        public AntiV() : this(Guid.NewGuid(), "", "", "", false, DateTime.Now)
        {
           // _FilesUploaded.Add(new FileUpload(0m, 0m, "account created", DateTime.Now));
        }
        
        public AntiV(Guid AVId, string AVName,string MalwareName, string DetectType, bool Result, DateTime AssessDate)
        {
            _avid = AVId;
            _avname = AVName;
            _malware = MalwareName;
            _detecttype = DetectType;
            _date = AssessDate;
            _result = Result;
        }
        public Guid AV_ID
        {
            get { return _avid; }
            internal set { _avid = value; }
        }
        public Guid UserFileID
        {
            get { return _userfileid; }
            set { _userfileid = value; }
        }
        public Guid FileID
        {
            get { return _fileid; }
            set { _fileid = value; }
        }
        public string MalwareName
        {
            get { return _malware; }
            set { _malware = value; }
        }
        public string AVName
        {
            get { return _avname; }
            set { _avname = value; }
        }
        public bool Result
        {
            get { return _result; }
            set { _result = value; }
        }
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }
        public IEnumerable<AntiV> getResults()
        {
             return _results; 
            
        }
        public DateTime AssessDate
        {
            get { return _date; }
            internal set { _date = value; }
        }
        public string DetectType
        {
            get { return _detecttype; }
            set { _detecttype = value; }
        }
    }
}