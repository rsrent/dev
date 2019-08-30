using System;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RentAppProject;

namespace RentApp
{
    public abstract class RentMessage : Message
    {
        //public override string Type => throw new NotImplementedException();

        public class Text : RentMessage
        {
            public override string Type { get { return MessageType.Text.ToString(); } }
        }

        public class Video : RentMessage
        {
            public override string Type { get { return MessageType.Video.ToString(); } }
            public string VideoLocator { get; set; }
            public string ThumbnailLocator { get; set; }
            public NSUrl VideoUrl { get; set; }
            public NSData ImageData { get; set; }
        }

        public class Image : RentMessage
        {
            public override string Type { get { return MessageType.Image.ToString(); } }
            public string ImageLocator { get; set; }

            public byte[] ImageArray { get; set; }
        }

        public class Meeting : RentMessage
        {
            public override string Type { get { return MessageType.Meeting.ToString(); } }
            public DateTime Time { get; set; }
            public MessageStatus Status { get; set; }
        }

        public class Complaint : RentMessage
        {
            public override string Type { get { return MessageType.Complaint.ToString(); } }
            public string ImageLocator { get; set; }
            public DateTime Time { get; set; }
            public MessageStatus Status { get; set; }

        }

        public class MoreWork : RentMessage
        {
            public override string Type { get { return MessageType.MoreWork.ToString(); } }
            public DateTime Time { get; set; }
            public MessageStatus Status { get; set; }
        }

        public class Connect : RentMessage
        {
            public override string Type { get { return MessageType.Connect.ToString(); } }
        }

        public enum MessageType
        {
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
                try {
                    JObject jObject = JObject.Load(reader);
                    var type = (string)jObject["type"];
                    if (string.IsNullOrWhiteSpace(type))
                    {
                        type = (string)jObject["Type"];
                    }
                    MessageType messageType = (MessageType)Enum.Parse(typeof(MessageType), type);

                    Message target = null;

                    switch (messageType)
                    {
                        case MessageType.Text:
                            target = new Text();
                            break;
                        case MessageType.Image:
                            target = new Image();
                            break;
                        case MessageType.Video:
                            target = new Video();
                            break;
                        case MessageType.Meeting:
                            target = new Meeting();
                            break;
                        case MessageType.Complaint:
                            target = new Complaint();
                            break;
                        case MessageType.MoreWork:
                            target = new MoreWork();
                            break;
                        case MessageType.Connect:
                            target = new Connect();
                            break;
                    }
                    serializer.Populate(jObject.CreateReader(), target);
                    return target;
                } catch (Exception exc) {
                    return null;
                }
            }

            public override void WriteJson(JsonWriter writer, object value,
                JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
