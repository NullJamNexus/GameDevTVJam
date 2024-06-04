using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class DistractionProcessor : BaseComponent, IDistractionProcessor
    {
        [field: BoxGroup("Settings"), SerializeField, Range(0f, 100f)]
        public float ChanceToResist { get; private set; } = 0f;
        [field: BoxGroup("Settings"), SerializeField]
        public float YMaxDistance { get; private set; } = 2f;
        [field: BoxGroup("Settings"), SerializeField]
        public float CloseEnough { get; private set; } = 1f;
        
        public Vector2 DistractionLocation { get; private set; }
        
        public void SetLocation(Vector2 location)
        {
            DistractionLocation = location;
        }
    }
}
