using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using xrm;

namespace Xrm
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
                        return new ExitTask(new ConsoleLogger());
                    }
                case "connect":
                    {
                        return new ConnectTask(_service, new ConsoleLogger());
                    }
                default:
                    throw new InvalidOperationException(string.Format("Unknown command {0}", args[0]));
            }
        }
    }

    public class ConnectTask : IXrmTask
    {
        private readonly IOrganizationService _service;
        private readonly ILog _logger;

        public ConnectTask(IOrganizationService service, ILog logger)
        {
            _service = service;
            _logger = logger;
        }


        public void Execute()
        {
            _logger.Write("Connected to " + ((OrganizationServiceProxy)_service).ServiceManagement.CurrentServiceEndpoint.Address);
        }
    }

    public class ExitTask : IXrmTask
    {
        private readonly ILog _logger;

        public ExitTask(ILog logger)
        {
            _logger = logger;
        }

        public void Execute()
        {
            _logger.Write("Exiting...");
        }
    }
}