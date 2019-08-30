using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using ModuleLibraryiOS.Storage;
using RentApp.Repository;
using RentAppProject;

namespace RentApp
{
    public class MessageHandler
    {
        StorageRepository _storage;

        public MessageHandler(StorageRepository storage)
        {
            _storage = storage;
        }

        public async Task<ICollection<Message>> UnfoldMessages(ICollection<Message> messages)
        {

            ConcurrentDictionary<int, Message> unfoldedMessages = new ConcurrentDictionary<int, Message>();

            List<Task> TaskList = new List<Task>();
            foreach (var m in messages)
            {
                var LastTask = new Task(async () => {
                    var message = m;
                    unfoldedMessages.TryAdd(message.ID, await UnfoldMessageObject(message));
                });
                LastTask.Start();
                TaskList.Add(LastTask);
            }
            await Task.WhenAll(TaskList.ToArray());

            return unfoldedMessages.ToList().Select(kvp => kvp.Value).ToList();//ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }




        public async Task<Message> UnfoldMessageObject(Message message)
        {
            if(message == null) {
                return null;
            }
            if (message.GetType() == typeof(RentMessage.Text))
            {
                var textMessage = message as RentMessage.Text;
                if (textMessage != null)
                {
                    message = textMessage;
                }
            }
            else if (message.GetType() == typeof(RentMessage.Image))
            {
                var pictureMessage = message as RentMessage.Image;
                if (pictureMessage != null)
                {
                    pictureMessage.ImageArray = await _storage.DownloadImageArray(pictureMessage.ImageLocator);
                    message = pictureMessage;
                }
            }
            else if (message.GetType() == typeof(RentMessage.Video))
            {

            }
            else if (message.GetType() == typeof(RentMessage.Meeting))
            {

            }
            else if (message.GetType() == typeof(RentMessage.Complaint))
            {

            }
            else if (message.GetType() == typeof(RentMessage.MoreWork))
            {

            }
            return message;
        }
    }
}