using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
namespace NJN.Runtime.Components
{
    public class RemoteInteractableComponent : BaseComponent, IRemoteInteractableComponent
    {
        private IRemoteInteractable _interactable;
        [SerializeField] private GameObject _RemoteSelectedObject;

        private void OnEnable()
        {
            _interactable = GetComponent<IRemoteInteractable>();
            if (_interactable == null)
                print("_interactable is null");
        }
        public void HideRemoteInteractPrompt()
        {
            print("Hide Prompt");
        }

        public void SelectedForRemoteInteraction()
        {
            _RemoteSelectedObject.SetActive(true);
        }

        [Button]
        public void RemoteInteract()
        {
            _interactable.RemoteInteractTriggered();
            _RemoteSelectedObject.SetActive(false);
        }

        public void ShowRemoteInteractPrompt()
        {
            print("Show Prompt");
        }
    }
}
