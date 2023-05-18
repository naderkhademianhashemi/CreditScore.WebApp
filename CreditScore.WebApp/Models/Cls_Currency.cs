using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Currency
    {
        //public BktRatification[] Rat_Currency;
        private long _BcuId;
        private string _BcuTitle;
        private double _BcuAmount;
        private DateTime _BcuDate;
        private byte _BcuStatus;

        public long BcuId
        {
            get
            {
                return _BcuId;
            }
            set
            {
                if (this._BcuId != value)
                    this._BcuId = value;
            }
        }

        public string BcuTitle
        {
            get
            {
                return _BcuTitle;
            }
            set
            {
                if (this._BcuTitle != value)
                    this._BcuTitle = value;
            }
        }

        public double BcuAmount
        {
            get
            {
                return _BcuAmount;
            }
            set
            {
                if (this._BcuAmount != value)
                    this._BcuAmount = value;
            }
        }

        public DateTime BcuDate
        {
            get
            {
                return _BcuDate;
            }
            set
            {
                if (this._BcuDate != value)
                    this._BcuDate = value;
            }
        }

        public byte BcuStatus
        {
            get
            {
                return _BcuStatus;
            }
            set
            {
                if (this._BcuStatus != value)
                    this._BcuStatus = value;
            }
        }
    }
}
