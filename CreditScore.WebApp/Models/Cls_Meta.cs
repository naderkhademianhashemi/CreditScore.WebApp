using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Meta
    {
        private long _BmdId;
        private String _BmdKey;
        private String _BmdValue;
        private long _BmdSubjectId;
        private DateTime _BmdCreateDate;
        private Byte _BmdStatus;
        public Int32 BMD_FOR_ID;

        public long BmdId
        {
            get
            {
                return _BmdId;
            }
            set
            {
                if (this._BmdId != value)
                    this._BmdId = value;
            }
        }

        public String BmdKey
        {
            get
            {
                return _BmdKey;
            }
            set
            {
                if (this._BmdKey != value)
                    this._BmdKey = value;
            }
        }

        public String BmdValue
        {
            get
            {
                return _BmdValue;
            }
            set
            {
                if (this._BmdValue != value)
                    this._BmdValue = value;
            }
        }

        public long BmdSubjectId
        {
            get
            {
                return _BmdSubjectId;
            }
            set
            {
                if (this._BmdSubjectId != value)
                    this._BmdSubjectId = value;
            }
        }

        public DateTime BmdCreateDate
        {
            get
            {
                return _BmdCreateDate;
            }
            set
            {
                if (this._BmdCreateDate != value)
                    this._BmdCreateDate = value;
            }
        }

        public Byte BmdStatus
        {
            get
            {
                return _BmdStatus;
            }
            set
            {
                if (this._BmdStatus != value)
                    this._BmdStatus = value;
            }
        }

    }
}
