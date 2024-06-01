using System.Collections.Generic;
using NJN.Runtime.Scenes;
using UnityEngine;

namespace NJN.Scriptables
{
    public enum SceneType
    {
        Loader,
        MainMenu,
        Level
    }
    
    [CreateAssetMenu(fileName = "SceneSettings", menuName = "NJN/Scenes/New Scene Settings")]
    public class SceneSettingsSO : ScriptableObject
    {
        [field: SerializeField]
        public List<SceneBind> SceneBinds { get; private set; }
    }
}