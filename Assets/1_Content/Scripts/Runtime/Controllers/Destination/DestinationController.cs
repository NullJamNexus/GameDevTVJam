using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace NJN.Runtime.Controllers.Destination
{
    public class DestinationController : MonoBehaviour
    {
        public void MoveOldHouse(float accelerationRate, float maxSpeed)
        {
            StartCoroutine(leaveTheScene(accelerationRate, maxSpeed));
        }

        public void MoveNewHouse(float startSpeed, float decelerationRate)
        {
            StartCoroutine(EnterTheScene(startSpeed, decelerationRate));
        }
        
        IEnumerator leaveTheScene(float accelerationRate, float maxSpeed)
        {
            float speed = 0;
            while (true)
            {
                yield return null;
                speed += accelerationRate * Time.deltaTime;
                speed =math.min(speed, maxSpeed);
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
        }
        IEnumerator EnterTheScene(float startSpeed, float decelerationRate)
        {
            float speed = startSpeed;
            while (speed > 0)
            {
                yield return null;
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                speed -= decelerationRate * Time.deltaTime;
                speed = math.max(speed, 0);
            }
        }
    }
}