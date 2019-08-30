using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentApp.Shared.Models.Document;
using Newtonsoft.Json;

namespace RentApp.Shared.Repositories
{
    public class DocumentRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;


        public DocumentRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
        {
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
        }

        public async Task GetForLocation(int locationID, Action<DocumentFolder> success, JsonConverter converter, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Document/GetForLocation/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage(), converter: converter);
        }

        public async Task GetForCustomer(int customerID, Action<DocumentFolder> success, JsonConverter converter, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Document/GetForCustomer/" + customerID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage(), converter: converter);
        }

        public async Task GetForFolder(int folderID, Action<DocumentFolder> success, JsonConverter converter, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Document/GetForFolder/" + folderID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage(), converter: converter);
        }

        public async Task AddFolder(int folderID, string title, Action success, Action error = null)
        {
            await _clientProvider.Client.Post("/api/Document/AddFolder/" + folderID + "/" + title, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task GetUploadSAS(string container, Action<string> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Document/GetSASTokenUpload/" + container, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task GetDownloadSAS(string container, Action<string> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Document/GetSASTokenDownload/" + container, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }
    }
}