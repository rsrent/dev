using System;
using UIKit;

namespace ModuleLibraryiOS.Chat
{
    public abstract class SpecialChatFunction<MT>
    {
        public abstract string Title();
        public abstract UIColor Color();
        //public abstract void Clicked(UIViewController vc, Action<MT> completedAction);
        public abstract void Clicked();
    }
}
