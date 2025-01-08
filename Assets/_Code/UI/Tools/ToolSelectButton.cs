using System;
using Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class ToolSelectButton : MonoBehaviour
    {
        [SerializeField] private ToolConfig toolConfig;
        [SerializeField] private Image icon;
        [SerializeField] private Tooltip tooltip;

        private ToolsManager _toolsManager;
        private Button _button;
        
        private void Start()
        {
            _toolsManager = ToolsManager.Instance;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
            icon.sprite = toolConfig.Icon;
            tooltip.SetText(toolConfig.Tooltip);
            toolConfig.SelectToolAction.performed += OnToolAction;
        }
        
        private void OnToolAction(InputAction.CallbackContext context)
        {
            OnButtonClick();
        }
        
        private void OnButtonClick()
        {
            _toolsManager.SetCurrentTool(toolConfig);
        }
    }
}