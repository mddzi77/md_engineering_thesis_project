using System;
using System.Collections;
using System.Collections.Generic;
using TheLayers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Drawer
{
    public class Brush : MonoBehaviour
    {
        [SerializeField] private InputActionReference mousePosition;
        [SerializeField] private InputActionReference leftClickDown;
        [SerializeField] private InputActionReference leftClickUp;

        private LayersManager _layerManager;
        private Camera _camera;
        private Vector3 _oldGridPos;
        private bool _isDrawing;

        private void Awake()
        {
            _camera = Camera.main;
            leftClickDown.action.performed += OnClickDown;
            leftClickUp.action.performed += _ => _isDrawing = false;
        }

        private void Start()
        {
            _layerManager = LayersManager.Instance;
        }

        private void Update()
        {
            if (!_isDrawing) return;
            OnPressed();
        }

        private void OnPressed()
        {
            var mouseScreenPos = mousePosition.action.ReadValue<Vector2>();
            var mouseWorldPos = _camera.ScreenToWorldPoint(mouseScreenPos);
            var gridPos = CalculateGridPosition(mouseWorldPos);
            Draw(gridPos);
        }

        private void OnClickDown(InputAction.CallbackContext ctx)
        {
            if (PointerOnUI.Instance) return;
            _isDrawing = true;
        }

        private Vector3 CalculateGridPosition(Vector3 mouseWorldPos)
        {
            var gridPos = new Vector3(
                Mathf.Floor(mouseWorldPos.x),
                Mathf.Floor(mouseWorldPos.y),
                _layerManager.CurrentLayer.Order
            );
            return gridPos;
        }

        private void Draw(Vector3 gridPos)
        {
            if (_oldGridPos == gridPos) return;
            var rec = new GameObject(_layerManager.CurrentLayer.LayerName);
            rec.transform.position = gridPos;
            rec.AddComponent<BoxCollider2D>();
            var spriteRenderer = rec.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = _layerManager.CurrentLayer.Sprite;
            _oldGridPos = gridPos;
        }
    }
}
