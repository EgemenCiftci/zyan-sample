using System;

namespace ZyanSample.Shared
{
    public interface IMessageService
    {
        bool Register(string name, Action<string, Message> callback);

        bool Unregister(string name);

        void Send(string fromName, string toName, Message message);
    }
}
