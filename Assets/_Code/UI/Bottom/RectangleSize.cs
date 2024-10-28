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
        
        private List<IRectangleWithSize> _rectangleTools = new();
        private bool _enabled = true;

        private void Start()
        {
            foreach (var tool in ToolsManager.Instance.Tools)
            {
                if (tool.tool is IRectangleWithSize rectangleTool)
                {
                    _rectangleTools.Add(rectangleTool);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!IsEnabled()) return;
        }
        
        private bool IsEnabled()
        {
            if (_rectangleTools.Count == 0)
            {
                if (!_enabled) return false;
                text.gameObject.SetActive(false);
                _enabled = false;
                return false;
            }
            
            foreach (var tool in _rectangleTools)
            {
                if (tool.IsWorking)
                {
                    if (!_enabled)
                    {
                        text.gameObject.SetActive(true);
                        _enabled = true;
                    }
                    text.text = $"[{tool.SizeX} ; {tool.SizeY}]";
                    return true;
                }
            }

            if (!_enabled) return false;
            text.gameObject.SetActive(false);
            _enabled = false;
            return false; 
        }
    }
}