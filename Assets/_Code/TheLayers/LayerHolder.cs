using System.Collections.Generic;
using TheLayers.Grid;
using Tools;
using Tools.Drawing;
using UnityEngine;

namespace TheLayers
{
    public class LayerHolder : MonoBehaviour
    {
        public LayerGrid Grid { get; private set; }
        public List<Cell> Cells => _cells;
        
        private LayerConfig _layerConfig;
        private readonly List<Cell> _cells = new();

        public void Init(LayerConfig layerConfig, LayerGrid layerGrid)
        {
            _layerConfig = layerConfig;
            Grid = layerGrid;
        }
        
        public void SetPoint(Vector2Int point)
        {
            Grid.SetPoint(point);
        }
        
        public void AddPixel(Cell cell)
        {
            _cells.Add(cell);
        }
        
        public void RemovePixel(Cell cell)
        {
            _cells.Remove(cell);
        }
    }
}