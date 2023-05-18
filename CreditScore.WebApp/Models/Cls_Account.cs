using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Account
    {
        public Int32 BKA_AccountTypeId;
        public long BKA_BLO_ID;
        public long Bka_BankId;
        private long _BkaId;
        private string _BankName;
        private string _BkaAccountNo;
        private double _BkaCapital;
        private double _BkaTurnover;
        private DateTime _BkaOpenDate;
        private byte _BkaStatus;

        public long BkaId
        {
            get
            {
                return _BkaId;
            }
            set
            {
                if (this._BkaId != value)
                    this._BkaId = value;
            }
        }
        public string BankName
        {
            get
            {
                return _BankName;
            }
            set
            {
                if (this._BankName != value)
                    this._BankName = value;
            }
        }
        public string BkaAccountNo
        {
            get
            {
                return _BkaAccountNo;
            }
            set
            {
                if (this._BkaAccountNo != value)
                    this._BkaAccountNo = value;
            }
        }

        public double BkaCapital
        {
            get
            {
                return _BkaCapital;
            }
            set
            {
                if (this._BkaCapital != value)
                    this._BkaCapital = value;
            }
        }

        /// گردش بستانکار حساب
        public double BkaTurnover
        {
            get
            {
                return _BkaTurnover;
            }
            set
            {
                if (this._BkaTurnover != value)
                    this._BkaTurnover = value;
            }
        }

        public DateTime BkaOpenDate
        {
            get
            {
                return _BkaOpenDate;
            }
            set
            {
                if (this._BkaOpenDate != value)
                    this._BkaOpenDate = value;
            }
        }

        public byte BkaStatus
        {
            get
            {
                return _BkaStatus;
            }
            set
            {
                if (this._BkaStatus != value)
                    this._BkaStatus = value;
            }
        }
    }
}
