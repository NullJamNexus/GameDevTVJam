using System;
using System.Collections.Generic;
using System.Linq;
using NJN.Runtime.Components;
using NJN.Runtime.StateMachines;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Runtime.Controllers
{
    public abstract class BaseController<TController, TState> : MonoBehaviour
        where TController : BaseController<TController, TState>
        where TState : BaseControllerState<TState, TController>
    {
        [field: FoldoutGroup("General"), SerializeField, ReadOnly]
        public string StateName { get; set; }
        
        private List<IComponent> _components = new();
        
        public ControllerStateMachine<TState, TController> StateMachine { get; private set; }
        
        protected virtual void Awake()
        {
            GetComponentsInChildren(_components);
            StateMachine = new ControllerStateMachine<TState, TController>((TController)this);
            InitializeStateMachine();
        }

        protected abstract void InitializeStateMachine();

        protected virtual void Update()
        {
            StateMachine.CurrentState.LogicUpdate();
        }
        
        protected virtual void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }
        
        protected virtual void OnDestroy()
        {
            StateMachine.CleanUp();
        }

        protected T VerifyComponent<T>() where T : IComponent
        {
            EnsureInterface<T>();
            if (TryGetControllerComponent(out T component))
                return component;
            
            Debug.LogError($"[BaseController] Missing a critical component of type {typeof(T)}.", this);
            return default;
        }
        
        public T GetControllerComponent<T>() where T : IComponent
        {
            EnsureInterface<T>();
            return _components.OfType<T>().FirstOrDefault();
        }
        
        public bool TryGetControllerComponent<T>(out T component) where T : IComponent
        {
            EnsureInterface<T>();
            component = _components.OfType<T>().FirstOrDefault();
            return component != null;
        }
        
        private void EnsureInterface<T>()
        {
            if (!typeof(T).IsInterface)
            {
                throw new InvalidOperationException($"Type {typeof(T).Name} must be an interface.");
            }
        }
    }
}