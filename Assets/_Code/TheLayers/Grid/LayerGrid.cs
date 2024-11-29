using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace TheLayers.Grid
{
    public class LayerGrid : MonoBehaviour
    {
        [SerializeField] private Corners corners;
        
        public GridQuarter[] GridQuarters => _gridQuarters;

        // private NativeArray<bool> _grid;
        private GridQuarter[] _gridQuarters = 
        {
            new (new Vector2Int(0, 0)), // top right
            new (new Vector2Int(LayerConstants.WIDTH, 0)), // top left
            new (new Vector2Int(LayerConstants.WIDTH, LayerConstants.HEIGHT)), // bottom left
            new (new Vector2Int(0, LayerConstants.HEIGHT)) // bottom right
        };
        
        private bool _gridInitialized = false;

        public bool TryNewPoint(Vector2Int point)
        {
            if (!_gridInitialized)
            {
                corners = new Corners(point.x, point.y);
                _gridInitialized = true;
            }
            else corners.CheckPoint(point.x, point.y);
            var grid = SelectQuarter(point);
            var index = _gridQuarters[grid].TranslateTo1D(point.x, point.y);
            
            if (_gridQuarters[grid].Grid.IsSet(index)) return false;
            _gridQuarters[grid].Grid.Set(index, true);
            return true;
        }
        
        public void NewArea(Vector2Int firstPoint, Vector2Int secondPoint)
        {
            if (!_gridInitialized)
            {
                corners = new Corners(firstPoint.x, firstPoint.y);
                _gridInitialized = true;
            }
            corners.CheckPoint(firstPoint.x, firstPoint.y);
            corners.CheckPoint(secondPoint.x, secondPoint.y);
            
            var startX = firstPoint.x < secondPoint.x ? firstPoint.x : secondPoint.x;
            var startY = firstPoint.y < secondPoint.y ? firstPoint.y : secondPoint.y;
            var endX = firstPoint.x > secondPoint.x ? firstPoint.x : secondPoint.x;
            var endY = firstPoint.y > secondPoint.y ? firstPoint.y : secondPoint.y;
            
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    var grid = SelectQuarter(new Vector2Int(x, y));
                    var index = _gridQuarters[grid].TranslateTo1D(x, y);
                    _gridQuarters[grid].Grid.Set(index, true);
                }
            }
        }
        
        public void OnDestroy()
        {
            for (int i = 0; i < _gridQuarters.Length; i++)
            {
                _gridQuarters[i].Grid.Dispose();
            }
        }
        
        private int SelectQuarter(Vector2Int point)
        {
            return point.x switch
            {
                >= 0 when point.y >= 0 => 0,
                < 0 when point.y >= 0 => 1,
                >= 0 when point.y < 0 => 3,
                _ => 2
            };
        }

        // public void SetPoint(Vector2Int point)
        // {
        //     if (!_gridInitialized)
        //     {
        //         corners = new Corners(point.x, point.y);
        //         _width = 1;
        //         _height = 1;
        //         _grid = new NativeArray<bool>(_width * _height, Allocator.Persistent);
        //         _gridInitialized = true;
        //         return;
        //     }
        //
        //     if (corners.TryWiderBounds(point.x, point.y, out var diff)) // diff: x = top, y = right, z = bottom, w = left
        //     {
        //         int newWidth = corners.right - corners.left + 1;
        //         int newHeight = corners.top - corners.bottom + 1;
        //
        //         NativeArray<bool> newBoolArray = new NativeArray<bool>(newWidth * newHeight, Allocator.Persistent);
        //
        //         var copyJob = new CopyGridJob
        //         {
        //             OldArray = _grid,
        //             NewArray = newBoolArray,
        //             OldWidth = _width,
        //             OldHeight = _height,
        //             NewWidth = newWidth,
        //             NewHeight = newHeight,
        //             OffsetX = corners.left < 0 ? -corners.left : 0,
        //             OffsetY = corners.bottom < 0 ? -corners.bottom : 0
        //         };
        //
        //         JobHandle jobHandle = copyJob.Schedule(newWidth * newHeight, 64);
        //         jobHandle.Complete();
        //
        //         _grid.Dispose();
        //         _grid = newBoolArray;
        //         _width = newWidth;
        //         _height = newHeight;
        //     }
        // }
        //
        // [BurstCompile]
        // private struct CopyGridJob : IJobParallelFor
        // {
        //     [ReadOnly] public NativeArray<bool> OldArray;
        //     [WriteOnly] public NativeArray<bool> NewArray;
        //     public int OldWidth;
        //     public int OldHeight;
        //     public int NewWidth;
        //     public int NewHeight;
        //     public int OffsetX;
        //     public int OffsetY;
        //
        //     public void Execute(int index)
        //     {
        //         int x = index % NewWidth;
        //         int y = index / NewWidth;
        //
        //         int oldX = x - OffsetX;
        //         int oldY = y - OffsetY;
        //
        //         if (oldX >= 0 && oldX < OldWidth && oldY >= 0 && oldY < OldHeight)
        //         {
        //             int oldIndex = oldY * OldWidth + oldX;
        //             NewArray[index] = OldArray[oldIndex];
        //         }
        //         else
        //         {
        //             NewArray[index] = false;
        //         }
        //     }
        // }
        //
        // public void Dispose()
        // {
        //     if (_grid.IsCreated)
        //     {
        //         _grid.Dispose();
        //     }
        // }
    }
}