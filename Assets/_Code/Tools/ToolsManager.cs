using System;
using MdUtils;
using Tools.Editing;
using UI;
using UnityEngine;

namespace Tools
{
    public class ToolsManager : MonoSingleton<ToolsManager>
    {
        [SerializeField] private SelectContainer selectContainer;
        [SerializeField] private ToolHolder[] tools;
        
        public ToolHolder[] Tools => tools;

        private bool _toolIsActive;
        private ToolHolder _currentTool;
        
        public void SetCurrentTool(ToolConfig tool)
        {
            if (_toolIsActive)
            {
                _currentTool.tool.gameObject.SetActive(false);
            }
            if (selectContainer.HasSelection)
            {
                selectContainer.ClearSelection();
            }
            NewTool(tool);
        }
        
        private void NewTool(ToolConfig config)
        {
            foreach (var tool in tools)
            {
                if (tool.config.name.Equals(config.name))
                {
                    Debug.Log("Set tool to: " + config.name);
                    _currentTool = tool;
                    _currentTool.tool.gameObject.SetActive(true);
                    _toolIsActive = true;
                    return;
                }
            }
        }

        [Serializable]
        public class ToolHolder
        {
            public ToolConfig config;
            public ToolAbstract tool;
        }
    }
}
