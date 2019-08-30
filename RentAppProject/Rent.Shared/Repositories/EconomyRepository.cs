using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using Rent.Shared.Models;
using RentApp;

namespace Rent.Shared.Repositories
{
    public class EconomyRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;


        public EconomyRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
        {
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
        }

        public async Task GetForLocation(int locationID, Action<LocationEconomy> success, Action error = null)
        => await _clientProvider.Client.Get("/api/Economy/Location/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task GetForCustomer(int customerID, Action<ICollection<LocationEconomy>> success, Action error = null)
        => await _clientProvider.Client.Get("/api/Economy/Customer/" + customerID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task Update(LocationEconomy toUpdate, Action success, Action error = null)
        => await _clientProvider.Client.Put("/api/Economy/", content: toUpdate, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
    }
}
