//using Foundation;
//using System;
//using UIKit;
//using ObjCRuntime;
//using System.Collections.Generic;

//namespace ModuleLibraryiOS.Chat
//{
//    public partial class MessageRequestView : UIView
//    {
//        public MessageRequestView (IntPtr handle) : base (handle)
//        {
//        }

//        public static MessageRequestView Create(Message message, List<(string, Action)> Buttons = null, Action<UIView> AddToContainer = null)
//		{
//			var arr = NSBundle.MainBundle.LoadNib("MessageRequestView", null, null);
//			var v = Runtime.GetNSObject<MessageRequestView>(arr.ValueAt(0));
//			v.Start(message, Buttons, AddToContainer);
//			return v;
//		}

//		public void Start(Message message, List<(string, Action)> Buttons, Action<UIView> AddToContainer = null)
//		{

//            var time = new DateTime();
//            var title = "";
//            if (message.GetType() == typeof(Message.Meeting)) {
//                var meeting = message as Message.Meeting;
//                time = meeting.Time;
//                title = "Meeting";
//            }
//            if (message.GetType() == typeof(Message.Complaint)) {
//				var meeting = message as Message.Complaint;
//				time = meeting.Time;
//				title = "Complaint";
//            }
//            if (message.GetType() == typeof(Message.MoreWork)) {
//				var meeting = message as Message.MoreWork;
//				time = meeting.Time;
//				title = "Complaint";
//            }

//            TitleLabel.Text = title;
//            ContentLabel.Text = message.MessageText;
//            TimeLabel.Text = time.ToString("dd MMM HH:mm:ss");

//            AcceptButton.Layer.BorderColor = UIColor.White.CGColor;
//            DeclineButton.Layer.BorderColor = UIColor.White.CGColor;
//            AcceptButton.Layer.CornerRadius = 6;
//            DeclineButton.Layer.CornerRadius = 6;

//            AcceptButton.Layer.BorderWidth = 0.5f;
//            DeclineButton.Layer.BorderWidth = 0.5f;

//            if(Buttons == null || Buttons.Count == 0) {
//                AcceptButton.Hidden = true;
//                DeclineButton.Hidden = true;
//            } else if(Buttons.Count == 1) {
//                DeclineButton.Hidden = true;
//                AcceptButton.SetTitle(Buttons[0].Item1, UIControlState.Normal);
//            } else {
//                AcceptButton.SetTitle(Buttons[0].Item1, UIControlState.Normal);
//                DeclineButton.SetTitle(Buttons[1].Item1, UIControlState.Normal);
//            }

//            AcceptButton.TouchUpInside += (sender, e) => { Buttons[0].Item2.Invoke(); };
//            DeclineButton.TouchUpInside += (sender, e) => { Buttons[1].Item2.Invoke(); };

//            if (AddToContainer != null) {
//                AddToContainer.Invoke(ContainerView);
//                ContainerView.Hidden = false;
//            }
//            else ContainerView.Hidden = true;
//		}
//    }
//}