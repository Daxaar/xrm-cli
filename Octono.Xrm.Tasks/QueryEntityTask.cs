using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk.Query;
using Octono.Xrm.Tasks.Utils;

namespace Octono.Xrm.Tasks
{
    [XrmTask(typeof(QueryEntityCommandLine),"Query",Aliases = new [] {"q"})]
    public class QueryEntityTask : IXrmTask
    {
        private readonly QueryEntityCommandLine _cmdLine;

        public QueryEntityTask(QueryEntityCommandLine cmdLine)
        {
            _cmdLine = cmdLine;
        }
        public void Execute(IXrmTaskContext context)
        {
            var service = context.ServiceFactory.Create(_cmdLine.ConnectionName);

            var query = new QueryExpression(_cmdLine.EntityName);

            if (_cmdLine.Columns.First() == "*")
            {
                query.ColumnSet = new ColumnSet(true);
            }
            else
            {
                query.ColumnSet.AddColumns(_cmdLine.Columns.ToArray());
            }
            var result = service.RetrieveMultiple(query);
            var entities = result.Entities.Take(_cmdLine.Rows).ToList();
            var cols = new Dictionary<string, int>();

            var allAttributes = entities.SelectMany(x => x.Attributes);

            var header = "";
            //Get the max col size for each column
            foreach (var attribute in allAttributes)
            {
                //if (!_cmdLine.IncludeIdColumn && attribute.Key == _cmdLine.EntityName + "id") continue;

                var maxLength = attribute.Value.ToString().Length;

                if (maxLength < attribute.Key.Length)
                {
                    //Set the max length to the column name length in case none of the row values exceed the column name length
                    maxLength = attribute.Key.Length;
                }

                if (cols.ContainsKey(attribute.Key) == false)
                {
                    cols.Add(attribute.Key, maxLength+1);
                }
                else if (cols[attribute.Key] < maxLength)
                {
                    cols[attribute.Key] = maxLength+1;
                }
            }
            var tableWidth = cols.Sum(x => x.Value)+5;

            Console.WriteLine("".PadRight(tableWidth,'='));

            foreach (var col in cols)
            {
                header += $"| {col.Key.PadRight(col.Value)}";
            }
            Console.WriteLine(header + "|");
            Console.WriteLine("".PadRight(tableWidth, '-'));
            foreach (var entity in entities)
            {
                string line = "";
                foreach (var attribute in entity.Attributes)
                {
                    //if(!_cmdLine.IncludeIdColumn && attribute.Key == _cmdLine.EntityName + "id") continue;
                    var value = attribute.Value.ToString();
                    var columnWidth = cols[attribute.Key];

                    if (attribute.Value.ToString().Length < cols[attribute.Key])
                    {
                        value = value.PadRight(columnWidth);
                    }
                    line += $"| {value}";
                }
                Console.WriteLine(line + "|");
            }
            Console.WriteLine("".PadRight(tableWidth, '='));
        }
    }
}