using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Reflection;
//using System.Resources;
using AlpsCreditScoring.Service.Messages;

namespace AlpsCreditScoring.Models
{
    public class FileUploadService
    {
        private IFileAvRepository _FileAvRepository;
        public FileUploadService(IFileAvRepository FileAvRepository)
        {
            _FileAvRepository = FileAvRepository;
        }
        public FileAV CheckFile(FileAV fileav)
        {
            FileAV oldFileAV;
            oldFileAV = _FileAvRepository.FindBy(fileav);
            return oldFileAV;
        }
        
        public ChkFileResponse CheckFiles(Guid userId)
        {
            ChkFileRequest request = new ChkFileRequest();
            request.UserId = userId;
            request.Results = new List<FileAV>();
            ChkFileResponse response = new ChkFileResponse();
            response = _FileAvRepository.FindAll(request);
            if (response.Count > 0)
                response.Message = Resources.localResource.ChkFileMsg02 + response.Count;
            else
                response.Message = Resources.localResource.ChkFileMsg01;
            return (response);            
        }

        public FileCreateResponse InsertandAssess(FileAV fileav)
        {
            
            return (_FileAvRepository.Add(fileav));
        }
        public AVResultsResponse GetReports(Guid fileId)
        {
            AVResultsRequest request = new AVResultsRequest();
            request.FileId = fileId;
            request.Results = new List<AntiV>();
            return (_FileAvRepository.ReportFile(request));
        }
        public AssessResponse ReAssess(Guid fileId)
        {
            AssessRequest request = new AssessRequest();
            request.FileId = fileId;
            return (_FileAvRepository.ReAssessFile(request));
        }

    }
}