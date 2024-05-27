using System;
using NJN.Runtime.Controllers.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Components
{
    public class Transporter : InteractableComponent
    {
        [BoxGroup("Teleport Settings"), SerializeField]
        private Transform _target;
        [BoxGroup("Teleport Settings"), SerializeField]
        private Vector2 _offset;
        
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
            
            if (_target == null) return;

            player.transform.position = TransportLocation();
        }

        private Vector2 TransportLocation()
        {
            return (Vector2)_target.position + _offset;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_target == null) return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _target.position);
            Gizmos.DrawWireSphere(TransportLocation(), 0.2f);
        }
#endif
    }
}