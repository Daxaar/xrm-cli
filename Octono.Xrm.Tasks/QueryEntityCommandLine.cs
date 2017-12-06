using System;
using System.Collections.Generic;
using System.Linq;
using Octono.Xrm.Tasks.Utils;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// xrm query contact column1,column2 n:10 connection-name
    /// </summary>
    public class QueryEntityCommandLine : CommandLine
    {
        public QueryEntityCommandLine(IList<string> args) : base(args)
        {
            if(args.Count <= 1) throw new ArgumentException("Must specify at least entity name and connection name arguments");
            EntityName = args[1];
            Columns = args[2].Split(',').ToList();
            if (Columns.Any() == false)
            {
                throw new ArgumentException("You must specify at last one column name");
            }

            //Quality of life fix to allow you to specify the id column for the entity as simply id in the field list rather than
            //the full name of logicalnameid
            if (Columns.Any(x => x == "id"))
            {
                Columns.Remove("id");
                Columns.Add($"{EntityName}id");
            }

            var rows = args.ArgumentValue("-n");
            if (rows != null)
            {
                Rows = Convert.ToInt32(rows);
            }

        }

        public int Rows { get; set; } = 1000;

        public List<string> Columns { get; set; }

        public string EntityName { get; set; }
    }
}