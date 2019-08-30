using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;

namespace ModuleLibraryiOS.Navigation
{
    public static class Navigation
    {
        public static void RightNavigationButton(this UIViewController vc, string text, Action clickAction) {
			vc.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(text, UIBarButtonItemStyle.Plain, (sender, e) => {
                clickAction.Invoke();
			}), true);
        }

        public static void RightNavigationButton(this UIViewController vc, string text, Action<UIBarButtonItem> clickAction)
        {
            Action click = null;
            var button = new UIBarButtonItem(text, UIBarButtonItemStyle.Plain, (sender, e) =>
            {
                click.Invoke();
            });

            click = () =>
            {
                clickAction.Invoke(button);
            };

            vc.NavigationItem.SetRightBarButtonItem(button, true);
        }

        public static void LeftNavigationButton(this UIViewController vc, string text, Action clickAction)
        {
            vc.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(text, UIBarButtonItemStyle.Plain, (sender, e) => {
                clickAction.Invoke();
            }), true);
        }

        public static async void AddNavigationStack(this UIViewController viewController)
        {
            

            //await Task.Delay(50);

            if (viewController == null)
                return;
            if (viewController.NavigationController == null)
                return;
            if (viewController.NavigationController.ViewControllers == null)
                return;
            if (viewController.NavigationController.ViewControllers.Length <= 1)
                return;

            viewController.NavigationItem.LeftBarButtonItems = null;
            viewController.NavigationItem.HidesBackButton = true;
            viewController.NavigationItem.BackBarButtonItem = null;

            //var bbs = new List<UIBarButtonItem>();

            var btnWidth = 65;
            UIColor color = UIColor.FromName("ThemeColor");
            var view = new UIView(new CoreGraphics.CGRect(0, 0, viewController.View.Frame.Width, 40));

            var N = (int)((viewController.View.Frame.Width - 100.0) / btnWidth);


            var rowWidth = -2;

            var vcsToRender = viewController.NavigationController.ViewControllers.Skip(Math.Max(0, viewController.NavigationController.ViewControllers.Count() - N)).ToArray();

            foreach (var vc in vcsToRender)
            {
                var title = vc.Title != null ? vc.Title : "----";
                //if (title.Length > 6)
                //    title = title.Substring(0, 5) + "..";
                /*
                bbs.Add(
                    new UIBarButtonItem("" + title, UIBarButtonItemStyle.Plain, (sender, e) =>
                    {
                        viewController.NavigationController.PopToViewController(vc, true);
                    }));
*/

                var button = new UIButton(new CGRect(rowWidth, 3, btnWidth, 36));

                button.SetTitle(title, UIControlState.Normal);
                button.TitleLabel.LineBreakMode = UILineBreakMode.TailTruncation;
                button.Font = UIFont.SystemFontOfSize(13);
                button.SetTitleColor(color, UIControlState.Normal);
                button.Layer.CornerRadius = 8.0f;
                button.Layer.BorderWidth = 0.5f;
                button.Layer.BorderColor = color.CGColor;
                //button.BackgroundColor = UIColor.Brown;

                view.AddSubview(button);
                rowWidth += btnWidth + 2;
                //AddedButtons.Add(button);

                button.TouchUpInside += (sender, e) =>
                {
                    //AddedList.Remove(user);
                    //RefreshAction.Invoke();
                    //UpdateButtons(RefreshAction);

                    viewController.NavigationController.PopToViewController(vc, true);
                    //UpdateTable();
                };



                //view.Add();

            }

            //bbs.RemoveAt(viewController.NavigationController.ViewControllers.Length - 1);
            //viewController.NavigationItem.LeftBarButtonItems = bbs.Skip(Math.Max(0, bbs.Count() - 4)).ToArray();




            //view.BackgroundColor = UIColor.Red;
            viewController.NavigationItem.TitleView = view;
        }
    }
}
