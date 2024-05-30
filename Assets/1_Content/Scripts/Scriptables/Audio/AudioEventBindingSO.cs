using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Scriptables
{
    [CreateAssetMenu(fileName = "AudioEventBinding", menuName = "NJN/Audio/New Binding")]
    public class AudioEventBindingSO : ScriptableObject
    {
        [field: FoldoutGroup("Player"), SerializeField] 
        public EventReference Hide { get; private set; }
        
        [field: FoldoutGroup("Enemy"), SerializeField] 
        public EventReference Attack { get; private set; }
    }
}