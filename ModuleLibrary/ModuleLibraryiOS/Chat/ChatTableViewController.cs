using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ModuleLibraryiOS.Services;
using ModuleLibraryiOS.Storage;
//using ModuleLibraryShared.Services;


//namespace ModuleLibraryiOS.Chat
//{
//    public partial class ChatTableViewController : ITableAndSourceViewController<Message>
//    {
//        public ChatTableViewController (IntPtr handle) : base (handle)
//        {
//        }

//        public static ChatTableViewController Start(UIViewController vc, UIView container, Conversation con, int thisUser, List<Func<Message, Action, UIView>> mfl, AzureStorage storage, string HttpUri)
//		{
//            //TODO FIX THIS
//            /*
//			var newView = new TableAndSourceController<ChatTableViewController, Message>(vc, container, "Chat", "ChatTableViewController").GetViewController();
//            newView.ParseInfo(con, thisUser, mfl, storage, HttpUri);
//            return newView;
//            */
//            return null;
//		}

//        public Func<bool, Task> ReloadFunc;
//        Conversation conversation;
//        int thisUserId;
//        List<Func<Message, Action, UIView>> messageFunctionList;
//        AzureStorage storage;
//        public string HttpUri;
//        private void ParseInfo(Conversation con, int thisUser, List<Func<Message, Action, UIView>> mfl, AzureStorage s, string h){
//            conversation = con;
//            thisUserId = thisUser;
//            messageFunctionList = mfl;
//            storage = s;
//            HttpUri = h;
//        }

//        public override UITableViewCell GetCell(NSIndexPath path, Message val)
//        {
//			var message = val;
//			Message previousMessage = null;
//			if (path.Row > 0) previousMessage = conversation.messages[path.Row - 1];

//			Message nextMessage = null;
//			if (path.Row < conversation.messages.Count() - 1) nextMessage = conversation.messages[path.Row + 1];

//			var showTime = previousMessage == null || message.SendTime.Ticks - previousMessage.SendTime.Ticks > new TimeSpan(0, 30, 0).Ticks;
//			var sameSenderAsNext = nextMessage != null && !nextMessage.UserId.Equals(thisUserId);
//			var thisUsersMessage = message.UserId == thisUserId;
//			return Table.TableFunctions.InstanciateCell<MessageCell>(TableView, MessageCell.Key, (cell) => { cell.UpdateCell(message, thisUsersMessage, showTime, sameSenderAsNext, messageFunctionList, this, () => { RefreshCell(path); }); });
//        }

//		void RefreshCell(NSIndexPath indexPath)
//		{
//			TableView.BeginUpdates();
//			TableView.ReloadRows(new[] { indexPath }, UITableViewRowAnimation.Automatic);
//			TableView.EndUpdates();
//		}

//        public override UITableView GetTable()
//        {
//            return TableView;
//        }

//        public override async Task RequestTableData(Action<ICollection<Message>> updateAction)
//        {
//			conversation = await getConversation(conversation.messages == null || conversation.messages.Count() == 0 ? 0 : conversation.messages.First().ID);
//			updateAction.Invoke(conversation.messages);
//        }

//        public override void RowSelected(NSIndexPath path, Message val)
//        {
            
//        }

//   //     public override void ViewDidLoadPrepersation()
//   //     {
//   //         TableView.SeparatorColor = new UIColor(0, 0, 0, 0);
//			//TableView.RegisterNibForCellReuse(MessageCell.Nib, MessageCell.Key);
//			//TableView.SetContentOffset(new CoreGraphics.CGPoint(0, float.MaxValue), true);
//        //}

//        public override void ViewDidLoad()
//        {
//            base.ViewDidLoad();
//			TableView.SeparatorColor = new UIColor(0, 0, 0, 0);
//			TableView.RegisterNibForCellReuse(MessageCell.Nib, MessageCell.Key);
//			TableView.SetContentOffset(new CoreGraphics.CGPoint(0, float.MaxValue), true);
//        }


//		private async Task<Conversation> getConversation(int afterID)
//		{
//			var messages = await new HttpCall.CallManager<List<Message>>().Call(HttpCall.CallType.Get, HttpUri + "Conversation/" + conversation.ID + "/Limit/" + 10 + "/InitialMessage/" + afterID, jsonConverter: new Message.MessageConverter());

//			for (int i = 0; i < messages.Count(); i++)
//			{
//				messages[i] = await UnfoldMessageObject(messages[i]);
//			}

//			conversation.messages.AddRange(messages);

//			var grouped = conversation.messages.GroupBy(item => item.ID);
//			conversation.messages = grouped.Select(grp => conversation.messages.FirstOrDefault(m => m.ID == grp.Key)).OrderBy(m => m.ID).ToList();
//			return conversation;
//		}

//		async Task<Message> UnfoldMessageObject(Message message)
//		{
//			if (message.GetType() == typeof(Message.Text))
//			{
//				var textMessage = message as Message.Text;
//				if (textMessage != null)
//				{
//					message = textMessage;
//				}
//			}
//			else if (message.GetType() == typeof(Message.Image))
//			{
//				var pictureMessage = message as Message.Image;
//				if (pictureMessage != null)
//				{
//					pictureMessage.ImageData = NSData.FromArray(await storage.GetImage(pictureMessage.ImageLocator));
//					message = pictureMessage;
//				}
//			}
//			else if (message.GetType() == typeof(Message.Video))
//			{

//			}
//			else if (message.GetType() == typeof(Message.Meeting))
//			{

//			}
//			else if (message.GetType() == typeof(Message.Complaint))
//			{

//			}
//			else if (message.GetType() == typeof(Message.MoreWork))
//			{

//			}

//			return message;
//		}

//        public override void ParseReloadFunction(Func<bool, Task> Reload)
//        {
//            ReloadFunc = Reload;
//        }
//    }
//}