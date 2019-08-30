using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using Newtonsoft.Json;
using RentAppProject;

namespace RentApp
{
    public class ChatRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;

        public ChatRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
        {
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
        }

        public async Task GetMessages(int conversationID, int limit, int firstMessageID, Action<ICollection<Message>> success, Action error = null, JsonConverter converter = null) {
            await _clientProvider.Client.Get("/api/Conversation/" + conversationID + "/" + limit + "/" + firstMessageID, success, error ?? _errorHandler.DisplayLoadErrorMessage(), converter: converter);
        }

        public async Task GetMessagesNewerThan(int conversationID, int messageId, Action<ICollection<Message>> success, Action error = null, JsonConverter converter = null) {
            await _clientProvider.Client.Get("/api/Conversation/MessagesNewerThan/" +conversationID+"/"+ messageId, success, error ?? _errorHandler.DisplayLoadErrorMessage(), converter: converter);
        }

        public async Task GetUserConversations(int userID, Action<ICollection<Conversation>> success, Action error = null, JsonConverter converter = null) {
            await _clientProvider.Client.Get("/api/Conversation/User/" + userID, success, error ?? _errorHandler.DisplayLoadErrorMessage(), converter: converter);
        }

        public async Task GetLocationConversations(int locationID, Action<ICollection<Conversation>> success, Action error = null, JsonConverter converter = null)
        => await _clientProvider.Client.Get("/api/Conversation/Location/" + locationID, success, error ?? _errorHandler.DisplayLoadErrorMessage(), converter: converter);

        public async Task GetConversationUsers(int conversationID, Action<ICollection<User>> success, Action error = null) {
            await _clientProvider.Client.Get("/api/Conversation/ConversationUsers/" + conversationID, success, error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task Create(string Title, ICollection<int> UserIDs, Action<Conversation> success, Action error = null) {
            await _clientProvider.Client.Post("/api/Conversation/", new { Title, UserIDs }, success, error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task AddUser(int conversationID, int userID, Action success, Action error = null) {
            await _clientProvider.Client.Put("/api/Conversation/" + conversationID + "/" + userID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task RemoveUser(int conversationID, int userID, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Conversation/RemoveUser/" + conversationID + "/" + userID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task UpdateName(int conversationID, string title, Action success, Action error = null) {
            await _clientProvider.Client.Put("/api/Conversation/UpdateTitle/" + conversationID + "/" + title, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task MessageSeen(int messageID, Action success, Action error = null) {
            await _clientProvider.Client.Put("/api/Conversation/MessageSeen/" + messageID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task NotificationsOn(int conversationID, bool on, Action success, Action error = null) {
            await _clientProvider.Client.Put("/api/Conversation/NotificationsOn/" + conversationID + "/" + on , successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }
    }
}
