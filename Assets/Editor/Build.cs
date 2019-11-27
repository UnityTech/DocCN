using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

#if UNITY_EDITOR
public class Build
{
    private static Action BuildFactory(string environment)
    {
        return () =>
        {
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] {"Assets/Scenes/MainScene.unity"},
                locationPathName = $"Builds/snapshot-{environment}",
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };


            var oldSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                BuildTargetGroup.WebGL
            );

            var newSymbols = oldSymbolsForGroup.Split(';').ToList();
            newSymbols.Add($"DOC_BUILD_{environment.ToUpper()}");

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                BuildTargetGroup.WebGL,
                string.Join(";", newSymbols.Distinct())
            );

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, oldSymbolsForGroup);
            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
                    break;
                case BuildResult.Failed:
                    Debug.Log("Build failed");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        };
    }

    static Build()
    {
        BuildMethods = new Dictionary<string, Action>();
        foreach (var s in new[] {"local", "test", "prd"})
        {
            BuildMethods[s] = BuildFactory(s);
        }
    }

    private static readonly Dictionary<string, Action> BuildMethods;

    [MenuItem("Build/Build Local", priority = 1)]
    public static void Build_local()
    {
        BuildMethods["local"].Invoke();
    }

    [MenuItem("Build/Build Test", priority = 2)]
    public static void Build_test()
    {
        BuildMethods["test"].Invoke();
    }

    [MenuItem("Build/Build Prd", priority = 3)]
    public static void Build_prd()
    {
        BuildMethods["prd"].Invoke();
    }
}
#endif