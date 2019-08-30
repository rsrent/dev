using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RentApp.Shared.Models.Document;

namespace RentApp.Other
{
    public class DocumentConverter : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return typeof(Document).IsAssignableFrom(objectType);
        }

        enum DocumentType {
            Folder, Item
        }

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            try
            {
                JObject jObject = JObject.Load(reader);
                var type = (string)jObject["type"];
                if (string.IsNullOrWhiteSpace(type))
                {
                    type = (string)jObject["Type"];
                }
                DocumentType documentType = (DocumentType)Enum.Parse(typeof(DocumentType), type);

                Document target = null;

                switch (documentType)
                {
                    case DocumentType.Folder:
                        target = new DocumentFolder();
                        break;
                    case DocumentType.Item:
                        target = new DocumentItem();
                        break;
                }
                serializer.Populate(jObject.CreateReader(), target);
                return target;
            }
            catch (Exception exc)
            {
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
