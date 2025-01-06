using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MdUtils;
using TheLayers.Grid;
using UnityEngine;

namespace TheLayers
{
    public class LayersManager: MonoSingleton<LayersManager>
    {
        [SerializeField] private LayerConfig[] layerConfigs;
        [SerializeField] GameObject cellBase;
        
        public LayerConfig[] LayerConfigs => layerConfigs;
        public Dictionary<LayerConfig, LayerHolder> LayerHolders => _layerHolders;
        public LayerConfig CurrentLayer => _currentLayer;
        public LayerHolder CurrentLayerHolder => _currentLayerHolder;
        
        public static event Action Initialized;
        public static event Action<LayerConfig> LayerChanged;
        
        public static bool IsInitialized { get; private set; }
        
        private LayerConfig _currentLayer;
        private LayerHolder _currentLayerHolder;
        
        private Dictionary<LayerConfig, LayerHolder> _layerHolders = new();

        private new void Awake()
        {
            base.Awake();
            
            foreach (var layer in layerConfigs)
            {
                CreateLayer(layer);
            }
            SetCurrentLayer(layerConfigs[0]); // set default layer
            IsInitialized = true;
            Initialized?.Invoke();
        }

        public void SetCurrentLayer(LayerConfig layerConfig)
        {
            _currentLayer = layerConfig;
            _currentLayerHolder = _layerHolders[layerConfig];
            LayerChanged?.Invoke(layerConfig);
        }

        public void SetCurrentLayer(string layerName)
        {
            foreach (var layerConfig in layerConfigs)
            {
                if (layerConfig.LayerName != layerName) continue;
                _currentLayer = layerConfig;
                _currentLayerHolder = _layerHolders[layerConfig];
                LayerChanged?.Invoke(layerConfig);
                return;
            }
        }
        
        public void DisableLayer(LayerConfig layerConfig)
        {
            _layerHolders[layerConfig].gameObject.SetActive(false);
        }
        
        public void EnableLayer(LayerConfig layerConfig)
        {
            _layerHolders[layerConfig].gameObject.SetActive(true);
        }

        public async UniTask<List<GameObject>> GetCellsAsync(List<Vector2> positions)
        {
            List<GameObject> cells = new();
            var tasks = new List<UniTask<List<GameObject>>>();
            
            foreach (var config in layerConfigs)
            {
                var holder = _layerHolders[config];
                tasks.Add(holder.GetCells(positions));
            }
            var results = await UniTask.WhenAll(tasks);
            
            foreach (var result in results)
            {
                cells.AddRange(result);
            }

            return cells;
        }
        
        private void CreateLayer(LayerConfig layerConfig)
        {
            var layer = new GameObject(layerConfig.LayerName);
            var holder = layer.AddComponent<LayerHolder>();
            var layerGrid = layer.AddComponent<LayerGrid>();
            holder.Init(layerConfig, layerGrid, cellBase);
            layer.transform.SetParent(transform);
            _layerHolders.Add(layerConfig, holder);
        }
    }
}