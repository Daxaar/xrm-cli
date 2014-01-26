using System;
using System.Linq;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
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
                using (var connection = new ServerConnection(args.ToList(),_logger))
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