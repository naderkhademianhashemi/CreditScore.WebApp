using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlpsCreditScoring.Service.Messages;

namespace AlpsCreditScoring.Models
{
    public interface IFileAvRepository
    {
        FileCreateResponse Add(FileAV file);
        FileCreateResponse Save(FileAV file);
        AVResultsResponse ReportFile(AVResultsRequest request);
        IList<FileAV> FindAll();
        ChkFileResponse FindAll(ChkFileRequest request);
        FileAV FindBy(FileAV file);

        AssessResponse ReAssessFile(AssessRequest request);
    }
}
