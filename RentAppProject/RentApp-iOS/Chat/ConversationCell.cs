using Foundation;
using System;
using UIKit;
using RentAppProject;
using System.Linq;

namespace RentApp
{
    public partial class ConversationCell : UITableViewCell
    {
        public ConversationCell (IntPtr handle) : base (handle)
        {
            TextLabel.TextColor = UIColor.FromName("ThemeColor");
        }

        public void UpdateCell(Conversation conversation) {
            ContentLabel.Text = "";
            SendTimeLabel.Text = "";
            SenderNameLabel.Text = "";

            TitleLabel.Text = conversation.Title;
            if (conversation.NewestMessage != null) {
                if(conversation.NewestMessage.GetType() == typeof(RentMessage.Text))
                    ContentLabel.Text = conversation.NewestMessage.MessageText;
                if (conversation.NewestMessage.GetType() == typeof(RentMessage.Image)) {
                    var user = conversation.Users.FirstOrDefault(u => u.ID == conversation.NewestMessage.UserId);
                    if (user != null)
                        ContentLabel.Text = user.FirstName + " sendte et billede";
                    else
                        ContentLabel.Text = "Nyt billede";
                }

                SendTimeLabel.Text = conversation.NewestMessage.SentTime.ToLocalTime().ToString("f");
                var sender = conversation.Users.Find(u => u.ID == conversation.NewestMessage.UserId);
                if (sender != null)
                    SenderNameLabel.Text = sender.FirstName + " " + sender.LastName;
            }
        }

        public void WillDisplayCell(Conversation conversation) {
            if (conversation.LastSeenMessageID == null || (conversation.NewestMessage != null && conversation.LastSeenMessageID != null && conversation.LastSeenMessageID < conversation.NewestMessage.ID))
            {
                TitleLabel.TextColor = UIColor.FromName("ThemeColor");
                ContentLabel.TextColor = UIColor.Black;

                TitleLabel.Font = UIFont.BoldSystemFontOfSize(17);
                ContentLabel.Font = UIFont.BoldSystemFontOfSize(12);
                SendTimeLabel.Font = UIFont.BoldSystemFontOfSize(10);
                SenderNameLabel.Font = UIFont.BoldSystemFontOfSize(10);
            }
            else
            {
                TitleLabel.TextColor = UIColor.FromName("FadeTextColor");
                ContentLabel.TextColor = UIColor.FromName("FadeTextColor");

                TitleLabel.Font = UIFont.SystemFontOfSize(17);
                ContentLabel.Font = UIFont.SystemFontOfSize(12);
                SendTimeLabel.Font = UIFont.SystemFontOfSize(10);
                SenderNameLabel.Font = UIFont.SystemFontOfSize(10);
            }
        }
    }
}