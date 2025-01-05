using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MouseGridPosition;
using TheLayers;
using Tools.Drawing;
using UI;
using UI.Bottom;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools.Editing
{
    public class Select : ToolAbstract, IRectangleWithSize
    {
        [SerializeField] private InputActionReference leftMouse;
        [SerializeField] private InputActionReference modifierAction;
        [SerializeField] private SelectContainer selectContainer;

        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public event Action<bool, IRectangleWithSize> OnToggle;

        private Mode _mode = Mode.None;
        private LayersManager _layerManager;
        private Vector2 _oldGridPos = Vector2.zero;
        private Vector2 _startPos;
        private Vector2 _endPos;
        private bool _startPointSet;
        private SpriteRenderer _toolSprite;

        private void Start()
        {
            _toolSprite = GetComponent<SpriteRenderer>();
            _toolSprite.enabled = false;
            _layerManager = LayersManager.Instance;
            leftMouse.action.performed += OnLeftMouse;
            leftMouse.action.canceled += OnLeftMouseCancel;
            modifierAction.action.canceled += ModifierStop;
        }

        private void OnEnable()
        {
            leftMouse.action.performed += OnLeftMouse;
            leftMouse.action.canceled += OnLeftMouseCancel;
            modifierAction.action.canceled += ModifierStop;
        }

        private void OnDisable()
        {
            leftMouse.action.performed -= OnLeftMouse;
            leftMouse.action.canceled -= OnLeftMouseCancel;
            modifierAction.action.canceled -= ModifierStop;
            ResetTool();
        }

        private void Update()
        {
            if (_mode is Mode.None) return;
            DragUpdate();
        }

        private void DragUpdate()
        {
            var currentPos = MouseGrid.GridPos;
            if (currentPos == _oldGridPos) return;
            
            float deltaX, deltaY;

            if (currentPos.x < _startPos.x)
            {
                var position = transform.position;
                position.x = currentPos.x;
                transform.position = position;
                deltaX = _startPos.x - currentPos.x + 1;
            }
            else
            {
                var position = transform.position;
                position.x = _startPos.x;
                transform.position = position;
                deltaX = currentPos.x - _startPos.x + 1;
            }

            if (currentPos.y < _startPos.y)
            {
                var position = transform.position;
                position.y = currentPos.y;
                transform.position = position;
                deltaY = _startPos.y - currentPos.y + 1;
            }
            else
            {
                var position = transform.position;
                position.y = _startPos.y;
                transform.position = position;
                deltaY = currentPos.y - _startPos.y + 1;
            }
            SizeX = (int) deltaX;
            SizeY = (int) deltaY;
            transform.localScale = new Vector3(deltaX, deltaY, 1);
            
            _oldGridPos = currentPos;
        }
        
        private void OnLeftMouse(InputAction.CallbackContext context)
        {
            if (PointerOnUI.Instance) return;

            if (_mode == Mode.Click)
            {
                SecondClick();
                return;
            }
            
            OnToggle?.Invoke(true, this);
            _startPos = MouseGrid.GridPos;
            _mode = modifierAction.action.IsPressed() ? Mode.Click : Mode.Drag;
            _toolSprite.enabled = true;
        }
        
        private void OnLeftMouseCancel(InputAction.CallbackContext context)
        {
            if (_mode is Mode.Click or Mode.None) return;
            
            _endPos = MouseGrid.GridPos;
            SelectingCoroutine();
            ResetTool();
        }
        
        private void ModifierStop(InputAction.CallbackContext context)
        {
            ResetTool();
        }

        private void SecondClick()
        {
            _endPos = MouseGrid.GridPos;
            SelectingCoroutine();
            ResetTool();
        }

        private async UniTask SelectingCoroutine()
        {
            var startX = _startPos.x < _endPos.x ? (int)_startPos.x : (int)_endPos.x;
            var endX = _startPos.x > _endPos.x ? (int)_startPos.x : (int)_endPos.x;
            var startY = _startPos.y < _endPos.y ? (int)_startPos.y : (int)_endPos.y;
            var endY = _startPos.y > _endPos.y ? (int)_startPos.y : (int)_endPos.y;

            List<Vector2> positions = new();

            var counter = 0;
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    var position = new Vector3(x, y, _layerManager.CurrentLayer.Order);
                    positions.Add(position);

                    counter++;
                }
            }

            var result = await LayersManager.Instance.GetCellsAsync(positions);
            selectContainer.SetSelectedObjects(result);

            // Debug.Log($"{(endX - startX + 1) * (endY - startY + 1)} cells drawn");
            await UniTask.Yield();
        }

        private void ResetTool()
        {
            OnToggle?.Invoke(false, this);
            transform.localScale = Vector3.one;
            // detectedObjects.Clear();
            _mode = Mode.None;
            _toolSprite.enabled = false;
        }

        private enum Mode
        {
            Drag,
            Click,
            None
        }
    }
}