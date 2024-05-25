using NJN.Runtime.StateMachines;

namespace NJN.Runtime.Controllers.States
{
    public class CharacterBusyState : CharacterState
    {
        public CharacterBusyState(BaseCharacterController controller, ControllerStateMachine<CharacterState, 
            BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}