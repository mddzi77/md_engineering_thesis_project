using System;
using System.Collections;
using System.Collections.Generic;
using MouseGridPosition;
using TheLayers;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Drawer
{
    public class Brush : DrawerAbstract
    {
        [SerializeField] private InputActionReference leftMouse;

        private LayersManager _layerManager;
        private Vector2 _gridPos;
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
            _gridPos = MouseGrid.GridPos;
            if (_oldGridPos == _gridPos || PointerOnUI.Instance) return;
            Draw();
            _oldGridPos = _gridPos;
        }

        private void Draw()
        {
            var position = new Vector3(_gridPos.x, _gridPos.y, _layerManager.CurrentLayer.Order);
            
            if (CanDraw(position) == false) return;
            
            var pixel = Instantiate(pixelBase, _layerManager.CurrentLayerHolder.transform).GetComponent<Pixel>();
            _layerManager.CurrentLayerHolder.AddPixel(pixel);
            pixel.transform.position = position;
            pixel.SetSprite(_layerManager.CurrentLayer.Sprite);
        }

        private bool CanDraw(Vector3 position)
        {
            // Raycast version
            position.z -= 0.5f;
            // Debug.DrawRay(position, Vector3.forward, Color.red, 2f);
            return Physics.Raycast(position, Vector3.forward, out var hit, 1f) == false;
            
            // List version
            // var pixels = _layerManager.CurrentLayerHolder.Pixels;
            //
            // if (pixels.Count == 0) return true;
            //
            // foreach (var pixel in pixels)
            // {
            //     if (Vector2.Distance(pixel.transform.position, position) < 0.5f)
            //         return false;
            // }
            // return true;
        }
    }
}
