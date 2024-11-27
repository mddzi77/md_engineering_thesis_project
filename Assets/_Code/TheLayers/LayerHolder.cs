using System;
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
        [SerializeField] private LayerRenderer layerRenderer;
        public List<Cell> Cells => _cells;
        
        private LayerConfig _layerConfig;
        private readonly List<Cell> _cells = new();
        private LayerGrid _layerGrid;

        public void Init(LayerConfig layerConfig, LayerRenderer rendererComponent, LayerGrid layerGrid)
        {
            _layerConfig = layerConfig;
            _layerGrid = layerGrid;
            layerRenderer = rendererComponent;
            layerRenderer.SetMaterial(layerConfig.Material);
        }

        public void NewPoint(Vector2Int point)
        {
            _layerGrid.NewPoint(point);
            layerRenderer.Redraw();
        }

        public void NewArea(Vector2Int firstPoint, Vector2Int secondPoint)
        {
            // 
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