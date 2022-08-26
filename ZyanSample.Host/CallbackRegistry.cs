using System;
using System.Collections.Concurrent;
using ZyanSample.Shared;

namespace ZyanSample.Host
{
    internal class CallbackRegistry
    {
        private readonly ConcurrentDictionary<string, Action<string, Message>> _registry;

        private static readonly object _singletonLock = new object();
        private static CallbackRegistry _singleton;

        public static CallbackRegistry Instance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (_singletonLock)
                    {
                        if (_singleton == null)
                        {
                            _singleton = new CallbackRegistry();
                        }
                    }
                }
                return _singleton;
            }
        }

        private CallbackRegistry()
        {
            _registry = new ConcurrentDictionary<string, Action<string, Message>>();
        }

        public bool Register(string name, Action<string, Message> callback)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Parameter 'name' must not be empty.", "name");
            }

            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            if (_registry.ContainsKey(name))
            {
                return false;
            }

            bool success = _registry.TryAdd(name, callback);

            if (success)
            {
                Console.WriteLine("{0}: Registered '{1}'.", DateTime.Now.ToString(), name);
            }

            return success;
        }

        public bool Unregister(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Parameter 'name' must not be empty.", "name");
            }

            bool success = _registry.TryRemove(name, out Action<string, Message> removedCallback);

            if (success)
            {
                Console.WriteLine("{0}: Unregistered '{1}'.", DateTime.Now.ToString(), name);
            }

            return success;
        }

        public Action<string, Message> GetCallbackByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Parameter 'name' must not be empty.", "name");
            }


            _ = _registry.TryGetValue(name, out Action<string, Message> callback);
            return callback;
        }
    }
}
