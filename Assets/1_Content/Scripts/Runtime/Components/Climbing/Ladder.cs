using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class Ladder : BaseComponent, IComponent, IClimbable
    {
        [BoxGroup("Settings"), SerializeField]
        private Vector2 _topPosition;
        [BoxGroup("Settings"), SerializeField]
        private Vector2 _bottomPosition;
        [BoxGroup("Settings"), SerializeField]
        private bool _showDebug = true;
        
        public Vector2 GetTop()
        {
            return (Vector2)transform.position + _topPosition;
        }
        
        public Vector2 GetCenter()
        {
            return transform.position;
        }
        
        public Vector2 GetBottom()
        {
            return (Vector2)transform.position + _bottomPosition;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!_showDebug) return;

            Gizmos.color = Color.blue;

            Vector3 topWorldPosition = GetTop();
            Vector3 bottomWorldPosition = GetBottom();

            Gizmos.DrawWireSphere(topWorldPosition, 0.2f);
            Gizmos.DrawWireSphere(bottomWorldPosition, 0.2f);
        }
#endif
    }
}