using System;
using System.Diagnostics;
using Zyan.Communication;
using ZyanSample.Shared;

namespace ZyanSample.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            Console.ReadKey();
        }

        private static void Init()
        {
            int port = 3333;

            Zyan.Communication.Protocols.Ipc.IpcBinaryServerProtocolSetup ps = new Zyan.Communication.Protocols.Ipc.IpcBinaryServerProtocolSetup("ZyanSample");

            ZyanComponentHost host = new ZyanComponentHost("ZyanSample", ps);
            host.RegisterComponent<IMessageService, MessageService>();

            host.ClientLoggedOn += (s, e) =>
            {
                Console.WriteLine("A client is logged on. {0}", e.ClientAddress);
            };

            Console.WriteLine("Listening on port {0}.", port);

            // Start three clients
            Process.Start("Client\\ZyanSample.Client.exe", "0");
            Process.Start("Client\\ZyanSample.Client.exe", "1");
            Process.Start("Client\\ZyanSample.Client.exe", "2");
        }
    }
}
