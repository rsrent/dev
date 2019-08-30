using System;
using ModuleLibraryiOS.Services;
using Microsoft.Extensions.DependencyInjection;
using RentAppProject;
using Newtonsoft.Json;
using System.Collections.Generic;
using Foundation;
using UIKit;
using System.Threading.Tasks;
using RentApp.ViewModels;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public class MyConversationSocketConnection : SocketConnection
    {
        UserVM _userVM;
        MessageHandler _messageHandler = AppDelegate.ServiceProvider.GetService<MessageHandler>();
        ChatRepository _chatRepository = AppDelegate.ServiceProvider.GetService<ChatRepository>();
        Dictionary<int, ChatVC> ActiveChats = new Dictionary<int, ChatVC>();

        ConversationsTableVC ConversationTable;

        public MyConversationSocketConnection(Settings settings, UserVM user) : base(settings.SocketUri)
        {
            _userVM = user;
        }

        public void SetConversationTable(ConversationsTableVC conversationTable) 
        {
            ConversationTable = conversationTable;
        }

        public void Listen(ChatVC chat) 
        {
            if(ActiveChats.ContainsKey(chat.Conversation.ID)) {
                return;
            }
            ActiveChats.Add(chat.Conversation.ID, chat);
        }

        public void StopListen(ChatVC chat)
        {
            ActiveChats.Remove(chat.Conversation.ID);
        }

        public async Task<bool> Post(Message message)
        {
            if(!Connected())
                await RestartConnection();
            if (!Connected())
                return false;
            
            message.UserId = _userVM.LoggedInUser().ID;
            SendMessage(JsonConvert.SerializeObject(message));
            return true;
        }

        public override void ConnectionEstablished()
        {
            Post(new RentMessage.Connect());
        }

        public override async void MessageReceived(string message)
        {
            var receivedMessage = await _messageHandler.UnfoldMessageObject(JsonConvert.DeserializeObject<RentMessage>(message, new RentMessage.MessageConverter()));

            if(receivedMessage.Type == "Connect") {
                System.Diagnostics.Debug.WriteLine("Connection confirmed");
                return;
            }

            if(ActiveChats.ContainsKey(receivedMessage.ConversationId)) {

                _chatRepository.MessageSeen(receivedMessage.ID, () => { });

                var chatVC = ActiveChats[receivedMessage.ConversationId];

                bool scrollToBottom = false;
                if (chatVC.GetTable().VisibleCells.Length > 0)
                {
                    var indexOfNewestVisible = chatVC.GetTable().IndexPathForCell(chatVC.GetTable().VisibleCells[chatVC.GetTable().VisibleCells.Length - 1]).Row;
                    scrollToBottom = (indexOfNewestVisible == (chatVC.Conversation.Messages.Count - 1));
                }

                chatVC.TableController.InsertIntoTable(new List<Message>() { receivedMessage });

                chatVC.Conversation.Messages.Add(receivedMessage);
                await Task.Delay(50);
                if (scrollToBottom) chatVC.GetTable().ScrollToRow(NSIndexPath.Create(new[] { 0, chatVC.Conversation.Messages.Count - 1 }), UITableViewScrollPosition.Bottom, false);
            }
            if(ConversationTable != null)
                ConversationTable.ReloadConversation(receivedMessage.ConversationId, receivedMessage);
        }
    }
}
