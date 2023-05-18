using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AlpsCreditScoring.Service.Messages;

namespace AlpsCreditScoring.Models
{
    public interface IConnect
    {
        string CaseTableName { get; set; }
        string MiningStructNameHoghooghy { get; set; }
        string MiningStructNameHaghighy { get; set; }
        string MiningStructName { get; set; }
        string DataBaseName { get; set; }
        string UserTableName { get; set; }
        string CorpUserTableName { get; set; }
        
        ScoreResponse ConnectAnalysisServices(ScoreRequest request);
        DataTable getTableFields(string tbName, string strDSV);
        ScoreResponse initformObjects(ScoreRequest request);
        ScoreResponse initCorpformObjects(ScoreRequest request);
        bool InsertNewCustomerForDataGathering(long PersonID, string ModelName, bool Prediction, double PD, string query);
        void creatTempTable();
        void creatCorpTempTable();
        ScoreResponse viewPredictResult(ScoreRequest request);
        ScoreResponse viewCorpPredictResult();
        bool packPredictResult(string packTable);
        ScoreResponse getModels();
        //ScoreResponse getBaseCode(ScoreRequest request);
        ScoreResponse getPageControls(string ModelName);
        ScoreResponse ExecuteQuery(string Query);
        ScoreResponse GetUserID(string UserName);
    }
}
