using System;
namespace ModuleLibrary.Chat
{
    public class ConnectDTO
    {
        public int UserID { get; set; }
        public string Type = "connect";
        public ConnectDTO(int userID) {
            UserID = userID;
        }
    }
}
