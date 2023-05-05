using LoggerService;
using RemoteAccess;

var loggingService = new BasicLoggingService();

var serverRAS = new RemoteAccessService(loggingService);
serverRAS.SetConnection("10.0.0.2", 41414, "OnlineTelevizor");

serverRAS.OnMessageReceived += (sender, args) =>
{
    Console.WriteLine($"Received message: {args.Message}");
};

var clientRAS = new RemoteAccessService(loggingService);
clientRAS.SetConnection("10.0.0.2", 41414, "OnlineTelevizor");

serverRAS.StartListening();

clientRAS.SendMessage(new RemoteAccessMessage()
{
     command = "Test",
     commandArg1 = "arg1",
     commandArg2 = "arg2"
});

Console.WriteLine("Press Key....");
Console.ReadLine();


clientRAS.SendMessage(new RemoteAccessMessage()
{
    command = "Test 2",
    commandArg1 = "arg3",
    commandArg2 = "arg4"
});

Console.WriteLine("Press Key....");
Console.ReadLine();
