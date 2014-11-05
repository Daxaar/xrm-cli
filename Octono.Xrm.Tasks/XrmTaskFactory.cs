using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public IXrmTask CreateTask(IList<string> args)
        {
            args = StripConnectionInfo(args.ToList());
            switch
                (args[0].ToLower().Trim())
            {
                case "pull":
                    {
                        if (args.Count == 2)
                        {
                            args.Add(Directory.GetCurrentDirectory());
                        }
                        return new PullWebResourceTask(new PullWebResourceCommandLine(args),_writer);
                    }
                case "deploy":
                    {
                        //Assume the current directory if nothing is specified as the second argument
                        if (args.Count == 1)
                        {
                            args.Add(Directory.GetCurrentDirectory());
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

        private static List<string> StripConnectionInfo(List<string> args)
        {
            return args.Except(args.Where(  a => a.StartsWith("o:") || 
                                            a.StartsWith("s:") || 
                                            a.StartsWith("p:") || 
                                            a.StartsWith("protocol:")))
                       .ToList();
        }
    }

    public class DeployWebResourceCommandLine
    {
        private readonly string[] _args;

        public DeployWebResourceCommandLine(IEnumerable<string> args)
        {
            _args = args.ToArray();
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