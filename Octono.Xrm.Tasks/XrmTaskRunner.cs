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
                var taskFactory = new XrmTaskFactory(new SystemFileReader(),new SystemFileWriter());
                IXrmTask task   = taskFactory.CreateTask(args);

                //Some tasks act on configuration and don't require a connection
                if (task.RequiresServerConnection)
                {
                    using (var connection = new ServerConnection(args, _logger, new SystemConfigurationManager()))
                    {
                        task.Execute(new DefaultXrmTaskContext(connection.CreateOrgService(), _logger));
                    }                    
                }
                else
                {
                    task.Execute(new DefaultXrmTaskContext(_logger));
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