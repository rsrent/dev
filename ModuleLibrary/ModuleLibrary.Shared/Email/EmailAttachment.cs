using System;
namespace ModuleLibrary.Shared.Email
{
    public class EmailAttachment
    {
        public string Path { get; set; }
        public byte[] Bytes { get; set; }
        public string MediaType { get; set; }
        public string MediaSubType { get; set; }

        public EmailAttachment(string path, string mediatype, string mediasubtype)
        {
            Path = path;
            MediaType = mediatype;
            MediaSubType = mediasubtype;
        }

        public EmailAttachment(byte[] bytes, string mediatype, string mediasubtype)
        {
            Bytes = bytes;
            MediaType = mediatype;
            MediaSubType = mediasubtype;
        }
    }
}
