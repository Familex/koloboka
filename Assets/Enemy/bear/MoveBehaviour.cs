using UnityEngine;

namespace Enemy.bear
{
    /// <summary>
    /// State machine behaviour for red barrel move animation.
    /// </summary>
    public class MoveBehaviour : StateMachineBehaviour
    {
        private static readonly int StartMove = Animator.StringToHash("startMove");

        /// <summary>
        /// Update "startMove" trigger when entering the state.
        /// </summary>
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetTrigger(StartMove);
        }
    }
}
