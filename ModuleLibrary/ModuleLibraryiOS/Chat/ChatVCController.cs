using System;
using System.Collections.Generic;
using Foundation;
using ModuleLibraryiOS.Table;
using UIKit;

namespace ModuleLibraryiOS.Chat
{
    public class ChatVCController<Message>
    {
        IChatViewController<Message> ChatVC;

        public ChatVCController(IChatViewController<Message> chatVC)
        {
            ChatVC = chatVC;

            TableAndSourceController<IChatViewController<Message>, Message>.Start(ChatVC);
            ChatVC.GetTable().SeparatorStyle = UITableViewCellSeparatorStyle.None;
            ChatVC.GetTable().AllowsSelection = false;

            UIKeyboard.Notifications.ObserveWillChangeFrame(Callback);
            ModuleLibraryiOS.Chat.UITextViewPlaceholder.SetUpTextField(ChatVC.GetTextField(), "Type message... ");

            ChatVC.GetTable().PanGestureRecognizer.AddTarget((obj) => {
                var gesture = obj as UIPanGestureRecognizer;

                var dif = gesture.LocationInView(ChatVC.View).Y - ChatVC.GetTextField().Frame.Location.Y;

                if (dif > 20)
                {
                    ChatVC.GetTextField().ResignFirstResponder();
                }
            });

            if(ChatVC.GetSpecialChatFunctions() != null) {
                AddChatFunctionButtons();
            }

            ChatVC.GetSpecialChatFunctionsViewWidthConstraint().Constant = 0;
            ChatVC.GetSpecialFunctionsButton().TouchUpInside += (sender, e) => {
                if (ChatVC.GetSpecialChatFunctionsViewWidthConstraint().Constant == 0)
                {
                    ChatVC.GetSpecialChatFunctionsViewWidthConstraint().Constant = 66;
                }
                else { ChatVC.GetSpecialChatFunctionsViewWidthConstraint().Constant = 0; }
                UIView.Animate(0.2f, 0, UIViewAnimationOptions.CurveEaseOut, ChatVC.View.LayoutIfNeeded, null);
            };

            ChatVC.GetSendButton().TouchUpInside += (sender, e) => {
                ChatVC.Post(ChatVC.GetTextField().Text);
                ChatVC.GetTextField().Text = "";
            };
        }



        void Callback(object sender, UIKeyboardEventArgs args)
        {
            if (args.FrameBegin.Y > args.FrameEnd.Y)
                ChatVC.GetTextFieldBottomConstraint().Constant = args.FrameEnd.Height + 4;
            else
                ChatVC.GetTextFieldBottomConstraint().Constant = 4;
            UIView.Animate(args.AnimationDuration, 0, UIViewAnimationOptions.CurveEaseOut, () => {
                ChatVC.View.LayoutIfNeeded();
                try
                {
                    if (ChatVC.GetMessages().Count > 0)
                        ChatVC.GetTable().ScrollToRow(NSIndexPath.Create(new[] { 0, ChatVC.GetMessages().Count - 1 }), UITableViewScrollPosition.Bottom, false);
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(exc.Message);
                }

            }, () => {

            });
        }

        private void AddChatFunctionButtons()
        {
            float rowWidth = 0;
            foreach (var function in ChatVC.GetSpecialChatFunctions())
            {
                var button = new UIButton(new CoreGraphics.CGRect(rowWidth + 3, 3, 60, 60));
                button.SetTitle(function.Title(), UIControlState.Normal);
                button.Font = UIFont.FromName("Helvetica-Bold", 10f);
                button.SetTitleColor(UIColor.Gray, UIControlState.Normal);
                button.Layer.CornerRadius = 30;
                button.BackgroundColor = UIColor.White;
                button.Layer.BorderColor = function.Color().CGColor;
                button.Layer.BorderWidth = 5;
                ChatVC.GetSpecialChatFunctionsView().AddSubview(button);
                rowWidth += (float)button.Bounds.Width + 6;
                //button.TouchUpInside += (sender, e) => { function.Clicked(ChatVC, ChatVC.Post(); };
            }
            if (rowWidth > ChatVC.GetSpecialChatFunctionsView().Bounds.Width) ChatVC.GetSpecialChatFunctionsViewWidthConstraint().Constant = rowWidth;
        }
    }
}
