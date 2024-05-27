using System.Collections.Generic;
using UnityEngine;
using NJN.Runtime.Components;

namespace NJN.Runtime.Systems.Distraction
{
    public class DistractionSystem 
    {
        public static void FireDistraction(Vector3 position, float radius, float distractTime, LayerMask layerMask)
        {
            foreach (IDistractable distractable in GetDistractablesInRadius(position, radius, layerMask)) 
            { 
                distractable.ThereIsDistraction(position, distractTime);
            }
        }

        private static IDistractable[] GetDistractablesInRadius(Vector3 position, float radius, LayerMask layerMask)
        {
            List<IDistractable> L_distractables = new List<IDistractable>();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius, layerMask);
            foreach (Collider2D collider in colliders)
            {
                IDistractable distractable = collider.GetComponent<IDistractable>();
                if (distractable != null)
                {
                    L_distractables.Add(distractable);
                }
            }
            return L_distractables.ToArray();
        }
    }
}
