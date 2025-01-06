using System;
using UI.Bottom;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools.Editing
{
    public class Move : ToolAbstract, IRectangleWithSize
    {
        [SerializeField] private InputActionReference leftMouse;


        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public event Action<bool, IRectangleWithSize> OnToggle;
        
        
    }
}