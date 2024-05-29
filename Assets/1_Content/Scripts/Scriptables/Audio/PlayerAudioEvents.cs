using FMODUnity;
using UnityEngine;

namespace NJN.Scriptables.Audio.Player
{
    [CreateAssetMenu(fileName = "PlayerAudioEvents", menuName = "NJN/Audio/Player")]
    public class PlayerAudioEvents : ScriptableObject
    {
        [field:SerializeField] public EventReference Hide { get;private set; }
    }
}