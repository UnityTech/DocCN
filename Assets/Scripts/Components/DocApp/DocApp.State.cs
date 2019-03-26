using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniRx;
using DocCN.Page;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace DocCN.Components
{
    public partial class DocApp
    {
        private class DocAppState : State<DocApp>
        {
            private static readonly Dictionary<Regex, Tuple<Func<Dictionary<string, string>, Widget>, string[]>> Router;
            private static readonly Func<Widget> OnUnknownPath = () => new NotFoundPage();
            private string _currentPath = "/";
            private ScrollController _controller;
            private IDisposable _subscription;

            static DocAppState()
            {
                var rawRouter =
                    new Dictionary<string, Func<Dictionary<string, string>, Widget>>
                    {
                        ["/"] = @params => new LandingPage(),
                        ["/Manual/:name"] = @params => new DocumentPage(@params["name"]),
                        ["/Scripting/:name"] = @params => new ScriptingPage(@params["name"]),
                        ["/Search"] = @params => new SearchPage()
                    };
                Router = new Dictionary<Regex, Tuple<Func<Dictionary<string, string>, Widget>, string[]>>();
                foreach (var entry in rawRouter)
                {
                    var regex = entry.Key.CompileAsRouter(out var parameterNames);
                    Router[regex] =
                        new Tuple<Func<Dictionary<string, string>, Widget>, string[]>(entry.Value, parameterNames);
                }
            }

            public override void initState()
            {
                base.initState();
                _subscription = Reactive.CurrentPath.Subscribe(path =>
                {
                    setState(() => _currentPath = path);
                });
                _controller = new ScrollController();
            }

            public override void dispose()
            {
                base.dispose();
                _subscription.Dispose();
            }

            public override Widget build(BuildContext buildContext)
            {
                Widget page = null;
                foreach (var entry in Router)
                {
                    if (!entry.Key.IsMatch(_currentPath)) continue;
                    var match = entry.Key.Match(_currentPath);
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
                    page = OnUnknownPath();
                }

                var defaultTextStyle = new DefaultTextStyle(
                    child: page,
                    style: new TextStyle(
                        fontFamily: "PingFang-Regular"
                    )
                );

                return defaultTextStyle;
            }
        }
    }
}