using UnityEngine;

namespace Player
{
    public class PlayerControls : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private const float Speed = 3;
        
        // Start is called before the first frame update
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                _rigidbody.AddForce(0, 0, Speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                _rigidbody.AddForce(-Speed, 0, 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                _rigidbody.AddForce(Speed, 0, 0);
            }
            if (Input.GetKey(KeyCode.R))
            {
                _rigidbody.AddForce(0, -Speed, 0);
            }
        }
    }
}
