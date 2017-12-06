using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace Octono.Xrm.Tasks
{
    public class AddConnectionCommandLine : CommandLine
    {
        private readonly IList<string> _args;

        public AddConnectionCommandLine(IList<string> args) : base(args)
        {
            _args = args;
        }

        public SecureString Password
        {
            get
            {

                var password = new SecureString();

                //Looping rather than a FirstOrDefault allows us to clear the unsecure string value of the user password from
                //the arg array before returning it as a secure string
                for (int i = 0; i < _args.Count; i++)
                {
                    if (_args[i].StartsWith("pwd:") || _args[i].StartsWith("password:") || _args[i].StartsWith("p:"))
                    {
                        password = DapiSecurePassword.ToSecureString(_args[i].Remove(0, _args[i].IndexOf(':')));
                        _args[i] = "";
                        break;
                    }
                }

                if(password.Length == 0) throw new ArgumentException("password cannot be empty");

                return password;
            }
        }

        public string UserName
        {
            get
            {
                var user = _args.FirstOrDefault(a => a.StartsWith("user:") || a.StartsWith("u:"));
                user = string.IsNullOrEmpty(user) == false ? user.Remove(0, 5) : string.Empty;

                if(string.IsNullOrEmpty(user)) throw new ArgumentException("username cannot be empty");

                return user;
            }
        }

        public string Name { get { return _args.Last(); }}

        public Uri Uri { get { return new Uri(_args[1]); } }

        public bool ShowHelp
        {
            get { return _args.Any(arg => arg.Contains("help")); }
        }
    }
}