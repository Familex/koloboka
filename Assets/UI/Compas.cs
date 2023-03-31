using UnityEngine;

public class Compas : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void Update()
    {
        transform.eulerAngles = _target.eulerAngles.y * Vector3.forward;
        // transform.eulerAngles = _target.eulerAngles.x * Vector3.up;
    }
}
