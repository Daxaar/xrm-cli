﻿using Octono.Xrm.Tasks;

namespace Octono.Xrm.ConsoleTaskRunner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Uncomment for testing via console.
            //args = new[] {"addconnection","test", "http://server/org"};
            //args = new string[]{"pull","new_/app.js","to:app.js","w12dev"};
            var runner = new XrmTaskRunner(new ConsoleLogger());
            
            runner.Run(args);
        }
    }
}
