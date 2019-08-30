using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryiOS.ViewControllerInstanciater;
using Microsoft.Extensions.DependencyInjection;
using RentAppProject;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Alert;
using ModuleLibraryiOS.Storage;
using ModuleLibraryiOS.Image;
using System.Collections.Concurrent;
using System.Linq;
using RentApp.Repository;
using RentApp.ViewModels;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class ConversationsTableVC : ITableAndSourceViewController<Conversation>
    {
        TableAndSourceController<ConversationsTableVC, Conversation> TableController;

        ChatRepository _chatRepository = AppDelegate.ServiceProvider.GetService<ChatRepository>();
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        iOSImageRepository _iOSImageRepository = AppDelegate.ServiceProvider.GetService<iOSImageRepository>();
        MyConversationSocketConnection _socketConnection = AppDelegate.ServiceProvider.GetService<MyConversationSocketConnection>();
        public List<Conversation> Conversations;
        int? locationID;


        public ConversationsTableVC (IntPtr handle) : base (handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			_socketConnection.SetConversationTable(this);
            TableController = TableAndSourceController<ConversationsTableVC, Conversation>.Start(this);

            if (_userVM.HasPermission(Permission.CHAT, Permission.CRUDD.Create) && locationID == null)
            {
                this.RightNavigationButton("Ny", (btn) =>
                {
                    CreateNewConversation();
                });
            }
        }

        void CreateNewConversation() {
            this.Start<EmployeeTableVC>().SetLoading_PotentialConversationUsers(null, (vc, obj) => {
                obj.Add(_userVM.LoggedInUser());
                obj = obj.ToHashSet();
                string title = "";
                foreach(var user in obj){
                    title += user.FirstName + ", ";
                }
                _chatRepository.Create(title.Substring(0, title.Length - 2), obj.Select(u => u.ID).ToList(), (result) => {
                    this.NavigationController.PopViewController(true);
                    TableController.ReloadTable();
                }).LoadingOverlay(vc);
            });

            /*
            this.DisplayTextField("Ny samtales navn", "Titel...", (title) =>
            {
                _chatRepository.Create(title, new List<int> { _userVM.UserID() }, (result) => {
                    TableController.ReloadTable();
                }).LoadingOverlay(this);
            }); */
        }

        public override UITableViewCell GetCell(NSIndexPath path, Conversation val) => 
                                            Table.StartCell<ConversationCell>((obj) => obj.UpdateCell(val));

        public override void WillDisplayCell(UITableViewCell cell, NSIndexPath path, Conversation val)
        {
            ((ConversationCell)cell).WillDisplayCell(val);
        }

        public override UITableView GetTable() => Table;
        (NSIndexPath, Conversation)? LastSelected = null;

        public override async Task RequestTableData(Action<ICollection<Conversation>> updateAction)
        {
            if(locationID != null)
            {
                await _chatRepository.GetLocationConversations((int) locationID, (obj) => {
                    Conversations = SortConversations(obj);
                    updateAction.Invoke(Conversations);
                    UpdateBadge();
                    LastSelected = null;
                }, converter: new RentMessage.MessageConverter());
            }
            else {
                await _chatRepository.GetUserConversations(_userVM.LoggedInUser().ID, (obj) => {
                    Conversations = SortConversations(obj);
                    updateAction.Invoke(Conversations);
                    UpdateBadge();
                    LastSelected = null;
                }, converter: new RentMessage.MessageConverter());
            }
        }

        public override void RowSelected(NSIndexPath path, Conversation val)
        {
            this.DisplayLoadingWhile(async () => {
                val.UserImages = await _iOSImageRepository.GetUserImages(val.Users);
                //val.UserImages = new Dictionary<int, object>();
                LastSelected = (path, val);
                this.Start<ChatVC>().SetConversation(val);
            });
        }
        /*
        Func<Task> ReloadFunction;
        public override void ParseReloadFunction(Func<Task> Reload) => 
            ReloadFunction = Reload;

        Func<Task> RefreshFunction;
        public override void ParseRefreshFunction(Func<Task> Refresh) => 
            RefreshFunction = Refresh; 

        public override async Task RequestRefreshedData(Action<ICollection<Conversation>> updateAction)
        {
            Conversations = SortConversations(Conversations);
            updateAction(Conversations);
        }*/

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            TabBarController.TabBar.Hidden = false;

            if(LastSelected != null) {
                var last = LastSelected.GetValueOrDefault();
                if(last.Item2.Messages != null && last.Item2.Messages.Count > 0) {
					last.Item2.NewestMessage = last.Item2.Messages.Last();
                    last.Item2.LastSeenMessageID = last.Item2.NewestMessage.ID;
                    Table.ReloadRows(new[] { last.Item1 }, UITableViewRowAnimation.Fade);
                }
                LastSelected = null;
            }

            AppDelegate.ServiceProvider.GetService<MyConversationSocketConnection>().RestartConnection();
            UpdateBadge();
        }

        public void ReloadConversation(int conversationID, Message message) {
            try {
                var index = Conversations.IndexOf(Conversations.FirstOrDefault(c => c.ID == conversationID));
                Conversations.FirstOrDefault(c => c.ID == conversationID).NewestMessage = message;
                Conversations = SortConversations(Conversations);
                TableController.ReloadTable(Conversations); 
            } catch (Exception exc){
                System.Diagnostics.Debug.WriteLine("Conversation not known yet");
            }
            UpdateBadge();
        }

        List<Conversation> SortConversations(ICollection<Conversation> conversations) {
            var newConversations = conversations.Where(c => c.NewestMessage == null).OrderByDescending(c => c.ID);
            var activeConversations = conversations.Where(c => c.NewestMessage != null).OrderByDescending(c => c.NewestMessage.SentTime);
            var sortedConversations = activeConversations.ToList();
            sortedConversations.AddRange(newConversations.ToList());
            return sortedConversations;
        }

        public void UpdateBadge() {
            var unseen = 0;
            if (Conversations == null)
                return;
            foreach (var c in Conversations)
            {
                if (c.NewestMessage != null && c.NewestMessage.ID != c.LastSeenMessageID)
                    unseen++;
            }

            UIApplication.SharedApplication.ApplicationIconBadgeNumber = unseen;

            this.TabBarItem.BadgeValue = unseen > 0 ? ("" + unseen) : null;
        }

        public void SetLocation(int locationID)
        {
            this.locationID = locationID;
        }

    }
}