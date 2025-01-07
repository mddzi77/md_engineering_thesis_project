using System;
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
            var dY = Mathf.Abs(_gridPos.y - _oldGridPos.y);
            var dX = Mathf.Abs(_gridPos.x - _oldGridPos.x);
            if (_isDrawing && (dX > 1 || dY > 1))
            {
                DrawInterpolate(dX, dY);
            }
            else
            {
                Draw(_gridPos.x, _gridPos.y);
            }
            
            // if (_layerManager.CurrentLayerHolder.CanDraw(position)) return;
            //
            // var pixel = Instantiate(cellBase, _layerManager.CurrentLayerHolder.transform).GetComponent<Cell>();
            // _layerManager.CurrentLayerHolder.AddPixel(pixel);
            // pixel.transform.position = position;
            // pixel.SetSprite(_layerManager.CurrentLayer.Sprite);
        }

        private void Draw(float posX, float posY)
        {
            var position = new Vector3(posX, posY, _layerManager.CurrentLayer.Order);
            if (!CanDraw(position)) return;
            _layerManager.CurrentLayerHolder.NewCell(position);
        }

        private void DrawInterpolate(float dX, float dY)
        {
            var axis = dX > dY ? 1 : 0; // 1 - horizontal axis, 0 - vertical axis
            var dirX = _gridPos.x > _oldGridPos.x ? 1 : -1;
            var dirY = _gridPos.y > _oldGridPos.y ? 1 : -1;
            
            if ((int)dX == 0 || (int)dY == 0)
            {
                StraightLine((int)dX, (int)dY);
                return;
            }
            
            var step = dX > dY ? dX / dY : dY / dX;
            float rest = 0f;
            float sum = 0f;

            var bigger = dX > dY ? dX : dY;
            var curX = _oldGridPos.x;
            var curY = _oldGridPos.y;
            while (sum < bigger)
            {
                rest += step;
                var amount = (int)Mathf.Floor(rest);
                rest -= amount;
                
                if (axis == 1) // horizontal
                {
                    DrawForHorizontal(amount, dirX, curX, curY);
                    curY += dirY;
                }
                else // vertical
                {
                    DrawForVertical(amount, dirY, curX, curY);
                    curX += dirX;
                }
                
                sum += step;
            }
            Draw(_gridPos.x, _gridPos.y); // make sure to draw the last pixel  
        }
        
        private void DrawForHorizontal(int amount, int dirX, float curX, float curY)
        {
            for (int i = 0; i < amount; i++)
            {
                Draw(curX, curY);
                curX += dirX;
            }
        }
        
        private void DrawForVertical(int amount, int dirY, float curX, float curY)
        {
            for (int i = 0; i < amount; i++)
            {
                Draw(curX, curY);
                curY += dirY;
            }
        }
        
        private void StraightLine(int dX, int dY)
        {
            if (dX == 0)
            {
                for (int i = 0; i < dY; i++)
                {
                    Draw(_oldGridPos.x, _oldGridPos.y + i);
                }
            }
            else
            {
                for (int i = 0; i < dX; i++)
                {
                    Draw(_oldGridPos.x + i + 1, _oldGridPos.y);
                }
            }
            Draw(_gridPos.x, _gridPos.y); // make sure to draw the last pixel
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
