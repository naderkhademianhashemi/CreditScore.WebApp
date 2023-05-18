using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Ratification
    {
        //public BktLoans LoanRatification;
        public Int32 BRA_BCU_ID;
        public Int32 BRA_BBN_ID;
        public Int32 BRA_LoanTypeId;
        public Int32 BRA_LoanType2Id;
        public Int32 BRA_LoanRequestId;
        private long _BraId;
        private byte _BraAssurance1;
        private byte _BraAssurance2;
        private double _BraAmount;
        private int _BraPaymentNBR;
        private DateTime _BraCreateDate;
        private byte _BraStatus;

        public long BraId
        {
            get
            {
                return _BraId;
            }
            set
            {
                if (this._BraId != value)
                    this._BraId = value;
            }
        }

        /// درصد وثیقه اول
        public byte BraAssurance1
        {
            get
            {
                return _BraAssurance1;
            }
            set
            {
                if (this._BraAssurance1 != value)
                    this._BraAssurance1 = value;
            }
        }

        public byte BraAssurance2
        {
            get
            {
                return _BraAssurance2;
            }
            set
            {
                if (this._BraAssurance2 != value)
                    this._BraAssurance2 = value;
            }
        }

        public double BraAmount
        {
            get
            {
                return _BraAmount;
            }
            set
            {
                if (this._BraAmount != value)
                    this._BraAmount = value;
            }
        }

        public DateTime BraCreateDate
        {
            get
            {
                return _BraCreateDate;
            }
            set
            {
                if (this._BraCreateDate != value)
                    this._BraCreateDate = value;
            }
        }

        public byte BraStatus
        {
            get
            {
                return _BraStatus;
            }
            set
            {
                if (this._BraStatus != value)
                    this._BraStatus = value;
            }
        }
        public int BraPaymentNBR
        {
            get
            {
                return _BraPaymentNBR;
            }
            set
            {
                if (this._BraPaymentNBR != value)
                    this._BraPaymentNBR = value;
            }
        }
    }
}
