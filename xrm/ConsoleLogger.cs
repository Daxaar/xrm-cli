using System;
using Xrm;

namespace xrm
{
    public class ConsoleLogger : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}