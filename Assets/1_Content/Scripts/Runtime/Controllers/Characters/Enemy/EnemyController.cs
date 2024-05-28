using NJN.Runtime.Components;
using NJN.Runtime.Controllers.States;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyController : BaseCharacterController
    {
        #region Components

        public IMovement Movement { get; private set; }
        public IDamageProcessor DamageProcessor { get; private set; }
        public IHealth Health { get; private set; }
        public ILineOfSight LineOfSight { get; private set; }
        public IPatrolDesignator Patrol { get; private set; }
        //public IAttack Attack { get; private set; }
        //public IDistractable Distractable { get; private set; }
        //public IChase Chase { get; private set; }

        #endregion
        
        #region States
        
        public EnemyIdleState IdleState { get; private set; }
        public EnemyPatrolState PatrolState { get; private set; }
        public EnemyChaseState ChaseState { get; private set; }
        public EnemyAttackState AttackState { get; private set; }
        public CharacterBusyState BusyState { get; private set; }
        public CharacterDeadState DeadState { get; private set; }
        public EnemyDistractedState DistractedState { get; private set; }
        
        #endregion
        
        protected override void InitializeStateMachine()
        {
            IdleState = new EnemyIdleState(this, StateMachine);
            PatrolState = new EnemyPatrolState(this, StateMachine);
            ChaseState = new EnemyChaseState(this, StateMachine);
            AttackState = new EnemyAttackState(this, StateMachine);
            DistractedState = new EnemyDistractedState(this, StateMachine);
            BusyState = new CharacterBusyState(this, StateMachine);
            DeadState = new CharacterDeadState(this, StateMachine);
            
            StateMachine.Initialize(BusyState);
        }

        protected override void Awake()
        {
            base.Awake();
            
            Movement = VerifyComponent<IMovement>();
            DamageProcessor = VerifyComponent<IDamageProcessor>();
            Health = VerifyComponent<IHealth>();
            LineOfSight = VerifyComponent<ILineOfSight>();
            Patrol = VerifyComponent<IPatrolDesignator>();
            //Attack = VerifyComponent<IAttack>();
            //Distractable = VerifyComponent<IDistractable>();
            //Chase = VerifyComponent<IChase>();
        }
        
        private void Start()
        {
            StateMachine.ChangeState(IdleState);
        }
    }
}