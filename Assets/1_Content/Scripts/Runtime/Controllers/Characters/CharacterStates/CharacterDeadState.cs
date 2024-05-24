using NJN.Runtime.StateMachines;

namespace NJN.Runtime.Controllers.States
{
    public class CharacterDeadState : CharacterState
    {
        public CharacterDeadState(CharacterController controller, ControllerStateMachine<CharacterState, 
            CharacterController> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}