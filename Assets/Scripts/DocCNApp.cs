using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocCN.Page;
using UniRx;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using UnityEngine;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace DocCN
{
    public class DocCNApp : StatefulWidget
    {
        public DocCNApp(Key key = null) : base(key)
        {
        }

        public override State createState() => new DocCNAppState();
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

    internal class DocCNAppState : State<DocCNApp>
    {
        private static readonly Dictionary<Regex, Tuple<Func<Dictionary<string, string>, Widget>, string[]>> router;

        private readonly Func<Widget> onUnknownPath = () => new NotFoundPage();

        static DocCNAppState()
        {
            var rawRouter =
                new Dictionary<string, Func<Dictionary<string, string>, Widget>>
                {
                    ["/"] = @params => new LandingPage(),
                    ["/Manual/:name"] = @params => new DocumentPage(@params["name"]),
                    ["/Search"] = @params => new SearchPage()
                };
            router = new Dictionary<Regex, Tuple<Func<Dictionary<string, string>, Widget>, string[]>>();
            foreach (var entry in rawRouter)
            {
                var regex = entry.Key.CompileAsRouter(out var parameterNames);
                router[regex] =
                    new Tuple<Func<Dictionary<string, string>, Widget>, string[]>(entry.Value, parameterNames);
            }
        }

        private string currentPath = "/";
        private ScrollController controller;
        private IDisposable subscription;

        public override void initState()
        {
            base.initState();
            subscription = Reactive.CurrentPath.Subscribe(onNext: (path) =>
            {
                setState(() => this.currentPath = path);
            });
            controller = new ScrollController();
        }

        public override void dispose()
        {
            base.dispose();
            subscription.Dispose();
        }

        public override Widget build(BuildContext context)
        {
            Widget page = null;
            foreach (var entry in router)
            {
                if (!entry.Key.IsMatch(currentPath)) continue;
                var match = entry.Key.Match(currentPath);
                var arguments = new Dictionary<string, string>();
                foreach (var parameterName in entry.Value.Item2)
                {
                    arguments[parameterName] = match.Groups[parameterName].Value;
                }

                page = entry.Value.Item1.Invoke(arguments);

                break;
            }

            if (page == null)
            {
                page = onUnknownPath();
            }

            var defaultTextStyle = new DefaultTextStyle(
                child: page,
                style: new TextStyle(
                    fontFamily: "PingFang SC"
                )
            );

            return defaultTextStyle;
        }
    }
}