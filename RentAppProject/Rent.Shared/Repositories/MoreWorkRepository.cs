using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using Rent.Shared.Models;
using RentApp;

namespace Rent.Shared.Repositories
{
    public class MoreWorkRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;

        public MoreWorkRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
        {
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
        }

        public async Task Get(int locationID, Action<ICollection<MoreWork>> success, Action error = null)
        => await _clientProvider.Client.Get("/api/MoreWork/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task Add(MoreWork morework, Action<MoreWork> success, Action error = null)
        => await _clientProvider.Client.Post("/api/MoreWork/", content: morework, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task Complete(int moreworkID, float hours, Action success, Action error = null)
        => await _clientProvider.Client.Put("/api/MoreWork/" + moreworkID + "/" + hours, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
    }
}
