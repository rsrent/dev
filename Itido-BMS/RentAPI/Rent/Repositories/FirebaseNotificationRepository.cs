using System;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Rent.Data;
using Rent.Models.TimePlanning;
using System.Linq;
using System.Collections.Generic;

namespace Rent.Repositories
{
    public class FirebaseNotificationRepository
    {
        private readonly RentContext _context;

        public FirebaseNotificationRepository(RentContext context)
        {
            if (FirebaseApp.DefaultInstance == null)
                FirebaseApp.Create();

            this._context = context;
        }

        public async Task PushNotificationTo(ICollection<string> topics, string title, string body)
        {
            var messages = new List<Message>();
            topics.ToList().ForEach((topic) => messages.Add(CreateMessage(topic, title, body)));
            while (messages.Count > 0)
            {
                var messagesToSend = messages.GetRange(0, Math.Min(100, messages.Count));
                //messagesToSend.ForEach((obj) => Console.WriteLine("Message: " + obj.Topic));
                await FirebaseMessaging.DefaultInstance.SendAllAsync(messagesToSend);
                messages.RemoveAll((m) => messagesToSend.Contains(m));
            }
        }

        public async Task PushNotificationTo(string topic, string title, string body)
        {
            var message = CreateMessage(topic, title, body);
            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        public Message CreateMessage(string topic, string title, string body)
        {
            return new Message
            {
                Topic = topic,
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                },
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        Title = title,
                        Body = body,
                        Sound = "default",
                    }
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Sound = "default",
                    }
                },
            };
        }

        public async Task PushNotificationFromNoti(Noti noti)
        {
            await PushNotificationTo("user_" + noti.ReceiverID.ToString(), noti.Title, noti.Body);
        }

        public async Task PushNotificationFromNotis(ICollection<Noti> notis)
        {
            var messages = new List<Message>();

            notis.ToList().ForEach((noti) => messages.Add(CreateMessage("user_" + noti.ReceiverID.ToString(), noti.Title, noti.Body)));

            while (messages.Count > 0)
            {
                Console.WriteLine("Start count: " + messages.Count);
                var messagesToSend = messages.GetRange(0, Math.Min(100, messages.Count));

                messagesToSend.ForEach((obj) => Console.WriteLine("Message: " + obj.Topic));

                await FirebaseMessaging.DefaultInstance.SendAllAsync(messagesToSend);

                messages.RemoveAll((m) => messagesToSend.Contains(m));

                Console.WriteLine("End count: " + messages.Count);
            }


            //await PushNotificationTo("user_" + noti.ReceiverID.ToString(), noti.Title, noti.Body);
        }

    }
}
