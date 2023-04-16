using UnityEngine;

namespace UI
{
    public class Compas : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Update()
        {
            transform.eulerAngles = -target.eulerAngles.y * Vector3.forward;
        }
    }
}
