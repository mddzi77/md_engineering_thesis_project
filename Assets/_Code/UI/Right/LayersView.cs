using System.Collections.Generic;
using TheLayers;
using TriInspector;
using UnityEngine;

namespace UI.Right
{
    public class LayersView : MonoBehaviour
    {
        [AssetsOnly]
        [SerializeField] private GameObject layerViewPrefab;
        
        private LayersManager _layersManager;
        private Dictionary<LayerConfig, LayerHolder> _layerHolders;

        private void Start()
        {
            _layersManager = LayersManager.Instance;
            _layerHolders = _layersManager.LayerHolders;
            DrawLayers();
        }
        
        private void DrawLayers()
        {
            var layerConfigs = _layersManager.LayerConfigs;
            foreach (var layerConfig in layerConfigs)
            {
                if (layerConfig.LayerName.Equals("Contact")) continue;
                var layerView = Instantiate(layerViewPrefab, transform).GetComponent<LayerView>();
                layerView.SetLayer(layerConfig);
                layerView.OnVisibility += OnVisibility;
            }
        }
        
        private void OnVisibility(LayerConfig layerConfig, bool isVisible)
        {
            if (isVisible)
            {
                _layersManager.EnableLayer(layerConfig);
            }
            else
            {
                _layersManager.DisableLayer(layerConfig);
            }
        }
    }
}