using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public class ParallaxBuilding : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabTransitionBuilding; // there should not be any collision or trigger or close player 
        List<GameObject> _lBuildingList = new List<GameObject>();
        float _maxSpeed = 25;
        float _moveSpeed;
        float _acceleration = 2;

        private float _spawnX = -5;
        private float _destroyX = +20;
        private float _spawnY = 2;

        private bool _spawnBuilding;

        void Start()
        {
            //StartMovement();
        }

        public void StartMovement()
        {
            _spawnBuilding = true;
            _moveSpeed = 0;
            StartCoroutine(SpeedUp());
            StartCoroutine(Movement());
            StartCoroutine(Spawner());
        }
        public void StopSpawner()
        {
            _spawnBuilding = false;
        }

        private IEnumerator Spawner()
        {
            while (_spawnBuilding)
            {
                yield return new WaitForSeconds(5); //randomize 5 is good for testing
                GameObject building = Instantiate(_prefabTransitionBuilding);
                building.transform.position = new Vector3(_spawnX, _spawnY, 0);
                _lBuildingList.Add((building));
            }
        }

        private IEnumerator SpeedUp()
        {
            while (_moveSpeed < _maxSpeed)
            {
                _moveSpeed += _acceleration * Time.deltaTime;
                yield return null;
            }
            _moveSpeed = _maxSpeed;
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
                    if (pos.x > _destroyX)
                        lDestroyBuilding.Add(Building);
                    else
                        Building.transform.position = pos;
                }
                foreach (GameObject destroyBuilding in lDestroyBuilding)
                {
                    _lBuildingList.Remove(destroyBuilding);
                    Destroy(destroyBuilding);
                    if (_lBuildingList.Count == 0 && !_spawnBuilding)
                        StopAllCoroutines();
                }

            }
        }
    }
}
