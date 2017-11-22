using System;
using IO.Ably;

namespace MonkeyChat.Console
{
    class Program
    {

        const string AblyApiKey = "Bss0RA.2NPWDA:nKjEFbpTlwCR1zMg";


        const string MessageEvent = "message";
        static AblyRealtime _client;
        static IO.Ably.Realtime.IRealtimeChannel _channel;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");

            _client = new AblyRealtime(new ClientOptions()
            {
                Key = AblyApiKey,
                EchoMessages = false // prevent messages the client sends echoing back
            });

            _client.Connection.On(arguments =>
            {
                if (arguments.Current == IO.Ably.Realtime.ConnectionState.Connected)
                {
                    // channels don't get automatically reattached 
                    // if the connection drop, so do that manually 
                    foreach (var channel in _client.Channels)
                        channel.Attach();

                    _channel = _client.Channels.Get("general");
                    _channel.Subscribe(MessageEvent, (IO.Ably.Message msg) => {
                        System.Console.WriteLine(msg.Data.ToString());
                    });
                }
                if (arguments.Current == IO.Ably.Realtime.ConnectionState.Disconnected)
                {
                    _client.Connect();
                }
            });

            bool exit = false;
            while(!exit)
            {
                string message = System.Console.ReadLine();
                exit = message == "exit";
                _channel.Publish(MessageEvent, message);
            }
        }

    }
}
