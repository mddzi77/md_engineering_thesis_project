using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UI.Right
{
    public class VisibilityButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        [SerializeField] private Sprite onIcon;
        [SerializeField] private Sprite offIcon;
        
        public void AddListener(UnityAction onClick)
        {
            button.onClick.AddListener(onClick);
        }
        
        public void ToggleIcon(bool isOn)
        {
            icon.sprite = isOn ? onIcon : offIcon;
        }
    }
}