using Object = UnityEngine.Object;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using AdvancedSceneManager.Utility;
using AdvancedSceneManager.Models.Utility;

#if UNITY_EDITOR
using AdvancedSceneManager.Editor.Utility;
using UnityEditor;
#endif

namespace AdvancedSceneManager.Models.Internal
{

#if UNITY_EDITOR
    class AssetsPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            if (SceneManager.isInitialized && !didDomainReload)
                Assets.RegenerateSingletons();
        }
    }
#endif

    /// <summary>Manages all ASM assets.</summary>
    [InitializeInEditor]
    static class Assets
    {

        #region Initialize

#if UNITY_EDITOR

        static Assets()
        {
            BuildUtility.preBuild += (e) => m_fallbackScenePath = fallbackScenePath;
            SceneManager.OnInitialized(() =>
            {
                RegenerateSingletons();
                ImportASMScenes();
            });
        }

        internal static void RegenerateSingletons()
        {
            SceneManager.OnInitialized(() =>
            {
                GenerateSceneHelper();
                GenerateFallbackScene();
            });
        }

        #region Scene helper

        static void GenerateSceneHelper()
        {

            if (!SceneManager.app.isInstalled)
                return;

            if (sceneHelper)
                return;

            var asset = AssetDatabase.LoadAssetAtPath<ASMSceneHelper>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:SceneHelper").FirstOrDefault()));
            if (!asset)
            {
                asset = ScriptableObject.CreateInstance<ASMSceneHelper>();
                AssetDatabase.CreateAsset(asset, "Assets/AdvancedSceneManager/SceneHelper.asset");
            }

            m_sceneHelper = asset;
            Save();

        }

        #endregion
        #region Fallback scene

        public static SceneAsset fallbackScene { get; private set; }

        internal static void GenerateFallbackScene()
        {

            if (!SceneManager.app.isInstalled)
                return;

            if (EditorUtility.IsPersistent(fallbackScene))
            {
                MakeSureCorrectPath();
                return;
            }

            if (!LoadAny(out var scene))
                if (Create(out scene))
                    fallbackScene = scene;
                else
                    throw new Exception("Default scene could not be generated.");

            fallbackScene = scene;
            MakeSureCorrectPath();

            bool LoadAny(out SceneAsset asset) =>
                Load(m_fallbackScenePath, out asset) ||
                Load($"Assets/AdvancedSceneManager/{FallbackSceneUtility.Name}.unity", out asset) ||
                Load(typeof(SceneAsset).FindAssetPaths().FirstOrDefault(s => s.EndsWith($"/{FallbackSceneUtility.Name}.unity")), out asset);

            bool Load(string path, out SceneAsset asset) =>
                asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

            bool Create(out SceneAsset asset)
            {
                var path = $"Assets/AdvancedSceneManager/{FallbackSceneUtility.Name}.unity";
                SceneUtility.Create(path);
                return Load(path, out asset);
            }

            void MakeSureCorrectPath()
            {

                var path = AssetDatabase.GetAssetPath(fallbackScene);
                if (m_fallbackScenePath != path)
                {
                    m_fallbackScenePath = path;
                    Save();
                }

            }

        }

        #endregion
        #region ASM scenes

        static void ImportASMScenes()
        {

            if (!SceneManager.app.isInstalled)
                return;

            var folder = "Assets/AdvancedSceneManager/Defaults";
            var importedFolder = folder + "/Imported";

#if ASM_DEV
            var scenesToImport = SceneManager.assets.defaults.EnumeratePaths().Where(NotImported).ToArray();
            SceneImportUtility.Import(scenesToImport, importedFolder, useNameAsID: true);
#endif

            if (AssetDatabase.IsValidFolder(importedFolder))
            {
                var scenes = AssetDatabase.FindAssets(Scene.AssetSearchString, new[] { importedFolder }).Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<Scene>).NonNull();
                Add(scenes, importedFolder);
            }

        }

        static bool NotImported(string path)
        {
            var obj = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            return !obj.ASMScene();
        }

        #endregion

#endif

        #endregion
        #region Properties

        #region Internal

        static List<Profile> m_profiles
        {
            get => ASMSettings.instance.m_profiles;
            set => ASMSettings.instance.m_profiles = value;
        }

        static List<Scene> m_scenes
        {
            get => ASMSettings.instance.m_scenes;
            set => ASMSettings.instance.m_scenes = value;
        }

        static List<SceneCollection> m_collections
        {
            get => ASMSettings.instance.m_collections;
            set => ASMSettings.instance.m_collections = value;
        }

        static List<SceneCollectionTemplate> m_collectionTemplates
        {
            get => ASMSettings.instance.m_collectionTemplates;
            set => ASMSettings.instance.m_collectionTemplates = value;
        }

        static ASMSceneHelper m_sceneHelper
        {
            get => ASMSettings.instance.m_sceneHelper;
            set => ASMSettings.instance.m_sceneHelper = value;
        }

        static string m_fallbackScenePath
        {
            get => ASMSettings.instance.m_fallbackScenePath;
            set => ASMSettings.instance.m_fallbackScenePath = value;
        }

        static string GetFallbackScenePath()
        {

            if (Profile.current && Profile.current.startupScene)
                return Profile.current.startupScene.path;

            if (Application.isPlaying)
                return m_fallbackScenePath;

#if UNITY_EDITOR
            if (!fallbackScene)
                GenerateFallbackScene();
            return AssetDatabase.GetAssetPath(fallbackScene);
#else
            return m_fallbackScenePath;
#endif

        }

        #endregion

        /// <summary>Enumerates all imported profiles.</summary>
        public static IEnumerable<Profile> profiles => m_profiles.NonNull();

        /// <summary>Enumerates all imported collections.</summary>
        public static IEnumerable<SceneCollection> collections => m_collections.NonNull();

        /// <summary>Enumerates all imported collection templates.</summary>
        public static IEnumerable<SceneCollectionTemplate> collectionTemplates => m_collectionTemplates.NonNull();

        /// <summary>Enumerates all imported scenes.</summary>
        public static IEnumerable<Scene> scenes => m_scenes.NonNull();

        /// <summary>Gets scene helper singleton.</summary>
        public static ASMSceneHelper sceneHelper => m_sceneHelper;

        /// <summary>Enumerates all imported assets.</summary>
        public static IEnumerable<Object> allAssets =>
            profiles.OfType<Object>().Concat(scenes).Concat(collections).Concat(collectionTemplates).Concat(new[] { sceneHelper }).NonNull();

        /// <summary>Gets the import path.</summary>
        /// <remarks>Can be changed using <see cref="ProjectSettings.assetPath"/></remarks>
        public static string assetPath => SceneManager.settings.project.assetPath;

        /// <summary>Gets the path to the fallback scene.</summary>
        public static string fallbackScenePath => GetFallbackScenePath();

        #endregion
        #region Scene paths

#if UNITY_EDITOR

        /// <inheritdoc cref="SetSceneAssetPath(Scene, string, bool)"/>
        internal static void SetSceneAssetPath(string id, string path, bool save = true) =>
            SetSceneAssetPath(scenes.Find(id), path, save);

        /// <summary>Sets scene path and asset.</summary>
        /// <remarks>Only available in editor.</remarks>
        internal static void SetSceneAssetPath(Scene scene, string path, bool save = true)
        {

            if (!scene)
                return;

            var didChange = false;
            if (scene.path != path)
            {
                scene.path = path;
                didChange = true;
            }

            var asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            if (scene.sceneAsset != asset)
            {
                scene.sceneAsset = asset;
                didChange = true;
            }

            if (didChange && save)
                scene.Save();

        }

#endif
        #endregion
        #region Add / Remove

#if UNITY_EDITOR

        static void AddInternal<T>(params T[] items) where T : ASMModel
        {

            var paths = items.NonNull().Where(EditorUtility.IsPersistent).Select(AssetDatabase.GetAssetPath).ToArray();

            var list = GetList<T>();
            if (items.Select(Add).Any())
            {
                LogUtility.LogImport<T>("Imported", paths);
                Save();
            }

            bool Add(T item)
            {
                if (item && !list.Contains(item))
                {
                    list.Add(item);
                    AddToHooks(item);
                    return true;
                }
                return false;
            }

        }

        static void RemoveInternal<T>(params T[] items) where T : ASMModel, new()
        {

            items = items.NonNull().Where(EditorUtility.IsPersistent).ToArray();
            var paths = items.Select(AssetDatabase.GetAssetPath).ToArray();
            var folders = items.Select(item => GetFolder<T>(item.id)).Where(AssetDatabase.IsValidFolder).ToArray();

            var list = GetList<T>();

            var failedPaths = new List<string>();
            AssetDatabase.DeleteAssets(folders, failedPaths);
            if (failedPaths.Any())
                Debug.LogError("The following assets could not be removed:\n" + string.Join("\n", failedPaths));

            if (items.Select(list.Remove).Any())
            {
                LogUtility.LogImport<T>("Unimported", paths);
                Save();
            }

            foreach (var item in items)
                if (!EditorUtility.IsPersistent(item))
                    RemoveFromHooks(item);

        }

        static void RemoveInternal(params ASMModel[] items)
        {
            RemoveInternal(items.OfType<Scene>().ToArray());
            RemoveInternal(items.OfType<SceneCollection>().Except(items.OfType<SceneCollectionTemplate>()).ToArray());
            RemoveInternal(items.OfType<SceneCollectionTemplate>().ToArray());
            RemoveInternal(items.OfType<Profile>().ToArray());
        }

        static List<T> GetList<T>()
        {
            if (typeof(T) == typeof(Profile)) return (List<T>)(object)m_profiles;
            else if (typeof(T) == typeof(SceneCollectionTemplate)) return (List<T>)(object)m_collectionTemplates;
            else if (typeof(T) == typeof(SceneCollection)) return (List<T>)(object)m_collections;
            else if (typeof(T) == typeof(Scene)) return (List<T>)(object)m_scenes;
            throw new InvalidOperationException("Could not find list of " + typeof(T).Name);
        }

#endif

        #endregion
        #region Import

#if UNITY_EDITOR

        public static void Remove(IEnumerable<ASMModel> list) =>
            RemoveInternal(list.ToArray());

        public static void Remove<T>(IEnumerable<T> list) where T : ASMModel =>
            list.ForEach(Remove);

        public static void Add<T>(IEnumerable<T> list) where T : ASMModel =>
            Add(list, SceneManager.settings.project.assetPath);

        public static void Add<T>(IEnumerable<T> list, string importFolder) where T : ASMModel =>
            list.ForEach(m => Add(m, importFolder));

        public static void Remove<T>(T obj) where T : ASMModel =>
            RemoveInternal(obj);

        public static void Add<T>(T obj) where T : ASMModel =>
            Add(obj, SceneManager.settings.project.assetPath);

        public static void Add<T>(T obj, string importFolder) where T : ASMModel
        {

            if (!obj)
                return;

            if (!IsImportedByPath(obj, importFolder))
                Import(obj, importFolder);

            AddInternal(obj);

        }

        public static bool IsImported<T>(T model) where T : ASMModel =>
            allAssets.Contains(model);

        public static bool Contains<T>(T model) where T : ASMModel =>
            IsImported(model);

        static bool IsImportedByPath(ASMModel obj, string importFolder)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            return path.StartsWith(importFolder);
        }

        static void Import(ASMModel obj, string importFolder)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path))
                Create(obj, importFolder);
            else
                Move(obj, path, importFolder);
        }

        static void Create(ASMModel obj, string importFolder)
        {
            var path = GetPath(obj, importFolder);
            AssetDatabase.CreateAsset(obj, path);
        }

        static void Move(ASMModel obj, string path, string importFolder) =>
            AssetDatabase.MoveAsset(path, GetPath(obj, importFolder));

        public static string GetPath(ASMModel obj, string importFolder)
        {
            var path = $"{GetFolder(obj, importFolder)}/{obj.name}.asset";
            Directory.GetParent(path).Create();
            return path;
        }

        public static string GetFolder(ASMModel obj, string importFolder) =>
            $"{importFolder}/{obj.GetType().Name}/{obj.id}";

        public static string GetFolder<T>() where T : ASMModel, new() =>
            $"{assetPath}/{typeof(T).Name}";

        public static string GetFolder<T>(string id) where T : ASMModel, new() =>
            $"{GetFolder<T>()}/{id}";

        public static string GetPath<T>(string id, string name) where T : ASMModel, new() =>
            $"{GetFolder<T>(id)}/{name}.asset";

#endif

        #endregion
        #region Save

        static void Save()
        {
#if UNITY_EDITOR
            _ = Cleanup();
            ASMSettings.instance.Save();
#endif
        }

#if UNITY_EDITOR

        public static void CleanupAndSave()
        {
            if (Cleanup())
                Save();
        }

        public static bool Cleanup()
        {

            CleanupFolder(GetFolder<Profile>());
            CleanupFolder(GetFolder<Scene>());
            CleanupFolder(GetFolder<SceneCollection>());
            CleanupFolder(GetFolder<SceneCollectionTemplate>());

            var needsSave = false;
            if (EnsureAssetsAdded<Profile>() ||
                EnsureAssetsAdded<SceneCollection>() ||
                EnsureAssetsAdded<SceneCollectionTemplate>())
                needsSave = true;

            if (CleanupNulls())
                return true;

            return needsSave;

        }

        public static void CleanupFolder(string folder)
        {

            var emptySceneFolders = AssetDatabase.GetSubFolders(folder);

            foreach (var subfolder in emptySceneFolders)
            {

                var path = Application.dataPath + subfolder.Replace("Assets", "");

                if (Directory.Exists(path) && Directory.GetFileSystemEntries(path).Length == 0)
                    AssetDatabase.DeleteAsset(subfolder);

            }

        }

        internal static bool EnsureAssetsAdded<T>() where T : ASMModel
        {

            var didAddAny = false;

            var list = GetList<T>();

            var assets = AssetDatabase.FindAssets($"t:{typeof(T).FullName}").Select(AssetDatabase.GUIDToAssetPath).Select(AssetDatabase.LoadAssetAtPath<T>).NonNull();
            foreach (var asset in assets)
            {
                if (!list.Contains(asset))
                {
                    list.Add(asset);
                    didAddAny = true;
                }
            }

            return didAddAny;

        }

        static bool CleanupNulls()
        {
            var itemsRemoved = m_scenes.RemoveAll(s => !s) + m_collections.RemoveAll(c => !c) + m_profiles.RemoveAll(p => !p) + m_collectionTemplates.RemoveAll(c => !c);
            return itemsRemoved > 0;
        }

#endif

        #endregion
        #region Hook

        static void AddToHooks(ASMModel model)
        {

#if UNITY_EDITOR

            SceneAsset sceneAsset = null;
            if (model is Scene scene && scene)
                sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);

            foreach (var hook in hooks)
            {
                hook.list.Add(model);
                if (sceneAsset)
                    hook.sceneAssets.Add(sceneAsset);
            }

#endif

        }

        static void RemoveFromHooks(ASMModel model)
        {
#if UNITY_EDITOR
            foreach (var hook in hooks)
                hook.list.Remove(model);
#endif

        }

#if UNITY_EDITOR

        static readonly List<ImportHook> hooks = new();

        /// <summary>Adds a hook for asset import. Released on <see cref="IDisposable.Dispose"/>.</summary>
        /// <param name="removeAssetsOnDispose">Automatically delete ASM assets that was collected by this hook on <see cref="IDisposable.Dispose"/>.</param>
        /// <param name="removeSceneAssetsOnDispose">Automatically delete <see cref="SceneAsset"/> that was collected by this hook on <see cref="IDisposable.Dispose"/>.</param>
        /// <remarks>Only available in editor.</remarks>
        public static ImportHook Hook(bool removeAssetsOnDispose = false, bool removeSceneAssetsOnDispose = false)
        {
            var hook = new ImportHook() { removeAssetsOnDispose = removeAssetsOnDispose, removeSceneAssetsOnDispose = removeSceneAssetsOnDispose };
            hooks.Add(hook);
            return hook;
        }

        /// <summary>Collects imported assets when added.</summary>
        /// <remarks>Only available in editor.</remarks>
        public class ImportHook : IDisposable
        {

            internal ImportHook()
            { }

            /// <summary>Specifies whatever collected ASM assets should be removed on <see cref="Dispose"/>.</summary>
            public bool removeAssetsOnDispose { get; set; }

            /// <summary>Specifies whatever collected <see cref="SceneAsset"/> should be removed on <see cref="Dispose"/>.</summary>
            public bool removeSceneAssetsOnDispose { get; set; }

            internal readonly List<SceneAsset> sceneAssets = new();
            internal readonly List<ASMModel> list = new();

            /// <summary>Enumerates the collected assets.</summary>
            public IEnumerable<ASMModel> importedModels => list.NonNull();

            /// <summary>Enumerates the collected <see cref="SceneAsset"/>.</summary>
            public IEnumerable<SceneAsset> addedScenes => sceneAssets.NonNull();

            /// <summary>Releases the hook.</summary>
            public void Dispose() => Release();

            /// <summary>Releases hook, and optionally removes assets, if specified by <see cref="removeAssetsOnDispose"/> and <see cref="removeSceneAssetsOnDispose"/>.</summary>
            public void Release()
            {
                if (removeAssetsOnDispose)
                    RemoveAll(removeSceneAssetsOnDispose);
                hooks.Remove(this);
            }

            /// <summary>Removes all collected ASM assets, and optionally all <see cref="SceneAsset"/>.</summary>
            /// <param name="removeSceneAssets">Specifies whatever all added <see cref="SceneAsset"/> should also be removed.</param>
            /// <remarks>See also: <see cref="RemoveAll(bool)"/> and <see cref="RemoveAllAndRelease(bool)"/>.</remarks>
            public void RemoveAll(bool removeSceneAssets = false)
            {

                Remove(importedModels);

                if (removeSceneAssets)
                {

                    var paths = addedScenes.NonNull().Where(EditorUtility.IsPersistent).Select(AssetDatabase.GetAssetPath).ToArray();

                    var failedPaths = new List<string>();
                    AssetDatabase.DeleteAssets(paths, failedPaths);

                    if (failedPaths.Any())
                        Debug.LogError("Could not remove the following assets:\n" + string.Join("\n", failedPaths));

                }

            }

            /// <summary>Removes all collected ASM assets, and optionally all <see cref="SceneAsset"/>, then releases.</summary>
            /// <param name="removeSceneAssets">Specifies whatever all added <see cref="SceneAsset"/> should also be removed.</param>
            public void RemoveAllAndRelease(bool removeSceneAssets = false)
            {
                RemoveAll(removeSceneAssets);
                Release();
            }

        }

#endif

        #endregion

    }

}
