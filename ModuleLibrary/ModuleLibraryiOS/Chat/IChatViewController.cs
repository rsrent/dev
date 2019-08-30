using System;
using System.Collections.Generic;
using ModuleLibraryiOS.Table;
using UIKit;

namespace ModuleLibraryiOS.Chat
{
    public abstract class IChatViewController<Message> : ITableAndSourceViewController<Message>
    {

        public IChatViewController(IntPtr handle) : base(handle) { }

        public abstract UIButton GetSpecialFunctionsButton();
        public abstract UIButton GetSendButton();

        public abstract ICollection<Message> GetMessages();
        public abstract NSLayoutConstraint GetTextFieldBottomConstraint();
        public abstract UITextViewPlaceholder GetTextField();
        public abstract ICollection<SpecialChatFunction<Message>> GetSpecialChatFunctions();
        public abstract UIView GetSpecialChatFunctionsView();
        public abstract NSLayoutConstraint GetSpecialChatFunctionsViewWidthConstraint();
        public abstract void Post(object message);
    }
}
