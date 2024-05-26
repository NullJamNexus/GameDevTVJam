using NJN.Runtime.Controllers.Player;

namespace NJN.Runtime.Components
{
    public class ScrapResource : Resource
    {
        public override void Collect(ISurvivalStats player)
        {
            _signalBus.Fire(new ResourceCollectedSignal(this));
            Destroy(gameObject);
        }
    }
}