#if UNITY_EDITOR

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AdvancedSceneManager.Callbacks;
using AdvancedSceneManager.Utility;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AdvancedSceneManager.Models
{

    /// <summary>Contains settings that are stored locally, that aren't synced to source control.</summary>
    /// <remarks>Only available in editor.</remarks>
    [ASMFilePath("UserSettings/AdvancedSceneManager.asset")]
    public class ASMUserSettings : ASMScriptableSingleton<ASMUserSettings>, INotifyPropertyChanged
    {

        public override bool editorOnly => true;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new(propertyName));

        bool isInitialValidate { get; set; } = true;
        void OnValidate()
        {

            if (!isInitialValidate)
                Save();

            //Fixes 'You may not pass in objects that are already persistent',
            //that seems to happen when saving too early?
            isInitialValidate = false;

            OnPropertyChanged("");

        }

        #endregion
        #region Callback utility

        [Header("Callback Utility")]
        [SerializeField] private bool m_isCallbackUtilityEnabled;
        [SerializeField] private string m_callbackUtilityWindow;

        internal bool isCallbackUtilityEnabled
        {
            get => m_isCallbackUtilityEnabled;
            set { m_isCallbackUtilityEnabled = value; OnPropertyChanged(); }
        }

        internal string callbackUtilityWindow
        {
            get => m_callbackUtilityWindow;
            set { m_callbackUtilityWindow = value; OnPropertyChanged(); }
        }

        #endregion
        #region Menu popup

        [Header("Menu Popup")]
        [SerializeField] private string m_quickBuildPath;
        [SerializeField] private bool m_quickBuildUseProfiler;
        [SerializeField] private SerializableDateTime m_lastPatchCheck;
        [SerializeField] private string m_patchVersion;
        [SerializeField] private string m_patchNotes;
        [SerializeField] private bool m_isTooOutOfDate;

        internal string quickBuildPath
        {
            get => m_quickBuildPath;
            set { m_quickBuildPath = value; OnPropertyChanged(); }
        }

        internal bool quickBuildUseProfiler
        {
            get => m_quickBuildUseProfiler;
            set { m_quickBuildUseProfiler = value; OnPropertyChanged(); }
        }

        internal SerializableDateTime lastPatchCheck
        {
            get => m_lastPatchCheck;
            set { m_lastPatchCheck = value; OnPropertyChanged(); }
        }

        internal string patchVersion
        {
            get => m_patchVersion;
            set { m_patchVersion = value; OnPropertyChanged(); }
        }

        internal string patchNotes
        {
            get => m_patchNotes;
            set { m_patchNotes = value; OnPropertyChanged(); }
        }

        internal bool isTooOutOfDate
        {
            get => m_isTooOutOfDate;
            set { m_isTooOutOfDate = value; OnPropertyChanged(); }
        }

        #endregion
        #region Startup

        [Header("Startup")]
        [SerializeField] private Profile m_activeProfile;
        [SerializeField] internal bool runStartupProcess;
        [SerializeField] private bool m_isBuildMode;
        [SerializeField] private bool m_startupProcessOnCollectionPlay = true;

        /// <summary>Specifies the active profile in editor.</summary>
        public Profile activeProfile
        {
            get => m_activeProfile;
            set { m_activeProfile = value; OnPropertyChanged(); }
        }

        internal bool isBuildMode
        {
            get => m_isBuildMode;
            set { m_isBuildMode = value; OnPropertyChanged(); }
        }

        /// <summary>Specifies whatever startup process should run when pressing collection play button.</summary>
        public bool startupProcessOnCollectionPlay
        {
            get => m_startupProcessOnCollectionPlay;
            set { m_startupProcessOnCollectionPlay = value; OnPropertyChanged(); }
        }

        #endregion
        #region Display

        [Header("Appearance")]
        [SerializeField] private bool m_displayCollectionPlayButton = true;
        [SerializeField] private bool m_displayCollectionOpenButton = true;
        [SerializeField] private bool m_displayCollectionAdditiveButton = true;
        [SerializeField] private bool m_displayIncludeInBuildToggle = false;
        [SerializeField] private int m_toolbarButtonCount = 1;
        [SerializeField] private float m_toolbarPlayButtonOffset = 0;
        [SerializeField] private SerializableDictionary<int, SceneCollection> m_toolbarButtonActions = new SerializableDictionary<int, SceneCollection>();
        [SerializeField] private SerializableDictionary<int, bool> m_toolbarButtonActions2 = new SerializableDictionary<int, bool>();

        /// <summary>Specifies whatever the include in build toggle should be displayed on collections.</summary>
        public bool displayIncludeInBuildToggle
        {
            get => m_displayIncludeInBuildToggle;
            set { m_displayIncludeInBuildToggle = value; OnPropertyChanged(); }
        }

        /// <summary>Specifies whatever collection play button should be visible.</summary>
        public bool displayCollectionPlayButton
        {
            get => m_displayCollectionPlayButton;
            set { m_displayCollectionPlayButton = value; OnPropertyChanged(); }
        }

        /// <summary>Specifies whatever collection open button should be visible.</summary>
        public bool displayCollectionOpenButton
        {
            get => m_displayCollectionOpenButton;
            set { m_displayCollectionOpenButton = value; OnPropertyChanged(); }
        }

        /// <summary>Specifies whatever collection open additive button should be visible.</summary>
        public bool displayCollectionAdditiveButton
        {
            get => m_displayCollectionAdditiveButton;
            set { m_displayCollectionAdditiveButton = value; OnPropertyChanged(); }
        }

        /// <summary>Specifies how many buttons should be placed in toolbar.</summary>
        /// <remarks>Only has an effect if https://github.com/marijnz/unity-toolbar-extender is installed.</remarks>
        public int toolbarButtonCount
        {
            get => m_toolbarButtonCount;
            set { m_toolbarButtonCount = value; OnPropertyChanged(); }
        }

        /// <summary>Specifies offset for toolbar play buttons.</summary>
        /// <remarks>Only has an effect if https://github.com/marijnz/unity-toolbar-extender is installed.</remarks>
        public float toolbarPlayButtonOffset
        {
            get => m_toolbarPlayButtonOffset;
            set { m_toolbarPlayButtonOffset = value; OnPropertyChanged(); }
        }

        /// <summary>Sets the scene collection to open for the specified toolbar button, if any.</summary>
        /// <remarks>Only has an effect if https://github.com/marijnz/unity-toolbar-extender is installed.</remarks>
        public void ToolbarAction(int i, out SceneCollection collection, out bool runStartupProcess)
        {
            collection = m_toolbarButtonActions?.GetValueOrDefault(i);
            runStartupProcess = m_toolbarButtonActions2?.GetValueOrDefault(i) ?? true;
        }

        /// <summary>Sets the scene collection to open for the specified toolbar button, if any.</summary>
        /// <remarks>Only has an effect if https://github.com/marijnz/unity-toolbar-extender is installed.</remarks>
        public void ToolbarAction(int i, SceneCollection collection, bool runStartupProcess)
        {

            if (m_toolbarButtonActions == null)
                m_toolbarButtonActions = new SerializableDictionary<int, SceneCollection>();

            if (m_toolbarButtonActions2 == null)
                m_toolbarButtonActions2 = new SerializableDictionary<int, bool>();

            m_toolbarButtonActions.Set(i, collection);
            m_toolbarButtonActions2.Set(i, runStartupProcess);

            this.Save();

        }

        #endregion
        #region Logging

        [Header("Logging")]
        [SerializeField] private bool m_logImport;
        [SerializeField] private bool m_logTracking;
        [SerializeField] private bool m_logLoading;
        [SerializeField] private bool m_logStartup;
        [SerializeField] private bool m_logOperation;
        [SerializeField] private bool m_logBuildScenes;

        /// <summary>Specifies whatever ASM should log when a <see cref="ASMModel"/> is imported.</summary>
        public bool logImport
        {
            get => m_logImport;
            set { m_logImport = value; OnPropertyChanged(); }
        }

        /// <summary>Specifies whatever ASM should log when a scene is tracked after loaded.</summary>
        public bool logTracking
        {
            get => m_logTracking;
            set { m_logTracking = value; OnPropertyChanged(); }
        }

        /// <summary>Specifies whatever ASM should log when a scene is loaded.</summary>
        public bool logLoading
        {
            get => m_logLoading;
            set { m_logLoading = value; OnPropertyChanged(); }
        }

        /// <summary>Specifies whatever ASM should log during startup.</summary>
        public bool logStartup
        {
            get => m_logStartup;
            set { m_logStartup = value; OnPropertyChanged(); }
        }

        /// <summary>Specifies whatever ASM should log during scene operations.</summary>
        public bool logOperation
        {
            get => m_logOperation;
            set { m_logOperation = value; OnPropertyChanged(); }
        }

        /// <summary>Specifies whatever ASM should log when build scene list is updated.</summary>
        public bool logBuildScenes
        {
            get => m_logBuildScenes;
            set { m_logBuildScenes = value; OnPropertyChanged(); }
        }

        #endregion
        #region Netcode

#pragma warning disable CS0414
        [Header("Netcode")]
        [SerializeField] private bool m_displaySyncedIndicator = true;
#pragma warning restore CS0414

#if NETCODE

        /// <summary>Specifies that the 'synced' hierarchy indicator should be shown for synced scenes when using netcode.</summary>
        public bool displaySyncedIndicator
        {
            get => m_displaySyncedIndicator;
            set { m_displaySyncedIndicator = value; OnPropertyChanged(); }
        }

#endif

        #endregion
        #region Runtime

        [SerializeField] internal SceneCollection m_openCollection;
        [SerializeField] internal List<SceneCollection> m_additiveCollections = new();

        #endregion

        internal SceneSetup[] sceneSetup;

        [Header("Misc")]
        [SerializeField] private bool m_openCollectionOnSceneAssetOpen;

        /// <summary>When <see langword="true"/>: opens the first found collection that a scene is contained in when opening an SceneAsset in editor.</summary>
        public bool openCollectionOnSceneAssetOpen
        {
            get => m_openCollectionOnSceneAssetOpen;
            set { m_openCollectionOnSceneAssetOpen = value; OnPropertyChanged(); }
        }

    }

}
#endif