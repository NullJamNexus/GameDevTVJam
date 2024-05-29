using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using AdvancedSceneManager.Editor.Utility;
using AdvancedSceneManager.Models;
using AdvancedSceneManager.Setup;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Environment = System.Environment;
using Object = UnityEngine.Object;

namespace AdvancedSceneManager.Editor.UI
{

    partial class SceneManagerWindow
    {

        static class Icons
        {

            public const string UpdateNotNeeded = "";
            public const string UpdateAvailable = "";
            public const string UpdateIsChecking = "";
            public const string UpdateIsTooOutOfDate = "X";

            public const string DebugEnabled = "";
            public const string DebugDisabled = "";

        }

        static class Urls
        {
            public const string AssetStoreReview = "https://assetstore.unity.com/packages/tools/utilities/advanced-scene-manager-174152#reviews";
            public const string GithubIssue = "https://github.com/Lazy-Solutions/AdvancedSceneManager/issues/new";
            public const string Discord = "https://discord.gg/pnRn6zeFEJ";
            public const string GithubDocs = "https://github.com/Lazy-Solutions/AdvancedSceneManager/blob/2.0";
            public const string GithubReleases = "https://github.com/Lazy-Solutions/AdvancedSceneManager/releases/latest";
            public const string GithubPatchFile = "https://gist.githubusercontent.com/Zumwani/195afd3053cf1cb951013e30908903c0/raw";
            public const string Examples = "https://github.com/Lazy-Solutions/AdvancedSceneManager/blob/main/examples.md";
        }

        class MenuPopup : ViewModel, IPopup
        {

            public override void OnCreateGUI(VisualElement element)
            {
                InitializeUpdate(element);
                InitializeBuild(element);
                InitializeDocs(element);
                InitializeReview(element);
            }

            ASMUserSettings settings => SceneManager.settings.user;

            #region Update

            bool isCheckingForUpdates;
            void InitializeUpdate(VisualElement element)
            {

                element.Q<Label>("text-version").text = ASMInfo.GetVersionInfo().version;
                SetupLink(element.Q<Button>("button-view-updates"), Urls.GithubReleases);

                var updateButton = element.Q<Button>("button-update");
                updateButton.clicked += async () =>
                {
                    await CheckUpdate(false, true);
                    if (HasUpdateCached())
                        Update();
                };

                _ = CheckUpdate(true);
                async Task CheckUpdate(bool isAuto, bool forceCheck = false)
                {

                    if (isCheckingForUpdates)
                        return;

                    isCheckingForUpdates = true;
                    UpdateUpdateButton();

                    if (forceCheck || !HasUpdateCached())
                        await DoCheckUpdate(isAuto);

                    isCheckingForUpdates = false;
                    UpdateUpdateButton();

                }

                void UpdateUpdateButton()
                {

                    if (isCheckingForUpdates)
                    {

                        updateButton.pickingMode = PickingMode.Ignore;

                        updateButton.text = Icons.UpdateIsChecking;
                        updateButton.tooltip = "Currently checking for updates...";
                        updateButton.RotateAnimation(speed: 8);

                    }
                    else
                    {

                        updateButton.pickingMode = PickingMode.Position;

                        var hasUpdate = HasUpdateCached();

                        if (settings.isTooOutOfDate)
                        {
                            updateButton.text = Icons.UpdateIsTooOutOfDate;
                            updateButton.tooltip = "Your ASM version is too old to receive updates, please install the latest asset store version. Patches only apply to newest asset store version, old ones are removed, as they are merged into asset store release.";
                        }
                        else if (hasUpdate)
                        {
                            updateButton.text = Icons.UpdateAvailable;
                            updateButton.tooltip = $"An update is available, click here to apply it.";
                        }
                        else
                        {
                            updateButton.text = Icons.UpdateNotNeeded;
                            updateButton.tooltip = "Your version of ASM is currently up to date. Click here to check again.";
                        }

                        updateButton.StopRotateAnimation();

                    }

                }

            }

            bool HasUpdateCached() =>
                !settings.isTooOutOfDate &&
                Version.TryParse(ASMInfo.GetVersionInfo().version, out var currentVersion) &&
                Version.TryParse(settings.patchVersion, out var patchVersion) &&
                patchVersion > currentVersion;

#if ASM_DEV
            static string actualPatchVersion;
#endif

            async Task DoCheckUpdate(bool isAuto)
            {

#if !ASM_DEV
                if (isAuto && (settings.lastPatchCheck?.value == default || DateTime.Now - settings.lastPatchCheck < TimeSpan.FromDays(1)))
                    return;
                settings.lastPatchCheck = DateTime.Now;
#endif

                try
                {

                    using var client = new HttpClient();
                    var text = await client.GetStringAsync(Urls.GithubPatchFile);

                    if (!text.Contains("\n"))
                        throw new Exception("Could not parse version file.");

                    var versionStr = text[..text.IndexOf("\n")];
                    var patchNotes = text[text.IndexOf("\n")..];

#if ASM_DEV
                    actualPatchVersion = versionStr;
                    versionStr = "2.0.9999";
#endif

                    if (!Version.TryParse(versionStr, out var version))
                        throw new Exception("Could not parse version file.");

                    if (!Version.TryParse(ASMInfo.GetVersionInfo().version, out var currentVersion))
                        throw new Exception("Could not retrieve current version.");

                    if (currentVersion < version && version.Minor > currentVersion.Minor)
                    {
                        settings.patchVersion = "";
                        settings.patchNotes = "";
                        settings.isTooOutOfDate = true;
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(settings.patchVersion) && settings.patchVersion != versionStr)
                            Debug.Log("ASM " + settings.patchVersion + " is available:" + settings.patchNotes);

                        settings.patchVersion = versionStr;
                        settings.patchNotes = patchNotes;
                        settings.isTooOutOfDate = false;

                    }

                    await Task.Delay(250);

                    element.Q<Label>("text-version").text = ASMInfo.GetVersionInfo().version;

                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

            }

            async void Update()
            {

                try
                {

                    var version = settings.patchVersion;
#if ASM_DEV
                    version = actualPatchVersion;
#endif

                    var url = $"https://github.com/Lazy-Solutions/AdvancedSceneManager/releases/download/{version}/AdvancedSceneManager.{version}.partial.unitypackage";

                    using var client = new HttpClient();
                    var stream = await client.GetStreamAsync(url);

                    var path = Path.Combine(Path.GetTempPath(), "ASM", "update");
                    if (!Directory.Exists(path))
                        _ = Directory.CreateDirectory(path);

                    path = Path.Combine(path, $"ASM.{version}.partial.unitypackage");
                    using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                    await stream.CopyToAsync(fs);

                    fs.Close();
                    stream.Close();

#if !ASM_DEV
                    AssetDatabase.ImportPackage(path, true);
#else
                    Debug.Log($"AssetDatabase.ImportPackage({path}, true);");
#endif

                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }

            }

            #endregion
            #region Build

            void InitializeBuild(VisualElement element)
            {

                InitializeFolderButton(element);
                InitializeProfilerButton(element);

                element.Q<Button>("button-build").clicked += () =>
                {
                    var path = GetBuildPath().Replace("%temp%", Path.GetTempPath());
                    BuildUtility.DoBuild(path + "/app.exe", attachProfiler: settings.quickBuildUseProfiler, true);
                };

            }

            string GetBuildPath() =>
                string.IsNullOrEmpty(settings.quickBuildPath)
                ? Path.Combine("%temp%", "ASM", "builds", Application.companyName, Application.productName)
                : settings.quickBuildPath;

            void InitializeFolderButton(VisualElement element)
            {

                var folderButton = element.Q<Button>("button-build-folder");
                folderButton.clicked += () =>
                {

                    var path = EditorUtility.OpenFolderPanel("Select folder to put builds in...", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), Application.productName);
                    settings.quickBuildPath = Directory.Exists(path) ? path : null;

                    UpdateFolderButton();

                };

                UpdateFolderButton();
                void UpdateFolderButton() =>
                    folderButton.tooltip = GetBuildPath();

            }

            void InitializeProfilerButton(VisualElement element)
            {

                var profilerButton = element.Q<Button>("button-build-profiler");

                profilerButton.clicked += () =>
                {
                    settings.quickBuildUseProfiler = !settings.quickBuildUseProfiler;
                    UpdateProfilerButton();
                };

                UpdateProfilerButton();
                void UpdateProfilerButton()
                {
                    profilerButton.text = settings.quickBuildUseProfiler ? Icons.DebugEnabled : Icons.DebugDisabled;
                    profilerButton.tooltip = settings.quickBuildUseProfiler ? "Profiler enabled. Press to disable." : "Profiler disabled. Press to enable.";
                }

            }

            #endregion
            #region Docs

            void InitializeDocs(VisualElement element)
            {

                SetupLink(element.Q<Button>("button-docs-online"), Urls.GithubDocs);

                var obj = FindLocalDocs();
                var folder = $"{ProjectWindowUtil.GetContainingFolder(AssetDatabase.GetAssetPath(obj))}/";

                var button = element.Q<Button>("button-docs-local");
                SetupTooltip(button, folder);

                button.clicked += () => UnityEditor.Selection.activeObject = obj;

            }

            Object FindLocalDocs()
            {
                var path = ASMInfo.GetFolder() + "/Documentation/_api-reference.pdf";
                return AssetDatabase.LoadAssetAtPath<Object>(path);
            }

            static void OpenDir(string folder)
            {
                var pt = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
                var ins = pt.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public).GetValue(null);
                var showDirMeth = pt.GetMethod("ShowFolderContents", BindingFlags.NonPublic | BindingFlags.Instance);
                showDirMeth.Invoke(ins, new object[] { folder, true });
            }

            #endregion
            #region Review

            void InitializeReview(VisualElement element)
            {
                SetupLink(element.Q<Button>("button-asset-store"), Urls.AssetStoreReview);
                SetupLink(element.Q<Button>("button-github"), Urls.GithubIssue);
                SetupLink(element.Q<Button>("button-discord"), Urls.Discord);
                SetupLink(element.Q<Button>("button-examples"), Urls.Examples);
            }

            #endregion

            void SetupLink(Button button, string url)
            {
                button.clicked += () => Application.OpenURL(url);
                SetupTooltip(button, url);
            }

            void SetupTooltip(Button button, string url)
            {
                if (EditorGUIUtility.isProSkin)
                    button.tooltip += "\n\n" + $"<color=#00ffffff>{url}</color>";
                else
                    button.tooltip += "\n\n" + $"<color=#00008B>{url}</color>";
            }
        }

    }

}
