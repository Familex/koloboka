using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShowPlayerPosition : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Text text;

        private void Update()
        {
            text.text = playerTransform.position.ToString();
        }
    }
}
