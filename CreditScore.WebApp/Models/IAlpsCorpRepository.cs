using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlpsCreditScoring.Service.Messages;

namespace AlpsCreditScoring.Models
{
    public interface IAlpsCorpRepository
    {
        CorpSearchResponse FindBy(CorpSearchRequest Request);
        CorpSearchResponse FindTableBy(CorpSearchRequest Request);
        CorpSearchResponse FindAll(int pageIndex, int pageSize);
        CorpSearchResponse FindByAddress(long BprID);
        CorpBaseResponse GetBase();
        CorpBaseResponse GetEditBase();
        CorpSearchResponse GetPermits(CorpSearchRequest Request);
        CorpSearchResponse DeletePermit(long BpeID);
        CorpSearchResponse InsertPermit(CorpSearchRequest request);
        CorpSearchResponse UpdatePermit(CorpSearchRequest request);
        CorpSearchResponse GetBoard(CorpSearchRequest Request);
        CorpSearchResponse DeleteBoard(long BboID);
        CorpSearchResponse UpdateBoard(CorpSearchRequest request);
        CorpSearchResponse InsertBoard(CorpSearchRequest request);
        CorpSearchResponse GetShare(CorpSearchRequest Request);
        CorpSearchResponse DeleteShare(long BshID);
        CorpSearchResponse UpdateLoan(Cls_Loan loan);
        CorpSearchResponse UpdateCorp(Cls_CorpPerson Corporate);
        CorpSearchResponse InsertOffice(Cls_Loan loan);
        CorpSearchResponse InsertFactory(Cls_Loan loan);
        CorpSearchResponse GetOffice(long BloID);
        CorpSearchResponse GetFactory(long BloID);
        CorpSearchResponse FindByCustomerID(CorpSearchRequest Request);
     }
}
