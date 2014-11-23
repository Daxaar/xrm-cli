using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Initiating class called from containing application (Program.cs)
    /// </summary>
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
            
            IConfigurationManager configManager = new JsonConfigurationManager(reader,writer);

            IXrmConfiguration config = configManager.Load();
            var taskFactory = new XrmTaskFactory(reader, writer, config);
            try
            {
                IXrmTask task = taskFactory.CreateTask(args);

                using (var factory = new XrmServiceFactory(_logger, config))
                {
                    task.Execute(new XrmTaskContext(factory, _logger,config));
                }
            }
            catch (Exception e)
            {
                _logger.Write("Error:\n" + e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                writer.Write(_logger.History.SelectMany(s => System.Text.Encoding.ASCII.GetBytes(s)).ToArray(),
                             Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log"));
                configManager.Save(config);
            }
        }
    }

}