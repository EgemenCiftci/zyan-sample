using System;
using ZyanSample.Shared;

namespace ZyanSample.Host
{
    internal class MessageService : IMessageService
    {
        public bool Register(string name, Action<string, Message> callback)
        {
            return CallbackRegistry.Instance.Register(name, callback);
        }

        public bool Unregister(string name)
        {
            return CallbackRegistry.Instance.Unregister(name);
        }

        public void Send(string fromName, string toName, Message message)
        {
            Action<string, Message> callback = CallbackRegistry.Instance.GetCallbackByName(toName);

            if (callback != null)
            {
                try
                {
                    callback(fromName, message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{2}: Error whispering to client '{0}': {1}", toName, ex.Message, DateTime.Now.ToString());
                }
            }
        }
    }
}
