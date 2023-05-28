using UnityEngine;

namespace Enemy.red_barrel
{
    /// <summary>
    /// State machine behaviour for the red barrel jump animation.
    /// </summary>
    public class JumpBehaviour : StateMachineBehaviour
    {
        private static readonly int StartJump = Animator.StringToHash("startJump");

        /// <summary>
        /// Update "startJump" trigger when entering the state.
        /// </summary>
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetTrigger(StartJump);
        }
    }
}