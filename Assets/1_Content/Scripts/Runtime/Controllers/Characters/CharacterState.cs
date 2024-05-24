using NJN.Runtime.StateMachines;

namespace NJN.Runtime.Controllers
{
    public abstract class CharacterState : BaseControllerState<CharacterState, CharacterController>
    {
        public CharacterState(CharacterController controller, ControllerStateMachine<CharacterState, 
            CharacterController> stateMachine) : base(controller, stateMachine)
        {
        }
    }
}