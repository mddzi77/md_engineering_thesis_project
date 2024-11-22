using System;
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

        private NativeArray<bool> _grid;
        private int _width;
        private int _height;

        private bool _gridInitialized;

        public void SetPoint(Vector2Int point)
        {
            if (!_gridInitialized)
            {
                corners = new Corners(point.x, point.y);
                _width = 1;
                _height = 1;
                _grid = new NativeArray<bool>(_width * _height, Allocator.Persistent);
                _gridInitialized = true;
                return;
            }

            if (corners.TryWiderBounds(point.x, point.y, out var diff)) // diff: x = top, y = right, z = bottom, w = left
            {
                int newWidth = corners.right - corners.left + 1;
                int newHeight = corners.top - corners.bottom + 1;

                NativeArray<bool> newBoolArray = new NativeArray<bool>(newWidth * newHeight, Allocator.Persistent);

                var copyJob = new CopyGridJob
                {
                    OldArray = _grid,
                    NewArray = newBoolArray,
                    OldWidth = _width,
                    OldHeight = _height,
                    NewWidth = newWidth,
                    NewHeight = newHeight,
                    OffsetX = corners.left < 0 ? -corners.left : 0,
                    OffsetY = corners.bottom < 0 ? -corners.bottom : 0
                };

                JobHandle jobHandle = copyJob.Schedule(newWidth * newHeight, 64);
                jobHandle.Complete();

                _grid.Dispose();
                _grid = newBoolArray;
                _width = newWidth;
                _height = newHeight;
            }
        }
        
        [BurstCompile]
        private struct CopyGridJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<bool> OldArray;
            [WriteOnly] public NativeArray<bool> NewArray;
            public int OldWidth;
            public int OldHeight;
            public int NewWidth;
            public int NewHeight;
            public int OffsetX;
            public int OffsetY;

            public void Execute(int index)
            {
                int x = index % NewWidth;
                int y = index / NewWidth;

                int oldX = x - OffsetX;
                int oldY = y - OffsetY;

                if (oldX >= 0 && oldX < OldWidth && oldY >= 0 && oldY < OldHeight)
                {
                    int oldIndex = oldY * OldWidth + oldX;
                    NewArray[index] = OldArray[oldIndex];
                }
                else
                {
                    NewArray[index] = false;
                }
            }
        }

        public void Dispose()
        {
            if (_grid.IsCreated)
            {
                _grid.Dispose();
            }
        }
    }
}