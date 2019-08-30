using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentAppProject;

namespace RentApp
{
    public class QualityReportRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly CleaningTasksRepository _cleaningTasksRepository;
        private readonly IErrorMessageHandler _errorHandler;

        public QualityReportRepository(HttpClientProvider clientProvider, CleaningTasksRepository cleaningTasksRepository, IErrorMessageHandler errorHandler)
        {
            _clientProvider = clientProvider;
            _cleaningTasksRepository = cleaningTasksRepository;
            _errorHandler = errorHandler;
        }

        public async Task Get(int qualityReportID, Action<QualityReport> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/QualityReports/GetPlanWithFloors/" + qualityReportID, successA: success, errorA: error);
        }

        public async Task GetMany(int locationID, Action<List<QualityReport>> success, Action error = null) {
            await _clientProvider.Client.Get("/api/QualityReports/GetForLocation/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task Create(int locationID, Action<QualityReport> success, Action error = null)
        {
            await _clientProvider.Client.Post("/api/QualityReports/Create/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task AddItem(QualityReportItem item, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/QualityReports/AddItem/" + item.QualityReportID, content: item, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task CompleteQualityRapport(int qualityReportID, DateTime nextQualityReportDate, int rating, Action success, Action error = null) 
        {
            await _clientProvider.Client.Post("/api/QualityReports/CompleteQualityReport/" + qualityReportID + "/" + rating, content: nextQualityReportDate, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }
    }
}
