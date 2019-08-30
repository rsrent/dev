using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rent.Data;

namespace Rent.DTOs
{
    public abstract class MessageDTO
    {
        public abstract string Type { get; }
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ConversationID { get; set; }
        public DateTime SentTime { get; set; }
        public string MessageText { get; set; }

        public static MessageDTO MessageDTOFactory(Message message)
        {
            
            //System.Diagnostics.Debug.WriteLine(message.ID + " - " + message.MessageText);
            //if (message.Type != null) System.Diagnostics.Debug.WriteLine("Has type " + message.Type);
            //else System.Diagnostics.Debug.WriteLine("Type is null");

            try {
				if (message.Type.ToLower().Equals("text"))
				{
					return new Text
					{
						ID = message.ID,
						ConversationID = message.ConversationID,
						MessageText = message.MessageText,
						SentTime = message.SentTime,
						UserID = message.UserID
					};
				}

				if (message.Type.ToLower().Equals("image"))
				{
					var specialMessage = message.SpecialMessage as MessageImage;
					return new Image
					{
						ID = message.ID,
						ConversationID = message.ConversationID,
						MessageText = message.MessageText,
						SentTime = message.SentTime,
						UserID = message.UserID,
						ImageLocator = specialMessage.ImageLocator
					};
				}

				if (message.Type.ToLower().Equals("video"))
				{
					var specialMessage = message.SpecialMessage as MessageVideo;
					return new Video
					{
						ID = message.ID,
						ConversationID = message.ConversationID,
						MessageText = message.MessageText,
						SentTime = message.SentTime,
						UserID = message.UserID,
						ThumbnailLocator = specialMessage.ThumbnailLocator,
						VideoLocator = specialMessage.VideoLocator
					};
				}

				if (message.Type.ToLower().Equals("meeting"))
				{
					var specialMessage = message.SpecialMessage as MessageMeeting;
					return new Meeting
					{
						ID = message.ID,
						ConversationID = message.ConversationID,
						MessageText = message.MessageText,
						SentTime = message.SentTime,
						UserID = message.UserID,
						Time = specialMessage.Time,
						Status = specialMessage.Status
					};
				}

				if (message.Type.ToLower().Equals("complaint"))
				{
					var specialMessage = message.SpecialMessage as MessageComplaint;
					return new Complaint
					{
						ID = message.ID,
						ConversationID = message.ConversationID,
						MessageText = message.MessageText,
						SentTime = message.SentTime,
						UserID = message.UserID,
						ImageLocator = specialMessage.ImageLocator,
						Time = specialMessage.Time,
						Status = specialMessage.Status
					};
				}

				if (message.Type.ToLower().Equals("morework"))
				{
                    var specialMessage = message.SpecialMessage as MessageMoreWork;
					return new MoreWork
					{
						ID = message.ID,
						ConversationID = message.ConversationID,
						MessageText = message.MessageText,
						SentTime = message.SentTime,
						UserID = message.UserID,
						Time = specialMessage.Time,
						Status = specialMessage.Status
					};
				}
            } catch (Exception exc) {
                System.Diagnostics.Debug.WriteLine("An error occured while building message in MesageDTOFactory: \n" + exc.Message);
            }
            return null;
        }

        public class Text : MessageDTO
        {
            public override string Type { get { return MessageType.Text.ToString(); } }
        }

        public class Video : MessageDTO
        {
            public override string Type { get { return MessageType.Video.ToString(); } }
            public string VideoLocator { get; set; }
            public string ThumbnailLocator { get; set; }
        }

        public class Image : MessageDTO
        {
            public override string Type { get { return MessageType.Image.ToString(); } }
            public string ImageLocator { get; set; }
        }

        public class Meeting : MessageDTO
        {
            public override string Type { get { return MessageType.Meeting.ToString(); } }
            public DateTime Time { get; set; }
            public Status Status { get; set; }
        }

        public class Complaint : MessageDTO
        {
            public override string Type { get { return MessageType.Complaint.ToString(); } }
            public string ImageLocator { get; set; }
            public DateTime Time { get; set; }
            public Status Status { get; set; }

        }

        public class MoreWork : MessageDTO
        {
            public override string Type { get { return MessageType.MoreWork.ToString(); } }
            public DateTime Time { get; set; }
            public Status Status { get; set; }
        }
        public class Connect : MessageDTO
        {
            public override sealed string Type { get { return MessageType.Connect.ToString(); } }
        }

        public enum MessageType
        {
            Text, Video, Image, Meeting, Complaint, MoreWork, Location, Connect
        }

        public class MessageConverter : JsonConverter
        {
            public override bool CanConvert(System.Type objectType)
            {
                return typeof(MessageDTO).IsAssignableFrom(objectType);
            }

            public override object ReadJson(JsonReader reader, System.Type objectType,
                object existingValue, JsonSerializer serializer)
            {
                JObject jObject = JObject.Load(reader);
                var type = (string)jObject["Type"];
                MessageType messageType = (MessageType)Enum.Parse(typeof(MessageType), type);

                MessageDTO target = null;

                switch (messageType)
                {
                    case MessageType.Text:
                        target = new MessageDTO.Text();
                        break;
                    case MessageType.Image:
                        target = new MessageDTO.Image();
                        break;
                    case MessageType.Video:
                        target = new MessageDTO.Video();
                        break;
                    case MessageType.Meeting:
                        target = new MessageDTO.Meeting();
                        break;
                    case MessageType.Complaint:
                        target = new MessageDTO.Complaint();
                        break;
                    case MessageType.MoreWork:
                        target = new MessageDTO.MoreWork();
                        break;
                    case MessageType.Connect:
                        target = new MessageDTO.Connect();
                        break;
                }
                serializer.Populate(jObject.CreateReader(), target);
                return target;
            }

            public override void WriteJson(JsonWriter writer, object value,
                JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
