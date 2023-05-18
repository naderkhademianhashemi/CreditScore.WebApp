using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Expenditure
    {
        public long BEX_BLO_ID;
        public Int32 Bex_ExpendTypeID;
        private long _BexId;
        private string _BexCode;
        private string _BexTitle;
        private double _BexAmount;
        private DateTime _BexCreateDate;
        private byte _BexStatus;

        public long BexId
        {
            get
            {
                return _BexId;
            }
            set
            {
                if (this._BexId != value)
                    this._BexId = value;
            }
        }

        public string BexCode
        {
            get
            {
                return _BexCode;
            }
            set
            {
                if (this._BexCode != value)
                    this._BexCode = value;
            }
        }

        public string BexTitle
        {
            get
            {
                return _BexTitle;
            }
            set
            {
                if (this._BexTitle != value)
                    this._BexTitle = value;
            }
        }
        public double BexAmount
        {
            get
            {
                return _BexAmount;
            }
            set
            {
                if (this._BexAmount != value)
                    this._BexAmount = value;
            }
        }

        public DateTime BexCreateDate
        {
            get
            {
                return _BexCreateDate;
            }
            set
            {
                if (this._BexCreateDate != value)
                    this._BexCreateDate = value;
            }
        }

        public byte BexStatus
        {
            get
            {
                return _BexStatus;
            }
            set
            {
                if (this._BexStatus != value)
                    this._BexStatus = value;
            }
        }

    }
}
