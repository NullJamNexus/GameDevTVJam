using System;

namespace NJN.Runtime.Managers.Scenes.Signals
{
    public class LoadingFadeSignal
    {
        public enum FadeDirection
        {
            In,
            Out
        }

        public FadeDirection Direction { get; }
        public Action OnFadeComplete { get; }

        public LoadingFadeSignal(FadeDirection direction, Action onFadeComplete)
        {
            Direction = direction;
            OnFadeComplete = onFadeComplete;
        }
    }
}