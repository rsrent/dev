/*
using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace ModuleLibraryiOS.Chat
{
    public partial class MessageCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MessageCell");
        public static readonly UINib Nib;

        static MessageCell()
        {
            Nib = UINib.FromName("MessageCell", NSBundle.MainBundle);
        }

        protected MessageCell(IntPtr handle) : base(handle)
        {
            //Transform = CoreGraphics.CGAffineTransform.MakeRotation(3.14159f);
        }

		public MessageCell(NSString cellId) : base (UITableViewCellStyle.Default, cellId)
        {
		}

        public void UpdateCell(Message message, bool usersMessage, bool showTime, bool sameSenderAsPrevious, List<Func<Message, Action, UIView>> messageFunctionList, UIViewController vc, Action refreshCell)
		{
            TimeLabel.Text = "";
			//MessageLabel.Text = "";
            LeftImageButton.Layer.BorderColor = UIColor.White.CGColor;
            RightImageButton.Layer.BorderColor = UIColor.White.CGColor;

			if (showTime)
			{
                TimeLabel.Text = message.SendTime.ToString("U");
				TimeLabelHeightConstraint.Constant = 30;
			}
			else if(sameSenderAsPrevious)
			{
                TimeLabel.Text = "";
				TimeLabelHeightConstraint.Constant = 2;
            } else {
                TimeLabel.Text = "";
                TimeLabelHeightConstraint.Constant = 10;
            }

			if (usersMessage)
			{
                LeftWidthConstraint.Constant = 70;
                LeftImage.Hidden = true;
                RightImage.Hidden = false;
                RightWidthConstraint.Constant = 46;
			}
			else
			{
                RightWidthConstraint.Constant = 70;
                RightImage.Hidden = true;
                LeftImage.Hidden = false;
                LeftWidthConstraint.Constant = 46;
			}
            FillContentView(message, messageFunctionList, vc, refreshCell);
		}


        UIView content;
        private void FillContentView(Message message, List<Func<Message, Action, UIView>> messageFunctionList, UIViewController vc, Action refreshCell)
		{
            if (content != null) content.RemoveFromSuperview();
            content = null;

			foreach (Func<Message, Action, UIView> func in messageFunctionList)
			{
				content = func.Invoke(message, refreshCell);
				if (content != null) break;
			}
            if (content == null)
			{
                if (message.GetType() == typeof(Message.Text)) content = Text(message as Message.Text);
                if (message.GetType() == typeof(Message.Meeting)) content = Request(message as Message.Meeting);
                if (message.GetType() == typeof(Message.Complaint)) content = Request(message as Message.Complaint);
                if (message.GetType() == typeof(Message.MoreWork)) content = Request(message as Message.MoreWork);
                if (message.GetType() == typeof(Message.Image)) content = Picture(message as Message.Image, vc);
                if (message.GetType() == typeof(Message.Video)) content = Video(message as Message.Video, vc);				
            }
            content.Frame = new CoreGraphics.CGRect(0, 0, MessageContainerView.Frame.Width, MessageContainerView.Frame.Height);
            MessageContainerView.AddSubview(content);
		}

        private UIView Text (Message.Text message) 
        {
            return MessageTextView.Create(message.MessageText);
        }

        private UIView Request(Message.Meeting message)
		{
            return MessageRequestView.Create(message);
		}

        private UIView Request(Message.Complaint message)
		{
			return MessageRequestView.Create(message);
		}

        private UIView Request(Message.MoreWork message)
		{
			return MessageRequestView.Create(message);
		}

        private UIView Picture(Message.Image message, UIViewController vc)
		{
            return MessageMediaView.Create(message.ImageData, null, vc);
		}

        private UIView Video(Message.Video message, UIViewController vc)
		{
            return MessageMediaView.Create(null, message.VideoUrl, vc);
		}
    }
}
*/