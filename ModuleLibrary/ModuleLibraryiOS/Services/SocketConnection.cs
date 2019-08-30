using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Foundation;
using Newtonsoft.Json;
using Square.SocketRocket;

namespace ModuleLibraryiOS.Services
{
    public abstract class SocketConnection
    {
        WebSocket webSocket;
        string ConnectionString;
        //Action<string> MessageReceived;
        //Action ConnectionEstablished;


        public SocketConnection(string connectionString)
        {
            ConnectionString = connectionString;
            StartConnection();
            /*
            webSocket.WebSocketOpened += (sender, e) => {
                // the socket was opened, so we can start using it
                SendMessage("Hallo");
            };

            webSocket.ReceivedMessage += (sender, e) => {
                // read the contents
                System.Diagnostics.Debug.WriteLine("error???? " + e.Message.ToString());
            }; */


        }

        void StartConnection() {
            webSocket = null;
            NSUrl url = new NSUrl(ConnectionString);
            webSocket = new WebSocket(url);
            webSocket.Delegate = new SocketDelegate(this);
            webSocket.Open();
        }

        public async Task RestartConnection() 
        {
            if (webSocket.ReadyState == ReadyState.Open)
                return;
            StartConnection();
            while(webSocket.ReadyState != ReadyState.Open) {
                await Task.Delay(100);
                if (webSocket.ReadyState == ReadyState.Connecting)
                {
                    System.Diagnostics.Debug.WriteLine("Connection connecting, please wait");
                }
                if(webSocket.ReadyState == ReadyState.Closed) 
                {
                    System.Diagnostics.Debug.WriteLine("Connection closed. Failed to restart");
                    break;
                }
                if (webSocket.ReadyState == ReadyState.Closing)
                {
                    System.Diagnostics.Debug.WriteLine("Connection closing. Failed to restart");
                    break;
                }
            }
            await Task.Delay(10);
        }

        public abstract void MessageReceived(string message);

        public abstract void ConnectionEstablished();

        public bool Connected () => webSocket.ReadyState == ReadyState.Open;

        public bool Connecting() => webSocket.ReadyState == ReadyState.Connecting;

        public bool Closed() => webSocket.ReadyState == ReadyState.Closed || webSocket.ReadyState == ReadyState.Closing;

        public void SendMessage(string message)
        {
            webSocket.Send((NSString)message);
        }

        public void CloseConnection() 
        {
            webSocket.Close();
        }

        class SocketDelegate : WebSocketDelegate
        {
            SocketConnection SocketConnection;

            public SocketDelegate(SocketConnection socketConnection) {
                SocketConnection = socketConnection;
            }

			public override void WebSocketOpened(WebSocket webSocket)
			{
                SocketConnection.ConnectionEstablished();
                System.Diagnostics.Debug.WriteLine("Open");
				// the socket was opened
			}
			public override void WebSocketClosed(WebSocket webSocket, StatusCode code, string reason, bool wasClean)
			{
                System.Diagnostics.Debug.WriteLine("Closed: " + reason);
                //SocketConnection.ConnectionClosed();
				// the connection was closed
			}
			public override void WebSocketFailed(WebSocket webSocket, NSError error)
			{
                System.Diagnostics.Debug.WriteLine("Failed " + error);
				// there was an error
			}
			public override void ReceivedMessage(WebSocket webSocket, NSObject message)
			{
				//System.Diagnostics.Debug.WriteLine("received message < " + message.ToString());
                SocketConnection.MessageReceived(message.ToString());
				// we received a message
			}
			public override void ReceivedPong(WebSocket webSocket, NSData pongPayload)
			{
                System.Diagnostics.Debug.WriteLine("received pong");
				// respond to a ping
			}
		}
    }
}
