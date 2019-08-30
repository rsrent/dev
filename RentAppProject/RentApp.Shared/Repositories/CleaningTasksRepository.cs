using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentApp.Shared.Models;
using RentAppProject;

namespace RentApp
{
    public class CleaningTasksRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;

		public CleaningTasksRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
		{
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
		}

        /*
        public async Task Get(int locationID, Action<LocationCleaningSchedule> success, Action error = null)
		{
            await _clientProvider.Client.Get<IEnumerable<CleaningTask>>("/api/CleaningTask/Get/" + locationID, (tasks) =>
			{
				LocationCleaningSchedule plan = new LocationCleaningSchedule();
                plan.TaskDictionary = new Dictionary<string, Dictionary<string, Dictionary<string, List<CleaningTask>>>>();
                plan.TaskList = tasks.ToList();

				foreach (var item in tasks)
				{
                    var planType = item.CleaningPlan.Description.ToString();
					var floor = item.Floor.Description;
					var area = item.Area.Description;

					if (!plan.TaskDictionary.ContainsKey(planType))
						plan.TaskDictionary.Add(planType, new Dictionary<string, Dictionary<string, List<CleaningTask>>>());
					if (!plan.TaskDictionary[planType].Keys.Contains(floor))
						plan.TaskDictionary[planType].Add(floor, new Dictionary<string, List<CleaningTask>>());
					if (!plan.TaskDictionary[planType][floor].Keys.Contains(area))
						plan.TaskDictionary[planType][floor].Add(area, new List<CleaningTask>());
					plan.TaskDictionary[planType][floor][area].Add(item);
				}
                //plan.TaskDictionary = dic;
				success.Invoke(plan);

			}, error ?? _errorHandler.DisplayLoadErrorMessage());
		} */

        public async Task Get(int locationID, Action<List<CleaningSchedule.SchedulePlan>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/CleaningTask/Plans/" + locationID, success, error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        /*
        public async Task GetDictionary(int locationID, Action<Dictionary<CleaningPlan, Dictionary<Floor, Dictionary<Area, List<CleaningTask>>>>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/CleaningTask/GetDictionary/" + locationID, success, error ?? _errorHandler.DisplayLoadErrorMessage());
        } */

		public async Task Add(int locationID, CleaningTask task, Action<CleaningTask> success, Action error = null)
		{
            await _clientProvider.Client.Post("/api/CleaningTask/Add/" + locationID, content: task, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
		}

        public async Task Update(CleaningTask task, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/CleaningTask/", content: task, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

		public async Task Delete(CleaningTask task, Action success, Action error = null)
		{
            await _clientProvider.Client.Delete("/api/CleaningTask/Delete/" + task.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
		}
    }
}
