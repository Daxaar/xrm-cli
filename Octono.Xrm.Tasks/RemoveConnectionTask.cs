using System;
using System.Collections.Generic;

namespace Octono.Xrm.Tasks
{
    public class RemoveConnectionTask : IXrmTask
    {
        private readonly IList<string> _args;

        public RemoveConnectionTask(IList<string> args)
        {
            _args = args;
        }

        public void Execute(IXrmTaskContext context)
        {
            if(_args.Count != 2)
                throw new InvalidOperationException("You must specify the name of the connection to remove.");

            if (context.Configuration.ConnectionStrings.ContainsKey(_args[1]))
            {
                context.Configuration.ConnectionStrings.Remove(_args[1]);                
            }
            else
            {
                context.Log.Write(string.Format("A connection with the name {0} was not found",_args[1]));
            }
            
        }
    }
}