using System;
using Microsoft.Xrm.Sdk;
using xrm;

namespace Xrm
{
    public class XrmTaskFactory : IXrmTaskFactory
    {
        private readonly IFileReader _reader;
        private readonly IOrganizationService _service;

        public XrmTaskFactory(IFileReader reader, IOrganizationService service)
        {
            _reader = reader;
            _service = service;
        }

        public IXrmTask CreateTask(string[] args)
        {
            switch (args[0].ToLower().Trim())
            {
                case "import":
                    {
                        return new ImportSolutionTask(new ImportSolutionCommandLine(args, _reader), _service, new ConsoleLogger());                        
                    }
                case "export":
                    {
                        return new ExportSolutionTask(args[1], new SystemFileWriter(),_service,"");
                    }
                default:
                    throw new InvalidOperationException(string.Format("Unknown command {0}", args[0]));
            }
        }
    }

    //public class ExportSolutionCommandLine
    //{
    //    public ExportSolutionCommandLine(string[] args)
    //    {
    //        ExportFilePath = args.ReadArg()
    //    }
    //}

    public interface IXrmTaskFactory
    {
        IXrmTask CreateTask(string[] args);
    }
}