using System;
using Microsoft.Xrm.Sdk;

namespace Xrm
{
    public class XrmTaskFactory : IXrmTaskFactory
    {
        private readonly string[] _args;
        private readonly IFileReader _reader;
        private readonly IOrganizationService _service;

        public XrmTaskFactory(string[] args, IFileReader reader, IOrganizationService service)
        {
            _args = args;
            _reader = reader;
            _service = service;
        }

        public IXrmTask CreateTask()
        {
            switch (_args[0].ToLower())
            {
                case "import":
                    {
                        return new ImportSolutionTask(new ImportSolutionCommandLine(_args, _reader), _service);                        
                    }
                default:
                    throw new InvalidOperationException(string.Format("Unknown command {0}", _args[0]));
            }
        }
    }

    public interface IXrmTaskFactory
    {
        IXrmTask CreateTask();
    }
}