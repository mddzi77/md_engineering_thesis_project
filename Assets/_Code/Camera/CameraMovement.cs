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
        [SerializeField] private InputActionReference mouseDelta;
        [SerializeField] private float dragSpeed = 1;
        
        [Space] [SerializeField] private InputActionReference scroll;
        [SerializeField] private float scrollSpeed = .1f;
        [SerializeField] private float scrollMin = .5f;

        private Camera _mainCamera;
        private bool _isDragging;

        private void Awake()
        {
            _mainCamera = Camera.main;
            scroll.action.performed += OnScroll;
        }

        private void Update()
        {
            if (!middleMouse.action.IsPressed()) return;
            Vector3 delta = Time.deltaTime * dragSpeed * _mainCamera.orthographicSize *
                            mouseDelta.action.ReadValue<Vector2>();
            _mainCamera.transform.position -= delta;
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
