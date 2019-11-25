using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Unity.DocZh.Components
{
    internal static partial class StringExtension
    {
        public static Regex CompileAsRouter(this string sz, out string[] parameterNames)
        {
            var regex = new Regex(@":\w+");
            var matchCollection = regex.Matches(sz);
            var parameterSet = new HashSet<string>();
            for (var i = 0; i < matchCollection.Count; ++i)
            {
                parameterSet.Add(matchCollection[i].Value);
            }

            var routerRegexSz = sz.Replace(@"\", @"\\");
            var parameterNameList = new List<string>();
            foreach (var parameter in parameterSet)
            {
                var parameterName = parameter.Substring(1);
                parameterNameList.Add(parameterName);
                routerRegexSz = routerRegexSz.Replace(parameter, $@"(?<{parameterName}>[a-zA-Z0-9_\-+%\.\$@\(\)]+)");
            }

            parameterNames = parameterNameList.ToArray();

            return new Regex($"^{routerRegexSz}/?$");
        }
    }
}