using System;
using Foundation;
using Microsoft.Identity.Client;
using RentAppProject;
using UIKit;
using WindowsAzure.Messaging; using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Services;
using Newtonsoft.Json;
using System.Net;

namespace RentApp
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        public static ConversationsTableVC ConversationTable;


		private SBNotificationHub Hub { get; set; }
        public const string ConnectionString = "Endpoint=sb://rentnotifications.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=fqxxVl0lEdfPBPd1qgcxfslehOa0ookaTph4a7CM3Ds=";
		//public const string ConnectionString = "Endpoint=sb://tobystesthubnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=BQBRD5XQDz/DJQMIRdmacub4PSLvROEkF2Po45EXri4=";
        public const string NotificationHubPath = "RentNotificationsDevelopment";


        public static IServiceProvider ServiceProvider { get; } = iOSIoCContainer.Create();

        /*
		public static CleaningTasksRepository CleaningTasksRepository => AppDelegate.ServiceProvider.GetService<CleaningTasksRepository>();
		public static CompletedCleaningTasksRepository CompletedCleaningTasksRepository => AppDelegate.ServiceProvider.GetService<CompletedCleaningTasksRepository>();
		public static CustomerRepository CustomerRepository => AppDelegate.ServiceProvider.GetService<CustomerRepository>();
		public static LocationRepository LocationRepository => AppDelegate.ServiceProvider.GetService<LocationRepository>();
		public static QualityReportRepository QualityReportRepository => AppDelegate.ServiceProvider.GetService<QualityReportRepository>();
		public static RoleRepository RoleRepository => AppDelegate.ServiceProvider.GetService<RoleRepository>();
		public static PermissionRepository UserRepository => AppDelegate.ServiceProvider.GetService<PermissionRepository>();
*/
        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            if(SaveLoad.LoadText<bool>("IsLive", out var live)) {
                ServiceProvider.GetService<Settings>().Live = live;
            } else {
                SetLive(ServiceProvider.GetService<Settings>().Live);
            }

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());
			UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			UIApplication.SharedApplication.RegisterForRemoteNotifications();

            /*
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
								   UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
								   new NSSet());

				UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
				UIApplication.SharedApplication.RegisterForRemoteNotifications();
			}
			else
			{
				UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
				UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
			} */
            
            //ServiceProvider.Start();

			return true;
        }

        public static void SetLive(bool live) {
            SaveLoad.SaveText("IsLive", live);
            ServiceProvider.GetService<Settings>().Live = live;
            ServiceProvider.GetService<HttpClientProvider>().RestartClient();
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            ServiceProvider.GetService<MyConversationSocketConnection>().CloseConnection();
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            //if(ServiceProvider.GetService<IUser>())
            if (ConversationTable != null) {
				ConversationTable.UpdateBadge();
				ServiceProvider.GetService<MyConversationSocketConnection>().RestartConnection();
            }


            //UIApplication.SharedApplication.ApplicationIconBadgeNumber = 5;
            //UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			// Create a new notification hub with the connection string and hub path
			Hub = new SBNotificationHub(ConnectionString, NotificationHubPath);
            ServiceProvider.GetService<NotificationToken>().DeviceToken = deviceToken.ToString();

            System.Diagnostics.Debug.WriteLine(ServiceProvider.GetService<NotificationToken>().DeviceToken);

            //Model.Instance().StoredNotificationToken.DeviceToken = deviceToken.ToString();
			// Unregister any previous instances using the device token
            /*
			Hub.UnregisterAllAsync(deviceToken, (error) =>
			{
				if (error != null)
				{
					// Error unregistering
					return;
				}

				// Register this device with the notification hub
				Hub.RegisterNativeAsync(deviceToken, null, (registerError) =>
				{
					if (registerError != null)
					{
						// Error registering
					}
				});
			});
			*/
		}

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            //base.DidReceiveRemoteNotification(application, userInfo, completionHandler);
            //System.Diagnostics.Debug.WriteLine("HEY :D 1");
            try {
				UIApplication.SharedApplication.ApplicationIconBadgeNumber++;
            } catch {
                
            }
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            //System.Diagnostics.Debug.WriteLine("HEY :D 2");

            //UIApplication.SharedApplication.ApplicationIconBadgeNumber = 20;
            // This method is called when a remote notification is received and the

            // App is in the foreground - i.e., not backgrounded

            // We need to check that the notification has a payload (userInfo) and the payload

            // has the root "aps" key in the dictionary - this "aps" dictionary contains defined

            // keys by Apple which allows the system to determine how to handle the alert



            if (null != userInfo && userInfo.ContainsKey(new NSString("aps")))
            {
             




                // Get the aps dictionary from the alert payload

                NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;

                if (aps.ContainsKey(new NSString("content"))) {
                    NSDictionary contentDic = aps.ObjectForKey(new NSString("content")) as NSDictionary;

                    var contentType = contentDic["type"] as NSString;
                    var contentIDString = contentDic["id"] as NSString;
                    int contentID = 0;
                    if(int.TryParse(contentIDString, out int id)) {
                        contentID = id;
                    }

                    if(contentType.ToString().ToLower() == "conversation") {
                        
                    }
                    if (contentType.ToString().ToLower() == "location")
                    {

                    }

                    System.Diagnostics.Debug.WriteLine(contentDic["type"] + ", " + contentDic["id"]);

                }

                System.Diagnostics.Debug.WriteLine(aps["alert"]);

                //ConversationTable.Test();

                // Here we can do any additional processing upon receiving the notification

                // As the app is in the foreground, we can handle this alert manually

                // here by creating a UIAlert for example

            }
        }

		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
			return true;
		}
	}
}

