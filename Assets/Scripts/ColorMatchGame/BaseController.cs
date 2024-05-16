using UIView;
using UnityEngine;

namespace ColorMatchGame
{
    public class BaseController : MonoBehaviour
    {
        [SerializeField] private PanelView _panelView;
        protected void Win<T>() where T : MonoBehaviour
        {
            _panelView.ShowPanel();
            GetComponent<T>().enabled = false;
        }

        protected void Lose()
        {
            
        }

        
    }
}
