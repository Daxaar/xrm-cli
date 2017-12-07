using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Octono.Xrm.Tasks.IO;
using Octono.Xrm.Tasks.Utils;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Creates an IXrmTask concrete type based on the command passed in the arguments
    /// </summary>
    public class XrmTaskFactory : IXrmTaskFactory
    {
        private readonly IFileReader _reader;
        private readonly IFileWriter _writer;

        public XrmTaskFactory(IFileReader reader,IFileWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public IXrmTask CreateTask(IList<string> args)
        {
            //Find the ONE IXrmTask that has been decorated with the XrmTaskAttribute and whose name property matches
            //the first command in the args list or any of its aliases match the command
            var taskType = GetType().Assembly.DefinedTypes
                             .SingleOrDefault(x => x.GetCustomAttributes<XrmTaskAttribute>()
                             .Any(attribute => attribute.Name.Equals(args[0],StringComparison.CurrentCultureIgnoreCase) ||
                             attribute.Aliases.Any(a => a.Equals(args[0],StringComparison.CurrentCultureIgnoreCase))));

            //Instantiate and return the task passing its derived CommandLine type on the ctor
            if (taskType != null)
            {
                var cmdline = taskType.GetCustomAttributes<XrmTaskAttribute>().Single().CommandLine;
                var cmd = Activator.CreateInstance(cmdline,args);
                return (IXrmTask)Activator.CreateInstance(taskType,cmd);
            }

            switch
                (args[0].ToLower().Trim())
            {
                case "pull":
                    {
                        if (args.Count == 2)
                        {
                            args.Add(Directory.GetCurrentDirectory());
                        }
                        return new PullWebResourceTask(new PullWebResourceCommandLine(args), _writer, new WebResourceMetaData(_writer,_reader));
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
                            return new DeployWebResourceTask(
                                new DeployWebResourceCommandLine(args), _reader,new WebResourceQuery(),new WebResourceMetaData(_writer,_reader));
                        }

                        return new DeployMultipleWebResourceTask(new DeployWebResourceCommandLine(args), _reader,_writer);
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
                case "addconn":
                    {
                        return new AddConnectionTask(new AddConnectionCommandLine(args));
                    }
                case "listconnection":
                case "listconn":
                case "listconnections":
                case "connections":
                    {
                        return new ListConnectionsTask();
                    }
                case "removeconnection":
                case "removeconn":
                case "deleteconnection":
                case "deleteconn":
                    {
                        return new RemoveConnectionTask(args);        
                    }
                case "publish":
                    {
                        return new PublishSolutionTask(new PublishSolutionCommandLine(args));
                    }
                case "testconnection":
                case "connectiontest":
                case "getaccountnames":
                {
                    return new ConnectionTestTask(new ConnectionTestCommandLine(args));
                }
                case "query":
                case "q":
                {
                    return new QueryEntityTask(new QueryEntityCommandLine(args));
                }
                default:
                    throw new InvalidOperationException($"Unknown command {args[0]}");
            }
        }
    }
}