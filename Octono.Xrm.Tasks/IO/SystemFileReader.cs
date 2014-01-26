using System;
using System.Collections.Generic;
using System.IO;

namespace Octono.Xrm.Tasks.IO
{
    public class SystemFileReader : IFileReader
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public IEnumerable<string> GetSolutionsInExportFolder()
        {
            return Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export"));
        }
    }
}