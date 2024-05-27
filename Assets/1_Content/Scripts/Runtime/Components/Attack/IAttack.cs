using System;

namespace NJN.Runtime.Components
{
    public interface IAttack : IComponent
    {
        public void StartAttack(Action AttackEnd);
        public void UpdateAttack();
        public void CancellAttack();
    }
}
