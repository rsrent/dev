using System;
using ModuleLibraryiOS.Input;
using ModuleLibraryiOS.Map;
using ModuleLibraryiOS.Camera;
using ModuleLibraryiOS.Chat;
using UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ModuleLibraryShared.Services.HttpCall;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentAppProject;

namespace RentApp
{
    /*
    public class ChatFunctions
    {
        public static Dictionary<string, (Func<Message, Action, UIView>, Action<UIViewController, Action<Message>>, UIColor)> GetSpecialChatFunctions()
        {
            var dic = new Dictionary<string, (Func<Message, Action, UIView>, Action<UIViewController, Action<Message>>, UIColor)>();



            dic.Add("meeting",
            ((message, action) =>
            {
                var requestMessage = message as Message.Meeting;
                if (requestMessage != null)
                {
                    if(requestMessage.Status == Message.MessageStatus.Confirmed || requestMessage.Status == Message.MessageStatus.Declined) {
						return MessageRequestView.Create(requestMessage, Buttons: new List<(string, Action)>{
                            (requestMessage.Status.ToString(), ()=> { })
    					});
                    }
                    return MessageRequestView.Create(requestMessage, Buttons: new List<(string, Action)>{
                        ("Ok", async ()=> {
                            var success = await ChangeMessageStatus(requestMessage.ID, Message.MessageStatus.Confirmed);
                            if(success) {
                                requestMessage.Status = Message.MessageStatus.Confirmed;
                                action.Invoke();
                            }
                        }), ("Decline", async () => {
                            var success = await ChangeMessageStatus(requestMessage.ID, Message.MessageStatus.Declined);
							if(success) {
								requestMessage.Status = Message.MessageStatus.Declined;
								action.Invoke();
							}
                        })
                    });
                }
                return null;
            }
            ,
            NewMeeting, UIColor.Yellow));

            dic.Add("complaint",
            ((message, action) =>
            {
                var requestMessage = message as Message.Complaint;
                if (requestMessage != null)
                {
                    Action<UIView> AddToContainer = null;
                    if(requestMessage.ImageData != null) {
						AddToContainer = (obj) =>
						{
							var image = new UIImage(requestMessage.ImageData);
							var imageView = new UIImageView(new CoreGraphics.CGRect(0, 0, 240, 300));
							imageView.Image = image;
							obj.Add(imageView);
						};
                    }
					if (requestMessage.Status == Message.MessageStatus.Confirmed || requestMessage.Status == Message.MessageStatus.Declined)
					{
						return MessageRequestView.Create(requestMessage, Buttons: new List<(string, Action)>{
							(requestMessage.Status.ToString(), ()=> { })
						}, AddToContainer: AddToContainer);
					}
                    return MessageRequestView.Create(requestMessage, Buttons: new List<(string, Action)>{
                        ("Ok", async ()=> {
                            var success = await ChangeMessageStatus(requestMessage.ID, Message.MessageStatus.Confirmed);
							if(success) {
								requestMessage.Status = Message.MessageStatus.Confirmed;
								action.Invoke();
							}
                        })
                    }, AddToContainer: AddToContainer);
                }
                return null;
            }
            , NewComplaint, UIColor.Blue));

            dic.Add("work", ((message, action) =>
            {
                var requestMessage = message as Message.MoreWork;
                if (requestMessage != null)
                {
					if (requestMessage.Status == Message.MessageStatus.Confirmed || requestMessage.Status == Message.MessageStatus.Declined)
					{
						return MessageRequestView.Create(requestMessage, Buttons: new List<(string, Action)>{
							(requestMessage.Status.ToString(), ()=> { })
						});
					}
                    return MessageRequestView.Create(requestMessage, Buttons: new List<(string, Action)>{
						("Ok", async ()=> {
							var success = await ChangeMessageStatus(requestMessage.ID, Message.MessageStatus.Confirmed);
							if(success) {
								requestMessage.Status = Message.MessageStatus.Confirmed;
								action.Invoke();
							}
						}), ("Decline", async () => {
							var success = await ChangeMessageStatus(requestMessage.ID, Message.MessageStatus.Declined);
							if(success) {
								requestMessage.Status = Message.MessageStatus.Declined;
								action.Invoke();
							}
						})
                    });
                }
                return null;
            }
            , NewMoreWorkRequest, UIColor.Purple));
            return dic;
        }

        public static void NewMeeting(UIViewController vc, Action<Message> post)
        {
            DatePickerViewController.Start(vc).Initialize((date) =>
			{
				TextInputViewController.Start(vc).Initialize((text) =>
				{
					vc.NavigationController.PopToViewController(vc, true);
					post.Invoke(new Message.Meeting { Time = date, MessageText = text[0] });
                }, "Add comment",  new [] {"Type note here" });
            }, title: "Select time of meeting", minDate: DateTime.Now);
        }

        public static void NewComplaint(UIViewController vc, Action<Message> post)
        {
            DatePickerViewController.Start(vc).Initialize((date) =>
			{
				TextInputViewController.Start(vc).Initialize((text) =>
				{
					CameraContainerViewController.Start(null, vc, (data) =>
					{
						vc.NavigationController.PopToViewController(vc, true);
						post.Invoke(new Message.Complaint { Time = date, MessageText = text[0], ImageData = data });
					}, null);
				},"What is the issue?", new[] { "Type issue here" });
			}, "When did it happen");
        }

        public static void NewMoreWorkRequest(UIViewController vc, Action<Message> post)
        {
            DatePickerViewController.Start(vc).Initialize((date) =>
            {
                TextInputViewController.Start(vc).Initialize((text) =>
                {
                    vc.NavigationController.PopToViewController(vc, true);
                    post.Invoke(new Message.MoreWork { Time = date, MessageText = text[0] });
                },"What would you like cleaned", new[] { "Type request here" });
            }, "Select time of more work");
        }
        private static async Task<bool> ChangeMessageStatus(int id, Message.MessageStatus status) {
            //TODO FIX
            return true;
            //return await new CallManager().Call(CallType.Put, ServiceProvider.Settings.HttpUri + "Conversation/ChangeMessageStatus/" + id + "/" + status);
        }
    }
    */
}
