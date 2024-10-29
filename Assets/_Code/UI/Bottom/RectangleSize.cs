using System;
using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEditor.EditorTools;
using UnityEngine;

namespace UI.Bottom
{
    public class RectangleSize : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        private IRectangleWithSize _currentRectTool;
        private bool _enabled;

        private void Start()
        {
            text.gameObject.SetActive(false);
            foreach (var tool in ToolsManager.Instance.Tools)
            {
                if (tool.tool is IRectangleWithSize rectangleTool)
                {
                    rectangleTool.OnToggle += Toggle;
                }
            }
        }

        private void FixedUpdate()
        {
            if (!_enabled) return;
            text.text = $"[{_currentRectTool.SizeX} ; {_currentRectTool.SizeY}]";
        }
        
        private void Toggle(bool isWorking, IRectangleWithSize rectangleTool)
        {
            _currentRectTool = rectangleTool;
            _enabled = isWorking;
            if (text != null)
                text.gameObject.SetActive(isWorking);
        }
    }
}