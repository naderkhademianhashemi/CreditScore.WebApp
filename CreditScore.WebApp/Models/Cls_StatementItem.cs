using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_StatementItem
    {
        public long BsiId;

        public long BST_ID;
        public Int32 BSI_GROUP_ID;
        public Int32 BSI_TYPE_ID;
        private double _BsiValue;
        private byte _BsiSeq;
        private string _group;
        private string _type;

        public double BsiValue
        {
            get
            {
                return _BsiValue;
            }
            set
            {
                if (this._BsiValue != value)
                    this._BsiValue = value;
            }
        }
        public byte BsiSeq
        {
            get
            {
                return _BsiSeq;
            }
            set
            {
                if (this._BsiSeq != value)
                    this._BsiSeq = value;
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
