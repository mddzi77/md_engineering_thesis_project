using MouseGridPosition;
using TheLayers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tools.Drawing
{
    public class Brush : ToolAbstract
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
            
            var pixel = Instantiate(cellBase, _layerManager.CurrentLayerHolder.transform).GetComponent<Cell>();
            _layerManager.CurrentLayerHolder.AddPixel(pixel);
            _layerManager.CurrentLayerHolder.SetPoint(new Vector2Int((int) _gridPos.x, (int) _gridPos.y));
            pixel.transform.position = position;
            pixel.SetSprite(_layerManager.CurrentLayer.Sprite);
        }

        private bool CanDraw(Vector3 position)
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
