using System;
using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEngine;

namespace UI.Bottom
{
    public class RectangleSize : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private string unit = "um";
        [SerializeField] private float unityMultiplier = 0.3f;

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
            var sizeX = _currentRectTool.SizeX;
            var sizeY = _currentRectTool.SizeY;
            text.text = $"[{sizeX} ({sizeX*unityMultiplier} {unit}) ; {sizeY} ({sizeY*unityMultiplier} {unit})]";
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