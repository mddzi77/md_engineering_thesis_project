using System.Collections.Generic;
using Drawer;
using UnityEngine;

namespace TheLayers
{
    public class LayerHolder : MonoBehaviour
    {
        public List<Pixel> Pixels => _pixels;
        
        private LayerConfig _layerConfig;
        private readonly List<Pixel> _pixels = new();

        public void Init(LayerConfig layerConfig)
        {
            _layerConfig = layerConfig;
        }
        
        public void AddPixel(Pixel pixel)
        {
            _pixels.Add(pixel);
        }
        
        public void RemovePixel(Pixel pixel)
        {
            _pixels.Remove(pixel);
        }
    }
}