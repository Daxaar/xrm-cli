using System;
using System.Linq;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    public class DeployMultipleWebResourceTask : IXrmTask
    {
        private readonly DeployWebResourceCommandLine _commandLine;
        private readonly IFileReader _reader;
        private readonly DateTime? _lastModified;
        private readonly IConfigurationManager _config;

        public DeployMultipleWebResourceTask(DeployWebResourceCommandLine commandLine, IFileReader reader, IConfigurationManager config)
        {
            _commandLine = commandLine;
            _reader = reader;
            _config = config;
            _lastModified = _config.AppSettings.AllKeys.Contains("lastmodified") ? DateTime.Parse(config.AppSettings["lastmodified"]) : DateTime.MinValue;
            RequiresServerConnection = true;
        }

        public void Execute(IXrmTaskContext context)
        {
            var files = _reader.GetModifiedFilesSince(_lastModified.Value, _commandLine.FilePath).ToList();

            if (_commandLine.Confirm)
            {
                context.Log.Write("The following files will be deployed...");
                files.ForEach(file => context.Log.Write("\t" + file));
                if(context.Log.Prompt("Do you wish to continue y/n").Contains("n")) return;
            }

            foreach (var file in files)
            {
                //TODO: Refactor to support creation by TaskFactory.  CommandLine ctor args are too tightly coupled to tasks
                var commandLine = new DeployWebResourceCommandLine(new[]{"deploy",file});
                var task = new DeployWebResourceTask(commandLine, _reader,_config);
                task.Execute(context);
            }
            _config.Add("lastmodified",DateTime.Now.ToString());
        }

        public bool RequiresServerConnection { get; private set; }
    }
}