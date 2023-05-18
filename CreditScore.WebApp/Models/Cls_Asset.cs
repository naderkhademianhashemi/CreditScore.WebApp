using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Asset
    {
        public Int32 BSS_TYPE_Id;
        public long BSS_BLO_ID;
        private long _BssId;
        private string _BssCode;
        private string _BssTitle;
        private double _BssAmount;
        private DateTime _BssCreateDate;
        private byte _BssStatus;

        public long BssId
        {
            get
            {
                return _BssId;
            }
            set
            {
                if (this._BssId != value)
                    this._BssId = value;
            }
        }

        public string BssCode
        {
            get
            {
                return _BssCode;
            }
            set
            {
                if (this._BssCode != value)
                    this._BssCode = value;
            }
        }

        public string BssTitle
        {
            get
            {
                return _BssTitle;
            }
            set
            {
                if (this._BssTitle != value)
                    this._BssTitle = value;
            }
        }

        public double BssAmount
        {
            get
            {
                return _BssAmount;
            }
            set
            {
                if (this._BssAmount != value)
                    this._BssAmount = value;
            }
        }
        public DateTime BssCreateDate
        {
            get
            {
                return _BssCreateDate;
            }
            set
            {
                if (this._BssCreateDate != value)
                    this._BssCreateDate = value;
            }
        }

        public byte BssStatus
        {
            get
            {
                return _BssStatus;
            }
            set
            {
                if (this._BssStatus != value)
                    this._BssStatus = value;
            }
        }
    }
}
