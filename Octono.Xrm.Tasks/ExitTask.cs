namespace Octono.Xrm.Tasks
{
    public class ExitTask : IXrmTask
    {
        private readonly ILog _logger;

        public ExitTask(ILog logger)
        {
            _logger = logger;
        }

        public void Execute()
        {
            _logger.Write("Exiting...");
        }
    }
}