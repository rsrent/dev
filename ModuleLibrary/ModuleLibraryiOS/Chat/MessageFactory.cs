using System;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModuleLibrary.Chat
{
    /*
	public abstract class Message
	{
		public abstract string Type { get; }
		public int ID { get; set; }
        public int UserID { get; set; }
        public int ConversationID { get; set; }
		public DateTime SendTime { get; set; }
		public string MessageText { get; set; }

        public Message MessageDTOFactory(DatabaseMessage dbm) {
            if (dbm.Type.ToLower().Equals("text")) {
				return new Text
				{
					ConversationID = dbm.ConversationID,
					MessageText = dbm.MessageText,
					ID = dbm.ID,
					SendTime = dbm.SentTime,
					UserID = dbm.UserID
				};
            }
                
            if (dbm.Type.ToLower().Equals("image")) {
                var specialMessage = dbm.SpecialMessage as MessageImage;
				return new Image
				{
					ConversationID = dbm.ConversationID,
					MessageText = dbm.MessageText,
					ID = dbm.ID,
					SendTime = dbm.SentTime,
					UserID = dbm.UserID,
					ImageLocator = specialMessage.ImageLocator
				};
            }

			if (dbm.Type.ToLower().Equals("video"))
			{
                var specialMessage = dbm.SpecialMessage as MessageVideo;
				return new Video
				{
					ConversationID = dbm.ConversationID,
					MessageText = dbm.MessageText,
					ID = dbm.ID,
					SendTime = dbm.SentTime,
                    UserID = dbm.UserID,

					// ADD THE SPECIAL MESSAGE VALUES
					ThumbnailLocator = "", 
                    VideoLocator = ""
				};
			}

			if (dbm.Type.ToLower().Equals("meeting"))
			{
				var specialMessage = dbm.SpecialMessage as MessageMeeting;
                return new Meeting
				{
					ConversationID = dbm.ConversationID,
					MessageText = dbm.MessageText,
					ID = dbm.ID,
					SendTime = dbm.SentTime,
                    UserID = dbm.UserID,

					// ADD THE SPECIAL MESSAGE VALUES
					Time = new DateTime(), 
                    Status = MessageStatus.Awaiting 
					
				};
			}

			if (dbm.Type.ToLower().Equals("complaint"))
			{
				var specialMessage = dbm.SpecialMessage as MessageComplaint;
                return new Complaint
                {
                    ConversationID = dbm.ConversationID,
                    MessageText = dbm.MessageText,
                    ID = dbm.ID,
                    SendTime = dbm.SentTime,
                    UserID = dbm.UserID,

                    // ADD THE SPECIAL MESSAGE VALUES
                    ImageLocator = "",
					Time = new DateTime(),
					Status = MessageStatus.Awaiting

				};
			}

			if (dbm.Type.ToLower().Equals("morework"))
			{
                var specialMessage = dbm.SpecialMessage as MessageMoreWork;
                return new MoreWork
				{
					ConversationID = dbm.ConversationID,
					MessageText = dbm.MessageText,
					ID = dbm.ID,
					SendTime = dbm.SentTime,
					UserID = dbm.UserID,

					// ADD THE SPECIAL MESSAGE VALUES
					Time = new DateTime(),
					Status = MessageStatus.Awaiting

				};
			}
            return null;
        }

		public class Text : Message
		{
			public override string Type { get { return MessageType.Text.ToString(); } }
		}

		public class Video : Message
		{
			public override string Type { get { return MessageType.Video.ToString(); } }
			public string VideoLocator { get; set; }
			public string ThumbnailLocator { get; set; }
			public NSUrl VideoUrl { get; set; }
			public NSData ImageData { get; set; }
		}

		public class Image : Message
		{
			public override string Type { get { return MessageType.Image.ToString(); } }
			public string ImageLocator { get; set; }

			public NSData ImageData { get; set; }
		}

		public class Meeting : Message
		{
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

		public enum MessageType
		{
			Text, Video, Image, Meeting, Complaint, MoreWork, Location
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

			public override object ReadJson(JsonReader reader, System.Type objectType,
				object existingValue, JsonSerializer serializer)
			{
				JObject jObject = JObject.Load(reader);
				var type = (string)jObject["Type"];
				MessageType messageType = (MessageType)Enum.Parse(typeof(MessageType), type);

				Message target = null;

				switch (messageType)
				{
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
	}*/
}