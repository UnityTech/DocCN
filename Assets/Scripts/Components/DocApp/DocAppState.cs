using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.DocZh.Pages;
using Unity.DocZh.Utility;
using Unity.DocZh.Utility.Json;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;
using Version = Unity.DocZh.Models.Json.Version;

namespace Unity.DocZh.Components
{
    public class DocAppState : State<DocApp>
    {
        private static readonly Dictionary<Regex, Tuple<Func<Dictionary<string, string>, Widget>, string[]>> Router;
        private static readonly Func<Widget> OnUnknownPath = () => new NotFoundPage();
        private string _currentPath = "/";
        private ScrollController _controller;
        private bool _overwriteUnknown;
        private Models.Json.Version _version;

        public Models.Json.Version version => _version;

        static DocAppState()
        {
            var pageBase = Configuration.Instance.pageBase;
            var rawRouter =
                new Dictionary<string, Func<Dictionary<string, string>, Widget>>
                {
                    [$"{pageBase}"] = @params => new LandingPage(),
                    [$"{pageBase}/Manual"] = @params => new ManualPage("UnityManual"),
                    [$"{pageBase}/Manual/:name"] = @params => new ManualPage(@params["name"]),
                    [$"{pageBase}/Scripting"] = @params => new ScriptingPage(""),
                    [$"{pageBase}/Scripting/:name"] = @params => new ScriptingPage(@params["name"]),
                    [$"{pageBase}/Search"] = @params => new SearchPage(),
                    [$"{pageBase}/Search/:type"] = @params => new SearchPage(),
                    [$"{pageBase}/Search/:type/:keyword"] = @params =>
                        new SearchPage(filterType: @params["type"].ToFilterType(), keyword: @params["keyword"]),
                    [$"{pageBase}/Search/:type/:keyword/:page"] = @params =>
                        new SearchPage(filterType: @params["type"].ToFilterType(), keyword: @params["keyword"],
                            page: int.Parse(@params["page"])),
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
            _version = null;
            var url =
                $"{Configuration.Instance.apiHost}/api/documentation/parse_version/v/2018.1";
            var request = UnityWebRequest.Get(url);
            var asyncOperation = request.SendWebRequest();
            asyncOperation.completed += operation =>
            {
                if (!mounted)
                {
                    return;
                }

                using (WindowProvider.of(context).getScope())
                {
                    var content = DownloadHandlerBuffer.GetContent(request);
                    var model = Version.FromJson(JsonValue.Parse(content));
                    setState(() => _version = model);
                }
            };
        }

        private void OnPathChanged(string path)
        {
            if (!mounted)
            {
                return;
            }

            using (WindowProvider.of(context).getScope())
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
            if (_version == null)
            {
                return new Container();
            }

            if (_overwriteUnknown)
            {
                page = OnUnknownPath();
            }
            else
            {
                foreach (var entry in Router)
                {
                    if (!entry.Key.IsMatch(_currentPath))
                    {
                        continue;
                    }

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

            var screenOverlay = new ScreenOverlay(
                child: new GestureDetector(
                    onTap: () => { FocusScope.of(context).requestFocus(new FocusNode()); },
                    child: page ?? OnUnknownPath()
                )
            );

            var defaultTextStyle = new DefaultTextStyle(
                child: screenOverlay,
                style: new TextStyle(
                    fontFamily: "PingFang"
                )
            );

            return defaultTextStyle;
        }
    }
}