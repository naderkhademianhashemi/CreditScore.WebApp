using AlpsCreditScoring.Repository;
using AlpsCreditScoring.Service.Messages;
using AlpsCreditScoring.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CreditScore.WebApp.Members
{
    public partial class RetailSearch : System.Web.UI.Page
    {
        CreditService GetLoanService = new CreditService(new sqlAlpsRepository());
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSearch.ServerClick += new EventHandler(SearchLoan);
            var res = new RetailSearchResponse();
            var request = new RetailSearchRequest();
            res = GetLoanService.GetLoan(request);
            dgvSearch.DataSource = res.Table;
            dgvSearch.DataBind();
            txtSchNational.Attributes.Add("onkeyPress", 
                "return handleEnter('" 
                + btnSearch.ClientID + "', event)");
        }

        private void SearchLoan(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, 
                this.GetType(), 
                "Pop",
                "openModalPrint()", true);
        }
    }
}