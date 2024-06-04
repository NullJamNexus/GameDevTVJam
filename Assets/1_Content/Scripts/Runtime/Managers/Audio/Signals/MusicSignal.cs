namespace NJN.Runtime.SoundSignal
{
    public enum EMusic { stop, menu, level, truck }
    public struct MusicSignal
    {
        public EMusic Music { get; private set; }

        public MusicSignal(EMusic music)
        {
            Music = music;
        }
    }
}
