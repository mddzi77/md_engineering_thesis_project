using System.Collections.Generic;
using TheLayers.Table;
using Tools;
using Tools.Drawing;
using UnityEngine;

namespace TheLayers
{
    public class LayerHolder : MonoBehaviour
    {
        public LayerTable Table { get; private set; }
        public List<Cell> Cells => _cells;
        
        private LayerConfig _layerConfig;
        private readonly List<Cell> _cells = new();

        public void Init(LayerConfig layerConfig, LayerTable layerTable)
        {
            _layerConfig = layerConfig;
            Table = layerTable;
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