using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Octono.Xrm.Tasks.Crm;
using Octono.Xrm.Tasks.Utils;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Deploys one or more web resource to an Organisation
    /// </summary>
    [XrmTask(typeof(CopyRecordsCommandLine),"Copy",Aliases = new []{"copy","cp"})]
    public class CopyRecordsTask : IXrmTask
    {
        private readonly CopyRecordsCommandLine _commandLine;

        public CopyRecordsTask(CopyRecordsCommandLine commandLine)
        {
            _commandLine = commandLine;
        }

        public void Execute(IXrmTaskContext context)
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
}