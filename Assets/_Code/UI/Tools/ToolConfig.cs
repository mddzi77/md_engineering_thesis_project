using MdUtils.InputCombos;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    [CreateAssetMenu(menuName = "Tools/Tool Config")]
    public class ToolConfig : ScriptableObject
    {
        [SerializeField] private string toolName;
        [SerializeField] private Sprite icon;
        [SerializeField] private InputActionReference selectToolAction;
        [SerializeField] private bool isForEditing;
        
        public string Name => toolName;
        public Sprite Icon => icon;
        public InputAction SelectToolAction => selectToolAction.action;
        public bool IsForEditing => isForEditing;
    }
}