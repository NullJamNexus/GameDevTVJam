using Sirenix.OdinInspector;
using UnityEngine;
using Vit.Utilities;

namespace NJN.Runtime.Components
{
    public class RemoteTrigger : BaseComponent
    {
        [BoxGroup("Settings"), SerializeField]
        private LayerMask _interactableLayer;

        private IRemoteInteractableComponent _remoteInteractable;
        public void SetRemoteInteractable(IRemoteInteractableComponent remoteInteractable)
        {
            _remoteInteractable = remoteInteractable;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Tools.IsInLayerMask(collision.gameObject, _interactableLayer))
            {
                if (_remoteInteractable != null)
                {
                    FireTrigger();
                }
            }
        }

        [Button]
        private void FireTrigger()
        {
            _remoteInteractable.RemoteInteract();
            Destroy(gameObject);
        }
    }
}