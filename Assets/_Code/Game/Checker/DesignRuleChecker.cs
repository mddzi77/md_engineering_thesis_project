using System;
using System.Collections.Generic;
using TheLayers;
using UnityEngine;

namespace Game.Checker
{
    [Serializable]
    public class DesignRuleChecker
    {
        private Dictionary<LayerConfig, LayerHolder> _layerHolders;
        
        public void StartCheck()
        {
            _layerHolders = LayersManager.Instance.LayerHolders;

            foreach (var pair in _layerHolders)
            {
                CheckRules(pair.Key, pair.Value);
            }
        }
        
        private void CheckRules(LayerConfig layerConfig, LayerHolder layerHolder)
        {
            var rules = layerConfig.Rules;
            foreach (var cell in layerHolder.Cells)
            {
                Raycast(cell.transform.position, rules.MinimumSpacing, layerConfig.LayerName);
            }
        }

        private void Raycast(Vector3 origin, int spacing, string layerName)
        {
            var start = origin;
            start.x += 0.5f;
            start.y += 0.5f;
            
            CheckSpacing(origin, start, spacing, layerName);
            CheckWidth(origin, start, spacing, layerName);
        }

        private void CheckSpacing(Vector3 origin, Vector3 start, int spacing, string layerName)
        {
            Vector3[] directions = {
                Vector3.up,
                Vector3.down,
                Vector3.left,
                Vector3.right
            };
            foreach (var direction in directions)
            {
                if (Physics.Raycast(start, direction, out var hit , spacing))
                {
                    if (!(Vector3.Distance(start, hit.point) > 0.6f)) continue;
                    var grid = origin;
                    if (grid.x > 0) grid.x += 1;
                    if (grid.y > 0) grid.y += 1;
                    Debug.Log($"Spacing is not correct for: {grid.x}, {grid.y} in layer {layerName}");
                    Debug.DrawRay(start, Vector3.back * 10, Color.red, 5);
                }
            }
        }
        
        private void CheckWidth(Vector3 origin, Vector3 start, int width, string layerName)
        {
            
            var grid = origin;
            if (grid.x > 0) grid.x += 1;
            if (grid.y > 0) grid.y += 1;
            
            //horizontal
            int horizontalCounter = 0;
            for (int i = 0; i < width - 1; i++)
            {
                var position = start;
                position.x += i;
                if (Physics.Raycast(position, Vector3.right, out var hit, 1))
                    horizontalCounter++;
                else
                    break;
            }
            for (int i = 0; i < width - 1; i++)
            {
                var position = start;
                position.x -= i;
                if (Physics.Raycast(position, Vector3.left, out var hit, 1))
                    horizontalCounter++;
                else
                    break;
            }

            if (horizontalCounter < (width - 1))
            {
                Debug.Log($"Width is not correct for: {grid.x}, {grid.y} horizontally in layer {layerName}");
                Debug.DrawRay(start, Vector3.forward * 10, Color.green, 5);
            }
            
            //vertical
            int verticalCounter = 0;
            for (int i = 0; i < width - 1; i++)
            {
                var position = start;
                position.y += i;
                if (Physics.Raycast(position, Vector3.up, out var hit, 1))
                    verticalCounter++;
                else
                    break;
            }
            for (int i = 0; i < width - 1; i++)
            {
                var position = start;
                position.y -= i;
                if (Physics.Raycast(position, Vector3.down, out var hit, 1))
                    verticalCounter++;
                else
                    break;
            }

            if (verticalCounter < (width - 1))
            {
                Debug.Log($"Width is not correct for: {grid.x}, {grid.y} vertically in layer {layerName}");
                Debug.DrawRay(start, Vector3.back * 10, Color.green, 5);
            }
        }
    }
}