using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace Octono.Xrm.Tasks.IO
{
    public interface IConfigurationManager
    {
        NameValueCollection AppSettings{get;}
        Dictionary<string,ServerConnection> ConnectionStrings { get; }
        void Save();
        void Add(string key, string value);
    }

    public class JsonConfigurationManager : IConfigurationManager
    {
        private readonly IFileReader _reader;
        private readonly IFileWriter _writer;
        private readonly XrmConfiguration _configuration;
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");

        public JsonConfigurationManager(IFileReader reader, IFileWriter writer)
        {
            _configuration = File.Exists(_filePath)
                                 ? JsonConvert.DeserializeObject<XrmConfiguration>(System.Text.Encoding.ASCII.GetString(reader.ReadAllBytes(_filePath)))
                                 : new XrmConfiguration();
            _reader = reader;
            _writer = writer;
        }

 
        public NameValueCollection AppSettings { get; private set; }
        public Dictionary<string,ServerConnection> ConnectionStrings { get { return _configuration.Connections ?? (_configuration.Connections = new Dictionary<string, ServerConnection>()); }}

        public void Save()
        {
            var file = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(_configuration));
            _writer.Write(file, _filePath);
        }

        public void Add(string key, string value)
        {
            _configuration.AppSettings.Add(key,value);
        }
    }

    public class XrmConfiguration
    {
        public Dictionary<string,ServerConnection> Connections { get; set; }
        public Dictionary<string, string> AppSettings { get; set; } 
    }
}