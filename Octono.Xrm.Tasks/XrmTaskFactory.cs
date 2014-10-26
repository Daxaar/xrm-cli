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
        private readonly IConfigurationManager _configurationManager;
        public XrmTaskFactory(IFileReader reader,IFileWriter writer,IConfigurationManager configurationManager)
        {
            _reader = reader;
            _writer = writer;
            _configurationManager = configurationManager;
        }

        public IXrmTask CreateTask(string[] args)
        {
            switch
                (args[0].ToLower().Trim())
            {
                case "pull":
                    {
                        if (args.Length == 2)
                        {
                            args = new[] { args[0],args[1], Directory.GetCurrentDirectory() };
                        }
                        return new PullWebResourceTask(new PullWebResourceCommandLine(args),_writer);
                    }
                case "deploy":
                    {
                        //Assume the current directory if nothing is specified as the second argument
                        if (args.Length == 1)
                        {
                            args = new[] {args[0], Directory.GetCurrentDirectory()};
                        }
                        if (Path.GetExtension(args[1]) == ".js")
                        {
                            return new DeployWebResourceTask(new DeployWebResourceCommandLine(args), _reader, _configurationManager);                            
                        }
                        
                        return new DeployMultipleWebResourceTask(new DeployWebResourceCommandLine(args), _reader,_configurationManager);
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

        public DateTime? LastModified
        {
            get
            { 
                var modified = _args.FirstOrDefault(arg => arg.StartsWith("m:"));
                if (modified != null)
                {
                    DateTime lastModified;
                    if (DateTime.TryParse(modified.Substring(2), out lastModified))
                    {
                        return lastModified;
                    }
                }
                return null;
            }
        }

        public bool Confirm { get { return !_args.Contains("-nc") && !_args.Contains("--noconfirm"); } }
    }
}