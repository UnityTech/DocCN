using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DocCN.Pages;
using DocCN.Utility;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace DocCN.Components
{
    public class DocAppState : State<DocApp>
    {
        private static readonly Dictionary<Regex, Tuple<Func<Dictionary<string, string>, Widget>, string[]>> Router;
        private static readonly Func<Widget> OnUnknownPath = () => new NotFoundPage();
        private string _currentPath = "/";
        private ScrollController _controller;
        private bool _overwriteUnknown;

        static DocAppState()
        {
            var pageBase = Configuration.Instance.pageBase;
            var rawRouter =
                new Dictionary<string, Func<Dictionary<string, string>, Widget>>
                {
                    [$"{pageBase}/"] = @params => new LandingPage(),
                    [$"{pageBase}/Manual/:name"] = @params => new ManualPage(@params["name"]),
                    [$"{pageBase}/Scripting"] = @params => new ScriptingPage(""),
                    [$"{pageBase}/Scripting/"] = @params => new ScriptingPage(""),
                    [$"{pageBase}/Scripting/:name"] = @params => new ScriptingPage(@params["name"]),
                    [$"{pageBase}/Search"] = @params => new SearchPage(),
                    [$"{pageBase}/Search/"] = @params => new SearchPage(),
                    [$"{pageBase}/Search/:keyword"] = @params => new SearchPage(keyword: @params["keyword"]),
                    [$"{pageBase}/Search/:keyword/:page"] = @params => new SearchPage(keyword: @params["keyword"], page: int.Parse(@params["page"])),
                };
            Router = new Dictionary<Regex, Tuple<Func<Dictionary<string, string>, Widget>, string[]>>();
            foreach (var entry in rawRouter)
            {
                var regex = entry.Key.CompileAsRouter(out var parameterNames);
                Router[regex] =
                    new Tuple<Func<Dictionary<string, string>, Widget>, string[]>(entry.Value, parameterNames);
            }
        }

        public void ForceUnknown()
        {
            if (mounted)
            {
                setState(() => _overwriteUnknown = true);
            }
        }

        public override void initState()
        {
            base.initState();
            ObservableUtil.currentPath.OnChanged += OnPathChanged;
            _currentPath = ObservableUtil.currentPath.value;
            _controller = new ScrollController();
            _overwriteUnknown = false;
        }

        private void OnPathChanged(string path)
        {
            if (mounted)
            {
                setState(() =>
                {
                    _overwriteUnknown = false;
                    _currentPath = path;
                });
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
            if (_overwriteUnknown)
            {
                page = OnUnknownPath();
            }
            else
            {
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