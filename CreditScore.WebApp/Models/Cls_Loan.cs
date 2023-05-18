using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    
    public class Cls_Loan
    {
        public AlpsCreditScoring.Models.Cls_RealPerson bktCustomer = new AlpsCreditScoring.Models.Cls_RealPerson();
        public AlpsCreditScoring.Models.Cls_CorpPerson bktCorp;
        public AlpsCreditScoring.Models.Cls_Expenditure[] bktExpenditures;
        public AlpsCreditScoring.Models.Cls_Asset[] bktAssets;
        public AlpsCreditScoring.Models.Cls_Account[] bktAccounts;
        public AlpsCreditScoring.Models.Cls_Income[] bktIncome;
        public AlpsCreditScoring.Models.Cls_ItemLoan[] bktActualLoans;
        public AlpsCreditScoring.Models.Cls_ItemLoan[] bktActualGuaranty;
        public AlpsCreditScoring.Models.Cls_Collateral[] bktCollaterals;
        public AlpsCreditScoring.Models.Cls_Ratification bktRatification;
        public Int32 BLO_LoanTypeId;
        public Int32 BLO_LoanType2Id;
        public Int32 BLO_LoanRequest;
        public Int32 BLO_LoanState;
        public Int32 BLO_LoanAim;
        public Int32 BLO_BCU_ID;
        public Int32 BLO_EconomicSector;
        public Int32 BLO_CustomerGroup;
        public Int32 BLO_ContractType;
        public String StateCode;
        public long BLO_BBN_ID;
        private long _BloId;
        private string _BloCode;
        private double _BloAmount;
        private double _BloPayment;
        private byte _BloPaymentPeriod;
        private int _BloPaymentNbr;
        private string _BloAccountNo;
        private byte _BloAssurance1;
        private byte _BloAssurance2;
        private DateTime _BloPaymentDate;
        private DateTime _BloCreateDate;
        private DateTime _BloMaturityDate;
        private byte _BloStatus;
        private string _BloBranchCode;
        private bool _BloFlgSpouse;
        private bool _BloFlgLoan;
        private bool _BloFlgIncome;
        private bool _BloFlgExpend;
        private bool _BloFlgAsset;
        private bool _BloFlgActLoan;
        private bool _BloFlgActGuaranty;
        public long BloId
        {
            get
            {
                return _BloId;
            }
            set
            {
                if (this._BloId != value)
                    this._BloId = value;
            }
        }
        public string BloCode
        {
            get
            {
                return _BloCode;
            }
            set
            {
                if (this._BloCode != value)
                    this._BloCode = value;
            }
        }
       /// مبلغ وام
        public double BloAmount
        {
            get
            {
                return _BloAmount;
            }
            set
            {
                if (this._BloAmount != value)
                    this._BloAmount = value;
            }
        }

        /// مبلغ قسط
        public double BloPayment
        {
            get
            {
                return _BloPayment;
            }
            set
            {
                if (this._BloPayment != value)
                    this._BloPayment = value;
            }
        }

        /// فاصله اقساط
        public byte BloPaymentPeriod
        {
            get
            {
                return _BloPaymentPeriod;
            }
            set
            {
                if (this._BloPaymentPeriod != value)
                    this._BloPaymentPeriod = value;
            }
        }
        /// شماره حساب
        public string BloAccountNo
        {
            get
            {
                return _BloAccountNo;
            }
            set
            {
                if (this._BloAccountNo != value)
                    this._BloAccountNo = value;
            }
        }
        public byte BloAssurance1
        {
            get
            {
                return _BloAssurance1;
            }
            set
            {
                if (this._BloAssurance1 != value)
                    this._BloAssurance1 = value;
            }
        }
        public byte BloAssurance2
        {
            get
            {
                return _BloAssurance2;
            }
            set
            {
                if (this._BloAssurance2 != value)
                    this._BloAssurance2 = value;
            }
        }

        /// تعداد اقساط
        public int BloPaymentNbr
        {
            get
            {
                return _BloPaymentNbr;
            }
            set
            {
                if (this._BloPaymentNbr != value)
                    this._BloPaymentNbr = value;
            }
        }

        public DateTime BloPaymentDate
        {
            get
            {
                return _BloPaymentDate;
            }
            set
            {
                if (this._BloPaymentDate != value)
                    this._BloPaymentDate = value;
            }
        }

        public DateTime BloCreateDate
        {
            get
            {
                return _BloCreateDate;
            }
            set
            {
                if (this._BloCreateDate != value)
                    this._BloCreateDate = value;
            }
        }
        public DateTime BloMaturityDate
        {
            get
            {
                return _BloMaturityDate;
            }
            set
            {
                if (this._BloMaturityDate != value)
                    this._BloMaturityDate = value;
            }
        }
        public byte BloStatus
        {
            get
            {
                return _BloStatus;
            }
            set
            {
                if (this._BloStatus != value)
                    this._BloStatus = value;
            }
        }
        public string BloBranchCode
        {
            get
            {
                return _BloBranchCode;
            }
            set
            {
                if (this._BloBranchCode != value)
                    this._BloBranchCode = value;
            }
        }
        public bool BloFlgSpouse
        {
            get
            {
                return _BloFlgSpouse;
            }
            set
            {
                if (this._BloFlgSpouse != value)
                    this._BloFlgSpouse = value;
            }
        }
        public bool BloFlgLoan
        {
            get
            {
                return _BloFlgLoan;
            }
            set
            {
                if (this._BloFlgLoan != value)
                    this._BloFlgLoan = value;
            }
        }
        public bool BloFlgIncome
        {
            get
            {
                return _BloFlgIncome;
            }
            set
            {
                if (this._BloFlgIncome != value)
                    this._BloFlgIncome = value;
            }
        }
        public bool BloFlgExpend
        {
            get
            {
                return _BloFlgExpend;
            }
            set
            {
                if (this._BloFlgExpend != value)
                    this._BloFlgExpend = value;
            }
        }
        public bool BloFlgAsset
        {
            get
            {
                return _BloFlgAsset;
            }
            set
            {
                if (this._BloFlgAsset != value)
                    this._BloFlgAsset = value;
            }
        }
        public bool BloFlgActLoan
        {
            get
            {
                return _BloFlgActLoan;
            }
            set
            {
                if (this._BloFlgActLoan != value)
                    this._BloFlgActLoan = value;
            }
        }
        public bool BloFlgActGuaranty
        {
            get
            {
                return _BloFlgActGuaranty;
            }
            set
            {
                if (this._BloFlgActGuaranty != value)
                    this._BloFlgActGuaranty = value;
            }
        }
    }
}
