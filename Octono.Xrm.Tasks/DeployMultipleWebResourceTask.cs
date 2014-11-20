using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Octono.Xrm.Tasks;
using Octono.Xrm.Tasks.Crm;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    public class CopyRecordsTask : XrmTask
    {
        private readonly CopyRecordsCommandLine _commandLine;

        public CopyRecordsTask(CopyRecordsCommandLine commandLine)
        {
            _commandLine = commandLine;
        }

        public override void Execute(IXrmTaskContext context)
        {
            IOrganizationService service = context.ServiceFactory.Create(_commandLine.ConnectionName);
            using (var crmContext = new CrmContext(service))
            using (var destinationContext = new CrmContext(context.ServiceFactory.Create(_commandLine.Destination)))
            {
                foreach (var entity in _commandLine.Entities)
                {
                    var sourceRecords = crmContext.GetAll(entity.Key)
                                                  .Where(
                                                      e =>
                                                      e.GetAttributeValue<DateTime>("modifiedon") >=
                                                      _commandLine.FromDate);

                    var targetRecords = destinationContext.GetAll(entity.Key);

                    var sync = new EntitySync(sourceRecords, targetRecords, entity.Value);

                    destinationContext.Delete(sync.ToBeDeleted);
                    destinationContext.Add(sync.ToBeAdded);
                }
            }
        }
    }

    public class CopyRecordsCommandLine : CommandLine
    {
        public CopyRecordsCommandLine(IList<string> args) : base(args)
        {
        }

        public IDictionary<string, string> Entities { get; private set; }

        public DateTime FromDate { get; set; }

        public string Destination { get; set; }
    }

    public class DeployMultipleWebResourceTask : XrmTask
    {
        private readonly DeployWebResourceCommandLine _commandLine;
        private readonly IFileReader _reader;

        public DeployMultipleWebResourceTask(DeployWebResourceCommandLine commandLine, IFileReader reader)
        {
            _commandLine = commandLine;
            _reader = reader;
        }

        public override void Execute(IXrmTaskContext context)
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
                var task = new DeployWebResourceTask(commandLine, _reader);
                task.Execute(context);
            }
            context.Configuration.AppSettings["lastmodified"] = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }
    }
}