using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer.Server.Common
{
    public class CoreValidator
    {
        public static void ThrowIfNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentException(name);
            }
        }

        public static void ThrowIfNullOrEmpty(string text, string name)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"{name} cannot be null or empty.", name);
            }
        }
    }
}
