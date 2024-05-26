using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVitals : MonoBehaviour
{
    [SerializeField] private bool _testingValues;
    private PlayerRes _thirst = new PlayerRes(PlayerRes.ResourceType.Thirst,0);
    private PlayerRes _hunger = new PlayerRes(PlayerRes.ResourceType.Hunger,0);
    private PlayerRes _wood = new PlayerRes(PlayerRes.ResourceType.Wood,0);
   [SerializeField] private Slider _hungerSlider;
   [SerializeField] private Slider _thirstSlider;
   [SerializeField] private TextMeshProUGUI _woodText;

   private void Awake(){
    //if testing values is true, assign some values
        if(_testingValues){
            AddResource(PlayerRes.ResourceType.Thirst,45);
            AddResource(PlayerRes.ResourceType.Hunger,55);
            AddResource(PlayerRes.ResourceType.Wood,65);
        }
   }

//add resource to player stats
    public void AddResource(PlayerRes.ResourceType resourceType, int amount){
        //TODO:
        //should be switch case statement
        if(resourceType == PlayerRes.ResourceType.Thirst){
            _thirst.ThisResourceAmount += amount;
        }

        if(resourceType == PlayerRes.ResourceType.Hunger){
            _hunger.ThisResourceAmount += amount;
        }

        if(resourceType == PlayerRes.ResourceType.Wood){
            _wood.ThisResourceAmount += amount;
        }

        //update sliders

        _thirstSlider.value = _thirst.ThisResourceAmount;
        _hungerSlider.value = _hunger.ThisResourceAmount;
        _woodText.text = _wood.ThisResourceAmount.ToString();

    }



}

