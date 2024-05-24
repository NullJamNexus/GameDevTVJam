using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Scriptables.Settings
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "NJN/Settings/New Level Settings")]
    public class LevelSettingsSO : ScriptableObject
    {
        [field: BoxGroup("Player Settings"), SerializeField]
        public GameObject PlayerPrefab { get; private set; }
    }
}