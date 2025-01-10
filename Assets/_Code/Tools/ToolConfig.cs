using TriInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools
{
    [CreateAssetMenu(menuName = "Tools/Tool Config")]
    public class ToolConfig : ScriptableObject
    {
        // [SerializeField] private string toolName;
        [SerializeField] private string tooltip;
        [SerializeField] private Sprite icon;
        [SerializeField] private InputActionReference selectToolAction;
        [SerializeField] private bool isForEditing;
        [Title("Cursor")]
        [Indent, SerializeField] private Texture2D texture;
        [Indent, SerializeField] private Vector2 hotspot;
        
        // public string Name => toolName;
        public string Tooltip => tooltip;
        public Sprite Icon => icon;
        public Texture2D Cursor => texture;
        public Vector2 Hotspot => hotspot;
        public InputAction SelectToolAction => selectToolAction.action;
        public bool IsForEditing => isForEditing;
    }
}