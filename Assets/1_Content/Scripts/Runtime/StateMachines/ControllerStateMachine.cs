using NJN.Runtime.Controllers;

namespace NJN.Runtime.StateMachines
{
    public class ControllerStateMachine<TState, TController> 
        where TState : BaseControllerState<TState, TController>
        where TController : BaseController<TController, TState>
    {
        public TState CurrentState { get; private set; }
        public TState PreviousState { get; private set; }
        public TController Controller { get; private set; }

        public ControllerStateMachine(TController controller)
        {
            Controller = controller;
        }

        public void Initialize(TState startState)
        {
            CurrentState = startState;
            PreviousState = startState;
            CurrentState.Enter();
        }

        public void ChangeState(TState newState)
        {
            CurrentState.Exit();
            PreviousState = CurrentState;
            CurrentState = newState;
            CurrentState.Enter();
        }
        
        public void CleanUp()
        {
            CurrentState.Exit();
        }
    }
}