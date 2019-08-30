using System;
using UIKit;

namespace ModuleLibraryiOS.ViewControllerInstanciater
{
    public abstract class IStartable : UIViewController
    {
        public IStartable(IntPtr handle) : base (handle) { }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            if(viewDidLoad != null)
			    viewDidLoad.Invoke();
            ViewLoaded = true;
		}

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        bool ViewLoaded;
		Action viewDidLoad;
		public void AddViewDidLoadMethod(Action didLoad)
		{
			viewDidLoad = didLoad;
            if(ViewLoaded)
                viewDidLoad.Invoke();
		}
    }
}
