using FMOD.Studio;
using FMODUnity;

namespace NJN.Runtime.Fmod
{
    public class FmodCommunication
    {
        public void PlayOneShot(EventReference eventReference)
        {
            RuntimeManager.PlayOneShot(eventReference);
        }
        public void SetInstance(ref EventInstance eventInstance, EventReference eventReference)
        {
            RelaeseInstance(ref eventInstance);
            eventInstance = RuntimeManager.CreateInstance(eventReference);
        }

        public void PlayInstance(ref EventInstance eventInstance)
        {
            if(eventInstance.isValid())
                eventInstance.start();
        }

        public void RelaeseInstance(ref EventInstance eventInstance)
        {
            if (eventInstance.isValid())
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
        }
        public void SetParameter(ref EventInstance eventInstance, string parameterName, float value)
        {
            if (eventInstance.isValid())
            {
                eventInstance.setParameterByName(parameterName, value);
            }
        }
        public void SetGlobalParameter(string parameterName, float value)
        {
            RuntimeManager.StudioSystem.setParameterByName(parameterName, value);
        }
    }
}
