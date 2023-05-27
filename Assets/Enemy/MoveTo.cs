using UnityEngine;
using UnityEngine.AI;

namespace Enemy {
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveTo : MonoBehaviour {
        /* public fields */
        public Transform goal;
        
        /* ---- Private constants ---- */
        private NavMeshAgent _navMeshAgent;
        
        /* ---- Unity overrides ---- */
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (goal is null || _navMeshAgent.isStopped) return;
        
            Debug.Log("MoveTo.Update()");
            _navMeshAgent.destination = goal.position;
            _navMeshAgent.updatePosition = true;
            _navMeshAgent.updateRotation = true;
        }
    }
}
