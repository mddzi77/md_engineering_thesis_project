using MdUtils;
using UnityEngine;

namespace TheLayers
{
    public class LayersManager: Singleton
    {
        [SerializeField] private LayerConfig[] _layerConfigs;
        
        public LayerConfig CurrentLayer => _currentLayer;
        
        private LayerConfig _currentLayer;
        
        public void SetCurrentLayer(LayerConfig layerConfig) => _currentLayer = layerConfig;

        public void SetCurrentLayer(string layerName)
        {
            foreach (var layerConfig in _layerConfigs)
            {
                if (layerConfig.LayerName == layerName)
                {
                    _currentLayer = layerConfig;
                    return;
                }
            }
        }
    }
}