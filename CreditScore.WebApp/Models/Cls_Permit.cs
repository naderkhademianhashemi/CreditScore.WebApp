using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Permit
    {
        public long BcpId;
        public int BPE_TYPE_ID;
        private string _BpeType;
        private int _BpeId;
        private string _BpePermitNo;
        private DateTime _BpeIssueDate;
        private DateTime _BpeExpireDate;
        private string _BpeIssuePlace;
        public Int32 BPE_IssueId;
        private byte _BpeValidMonth;
        private DateTime _BpeCreateDate;
        private byte _BpeStatus;
        public int BpeId
        {
            get
            {
                return _BpeId;
            }
            set
            {
                if (this._BpeId != value)
                    this._BpeId = value;
            }
        }
        public string BpePermitNo
        {
            get
            {
                return _BpePermitNo;
            }
            set
            {
                if (this._BpePermitNo != value)
                    this._BpePermitNo = value;
            }
        }
        public DateTime BpeIssueDate
        {
            get
            {
                return _BpeIssueDate;
            }
            set
            {
                if (this._BpeIssueDate != value)
                    this._BpeIssueDate = value;
            }
        }

        public DateTime BpeExpireDate
        {
            get
            {
                return _BpeExpireDate;
            }
            set
            {
                if (this._BpeExpireDate != value)
                    this._BpeExpireDate = value;
            }
        }
        public string BpeIssuePlace
        {
            get
            {
                return _BpeIssuePlace;
            }
            set
            {
                if (this._BpeIssuePlace != value)
                    this._BpeIssuePlace = value;
            }
        }
        public string BpeType
        {
            get
            {
                return _BpeType;
            }
            set
            {
                if (this._BpeType != value)
                    this._BpeType = value;
            }
        }
        public byte BpeValidMonth
        {
            get
            {
                return _BpeValidMonth;
            }
            set
            {
                if (this._BpeValidMonth != value)
                    this._BpeValidMonth = value;
            }
        }
        public DateTime BpeCreateDate
        {
            get
            {
                return _BpeCreateDate;
            }
            set
            {
                if (this._BpeCreateDate != value)
                    this._BpeCreateDate = value;
            }
        }

        public byte BpeStatus
        {
            get
            {
                return _BpeStatus;
            }
            set
            {
                if (this._BpeStatus != value)
                    this._BpeStatus = value;
            }
        }
    }
}
