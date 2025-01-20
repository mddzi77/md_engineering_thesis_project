using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MouseGridPosition;
using TheLayers;
using TriInspector;
using UI;
using UI.Bottom;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools.Editing
{
    public class RectangleErase : ToolAbstract, IRectangleWithSize
    {
        [SerializeField] private InputActionReference leftMouse;
        [SerializeField] private InputActionReference modifierAction;
        [SerializeField] private SpriteRenderer toolSprite;
        [SerializeField] private BoxCollider detector;
        [SerializeField] private string tooltip;
        [ReadOnly]
        [SerializeField] private List<GameObject> detectedObjects;

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
        }

        private void OnEnable()
        {
            ShowTooltip(tooltip);
            EnableInput();
        }

        private void OnDisable()
        {
            HideTooltip();
            DisableInput();
            ResetTool();
        }

        private void Update()
        {
            if (_mode is Mode.None) return;
            DragUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            detectedObjects.Add(other.gameObject);
        }
        
        private void OnTriggerExit(Collider other)
        {
            detectedObjects.Remove(other.gameObject);
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
            ModifyBounds(deltaX, deltaY);
            SizeX = (int) deltaX;
            SizeY = (int) deltaY;
            transform.localScale = new Vector3(deltaX, deltaY, 1);
            
            _oldGridPos = currentPos;
        }

        private async UniTask ErasingCoroutine()
        {
            DisableInput(); // prevent tool usage while drawing
            ToolsManager.Instance.ToggleWorkingCursor(true);
            _mode = Mode.None;
            
            LayersManager.Instance.ReturnCells(detectedObjects);
            
            await UniTask.Yield();
            
            ToolsManager.Instance.ToggleWorkingCursor(false);
            EnableInput();
            ResetTool();
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
            ErasingCoroutine().Forget();
        }
        
        private void ModifierStop(InputAction.CallbackContext context)
        {
            ResetTool();
        }

        private void SecondClick()
        {
            _endPos = MouseGrid.GridPos;
            DragUpdate();
            ErasingCoroutine().Forget();
        }
        
        private void ModifyBounds(float deltaX, float deltaY)
        {
            detector.size = new Vector3(1 - 0.1f / deltaX, 1 - 0.1f / deltaY, 20);
        }

        private bool CanDraw(Vector3 position)
        {
            for (var i = 0; i < detectedObjects.Count; i++)
            {
                var detected = detectedObjects[i];
                if (!Mathf.Approximately(position.x, detected.transform.position.x) ||
                    !Mathf.Approximately(position.y, detected.transform.position.y)) continue;
                detectedObjects.Remove(detected);
                return false;
            }

            return true;
        }

        private void ResetTool()
        {
            OnToggle?.Invoke(false, this);
            transform.localScale = Vector3.one;
            detectedObjects.Clear();
            _mode = Mode.None;
            _toolSprite.enabled = false;
        }

        private void EnableInput()
        {
            leftMouse.action.performed += OnLeftMouse;
            leftMouse.action.canceled += OnLeftMouseCancel;
            modifierAction.action.canceled += ModifierStop;
        }
        
        private void DisableInput()
        {
            leftMouse.action.performed -= OnLeftMouse;
            leftMouse.action.canceled -= OnLeftMouseCancel;
            modifierAction.action.canceled -= ModifierStop;
        }

        private enum Mode
        {
            Drag,
            Click,
            None
        }
    }
}