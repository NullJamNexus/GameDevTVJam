using UnityEngine;
using System.Collections.Generic;
using MEC;
using UnityEngine.SceneManagement;

namespace NJN.Runtime.Managers.Bootstrapper
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField]
        private float _fmodInitializationDelay = 1f;
        //[SerializeField]
        //private SceneCollection _nextCollection;

        private void Start()
        {
            Timing.RunCoroutine(LoadSceneWhenReady().CancelWith(this));
        }

        private IEnumerator<float> LoadSceneWhenReady()
        {
            yield return Timing.WaitForSeconds(_fmodInitializationDelay);

            FmodLoader fmodLoader = GetComponent<FmodLoader>();
            if (fmodLoader != null)
            {
                yield return Timing.WaitUntilDone(Timing.RunCoroutine(fmodLoader.LoadBanks().CancelWith(this)));
            }
            else
            {
                Debug.LogWarning("FmodLoader not found.");
            }

            //_nextCollection.Open();
            SceneManager.LoadScene("3_Level");
        }
    }
}