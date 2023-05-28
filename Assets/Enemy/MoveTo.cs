using UnityEngine;
using UnityEngine.AI;

namespace Enemy {
    /// <summary>
    /// MonoBehaviour that moves the GameObject to a given goal.
    /// FIXME: unused.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveTo : MonoBehaviour {
        public Transform goal;
        
        private NavMeshAgent _navMeshAgent;
        
        /// <summary>
        /// Get the NavMeshAgent component.
        /// </summary>
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// Update the destination of the NavMeshAgent.
        /// </summary>
        private void Update()
        {
            if (goal is null || _navMeshAgent.isStopped) return;
        
            _navMeshAgent.destination = goal.position;
            _navMeshAgent.updatePosition = true;
            _navMeshAgent.updateRotation = true;
        }
    }
}
