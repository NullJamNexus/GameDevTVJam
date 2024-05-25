using NJN.Runtime.StateMachines;

namespace NJN.Runtime.Controllers.States
{
    public class CharacterDeadState : CharacterState
    {
        public CharacterDeadState(BaseCharacterController controller, ControllerStateMachine<CharacterState, 
            BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}