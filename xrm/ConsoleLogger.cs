using System;
using System.Collections.Generic;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.ConsoleTaskRunner
{
    public class ConsoleLogger : ILog
    {
        public ConsoleLogger()
        {
            History=new List<string>();
        }

        public void Write(string message, bool withTimestamp=true)
        {
            if (withTimestamp)
            {
                var time = DateTime.Now.TimeOfDay;
                message = string.Format("{0}:{1}:{2} - {3}", time.Hours.ToString("##"), time.Minutes.ToString("##"), time.Seconds.ToString("##"), message);                
            }
            Console.WriteLine(message);
            History.Add(message + "\n");
        }

        public string Prompt(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        public List<string> History { get; private set; }
    }
}