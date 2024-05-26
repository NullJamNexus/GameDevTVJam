using Mono.Cecil;
using Sirenix.OdinInspector;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    private enum BoxType{
        Water,
        Food,
        Wood
    }

    private int _randomNumberForType;
    private int _randomNumberForAmount;
    private PlayerVitals _playerVitalsManager;
    [SerializeField] private Sprite _waterBox;
    [SerializeField] private Sprite _foodBox;
    [SerializeField] private Sprite _woodBox;
    [SerializeField] private BoxType _boxType;
    [SerializeField] private Sprite _spriteToDraw;

    private void Awake(){
        //TODO:
        //this is terrible terrible terrible :(, fix this!!!, ONLY USE IN HAWKY'S PROTOTYPE!! 
        _playerVitalsManager = GameObject.Find("PlayerStatsManager").GetComponent<PlayerVitals>();

        //generate random number for type
        _randomNumberForType = Random.Range(1,4);

        //generate random number for amount
        _randomNumberForAmount = Random.Range(1,101);

        //assign box type according to random number
        switch(_randomNumberForType){

            case 1:
                _boxType = BoxType.Water;
            break;

            case 2:
                _boxType = BoxType.Food;
            break;

            case 3:
                _boxType = BoxType.Wood;
            break;

        }
        
        //assign box sprites in accordance
        if(_boxType == BoxType.Water){
            _spriteToDraw = _waterBox;
            GetComponent<SpriteRenderer>().sprite = _spriteToDraw;
        }

        if(_boxType == BoxType.Food){
            _spriteToDraw = _foodBox;
            GetComponent<SpriteRenderer>().sprite = _spriteToDraw;
        }

        if(_boxType == BoxType.Wood){
            _spriteToDraw = _woodBox;
            GetComponent<SpriteRenderer>().sprite = _spriteToDraw;
        }
    }

 //THIS SCRIPT NEEDS TO OUTPUT A RESOURCE TYPE WHEN ITS MADE!!! FOR NOW IT ONLY RETURNS INTEGER
 //Get Contents of Box
 [Button("Get Loot")]
    public void GetLoot(){
        int amountOfResourceToGive;

        //TODO:
        //why does this require to be initialised?? the switch case should do that, this is a hack :(
        PlayerRes.ResourceType resourceToGive=PlayerRes.ResourceType.Wood; 
        
    //convert boxtype into resourcetype
        switch(_boxType){
            case BoxType.Water:
                resourceToGive = PlayerRes.ResourceType.Thirst;
            break;

            case BoxType.Food:
                resourceToGive = PlayerRes.ResourceType.Hunger;
            break;

            case BoxType.Wood:
                resourceToGive = PlayerRes.ResourceType.Wood;
            break;
        }

        //assign random number of resource to give
        amountOfResourceToGive = _randomNumberForAmount;
        
       

        
        _playerVitalsManager.AddResource(resourceToGive,amountOfResourceToGive);
        Debug.Log("LOOTBOX: Given "+resourceToGive+" amount:"+amountOfResourceToGive+" to player");
        Destroy(gameObject);
        Destroy(gameObject.transform.parent.gameObject);
    }
    


}
