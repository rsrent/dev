using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Services;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using ModuleLibraryiOS.Search;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.ViewControllerInstanciater;
using ModuleLibraryiOS.Alert;
using ModuleLibraryiOS.Chat;
using CoreGraphics;
using RentApp.Chat;
using RentApp.ViewModels;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class ChatVC : ITableAndSourceViewController<Message>
    {
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        MessageHandler _messageHandler = AppDelegate.ServiceProvider.GetService<MessageHandler>();

        ChatRepository _chatRepository = AppDelegate.ServiceProvider.GetService<ChatRepository>();
        MyConversationSocketConnection _socketConnection = AppDelegate.ServiceProvider.GetService<MyConversationSocketConnection>();

        public TableAndSourceController<ChatVC, Message> TableController;
		public Conversation Conversation;
        //MyChatSocketConnection socketConnection;
        //Func<Task> ReloadFunction;
        //public Func<Task> InsertFunction;
        //public List<Message> ToInsert = new List<Message>();

        List<SpecialChatFunction<Message>> ChatFunctions = new List<SpecialChatFunction<Message>>();

        public ChatVC (IntPtr handle) : base (handle) { }

        public void SetConversation(Conversation conversation) {
            Conversation = conversation;
            if (Conversation.Messages == null)
                Conversation.Messages = new List<Message>();
        }

        public override UITableViewCell GetCell(NSIndexPath path, Message val)
        {
            return Table.StartCell<ChatCell>( (obj) => obj.UpdateCell(val, this));
        }

        public override UITableView GetTable() => Table;
        //public override void ParseReloadFunction(Func<Task> Reload) => ReloadFunction = Reload;
        //public override void ParseInsertItemsFunction(Func<Task> Insert) => InsertFunction = Insert;

        bool FirstLoad = true;

        public override async Task RequestTableData(Action<ICollection<Message>> updateAction)
        {
            if(Conversation.Messages.Count > 0 && FirstLoad) 
            {
                await _chatRepository.GetMessagesNewerThan(Conversation.ID, Conversation.Messages.Last().ID, async (obj) => {
                    updateAction.Invoke(await MessagesLoaded(obj));
                }, converter: new RentMessage.MessageConverter());
            }
            else 
            {
                int oldest = 0;
                if (Conversation.Messages.Count > 0 && !FirstLoad)
                    oldest = Conversation.Messages.First().ID;
                
                await _chatRepository.GetMessages(Conversation.ID, 10, oldest, async (obj) => {
                    updateAction.Invoke(await MessagesLoaded(obj));
                }, converter: new RentMessage.MessageConverter());
            }
            IsFirst();
        }

        async Task<ICollection<Message>> MessagesLoaded(ICollection<Message> messages) {
            var newMessages = await _messageHandler.UnfoldMessages(messages.Where(m => !Conversation.Messages.Any(cm => cm.ID == m.ID)).ToList());
            Conversation.Messages.AddRange(newMessages);
            Conversation.Messages = Conversation.Messages.OrderBy(m => m.ID).ToList();
            return Conversation.Messages.ToList();
        }

        /*
        public override async Task RequestItemsToInsert(Action<ICollection<Message>> updateAction)
        {
            updateAction.Invoke(ToInsert);
            ToInsert.RemoveRange(0, ToInsert.Count);
            ScrollToBottom();
        } */

        async void ScrollToBottom() {
            await Task.Delay(100);
            Table.ScrollRectToVisible(Footer.Frame, true);
        }

        UIView Footer;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = Conversation.Title;
            TabBarController.TabBar.Hidden = true;

            TableController = TableAndSourceController<ChatVC, Message>.Start(this);
            Table.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            Table.TableHeaderView = new UIView(new CoreGraphics.CGRect(0, 0, View.Frame.Width, 100));
            Footer = new UIView(new CoreGraphics.CGRect(0, 0, View.Frame.Width, 10));
            Table.TableFooterView = Footer;

            UIKeyboard.Notifications.ObserveWillChangeFrame(Callback);

            ModuleLibraryiOS.Chat.UITextViewPlaceholder.SetUpTextField(MessageTF, "Type message... ");

            Table.PanGestureRecognizer.AddTarget((obj) => {
                var gesture = obj as UIPanGestureRecognizer;

                var dif = gesture.LocationInView(View).Y - MessageTF.Frame.Location.Y;

                if(dif > 20) {
                    MessageTF.ResignFirstResponder();
                }
            });

			ChatFunctionsViewHeightConstraint.Constant = 0;
            ChatFunctionsButton.TouchUpInside += (sender, e) => {
                if (ChatFunctionsViewHeightConstraint.Constant == 0)
                {
                    ChatFunctionsViewHeightConstraint.Constant = 66;
                }
                else { ChatFunctionsViewHeightConstraint.Constant = 0; }
                UIView.Animate(0.2f, 0, UIViewAnimationOptions.CurveEaseOut, View.LayoutIfNeeded, null);
            };
            SetupChatFunctions();

            _socketConnection.Listen(this);

            SendButton.TouchUpInside += (sender, e) => {
                _socketConnection.Post(new RentMessage.Text { MessageText = MessageTF.Text, ConversationId = Conversation.ID });
                MessageTF.Text = "";
            };

            var titleButton = new UIButton(new CGRect(0, 0, 100, 30));
            titleButton.TouchUpInside += (sender, e) => {
                //if(_userVM.HasPermission(Permission.))
                this.Start<EmployeeTableVC>().SetLoading_ConversationUsers(Conversation);
            };
            titleButton.SetTitle(Conversation.Title, UIControlState.Normal);
            titleButton.SetTitleColor(UIColor.FromName("ThemeColor"), UIControlState.Normal);
            this.NavigationItem.TitleView = titleButton;
            Title = "Conversation.Title";

            var options = new List<(string, Action)>();

            if (_userVM.HasPermission(Permission.CHAT, Permission.CRUDD.Update) && Conversation.Open)
            {
                options.AddRange(new List<(string, Action)> {
                    ("Opdater samtalens navn", () => {
                        Alert.DisplayTextField("Navngiv samtalen", "Samtalens navn", async (title) => {
                            await _chatRepository.UpdateName(Conversation.ID, title, () => {
                                this.DisplayToast("Navn opdateret");
                            });
                        }, this);
                    }),
                    ("Notifikationer", Notifications)
                    });
            }

            if (Conversation.Open) {
                options.Add(("Forlad samtale", () =>
                {
                    RemoveUserFromConversation(_userVM.ID);
                }));
            }

            if(options.Count > 0) {
                this.RightNavigationButton("Rediger", (button) =>
                {
                    this.DisplayMenu("Rediger samtale", options, button: button);
                });
            }

        }

        void Notifications() 
        {
            this.DisplayAlert("Notifikationer", "Vil du have notifikationer fra denne samtale?", new List<(string, Action)>{
                ("Ja", () => { ChangeNotificationSettings(true); }),
                ("Nej", () => { ChangeNotificationSettings(false); })
            });
        }

        void RemoveUserFromConversation(int userID) 
        {
            _chatRepository.RemoveUser(Conversation.ID, userID,() => {
                this.NavigationController.PopViewController(true);
            }).LoadingOverlay(this);
        }

        void ChangeNotificationSettings(bool on) {
            _chatRepository.NotificationsOn(Conversation.ID, on, () => {

            }).LoadingOverlay(this);
        }

        void SetupChatFunctions() {
            var camera = new CameraChatFunction(this,(Message obj) => {
                obj.ConversationId = Conversation.ID;
                _socketConnection.Post(obj);
            });
            ChatFunctions.Add(camera);

            ChatFunctionsHelper.AddChatFunctionButtons<Message>(ChatFunctionsView, ChatFunctions);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            try {
                if (this.NavigationController == null)
                {
                    _socketConnection.StopListen(this);
                }
            } catch (Exception exc) {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }
        }

        void Callback(object sender, UIKeyboardEventArgs args)
        {
            if (args.FrameBegin.Y > args.FrameEnd.Y)
                BottomConstraint.Constant = args.FrameEnd.Height + 4;
            else
                BottomConstraint.Constant = 4;
            UIView.Animate(args.AnimationDuration, 0, UIViewAnimationOptions.CurveEaseOut, () => {
                View.LayoutIfNeeded();
                try {
                    if (Conversation.Messages.Count > 0)
                        Table.ScrollToRow(NSIndexPath.Create(new[] { 0, Conversation.Messages.Count - 1 }), UITableViewScrollPosition.Bottom, false);
                } catch (Exception exc) {
                    System.Diagnostics.Debug.WriteLine(exc.Message);
                }

            }, () => {
                
            });
        }

        async void IsFirst() {
            if (FirstLoad)
            {
                FirstLoad = false;
                await Task.Delay(10);
                try
                {
                    if (Conversation.Messages.Count > 0)
                        Table.ScrollToRow(NSIndexPath.Create(new[] { 0, Conversation.Messages.Count - 1 }), UITableViewScrollPosition.Bottom, false);
                }
                catch { }
            }
        }
    }
}