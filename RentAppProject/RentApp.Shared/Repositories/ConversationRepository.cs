using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentAppProject;

namespace RentApp.Shared.Repositories
{
    public class ConversationRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;

        public ConversationRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
        {
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
        }

        public async Task GetAll(Action<List<Conversation>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Customers/", successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }
    }
}
