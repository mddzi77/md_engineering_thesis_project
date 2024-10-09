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
            var pixel = Instantiate(pixelBase, _layerManager.CurrentLayerHolder).GetComponent<Pixel>();
            pixel.transform.position = new Vector3(_gridPos.x, _gridPos.y, _layerManager.CurrentLayer.Order);
            pixel.SetSprite(_layerManager.CurrentLayer.Sprite);
        }
        
        // private void 
    }
}
