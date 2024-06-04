using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Vit.Utilities;
namespace NJN.Runtime.Components
{
    public class PatrolDesignatorNoPoints : BaseComponent, IPatrolDesignator
    {
        [BoxGroup("Patrol Settings"), SerializeField, Tooltip("How close to a block object to change direction")]
        private float _closeEnough = 1f;
        [BoxGroup("Patrol Settings"), SerializeField, Tooltip("Which direction to move at the start")]
        private bool _startTowardsRight;

        [BoxGroup("Layer Settings"), SerializeField, ValueDropdown("GetLayerNames")]
        private string[] _blockLayers;
        private IEnumerable<string> GetLayerNames()
        {
            return Tools.GetLayerNames();
        }

        private LayerMask _layerMask;

        private Vector2 _direction;

        private void Start()
        {
            float xDirection = _startTowardsRight ? 1 : -1;
            _direction = new Vector2(xDirection, 0);

            _layerMask = LayerMask.GetMask(_blockLayers);
        }

        public Vector2 GetNextPointDirection(Vector2 position)
        {
            return _direction;
        }

        public bool HasArrivedAtPoint(Vector2 position)
        {
            if (!IsPathBlocked())
                return false;

            //Change Direction for the next patrol
            _direction = _direction * -1;

            return true;
        }

        private bool IsPathBlocked()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, _closeEnough, _layerMask);
            if(hit.collider != null)
            {
                return true;
            }
            return false;
        }
    }
}

