using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MouseGridPosition;
using TheLayers;
using UI;
using UI.Bottom;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Tools.Drawing
{
    public class Rectangle : ToolAbstract, IRectangleWithSize
    {
        [SerializeField] private InputActionReference leftMouse;
        [SerializeField] private InputActionReference modifierAction;
        [SerializeField] private SpriteRenderer layerSprite;
        [SerializeField] private BoxCollider detector;
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

        private void Start()
        {
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

        private void Draw()
        {
            var startX = _startPos.x < _endPos.x ? (int) _startPos.x : (int) _endPos.x;
            var endX = _startPos.x > _endPos.x ? (int) _startPos.x : (int) _endPos.x;
            var startY = _startPos.y < _endPos.y ? (int) _startPos.y : (int) _endPos.y;
            var endY = _startPos.y > _endPos.y ? (int) _startPos.y : (int) _endPos.y;

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    var position = new Vector3(x, y, _layerManager.CurrentLayer.Order);
                    DrawPixel(position);
                }
            }
            Debug.Log($"{(endX - startX + 1) * (endY - startY + 1)} cells drawn");
        }

        private IEnumerator DrawingCoroutine()
        {
            yield return null;
        }
        
        private void DrawPixel(Vector3 position)
        {
            if (!CanDraw(position)) return;
            // var asyncOperation = await InstantiateAsync(cellBase, _layerManager.CurrentLayerHolder.transform);
            // var pixel = asyncOperation[0].GetComponent<Cell>();
            var pixel = Instantiate(cellBase, _layerManager.CurrentLayerHolder.transform).GetComponent<Cell>();
            _layerManager.CurrentLayerHolder.AddPixel(pixel);
            pixel.transform.position = position;
            pixel.SetSprite(_layerManager.CurrentLayer.Sprite);
        }
        
        private void OnLeftMouse(InputAction.CallbackContext context)
        {
            if (PointerOnUI.Instance) return;

            if (_mode == Mode.Click)
            {
                SecondClick();
                return;
            }
            
            SetLayerOrder();
            OnToggle?.Invoke(true, this);
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
            // DragUpdate();
            // await UniTask.WaitForSeconds(0.4f);
            Draw();
            ResetTool();
        }
        
        private void SetLayerOrder()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _layerManager.CurrentLayer.Order);
        }
        
        private void ModifyBounds(float deltaX, float deltaY)
        {
            detector.size = new Vector3(1 - 0.1f / deltaX, 1 - 0.1f / deltaY, 0.1f);
        }

        private bool CanDraw(Vector3 position)
        {
            foreach (var detected in detectedObjects)
            {
                if (detected.transform.position == position)
                    return false;
            }
            return true;
        }

        private void ResetTool()
        {
            OnToggle?.Invoke(false, this);
            transform.localScale = Vector3.one;
            layerSprite.sprite = null;
            detectedObjects.Clear();
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