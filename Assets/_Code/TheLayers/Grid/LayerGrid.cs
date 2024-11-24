using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace TheLayers.Grid
{
    [Serializable]
    public class LayerGrid
    {
        [SerializeField] private Corners corners;

        // private NativeArray<bool> _grid;
        private GridQuarter _topRight = new (new Vector2Int(0, 0));
        private GridQuarter _topLeft = new (new Vector2Int(LayerConstants.WIDTH, 0));
        private GridQuarter _bottomRight = new (new Vector2Int(0, LayerConstants.HEIGHT));
        private GridQuarter _bottomLeft = new (new Vector2Int(LayerConstants.WIDTH, LayerConstants.HEIGHT));

        public void NewPoint(Vector2Int point)
        {
            corners.CheckPoint(point.x, point.y);
            var grid = SelectQuarter(point);
            var index = grid.TranslateTo1D(point.x, point.y);
            grid.Grid.Set(index, true);
        }
        
        public void NewArea(Vector2Int firstPoint, Vector2Int secondPoint)
        {
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
                    var index = grid.TranslateTo1D(x, y);
                    grid.Grid.Set(index, true);
                }
            }
        }
        
        private GridQuarter SelectQuarter(Vector2Int point)
        {
            return point.x switch
            {
                >= 0 when point.y >= 0 => _topRight,
                < 0 when point.y >= 0 => _topLeft,
                >= 0 when point.y < 0 => _bottomRight,
                _ => _bottomLeft
            };
        }

        public struct GridQuarter
        {
            public NativeBitArray Grid;
            public Vector2Int Offset;

            public GridQuarter(Vector2Int offset)
            {
                Grid = new NativeBitArray(LayerConstants.WIDTH * LayerConstants.HEIGHT, Allocator.Persistent);
                Offset = offset;
            }
            public int TranslateTo1D(int x, int y)
            {
                x += Offset.x;
                y += Offset.y;
                return x * LayerConstants.WIDTH + y;
            }
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