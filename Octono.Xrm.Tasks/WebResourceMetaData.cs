using System;
using System.IO;
using System.Linq;
using Octono.Xrm.Tasks.IO;

namespace Octono.Xrm.Tasks
{
    /// <summary>
    /// Stores metadata about downloaded webresources such as the actual webresource name when the filename
    /// doesn't match due to invalid filename characters in the webresource name 
    /// </summary>
    public class WebResourceMetaData : IWebResourceMetaData
    {
        private readonly IFileWriter _fileWriter;
        private readonly IFileReader _fileReader;

        private WebResourceMetaData(string webresourceName)
        {
            _webresourceName = webresourceName;
        }

        public WebResourceMetaData(IFileWriter fileWriter, IFileReader fileReader)
        {
            _fileWriter = fileWriter;
            _fileReader = fileReader;
        }

        private readonly string _webresourceName;
        public string WebResourceName { get { return _webresourceName; } }
        public WebResourceMetaData Load(string filePath)
        {
            var path = string.Concat(filePath, ".meta");
            if (!_fileReader.Exists(path))
            {
                throw new ArgumentException(string.Format("Metadata file does not exist for file {0}", filePath),filePath);
            }

            var data = _fileReader.ReadLines(path).ToList();

            if (data.Count == 0)
            {
                throw new InvalidDataException(string.Format("Metadata file was empty {0}", filePath));
            }

            return new WebResourceMetaData(data.FirstOrDefault());
        }

        public void Create(string name, string path)
        {
            _fileWriter.Write(name,path + ".meta");
        }
    }

    public interface IWebResourceMetaData
    {
        WebResourceMetaData Load(string filePath);
        void Create(string name, string path);
    }
}