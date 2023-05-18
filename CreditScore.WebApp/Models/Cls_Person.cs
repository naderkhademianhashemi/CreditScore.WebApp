using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Person
    {
        public Cls_Address[] bktAddress;
        private long _BprId;
        private string _BprCode;
        private byte _BprType;
        private byte _BprStatus;
        private DateTime _BprDate;
        
        public long BprId
        {
            get
            {
                return _BprId;
            }
            set
            {
                if (this._BprId != value)
                    this._BprId = value;
            }
        }
        /// Could be the customer code
        public string BprCode
        {
            get
            {
                return _BprCode;
            }
            set
            {
                if (this._BprCode != value)
                    this._BprCode = value;
            }
        }
        /// The type of the person, Corporate or Real
        public byte BprType
        {
            get
            {
                return _BprType;
            }
            set
            {
                if (this._BprType != value)
                    this._BprType = value;
            }
        }
        public byte BprStatus
        {
            get
            {
                return _BprStatus;
            }
            set
            {
                if (this._BprStatus != value)
                    this._BprStatus = value;
            }
        }
        public DateTime BprDate
        {
            get
            {
                return _BprDate;
            }
            set
            {
                if (this._BprDate != value)
                    this._BprDate = value;
            }
        }
    }
}
