using UnityEngine;

namespace UIView
{
    public class PanelView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;

        public void ShowPanel()
        {
            panel.SetActive(true);
        }

        public void HidePanel()
        {
            panel.SetActive(false);
        }
    }
}
