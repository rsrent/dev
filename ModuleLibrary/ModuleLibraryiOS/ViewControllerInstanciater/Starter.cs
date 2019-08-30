using System;
using System.Linq;
using UIKit;

namespace ModuleLibraryiOS.ViewControllerInstanciater
{
    public static class Starter
    {

		public static T Start<T>(UIView container, UIViewController viewController, string storyBoard, string className) where T : UIViewController
		{

			var chatStoryboard = UIStoryboard.FromName(storyBoard, null);
            var newView = chatStoryboard.InstantiateViewController(className) as T;

			if (container != null)
			{
				newView.View.Frame = container.Bounds;
				newView.View.HeightAnchor.ConstraintEqualTo(container.HeightAnchor, 1.0f);
				if (viewController == null)
				{
					container.Add(newView.View);
					//NSLayoutConstraint.Create(newView.View, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1.0f, 300.0f).Active = true;
				}
				else
				{
					newView.WillMoveToParentViewController(viewController);
					container.AddSubview(newView.View);
					viewController.AddChildViewController(newView);
					newView.DidMoveToParentViewController(viewController);
				}
			}
			else if (viewController.NavigationController != null) viewController.NavigationController.PushViewController(newView, true);
			else viewController.PresentViewController(newView, true, null);
            return newView;
		}

        public static T Start<T>(this UIViewController viewController, UIView container) where T : UIViewController
        {
            var storyboard = viewController.Storyboard;
            var newView = storyboard.InstantiateViewController(typeof(T).ToString().Split('.').Last()) as T;

            if (container != null)
            {
                newView.View.Frame = container.Bounds;
                newView.View.HeightAnchor.ConstraintEqualTo(container.HeightAnchor, 1.0f);
                newView.WillMoveToParentViewController(viewController);

                container.AddSubview(newView.View);
                viewController.AddChildViewController(newView);
                newView.DidMoveToParentViewController(viewController);
            }
            else if (viewController.NavigationController != null) viewController.NavigationController.PushViewController(newView, true);
            else viewController.PresentViewController(newView, true, null);
            return newView;
        }

		public static T Start<T>(this UIViewController viewController, string storyBoard, string className) where T : UIViewController
		{
            var storyboard = UIStoryboard.FromName(storyBoard, null);
			var newView = storyboard.InstantiateViewController(className) as T;
			if (viewController.NavigationController != null) viewController.NavigationController.PushViewController(newView, true);
			else viewController.PresentViewController(newView, true, null);
			return newView;
		}

		public static T Start<T>(this UIViewController viewController) where T : UIViewController
		{
            var storyboard = viewController.Storyboard;
			var newView = storyboard.InstantiateViewController(typeof(T).ToString().Split('.').Last()) as T;
			if (viewController.NavigationController != null) viewController.NavigationController.PushViewController(newView, true);
			else viewController.PresentViewController(newView, true, null);
			return newView;
		}

        public static T Start<T>(this UIViewController viewController, string storyBoardName) where T : UIViewController
        {
            var storyboard = UIStoryboard.FromName(storyBoardName, null);
            var newView = storyboard.InstantiateViewController(typeof(T).ToString().Split('.').Last()) as T;
            if (viewController.NavigationController != null) viewController.NavigationController.PushViewController(newView, true);
            else viewController.PresentViewController(newView, true, null);
            return newView;
        }

        public static T Get<T>(this UIViewController viewController) where T : UIViewController
        {
            var storyboard = viewController.Storyboard;
            var newView = storyboard.InstantiateViewController(typeof(T).ToString().Split('.').Last()) as T;
            var view = newView.View;
            return newView;
        }
    }
}
