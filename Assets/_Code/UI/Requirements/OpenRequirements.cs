using UnityEngine;
using UnityEngine.UI;

namespace UI.Requirements
{
    public class OpenRequirements : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        private void Start()
        {
            button.onClick.AddListener(Open);
        }
        
        private void Open()
        {
            RequirementsWindow.Instance.OpenWindow();
        }
    }
}