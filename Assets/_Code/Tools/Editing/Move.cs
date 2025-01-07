using System;
using MouseGridPosition;
using UI;
using UI.Bottom;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools.Editing
{
    public class Move : ToolAbstract, IRectangleWithSize
    {
        [SerializeField] private InputActionReference leftMouse;
        [SerializeField] private InputActionReference modifierAction;
        [SerializeField] private LayerMask selectedMask;
        [SerializeField] private SelectContainer selectContainer;

        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public event Action<bool, IRectangleWithSize> OnToggle;

        private bool _isDragging;
        private Vector2 _dragOrigin;
        private Vector2 _oldGridPos;
        
        private void OnEnable()
        {
            leftMouse.action.performed += OnLeftMouseDown;
            leftMouse.action.canceled += OnLeftMouseUp;
        }
        
        private void OnDisable()
        {
            leftMouse.action.performed -= OnLeftMouseDown;
            leftMouse.action.canceled -= OnLeftMouseUp;
            transform.position = Vector3.zero;
        }

        private void Update()
        {
            if (!selectContainer.HasSelection) return;
            
            if (_isDragging)
            {
                MovementUpdate();
            }
        }
        
        private void OnLeftMouseDown(InputAction.CallbackContext context)
        {
            if (!selectContainer.HasSelection && !PointerOnUI.Instance) return;
            if (!IsOnSelected(out var hit)) return;
            _isDragging = true;
            MovementUpdate();
            selectContainer.ReParent(transform);
            _dragOrigin = MouseGrid.GridPos;
            OnToggle?.Invoke(true, this);
        }
        
        private void OnLeftMouseUp(InputAction.CallbackContext context)
        {
            if (!_isDragging) return;
            selectContainer.ResetParent();
            _isDragging = false;
            OnToggle?.Invoke(false, this);
        }
        
        private void MovementUpdate()
        {
            var gridPosition = MouseGrid.GridPos;
            var position = transform.position;
            position.x = gridPosition.x;
            position.y = gridPosition.y;
            transform.position = position;
            
            SizeX = (int) (gridPosition.x - _dragOrigin.x);
            SizeY = (int) (gridPosition.y - _dragOrigin.y);
            // var movement = _dragOrigin - gridPosition;
            // SizeX = (int) movement.x;
            // SizeY = (int) movement.y;
            // var newPos = _startPos - new Vector3(movement.x, movement.y, 0);
            // transform.position = newPos;
        }
        
        private bool IsOnSelected(out RaycastHit hit)
        {
            var gridPosition = MouseGrid.GridPos;
            Vector3 start = new(
                gridPosition.x,
                gridPosition.y,
                -1);
            return Physics.Raycast(start, Vector3.forward, out hit, 15f, selectedMask);
        }
    }
}