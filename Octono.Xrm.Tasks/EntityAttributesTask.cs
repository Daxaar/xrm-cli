using System;
using System.Linq;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Octono.Xrm.Tasks.Utils;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Lists the attribute names of an entity
    /// </summary>
    [XrmTask(typeof(EntityAttributesCommandLine), "Attributes", Aliases = new[] {"attr","attributes"})]
    public class EntityAttributesTask : IXrmTask
    {
        private readonly EntityAttributesCommandLine _cmdLine;

        public EntityAttributesTask(EntityAttributesCommandLine cmdLine)
        {
            _cmdLine = cmdLine;
        }
        public void Execute(IXrmTaskContext context)
        {
            var service = context.ServiceFactory.Create(_cmdLine.ConnectionName);

            var req = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = _cmdLine.EntityName
            };

            var response  = service.Execute(req);

            var attributes = response.Results.Values.SelectMany(x =>
                    ((EntityMetadata) x).Attributes
                    .Select(y => new {
                        SchemaName = y.SchemaName + (y.IsPrimaryId == true ? " * (id)" : "") + (y.IsPrimaryName == true ? " * (name)" : ""),
                        Type = y.AttributeTypeName.Value.Replace("Type","")})
                    .OrderBy(a => a.SchemaName))
                .ToList();

            var longestSchemaName = attributes.Max(x => x.SchemaName.Length);
            var longestTypeName = attributes.Max(x => x.Type.Length);
            
            Console.WriteLine("|" + string.Join("",Enumerable.Repeat("=", longestSchemaName + longestTypeName + 5)) + "|");
            foreach (var attr in attributes.Where(x => x.SchemaName.Contains(_cmdLine.Filter)))
            {
                Console.WriteLine($"| {attr.SchemaName.PadRight(longestSchemaName)} | {attr.Type.PadRight(longestTypeName)} |");
            }
            Console.WriteLine("|" + string.Join("", Enumerable.Repeat("=", longestSchemaName + longestTypeName + 5)) + "|");
        }
    }
}