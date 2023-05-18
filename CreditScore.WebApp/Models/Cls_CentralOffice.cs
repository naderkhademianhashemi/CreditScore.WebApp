using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_CentralOffice
    {
        public long BCO_BPR_ID;
        public Int32 BCO_SANADSTATUS_ID;
        public Int32 BCO_PROPSTATUS_ID;
        public Int32 BCO_TYPE_ID;
        public long BCO_BBN_ID;
        private long _BcoId;
        public Int32 BBD_ADDRESS; //Type ID for address
        public Int32 BBD_PHONE;   //Type ID for phone
        public Int32 BBD_FAX;     //Type ID for fax
        public Int32 BBD_CODEPOSTAL;    //Type ID for codepostal
        public string ADDRESS;
        public string PHONE;
        public string FAX;
        public string CODEPOSTAL;
        public long BAP_ADDRESS_ID;
        public long BAP_PHONE_ID;
        public long BAP_FAX_ID;
        public long BAP_CODEPOSTAL_ID;
        private double _BcoValue;
        private decimal _BcoSurface;
        private decimal _BcoArena;
        private decimal _BcoStdproperty;
        private int _BcoPersonal;
        private string _BcoRahnBank;
        private int _BcoRahnDuration;
        private double _BcoRahnAmount;
        private DateTime _BcoRahnDate;
        private double _BcoRent;
        private double _BcoMachinery;
        public long BcoId
        {
            get
            {
                return _BcoId;
            }
            set
            {
                if (this._BcoId != value)
                    this._BcoId = value;
            }
        }

        public double BcoValue
        {
            get
            {
                return _BcoValue;
            }
            set
            {
                if (this._BcoValue != value)
                    this._BcoValue = value;
            }
        }

        public decimal BcoSurface
        {
            get
            {
                return _BcoSurface;
            }
            set
            {
                if (this._BcoSurface != value)
                    this._BcoSurface = value;
            }
        }

        /// Arse
        public decimal BcoArena
        {
            get
            {
                return _BcoArena;
            }
            set
            {
                if (this._BcoArena != value)
                    this._BcoArena = value;
            }
        }

        /// اعيان
        public decimal BcoStdproperty
        {
            get
            {
                return _BcoStdproperty;
            }
            set
            {
                if (this._BcoStdproperty != value)
                    this._BcoStdproperty = value;
            }
        }

        public int BcoPersonal
        {
            get
            {
                return _BcoPersonal;
            }
            set
            {
                if (this._BcoPersonal != value)
                    this._BcoPersonal = value;
            }
        }

        public string BcoRahnBank
        {
            get
            {
                return _BcoRahnBank;
            }
            set
            {
                if (this._BcoRahnBank != value)
                    this._BcoRahnBank = value;
            }
        }

        public int BcoRahnDuration
        {
            get
            {
                return _BcoRahnDuration;
            }
            set
            {
                if (this._BcoRahnDuration != value)
                    this._BcoRahnDuration = value;
            }
        }

        public double BcoRahnAmount
        {
            get
            {
                return _BcoRahnAmount;
            }
            set
            {
                if (this._BcoRahnAmount != value)
                    this._BcoRahnAmount = value;
            }
        }

        public DateTime BcoRahnDate
        {
            get
            {
                return _BcoRahnDate;
            }
            set
            {
                if (this._BcoRahnDate != value)
                    this._BcoRahnDate = value;
            }
        }

        public double BcoRent
        {
            get
            {
                return _BcoRent;
            }
            set
            {
                if (this._BcoRent != value)
                    this._BcoRent = value;
            }
        }
        public double BcoMachinery
        {
            get
            {
                return _BcoMachinery;
            }
            set
            {
                if (this._BcoMachinery != value)
                    this._BcoMachinery = value;
            }
        }

    }
}
