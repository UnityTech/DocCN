using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocCN.Pages;
using DocCN.Utility;
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
            static DocAppState()
            {
                var rawRouter =
                    new Dictionary<string, Func<Dictionary<string, string>, Widget>>
                    {
                        ["/"] = @params => new LandingPage(),
                        ["/Manual/:name"] = @params => new ManualPage(@params["name"]),
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
                ObservableUtil.currentPath.OnChanged += OnPathChanged;
                _currentPath = ObservableUtil.currentPath.value;
                _controller = new ScrollController();
            }

            private void OnPathChanged(string path)
            {
                if (mounted)
                {
                    setState(() => _currentPath = path);
                }
            }

            public override void dispose()
            {
                base.dispose();
                _controller.dispose();
                ObservableUtil.currentPath.OnChanged -= OnPathChanged;
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

                var defaultTextStyle = new DefaultTextStyle(
                    child: page ?? OnUnknownPath(),
                    style: new TextStyle(
                        fontFamily: "PingFang"
                    )
                );

                return defaultTextStyle;
            }
        }
    }
}