using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using AlpsCreditScoring.Repository;

namespace AlpsCreditScoring.Service.Messages
{
    public class UserResponse : ResponseBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Group { get; set; }
        public string Profile { get; set; }
        public string AccessLevel { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public int ProfileId { get; set; }
        public int SectionId { get; set; }

        public Dictionary<int, string> Users = new Dictionary<int, string>();
        public Dictionary<int, string> Groups = new Dictionary<int, string>();
        public Dictionary<int, string> Accesses = new Dictionary<int, string>();
        public DataTable Table { get; set; }
        public DataRow[] Row { get; set; }
        public DataRow[] RowParent { get; set; }
        public DataTable AddSelectColumn(DataTable table)
        {
            table.Columns.Add(new DataColumn("IsSelected", typeof(bool)));
            table.Columns["IsSelected"].ReadOnly = false;
            table.Columns["IsSelected"].DefaultValue = bool.FalseString;
            return table;
        }

    }
}