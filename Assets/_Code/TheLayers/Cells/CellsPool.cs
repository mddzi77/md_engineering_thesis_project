using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TheLayers.Cells
{
    public class CellsPool : MonoBehaviour
    {
        [SerializeField] private LayersManager layersManager;
        [SerializeField] private Transform poolParent;
        [SerializeField] private int poolSize;
        [SerializeField] private int instantiatePerFrame;
        
        private static List<PoolData> _pools = new();
        private static Transform staticTransform;
        private int _poolIndex;

        private void Start()
        {
            staticTransform = poolParent;
            foreach (var config in layersManager.LayerConfigs)
            {
                _pools.Add(new PoolData
                {
                    layerConfig = config,
                    pool = new Queue<GameObject>(poolSize)
                });
            }
        }

        private void FixedUpdate()
        {
            var curConfig = _pools[_poolIndex].layerConfig;
            InstantiateCells(curConfig);
            IncrementPoolIndex();
        }
        
        public static GameObject GetCell(LayerConfig layerConfig)
        {
            var pool = _pools.Find(p => p.layerConfig == layerConfig).pool;
            if (pool.Count == 0)
            {
                return InstantiateCell(layerConfig);
            }
            return pool.Dequeue();
        }

        public static async UniTask<GameObject> GetCellAsync(LayerConfig layerConfig)
        {
            var pool = _pools.Find(p => p.layerConfig == layerConfig).pool;
            if (pool.Count == 0)
            {
                return InstantiateCell(layerConfig);
            }
            return pool.Dequeue();
        }
        
        private void InstantiateCells(LayerConfig layerConfig)
        {
            for (var i = 0; i < instantiatePerFrame; i++)
            {
                if (_pools[_poolIndex].pool.Count >= poolSize) return;
                var cell = InstantiateCell(layerConfig);
                _pools[_poolIndex].pool.Enqueue(cell);
            }
        }
        
        private void IncrementPoolIndex()
        {
            _poolIndex++;
            if (_poolIndex >= _pools.Count)
            {
                _poolIndex = 0;
            }
        }
        
        private static GameObject InstantiateCell(LayerConfig layerConfig)
        {
            var cell = new GameObject();
            cell.transform.parent = staticTransform;
            cell.SetActive(false);
                
            var spriteRenderer = cell.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = layerConfig.Sprite;
                
            return cell;
        }
        
        [Serializable]
        public struct PoolData
        {
            public LayerConfig layerConfig;
            public Queue<GameObject> pool;
        }
    }
}