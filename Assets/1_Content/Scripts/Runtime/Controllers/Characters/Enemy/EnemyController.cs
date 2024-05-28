using NJN.Runtime.Components;
using NJN.Runtime.Controllers.States;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyController : BaseCharacterController, IDistractable
    {
        #region Components

        public IMovement Movement { get; private set; }
        public IDamageProcessor DamageProcessor { get; private set; }
        public IHealth Health { get; private set; }
        public ILineOfSight LineOfSight { get; private set; }
        public IPatrolDesignator Patrol { get; private set; }
        public IDistractionProcessor DistractionProcessor { get; private set; }

        #endregion
        
        #region States
        
        // Active
        public EnemyIdleState IdleState { get; private set; }
        public EnemyPatrolState PatrolState { get; private set; }
        public EnemyChaseState ChaseState { get; private set; }
        public EnemyAttackState AttackState { get; private set; }
        // Busy
        public CharacterBusyState BusyState { get; private set; }
        public EnemyDistractedState DistractedState { get; private set; }
        // Dead
        public CharacterDeadState DeadState { get; private set; }
        
        
        #endregion
        
        protected override void InitializeStateMachine()
        {
            // Active
            IdleState = new EnemyIdleState(this, StateMachine);
            PatrolState = new EnemyPatrolState(this, StateMachine);
            ChaseState = new EnemyChaseState(this, StateMachine);
            AttackState = new EnemyAttackState(this, StateMachine);
            // Busy
            BusyState = new CharacterBusyState(this, StateMachine);
            DistractedState = new EnemyDistractedState(this, StateMachine);
            // Dead
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
            DistractionProcessor = VerifyComponent<IDistractionProcessor>();
        }
        
        private void Start()
        {
            StateMachine.ChangeState(IdleState);
        }
        
        public void Distract(Vector2 location)
        {
            float yDistance = Mathf.Abs(location.y - transform.position.y);
            float randomRoll = Random.Range(0f, 100f);
            
            if (yDistance > DistractionProcessor.YMaxDistance || randomRoll < DistractionProcessor.ChanceToResist) 
                return;
            
            DistractionProcessor.SetLocation(location);
            StateMachine.ChangeState(DistractedState);
        }
    }
}