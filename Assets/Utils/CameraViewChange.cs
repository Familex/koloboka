using UnityEngine;

namespace Utils
{
    [System.Serializable]
    public class CamSwitch
    {
        [SerializeField] public Camera cam;
        [SerializeField] public KeyCode key;
            
        public void Deconstruct(out Camera camInner, out KeyCode keyInner)
        {
            camInner = cam;
            keyInner = key;
        }
    }
    
    public class CameraViewChange : MonoBehaviour
    {
        [SerializeField] private CamSwitch[] cameras;
        
        private void OffAll()
        {
            foreach (var cam in cameras)
            {
                cam.cam.enabled = false;
            }
        }
       
        #if UNITY_EDITOR
            private void Start()
            {
                OffAll();
                if (cameras.Length > 0)
                {
                    cameras[0].cam.enabled = true;
                }
            }

            private void Update()
            {
                foreach (var (cam, key) in cameras)
                {
                    if (!Input.GetKey(key)) continue;
                    OffAll();
                    cam.enabled = true;
                }
            }
        #endif
    }
}
