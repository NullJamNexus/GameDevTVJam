using UnityEngine;

public class LootBoxSpawner : MonoBehaviour
{
    
    [SerializeField] GameObject _lootBoxGameObject;
    
    void Start()
    {
        //remove placeholder sprite
        GetComponent<SpriteRenderer>().sprite = null;

        //instantiate loot box
        Instantiate(_lootBoxGameObject,transform);
    }


}
