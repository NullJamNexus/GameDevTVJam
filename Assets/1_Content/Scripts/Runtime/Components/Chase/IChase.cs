using UnityEngine;

namespace NJN.Runtime.Components
{
    public interface IChase : IComponent
    {
        public Vector2 Direction { get; }
        
        public void StartChase();

        public void UpdateChase();

        public void CancellChase();
    }
}
