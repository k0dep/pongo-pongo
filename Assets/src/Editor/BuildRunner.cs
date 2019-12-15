using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Building
{
    public class BuildRunner
    {
        [MenuItem("Build/Android AAB")]
        public static void RunAndroidAAB()
        {
            SetupBuilding();

            EditorUserBuildSettings.buildAppBundle = true;
            
            var report = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes,
                Path.GetFullPath(Path.Combine(Application.dataPath, "..",
                $"{PlayerSettings.productName.Replace(" ", "_")}_{PlayerSettings.bundleVersion}.aab")), BuildTarget.Android, BuildOptions.None);

            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new Exception("Build failed");
            }
        }

        [MenuItem("Build/Android APK")]
        public static void RunAndroidAPK()
        {
            SetupBuilding();

            EditorUserBuildSettings.buildAppBundle = false;

            var report = BuildPipeline.BuildPlayer(EditorBuildSettings.scenes,
                Path.GetFullPath(Path.Combine(Application.dataPath, "..",
                $"{PlayerSettings.productName.Replace(" ", "_")}_{PlayerSettings.bundleVersion}.apk")), BuildTarget.Android,
                    BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging);

            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new Exception("Build failed");
            }
        }

        static void SetupBuilding()
        {
            var version = Environment.GetEnvironmentVariable("BUILD_VERSION")
                          ?? GetVersionFromGit()
                          ?? PlayerSettings.bundleVersion;
            
            Debug.Log($"Version: {version}");

            var versionCode = Version.Parse(version);

            PlayerSettings.Android.bundleVersionCode = versionCode.Major * 1000 + versionCode.Minor * 100 + versionCode.Build;

            PlayerSettings.bundleVersion = version;
        }

        private static string GetVersionFromGit()
        {
            try
            {
                var process = new Process();
                var startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo.FileName = "git";
                startInfo.WorkingDirectory = Environment.CurrentDirectory;
                startInfo.Arguments = "describe --tags --always";
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                process.StartInfo = startInfo;
                process.Start();

                var output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();

                var data = output.Split('-');
                return $"{data[0]}.{data[1]}";
            }
            catch (Exception e)
            {
                Debug.LogError("Cant fetch version from git");
            }

            return null;
        }
    }
}