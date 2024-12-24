using System;
using UI.Bottom;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools.Editing
{
    public class Select : ToolAbstract, IRectangleWithSize
    {
        [SerializeField] private InputActionReference selectAction;
        [SerializeField] private InputActionReference modifierAction;
        [SerializeField] private SpriteRenderer selectionSprite;


        public int SizeX { get; }
        public int SizeY { get; }
        public event Action<bool, IRectangleWithSize> OnToggle;
        
        
    }
}