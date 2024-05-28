using NJN.Runtime.Controllers.Player;
using NJN.Runtime.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace NJN.Runtime.Components
{
    public class Stove : InteractableComponent
    {
        [BoxGroup("Stove"), SerializeField]
        private int _foodCost = 1;
        [BoxGroup("Stove"), SerializeField]
        private float _hungerPerFood = 20f;
        [BoxGroup("Stove"), SerializeField]
        private string _notEnoughResourcesPrompt = "Need More Food";
        
        private LevelManager _levelManager;
        
        // TODO: Interface level manager...
        [Inject]
        private void Construct(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }
        
        public override void Interact(PlayerController player)
        {
            base.Interact(player);

            if (!HasEnoughResources()) return;
            
            _levelManager.LevelInventory.AddFood(-_foodCost);
            _signalBus.Fire(new CookedFoodSignal(_hungerPerFood));
            ShowInteractPrompt();
        }
        
        public override void ShowInteractPrompt()
        {
            if (HasEnoughResources())
            {
                base.ShowInteractPrompt();
                return;
            }
            
            Vector2 position = transform.position + (Vector3)_promptOffset;
            string prompt = $"<color=red>{_notEnoughResourcesPrompt}</color>";
            _interactionPrompt.ShowPrompt(prompt, position);
        }
        
        private bool HasEnoughResources()
        {
            return _levelManager.LevelInventory.Food.Value >= _foodCost;
        }
    }
}