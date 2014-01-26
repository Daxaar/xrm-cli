using System;
using Octono.Xrm.Tasks;

namespace Octono.Xrm.ConsoleTaskRunner
{
    public class ConsoleLogger : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine("\t" + message);
        }
    }
}