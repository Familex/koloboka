using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// MonoBehaviour that shows the player's position in the UI.
    /// </summary>
    public class ShowPlayerPosition : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Text text;

        /// <summary>
        /// Updates the text to show the player's position.
        /// </summary>
        private void Update()
        {
            text.text = playerTransform.position.ToString();
        }
    }
}
