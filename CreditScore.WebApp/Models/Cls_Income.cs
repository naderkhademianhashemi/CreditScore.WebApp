using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Income
    {
        public Int32 Bin_IncomeTypeID;
        public long BIN_BLO_ID;
        private long _BinId;
        private string _BinCode;
        private string _BinTitle;
        private double _BinAmount;
        private DateTime _BinCreateDate;
        private byte _BinStatus;

        public long BinId
        {
            get
            {
                return _BinId;
            }
            set
            {
                if (this._BinId != value)
                    this._BinId = value;
            }
        }

        public string BinCode
        {
            get
            {
                return _BinCode;
            }
            set
            {
                if (this._BinCode != value)
                    this._BinCode = value;
            }
        }

        public string BinTitle
        {
            get
            {
                return _BinTitle;
            }
            set
            {
                if (this._BinTitle != value)
                    this._BinTitle = value;
            }
        }
        public double BinAmount
        {
            get
            {
                return _BinAmount;
            }
            set
            {
                if (this._BinAmount != value)
                    this._BinAmount = value;
            }
        }

        public DateTime BinCreateDate
        {
            get
            {
                return _BinCreateDate;
            }
            set
            {
                if (this._BinCreateDate != value)
                    this._BinCreateDate = value;
            }
        }

        public byte BinStatus
        {
            get
            {
                return _BinStatus;
            }
            set
            {
                if (this._BinStatus != value)
                    this._BinStatus = value;
            }
        }

    }
}
