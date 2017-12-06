using System.Collections.Generic;
using System.Linq;

namespace Octono.Xrm.Tasks.Utils
{
    public static class CommandLineArgumentExtensions
    {
        public static IEnumerable<T> Next<T>(this IEnumerable<T> list)
        {
            return list.Skip(1).Take(1);
        }

        public static string ArgumentValue(this IEnumerable<string> list, string argument)
        {
            return list.SkipWhile(x => x.StartsWith(argument) == false).Next().SingleOrDefault();
        }
    }
}