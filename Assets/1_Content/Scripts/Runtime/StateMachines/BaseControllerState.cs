using NJN.Runtime.Controllers;

namespace NJN.Runtime.StateMachines
{
    public abstract class BaseControllerState<TState, TController> 
        where TState : BaseControllerState<TState, TController>
        where TController : BaseController<TController, TState>
    {
        protected ControllerStateMachine<TState, TController> _stateMachine;
        protected TController _controller;

        public BaseControllerState(TController controller, ControllerStateMachine<TState, TController> stateMachine)
        {
            _controller = controller;
            _stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate() { }
        public virtual void Exit() { }
    }
}