using System;
using System.Threading.Tasks;

namespace BMS_Test_Client.Files
{
    public class DocumentRepository
    {
        private readonly ClientController _client;
        public DocumentRepository(ClientController client)
        {
            _client = client;
        }

        /*
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

*/

        public async Task GetUploadSAS(string container, Action<string> success, Action error = null)
        {
            var res = await _client.GetString("Document/GetSASTokenUpload/" + container);
            success(res);
            //await _clientProvider.Client.Get("/api/Document/GetSASTokenUpload/" + container, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task GetDownloadSAS(string container, Action<string> success, Action error = null)
        {
            var res = await _client.GetString("Document/GetSASTokenDownload/" + container);
            success(res);
            //await _clientProvider.Client.Get("/api/Document/GetSASTokenDownload/" + container, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }
    }
}
