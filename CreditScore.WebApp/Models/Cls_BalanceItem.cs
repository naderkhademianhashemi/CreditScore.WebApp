using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    
    public class Cls_BalanceItem
    {
        public long BbiId;
        public long BBL_ID;
        public Int32 BBI_GROUP_ID;
        public Int32 BBI_TYPE_ID;
        private double _BbiValue;
        private byte _BbiSeq;
        private string _group;
        private string _type;
        public double BbiValue
        {
            get
            {
                return _BbiValue;
            }
            set
            {
                if (this._BbiValue != value)
                    this._BbiValue = value;
            }
        }
        public byte BbiSeq
        {
            get
            {
                return _BbiSeq;
            }
            set
            {
                if (this._BbiSeq != value)
                    this._BbiSeq = value;
            }
        }
        public string Group
        {
            get
            {
                return _group;
            }
            set
            {
                if (this._group != value)
                    this._group = value;
            }
        }
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (this._type != value)
                    this._type = value;
            }
        }
    }
}
