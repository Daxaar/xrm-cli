using System;
using System.Globalization;
using System.Linq;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    public class DeployMultipleWebResourceTask : IXrmTask
    {
        private readonly DeployWebResourceCommandLine _commandLine;
        private readonly IFileReader _reader;
        private readonly IFileWriter _writer;

        public DeployMultipleWebResourceTask(DeployWebResourceCommandLine commandLine, IFileReader reader, IFileWriter writer)
        {
            _commandLine = commandLine;
            _reader = reader;
            _writer = writer;
        }

        public void Execute(IXrmTaskContext context)
        {
            DateTime lastModified = context.Configuration.AppSettings.ContainsKey("lastmodified")
                    ? DateTime.Parse(context.Configuration.AppSettings["lastmodified"])
                    : DateTime.MinValue;

            var files = _reader.GetModifiedFilesSince(lastModified, _commandLine.FilePath).ToList();

            if (_commandLine.Confirm)
            {
                context.Log.Write("The following files will be deployed...");
                files.ForEach(file => context.Log.Write("\t" + file));
                if (context.Log.Prompt("Do you wish to continue y/n").Contains("n")) return;
            }

            foreach (var file in files)
            {
                //TODO: Refactor to support creation by TaskFactory.  CommandLine ctor args are too tightly coupled to tasks
                var commandLine = new DeployWebResourceCommandLine(file,_commandLine.ConnectionName);
                var task = new DeployWebResourceTask(commandLine, _reader,new WebResourceQuery(),new WebResourceMetaData(_writer,_reader));
                task.Execute(context);
            }
            context.Configuration.AppSettings["lastmodified"] = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }
    }
}