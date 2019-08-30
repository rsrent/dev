using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentAppProject;

namespace RentApp
{
    public class CompletedCleaningTasksRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;

		public CompletedCleaningTasksRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
		{
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
		}

        public async Task Create(CleaningTaskCompleted task, Action success, Action error = null) {
            await _clientProvider.Client.Post("/api/CompletedTask/", task, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task Get(CleaningTask task, Action<List<CleaningTaskCompleted>> success, Action error = null) {
            await _clientProvider.Client.Get("/api/CompletedTask/" + task.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }
    }
}
