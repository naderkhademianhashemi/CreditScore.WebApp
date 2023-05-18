using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlpsCreditScoring.Models;
using AlpsCreditScoring.Service.Messages;


namespace AlpsCreditScoring.Service
{
    public class ScoreService
    {
        private IConnect _IConnect;
        public ScoreService(IConnect connectClass)
        {
            _IConnect = connectClass;
        }
        public ScoreResponse InitFormObjects(ScoreRequest request)
        {
            request.strDataBaseName = "test";
            return _IConnect.initformObjects(request);
        }
        public ScoreResponse InitCorpFormObjects(ScoreRequest request)
        {
            request.strDataBaseName = "test";
            return _IConnect.initCorpformObjects(request);
        }
        public ScoreResponse GetModelNames()
        {
            return _IConnect.getModels();
        }
        public ScoreResponse GetPageControls(string ModelName)
        {
            return _IConnect.getPageControls(ModelName);
        }
        public ScoreResponse InsertNewCustomer(string Query)
        {
            return _IConnect.ExecuteQuery(Query);
        }
        public ScoreResponse CalculatePD(ScoreRequest request)
        {
            return _IConnect.viewPredictResult(request);
        }
        public ScoreResponse CalculateCorpPD()
        {
            return _IConnect.viewCorpPredictResult();
        }
        public bool LogPD(long PersonID, string ModelName, double PD, string query)
        {
            bool Prediction = false;
            if (PD > 0.4)
                Prediction = true;
            return _IConnect.InsertNewCustomerForDataGathering(PersonID, ModelName, Prediction, PD, query);
        }
        public ScoreResponse GetUserID(string UserName)
        {
            return _IConnect.GetUserID(UserName);
        }
    }
}