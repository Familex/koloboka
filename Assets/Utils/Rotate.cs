using UnityEngine;

namespace Utils
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private float speed = 1;
        [SerializeField] private Vector3 direction = new(0, 1, 0);

        private void FixedUpdate()
        {
            transform.Rotate(direction.normalized * (speed * Time.deltaTime));
        }
    }
}
