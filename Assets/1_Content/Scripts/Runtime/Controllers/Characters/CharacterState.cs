using NJN.Runtime.StateMachines;
using UnityEngine;

namespace NJN.Runtime.Controllers
{
    public abstract class CharacterState : BaseControllerState<CharacterState, BaseCharacterController>
    {
        public CharacterState(BaseCharacterController controller, ControllerStateMachine<CharacterState, 
            BaseCharacterController> stateMachine) : base(controller, stateMachine)
        {
        }
        
        public virtual void OnCollisionEnter(Collision2D collision) { }
        public virtual void OnCollisionStay(Collision2D collision) { }
        public virtual void OnCollisionExit(Collision2D collision) { }
        public virtual void OnTriggerEnter(Collider2D collider) { }
        public virtual void OnTriggerStay(Collider2D collider) { }
        public virtual void OnTriggerExit(Collider2D collider) { }
    }
}