using System;
using System.Collections.Generic;
using System.Linq;
using xrm;

namespace Xrm
{
    public class XrmTaskRunner
    {
        private readonly ILog _logger;

        public XrmTaskRunner(ILog logger)
        {
            _logger = logger;
        }

        public void Run(string[] args)
        {
            try
            {
                using (var connection = new ServerConnection(args.ToList(),new ConsoleLogger()))
                {
                    var taskFactory = new XrmTaskFactory(new SystemFileReader(), connection.CreateOrgService(), _logger);
                    IXrmTask task = taskFactory.CreateTask(args);
                    task.Execute();
                }
            }
            catch (Exception e)
            {
                _logger.Write("Error:" + e.Message + "\n");
                _logger.Write(args.Contains("--debug")
                               ? e.StackTrace
                               : "\tUse --debug to view the full stacktrace");
            }
        }
    }
}