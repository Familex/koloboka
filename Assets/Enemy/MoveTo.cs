using UnityEngine;
using UnityEngine.AI;

namespace Enemy {
    [RequireComponent(typeof(NavMeshAgent))]
    public class MoveTo : MonoBehaviour {
        /* ---- Inspector things ---- */
        [SerializeField] private Transform goal;
        
        /* ---- Private constants ---- */
        private NavMeshAgent _navMeshAgent;
        
        /* ---- Unity overrides ---- */
        private void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
        }

        private void Update () {
            _navMeshAgent.destination = goal.position; 
        }
    }
}
