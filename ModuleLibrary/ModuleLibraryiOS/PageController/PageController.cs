using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using UIKit;

namespace ModuleLibraryiOS.PageController
{
    public class PageController
    {
        UIPageViewController pageVC;
        int CurrentIndex = 0;
        UISegmentedControl segmentControl;

        public PageController(IContentViewController startViewController, IContentViewController [] otherViewControllers, UIViewController Parent, UIView Container)
        {
            pageVC = new UIPageViewController(UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation.Horizontal);

            //otherViewControllers = otherViewControllers.ToList().Insert(0, startViewController);

            var startViewControllers = new UIViewController[] { startViewController as UIViewController };
            var contentViewControllers = otherViewControllers.ToList();
            contentViewControllers.Insert(0, startViewController);

            pageVC.DataSource = new PageDataSource(contentViewControllers.ToList(), (vc) => {
                CurrentIndex = vc.GetIndex();
                segmentControl.SelectedSegment = vc.GetIndex();
            });

            pageVC.SetViewControllers(startViewControllers, UIPageViewControllerNavigationDirection.Forward, true, null);
            CurrentIndex = startViewController.GetIndex();

            pageVC.View.Frame = Container.Bounds;

            segmentControl = new UISegmentedControl();
            segmentControl.Frame = new CGRect(8, 8, Container.Bounds.Width - 16, 30);

            for (int i = 0; i < contentViewControllers.Count; i++)
                segmentControl.InsertSegment(contentViewControllers[i].GetTitel(), i, false);

            segmentControl.SelectedSegment = 0;
            segmentControl.ValueChanged += (sender, e) => {
                var selectedSegmentId = (sender as UISegmentedControl).SelectedSegment;
                GoToViewController(contentViewControllers[(int) selectedSegmentId]);
            };
            Container.AddSubview(segmentControl);

            pageVC.View.Frame = new CGRect(0, 40, Container.Bounds.Width, Container.Bounds.Height - 40);

            pageVC.View.HeightAnchor.ConstraintEqualTo(Container.HeightAnchor, 1.0f);
            pageVC.WillMoveToParentViewController(Parent);

            Container.AddSubview(pageVC.View);
            Parent.AddChildViewController(pageVC);
            pageVC.DidMoveToParentViewController(Parent);
        }

        public UISegmentedControl SegmentController => segmentControl;

        public void GoToViewController(IContentViewController startViewController) 
        {
            var direction = UIPageViewControllerNavigationDirection.Forward;
            if (startViewController.GetIndex() < CurrentIndex)
                direction = UIPageViewControllerNavigationDirection.Reverse;
            pageVC.SetViewControllers(new UIViewController[] { startViewController as UIViewController }, direction, true, null);
            CurrentIndex = startViewController.GetIndex();
        }

        public class PageDataSource : UIPageViewControllerDataSource
        {
            List<IContentViewController> Pages;
            Action<IContentViewController> CurrentVC;

            public PageDataSource(List<IContentViewController> pages, Action<IContentViewController> currentVC)
            {
                Pages = pages;
                CurrentVC = currentVC;
            }

            override public UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
            {
                var currentPage = referenceViewController as IContentViewController;
                CurrentVC(currentPage);
                if (currentPage.GetIndex() == 0)
                {
                    return Pages[Pages.Count - 1] as UIViewController;
                }
                else
                {
                    return Pages[currentPage.GetIndex() - 1] as UIViewController;
                }
            }

            override public UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
            {
                var currentPage = referenceViewController as IContentViewController;
                CurrentVC(currentPage);
                return Pages[(currentPage.GetIndex() + 1) % Pages.Count] as UIViewController;
            }
        }
    }
}
