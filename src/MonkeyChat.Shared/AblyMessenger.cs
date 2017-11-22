using System;
using System.Threading.Tasks;
using IO.Ably;

namespace MonkeyChat.Shared
{
    public class AblyMessenger : IAblyMessenger
    {


        string AblyApiKey = "Bss0RA.2NPWDA:nKjEFbpTlwCR1zMg";


        const string MessageEvent = "message";
        AblyRealtime _client;
        IO.Ably.Realtime.IRealtimeChannel _channel;

        public Action<Message> MessageAdded { get; set; }

        public async Task<bool> InitializeAsync()
        {
            var task = new TaskCompletionSource<bool>();

            _client = new AblyRealtime(new ClientOptions()
            {
                Key = AblyApiKey,
                EchoMessages = false // prevent messages the client sends echoing back
            });

            _client.Connection.On(args =>
            {
                if (args.Current == IO.Ably.Realtime.ConnectionState.Connected)
                {
                    // channels don't get automatically reattached 
                    // if the connection drop, so do that manually 
                    foreach (var channel in _client.Channels)
                        channel.Attach();

                    _channel = _client.Channels.Get("general");
                    _channel.Subscribe(MessageEvent, (IO.Ably.Message msg) => {
                        MessageAdded?.Invoke(new Message
                        {
                            IsIncoming = true,
                            MessageDateTime = msg.Timestamp.Value.DateTime,
                            Text = msg.Data.ToString()
                        });
                    });
                    task.SetResult(true);
                }
                if (args.Current == IO.Ably.Realtime.ConnectionState.Disconnected)
                {
                    _client.Connect();
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
