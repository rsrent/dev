using System;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModuleLibraryiOS.Chat
{
    /*
	public abstract class Message
	{
        public abstract string Type { get; }
        public int ID { get; set; }
        public int UserId { get; set; }
        public int ConversationId { get; set; }
        public DateTime SendTime { get; set; }
        public string MessageText { get; set; }

        public class Text : Message{
            public override string Type { get { return MessageType.Text.ToString(); } }
        }

        public class Video : Message {
            public override string Type { get { return MessageType.Video.ToString(); } }
			public string VideoLocator { get; set; }
            public string ThumbnailLocator { get; set; }
            public NSUrl VideoUrl { get; set; }
			public NSData ImageData { get; set; }
        }

		public class Image : Message {
            public override string Type { get { return MessageType.Image.ToString(); } }
            public string ImageLocator { get; set; }

            public NSData ImageData { get; set; }
		}

        public class Meeting : Message {
            public override string Type { get { return MessageType.Meeting.ToString(); } }
            public DateTime Time { get; set; }
			public MessageStatus Status { get; set; }
        }

		public class Complaint : Message
		{
            public override string Type { get { return MessageType.Complaint.ToString(); } }
			public string ImageLocator { get; set; }
			public DateTime Time { get; set; }
			public MessageStatus Status { get; set; }

            public NSData ImageData { get; set; }
		}

		public class MoreWork : Message
		{
            public override string Type { get { return MessageType.MoreWork.ToString(); } }
			public DateTime Time { get; set; }
			public MessageStatus Status { get; set; }
		}

		public class Connect : Message
		{
            public override string Type { get { return MessageType.Connect.ToString(); } }
		}

        public enum MessageType {
            Text, Video, Image, Meeting, Complaint, MoreWork, Location, Connect
        }

		public enum MessageStatus
		{
			Awaiting, Confirmed, Declined
		}

		public class MessageConverter : JsonConverter
		{
			public override bool CanConvert(System.Type objectType)
			{
                return typeof(Message).IsAssignableFrom(objectType);
			}

			public override object ReadJson(JsonReader reader, Type objectType,
				object existingValue, JsonSerializer serializer)
			{
				JObject jObject = JObject.Load(reader);
                var type = (string) jObject["type"];
                if(string.IsNullOrWhiteSpace(type)) {
                    type = (string) jObject["Type"];
                }
                MessageType messageType = (MessageType) Enum.Parse(typeof(MessageType), type);

                Message target = null;

                switch(messageType) {
                    case MessageType.Text:
                        target = new Message.Text();
                        break;
                    case MessageType.Image:
                        target = new Message.Image();
						break;
                    case MessageType.Video:
                        target = new Message.Video();
						break;
                    case MessageType.Meeting:
                        target = new Message.Meeting();
						break;
                    case MessageType.Complaint:
						target = new Message.Complaint();
						break;
                    case MessageType.MoreWork:
						target = new Message.MoreWork();
						break;
                    case MessageType.Connect:
                        target = new Message.Connect();
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
	*/
}
