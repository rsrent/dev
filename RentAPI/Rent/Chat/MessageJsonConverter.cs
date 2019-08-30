using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rent.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Chat
{
    public class MessageJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(MessageDTO));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);

            if (item["Type"].Value<string>().ToLowerInvariant().Equals("connect"))
            {
                return item.ToObject<ConversationConnectDTO>();
            }
            else if(item["Type"].Value<string>().ToLowerInvariant().Equals("text"))
            {
                return item.ToObject<MessageTextDTO>();
            }
            else if (item["Type"].Value<string>().ToLowerInvariant().Equals("image"))
            {
                return item.ToObject<MessageImageDTO>();
            }
            throw new NotImplementedException("Invalid type");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
