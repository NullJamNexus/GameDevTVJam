using NJN.Runtime.StateMachines;

namespace NJN.Runtime.Controllers.States
{
    public class CharacterBusyState : CharacterState
    {
        public CharacterBusyState(CharacterController controller, ControllerStateMachine<CharacterState, 
            CharacterController> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}