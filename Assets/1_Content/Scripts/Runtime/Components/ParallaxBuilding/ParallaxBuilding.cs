using Mono.CSharp;
using NUnit.Framework;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class ParallaxBuilding : MonoBehaviour
{
    [SerializeField] private GameObject _prefabBuilding;
    List<GameObject> _lBuildingList = new List<GameObject>();
    float _moveSpeed = 5;

    private float _spawnX = -5;
    private float _destroyX = +20;
    private float _spawnY = 2;

    [SerializeField] private float NewSpeed = 1;

    private Transform _newDestination;
    void Start()
    {
        //StartMovement();
    }

    [Button(ButtonSizes.Large)]
    public void SetSpeed()
    {
        _moveSpeed = NewSpeed;
    }
    private void StartMovement()
    {
        StartCoroutine(Movement());
        StartCoroutine(Spawner());
    }
    public void StopSpawner()
    {
        StopAllCoroutines();
    }

    private IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            GameObject building = Instantiate(_prefabBuilding);
            building.transform.position = new Vector3(_spawnX, _spawnY, 0); 
            _lBuildingList.Add((building));
        }
    }
    private IEnumerator Movement()
    {
        while (true)
        {
            yield return null;
            List<GameObject> lDestroyBuilding = new List<GameObject>();
            foreach (GameObject Building in _lBuildingList)
            {
                Vector3 pos = Building.transform.position;
                pos.x += _moveSpeed * Time.deltaTime;
                if (pos.x>_destroyX)
                    lDestroyBuilding.Add(Building);
                else
                    Building.transform.position = pos;  
            }
            foreach (GameObject destroyBuilding in lDestroyBuilding)
            {
                _lBuildingList.Remove(destroyBuilding);
                Destroy(destroyBuilding);
            }
            
        }
    }
 
}
