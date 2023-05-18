using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Statement
    {
        public long BstId;
        public AlpsCreditScoring.Models.Cls_StatementItem[] bktStatementItem;
        public long BprId;

        private DateTime _BstStatementDate;
        private DateTime _BstCreateDate;
        private byte _BstStatus;

        public DateTime BstStatementDate
        {
            get
            {
                return _BstStatementDate;
            }
            set
            {
                if (this._BstStatementDate != value)
                    this._BstStatementDate = value;
            }
        }

        public DateTime BstCreateDate
        {
            get
            {
                return _BstCreateDate;
            }
            set
            {
                if (this._BstCreateDate != value)
                    this._BstCreateDate = value;
            }
        }

        public byte BstStatus
        {
            get
            {
                return _BstStatus;
            }
            set
            {
                if (this._BstStatus != value)
                    this._BstStatus = value;
            }
        }

    }
}
