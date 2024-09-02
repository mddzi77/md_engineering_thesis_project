using System;
using System.Collections;
using System.Collections.Generic;
using MouseGridPosition;
using TheLayers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Drawer
{
    public class Brush : MonoBehaviour
    {
        [SerializeField] private InputActionReference leftMouse;

        private LayersManager _layerManager;
        private Vector2 _oldGridPos;

        private void Start()
        {
            _layerManager = LayersManager.Instance;
        }

        private void Update()
        {
            if (leftMouse.action.IsPressed())
                OnPressed();
        }

        private void OnPressed()
        {
            var gridPos = MouseGrid.GridPos;
            if (_oldGridPos == gridPos || PointerOnUI.Instance) return;
            Draw(gridPos);
            _oldGridPos = gridPos;
        }

        private void Draw(Vector2 gridPos)
        {
            var rec = new GameObject(_layerManager.CurrentLayer.LayerName);
            rec.transform.position = new Vector3(gridPos.x, gridPos.y, _layerManager.CurrentLayer.Order);
            rec.AddComponent<BoxCollider2D>();
            var spriteRenderer = rec.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = _layerManager.CurrentLayer.Sprite;
        }
    }
}
