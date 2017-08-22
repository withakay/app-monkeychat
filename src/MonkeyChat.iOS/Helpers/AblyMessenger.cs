using System;

using System.Threading.Tasks;
using Foundation;
using IO.Ably;

namespace MonkeyChat.iOS
{
    public class AblyMessenger : IAblyMessenger
    {
        string AblyApiKey = "Bss0RA.2NPWDA:nKjEFbpTlwCR1zMg";
        const string MessageEvent = "message";
        AblyRealtime _realtime;
        IO.Ably.Realtime.IRealtimeChannel _channel;

        public Action<Message> MessageAdded { get; set; }

        public async Task<bool> InitializeAsync()
        {
            var task = new TaskCompletionSource<bool>();

            _realtime = new AblyRealtime(AblyApiKey);
            _realtime.Connection.On(args =>
            {
                if(args.Current == IO.Ably.Realtime.ConnectionState.Connected) {
                    _channel = _realtime.Channels.Get("general");
                    _channel.Subscribe(MessageEvent, (IO.Ably.Message msg) => {

                        // We don't need to respond to messages from ourselves!
                        if(_realtime.ClientId == msg.ClientId) 
                            return;
                        
                        MessageAdded?.Invoke(new Message
                        {
                            IsIncoming = true,
                            MessageDateTime = msg.Timestamp.Value.DateTime,
                            Text = msg.Data.ToString()
                        });
                    });

                    task.SetResult(true);
                }
                if(args.Current == IO.Ably.Realtime.ConnectionState.Disconnected) {
                    _realtime.Connect();
                }
            });

            return await task.Task.ConfigureAwait(false);
        }

        public void SendMessage(string text)
        {
            _channel.Publish(MessageEvent, text);
        }
    }
}
