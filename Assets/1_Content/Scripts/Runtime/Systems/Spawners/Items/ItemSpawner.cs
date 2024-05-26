using NJN.Runtime.Components;
using NJN.Runtime.Factories;
using UnityEngine;

namespace NJN.Runtime.Systems.Spawners
{
    public class ItemSpawner : IItemSpawner
    {
        private float _xMin = -7f;
        private float _xMax = 7f;
        private float _yMin = -3.6f;
        private float _yMax = -3f;
        
        private IItemFactory _itemFactory;
        private GameObject _parentGameObject;
        private string _parentGameObjectName = "Items";
        
        public ItemSpawner(IItemFactory itemFactory)
        {
            _itemFactory = itemFactory;

            // TODO: Make this more efficient.
            _parentGameObject = GameObject.Find(_parentGameObjectName);
            if (_parentGameObject == null)
            {
                _parentGameObject = new GameObject(_parentGameObjectName);
            }
        }
        
        public void SpawnItem()
        {
            Vector2 spawnPosition = new (Random.Range(_xMin, _xMax), Random.Range(_yMin, _yMax));
            Resource resource =  _itemFactory.CreateRandomResource();
            resource.transform.position = spawnPosition;
            resource.transform.SetParent(_parentGameObject.transform);
        }
    }
}