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
        private readonly IXrmConfiguration _configuration;
        public XrmTaskFactory(IFileReader reader,IFileWriter writer,IXrmConfiguration configuration)
        {
            _reader = reader;
            _writer = writer;
            _configuration = configuration;
        }

        public IXrmTask CreateTask(IList<string> args)
        {
            //args = StripConnectionInfo(args.ToList());
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
                            return new DeployWebResourceTask(new DeployWebResourceCommandLine(args), _reader);                            
                        }
                        
                        return new DeployMultipleWebResourceTask(new DeployWebResourceCommandLine(args), _reader);
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
                case "addconnection":
                    {
                        return new AddConnectionTask(args);
                    }
                case "publish":
                    {
                        return new PublishSolutionTask(new PublishSolutionCommandLine(args));
                    }
                default:
                    throw new InvalidOperationException(string.Format("Unknown command {0}", args[0]));
            }
        }
    }
}