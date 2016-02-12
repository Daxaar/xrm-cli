using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public string RemoveFileExtension(string filename)
        {
            if(string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename","Cannot be null");
            
            string extension = Path.GetExtension(filename);
            return extension != null ? filename.Replace(extension, "") : filename;
        }

        public IEnumerable<string> GetModifiedFilesSince(DateTime lastModified, string path, bool recursive = false)
        {
            return Directory.GetFiles(path).Where(file => Directory.GetLastAccessTime(file) > lastModified);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public IEnumerable<string> ReadLines(string path)
        {
            return File.ReadLines(path);
        }
    }
}