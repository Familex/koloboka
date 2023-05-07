using UnityEngine;

namespace Enemy
{
    public class SphereCollider : MonoBehaviour
    {
        public Transform target;
        public float radius;
        public delegate void OnCollideDelegate();

        public event OnCollideDelegate OnCollide;

        private void Update()
        {
            if (Vector3.Distance(transform.position, target.position) < radius)
            {
                OnCollide?.Invoke();
            }
        }
    }
}