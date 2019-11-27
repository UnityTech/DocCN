using System.IO;
using UnityEditor;
using UnityEngine;

public class Export
{
    [MenuItem("Build/Export Package", priority = 101)]
    public static void ExportAsPackage()
    {
        var targetPath = EditorUtility.OpenFolderPanel(
            "Select Package Folder",
            null,
            null
        );
        var projectPath = System.Environment.CurrentDirectory;
        CopyFolder(
            new DirectoryInfo(Path.Combine(projectPath, "Assets", "Resources")),
            new DirectoryInfo(Path.Combine(targetPath, "Editor", "Resources"))
        );
        CopyFolder(
            new DirectoryInfo(Path.Combine(projectPath, "Assets", "Scripts")),
            new DirectoryInfo(Path.Combine(targetPath, "Editor", "Scripts"))
        );
        Debug.Log("Export succeed");
    }

    private static void CopyFolder(DirectoryInfo from, DirectoryInfo to)
    {
        if (to.Exists)
        {
            Directory.Delete(to.FullName, true);
        }

        Directory.CreateDirectory(to.FullName);
        foreach (var directoryInfo in @from.GetDirectories())
        {
            CopyFolder(directoryInfo, new DirectoryInfo(Path.Combine(to.FullName, directoryInfo.Name)));
        }

        foreach (var fileInfo in @from.GetFiles())
        {
            File.Copy(
                fileInfo.FullName,
                Path.Combine(to.FullName, fileInfo.Name),
                true
            );
        }
    }
}