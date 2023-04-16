using Player;
using UnityEngine;

namespace UI
{
    public class Compass : MonoBehaviour
    {
        [SerializeField] private PlayerControls controls;
        
        private void Update()
        {
            transform.eulerAngles = -controls.RelativeY * Vector3.forward;
        }
    }
}
