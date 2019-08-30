using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryiOS.Date;
using UIKit;

namespace ModuleLibraryiOS.Alert
{
    public static class Alert
    {
		public static void DisplayInfo(string title, string message, UIViewController viewController, Action Completed = null)
		{
			var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
			alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            viewController.PresentViewController(alert, animated: true, completionHandler: Completed);
		}

		public static void DisplayAlert(string title, string message, string[] buttonText, Action<UIAlertAction>[] actions, UIViewController viewController)
		{
			viewController.InvokeOnMainThread(() => {
				var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
				for (int i = 0; i < buttonText.Length; i++)
				{
					alert.AddAction(UIAlertAction.Create(buttonText[i], UIAlertActionStyle.Default, actions[i]));
				}
				viewController.PresentViewController(alert, animated: true, completionHandler: null);
			});
		}

        public static void DisplayAlert(this UIViewController viewController, string title, string message, List<(string, Action)> options)
        {
            viewController.InvokeOnMainThread(() => {
                var alert = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
                for (int i = 0; i < options.Count; i++)
                {
                    var action = options[i].Item2;
                    alert.AddAction(UIAlertAction.Create(options[i].Item1, UIAlertActionStyle.Default, (obj) => action.Invoke()));
                }
                viewController.PresentViewController(alert, animated: true, completionHandler: null);
            });
        }

        /*
		public static void DisplayMenu(string title, string[] buttonText, Action<UIAlertAction>[] actions, UIViewController viewController)
		{
            var alert = UIAlertController.Create(title, title, UIAlertControllerStyle.ActionSheet);

			for (int i = 0; i < buttonText.Length; i++)
			{
				alert.AddAction(UIAlertAction.Create(buttonText[i], UIAlertActionStyle.Default, actions[i]));
			}
			alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

			viewController.PresentViewController(alert, animated: true, completionHandler: null);
		}

        public static void DisplayMenu(string title, List<(string, Action)> actions, UIViewController viewController)
        {
            var alert = UIAlertController.Create(title, "", UIAlertControllerStyle.ActionSheet);
            var alertActions = new List<Action<UIAlertAction>>();
            foreach (var a in actions)
                alertActions.Add((UIAlertAction obj) => { a.Item2.Invoke(); });

            for (int i = 0; i < actions.Count; i++)
            {
                alert.AddAction(UIAlertAction.Create(actions[i].Item1, UIAlertActionStyle.Default, alertActions[i]));
            }
            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            UIPopoverPresentationController presentationPopover = alert.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = viewController.View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Right;
            }

            viewController.PresentViewController(alert, animated: true, completionHandler: null);
        } 
*/




        // til uitool bar button
        public static void DisplayMenu(this UIViewController viewController, string title, List<(string, Action)> actions, UIBarButtonItem button = null, UIView source = null)
        {
            var alert = UIAlertController.Create(title, "", UIAlertControllerStyle.ActionSheet);
            var alertActions = new List<Action<UIAlertAction>>();
            foreach (var a in actions)
                alertActions.Add((UIAlertAction obj) => { a.Item2?.Invoke(); });

            for (int i = 0; i < actions.Count; i++)
            {
                alert.AddAction(UIAlertAction.Create(actions[i].Item1, UIAlertActionStyle.Default, alertActions[i]));
            }
            alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

            UIPopoverPresentationController presentationPopover = alert.PopoverPresentationController;
            if (presentationPopover != null)
            {
                if (button != null)
                    presentationPopover.BarButtonItem = button;
                else if (source != null) {
                    presentationPopover.SourceView = source.Superview;
                    presentationPopover.SourceRect = source.Frame;
                    //presentationPopover.SourceRect = source.Frame;
                }
                else 
                    presentationPopover.SourceView = viewController.View;



                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
            }

            viewController.PresentViewController(alert, animated: true, completionHandler: null);
        }

        public static void DisplayTextField(this UIViewController viewController, string title, string placeholder, Action<string> action)
        {
            DisplayTextField(title, placeholder, action, viewController);
        }

		public static void DisplayTextField(string title, string placeholder, Action<string> action, UIViewController viewController)
		{
			var alert = UIAlertController.Create(title, "", UIAlertControllerStyle.Alert);
            UITextField tf = null;
			alert.AddTextField(textField => {
				textField.Placeholder = placeholder;
                tf = textField;
			});
            alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, new Action<UIAlertAction> ((obj) => {
                action.Invoke(tf.Text);
            })));
			alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, null));

			viewController.PresentViewController(alert, animated: true, completionHandler: null);
		}

        public static void DisplayToast(this UIViewController vc, string message)
        {
            DisplayToast(message, vc);
        }

        public static async void DisplayToast(string message, UIViewController vc) {
            var toastLabel = new UILabel(new CoreGraphics.CGRect(vc.View.Frame.Size.Width / 2 - 130, 150, 260, 260));
            toastLabel.BackgroundColor = UIColor.Black.ColorWithAlpha(0.7f);
            toastLabel.TextColor = UIColor.White;
            toastLabel.TextAlignment = UITextAlignment.Center;
            toastLabel.Text = message;
			toastLabel.Font = UIFont.FromName("Helvetica", 26f);
			toastLabel.Lines = 0;
            toastLabel.Alpha = 1.0f;
            toastLabel.Layer.CornerRadius = 10;
            toastLabel.ClipsToBounds = true;
            vc.View.AddSubview(toastLabel);

            await Task.Delay(1500);

            UIView.Animate(1.5, 0.1, UIViewAnimationOptions.CurveEaseOut,() => {
                toastLabel.Alpha = 0.0f;
            }, () => { 
                toastLabel.RemoveFromSuperview(); 
            });
        }

        /*
        public static Action DisplayLoading(UIViewController vc, string message = null) {
			var bounds = UIScreen.MainScreen.Bounds;
			var loadPop = new LoadingOverlay(bounds, message); 
			vc.View.Add(loadPop);
            return () => {
                loadPop.Hide();
                loadPop = null;
            };
        } */



		public static async void DisplayLoadingWhile(this UIViewController vc, Func<Task> loadTask, string message = null)
		{
			var bounds = UIScreen.MainScreen.Bounds;
            var loadPop = new LoadingOverlay(bounds, message); 
            vc.View.Add(loadPop);
            await Task.Delay(1);
            await loadTask.Invoke();
            loadPop.Hide();
		}

        public static async void LoadingOverlay(this Task action, UIViewController vc, string message = null)
        {
            var bounds = UIScreen.MainScreen.Bounds;
            var loadPop = new LoadingOverlay(bounds, message);
            vc.View.Add(loadPop);
            await Task.Delay(1);
            await action;
            loadPop.Hide();
        }

        public static async Task<T> LoadingOverlayWithReturn<T>(this Task<T> action, UIViewController vc, string message = null) {
            var bounds = UIScreen.MainScreen.Bounds;
            var loadPop = new LoadingOverlay(bounds, message);
            vc.View.Add(loadPop);
            await Task.Delay(1);
            var result = await action;
            loadPop.Hide();
            return result;
        } 
    }
}
