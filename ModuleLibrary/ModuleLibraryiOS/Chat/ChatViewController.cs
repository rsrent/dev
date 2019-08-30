//﻿using System;
//using Foundation;
//using UIKit;
//using System.Threading.Tasks;
//using System.Net.Http;
//using System.Collections.Generic;
//using ModuleLibraryiOS.Services;
//using System.Threading;
//using System.Net.WebSockets;
//using System.Linq;
//using ModuleLibraryiOS.Storage;
//using Newtonsoft.Json;
//using ModuleLibraryiOS.Design;
//using ModuleLibrary.Chat;
//using ModuleLibraryiOS.ViewControllerInstanciater;
//using ModuleLibraryiOS.Camera;
//using ModuleLibraryShared.Services;

//namespace ModuleLibraryiOS.Chat
//{
//    public partial class ChatViewController : UIViewController
//    {
//        protected ChatViewController(IntPtr handle) : base(handle) { }
//        static UIViewController ViewControllerStartingChat;
//		public int thisUserId;
//        public string SocketUri;
//		public string HttpUri;
//        public string AzureStorageConnectionString;
//		//public HttpClient httpClient;
//        UITableView tableView = null;
//        SocketConnection socketConnection;
//        AzureStorage storage;
//        private Conversation conversation;

//        private Dictionary<string, (Func<Message, Action, UIView>, Action<UIViewController, Action<Message>>, UIColor)> SpecialChatFuntions;
//        private List<Func<Message, Action, UIView>> messageFunctionList = new List<Func<Message, Action, UIView>>();
//        Func<bool, Task> reloadTable;

//        public static void Start(UIViewController viewController, Conversation con, int usrId, string socketUri, string httpUri, Dictionary<string, (Func<Message, Action, UIView>, Action<UIViewController, Action<Message>>, UIColor)> specialChatFuntions = null, string azureStorageConnectionString = null)
//        {
//            ViewControllerStartingChat = viewController;
//            var chatStoryboard = UIStoryboard.FromName("Chat", null);
//            var newView = chatStoryboard.InstantiateViewController("ChatViewController") as ChatViewController;
//            if (viewController.NavigationController != null) viewController.NavigationController.PushViewController(newView, true);
//            else viewController.PresentViewController(newView, true, null);
//            newView.ParseInfo(con, usrId, socketUri, httpUri, specialChatFuntions, azureStorageConnectionString);
//        }

//        private void ParseInfo(Conversation con, int usrId, string socketUri, string httpUri, Dictionary<string, (Func<Message, Action, UIView>, Action<UIViewController, Action<Message>>, UIColor)> specialChatFuntions, string azureStorageConnectionString = null)
//        {
//            this.conversation = con;
//            this.thisUserId = usrId;
//            this.SocketUri = socketUri;
//            this.HttpUri = httpUri;
//            this.AzureStorageConnectionString = azureStorageConnectionString;

//            //Instanciate.Start<CameraContainerViewController>(null, this, "Camera");

//            SpecialChatFuntions = new Dictionary<string, (Func<Message, Action, UIView>, Action<UIViewController, Action<Message>>, UIColor)>();
//			SpecialChatFuntions.Add("camera", (null, (vc, ac) => { 
//                Camera.CameraContainerViewController.Start(null, this, PostAnImage, null); 

//            }, UIColor.Red));
//			SpecialChatFuntions.Add("album", (null, (vc, ac) => { PhotoAssets.PhotoAssetsHandler.importVideo(this, PostAnImage, null); }, UIColor.Green));

//			foreach (var function in specialChatFuntions){
//                messageFunctionList.Add(function.Value.Item1);
//                SpecialChatFuntions.Add(function.Key, function.Value);
//            }
//        }

//        public override void ViewDidAppear(bool animated)
//        {
//            base.ViewDidAppear(animated);
//            if (NavigationController != null) NavigationController.NavigationBar.Hidden = false;
//        }

//        public override void ViewDidLoad()
//        {
//            base.ViewDidLoad();
//            SetDesign();

//            AutomaticallyAdjustsScrollViewInsets = false;

//            var notification = UIKeyboard.Notifications.ObserveWillChangeFrame(Callback);

//            UITextViewPlaceholder.SetUpTextField(TextField, "Type message... ");

//            SendButton.TouchUpInside += (sender, e) =>
//            {
//                Post(new Message.Text { MessageText = TextField.Text, UserId = thisUserId });
//                TextField.Text = "";
//                //reloadTable.Invoke();
//            };

//            ChatFunctionsButton.TouchUpInside += (sender, e) => {
//                if (ChatFunctionsViewHeightConstraint.Constant == 0) {
//                    ChatFunctionsViewHeightConstraint.Constant = 66;
//                } else { ChatFunctionsViewHeightConstraint.Constant = 0; }
//                UIView.Animate(0.2f, 0, UIViewAnimationOptions.CurveEaseOut, View.LayoutIfNeeded, null);
//            };

//			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("\ud83d\udd0d", UIBarButtonItemStyle.Plain, (sender, e) => {
//				if (SearchViewHeightConstraint.Constant == 0){
//					SearchViewHeightConstraint.Constant = 45;
//				} else { 
//                    SearchViewHeightConstraint.Constant = 0; 
//                    SearchBar.ResignFirstResponder();
//                }
//				UIView.Animate(0.2f, 0, UIViewAnimationOptions.CurveEaseOut, View.LayoutIfNeeded, null);
//			}), true);

//            SearchBar.SearchButtonClicked += (sender, e) => SearchBar.ResignFirstResponder();

//            ChatFunctionsViewHeightConstraint.Constant = 0;
//            AddChatFunctionButtons();

//            //socketConnection = new SocketConnection(SocketUri, MessageReceived, SendConnectionMessage);

//            //TODO FIX
//            //storage = new AzureStorage(AzureStorageConnectionString);
//            LoadAndAddConversation();
//        }

//        void SendConnectionMessage() {
//            socketConnection.SendMessage(JsonConvert.SerializeObject(new Message.Connect() { UserId = thisUserId, ConversationId = conversation.ID} ));
//		}

//        private async void LoadAndAddConversation()
//        {

//			if (conversation.messages == null)
//				conversation.messages = new List<Message>();


//            //TODO
//            //var chatTable = ChatTableViewController.Start(this, TableContainer, conversation, thisUserId, messageFunctionList, storage, HttpUri);
//            //tableView = chatTable.GetTable();
//            //reloadTable = chatTable.ReloadFunc;

//            var source = new Table.TableSource<Message>((table, indexPath, obj) =>
//			{
//				var message = obj;
//				Message previousMessage = null;
//                if (indexPath.Row > 0) previousMessage = conversation.messages[indexPath.Row - 1];

//				Message nextMessage = null;
//                if (indexPath.Row < conversation.messages.Count()-1) nextMessage = conversation.messages[indexPath.Row + 1];

//                var showTime = previousMessage == null || message.SendTime.Ticks - previousMessage.SendTime.Ticks > new TimeSpan(0, 30, 0).Ticks;
//                var sameSenderAsNext = nextMessage != null && !nextMessage.UserId.Equals(thisUserId);
//                var thisUsersMessage = message.UserId == thisUserId;
//                return Table.TableFunctions.InstanciateCell<MessageCell>(table, MessageCell.Key, (cell) => { cell.UpdateCell(message, thisUsersMessage, showTime, sameSenderAsNext, messageFunctionList, this, () => { RefreshCell(indexPath); }); });
//			}, out var opdateList);

//            opdateList.Invoke(conversation.messages);


//            var controller = Table.TableViewController.Start(this, TableContainer);
//            controller.SetTableSource(source);
//			controller.SetPreperation((table, view) => {
//				tableView = table;
//				table.SeparatorColor = new UIColor(0, 0, 0, 0);
//				table.RegisterNibForCellReuse(MessageCell.Nib, MessageCell.Key);

//				table.SetContentOffset(new CoreGraphics.CGPoint(0, float.MaxValue), true);

//			});
//            controller.SetReloadTableData(async () =>
//            {
//                conversation = await getConversation(conversation.messages == null || conversation.messages.Count() == 0 ? 0 : conversation.messages.First().ID);
//                opdateList.Invoke(conversation.messages);
//            });
//            tableView = controller.GetViewController().GetTable();
//            reloadTable = controller.GetRefreshFunction();
//            await reloadTable.Invoke(true);





//            if(conversation.messages != null && conversation.messages.Count > 0)
//                tableView.ScrollToRow(NSIndexPath.Create(new[] { 0, conversation.messages.Count - 1 }), UITableViewScrollPosition.Bottom, false);
//        }

//        void RefreshCell(NSIndexPath indexPath) {
//			tableView.BeginUpdates();
//            tableView.ReloadRows(new[] {indexPath }, UITableViewRowAnimation.Automatic);
//			tableView.EndUpdates();
//        }

//        void Callback(object sender, UIKeyboardEventArgs args)
//        {
//            if (args.FrameBegin.Y > args.FrameEnd.Y) {
//                SendButtonBottomConstraint.Constant = args.FrameEnd.Height + 4;
//                BottomConstraint.Constant = args.FrameEnd.Height + 4;
//            }
//            else {
//                SendButtonBottomConstraint.Constant = 4;
//                BottomConstraint.Constant = 4;
//            }
//            UIView.Animate(args.AnimationDuration, 0, UIViewAnimationOptions.CurveEaseOut, () => { 
//                View.LayoutIfNeeded(); 
//            },() => {
//                if(conversation.messages.Count > 0) 
//                    tableView.ScrollToRow(NSIndexPath.Create(new[] { 0, conversation.messages.Count - 1 }), UITableViewScrollPosition.Bottom, false);
//            });
//        }

//        private async Task<Conversation> getConversation(int afterID)
//        {
//            var messages = await new HttpCall.CallManager<List<Message>>().Call(HttpCall.CallType.Get, HttpUri + "Conversation/" + conversation.ID + "/Limit/" + 10 + "/InitialMessage/" + afterID, jsonConverter: new Message.MessageConverter());

//            for (int i = 0; i < messages.Count; i++) {
//                messages[i] = await UnfoldMessageObject(messages[i]);
//            }

//			conversation.messages.AddRange(messages);

//			var grouped = conversation.messages.GroupBy(item => item.ID);
//			conversation.messages = grouped.Select(grp => conversation.messages.FirstOrDefault(m => m.ID == grp.Key)).OrderBy(m => m.ID).ToList();
//			return conversation;
//        }

//        private async void MessageReceived(NSObject messageObject) {
//            bool scrollToBottom = false;
//            if(tableView.VisibleCells.Length > 0){
//				var indexOfNewestVisible = tableView.IndexPathForCell(tableView.VisibleCells[tableView.VisibleCells.Length - 1]).Row;
//				scrollToBottom = (indexOfNewestVisible == (conversation.messages.Count - 1));
//            }
//            var receivedMessage = await UnfoldMessageObject(JsonConvert.DeserializeObject<Message>(messageObject.ToString(), new Message.MessageConverter()));

//            if(receivedMessage.Type != "Connect"){
//				conversation.messages.Add(await UnfoldMessageObject(JsonConvert.DeserializeObject<Message>(messageObject.ToString(), new Message.MessageConverter())));
//				await reloadTable.Invoke(false);
//				if (scrollToBottom) tableView.ScrollToRow(NSIndexPath.Create(new[] { 0, conversation.messages.Count - 1 }), UITableViewScrollPosition.Bottom, false);
//            } else {
//                System.Diagnostics.Debug.WriteLine("Connection confirmed");
//            }
//        }

//        async Task<Message> UnfoldMessageObject(Message message) {
//			if (message.GetType() == typeof(Message.Text))
//			{
//                var textMessage = message as Message.Text;
//				if (textMessage != null)
//				{
//					message = textMessage;
//				}
//			}
//			else if (message.GetType() == typeof(Message.Image))
//			{
//                var pictureMessage = message as Message.Image;
//				if (pictureMessage != null)
//				{
//                    pictureMessage.ImageData = NSData.FromArray(await storage.GetImage(pictureMessage.ImageLocator));
//					message = pictureMessage;
//				}
//			}
//			else if (message.GetType() == typeof(Message.Video))
//			{

//			}
//            else if (message.GetType() == typeof(Message.Meeting))
//			{

//			}
//            else if (message.GetType() == typeof(Message.Complaint))
//			{

//			}
//            else if (message.GetType() == typeof(Message.MoreWork))
//			{

//			}

//            return message;
//        }

//		void Post(Message message)
//		{
//            message.UserId = thisUserId;
//            message.ConversationId = conversation.ID;
//			socketConnection.SendMessage(JsonConvert.SerializeObject(message));
//		}

//        public async void PostAnImage(NSData array) {
//            var imageStream = array.AsStream();
//			var name = await storage.UploadImage(imageStream);
//            Post(new Message.Image { ImageLocator = name });
//        }
//        public void PostAVideo(NSUrl url) { Post(new Message.Video { VideoUrl = url }); }

//		private void AddChatFunctionButtons()
//		{
//			float rowWidth = 0;
//			foreach (var function in SpecialChatFuntions)
//			{
//				var button = new UIButton(new CoreGraphics.CGRect(rowWidth + 3, 3, 60, 60));
//				button.SetTitle(function.Key, UIControlState.Normal);
//				button.Font = UIFont.FromName("Helvetica-Bold", 10f);
//				button.SetTitleColor(UIColor.Gray, UIControlState.Normal);
//				button.Layer.CornerRadius = 30;
//				button.BackgroundColor = UIColor.White;
//				button.Layer.BorderColor = function.Value.Item3.CGColor;
//				button.Layer.BorderWidth = 5;
//				ChatFunctionsView.AddSubview(button);
//				rowWidth += (float)button.Bounds.Width + 6;
//				button.TouchUpInside += (sender, e) => { function.Value.Item2.Invoke(this, Post); };
//			}
//			if (rowWidth > ChatFunctionsView.Bounds.Width) ChatFunctionsViewWidthConstraint.Constant = rowWidth;
//		}

//        public override void ViewDidDisappear(bool animated)
//        {
//            base.ViewDidDisappear(animated);
//            if (!ViewControllerStartingChat.NavigationController.ViewControllers.Contains(this))
//                socketConnection.CloseConnection();
//        }

//        static DesignGuide DesignGuide;

//        public static void SetDesignGuide(DesignGuide guide) {
//            DesignGuide = guide;
//        }

//        public static DesignGuide GetDesignGuide() {
//            return DesignGuide;
//        }

//        void SetDesign() {
//            if (DesignGuide == null) return;

//            DesignGuide.DesignButton(SendButton);
//            DesignGuide.DesignButton(ChatFunctionsButton);
//        }
//    }
//}
