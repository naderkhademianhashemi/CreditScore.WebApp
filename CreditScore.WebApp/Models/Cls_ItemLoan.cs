using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_ItemLoan
    {
        //public BktLoans LoanItem;
        public Int32 BRI_KindItemId;
        public Int32 BRI_ItemTypeId;
        public long BRI_BLO_ID;
        private long _BriId;
        private string _BriInstitutionName;
        private double _BriAmount;
        private int _BriPaymentNbr;
        private double _BriPayment;
        private double _BriRemain;
        private DateTime _BriCreateDate;
        private byte _BriStatus;

        public long BriId
        {
            get
            {
                return _BriId;
            }
            set
            {
                if (this._BriId != value)
                    this._BriId = value;
            }
        }

        public string BriInstitutionName
        {
            get
            {
                return _BriInstitutionName;
            }
            set
            {
                if (this._BriInstitutionName != value)
                    this._BriInstitutionName = value;
            }
        }
        public double BriAmount
        {
            get
            {
                return _BriAmount;
            }
            set
            {
                if (this._BriAmount != value)
                    this._BriAmount = value;
            }
        }

        public int BriPaymentNbr
        {
            get
            {
                return _BriPaymentNbr;
            }
            set
            {
                if (this._BriPaymentNbr != value)
                    this._BriPaymentNbr = value;
            }
        }

        public double BriPayment
        {
            get
            {
                return _BriPayment;
            }
            set
            {
                if (this._BriPayment != value)
                    this._BriPayment = value;
            }
        }

        public double BriRemain
        {
            get
            {
                return _BriRemain;
            }
            set
            {
                if (this._BriRemain != value)
                    this._BriRemain = value;
            }
        }

        public DateTime BriCreateDate
        {
            get
            {
                return _BriCreateDate;
            }
            set
            {
                if (this._BriCreateDate != value)
                    this._BriCreateDate = value;
            }
        }

        public byte BriStatus
        {
            get
            {
                return _BriStatus;
            }
            set
            {
                if (this._BriStatus != value)
                    this._BriStatus = value;
            }
        }

    }
}
