using System.Collections.Generic;
using UnityEngine;

namespace NJN.Runtime.Systems.Distraction
{
    public interface I_Distractable
    {
        public void Distraction(Vector3 position, float distractionTime);
    }
    public class DistractionSystem 
    {
        public static void FireDistraction(Vector3 position, float radius, float distractTime, LayerMask layerMask)
        {
            foreach (I_Distractable distractable in GetDistractablesInRadius(position, radius, layerMask)) 
            { 
                distractable.Distraction(position, distractTime);
            }
        }

        private static I_Distractable[] GetDistractablesInRadius(Vector3 position, float radius, LayerMask layerMask)
        {
            List<I_Distractable> L_distractables = new List<I_Distractable>();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius, layerMask);
            foreach (Collider2D collider in colliders)
            {
                I_Distractable distractable = collider.GetComponent<I_Distractable>();
                if (distractable != null)
                {
                    L_distractables.Add(distractable);
                }
            }
            return L_distractables.ToArray();
        }
    }
}
