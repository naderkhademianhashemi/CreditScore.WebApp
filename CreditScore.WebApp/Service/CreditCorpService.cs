using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;
using AlpsCreditScoring.Service.Messages;


namespace AlpsCreditScoring.Service
{
    public class CreditCorpService
    {
        private IAlpsCorpRepository _AlpsCorpRepository;
        public CreditCorpService(IAlpsCorpRepository AlpsCorpRepository)
        {
            _AlpsCorpRepository = AlpsCorpRepository;
        }
        public CorpSearchResponse GetLoanSelect(CorpSearchRequest request)
        {
            return _AlpsCorpRepository.FindTableBy(request);
        }
        public CorpSearchResponse GetLoan(CorpSearchRequest request)
        {
            return _AlpsCorpRepository.FindAll(request.pageIndex, request.pageSize);
        }
        public CorpBaseResponse FillBaseData()
        {
            return _AlpsCorpRepository.GetBase();
        }
        public CorpBaseResponse FillEditBaseData()
        {
            return _AlpsCorpRepository.GetEditBase();
        }
        public CorpSearchResponse GetLoanEdit(CorpSearchRequest request)
        {
            return _AlpsCorpRepository.FindBy(request);
        }
        public CorpSearchResponse GetPermits(CorpSearchRequest request)
        {
            return _AlpsCorpRepository.GetPermits(request);
        }
        public CorpSearchResponse UpdatePermits(CorpSearchRequest request)
        {
            return _AlpsCorpRepository.UpdatePermit(request);
        }
        public CorpSearchResponse InsertPermits(CorpSearchRequest request)
        {
            return _AlpsCorpRepository.InsertPermit(request);
        }
        public CorpSearchResponse DeletePermit(long BpeID)
        {
            return _AlpsCorpRepository.DeletePermit(BpeID);
        }
        public CorpSearchResponse GetBoard(CorpSearchRequest request)
        {
            return _AlpsCorpRepository.GetBoard(request);
        }
        public CorpSearchResponse DeleteBoard(long BboID)
        {
            return _AlpsCorpRepository.DeleteBoard(BboID);
        }
        public CorpSearchResponse UpdateBoards(CorpSearchRequest request)
        {
            return _AlpsCorpRepository.UpdateBoard(request);
        }
        public CorpSearchResponse InsertBoard(CorpSearchRequest request)
        {
            return _AlpsCorpRepository.InsertBoard(request);
        }
        public CorpSearchResponse GetShare(CorpSearchRequest request)
        {
            return _AlpsCorpRepository.GetShare(request);
        }
        public CorpSearchResponse DeleteShare(long BshID)
        {
            return _AlpsCorpRepository.DeleteShare(BshID);
        }
        public CorpSearchResponse UpdateLoan(Cls_Loan loan)
        {
            return _AlpsCorpRepository.UpdateLoan(loan);
        }
        public CorpSearchResponse UpdateCorporate(Cls_CorpPerson Corporate)
        {
            return _AlpsCorpRepository.UpdateCorp(Corporate);
        }
        public CorpSearchResponse ChangeOffice(Cls_Loan loan)
        {
            return _AlpsCorpRepository.InsertOffice(loan);
        }
        public CorpSearchResponse ChangeFactory(Cls_Loan loan)
        {
            return _AlpsCorpRepository.InsertFactory(loan);
        }
        public CorpSearchResponse GetOffice(long BloId)
        {
            return _AlpsCorpRepository.GetOffice(BloId);
        }
        public CorpSearchResponse GetFactory(long BloId)
        {
            return _AlpsCorpRepository.GetFactory(BloId);
        }
        public CorpSearchResponse GetAddressEdit(long BprID)
        {
            return _AlpsCorpRepository.FindByAddress(BprID);
        }
        public CorpSearchResponse AllCorpLoans(CorpSearchRequest request)
        {
            return _AlpsCorpRepository.FindByCustomerID(request);
        }
    }
}