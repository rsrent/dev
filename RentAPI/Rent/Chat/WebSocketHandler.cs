using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;
using Rent.Repositories;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Rent.Helpers;
using Microsoft.AspNetCore.Hosting;
using Rent.PushNotifications;

namespace Rent.Chat
{
    public abstract class WebSocketHandler
    {
        protected WebSocketConnectionManager WebSocketConnectionManager { get; set; }
        //protected RentContext _rentContext { get; }
        PushNotificationController _phoneChannel;
        //NotificationRepository _notificationRepository;
        private readonly ChatRepository _chatRepository;
        protected readonly DbContextOptions<RentContext> _dbOptions;

        public WebSocketHandler(WebSocketConnectionManager webSocketConnectionManager, DbContextOptions<RentContext> dbOptions, IHostingEnvironment Environment)
        {
            WebSocketConnectionManager = webSocketConnectionManager;
            //_rentContext = new RentContext(dbOptions);
            _dbOptions = dbOptions;

            if (Startup.Live)
            {
                _phoneChannel = new PushNotificationController(new LivePhoneNotificationSettings());
            }
            else
            {
                _phoneChannel = new PushNotificationController(new DevPhoneNotificationSettings());
            }
            //_chatRepository = new ChatRepository(_rentContext);
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            WebSocketConnectionManager.AddSocket(socket);
        }

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await WebSocketConnectionManager.RemoveSocket(WebSocketConnectionManager.GetId(socket), socket);
        }

        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            try {
                if (socket.State != WebSocketState.Open)
                    return;

                var encoded = Encoding.UTF8.GetBytes(message);

                await socket.SendAsync(buffer: new ArraySegment<byte>(array: encoded,
                                                                      offset: 0,
                                                                      count: encoded.Length),
                                       messageType: WebSocketMessageType.Text,
                                       endOfMessage: true,
                                       cancellationToken: CancellationToken.None);
            } catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        /*public async Task SendMessageAsync(Int16 socketId, string message)
        {
            throw new NotImplementedException();
            //await SendMessageAsync(WebSocketConnectionManager.GetSocketsById(socketId), message);
        }*/

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in WebSocketConnectionManager.GetAll())
            {
                var socket = pair.Value;
                if (socket.State == WebSocketState.Open)
                    await SendMessageAsync(socket, message);
            }
        }



        public async Task SendMessageToConversation(MessageDTO messageSendDTO, RentContext _rentContext)
        {


            var conversation = _rentContext.Conversation.Find(messageSendDTO.ConversationID);
            if (conversation == null)
            {
                throw new Exception("Conversation not found"); //TODO: Change exception type
            } 
            var message = new Models.Message
            {
                Type = messageSendDTO.Type,
                UserID = messageSendDTO.UserID,
                ConversationID = messageSendDTO.ConversationID,
                MessageText = messageSendDTO.MessageText,
                SentTime = DateTime.Now
            };

            // TODO 

            SpecialMessage specialMessage = null;
            if (messageSendDTO.GetType() == typeof(MessageDTO.Image))
            {
                
                var dto = messageSendDTO as MessageDTO.Image;
                specialMessage = new MessageImage()
                {
                    ImageLocator = dto.ImageLocator
                };
            }/*
            else if (messageSendDTO.GetType() == typeof(MessageDTO.Video))
            {
                var dto = messageSendDTO as MessageDTO.Video;
                specialMessage = new MessageVideo()
                {
                    VideoLocator = dto.VideoLocator,
                    ThumbnailLocator = dto.ThumbnailLocator
                };
            }
            else if (messageSendDTO.GetType() == typeof(MessageDTO.Meeting))
            {
                var dto = messageSendDTO as MessageDTO.Meeting;
                specialMessage = new MessageMeeting()
                {
                    Time = dto.Time,
                    Status = dto.Status
                };
            }
            else if (messageSendDTO.GetType() == typeof(MessageDTO.Complaint))
            {
                var dto = messageSendDTO as MessageDTO.Complaint;
                specialMessage = new MessageComplaint()
                {
                    ImageLocator = dto.ImageLocator,
                    Time = dto.Time,
                    Status = dto.Status
                };
            }
            else if (messageSendDTO.GetType() == typeof(MessageDTO.MoreWork))
            {
                var dto = messageSendDTO as MessageDTO.MoreWork;
                specialMessage = new MessageMoreWork()
                {
                    Time = dto.Time,
                    Status = dto.Status
                };
            }*/

            if (specialMessage != null) {
                _rentContext.SpecialMessage.Add(specialMessage);
                await _rentContext.SaveChangesAsync();

                message.SpecialMessageID = specialMessage.ID;
            }

            _rentContext.Message.Add(message);
            _rentContext.SaveChanges();

            conversation.NewestMessageID = message.ID;
            _rentContext.Conversation.Update(conversation);
            _rentContext.SaveChanges();

            messageSendDTO.ID = message.ID;
            messageSendDTO.SentTime = message.SentTime;
            /*
            var sockets = _rentContext.ConversationUsers
                .Where(c => c.ConversationID == messageSendDTO.ConversationID)
                .Select(u => WebSocketConnectionManager.GetSocketsById(u.User.ID));
            foreach (var socket in sockets)
            {
                if (socket == null) continue;
                if (socket.State == WebSocketState.Open)
                    await SendMessageAsync(socket, JsonConvert.SerializeObject(messageSendDTO));
            }*/

            var users =  _rentContext.ConversationUsers
                        .Include(cu => cu.User)
                        .Where(cu => cu.ConversationID == messageSendDTO.ConversationID);
            var conversationUsers =  users.ToList();

            //messageSendDTO.MessageText += "" + JsonConvert.SerializeObject(notificationUsers) + " | " + _phoneChannel.ConnectionString();

            foreach (var user in conversationUsers)
            {
                if (user.NotificationsOn) {
					_phoneChannel.Send(user.UserID, "Ny besked i " + conversation.Title, messageSendDTO.MessageText);
                }

                var socket = WebSocketConnectionManager.GetSocketsById(user.UserID);
                if (socket == null) continue;
                if (socket.State == WebSocketState.Open)
                {
                    await SendMessageAsync(socket, JsonConvert.SerializeObject(messageSendDTO));
                }

                //user.LastSeenMessageID = message.ID;
            }
            /*
            var connectedConversationUsers = 
                _rentContext.ConversationUsers
                .Where(cu => cu.ConversationID == message.ConversationID 
                && (cu.UserID == message.UserID 
                || connectedUser.Any(u => u.Item2 == cu.UserID && u.Item1 != null &&u.Item1.State == WebSocketState.Open))); */

            /*
            _rentContext.ConversationUsers.UpdateRange(conversationUsers);
            _rentContext.SaveChanges(); */

        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
