using System;
using System.IO;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    public class XrmTaskFactory : IXrmTaskFactory
    {
        private readonly IFileReader _reader;
        private readonly IFileWriter _writer;

        public XrmTaskFactory(IFileReader reader,IFileWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public IXrmTask CreateTask(string[] args)
        {
            switch
                (args[0].ToLower().Trim())
            {
                case "deploy":
                    {
                        if (Path.GetExtension(args[1]) == ".js")
                            return new DeployWebResourceTask(new DeployWebResourceCommandLine(args), _reader);
                        throw new InvalidOperationException("Unsupported deployment file");
                    }
                case "deletesolution":
                    {
                        return new DeleteSolutionTask(new DeleteSolutionCommandLine(args));                            
                    }
                case "import":
                    {
                        return new ImportSolutionTask(new ImportSolutionCommandLine(args, _reader));
                    }
                case "export":
                    {
                        return new ExportSolutionTask(new ExportSolutionCommandLine(args), _writer);
                    }
                case "exit":
                case "close":
                    {
                        return new ExitTask();
                    }
                case "connect":
                    {
                        return new ConnectTask();
                    }
                case "publish":
                    {
                        return new PublishSolutionTask();
                    }
                default:
                    throw new InvalidOperationException(string.Format("Unknown command {0}", args[0]));
            }
        }
    }

    public class DeployWebResourceCommandLine
    {
        private readonly string[] _args;

        public DeployWebResourceCommandLine(string[] args)
        {
            _args = args;
        }

        public string FilePath { get { return _args[1]; } }

        public bool AllowEmptyFile
        {
            get { return _args.Contains("-f") || _args.Contains("-force"); }
        }
    }
}