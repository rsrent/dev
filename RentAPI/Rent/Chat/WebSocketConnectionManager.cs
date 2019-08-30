using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Rent.Chat
{
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<int, WebSocket> _sockets = new ConcurrentDictionary<int, WebSocket>();

        public WebSocket GetSocketsById(int id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        /*
        public List<(WebSocket, int)> GetConversationSocketsById(int conversationID)
        {
            return _sockets.Where(p => p.Value.Item2 == conversationID).Select(p => p.Value).ToList();
        } */

        public ConcurrentDictionary<int, WebSocket> GetAll()
        {
            return _sockets;
        }

        public int GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value.Equals(socket)).Key;
        }
        public void AddSocket(WebSocket socket)
        {
            var tempId = CreateTempConnectionId();
            _sockets.TryAdd(tempId, socket);
        }

        public async Task RemoveSocket(int id, WebSocket socket)
        {
            _sockets.TryRemove(id, out var junk);

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketManager",
                                    cancellationToken: CancellationToken.None);
        }

        public void UpdateID(int id, int newId)
        {
            //to avoid several socekts with same userID
            _sockets.TryRemove(newId, out var trash);
            _sockets.TryRemove(id, out WebSocket item);
            _sockets.TryAdd(newId, item);
        }

        private int CreateTempConnectionId()
        {
            return (short) new Random().Next(0, 1000000);
        }
    }
}
