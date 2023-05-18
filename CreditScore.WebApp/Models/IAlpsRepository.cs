using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlpsCreditScoring.Service.Messages;

namespace AlpsCreditScoring.Models
{
    public interface IAlpsRepository
    {
        RetailSearchResponse FindBy(RetailSearchRequest Request);
        RetailSearchResponse FindTableBy(RetailSearchRequest Request);
        RetailSearchResponse FindAll(int pageIndex, int pageSize);
        RetailBaseResponse GetBase();
        RetailBaseResponse GetBaseEdit();
        RetailSearchResponse GetClientFacility(RetailSearchRequest Request);
        RetailSearchResponse GetClientAssets(RetailSearchRequest Request);
        RetailSearchResponse GetClientAccounts(RetailSearchRequest Request);
        RetailSearchResponse GetClientGuaranty(RetailSearchRequest Request);
        RetailSearchResponse GetClientCost(RetailSearchRequest Request);
        RetailSearchResponse GetClientIncome(RetailSearchRequest Request);
        UserResponse BaseGroup();
        UserResponse BaseAccesses();
        UserResponse InsertUser(UserRequest request);
        UserResponse Lookup(string query);
        UserResponse DeleteUser(string UserName);
        UserResponse InsertProfile(UserRequest request);
        UserResponse UpdateProfile(UserRequest request);
        UserResponse UpdateGroup(UserRequest request);
        UserResponse GroupAccess(UserRequest request);
        UserResponse DeleteGroup(int GroupID);
        UserResponse InsertGroup(UserRequest request);
        RetailSearchResponse InsertActualLoans(RetailSearchRequest request);
        RetailSearchResponse UpdateActualLoans(RetailSearchRequest request);
        RetailSearchResponse DeleteActualLoans(long BriID);
        RetailSearchResponse InsertIncome(RetailSearchRequest request);
        RetailSearchResponse InsertExpenses(RetailSearchRequest request);
        RetailSearchResponse UpdateExpenses(RetailSearchRequest request);
        RetailSearchResponse DeleteExpenses(long BexID);
        RetailSearchResponse UpdateIncome(RetailSearchRequest request);
        RetailSearchResponse DeleteIncome(long BinID);
        RetailSearchResponse UpdateAsset(RetailSearchRequest request);
        RetailSearchResponse DeleteAsset(long BssID);
        RetailSearchResponse InsertAssets(RetailSearchRequest request);
        RetailSearchResponse UpdateGuaranty(RetailSearchRequest request);
        RetailSearchResponse InsertGuaranty(RetailSearchRequest request);
        RetailSearchResponse InsertAccount(RetailSearchRequest request);
        RetailSearchResponse UpdateAccount(RetailSearchRequest request);
        RetailSearchResponse DeleteAccount(long BkaID);
        RetailSearchResponse FindBySpouse(long BprID);
        RetailSearchResponse FindByJob(long BprID);
        RetailSearchResponse FindByAddress(long BprID);
        RetailSearchResponse InsertPerson(string BprCode);
        RetailSearchResponse InsertRetailSpouse(Cls_Loan loan);
        RetailSearchResponse InsertAdditioanlData(Cls_Meta meta);
        RetailSearchResponse InsertJob(Cls_Job job, long BprID);
        RetailSearchResponse UpdateFlag(int What, long BloID);
        RetailSearchResponse UpdateLoan(Cls_Loan loan);
        RetailSearchResponse UpdateRetail(Cls_RealPerson Customer);
        RetailSearchResponse InsertJudg(RetailSearchRequest request);
        RetailSearchResponse InsertAddress(Cls_Address Address, Int64 BprID);
        RetailSearchResponse UpdatePersonCode(Cls_Person Person);
        RetailSearchResponse FindByCustomerID(RetailSearchRequest Request);
        
    }
}
