using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using Rent.Shared.Models;
using RentApp;

namespace Rent.Shared.Repositories
{
    public class LogRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;

        public LogRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
        {
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
        }

        public async Task Get(int logID, Action<LocationLog> success, Action error = null)
        => await _clientProvider.Client.Get("/api/Log/" + logID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task GetMany(int locationID, Action<ICollection<LocationLog>> success, Action error = null)
        => await _clientProvider.Client.Get("/api/Log/Many/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task Add(int locationID, Action success, Action error = null)
        => await _clientProvider.Client.Post("/api/Log/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task Update(int logID, LocationLog log, Action success, Action error = null)
        => await _clientProvider.Client.Put("/api/Log/" + logID, content: log, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
    }
}
