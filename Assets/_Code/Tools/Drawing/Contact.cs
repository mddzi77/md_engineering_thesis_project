using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MouseGridPosition;
using TheLayers;
using TheLayers.Cells;
using TriInspector;
using UI;
using UI.Bottom;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools.Drawing
{
    public class Contact : ToolAbstract, IRectangleWithSize
    {
        [SerializeField] private InputActionReference leftMouse;
        [SerializeField] private InputActionReference modifierAction;
        [SerializeField] private SpriteRenderer layerSprite;
        [SerializeField] private BoxCollider detector;
        [SerializeField] private LayerConfig layerConfig;
        [SerializeField] private string tooltip;
        [ReadOnly]
        [SerializeField] private List<GameObject> detectedObjects;

        public int SizeX { get; private set; }
        public int SizeY { get; private set; }
        public event Action<bool, IRectangleWithSize> OnToggle;

        private Mode _mode = Mode.None;
        private LayersManager _layerManager;
        private LayerHolder _layerHolder;
        private Vector2 _oldGridPos = Vector2.zero;
        private Vector2 _startPos;
        private Vector2 _endPos;
        private bool _startPointSet;
#if UNITY_EDITOR
        private int _counter;
#endif
        private void Start()
        {
            _layerManager = LayersManager.Instance;
            _layerHolder = _layerManager.LayerHolders[layerConfig];
        }

        private void OnEnable()
        {
            ShowTooltip(tooltip, 5);
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
            InfoPanel.Instance.SetErrorText("Can't overlap with other contacts");
            detectedObjects.Add(other.gameObject);
        }
        
        private void OnTriggerExit(Collider other)
        {
            HideTooltip();
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
            // transform.localScale = new Vector3(deltaX, deltaY, 1);
            
            _oldGridPos = currentPos;
        }

        private async UniTask DrawingCoroutine()
        {
            if (detectedObjects.Count == 0)

            {
                DisableInput(); // prevent tool usage while drawing
                ToolsManager.Instance.ToggleWorkingCursor(true);
                _mode = Mode.None;

                bool canDraw = true;

                var startX = _startPos.x < _endPos.x ? (int)_startPos.x : (int)_endPos.x;
                var endX = _startPos.x > _endPos.x ? (int)_startPos.x : (int)_endPos.x;
                var startY = _startPos.y < _endPos.y ? (int)_startPos.y : (int)_endPos.y;
                var endY = _startPos.y > _endPos.y ? (int)_startPos.y : (int)_endPos.y;

                var counter = 0;
                for (int x = startX; x <= endX; x++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        var position = new Vector3(x, y, transform.position.z);
                        if (!CheckLayers(position))
                        {
                            canDraw = false;
                            break;
                        }
                    }
                }

                if (canDraw) Draw();
            }
            
            
            await UniTask.Yield();
            
            ToolsManager.Instance.ToggleWorkingCursor(false);
            EnableInput();
            ResetTool();
        }

        private void Draw()
        {
            // var contact = new GameObject("Contact");
            // contact.transform.position = new Vector3(_startPos.x, _startPos.y, transform.position.z);
            // contact.layer = LayerMask.NameToLayer("Contact");
            var position = transform.position;
            position.x = _startPos.x < _endPos.x ? _startPos.x : _endPos.x;
            position.y = _startPos.y < _endPos.y ? _startPos.y : _endPos.y;
            var contact = CellsPool.GetCell(layerConfig);
            contact.transform.position = position;
            
            var sprite = contact.GetComponent<SpriteRenderer>();
            sprite.drawMode = SpriteDrawMode.Sliced;
            sprite.size = new Vector2(SizeX, SizeY);
            
            var bounds = contact.GetComponent<BoxCollider>();
            bounds.center = new Vector3((float)SizeX / 2, (float)SizeY / 2, 5);
            bounds.size = new Vector3(SizeX, SizeY, 10);
            
            _layerHolder.NewCell(contact);
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
            layerSprite.enabled = true;
            _mode = modifierAction.action.IsPressed() ? Mode.Click : Mode.Drag;
        }
        
        private void OnLeftMouseCancel(InputAction.CallbackContext context)
        {
            if (_mode is Mode.Click or Mode.None) return;
            
            _endPos = MouseGrid.GridPos;
            DrawingCoroutine().Forget();
        }
        
        private void ModifierStop(InputAction.CallbackContext context)
        {
            ResetTool();
        }

        private void SecondClick()
        {
            _endPos = MouseGrid.GridPos;
            DrawingCoroutine().Forget();
        }
        
        private void ModifyBounds(float deltaX, float deltaY)
        {
            // detector.size = new Vector3(1 - 0.1f / deltaX, 1 - 0.1f / deltaY, 0.1f);
            detector.size = new Vector3(deltaX - 0.1f, deltaY - 0.1f, 0.1f);
            detector.center = new Vector3(deltaX / 2, deltaY / 2, 0);
            layerSprite.size = new Vector2(deltaX, deltaY);
        }

        private bool CheckLayers(Vector3 position)
        {
            position.x += 0.5f;
            position.y += 0.5f;
            RaycastHit[] results = new RaycastHit[3];
            
            Debug.DrawRay(position, Vector3.forward * 10, Color.blue, 2f);
            
            var size = Physics.RaycastNonAlloc(position, Vector3.forward, results, 10);
            if (size == 2) return true;
            InfoPanel.Instance.SetErrorText("Contact must be drawn on two overlapping layers", 1f);
            return false;

        }

        private void ResetTool()
        {
            OnToggle?.Invoke(false, this);
            transform.localScale = Vector3.one;
            layerSprite.enabled = false;
            detectedObjects.Clear();
            _mode = Mode.None;
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