using NJN.Runtime.Controllers.Player;

namespace NJN.Runtime.Components
{
    public interface ICollectable
    {
        public void Collect(ISurvivalStats stats);
    }
}