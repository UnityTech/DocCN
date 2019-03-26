using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocCN.Page;
using UniRx;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN.Components
{
    public partial class DocApp : StatefulWidget
    {
        public DocApp(Key key = null) : base(key)
        {
        }

        public override State createState() => new DocAppState();
    }

    internal static class StringExtension
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
                routerRegexSz = routerRegexSz.Replace(parameter, $@"(?<{parameterName}>[a-zA-Z0-9\.\$@\(\)]+)");
            }

            parameterNames = parameterNameList.ToArray();

            return new Regex($"^{routerRegexSz}$");
        }
    }

    
}