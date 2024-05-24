using NJN.Runtime.StateMachines;

namespace NJN.Runtime.Controllers.States
{
    public class CharacterActiveState : CharacterState
    {
        public CharacterActiveState(CharacterController controller, ControllerStateMachine<CharacterState, 
            CharacterController> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}