using System;
using Octono.Xrm.Tasks;

namespace Octono.Xrm.ConsoleTaskRunner
{
    public class ConsoleLogger : ILog
    {
        public void Write(string message)
        {
            var time = DateTime.Now.TimeOfDay;
            Console.WriteLine(string.Format("{0}:{1}:{2}", time.Hours, time.Minutes, time.Seconds) + " - " + message);
        }

        public string Prompt(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
    }
}