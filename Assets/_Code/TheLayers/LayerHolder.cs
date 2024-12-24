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
        public List<Cell> Pixels => _pixels;
        
        private LayerConfig _layerConfig;
        private readonly List<Cell> _pixels = new();
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
            }

            await UniTask.Yield();
        }
        
        private void AddCell(GameObject cell, Vector2Int position)
        {
            // _pixels.Add(cell);
        }
        
        public void RemovePixel(Cell cell)
        {
            _pixels.Remove(cell);
        }
    }
}