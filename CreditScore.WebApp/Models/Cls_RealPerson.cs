using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_RealPerson
    {
        public AlpsCreditScoring.Models.Cls_RealPerson bktSpouse;
        public AlpsCreditScoring.Models.Cls_Job[] bktJobs = new AlpsCreditScoring.Models.Cls_Job[2];
        public AlpsCreditScoring.Models.Cls_Meta[] bktMeta = new AlpsCreditScoring.Models.Cls_Meta[2];
        public AlpsCreditScoring.Models.Cls_Person bktPerson = new AlpsCreditScoring.Models.Cls_Person();
        public Int32 BRP_MARIAGE_ID;
        public Int32 BRP_SEX_ID;
        public Int32 BRP_HOME_ID;
        public Int32 BRP_EMPLOY_ID;
        public Int32 BRP_EDU_DEGREE_ID;
        public Int32 BRP_SERVICEDUE_ID;
        public Int32 BRP_HEALTHSTATE_ID;
        public Int32 BRP_CHILDSTATE_ID;
        public Int32 BRP_SPOUSE_ID;
        public Int32 BRP_TYPE_ID;
        public Int32 BRP_RESIDENT_ID;
        public Int32 BRP_OZVIAT_ID;
        public Int32 BRP_SHOHRAT_ID;
        public Int32 BRP_NIROOU_ID;
        public Int32 BRP_CHILDREN_ID;
        public Int32 BRP_PERSONINCHARGE_ID;
        private string _BrpFirstName;
        private string _BrpLastName;
        private DateTime _BrpBirthDate;
        private string _BrpFatherName;
        private long _BrpBirthCertid;
        private long _BrpIdentificationId;
        private byte _BrpPersonIncharge;
        private byte _BrpChildren;
        private string _BrpIssuePlace;
        private string _BrpEduLastField;
        private Int16 _percent;
        private string _education;
        private string _position;
        public string BrpFirstName
        {
            get
            {
                return _BrpFirstName;
            }
            set
            {
                if (this._BrpFirstName != value)
                    this._BrpFirstName = value;
            }
        }

        public string BrpLastName
        {
            get
            {
                return _BrpLastName;
            }
            set
            {
                if (this._BrpLastName != value)
                    this._BrpLastName = value;
            }
        }

        public DateTime BrpBirthDate
        {
            get
            {
                return _BrpBirthDate;
            }
            set
            {
                if (this._BrpBirthDate != value)
                    this._BrpBirthDate = value;
            }
        }

        public string BrpFatherName
        {
            get
            {
                return _BrpFatherName;
            }
            set
            {
                if (this._BrpFatherName != value)
                    this._BrpFatherName = value;
            }
        }

        /// شماره شناسنامه
        public long BrpBirthCertid
        {
            get
            {
                return _BrpBirthCertid;
            }
            set
            {
                if (this._BrpBirthCertid != value)
                    this._BrpBirthCertid = value;
            }
        }

        public long BrpIdentificationId
        {
            get
            {
                return _BrpIdentificationId;
            }
            set
            {
                if (this._BrpIdentificationId != value)
                    this._BrpIdentificationId = value;
            }
        }

        /// تعداد افراد تحت تکفل بغير از همسر و فرزندان
        public byte BrpPersonIncharge
        {
            get
            {
                return _BrpPersonIncharge;
            }
            set
            {
                if (this._BrpPersonIncharge != value)
                    this._BrpPersonIncharge = value;
            }
        }
        public byte BrpChildren
        {
            get
            {
                return _BrpChildren;
            }
            set
            {
                if (this._BrpChildren != value)
                    this._BrpChildren = value;
            }
        }

        public string BrpIssuePlace
        {
            get
            {
                return _BrpIssuePlace;
            }
            set
            {
                if (this._BrpIssuePlace != value)
                    this._BrpIssuePlace = value;
            }
        }
        public string BrpEduLastField
        {
            get
            {
                return _BrpEduLastField;
            }
            set
            {
                if (this._BrpEduLastField != value)
                    this._BrpEduLastField = value;
            }
        }
        public Int16 Percent
        {
            get
            {
                return _percent;
            }
            set
            {
                if (this._percent != value)
                    this._percent = value;
            }
        }
        public string Education
        {
            get
            {
                return _education;
            }
            set
            {
                if (this._education != value)
                    this._education = value;
            }
        }
        public string Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (this._position != value)
                    this._position = value;
            }
        }
    }
}
