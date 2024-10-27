using System.Collections.Generic;
using Tools;
using Tools.Drawing;
using UnityEngine;

namespace TheLayers
{
    public class LayerHolder : MonoBehaviour
    {
        public List<Cell> Pixels => _pixels;
        
        private LayerConfig _layerConfig;
        private readonly List<Cell> _pixels = new();

        public void Init(LayerConfig layerConfig)
        {
            _layerConfig = layerConfig;
        }
        
        public void AddPixel(Cell cell)
        {
            _pixels.Add(cell);
        }
        
        public void RemovePixel(Cell cell)
        {
            _pixels.Remove(cell);
        }
    }
}