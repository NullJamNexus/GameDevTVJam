using NJN.Runtime.Controllers.Player;

namespace NJN.Runtime.Factories
{
    public interface IPlayerFactory
    {
        public PlayerController Create();
    }
}