using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
//Original author: https://fargesportfolio.com/unity-generic-auto-build/
/// 
/// Put me inside an Editor folder
/// 
/// Add a Build menu on the toolbar to automate multiple build for different platform
/// 
/// Use #define BUILD in your code if you have build specification 
/// Specify all your Target to build All
/// 
/// Install to Android device using adb install -r "pathofApk"
/// 
public class GenericBuildCommand : MonoBehaviour
{
    const string androidKeystorePass = "";
    const string androidKeyaliasName = "";
    const string androidKeyaliasPass = "";

    static BuildTarget[] targetToBuildAll =
    {
        BuildTarget.Android,
        BuildTarget.StandaloneWindows64
    };

    public static string ProductName
    {
        get
        {
            return PlayerSettings.productName;
        }
    }

    private static string BuildPathRoot
    {
        get
        {
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "1Projects", "UnityProjects", "Builds");
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            return path;
        }
    }

    static int AndroidLastBuildVersionCode
    {
        get
        {
            return PlayerPrefs.GetInt("LastVersionCode", -1);
        }
        set
        {
            PlayerPrefs.SetInt("LastVersionCode", value);
        }
    }

    static BuildTargetGroup ConvertBuildTarget(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {

            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneLinux64:
                return BuildTargetGroup.Standalone;
            case BuildTarget.Android:
                return BuildTargetGroup.Android;
            case BuildTarget.WebGL:
                return BuildTargetGroup.WebGL;

            default:
                return BuildTargetGroup.Standalone;
        }
    }
    static string GetExtension(BuildTarget buildTarget)
    {
        switch (buildTarget)
        {

            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return ".exe";

            case BuildTarget.Android:
                return ".apk";

            case BuildTarget.WebGL:
                break;

            default:
                break;
        }

        return ".unknown";
    }

    static BuildPlayerOptions GetDefaultPlayerOptions()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();

        List<string> listScenes = new List<string>();
        foreach (var s in EditorBuildSettings.scenes)
        {
            if (s.enabled)
                listScenes.Add(s.path);
        }

        buildPlayerOptions.scenes = listScenes.ToArray();
        buildPlayerOptions.options = BuildOptions.None;
        return buildPlayerOptions;
    }

    static void DefaultBuild(BuildTarget buildTarget)
    {
        BuildTargetGroup targetGroup = ConvertBuildTarget(buildTarget);

        string path = Path.Combine(Path.Combine(BuildPathRoot, targetGroup.ToString()), ProductName + "_" + System.DateTime.Now.ToString("dd.MM.yyyy_HH") + "h" + System.DateTime.Now.ToString("mm") + "_" + buildTarget);
        string name = ProductName + GetExtension(buildTarget);

        PlayerSettings.Android.useCustomKeystore = false;
        // PlayerSettings.Android.keystorePass = androidKeystorePass;
        // PlayerSettings.Android.keyaliasName = androidKeyaliasName;
        // PlayerSettings.Android.keyaliasPass = androidKeyaliasPass;

        BuildPlayerOptions buildPlayerOptions = GetDefaultPlayerOptions();

        buildPlayerOptions.locationPathName = Path.Combine(path, name);
        buildPlayerOptions.target = buildTarget;

        EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, buildTarget);

        string result = buildPlayerOptions.locationPathName + ": " + BuildPipeline.BuildPlayer(buildPlayerOptions);
        Debug.Log(result);

        if (buildTarget == BuildTarget.Android)
            AndroidLastBuildVersionCode = PlayerSettings.Android.bundleVersionCode;

        UnityEditor.EditorUtility.RevealInFinder(path);
    }

    [MenuItem("Build/Build Specific/Build Android")]
    static void BuildAndroid()
    {
        DefaultBuild(BuildTarget.Android);
    }


    [MenuItem("Build/Build Specific/Build Win64")]
    static void BuildWin64()
    {
        DefaultBuild(BuildTarget.StandaloneWindows64);
    }

}