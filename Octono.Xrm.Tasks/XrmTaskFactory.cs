using System;
using Microsoft.Xrm.Sdk;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    public class XrmTaskFactory : IXrmTaskFactory
    {
        private readonly IFileReader _reader;
        private readonly IOrganizationService _service;
        private readonly ILog _logger;

        public XrmTaskFactory(IFileReader reader, IOrganizationService service,ILog logger)
        {
            _reader = reader;
            _service = service;
            _logger = logger;
        }

        public IXrmTask CreateTask(string[] args)
        {
            switch (args[0].ToLower().Trim())
            {
                case "import":
                    {
                        return new ImportSolutionTask(new ImportSolutionCommandLine(args, _reader), _service, _logger);                        
                    }
                case "export":
                    {
                        return new ExportSolutionTask(new ExportSolutionCommandLine(args), new SystemFileWriter(),_service,_logger);
                    }
                case "exit":
                case "close":
                    {
                        return new ExitTask(_logger);
                    }
                case "connect":
                    {
                        return new ConnectTask(_service, _logger);
                    }
                case "publish" :
                    {
                        return new PublishSolutionTask(_service,_logger);
                    }
                default:
                    throw new InvalidOperationException(string.Format("Unknown command {0}", args[0]));
            }
        }
    }
}