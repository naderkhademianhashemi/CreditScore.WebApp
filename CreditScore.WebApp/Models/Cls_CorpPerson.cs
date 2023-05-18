using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_CorpPerson
    {
        public double BcpCapitalKickoff;

        public AlpsCreditScoring.Models.Cls_Permit[] bktPermit;
        public AlpsCreditScoring.Models.Cls_CentralOffice bktCentralOffice;
        public AlpsCreditScoring.Models.Cls_CentralOffice bktFactory;
        public AlpsCreditScoring.Models.Cls_RealPerson[] bktBoard;
        public AlpsCreditScoring.Models.Cls_RealPerson[] bktShareHolder;
        public AlpsCreditScoring.Models.Cls_Balance[] bktBalance;
        public AlpsCreditScoring.Models.Cls_Statement[] bktStatement;
        public AlpsCreditScoring.Models.Cls_Person bktPerson;
        public Int32 BCP_TYPE_ID;
        public Int32 BCP_ACTIVITY_ID;
        public long BLO_ID;
        private string _BcpName;
        private string _BcpRegisterNo;
        private long _BcpIdentificationID;
        private string _BcpRegisterPlace;
        private DateTime _BcpEstablishDate;
        private double _BcpCapitalActual;
        private bool _BcpInBourse;
        private DateTime _BcpInbourseDate;
        private short _BcpBranchesNbr;
        private int _BcpStaffNbr;
        private byte _BcpStatus;
        private DateTime _BcpStatDate;
        private string _type;
        private string _activity;
        public string BcpName
        {
            get
            {
                return _BcpName;
            }
            set
            {
                if (this._BcpName != value)
                    this._BcpName = value;
            }
        }
        public string BcpRegisterNo
        {
            get
            {
                return _BcpRegisterNo;
            }
            set
            {
                if (this._BcpRegisterNo != value)
                    this._BcpRegisterNo = value;
            }
        }
        public long BcpIdentificationID
        {
            get
            {
                return _BcpIdentificationID;
            }
            set
            {
                if (this._BcpIdentificationID != value)
                    this._BcpIdentificationID = value;
            }
        }
        public string BcpRegisterPlace
        {
            get
            {
                return _BcpRegisterPlace;
            }
            set
            {
                if (this._BcpRegisterPlace != value)
                    this._BcpRegisterPlace = value;
            }
        }

        public DateTime BcpEstablishDate
        {
            get
            {
                return _BcpEstablishDate;
            }
            set
            {
                if (this._BcpEstablishDate != value)
                    this._BcpEstablishDate = value;
            }
        }
        public double BcpCapitalActual
        {
            get
            {
                return _BcpCapitalActual;
            }
            set
            {
                if (this._BcpCapitalActual != value)
                    this._BcpCapitalActual = value;
            }
        }

        public bool BcpInBourse
        {
            get
            {
                return _BcpInBourse;
            }
            set
            {
                if (this._BcpInBourse != value)
                    this._BcpInBourse = value;
            }
        }
        public DateTime BcpInbourseDate
        {
            get
            {
                return _BcpInbourseDate;
            }
            set
            {
                if (this._BcpInbourseDate != value)
                    this._BcpInbourseDate = value;
            }
        }
        public short BcpBranchesNbr
        {
            get
            {
                return _BcpBranchesNbr;
            }
            set
            {
                if (this._BcpBranchesNbr != value)
                    this._BcpBranchesNbr = value;
            }
        }
        public int BcpStaffNbr
        {
            get
            {
                return _BcpStaffNbr;
            }
            set
            {
                if (this._BcpStaffNbr != value)
                    this._BcpStaffNbr = value;
            }
        }

        public byte BcpStatus
        {
            get
            {
                return _BcpStatus;
            }
            set
            {
                if (this._BcpStatus != value)
                    this._BcpStatus = value;
            }
        }

        public DateTime BcpStatDate
        {
            get
            {
                return _BcpStatDate;
            }
            set
            {
                if (this._BcpStatDate != value)
                    this._BcpStatDate = value;
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
        public string Activity
        {
            get
            {
                return _activity;
            }
            set
            {
                if (this._activity != value)
                    this._activity = value;
            }
        }
    }
}
