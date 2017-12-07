using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Octono.Xrm.Tasks.IO
{
    public class JsonConfigurationManager : IConfigurationManager
    {
        private class JsonConfiguration : IXrmConfiguration
        {
            public JsonConfiguration()
            {
                ConnectionStrings = new Dictionary<string, ConnectionInfo>();
                AppSettings = new Dictionary<string, string>();
            }
            public Dictionary<string, string> AppSettings { get; }
            public Dictionary<string, ConnectionInfo> ConnectionStrings { get; }
        }

        private readonly IFileReader _reader;
        private readonly IFileWriter _writer;
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

        public JsonConfigurationManager(IFileReader reader, IFileWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }

        public void Save(IXrmConfiguration configuration)
        {
            var file = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(configuration));
            _writer.Write(file, _filePath);
        }

        public IXrmConfiguration Load()
        {
            if (File.Exists(_filePath))
            {
                var config = Encoding.ASCII.GetString(_reader.ReadAllBytes(_filePath));
                return JsonConvert.DeserializeObject<JsonConfiguration>(config);
            }
            return new JsonConfiguration();
        }
    }
}