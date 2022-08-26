using System;
using Zyan.Communication;
using ZyanSample.Shared;

namespace ZyanSample.Client
{
    internal class Program
    {
        private static string userName;
        private static readonly Random random = new Random();

        private static void Main(string[] args)
        {
            try
            {
                userName = (args == null || args.Length < 1) ? null : args[0];

                Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            _ = Console.ReadKey();
        }

        private static void Init()
        {
            Zyan.Communication.Protocols.Ipc.IpcBinaryClientProtocolSetup ps = new Zyan.Communication.Protocols.Ipc.IpcBinaryClientProtocolSetup();
            string url = ps.FormatUrl("ZyanSample", "ZyanSample");

            ZyanConnection connection = new ZyanConnection(url, ps);
            IMessageService proxy = connection.CreateProxy<IMessageService>();

            bool success = proxy.Register(userName, (fromName, message) =>
            {
                Console.WriteLine("{0} whispers to you: {1}", fromName, message.Content);
            });

            if (success)
            {
                Console.WriteLine("Press a key to send a message...");
                _ = Console.ReadKey();

                for (int i = 0; true; i++)
                {
                    string randomName = userName;

                    while (randomName == userName)
                    {
                        randomName = random.Next(0, 3).ToString();
                    }

                    proxy.Send(userName, randomName, new Message { Id = i, Content = "Hello from client " + userName });

                    Console.WriteLine("Press a key to send a message...");
                    _ = Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Can not register.");
            }
        }
    }
}
