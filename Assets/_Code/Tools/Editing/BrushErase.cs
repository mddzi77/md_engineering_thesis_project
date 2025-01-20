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
        [SerializeField] private LayerMask layerMask;

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
            Erase(_gridPos.x, _gridPos.y);
            if (!_isDrawing) _isDrawing = true;
            _oldGridPos = _gridPos;
        }

        private void Erase(float posX, float posY)
        {
            var position = new Vector3(posX + 0.5f, posY + 0.5f, -1);
            RaycastHit hit;

            while (Physics.Raycast(position, Vector3.forward, out hit, 15f, layerMask))
            {
                var cell = hit.transform.gameObject;
                LayersManager.Instance.ReturnCell(cell);
            }
        }

        private bool CanErase(Vector3 position, out RaycastHit hit)
        {
            // Raycast version
            position.z -= 0.5f;
            position.x += 0.5f;
            position.y += 0.5f;
            // Debug.DrawRay(position, Vector3.forward, Color.red, 2f);
            return Physics.Raycast(position, Vector3.forward, out hit, 1f, layerMask) == false;

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