namespace Xrm
{
    public class XrmTaskRunner
    {
        private readonly IXrmTaskFactory _factory;
        private readonly ILog _log;

        public XrmTaskRunner(IXrmTaskFactory xrmTaskFactory, ILog log)
        {
            _factory = xrmTaskFactory;
            _log = log;
        }

        public void Run(string[] args)
        {
            var task = _factory.CreateTask(args);
            task.Execute();                
        }
    }
}