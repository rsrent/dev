using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace ModuleLibraryiOS.Chat
{
    public static class ChatFunctionsHelper
    {
        public static void AddChatFunctionButtons<Message>(UIView ChatFunctionsView, List<SpecialChatFunction<Message>> ChatFunctions)
        {
            float rowWidth = 0;
            foreach (var function in ChatFunctions)
            {
                var button = new UIButton(new CoreGraphics.CGRect(rowWidth + 3, 3, 60, 60));
                button.SetTitle(function.Title(), UIControlState.Normal);
                button.Font = UIFont.FromName("Helvetica-Bold", 10f);
                button.SetTitleColor(UIColor.Gray, UIControlState.Normal);
                button.Layer.CornerRadius = 30;
                button.BackgroundColor = UIColor.White;
                button.Layer.BorderColor = function.Color().CGColor;
                button.Layer.BorderWidth = 5;
                ChatFunctionsView.AddSubview(button);
                rowWidth += (float)button.Bounds.Width + 6;
                button.TouchUpInside += (sender, e) => { function.Clicked(); };
            }
            //if (rowWidth > ChatFunctionsView.Bounds.Width) ChatFunctionsViewWidthConstraint.Constant = rowWidth;
            if (rowWidth > ChatFunctionsView.Bounds.Width) 
                ChatFunctionsView.Constraints.FirstOrDefault(c => c.SecondAttribute == NSLayoutAttribute.Width).Constant = rowWidth;
        }
    }
}
