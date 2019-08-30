using Foundation;
using System;
using UIKit;
using RentAppProject;
using ModuleLibraryiOS.Image;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Storage;
using System.Linq;
using static RentApp.RentMessage;
using ModuleLibraryiOS.Image;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentApp.Repository;
using RentApp.ViewModels;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class ChatCell : UITableViewCell
    {
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();

        public ChatCell (IntPtr handle) : base (handle)
        {
        }

        public void UpdateCell(Message message, ChatVC chatVC) {

            //UserInteractionEnabled = false;

            bool isNextMessageSame = chatVC.Conversation.Messages.IndexOf(message) < chatVC.Conversation.Messages.Count - 1 && chatVC.Conversation.Messages[chatVC.Conversation.Messages.IndexOf(message) + 1].UserId == message.UserId;
            bool showTime = true;
            /*
            if(conversation.Messages.IndexOf(message) < conversation.Messages.Count - 1) {
                var previousMessage = conversation.Messages[conversation.Messages.IndexOf(message) + 1];
                if(message.SendTime - previousMessage.SendTime < new TimeSpan(1, 0, 0)) {
                    showTime = false;
                }
            }*/

            if (chatVC.Conversation.Messages.IndexOf(message) > 0)
            {
                var previousMessage = chatVC.Conversation.Messages[chatVC.Conversation.Messages.IndexOf(message) - 1];
                if (message.SentTime.ToLocalTime() - previousMessage.SentTime.ToLocalTime() < new TimeSpan(1, 0, 0))
                {
                    showTime = false;
                }
            }

            var senderImage = new UIImage();
            if(chatVC.Conversation.UserImages.ContainsKey(message.UserId))
                senderImage = chatVC.Conversation.UserImages[message.UserId] as UIImage;

            if(_userVM.ID == message.UserId) {
                ReceivedImage.Hidden = true;
                //SendedImage.Hidden = false;
                //SendedImage.Round();

                LeftSpaceConstraint.Active = false;
                LeftGreaterSpaceConstraint.Active = true;
                RightSpaceConstraint.Active = true;
                RightGreaterSpaceConstraint.Active = false;

                ReceivedBackground.Hidden = true;

                /*
                if (image != null && !isNextMessageSame)
                    SendedImage.Image = image as UIImage;
                else
                    SendedImage.Image = UIImage.FromFile("imagePlaceholder.png");
                */
                SenderNameHeightConstraint.Constant = 4;
                SenderName.Text = "";
            } else {
                ReceivedImage.Hidden = false;
                SendedImage.Hidden = true;
                ReceivedImage.Round();

                LeftSpaceConstraint.Active = true;
                LeftGreaterSpaceConstraint.Active = false;
                RightSpaceConstraint.Active = false;
                RightGreaterSpaceConstraint.Active = true;

                ReceivedBackground.Hidden = false;

                if (senderImage != null && !isNextMessageSame)
                    ReceivedImage.Image = senderImage as UIImage;
                else
                    ReceivedImage.Image = UIImage.FromFile("imagePlaceholder.png");

                if(!isNextMessageSame) {
                    SenderNameHeightConstraint.Constant = 16;
                    var sender = chatVC.Conversation.Users.Find(u => u.ID == message.UserId);
                    if(sender != null)
                       SenderName.Text = sender.FirstName + " " + sender.LastName;
                } else {
                    SenderNameHeightConstraint.Constant = 4;
                    SenderName.Text = "";
                }
            }

            if (showTime){
				TimeLabel.Text = message.SentTime.ToLocalTime().ToString("f");
                TimeLabelHeightConstraint.Constant = 14;
                TimeLabelTopConstraint.Constant = 14;
            }
            else {
                TimeLabel.Text = "";
                TimeLabelHeightConstraint.Constant = 0;
                TimeLabelTopConstraint.Constant = 0;
            }

            if(!String.IsNullOrWhiteSpace(message.MessageText)){
                TextLabel.Text = message.MessageText;
            } else {
                TextLabel.Text = "  ";
            }

            if(message.Type == MessageType.Image.ToString()) {
                var imageMessage = message as RentMessage.Image;
                var image = new UIImage(NSData.FromArray(imageMessage.ImageArray.ResizeImage((float)chatVC.View.Bounds.Width - 100f)));

                ImageView.Image = image;
                ImageView.Hidden = false;
                TextLabel.Hidden = true;

                ImageView.UserInteractionEnabled = false;

                var gesture = new UITapGestureRecognizer((obj) =>
                {
                    chatVC.Start<ImageVC>().ParseInfo(imageMessage.ImageArray);
                });

                gesture.NumberOfTapsRequired = 2;

                ImageView.AddGestureRecognizer(gesture);
            }
            else
            {
                ImageView.Image = null;
                ImageView.Hidden = true;
                TextLabel.Hidden = false;
            }
            /*
            if(message.ID % 2 == 0 && image != null) {
                ImageView.Image = image;
                ImageView.Hidden = false;
                TextLabel.Hidden = true;
            } else {
                ImageView.Image = null;
                ImageView.Hidden = true;
                TextLabel.Hidden = false;
            }*/
        }
    }
}