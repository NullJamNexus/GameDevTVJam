using NJN.Runtime.Controllers.Player;
using QFSW.QC.Actions;
using Sirenix.OdinInspector;
using UnityEngine;
using Vit.Utilities;


namespace NJN.Runtime.Components
{
    [RequireComponent(typeof(Collider2D))]
    public class RemoteInteractor : BaseComponent, IRemoteInteractor
    {
        [BoxGroup("Settings"), SerializeField]
        private LayerMask _interactableLayer;

        [SerializeField] private RemoteTrigger _remoteTrigger;
        public IRemoteInteractableComponent RemoteInteractable {  get; private set; }

        public IRemoteInteractableComponent PlacedRemoteInteractable { get; private set; }

        private void Update()
        {
            
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (PlacedRemoteInteractable != null)
                return;
            if (Tools.IsInLayerMask(other.gameObject, _interactableLayer)
                && other.TryGetComponent(out IRemoteInteractableComponent remoteInteractable))
            {
                RemoteInteractable = remoteInteractable;
                RemoteInteractable.ShowRemoteInteractPrompt();
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (PlacedRemoteInteractable != null)
                return;
            if (Tools.IsInLayerMask(other.gameObject, _interactableLayer)
                && other.TryGetComponent(out IRemoteInteractableComponent remoteInteractable) && RemoteInteractable == remoteInteractable)
            {
                RemoteInteractable = null;
                remoteInteractable.HideRemoteInteractPrompt();
            }
        }

        [Button]
        public void RemoteInteract()
        {
            if (RemoteInteractable != null)
                PlaceRemoteInteractable();
            else if (PlacedRemoteInteractable!= null)
                PlaceRemoteTrigger();
            else
            {
                print("Nothing happened");
            }
        }

        private void PlaceRemoteInteractable()
        {
            PlacedRemoteInteractable = RemoteInteractable;
            PlacedRemoteInteractable.SelectedForRemoteInteraction();
            RemoteInteractable = null;
        }

        private void PlaceRemoteTrigger()
        {
            GameObject remoteTrigger = Instantiate(_remoteTrigger.gameObject, transform.position, Quaternion.identity);
            remoteTrigger.GetComponent<RemoteTrigger>().SetRemoteInteractable(PlacedRemoteInteractable);
            PlacedRemoteInteractable = null;
        }
    }
}
    
