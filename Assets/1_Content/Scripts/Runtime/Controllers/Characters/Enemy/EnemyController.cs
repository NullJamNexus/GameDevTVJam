using System;
using System.Collections.Generic;
using NJN.Runtime.Components;
using NJN.Runtime.Controllers.States;
using NJN.Runtime.Systems.Distraction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Controllers.Enemy
{
    public enum E_FaceDirection {right, left};
    public class EnemyController : BaseCharacterController
    {
        [field: BoxGroup("Settings"), SerializeField]
        public List<Vector2> PatrolPoints { get; private set; }

        

        [field: BoxGroup("Settings"), SerializeField]
        public float PatrolSpeed { get; private set; } = 5f;


        public E_FaceDirection FaceDirection { get; private set; }
        
        #region Components

        public IMovement Movement { get; private set; }
        public IDamageProcessor DamageProcessor { get; private set; }
        public IHealth Health { get; private set; }
        public IAttack Attack { get; private set; }
        public IDistractable Distractable { get; private set; }
        public IChase Chase { get; private set; }

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
            BusyState = new CharacterBusyState(this, StateMachine);
            DeadState = new CharacterDeadState(this, StateMachine);
            DistractedState = new EnemyDistractedState(this, StateMachine);
            
            StateMachine.Initialize(BusyState);
        }

        protected override void Awake()
        {
            base.Awake();
            
            Movement = VerifyComponent<IMovement>();
            DamageProcessor = VerifyComponent<IDamageProcessor>();
            Health = VerifyComponent<IHealth>();
            Attack = VerifyComponent<IAttack>();
            Distractable = VerifyComponent<IDistractable>();
            Chase = VerifyComponent<IChase>();
        }
        
        private void Start()
        {
            StateMachine.ChangeState(IdleState);
        }

        public void ChangeFaceDirection(E_FaceDirection newDirection)
        {
            FaceDirection = newDirection;
        }
        public void ChangeFaceDirection(float direction)
        {
            if(direction < 0)
            {
                ChangeFaceDirection(E_FaceDirection.left);
            }
            else
            {
                ChangeFaceDirection(E_FaceDirection.right);
            }
        }

        public int GetFaceDirectionAsValue()
        {
            if (FaceDirection == E_FaceDirection.right)
                return 1;
            return -1;
        }
    }
}