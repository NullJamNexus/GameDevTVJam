using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Components
{
    public class PlayerStep : MonoBehaviour
    {
        [BoxGroup("Settings"), SerializeField]
        private LayerMask _groundLayer;

        private Vector2 _rayOriginOffSet { get; } = new Vector2(0, -0.49f);
        private float _rayLength { get; } = 0.03f;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        public void AnimationStepped() // fire from animation
        {
            if (IsGrounded())
            {
                _signalBus.TryFire(new PlayerStepSignal());
            }
        }

        private bool IsGrounded()
        {
            Vector2 origin = new Vector2(transform.position.x, transform.position.y) + _rayOriginOffSet;

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, _rayLength, _groundLayer);

            if (hit.collider != null)
            {
                return true;
            }
            return false;
        }
    }
}
