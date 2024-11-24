using System;
using System.Numerics;
using UnityEngine.Serialization;

namespace TheLayers.Grid
{
    [Serializable]
    public class Corners
    {
        public int top;
        public int right;
        public int bottom;
        public int left;
        
        public Vector2 TopRight => new(right, top);
        public Vector2 TopLeft => new(left, top);
        public Vector2 BottomRight => new(right, bottom);
        public Vector2 BottomLeft => new(left, bottom);

        public Corners(int x, int y)
        {
            top = y;
            right = x;
            bottom = y;
            left = x;
        }
        
        public Vector4 ToVector4() => new(top, left, right, bottom);
        
        public void CheckPoint(int xPoint, int yPoint)
        {
            if (xPoint < left)
            {
                left = xPoint;
            }
            else if (xPoint > right)
            {
                right = xPoint;
            }
            
            if (yPoint > top)
            {
                top = yPoint;
            }
            else if (yPoint < bottom)
            {
                bottom = yPoint;
            }
        }
    }

        // public bool TryWiderBounds(int xPoint, int yPoint, out Vector4 diff)
        // {
        //     diff = Vector4.Zero;
        //     var changed = false;
        //     
        //     if (xPoint < left)
        //     {
        //         diff.W = xPoint - left;
        //         left = xPoint;
        //         changed = true;
        //     }
        //     else if (xPoint > right)
        //     {
        //         diff.Y = xPoint - right;
        //         right = xPoint;
        //         changed = true;
        //     }
        //     
        //     if (yPoint > top)
        //     {
        //         diff.X = yPoint - top;
        //         top = yPoint;
        //         changed = true;
        //     }
        //     else if (yPoint < bottom)
        //     {
        //         diff.Z = yPoint - bottom;
        //         bottom = yPoint;
        //         changed = true;
        //     }
        //
        //     return changed;
        // }
}