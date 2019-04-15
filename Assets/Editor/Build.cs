#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Build
{
    private static Action BuildFactory(string environment)
    {
        return () =>
        {
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = new[] {"Assets/Scenes/SampleScene.unity"},
                locationPathName = $"Builds/snapshot-{environment}",
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };


            //PlayerSettings.SetScriptingDefineSymbolsForGroup();

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            var summary = report.summary;

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

    [MenuItem("Build/Test")]
    public static void Test()
    {

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            ""
        );
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

    [MenuItem("Build/Build Local")]
    public static void BuildLocal()
    {
        BuildMethods["local"].Invoke();
    }

    [MenuItem("Build/Build Test")]
    public static void BuildTest()
    {
        BuildMethods["test"].Invoke();
    }

    [MenuItem("Build/Build Prd")]
    public static void BuildProduction()
    {
        BuildMethods["prd"].Invoke();
    }
}
#endif