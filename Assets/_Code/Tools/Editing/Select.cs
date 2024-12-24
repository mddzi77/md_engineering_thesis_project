using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools.Editing
{
    public class Select : ToolAbstract
    {
        [SerializeField] private InputActionReference selectAction;
        [SerializeField] private InputActionReference modifierAction;
        [SerializeField] private SpriteRenderer selectionSprite;
        
        
    }
}