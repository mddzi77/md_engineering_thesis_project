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
        [SerializeField] private InputActionReference toolAction;

        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
            toolAction.action.performed += OnToolAction;
        }
        
        private void OnToolAction(InputAction.CallbackContext context)
        {
            OnButtonClick();
        }
        
        private void OnButtonClick()
        {
            ToolsManager.Instance.SetCurrentTool(toolConfig);
        }
    }
}