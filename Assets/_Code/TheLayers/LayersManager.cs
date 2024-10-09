using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MdUtils;
using UnityEngine;

namespace TheLayers
{
    public class LayersManager: MonoSingleton<LayersManager>
    {
        [SerializeField] private LayerConfig[] _layerConfigs;
        
        public LayerConfig CurrentLayer => _currentLayer;
        public Transform CurrentLayerHolder => _currentLayerHolder;
        
        private LayerConfig _currentLayer;
        private Transform _currentLayerHolder;
        
        private Dictionary<LayerConfig, Transform> _layerHolders = new();

        private new void Awake()
        {
            base.Awake();
            
            foreach (var layer in _layerConfigs)
            {
                CreateLayer(layer);
            }
        }

        public void SetCurrentLayer(LayerConfig layerConfig)
        {
            _currentLayer = layerConfig;
            _currentLayerHolder = _layerHolders[layerConfig];
        }

        public void SetCurrentLayer(string layerName)
        {
            foreach (var layerConfig in _layerConfigs)
            {
                if (layerConfig.LayerName == layerName)
                {
                    _currentLayer = layerConfig;
                    _currentLayerHolder = _layerHolders[layerConfig];
                    return;
                }
            }
        }
        
        private void CreateLayer(LayerConfig layerConfig)
        {
            var layer = new GameObject(layerConfig.LayerName);
            layer.transform.SetParent(transform);
            _layerHolders.Add(layerConfig, layer.transform);
        }
    }
}