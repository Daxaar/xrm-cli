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
            var reader = new SystemFileReader();
            var writer = new SystemFileWriter();

            IConfigurationManager config = new JsonConfigurationManager(reader,writer);
            try
            {
                var taskFactory = new XrmTaskFactory(reader,writer,config);
                IXrmTask task = taskFactory.CreateTask(args);

                //Some tasks act on configuration and don't require a connection
                if (task.RequiresServerConnection && DoesNotContainHelpArgument(args))
                {
                    using (var connection = new ServerConnection(args, _logger, config))
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
            finally
            {
                config.Save();
            }
        }

        private static bool DoesNotContainHelpArgument(string[] args)
        {
            return !args.Any(a => a.Contains("help") || a.Contains("/?"));
        }
    }
}