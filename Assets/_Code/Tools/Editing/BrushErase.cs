using MouseGridPosition;
using TheLayers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools.Editing
{
    public class BrushErase : ToolAbstract
    {
        [SerializeField] private InputActionReference leftMouse;

        private LayersManager _layerManager;
        private Vector2 _gridPos;
        private Vector2 _oldGridPos;
        private bool _isDrawing;

        private void Start()
        {
            _layerManager = LayersManager.Instance;
        }

        private void Update()
        {
            if (leftMouse.action.IsPressed())
                OnPressed();
            else if (_isDrawing) _isDrawing = false;
        }

        private void OnPressed()
        {
            _gridPos = MouseGrid.GridPos;
            if (_oldGridPos == _gridPos || PointerOnUI.Instance) return;
            Drawing();
            if (!_isDrawing) _isDrawing = true;
            _oldGridPos = _gridPos;
        }

        private void Drawing()
        {
            // var dY = Mathf.Abs(_gridPos.y - _oldGridPos.y);
            // var dX = Mathf.Abs(_gridPos.x - _oldGridPos.x);
            // if (_isDrawing && (dX > 1 || dY > 1))
            // {
            //     DrawInterpolate(dX, dY);
            // }
            // else
            // {
            //     Draw(_gridPos.x, _gridPos.y);
            // }
            Erase(_gridPos.x, _gridPos.y);
            
            // if (_layerManager.CurrentLayerHolder.CanDraw(position)) return;
            //
            // var pixel = Instantiate(cellBase, _layerManager.CurrentLayerHolder.transform).GetComponent<Cell>();
            // _layerManager.CurrentLayerHolder.AddPixel(pixel);
            // pixel.transform.position = position;
            // pixel.SetSprite(_layerManager.CurrentLayer.Sprite);
        }

        private void Erase(float posX, float posY)
        {
            var position = new Vector3(posX, posY, _layerManager.CurrentLayer.Order);
            // if (!CanDraw(position)) return;
            _layerManager.CurrentLayerHolder.NewCell(position);
        }

        private bool CanErase(Vector3 position)
        {
            // Raycast version
            position.z -= 0.5f;
            position.x += 0.5f;
            position.y += 0.5f;
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