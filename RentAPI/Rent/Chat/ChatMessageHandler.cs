using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rent.Data;
using Rent.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Rent.Helpers;
using Microsoft.AspNetCore.Hosting;
using Rent.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Rent.Chat
{
    public class ChatMessageHandler : WebSocketHandler
    {
        public ChatMessageHandler(WebSocketConnectionManager webSocketConnectionManager, DbContextOptions<RentContext> dbOptions, IHostingEnvironment Environment) : base(webSocketConnectionManager, dbOptions, Environment)
        {
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            //var socketId = WebSocketConnectionManager.GetId(socket);
            //await SendMessageToAllAsync($"{socketId} is now connected");
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var _rentContext = new RentContext(_dbOptions);

            try
            {
                var socketId = WebSocketConnectionManager.GetId(socket);
                var messageText = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var receivedJSON = JsonConvert.DeserializeObject<MessageDTO>(messageText, new MessageDTO.MessageConverter());
                if (receivedJSON == null)
                {
                    throw new NotImplementedException("Unknown message type");
                }
                else if (receivedJSON.GetType() == typeof(MessageDTO.Connect))
                {
                    //var conversation = _rentContext.Conversation.Find(receivedJSON.ConversationID);
                    var conversationUsers = _rentContext.ConversationUsers.Where(cu => cu.UserID == receivedJSON.UserID);
                    string serializedMessage;
                    /*
                    if (conversation == null)
                    {
                        serializedMessage = JsonConvert.SerializeObject(new ConversationConnectFailureDTO
                        {
                            Text = "Could not find a conversation with the given ID",
                            UserID = receivedJSON.UserID,
                            ConversationID = receivedJSON.ConversationID
                        });
                    }
                    else */
                    if (conversationUsers == null)
                    {
                        serializedMessage = JsonConvert.SerializeObject(new ConversationConnectFailureDTO
                        {
                            Text = "The given user ID does not belong to any conversations",
                            UserID = receivedJSON.UserID,
                            ConversationID = receivedJSON.ConversationID
                        });
                    }
                    else
                    {
                        if(socketId != receivedJSON.UserID)
                            WebSocketConnectionManager.UpdateID(socketId, receivedJSON.UserID);
                        serializedMessage = JsonConvert.SerializeObject(new ConversationConnectedSuccessDTO
                        {
                            ConversationID = receivedJSON.ConversationID
                        });
                    }
                    await SendMessageAsync(socket, serializedMessage);
                }
                else
                {
                    await SendMessageToConversation(receivedJSON, _rentContext);
                }
            } catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
    }
}
