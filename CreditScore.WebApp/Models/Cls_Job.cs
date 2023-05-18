using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Job
    {
        public Int32 Bjo_PositionId;
        //public BktRealPerson bktRealPerson;
        private long _BjoId;
        private string _BjoTitle;
        private string _BjoPlaceTitle;
        private byte _BjoSeq;
        private int _BjoDuration;
        private double _BjoSalary;
        private DateTime _BjoCreateDate;
        private byte _BjoStatus;

        public long BjoId
        {
            get
            {
                return _BjoId;
            }
            set
            {
                if (this._BjoId != value)
                    this._BjoId = value;
            }
        }

        /// عنوان اشتغال
        public string BjoTitle
        {
            get
            {
                return _BjoTitle;
            }
            set
            {
                if (this._BjoTitle != value)
                    this._BjoTitle = value;
            }
        }

        /// عنوان محل اشتغال
        public string BjoPlaceTitle
        {
            get
            {
                return _BjoPlaceTitle;
            }
            set
            {
                if (this._BjoPlaceTitle != value)
                    this._BjoPlaceTitle = value;
            }
        }

        /// چند شغله بودن در اين جا مشخص مي گردد
        public byte BjoSeq
        {
            get
            {
                return _BjoSeq;
            }
            set
            {
                if (this._BjoSeq != value)
                    this._BjoSeq = value;
            }
        }

        public int BjoDuration
        {
            get
            {
                return _BjoDuration;
            }
            set
            {
                if (this._BjoDuration != value)
                    this._BjoDuration = value;
            }
        }

        public double BjoSalary
        {
            get
            {
                return _BjoSalary;
            }
            set
            {
                if (this._BjoSalary != value)
                    this._BjoSalary = value;
            }
        }

        public DateTime BjoCreateDate
        {
            get
            {
                return _BjoCreateDate;
            }
            set
            {
                if (this._BjoCreateDate != value)
                    this._BjoCreateDate = value;
            }
        }

        public byte BjoStatus
        {
            get
            {
                return _BjoStatus;
            }
            set
            {
                if (this._BjoStatus != value)
                    this._BjoStatus = value;
            }
        }
    }
}
