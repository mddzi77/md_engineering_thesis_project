using System;
using MdUtils;
using Tools.Editing;
using UI;
using UnityEngine;

namespace Tools
{
    public class ToolsManager : MonoSingleton<ToolsManager>
    {
        [SerializeField] private Texture2D workingCursor;
        [SerializeField] private SelectContainer selectContainer;
        [SerializeField] private ToolHolder[] tools;
        
        public ToolHolder[] Tools => tools;

        private bool _toolIsActive;
        private ToolHolder _currentTool;
        private bool _isWorking;
        
        public void SetCurrentTool(ToolConfig tool)
        {
            if (_toolIsActive)
            {
                _currentTool.tool.gameObject.SetActive(false);
            }
            NewTool(tool);
        }
        
        public void ToggleWorkingCursor(bool isWorking)
        {
            if (isWorking)
            {
                Cursor.SetCursor(workingCursor, Vector2.zero, CursorMode.Auto);
                _isWorking = true;
            }
            else
            {
                _isWorking = false;
                SetCursor(_currentTool.config);
            }
        }
        
        private void NewTool(ToolConfig config)
        {
            foreach (var tool in tools)
            {
                if (tool.config.name.Equals(config.name))
                {
                    // clear selection if tool is not for editing
                    if (!config.IsForEditing)
                    {
                        selectContainer.ClearSelection();
                    }

                    SetCursor(config);
                    _currentTool = tool;
                    _currentTool.tool.gameObject.SetActive(true);
                    _toolIsActive = true;
                    
                    Debug.Log("Set tool to: " + config.name);
                    return;
                }
            }
        }
        
        private void SetCursor(ToolConfig config)
        {
            if (_isWorking) return;
            Cursor.SetCursor(config.Cursor, config.Hotspot, CursorMode.Auto);
        }

        [Serializable]
        public class ToolHolder
        {
            public ToolConfig config;
            public ToolAbstract tool;
        }
    }
}
