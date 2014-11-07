namespace Octono.Xrm.Tasks
{
    public class ExitTask : XrmTask
    {
        public ExitTask() : base(false){}
        public override void Execute(IXrmTaskContext context)
        {
            context.Log.Write("Exiting...");
        }

    }
}