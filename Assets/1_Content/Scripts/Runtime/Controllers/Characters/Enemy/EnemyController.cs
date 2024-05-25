using System;
using System.Collections.Generic;
using NJN.Runtime.Components;
using NJN.Runtime.Controllers.States;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public class EnemyController : BaseCharacterController
    {
        [field: BoxGroup("Settings"), SerializeField]
        public List<Vector2> PatrolPoints { get; private set; }

        

        [field: BoxGroup("Settings"), SerializeField]
        public float PatrolSpeed { get; private set; } = 5f;
        
        #region Components

        public IMovement Movement { get; private set; }
        public IDamageProcessor DamageProcessor { get; private set; }

        #endregion
        
        #region States
        
        public EnemyIdleState IdleState { get; private set; }
        public EnemyPatrolState PatrolState { get; private set; }
        public EnemyChaseState ChaseState { get; private set; }
        public EnemyAttackState AttackState { get; private set; }
        public CharacterBusyState BusyState { get; private set; }
        public CharacterDeadState DeadState { get; private set; }
        
        #endregion
        
        protected override void InitializeStateMachine()
        {
            IdleState = new EnemyIdleState(this, StateMachine);
            PatrolState = new EnemyPatrolState(this, StateMachine);
            ChaseState = new EnemyChaseState(this, StateMachine);
            AttackState = new EnemyAttackState(this, StateMachine);
            BusyState = new CharacterBusyState(this, StateMachine);
            DeadState = new CharacterDeadState(this, StateMachine);
            
            StateMachine.Initialize(BusyState);
        }

        protected override void Awake()
        {
            base.Awake();
            
            Movement = VerifyComponent<IMovement>();
            DamageProcessor = VerifyComponent<IDamageProcessor>();
        }
        
        private void Start()
        {
            StateMachine.ChangeState(IdleState);
        }
    }
}