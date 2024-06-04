using NJN.Runtime.Controllers.Enemy;
using NJN.Runtime.Controllers.Player;

namespace NJN.Runtime.Factories
{
    public interface ICharacterFactory
    {
        public PlayerController CreatePlayer();
        public EnemyController CreateEnemy();
    }
}