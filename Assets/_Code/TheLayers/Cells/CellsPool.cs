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
        [SerializeField] private Material material;
        [SerializeField] private LayerMask tileMask;

        private static CellsPool instance;
        
        private static List<PoolData> _pools = new();
        private int _poolIndex;

        private void Start()
        {
            instance = this;
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
                await UniTask.CompletedTask;
                return InstantiateCell(layerConfig);
            }
            return pool.Dequeue();
        }

        public static void ReturnCell(LayerConfig layerConfig, GameObject cell)
        {
            var pool = _pools.Find(p => p.layerConfig == layerConfig).pool;
            cell.transform.SetParent(instance.poolParent, false);
            cell.SetActive(false);
            pool.Enqueue(cell);
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
            var cell = new GameObject
            {
                name = "Cell",
                transform =
                {
                    parent = instance.poolParent
                }
            };
            cell.SetActive(false);
            cell.tag = layerConfig.LayerName;
            cell.layer = LayerMask.NameToLayer("Tile");
                
            var spriteRenderer = cell.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = layerConfig.Sprite;
            spriteRenderer.material = instance.material;
            spriteRenderer.color = layerConfig.Color;
            
            var collider = cell.AddComponent<BoxCollider>();
            collider.size = new Vector3(1, 1, 0.1f);
            collider.center = new Vector3(0.5f, 0.5f, 0);
                
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