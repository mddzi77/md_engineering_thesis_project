using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MainCamera
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private InputActionReference middleMouse;
        [SerializeField] private InputActionReference mousePosition;
        [SerializeField] private float dragSpeed = 1;
        
        [Space] [SerializeField] private InputActionReference scroll;
        [SerializeField] private float scrollSpeed = .1f;
        [SerializeField] private float scrollMin = .5f;

        private Camera _mainCamera;
        private bool _isDragging;
        private Vector3 _dragOrigin;

        private void Awake()
        {
            _mainCamera = Camera.main;
            scroll.action.performed += OnScroll;
            middleMouse.action.performed += OnMiddleMouseDown;
            middleMouse.action.canceled += OnMiddleMouseUp;
        }

        private void Update()
        {
            if (!_isDragging) return;
            Vector3 pos = _mainCamera.ScreenToWorldPoint(mousePosition.action.ReadValue<Vector2>());
            Vector3 move = _dragOrigin - pos;
            _mainCamera.transform.position += move;
        }
        
        private void OnMiddleMouseDown(InputAction.CallbackContext ctx)
        {
            _isDragging = true;
            _dragOrigin = _mainCamera.ScreenToWorldPoint(mousePosition.action.ReadValue<Vector2>());
        }
        
        private void OnMiddleMouseUp(InputAction.CallbackContext ctx)
        {
            _isDragging = false;
        }

        private void OnScroll(InputAction.CallbackContext ctx)
        {
            var scrollValue = ctx.ReadValue<Vector2>().y;
            float scrollMultiplier = 1f;
            if (scrollValue > 0) scrollMultiplier = 1 / scrollSpeed;
            else if (scrollValue < 0) scrollMultiplier = scrollSpeed;
            if (_mainCamera.orthographicSize * scrollMultiplier < scrollMin) return;
            _mainCamera.orthographicSize *= scrollMultiplier;
        }
    }
}
