using System;
using System.Collections.Generic;
using System.Text;

namespace AlpsCreditScoring.Models
{
    public class Cls_Balance
    {
       public long BblId;
       public AlpsCreditScoring.Models.Cls_BalanceItem[] bktBalanceItem;
       public long BprId;

   private DateTime _BblBalanceDate;
   private DateTime _BblCreateDate;
   private byte _BblStatus;

   public DateTime BblBalanceDate
   {
      get
      {
         return _BblBalanceDate;
      }
      set
      {
         if (this._BblBalanceDate != value)
            this._BblBalanceDate = value;
      }
   }
   public DateTime BblCreateDate
   {
      get
      {
         return _BblCreateDate;
      }
      set
      {
         if (this._BblCreateDate != value)
            this._BblCreateDate = value;
      }
   }
   public byte BblStatus
   {
        get
         {
            return _BblStatus;
         }
        set
        {
         if (this._BblStatus != value)
            this._BblStatus = value;
        }
   }
   }
}
