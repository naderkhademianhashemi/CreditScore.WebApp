using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;
using AlpsCreditScoring.Service.Messages;
using CreditScore.WebApp;


namespace AlpsCreditScoring.Service
{
    public class CreditService
    {
        private IAlpsRepository _AlpsRepository;
        public CreditService(IAlpsRepository AlpsRepository)
        {
            _AlpsRepository = AlpsRepository;
        }
        public RetailSearchResponse GetLoanSelect(RetailSearchRequest request)
        {
            return _AlpsRepository.FindTableBy(request);
        }
        public RetailSearchResponse GetLoan(RetailSearchRequest request)
        {
            return _AlpsRepository.FindAll(request.pageIndex, request.pageSize);
        }

        public RetailBaseResponse FillBaseData()
        {
            return _AlpsRepository.GetBase();
        }
        public RetailBaseResponse FillEditBaseData()
        {
            return _AlpsRepository.GetBaseEdit();
        }
        public RetailSearchResponse GetLoanEdit(RetailSearchRequest request)
        {
            return _AlpsRepository.FindBy(request);
        }
        public RetailSearchResponse GetSpouseEdit(long BprID)
        {
            return _AlpsRepository.FindBySpouse(BprID);
        }
        public RetailSearchResponse GetJobEdit(long BprID)
        {
            return _AlpsRepository.FindByJob(BprID);
        }
        public RetailSearchResponse GetAddressEdit(long BprID)
        {
            return _AlpsRepository.FindByAddress(BprID);
        }
        public RetailSearchResponse GetActualLoans(RetailSearchRequest request)
        {
            return _AlpsRepository.GetClientFacility(request);
        }
        public RetailSearchResponse GetAssets(RetailSearchRequest request)
        {
            return _AlpsRepository.GetClientAssets(request);
        }
        public RetailSearchResponse GetAccounts(RetailSearchRequest request)
        {
            return _AlpsRepository.GetClientAccounts(request);
        }
        public RetailSearchResponse GetGuaranty(RetailSearchRequest request)
        {
            return _AlpsRepository.GetClientGuaranty(request);
        }
        public RetailSearchResponse GetCosts(RetailSearchRequest request)
        {
            return _AlpsRepository.GetClientCost(request);
        }
        public RetailSearchResponse GetIncome(RetailSearchRequest request)
        {
            return _AlpsRepository.GetClientIncome(request);
        }
        public RetailSearchResponse InsertFacilityLoan(RetailSearchRequest request)
        {
            return _AlpsRepository.InsertActualLoans(request);
        }
        public RetailSearchResponse UpdateFacilityLoan(RetailSearchRequest request)
        {
            return _AlpsRepository.UpdateActualLoans(request);
        }
        public RetailSearchResponse DeleteFacilityLoan(long BriID)
        {
            return _AlpsRepository.DeleteActualLoans(BriID);
        }
        public RetailSearchResponse InsertIncome(RetailSearchRequest request)
        {
            return _AlpsRepository.InsertIncome(request);
        }
        public RetailSearchResponse InsertExpenses(RetailSearchRequest request)
        {
            return _AlpsRepository.InsertExpenses(request);
        }
        public RetailSearchResponse UpdateExpenses(RetailSearchRequest request)
        {
            return _AlpsRepository.UpdateExpenses(request);
        }
        public RetailSearchResponse DeleteExpend(long BexID)
        {
            return _AlpsRepository.DeleteExpenses(BexID);
        }
        public RetailSearchResponse UpdateIncome(RetailSearchRequest request)
        {
            return _AlpsRepository.UpdateIncome(request);
        }
        public RetailSearchResponse DeleteIncome(long BinID)
        {
            return _AlpsRepository.DeleteIncome(BinID);
        }
        public RetailSearchResponse UpdateAsset(RetailSearchRequest request)
        {
            return _AlpsRepository.UpdateAsset(request);
        }
        public RetailSearchResponse DeleteAsset(long BssID)
        {
            return _AlpsRepository.DeleteAsset(BssID);
        }
        public RetailSearchResponse InsertAssets(RetailSearchRequest request)
        {
            return _AlpsRepository.InsertAssets(request);
        }
        public RetailSearchResponse InsertGuaranty(RetailSearchRequest request)
        {
            return _AlpsRepository.InsertGuaranty(request);
        }
        public RetailSearchResponse UpdateGuaranty(RetailSearchRequest request)
        {
            return _AlpsRepository.UpdateGuaranty(request);
        }
        public RetailSearchResponse InsertAccount(RetailSearchRequest request)
        {
            return _AlpsRepository.InsertAccount(request);
        }
       
        public RetailSearchResponse UpdateAccount(RetailSearchRequest request)
        {
            Global g = new Global();
            request.OpeningDate = g.GetGregorian(request.OpeningDate);
            return _AlpsRepository.UpdateAccount(request);
        }
        public RetailSearchResponse DeleteAccount(long BkaID)
        {
            return _AlpsRepository.DeleteAccount(BkaID);
        }
        public RetailSearchResponse InsertPerson(string BprCode)
        {
            return _AlpsRepository.InsertPerson(BprCode);
        }
        public RetailSearchResponse InsertSpouse(Cls_Loan loan)
        {
            return _AlpsRepository.InsertRetailSpouse(loan);
        }
        public RetailSearchResponse InsertAdditional(Cls_Meta meta)
        {
            return _AlpsRepository.InsertAdditioanlData(meta);
        }
        public RetailSearchResponse InsertJob(Cls_Job job, long BprID)
        {
            return _AlpsRepository.InsertJob(job, BprID);
        }
        public RetailSearchResponse UpdateFlagSpouse(long BprID)
        {
            return _AlpsRepository.UpdateFlag(1, BprID);
        }
        public RetailSearchResponse UpdateLoan(Cls_Loan loan)
        {
            return _AlpsRepository.UpdateLoan(loan);
        }
        public RetailSearchResponse UpdateRetail(Cls_RealPerson Customer)
        {
            return _AlpsRepository.UpdateRetail(Customer);
        }
        public RetailSearchResponse InsertJudg(RetailSearchRequest request)
        {
            return _AlpsRepository.InsertJudg(request);
        }
        public RetailSearchResponse ChangeAddress(Cls_Address address, long BprID)
        {
            return _AlpsRepository.InsertAddress(address, BprID);
        }
        public RetailSearchResponse UpdatePersonCode(Cls_Person person)
        {
            return _AlpsRepository.UpdatePersonCode(person);
        }
        public RetailSearchResponse UpdateFlagFactory(long BloID)
        {
            return _AlpsRepository.UpdateFlag(21, BloID);
        }
        public RetailSearchResponse AllCustomerLoans(RetailSearchRequest request)
        {
            return _AlpsRepository.FindByCustomerID(request);
        }
    }
}