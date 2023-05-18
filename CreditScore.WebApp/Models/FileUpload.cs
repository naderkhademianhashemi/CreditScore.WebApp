using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

 

namespace AlpsCreditScoring.Models
{
    public class FileAV
    {
        private Guid _fileid;
        private Guid _userfileid;
        private Guid _userid;
        private string _path;
        private string _name;
        private string _smd5;
        private string _ssha1;
        private string _ssha256;
        private string _ip;
        private byte[] _bmd5;
        private byte[] _bsha1;
        private byte[] _bsha256;
        private int _count;
        private DateTime _date;
        public IList<FileAV> _filesUploaded;
        private bool _queue;
        private short _actiontype;
        private string _actiondesc;
        
        
        public FileAV() : this(Guid.NewGuid(), "", "",DateTime.Now,null,null,null)
        {
           // _FilesUploaded.Add(new FileUpload(0m, 0m, "account created", DateTime.Now));
        }
        public FileAV(Guid Id, string Name)
        {
            _fileid = Id;
            _name = Name;
        }
        public FileAV(Guid Id, string Path, string Name, DateTime DateAction, string MD5, string SHA1, string SHA256)
        {
            _fileid = Id;
            _path = Path;
            _name = Name;
            _date = DateAction;
            _smd5 = MD5; _ssha1 = SHA1; _ssha256 = SHA256;

        }
        public Guid FileID
        {
            get { return _fileid; }
            internal set { _fileid = value; }
        }
        public Guid UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string IP
        {
            get { return _ip; }
            set { _ip = value; }
        }
        public DateTime DateAction
        {
            get { return _date; }
            internal set { _date = value; }
        }
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        public bool Queue
        {
            get { return _queue; }
            set { _queue = value; }
        }
        public short ActionType
        {
            get { return _actiontype; }
            set { _actiontype = value; }
        }
        public string ActionDesc
        {
            get { return _actiondesc; }
            set { _actiondesc = value; }
        }
        public string sMD5
        {
            get { return _smd5; }
            set { _smd5 = value; }
        }
        public string sSHA1
        {
            get { return _ssha1; }
            set { _ssha1 = value; }
        }
        public string sSHA256
        {
            get { return _ssha256; }
            set { _ssha256 = value; }
        }
        public byte[] bMD5
        {
            get { return _bmd5; }
            set { _bmd5 = value; }
        }
        public byte[] bSHA1
        {
            get { return _bsha1; }
            set { _bsha1 = value; }
        }
        public byte[] bSHA256
        {
            get { return _bsha256; }
            set { _bsha256 = value; }
        }
        public Guid UserFileID
        {
            get { return _userfileid; }
            set { _userfileid = value; }
        }
        public string Convert(IEnumerable<byte> bValues)
        {
            string sValue = "";
            foreach (byte Value in bValues)
            {
                sValue += String.Format("{0:x2}", Value);
            }
            return sValue;
        }
        public IEnumerable<FileAV> getResults()
        {
            return _filesUploaded;

        } 
       
    }
}