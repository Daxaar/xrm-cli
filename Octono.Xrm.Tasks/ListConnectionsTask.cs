namespace Octono.Xrm.Tasks
{
    public class ListConnectionsTask : IXrmTask
    {
        public void Execute(IXrmTaskContext context)
        {
            context.Log.Write("\n--------------------------------------------------",false);
            context.Log.Write("The following connections are available:\n",false);
            
            foreach (var connectionInfo in context.Configuration.ConnectionStrings)
            {
                context.Log.Write("Name: " + connectionInfo.Key + "\tUrl: " + connectionInfo.Value.ConnectionString,false);
            }
            context.Log.Write("--------------------------------------------------\n",false);
        }
    }
}