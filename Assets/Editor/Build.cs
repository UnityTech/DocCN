#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Build
{
    [MenuItem("Build/Build WebGL")]
    public static void MyBuild()
    {
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] {"Assets/Scenes/SampleScene.unity"},
            locationPathName = "Builds/snapshot",
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

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
        }
    }
}
#endif