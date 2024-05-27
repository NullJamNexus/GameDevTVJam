using System;
using UnityEngine;

namespace NJN.Runtime.Components
{
    public interface IDistractable : IComponent
    {
        public void ThereIsDistraction(Vector3 position, float distractionTime);
        public void StartDistraction(Action EndFunction);
        public void UpdateLogic();
        public void CancellDistraction();

    }
}
