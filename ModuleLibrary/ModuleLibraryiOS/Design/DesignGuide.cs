using System;
using UIKit;

namespace ModuleLibraryiOS.Design
{
    public class DesignGuide
    {
        
        public UIColor TextColor;

        public UIColor ButtonTextColor;
        public UIColor ButtonBackgroundColor;




        public void DesignButton(UIButton button) 
        {
            if(ButtonTextColor != null) button.SetTitleColor(ButtonTextColor, UIControlState.Normal);
            if(ButtonBackgroundColor != null) button.Layer.BackgroundColor = ButtonBackgroundColor.CGColor;
        }
    }
}
