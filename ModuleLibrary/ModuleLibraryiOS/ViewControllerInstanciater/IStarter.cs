using System;
using UIKit;

namespace ModuleLibraryiOS.ViewControllerInstanciater
{
    public abstract class IStarter<T> : UIViewController where T : IStartable
    {
        public T viewController;

        public IStarter(IntPtr handle)
        {
            Handle = handle;
        }

        public IStarter(UIViewController viewController, string storyBoard, string identifier)
        {
            this.viewController = Starter.Start<T>(null, viewController, storyBoard, identifier);
            this.viewController.AddViewDidLoadMethod(this.ViewDidLoad);
        }

        public IStarter(UIViewController viewController, UIView container, string storyBoard, string identifier)
        {
            this.viewController = Starter.Start<T>(container, viewController, storyBoard, identifier);
            this.viewController.AddViewDidLoadMethod(this.ViewDidLoad);
            this.viewController.AddViewDidLoadMethod(SetViewLoaded);
        }

        void SetViewLoaded() {
            ViewLoaded = true;
        }

        public bool ViewLoaded;

		//public abstract void ViewDidLoad();

        public T GetViewController() {
            return viewController;
        }
	}
}
