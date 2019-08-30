using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using Rent.Shared.Models;
using RentApp;

namespace Rent.Shared.Repositories
{
    public class HoursRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;

        public HoursRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
        {
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
        }

        public async Task Get(int locationID, Action<LocationHours> success, Action error = null)
        => await _clientProvider.Client.Get("/api/Hour/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task GetForCustomer(int customerID, Action<ICollection<LocationHours>> success, Action error = null)
        => await _clientProvider.Client.Get("/api/Hour/Customer/" + customerID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task Update(LocationHours toUpdate, Action success, Action error = null)
        => await _clientProvider.Client.Put("/api/Hour/", content: toUpdate, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
    }
}
