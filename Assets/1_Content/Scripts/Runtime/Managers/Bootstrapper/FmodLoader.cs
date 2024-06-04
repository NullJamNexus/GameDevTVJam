using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using FMODUnity;

namespace NJN.Runtime.Managers.Bootstrapper
{
    public class FmodLoader : MonoBehaviour
    {
        // List of Banks to load
        [BankRef]
        public List<string> Banks = new ();

        public IEnumerator<float> LoadBanks()
        {
            //Iterate all the Studio Banks and start them loading in the background
            foreach (string bank in Banks)
            {
                RuntimeManager.LoadBank(bank, true);
            }
            
            // Wait until all the bank loading is done
            while (!RuntimeManager.HaveAllBanksLoaded)
            {
                yield return Timing.WaitForOneFrame;
            }
            
            // Wait until all the sample data loading is done
            while (RuntimeManager.AnySampleDataLoading())
            {
                yield return Timing.WaitForOneFrame;
            }

            Debug.Log("All FMOD banks and sample data loaded.");
        }
    }
}