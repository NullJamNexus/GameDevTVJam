using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NJN.Scriptables
{
    [CreateAssetMenu(fileName = "AudioEventBinding", menuName = "NJN/Audio/New Binding")]
    public class AudioEventBindingSO : ScriptableObject
    {
        #region Music

        [field: FoldoutGroup("Music"), SerializeField]
        public EventReference Mus_Menu { get; private set; }

        [field: FoldoutGroup("Music"), SerializeField]
        public EventReference Mus_Level { get; private set; }

        [field: FoldoutGroup("Music"), SerializeField]
        public EventReference Mus_Truck { get; private set; }

        #endregion
        #region Ambience

        [field: FoldoutGroup("Ambience"), SerializeField]
        public EventReference Amb_inTruck { get; private set; }

        [field: FoldoutGroup("Ambience"), SerializeField]
        public EventReference Amb_outside{ get; private set; }

        [field: FoldoutGroup("Ambience"), SerializeField]
        public EventReference Amb_inBuilding{ get; private set; }

    #endregion

        #region Player

        [field: FoldoutGroup("Player"), SerializeField]
        public EventReference Move { get; private set; }

        [field: FoldoutGroup("Player"), SerializeField]
        public EventReference TakeDamage { get; private set; }

        [field: FoldoutGroup("Player"), SerializeField]
        public EventReference ClimbingLadder { get; private set; }

        [field: FoldoutGroup("Player"), SerializeField]
        public EventReference DrinkingWater { get; private set; }

        [field: FoldoutGroup("Player"), SerializeField]
        public EventReference EatingFood { get; private set; }

        [field: FoldoutGroup("Player"), SerializeField]
        public EventReference GatherResource { get; private set; }

        [field: FoldoutGroup("Player"), SerializeField]
        public EventReference UsingStairs { get; private set; }

        [field: FoldoutGroup("Player"), SerializeField]
        public EventReference StepingOnBrokenGlass { get; private set; }

        #endregion

        #region Interaction

        [field: FoldoutGroup("Interaction"), SerializeField]
        public EventReference CookingOnStove{ get; private set; }

        [field: FoldoutGroup("Interaction"), SerializeField]
        public EventReference DrinkingFromCooler { get; private set; }

        [field: FoldoutGroup("Interaction"), SerializeField]
        public EventReference PickUpANote { get; private set; }

        [field: FoldoutGroup("Interaction"), SerializeField]
        public EventReference OpenDoor { get; private set; }

        [field: FoldoutGroup("Interaction"), SerializeField]
        public EventReference CloseDoor { get; private set; }

        [field: FoldoutGroup("Interaction"), SerializeField] 
        public EventReference Hide { get; private set; }

        [field: FoldoutGroup("Interaction"), SerializeField]
        public EventReference EndHide { get; private set; }

        [field: FoldoutGroup("Interaction"), SerializeField]
        public EventReference AlarmClock { get; private set; }

        #endregion

        #region Enemy

        [field: FoldoutGroup("Enemy"), SerializeField] 
        public EventReference EnemyMove { get; private set; }

        [field: FoldoutGroup("Enemy"), SerializeField]
        public EventReference EnemyAttack { get; private set; }

        #endregion
    }
}