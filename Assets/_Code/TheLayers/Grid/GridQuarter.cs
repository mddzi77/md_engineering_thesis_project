using Unity.Collections;
using UnityEngine;

namespace TheLayers.Grid
{
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
}