using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TheLayers.Cells;
using TheLayers.Grid;
using Tools.Drawing;
using UnityEngine;

namespace TheLayers
{
    public class LayerHolder : MonoBehaviour
    {
        public List<GameObject> Pixels => _pixels;
        
        private LayerConfig _layerConfig;
        private readonly List<GameObject> _pixels = new();
        private LayerGrid _layerGrid;

        public void Init(LayerConfig layerConfig, LayerGrid layerGrid, GameObject cellBase)
        {
            _layerConfig = layerConfig;
            _layerGrid = layerGrid;
        }

        public void NewCell(Vector3 position)
        {
            var intPosition = new Vector2Int((int)position.x, (int)position.y);
            if (_layerGrid.TryNewPoint(intPosition))
            {
                var cell = CellsPool.GetCell(_layerConfig);
                cell.transform.position = position;
                cell.transform.parent = transform;
                cell.SetActive(true);
                _pixels.Add(cell);

                // var pixel = new GameObject();
                // pixel.transform.position = position;
                // var spriteRenderer = pixel.AddComponent<SpriteRenderer>();
                // spriteRenderer.sprite = (_layerConfig.Sprite);
            }
        }
        
        public async UniTask NewCellAsync(Vector3 position)
        {
            var intPosition = new Vector2Int((int)position.x, (int)position.y);
            if (_layerGrid.TryNewPoint(intPosition))
            {
                var cell = await CellsPool.GetCellAsync(_layerConfig);
                cell.transform.position = position;
                cell.transform.parent = transform;
                cell.SetActive(true);
                _pixels.Add(cell);
            }

            await UniTask.Yield();
        }

        public async UniTask<List<GameObject>> GetCells(List<Vector2> positions)
        {
            List<GameObject> cells = new();
            
            if (_pixels.Count < 0) return cells;
            for (var i = 0; i < positions.Count; i++)
            {
                var pos = positions[i];
                var intPos = new Vector2Int((int)pos.x, (int)pos.y);
                // foreach (var pixel in _pixels)
                // {
                //     
                // }
                var cell = _pixels.Find(c =>
                    Mathf.Approximately(c.transform.position.x, intPos.x) &&
                    Mathf.Approximately(c.transform.position.y, intPos.y));
                if (cell != null)
                    cells.Add(cell.gameObject);
            }

            await UniTask.Yield();
            return cells;
        }
    }
}