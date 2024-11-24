using System.Collections.Generic;
using TheLayers.Grid;
using Tools;
using Tools.Drawing;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheLayers
{
    public class LayerHolder : MonoBehaviour
    {
        [FormerlySerializedAs("layerDraw")] [SerializeField] private LayerRenderer layerRenderer;
        [SerializeField] private LayerGrid grid;
        public List<Cell> Cells => _cells;
        
        private LayerConfig _layerConfig;
        private readonly List<Cell> _cells = new();

        public void Init(LayerConfig layerConfig)
        {
            _layerConfig = layerConfig;
            grid = new LayerGrid();
        }

        public void NewPoint(Vector2Int point)
        {
            grid.NewPoint(point);
        }

        public void NewArea(Vector2Int firstPoint, Vector2Int secondPoint)
        {
            grid.NewArea(firstPoint, secondPoint);
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