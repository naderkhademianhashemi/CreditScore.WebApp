using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Collateral
    {
        public Int32 BCL_TypeID;
        public long BCL_BLO_ID;
        private long _BclId;
        private double _BclValue;
        private double _BclTarhiniValue;
        private string _Type;
        public long BclId
        {
            get
            {
                return _BclId;
            }
            set
            {
                if (this._BclId != value)
                    this._BclId = value;
            }
        }
        /// مبلغ وثیقه
        public double BclValue
        {
            get
            {
                return _BclValue;
            }
            set
            {
                if (this._BclValue != value)
                    this._BclValue = value;
            }
        }

        /// مبلغ ترهینی وثیقه
        public double BclTarhiniValue
        {
            get
            {
                return _BclTarhiniValue;
            }
            set
            {
                if (this._BclTarhiniValue != value)
                    this._BclTarhiniValue = value;
            }
        }
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                if (this._Type != value)
                    this._Type = value;
            }
        }
    }
}
