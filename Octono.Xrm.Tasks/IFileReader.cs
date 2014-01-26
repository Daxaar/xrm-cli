using System.Collections.Generic;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Testable File Access
    /// </summary>
    public interface IFileReader
    {
        byte[] ReadAllBytes(string path);
        bool FileExists(string path);
        IEnumerable<string> GetSolutionsInExportFolder();
    }
}
