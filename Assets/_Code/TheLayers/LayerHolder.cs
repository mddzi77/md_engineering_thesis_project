using System.Collections.Generic;
using TheLayers.Grid;
using Tools;
using Tools.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace TheLayers
{
    public class LayerHolder : MonoBehaviour
    {
        public List<Cell> Pixels => _pixels;
        
        private LayerConfig _layerConfig;
        private readonly List<Cell> _pixels = new();
        private LayerGrid _layerGrid;
        private GameObject _cellBase;

        public void Init(LayerConfig layerConfig, LayerGrid layerGrid, GameObject cellBase)
        {
            _layerConfig = layerConfig;
            _layerGrid = layerGrid;
            _cellBase = cellBase;
        }

        public void NewCell(Vector3 position)
        {
            if (_layerGrid.TryNewPoint(new Vector2Int((int)position.x, (int)position.y)))
            {
                var pixel = Instantiate(transform);
                pixel.transform.position = position;
                var spriteRenderer = pixel.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = (_layerConfig.Sprite);
            }
        }
        
        public void AddPixel(Cell cell)
        {
            // _pixels.Add(cell);
        }
        
        public void RemovePixel(Cell cell)
        {
            _pixels.Remove(cell);
        }
    }
}