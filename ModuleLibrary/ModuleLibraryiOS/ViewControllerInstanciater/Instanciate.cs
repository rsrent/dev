using System;
using UIKit;

namespace ModuleLibraryiOS.ViewControllerInstanciater
{
    public class Instanciate
    {
        public static void Start(UIView container, UIViewController viewController, UIViewController newView) {

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

        }
    }
}
