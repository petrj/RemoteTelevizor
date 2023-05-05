using LoggerService;
using RemoteAccess;

var loggingService = new BasicLoggingService();

var serverRAS = new RemoteAccessService(loggingService);
serverRAS.SetConnection("10.0.0.2", 41414, "OnlineTelevizor");

void OnMessageReceived(RemoteAccessMessage msg)
{
    Console.WriteLine($"Received message: {msg}");
}

var clientRAS = new RemoteAccessService(loggingService);
clientRAS.SetConnection("10.0.0.2", 41414, "OnlineTelevizor");

serverRAS.StartListening(OnMessageReceived);

clientRAS.SendMessage(new RemoteAccessMessage()
{
     command = "Test",
     commandArg1 = "arg1",
     commandArg2 = "arg2"
});

Console.WriteLine("Press Key....");
Console.ReadLine();
