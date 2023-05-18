using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Address
    {
        public Int32 Bap_TypeID;
        public AlpsCreditScoring.Models.Cls_Person bktPerson;
        private long _BapId;
        private string _BapValue;
        private byte _BapPriority;
        private byte _BapStatus;
        private long _BapSubjectId;
        private DateTime _BapCreateDate;
        public long BapId
        {
            get
            {
                return _BapId;
            }
            set
            {
                if (this._BapId != value)
                    this._BapId = value;
            }
        }

        public string BapValue
        {
            get
            {
                return _BapValue;
            }
            set
            {
                if (this._BapValue != value)
                    this._BapValue = value;
            }
        }

        public byte BapPriority
        {
            get
            {
                return _BapPriority;
            }
            set
            {
                if (this._BapPriority != value)
                    this._BapPriority = value;
            }
        }

        public byte BapStatus
        {
            get
            {
                return _BapStatus;
            }
            set
            {
                if (this._BapStatus != value)
                    this._BapStatus = value;
            }
        }
        private long BapSubjectId
        {
            get
            {
                return _BapSubjectId;
            }
            set
            {
                if (this._BapSubjectId != value)
                    this._BapSubjectId = value;
            }
        }
        private DateTime BapCreateDate
        {
            get
            {
                return _BapCreateDate;
            }
            set
            {
                if (this._BapCreateDate != value)
                    this._BapCreateDate = value;
            }
        }
    }
}
