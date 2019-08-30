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

            DateTime.Now.ToString("");
        }

        public async Task Get(int qualityReportID, Action<QualityReport> success, Action error = null)
        {
            await _clientProvider.Client.Get<QualityReport>("/api/QualityReports/" + qualityReportID, successA: (qp) => {

                var dic = new Dictionary<string, Dictionary<string, List<QualityReportItem>>>();

                foreach (var item in qp.QualityReportItems)
                {
                    //var plan = "Regular";
                    var floor = item.CleaningTask.Floor.Description;
                    var area = item.CleaningTask.Area.Description;

                    if (!dic.ContainsKey(floor))
                        dic.Add(floor, new Dictionary<string, List<QualityReportItem>>());
                    if (!dic[floor].Keys.Contains(area))
                        dic[floor].Add(area, new List<QualityReportItem>());
                    dic[floor][area].Add(item);
                }
                qp.Plan = dic;

                success.Invoke(qp);




                /*

            //await new HttpCall.CallManager<QualityReport>().Call(HttpCall.CallType.Get, Model.Instance().HttpUri + "QualityReports/" + qualityReportID, successA: async (qp) => {

                await _cleaningTasksRepository.Get(qp.LocationID, (cleaningPlan) =>
                {
                    if (qp.QualityReportItems == null) qp.QualityReportItems = new List<QualityReportItem>();
                    foreach (var area in cleaningPlan.TaskList)
                    {
                        if (!qp.QualityReportItems.Any(qpi => qpi.CleaningTask.ID == area.ID && qpi.CleaningTask.Comment.Equals(area.Comment)))
                        {
                            qp.QualityReportItems.Add(new QualityReportItem { CleaningTask = area, QualityReportID = qp.ID });
                        }
                    }

                    var dic = new Dictionary<string, Dictionary<string, Dictionary<string, List<QualityReportItem>>>>();

                    foreach (var item in qp.QualityReportItems)
                    {
                        var plan = item.CleaningTask.PlanType.ToString();
                        var floor = item.CleaningTask.Floor.Description;
                        var area = item.CleaningTask.Area.Description;

                        if (!dic.ContainsKey(plan))
                            dic.Add(plan, new Dictionary<string, Dictionary<string, List<QualityReportItem>>>());
                        if (!dic[plan].Keys.Contains(floor))
                            dic[plan].Add(floor, new Dictionary<string, List<QualityReportItem>>());
                        if (!dic[plan][floor].Keys.Contains(area))
                            dic[plan][floor].Add(area, new List<QualityReportItem>());
                        dic[plan][floor][area].Add(item);
                    }
                    qp.Plan = dic;

                    success.Invoke(qp);
                }, error);
                    */
            }, errorA: error);
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
