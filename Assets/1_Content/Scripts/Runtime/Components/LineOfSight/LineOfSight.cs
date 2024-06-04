using NJN.Runtime.Controllers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class LineOfSight : BaseComponent, ILineOfSight
    {
        [BoxGroup("Settings"), SerializeField, Tooltip("Layer to look for")]
        private LayerMask _targetLayer;
        [BoxGroup("Settings"), SerializeField, Tooltip("Layers that can block line of sight")]
        private LayerMask _obstructionLayers;
        [BoxGroup("Settings"), SerializeField, Tooltip("Maximum distance for the line of sight")]
        private float _maxDistance = 10f;
        [field: BoxGroup("Settings"), SerializeField, Tooltip("How often to check for damagable target")]
        public float CheckInterval { get; private set; } = 0.5f;
        
        public IDamagable DamagableTarget { get; set; }

        public bool HasLineOfSight(Vector2 targetPosition)
        {
            Vector2 origin = transform.position;
            Vector2 direction = (targetPosition - origin).normalized;
            direction.y = 0;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, _maxDistance, _targetLayer | _obstructionLayers);
            if (hit.collider == null) return false;
            
            if (((1 << hit.collider.gameObject.layer) & _targetLayer) != 0)
            {
                // Has LOS
                return true;
            }

            if (((1 << hit.collider.gameObject.layer) & _obstructionLayers) != 0)
            {
                // LOS is blocked
                return false;
            }

            return false;
        }

        public bool TryGetVisibleDamagable(Vector2 direction, out IDamagable damagable)
        {
            Vector2 origin = transform.position;
            direction.y = 0;

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, _maxDistance, _targetLayer | _obstructionLayers);
            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & _targetLayer) != 0)
            {
                if (hit.collider.TryGetComponent(out damagable))
                {
                    return true;
                }
            }

            damagable = null;
            return false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _maxDistance);
            Gizmos.DrawLine(transform.position, transform.position - Vector3.right * _maxDistance);
        }
#endif
    }
}
