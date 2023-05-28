using UnityEngine;

namespace Utils
{
    /// <summary>
    /// MonoBehaviour that rotates the object it is attached to.
    /// </summary>
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private Vector3 direction = new(0, 1, 0);

        /// <summary>
        /// Rotates the object it is attached to.
        /// </summary>
        private void FixedUpdate()
        {
            transform.Rotate(direction.normalized * (speed * Time.deltaTime));
        }
    }
}
