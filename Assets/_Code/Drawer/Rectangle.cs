using System.Numerics;
using MouseGridPosition;
using TheLayers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Drawer
{
    public class Rectangle : DrawerAbstract
    {
        [SerializeField] private InputActionReference leftMouse;
        [SerializeField] private InputActionReference modifierAction;
        [SerializeField] private SpriteRenderer layerSprite;
        [SerializeField] private Collider detector;
        
        private Mode _mode = Mode.None;
        private LayersManager _layerManager;
        private Vector2 _oldGridPos = Vector2.zero;
        private Vector2 _startPos;
        private Vector2 _endPos;
        private bool _startPointSet;

        private void Start()
        {
            _layerManager = LayersManager.Instance;
            leftMouse.action.performed += OnLeftMouse;
            leftMouse.action.canceled += OnLeftMouseCancel;
            modifierAction.action.canceled += ModifierStop;
        }

        private void Update()
        {
            if (_mode != Mode.Drag) return;
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
            transform.localScale = new Vector3(deltaX, deltaY, 1);
            
            _oldGridPos = currentPos;
        }

        private void Draw()
        {
            
        }
        
        private void OnLeftMouse(InputAction.CallbackContext context)
        {
            if (PointerOnUI.Instance) return;

            if (_mode == Mode.Click)
            {
                SecondClick();
                return;
            }
            
            _startPos = MouseGrid.GridPos;
            layerSprite.sprite = _layerManager.CurrentLayer.Sprite;
            _mode = modifierAction.action.IsPressed() ? Mode.Click : Mode.Drag;
        }
        
        private void OnLeftMouseCancel(InputAction.CallbackContext context)
        {
            if (_mode is Mode.Click or Mode.None) return;
            
            _endPos = MouseGrid.GridPos;
            Draw();
            ResetTool();
        }
        
        private void ModifierStop(InputAction.CallbackContext context)
        {
            ResetTool();
        }

        private void SecondClick()
        {
            _endPos = MouseGrid.GridPos;
            DragUpdate();
            Draw();
            ResetTool();
        }

        private void ResetTool()
        {
            transform.localScale = Vector3.one;
            layerSprite.sprite = null;
            _mode = Mode.None;
        }

        private enum Mode
        {
            Drag,
            Click,
            None
        }
    }
}