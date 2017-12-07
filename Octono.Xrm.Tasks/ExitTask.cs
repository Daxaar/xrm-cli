namespace Octono.Xrm.Tasks
{
    public class ExitTask : IXrmTask
    {
        public void Execute(IXrmTaskContext context)
        {
            context.Log.Write("Exiting...");
        }
    }
}